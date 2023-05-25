using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ManagerReport
{
    public class ImageReport
    {
        private string pathnameimage;
        private string nameimage;
        
        public string Nameimage { get => nameimage; set => nameimage = value; }
        public string Pathnameimage { get => pathnameimage; set => pathnameimage = value; }
    }
}
