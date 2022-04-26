using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels
{
    public class MainPageViewModel
    {
        public string Titel { get; set; }
        public double Price { get; set; }
        public string Author { get; set; }
        public Bitmap TitelPic { get; set; }
    }
}
