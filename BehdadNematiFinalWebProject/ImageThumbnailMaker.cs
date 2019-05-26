using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Shop
{
    public class ImageThumbnailMaker
    {
        public ImageThumbnailMaker()
        {
        }

        public byte[] CreateThumbNail(Image image,int height=800,int width=800)
        {
            Bitmap bitmap = new Bitmap(image, height, width);
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Jpeg);
            //return Image.FromStream(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
