using System.Drawing;

namespace Photr_LBPrjoect.Model
{
    public class PhotrDb
    {
        public int ID { get; set; }
        public string Titel { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Author { get; set; }
        public Bitmap MainPic { get; set; }
    }
}
