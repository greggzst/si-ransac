using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace RANSAC.Utilities
{
    class Drawing
    {
        public static Bitmap mergeImages(Bitmap image1, Bitmap image2)
        {
            Bitmap result = new Bitmap(image1.Width + image2.Width, image1.Height);
            Graphics graphics = Graphics.FromImage(result);
            graphics.DrawImage(image1, 0, 0, image1.Width, image1.Height);
            graphics.DrawImage(image2, image1.Width, 0, image2.Width, image2.Height);

            return result;
        }


    }
}
