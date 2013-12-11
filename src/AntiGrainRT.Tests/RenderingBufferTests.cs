using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

namespace AntiGrainRT.Tests
{
	[TestClass]
	public class RenderingBufferTests
	{
		[TestMethod]
		public void ConstructorTest()
		{
			AntiGrainRT.RenderingBuffer c = new RenderingBuffer(100, 200, BitmapPixelFormat.Rgba8);
			Assert.AreEqual((uint)100, c.PixelWidth);
			Assert.AreEqual((uint)200, c.PixelHeight);
			Assert.AreEqual(BitmapPixelFormat.Rgba8, c.PixelFormat);
		}

		[TestMethod]
		public void SetPixelTestRgb()
		{
			AntiGrainRT.RenderingBuffer c = new RenderingBuffer(1, 1, BitmapPixelFormat.Rgba8);
			c.SetPixel(0, 0, Color.FromArgb(1, 2, 3, 4));
			var color = c.GetPixel(0, 0);
			Assert.AreEqual((byte)1, color.A);
			Assert.AreEqual((byte)2, color.R);
			Assert.AreEqual((byte)3, color.G);
			Assert.AreEqual((byte)4, color.B);
		}

		[TestMethod]
		public void SetPixelTestBgr()
		{
			AntiGrainRT.RenderingBuffer c = new RenderingBuffer(1, 1, BitmapPixelFormat.Bgra8);
			c.SetPixel(0, 0, Color.FromArgb(1, 2, 3, 4));
			var color = c.GetPixel(0, 0);
			Assert.AreEqual((byte)1, color.A);
			Assert.AreEqual((byte)2, color.R);
			Assert.AreEqual((byte)3, color.G);
			Assert.AreEqual((byte)4, color.B);
		}

		//[TestMethod]
		//public async Task SavePpm()
		//{
		//	AntiGrainRT.RenderingBuffer c = new RenderingBuffer(100, 100, BitmapPixelFormat.Bgra8);
		//	for (uint i = 0; i < c.PixelWidth; i++)
		//	{
		//		c.SetPixel(i, i, Colors.Blue);
		//	}
		//	var file = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("test.ppm");
		//	await c.SavePpmAsync(file);
		//}

		[TestMethod]
		public async Task CreateImageSourceTest()
		{
			AntiGrainRT.RenderingBuffer c = new RenderingBuffer(100, 100, BitmapPixelFormat.Bgra8);
			for (uint i = 0; i < c.PixelWidth; i++)
			{
				c.SetPixel(i, i, Colors.Blue);
			}
			var src = await c.CreateImageSourceAsync();
			Assert.IsNotNull(src);
		}
	}
}
