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

namespace RANSAC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BitmapImage firstImage;
        private ImageFeature<float>[] features1;

        private BitmapImage secondImage;
        private ImageFeature<float>[] features2;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void load1_click(object sender, RoutedEventArgs e)
        {
            image1.Source = loadImage(firstImage);
        }

        private void load2_click(object sender, RoutedEventArgs e)
        {
            image2.Source = loadImage(secondImage);
        }

        private ImageSource loadImage(BitmapImage image)
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
               return image;
            }

            return null;
        }
    }
}
