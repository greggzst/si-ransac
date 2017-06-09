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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.Features2D;
using System.Drawing;
using RANSAC.Structures;
using Microsoft.Win32;
using RANSAC.Utilities;

namespace RANSAC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BitmapImage firstImage;
        private Bitmap firstBitmap;
        private Bitmap firstOriginal;
        private ImageFeature<float>[] features1;

        private BitmapImage secondImage;
        private Bitmap secondBitmap;
        private Bitmap secondOriginal;
        private ImageFeature<float>[] features2;

        public MainWindow()
        {
            InitializeComponent();
        }

        public static ImageFeature<float>[] getImageFeatures(Bitmap bitmap)
        {
            Image<Gray, Byte> image = new Image<Gray, Byte>(bitmap);
            var detector = new SIFTDetector();
            return detector.DetectFeatures(image, null);
        }

        private void load1_click(object sender, RoutedEventArgs e)
        {
            image1.Source = loadImage(out firstImage, out firstBitmap,out firstOriginal);
        }

        private void load2_click(object sender, RoutedEventArgs e)
        {
            image2.Source = loadImage(out secondImage,out secondBitmap,out secondOriginal);
        }

        private ImageSource loadImage(out BitmapImage image, out Bitmap bitmap, out Bitmap bitmapOriginal)
        {
            OpenFileDialog op = new OpenFileDialog
            {
                Filter = "|*.jpg;*.jpeg;*.png|" +
                         "|*.jpg;*.jpeg|" +
                         "|*.png"
            };
            if (op.ShowDialog() == true)
            {
               image = new BitmapImage(new Uri(op.FileName));
               bitmap = new Bitmap(op.FileName);
               bitmapOriginal = new Bitmap(op.FileName);
               return image;
            }

            image = null;
            bitmap = null;
            bitmapOriginal = null;
            return null;
        }

        private void drawFeaturesOnBitmap(ImageFeature<float>[] features, Bitmap bitmap)
        {
            foreach (ImageFeature<float> element in features)
            {
                var pointX = element.KeyPoint.Point.X;
                var pointY = element.KeyPoint.Point.Y;

                Utilities.Drawing.drawPointOnBitmap(bitmap, (int)pointX, (int)pointY);
            }
        }

        private void features_click(object sender, RoutedEventArgs e)
        {
            if(firstImage != null)
            {
                features1 = getImageFeatures(firstBitmap);
                drawFeaturesOnBitmap(features1, firstBitmap);
            }

            if (secondImage != null)
            {
                features2 = getImageFeatures(secondBitmap);
                drawFeaturesOnBitmap(features2, secondBitmap);
            }

            if(firstImage != null && secondImage != null)
            {
                Bitmap result = Utilities.Drawing.mergeImages(firstBitmap, secondBitmap);
                resultImage.Source = Utilities.Drawing.imageFromBitmap(result);
                keyPoints.IsEnabled = true;
                neighbour.IsEnabled = true;
                neighbours.IsEnabled = true;
                threshold.IsEnabled = true;
            }
            else
            {
                showMessage("Both images must be loaded first!");
            }
        }

        private void keyPoints_click(object sender, RoutedEventArgs e)
        {
            var keyPointsPairs = BasePointUtilities.getKeyPointsPairs(features1, features2);
            Bitmap result = Utilities.Drawing.generateNewImage(firstOriginal, secondOriginal, keyPointsPairs, System.Drawing.Color.Brown);

            resultImage.Source = Utilities.Drawing.imageFromBitmap(result);
        }

        private void neighbourhood_click(object sender, RoutedEventArgs e)
        {
            var keyPointsPairs = BasePointUtilities.getKeyPointsPairs(features1, features2);
            var reducedKeyPointsPairs = BasePointUtilities.neighbourFilter(keyPointsPairs, int.Parse(neighbours.Text), double.Parse(threshold.Text));
            Bitmap result = Utilities.Drawing.generateNewImage(firstOriginal, secondOriginal, reducedKeyPointsPairs, System.Drawing.Color.Aqua);
            resultImage.Source = Utilities.Drawing.imageFromBitmap(result);
        }

        private void showMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
