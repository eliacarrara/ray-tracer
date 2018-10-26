using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RayTracer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        Scene s;
        Renderer r;
        readonly Int32Rect rect;
        readonly WriteableBitmap bmp;
        public MainWindow()
        {
            InitializeComponent();

            var width = (int)PaintCanvas.Width;
            var height = (int)PaintCanvas.Height;
            bmp = new WriteableBitmap(width, height, 96, 96, PixelFormats.Rgb24, null);
            var stride = Helper.CalcStride(width, bmp.Format.BitsPerPixel);

            r = new Renderer(width, height, stride);
            rect = new Int32Rect(0, 0, width, height);

            BoolDiffuse.IsChecked = r.Settings.Diffuse;
            BoolSpecular.IsChecked = r.Settings.Specular;
            BoolShadow.IsChecked = r.Settings.Shadow;
            BoolReflection.IsChecked = r.Settings.Reflection;

            //s = SceneFactory.CreateCornellBox();
            s = SceneFactory.CreateThisThing();
            //s = SceneFactory.CreateRandomSphereScene(1000, 0.1f);
            //s = SceneFactory.CreateCornellBoxTriLights();

            Paint();
        }

        private void BtnRender_Click(object sender, RoutedEventArgs e)
        {
            BtnRender.IsEnabled = false;
            LblRender.Content = "-";

            r.Settings.Diffuse = BoolDiffuse.IsChecked.Value;
            r.Settings.Specular = BoolSpecular.IsChecked.Value;
            r.Settings.Shadow = BoolShadow.IsChecked.Value;
            r.Settings.Reflection = BoolReflection.IsChecked.Value;

            Paint();
            BtnRender.IsEnabled = true;
        }

        private void Paint()
        {
            byte[] data;
            var sw = new Stopwatch();
            
            sw.Start();
            data = r.Render(s);
            sw.Stop();
            LblRender.Content = string.Format("Render time: {0:N2}ms", sw.Elapsed.TotalMilliseconds);

            bmp.WritePixels(rect, data, r.Stride, 0);
            SaveImage(bmp, @".\output.png");
            PaintCanvas.Background = new ImageBrush { ImageSource = bmp };
        }

        private void SaveImage(BitmapSource bmp, string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(bmp));
            png.Save(fs);
            fs.Close();
        }
    }
}