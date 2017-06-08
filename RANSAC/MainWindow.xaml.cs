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

namespace RANSAC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private ImageFeature<float>[] getImageFeatures(Bitmap bitmap)
        {
            Image<Gray, Byte> image = new Image<Gray, Byte>(bitmap);
            var detector = new SIFTDetector();
            return detector.DetectFeatures(image, null);
        }

        private List<Structures.Point> getKeyPoints(ImageFeature<float>[] features)
        {
            List<Structures.Point> keyPointList = new List<Structures.Point>();
            foreach (var feature in features)
            {
                var pointCoordinatesAndFeatures = feature.Descriptor;
                var pointFeatures = pointCoordinatesAndFeatures.Skip(5).ToArray();
                keyPointList.Add(new Structures.Point(pointCoordinatesAndFeatures[0], pointCoordinatesAndFeatures[1], pointFeatures));
            }
            return keyPointList;
        }

        private List<Tuple<Structures.Point, Structures.Point>> getKeyPointPairs(List<Structures.Point> firstKeyPoints, List<Structures.Point> secondKeyPoints)
        {
            List<Tuple<Structures.Point, Structures.Point>> firstPicturePairs = getPointsPair(firstKeyPoints, secondKeyPoints);
            List<Tuple<Structures.Point, Structures.Point>> secondPicturePairs = getPointsPair(secondKeyPoints, firstKeyPoints);

            List<Tuple<Structures.Point, Structures.Point>> keyPoints = new List<Tuple<Structures.Point, Structures.Point>>();

            foreach (var firstPoint in firstPicturePairs)
            {
                foreach (var secondPoint in secondPicturePairs)
                {
                    if(isMutual(firstPoint,secondPoint))
                    {
                        keyPoints.Add(firstPoint);
                    }
                }
            }

            return keyPoints;

        }

        private bool isMutual(Tuple<Structures.Point,Structures.Point> firstPair, Tuple<Structures.Point,Structures.Point> secondPair)
        {
            return firstPair.Item1.Features.SequenceEqual(secondPair.Item2.Features) && firstPair.Item2.Features.SequenceEqual(secondPair.Item1.Features);
        }

        private List<Tuple<Structures.Point, Structures.Point>> getPointsPair(List<Structures.Point> firstKeyPoints, List<Structures.Point> secondKeyPoints)
        {
            List<Tuple<Structures.Point, Structures.Point>> picturePointsPairs = new List<Tuple<Structures.Point, Structures.Point>>();

            foreach (var point in firstKeyPoints)
            {
                double distance = double.MaxValue;
                int index = 0;

                for (int i = 0; i < secondKeyPoints.Count; i++)
                {
                    var p = secondKeyPoints.ElementAt(i);
                    double dist = point.distance(p);
                    if (dist < distance)
                    {
                        distance = dist;
                        index = i;
                    }
                }

                picturePointsPairs.Add(new Tuple<Structures.Point, Structures.Point>(point, secondKeyPoints.ElementAt(index)));
            }

            return picturePointsPairs;
        }
    }
}
