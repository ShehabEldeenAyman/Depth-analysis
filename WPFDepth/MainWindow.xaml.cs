//WPF Greyscale viewer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using Microsoft.Kinect;

namespace WPFDepth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor sensor;
        public MainWindow()
        {
            InitializeComponent();
            sensor = KinectSensor.KinectSensors[0];
            sensor.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
            sensor.DepthFrameReady += DepthFrameReady;
            sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            sensor.ColorFrameReady += FrameReady;
            sensor.Start();
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
        }

        void FrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            ColorImageFrame imageFrame = e.OpenColorImageFrame();
            if (imageFrame != null)
            {
                BitmapSource bmap = ImageToBitmap(imageFrame);
                image2.Source = bmap;
            }
        }

        BitmapSource ImageToBitmap(ColorImageFrame Image)
        {
            byte[] pixeldata = new byte[Image.PixelDataLength];
            Image.CopyPixelDataTo(pixeldata);
            BitmapSource bmap = BitmapSource.Create(
             Image.Width,
             Image.Height,
             640, 480,
             PixelFormats.Bgr32,
             null,
             pixeldata,
             Image.Width * Image.BytesPerPixel);
            return bmap;
        }

        void DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            DepthImageFrame imageFrame = e.OpenDepthImageFrame();
            if (imageFrame != null)
            {
                image1.Source = DepthToBitmapSource(imageFrame);

            }
                      
        }

        int getValue(DepthImageFrame imageFrame, int x, int y)
        {
            short[] pixelData = new short[imageFrame.PixelDataLength];
            imageFrame.CopyPixelDataTo(pixelData);
            return ((ushort)pixelData[x + y * imageFrame.Width]) >> 3;

        }
        BitmapSource DepthToBitmapSource(DepthImageFrame imageFrame)
        {
            short[] pixelData = new short[imageFrame.PixelDataLength];
            imageFrame.CopyPixelDataTo(pixelData);
            /////////////////////////////
            


            BitmapSource bmap = BitmapSource.Create(
            imageFrame.Width,
            imageFrame.Height,
            320, 240,
            PixelFormats.Gray16,
            null,
            pixelData,
            imageFrame.Width*imageFrame.BytesPerPixel);

           
            //label1.Content = imageFrame.Format;
            return bmap;
        }
    }
}
