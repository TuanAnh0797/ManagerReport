using FlyCapture2Managed;
using FlyCapture2Managed.Gui;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ManagerReport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // public DateTime? Datetimeupload;
        List<string> listmonth;
        List<string> listyear;
        string Ipserver;
        private string totalCheckSheet;
        private string finishCheckSheet;
        public bool endreadQR;
        public string QRcodefromBarcode;
        public string ToltalCheckSheet
        {
            set
            {
                totalCheckSheet = value;
                OnPropertyChanged();
            }
            get { return totalCheckSheet; }
        }
        public string FinishCheckSheet
        {
            set
            {
                finishCheckSheet = value;
                OnPropertyChanged();
            }
            get
            {
                return finishCheckSheet;
            }
        }
       
        public ObservableCollection<ImageReport> listimage { get; set; }
        public ObservableCollection<CheckSheet> listchecksheet { get; set; }
        public struct WorkerHelper
        {
            public ManagedImage raw;
            public ManagedImage converted;
            public ManagedCameraBase cam;
            public BitmapSource source;
        }

        ManagedBusManager m_busmgr;
        ManagedCameraBase m_cam;
        CameraControlDialog m_ctldlg;
        CameraSelectionDialog m_selDlg;
        ManagedImage m_image;
        ManagedImage m_converted;
        BitmapImage m_bitmap;
        BackgroundWorker m_worker;
        bool m_continue = false;
        AutoResetEvent m_Done;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
            listmonth = new List<string>();
            listyear = new List<string>();
            IPAddress[] listip = Dns.GetHostAddresses(Dns.GetHostName());
            listip.ToList().ForEach(ip =>
            {
                string a = ip.ToString();
                if (ip.ToString().Contains("10.27.4"))
                {
                    Ipserver = "10.27.4.218";

                }
                else if (ip.ToString().Contains("192.168.2"))
                {
                    Ipserver = "192.168.2.159";
                }

            });
            if (Ipserver == "")
            {
                MessageBox.Show("Không thể kết nối với máy chủ\n Hãy kiểm tra lại địa chỉ IP", "Lỗi");
                Environment.Exit(0);
            }
            CultureInfo ct = new CultureInfo("en-US");
            ct.DateTimeFormat.LongDatePattern = "yyyy-MM-dd HH:mm:ss";
            ct.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            ct.DateTimeFormat.AMDesignator = "";
            ct.DateTimeFormat.PMDesignator = "";
            Thread.CurrentThread.CurrentCulture = ct;
            DateTime mydt = DateTime.Now;
            this.DataContext = this;
            listimage = new ObservableCollection<ImageReport>();
            listchecksheet = new ObservableCollection<CheckSheet>();
            Wd_MainWindow.WindowState = WindowState.Maximized;
            txb_QRCode.Focus();
            try
            {
                LoadNameLineFromDataBase();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Lỗi kết nối");
            }
            if(DateTime.Now.Month == 1)
            {
                listmonth.Add("1");
                listmonth.Add("12");
            }
            else
            {
                listmonth.Add($"{DateTime.Now.Month.ToString()}");
                listmonth.Add($"{(DateTime.Now.Month-1).ToString()}");

            }
            cmb_month.SelectedItem = DateTime.Now.Month.ToString();
            cmb_month.ItemsSource = listmonth;
            m_busmgr = new ManagedBusManager();
            m_ctldlg = new CameraControlDialog();
            m_selDlg = new CameraSelectionDialog();
            m_image = new ManagedImage();
            m_converted = new ManagedImage();
            m_bitmap = new BitmapImage();
            m_worker = new BackgroundWorker();
            m_worker.WorkerReportsProgress = true;
            m_worker.DoWork += new DoWorkEventHandler(m_worker_DoWork);
            m_worker.ProgressChanged += new ProgressChangedEventHandler(m_worker_ProgressChanged);
            m_Done = new AutoResetEvent(false);
            RenderOptions.SetBitmapScalingMode(myImage, BitmapScalingMode.LowQuality);
            RenderOptions.SetEdgeMode(myImage, EdgeMode.Aliased);
            if (m_selDlg.ShowModal())
            {
                ManagedPGRGuid[] guids = m_selDlg.GetSelectedCameraGuids();

                // Determine camera interface
                var interfaceType = m_busmgr.GetInterfaceTypeFromGuid(guids[0]);

                if (interfaceType == InterfaceType.GigE)
                {
                    m_cam = new ManagedGigECamera();
                }
                else
                {
                    m_cam = new ManagedCamera();
                }

                // Connect to camera object
                m_cam.Connect(guids[0]);

                // Connect control dialog
                m_ctldlg.Connect(m_cam);

                // Start capturing
                m_cam.StartCapture();

                WorkerHelper helper = new WorkerHelper();
                helper.converted = m_converted;
                helper.raw = m_image;
                helper.cam = m_cam;
                m_continue = true;
                m_worker.RunWorkerAsync(helper);
            }
            else
            {

                MessageBox.Show("Không thể kết nối đến Camera", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }
        }


        private void m_worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BitmapSource image = (BitmapSource)e.UserState;
            TransformedBitmap rotatedImage = new TransformedBitmap();
            rotatedImage.BeginInit();
            rotatedImage.Source = image;
            RotateTransform rotateTransform = new RotateTransform(90);
            rotatedImage.Transform = rotateTransform;
            rotatedImage.EndInit();
            this.Dispatcher.Invoke(DispatcherPriority.Render,
            (ThreadStart)delegate ()
            {
                myImage.Source = rotatedImage;
            }
            );
        }
        private void m_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (m_continue)
            {
                try
                {
                    BackgroundWorker worker = (BackgroundWorker)sender;
                    WorkerHelper helper = (WorkerHelper)e.Argument;
                    helper.cam.RetrieveBuffer(helper.raw);
                    helper.raw.ConvertToBitmapSource(helper.converted);
                    helper.source = helper.converted.bitmapsource;
                    helper.source.Freeze();
                    worker.ReportProgress(0, helper.source);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                    GC.Collect();
                }
            }
        }

        void SaveToPng(string PathFolder, string NameImage)
        {
            BitmapSource originalImage = (BitmapSource)myImage.Source;
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(originalImage));
            string FoldersaveImage = PathFolder + "\\" + cmb_year.Text.ToString() + "\\" + cmb_month.SelectedItem.ToString() + "\\" + cmb_line.SelectedItem.ToString(); ;
            if (!Directory.Exists(FoldersaveImage))
            {
                Directory.CreateDirectory(FoldersaveImage);
            }
            string FileNameImage = FoldersaveImage + "\\" + NameImage+".png";
            using (var stream = new FileStream(FileNameImage, FileMode.Create))
            {
                encoder.Save(stream);
                stream.Close();
            };

        }
        private void txb_QRCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txb_QRCode.Text.Contains("@"))
                {
                    endreadQR = true;
                    QRcodefromBarcode = txb_QRCode.Text.Remove(txb_QRCode.Text.Length - 1, 1).Split('_')[0];
                    LoadNameCheckSheetFromDataBase();
                    var kq1 = from p in listchecksheet
                              where p.NameCheckSheet == txb_QRCode.Text.Substring(0, txb_QRCode.Text.Length - 1)
                              select p;
                    if (kq1.Count() > 0)
                    {
                        cmb_line.SelectedItem = txb_QRCode.Text.Remove(txb_QRCode.Text.Length - 1, 1).Split('_')[0];
                        txb_QRCode.Text = txb_QRCode.Text.Remove(txb_QRCode.Text.Length - 1, 1);
                        if (File.Exists($"\\\\{Ipserver}\\Baocao2$\\{cmb_year.Text.ToString()}\\{cmb_month.SelectedItem.ToString()}\\{cmb_line.SelectedItem.ToString()}\\{txb_QRCode.Text}.png"))
                        {
                            
                            var kq = from p in listimage
                                     where p.Pathnameimage == $"\\\\{Ipserver}\\Baocao2$\\{cmb_year.Text.ToString()}\\{cmb_month.SelectedItem.ToString()}\\{cmb_line.SelectedItem.ToString()}\\{txb_QRCode.Text}.png"
                                     select p;
                            kq.ToList().ForEach((p) =>
                            {
                                listimage.Remove(p);
                            });
                            File.Delete($"\\\\{Ipserver}\\Baocao2$\\{cmb_year.Text.ToString()}\\{cmb_month.SelectedItem.ToString()}\\{cmb_line.SelectedItem.ToString()}\\{txb_QRCode.Text}.png");
                            SaveToPng($"\\\\{Ipserver}\\Baocao2$", txb_QRCode.Text);
                            AddDatatoDataLog($"\\\\{Ipserver}\\Baocao2$\\{cmb_year.Text.ToString()}\\{cmb_month.SelectedItem.ToString()}\\{cmb_line.SelectedItem.ToString()}".Replace('\\', '/'));
                            txb_QRCode.Text = "";
                            LoadimageToListView($"\\\\{Ipserver}\\Baocao2$\\{cmb_year.Text.ToString()}\\{cmb_month.SelectedItem.ToString()}\\{cmb_line.SelectedItem.ToString()}");
                        }
                        else
                        {
                            SaveToPng($"\\\\{Ipserver}\\Baocao2$", txb_QRCode.Text);
                            AddDatatoDataLog($"\\\\{Ipserver}\\Baocao2$\\{cmb_year.Text.ToString()}\\{cmb_month.SelectedItem.ToString()}\\{cmb_line.SelectedItem.ToString()}".Replace('\\', '/'));
                            txb_QRCode.Text = "";
                            LoadimageToListView($"\\\\{Ipserver}\\Baocao2$\\{cmb_year.Text.ToString()}\\{cmb_month.SelectedItem.ToString()}\\{cmb_line.SelectedItem.ToString()}");

                        }
                    }
                    else
                    {
                        LoadimageToListView($"\\\\{Ipserver}\\Baocao2$\\{cmb_year.Text.ToString()}\\{cmb_month.SelectedItem.ToString()}\\{cmb_line.SelectedItem.ToString()}");

                        txb_QRCode.Text = "";
                        MessageBox.Show("Mã QRCode chưa đúng");
                    }
                    endreadQR = false;
                    QRcodefromBarcode = "";

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           
        }
        public void LoadimageToListView(string pathfolder)
        {
            listimage.Clear();
            DirectoryInfo df = new DirectoryInfo(pathfolder);
            FileInfo[] FilePng;
            try
            {
                FilePng = df.GetFiles("*.png");
            }
            catch (Exception)
            {
                checkfinish(pathfolder);
                return;
            }
            var FilePngDes = FilePng.ToList().OrderByDescending((p) =>
            {
                return p.LastWriteTime;
            });
            foreach (FileInfo fi2 in FilePngDes)
            {
                listimage.Add(new ImageReport() { Pathnameimage = fi2.FullName, Nameimage = fi2.FullName.Split('\\').Last().Split('.')[0] });
            }
            checkfinish(pathfolder);
        }

        public void LoadNameLineFromDataBase()
        {
            using (MySqlConnection conn = new MySqlConnection($"server={Ipserver};uid=root;" + "pwd=6006;database=test"))
            {
                conn.Open();
                MySqlDataAdapter adap = new MySqlDataAdapter("SELECT * FROM checkmachine.machecksheet group by NameLine", conn);
                DataTable dt = new DataTable();
                adap.Fill(dt);
                var mydata = from DataRow p in dt.Rows
                             select (string)p["NameLine"];
                cmb_line.ItemsSource = mydata;
                conn.Close();
            }
        }
        public void LoadNameCheckSheetFromDataBase()
        {
            
            if(cmb_line.SelectedItem != null)
            {
                listchecksheet.Clear();
                using (MySqlConnection conn = new MySqlConnection($"server={Ipserver};uid=root;" + "pwd=6006;database=test"))
                {
                    conn.Open();
                    MySqlDataAdapter adap = new MySqlDataAdapter($"SELECT * FROM checkmachine.machecksheet where NameLine ='{cmb_line.SelectedItem.ToString()}'", conn);
                    DataTable dt = new DataTable();
                    adap.Fill(dt);
                    var mydata = from DataRow p in dt.Rows
                                 select (string)p["IdCheckSheet"];
                    mydata.ToList().ForEach((p) =>
                    {
                        listchecksheet.Add(new CheckSheet() { NameCheckSheet = p });
                    });
                    conn.Close();
                }
                ToltalCheckSheet = listchecksheet.Count().ToString();
            }
            if (endreadQR)
            {
                listchecksheet.Clear();
                using (MySqlConnection conn = new MySqlConnection($"server={Ipserver};uid=root;" + "pwd=6006;database=test"))
                {
                    conn.Open();
                    MySqlDataAdapter adap = new MySqlDataAdapter($"SELECT * FROM checkmachine.machecksheet where NameLine ='{QRcodefromBarcode}'", conn);
                    DataTable dt = new DataTable();
                    adap.Fill(dt);
                    var mydata = from DataRow p in dt.Rows
                                 select (string)p["IdCheckSheet"];
                    mydata.ToList().ForEach((p) =>
                    {
                        listchecksheet.Add(new CheckSheet() { NameCheckSheet = p });
                    });
                    conn.Close();
                }
                ToltalCheckSheet = listchecksheet.Count().ToString();
            }


        }

        private void cmb_line_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                LoadNameCheckSheetFromDataBase();
                if (cmb_month.SelectedItem != null && cmb_line.SelectedItem != null)
                {
                    LoadimageToListView($"\\\\{Ipserver}\\Baocao2$\\{cmb_year.Text.ToString()}\\{cmb_month.SelectedItem.ToString()}\\{cmb_line.SelectedItem.ToString()}");
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Lỗi kết nối");
            }
            
        }
        public void AddDatatoDataLog(string locationfile)
        {
            using (MySqlConnection conn = new MySqlConnection($"server={Ipserver};uid=root;" + "pwd=6006;database=test"))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"INSERT INTO `checkmachine`.`logdata` (`TimeUpData`, `NameLine`, `NameImage`, `LocationFIle`) VALUES ('{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{cmb_line.SelectedItem.ToString()}', '{txb_QRCode.Text}', '{locationfile}');", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void checkfinish(string pathfolder)
        {
            int countfinish = 0;
            DirectoryInfo df = new DirectoryInfo(pathfolder);
            FileInfo[] FilePng;
            try
            {
                FilePng = df.GetFiles("*.png");
            }
            catch (Exception)
            {
                FinishCheckSheet = "0";
                return;
            }
            var FilePngDes = FilePng.ToList().OrderByDescending((p) =>
            {
                return p.LastWriteTime;
            });
            listchecksheet.ToList().ForEach((p) =>
            {
                listimage.ToList().ForEach((p1) =>
                {
                    if(p.NameCheckSheet == p1.Nameimage)
                    {
                        p.StatusDone = true;
                        countfinish++;
                    }
                });
            });
            FinishCheckSheet = countfinish.ToString();
            
            
            
        }

        private void cmb_month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmb_month.SelectedItem.ToString() == "12")
                {
                    cmb_year.Text = (DateTime.Now.Year - 1).ToString();
                }
                else
                {
                    cmb_year.Text = DateTime.Now.Year.ToString();
                }
                LoadNameCheckSheetFromDataBase();
                if (cmb_line.SelectedItem != null)
                {
                    LoadimageToListView($"\\\\{Ipserver}\\Baocao2$\\{cmb_year.Text.ToString()}\\{cmb_month.SelectedItem.ToString()}\\{cmb_line.SelectedItem.ToString()}");

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Lỗi kết nối");
            }
           

        }
    }
    
}
