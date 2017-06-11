using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using RANSAC.Structures;

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

        public static void DrawKeyPoints(Bitmap image, List<Tuple<FPoint, FPoint>> keyPoints, Color penColor, int offset = 0)
        {
            Graphics graphics = Graphics.FromImage(image);

            foreach (var point in keyPoints)
            {
                FPoint point1 = point.Item1;
                FPoint point2 = point.Item2;
                Pen pen = new Pen(penColor, 2);

                drawPointOnBitmap(image, (int)point1.X, (int)point1.Y);
                drawPointOnBitmap(image, (int)point2.X + offset, (int)point2.Y);
                graphics.DrawLine(pen, point1.X, point1.Y, point2.X + offset, point2.Y);
            }
        }

        public static Bitmap generateNewImage(Bitmap image1, Bitmap image2, List<Tuple<FPoint, FPoint>> keyPoints, Color penColor)
        {
            var other = keyPoints;

            Bitmap newImage = mergeImages(image1, image2);
            DrawKeyPoints(newImage, other, penColor, image1.Width);
            return newImage;
        }

        public static void drawPointOnBitmap(Bitmap bitmap, int x, int y)
        {
            for (int mod_x = -3; mod_x <= 3; mod_x++)
            {
                for (int mod_y = -3; mod_y <= 3; mod_y++)
                {
                    int newX = x + mod_x;
                    int newY = y + mod_y;

                    if (newX >= 0 && newX < bitmap.Width && newY >= 0 && newY < bitmap.Height)
                    {
                        bitmap.SetPixel(newX, newY, System.Drawing.Color.Yellow);
                    }
                }
            }
        }

        public static BitmapImage imageFromBitmap(Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();

            bitmap.Save(@"E:\Studia\VI semestr\SI\lab\lab4\result.jpg");

            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }

            return bitmapImage;
        }
        

    }
}
