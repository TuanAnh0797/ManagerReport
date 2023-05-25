using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ManagerReport
{
    /// <summary>
    /// Interaction logic for PreviewPicture.xaml
    /// </summary>
    public partial class PreviewPicture : Window
    {
        public BitmapSource imagepreview;
        public PreviewPicture(string pathimagepreview, string nameimage)
        {
            InitializeComponent();
            mywindow.WindowState = WindowState.Maximized;
            txb_NameImage.Text = nameimage;
            var bmp = new BitmapImage(new Uri(pathimagepreview));
            mywindow.Title = $"PreviewPicture-{nameimage}";
            ImagePreview.Source = bmp;
        }

        private void mywindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space || e.Key == Key.Enter)
            {
                this.Close();
            }
            
                
        }
    }
}
