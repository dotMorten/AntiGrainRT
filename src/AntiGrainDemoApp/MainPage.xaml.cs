using AntiGrainRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
