using AntiGrainRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AntiGrainDemoApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			CreateImage();
			LoadLion();
		}

		private async void LoadLion()
		{
			StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///LionShape.Txt"));
			string shapeString = "";
			using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
			{
				shapeString = new StreamReader(fileStream.AsStreamForRead()).ReadToEnd();
			}
			Grid g = new Grid()
			{
				Background = new SolidColorBrush(Colors.CornflowerBlue),
				VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top,
				HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left
			};
			foreach(var shape in ShapeReader.Parse(shapeString))
			{
				var p = new Windows.UI.Xaml.Shapes.Path() { Data = shape.Geometry, Fill = new SolidColorBrush(shape.Color) };
				g.Children.Add(p);
			}
			LayoutRoot.Children.Add(g);
		}

		private async void CreateImage()
		{
			var c = new RenderingBuffer(100, 100, Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8);
			for (uint i = 0; i < c.PixelWidth; i++)
			{
				c.SetPixel(i, i, Colors.Blue);
			}
			var src = await c.CreateImageSourceAsync();
			Image img = new Image() { Source = src };
			LayoutRoot.Children.Add(img);
		}
    }
}
