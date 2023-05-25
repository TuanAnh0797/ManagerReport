using ManagerReport.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ManagerReport.ViewModel
{
    
    public class ControlBarViewModelcs : BaseViewModel
    {
        public string imagesource { get; set; }
        public ICommand cmd_CloseWindow { get; set; }
        public ICommand cmd_NormalWindow {  get; set; }
        public ICommand cmd_MinimizeWindow { get; set; }
        public ICommand cmd_MoveWindow { get; set; }
        public ControlBarViewModelcs()
        {
            
            cmd_CloseWindow = new RelayCommand<UserControl>((p) => { return true; }, (p) => MethodCloseWindow(p));
            cmd_NormalWindow = new RelayCommand<UserControl>((p) => { return true;}, (p)=>MethodNormalWindow(p));
            cmd_MinimizeWindow = new RelayCommand<UserControl>((p) => { return true; }, (p) => MethodMinimizeWindow(p));
            cmd_MoveWindow = new RelayCommand<UserControl>((p) => { return p != null ? true : false; }, (p) => MethodMoveWindow(p));
        }
        public void MethodCloseWindow(UserControl p)
        {
            var MyWindow = Window.GetWindow(p);
            if (MyWindow != null)
            {
                MyWindow.Close();
            }
        }
        public void MethodNormalWindow(UserControl p)
        {
            var MyWindow = Window.GetWindow(p);
            if (MyWindow != null)
            {
                if(MyWindow.WindowState == WindowState.Maximized)
                {
                    MyWindow.WindowState = WindowState.Normal;
                }
                else
                {
                    MyWindow.WindowState = WindowState.Maximized;
                }
            }
            
        }
        public void MethodMinimizeWindow(UserControl p)
        {
            var MyWindow = Window.GetWindow(p);
            if (MyWindow != null)
            {
                MyWindow.WindowState = WindowState.Minimized;
            }
        }
        public void MethodMoveWindow(UserControl p)
        {
            var Mywindow = Window.GetWindow(p);
            if(Mywindow != null)
            {
                Mywindow.DragMove();
            }
        }
    }
}
