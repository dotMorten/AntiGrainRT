﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

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
			AntiGrainRT.RenderingBuffer c = new RenderingBuffer(3, 2, BitmapPixelFormat.Rgba8);
			c.SetPixel(2, 1, Color.FromArgb(1, 2, 3, 4));
			var color = c.GetPixel(2, 1);
			Assert.AreEqual((byte)1, color.A);
			Assert.AreEqual((byte)2, color.R);
			Assert.AreEqual((byte)3, color.G);
			Assert.AreEqual((byte)4, color.B);
		}

		[TestMethod]
		public void SetPixelTestBgr()
		{
			AntiGrainRT.RenderingBuffer c = new RenderingBuffer(3, 2, BitmapPixelFormat.Bgra8);
			c.SetPixel(2, 1, Color.FromArgb(1, 2, 3, 4));
			var color = c.GetPixel(2, 1);
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
			AntiGrainRT.RenderingBuffer c = new RenderingBuffer(100, 150, BitmapPixelFormat.Bgra8);
			for (uint i = 0; i < c.PixelWidth; i++)
			{
				c.SetPixel(i, i, Colors.Blue);
			}
			Windows.UI.Xaml.Media.ImageSource src = null;
			var taskSource = new TaskCompletionSource<object>();
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
				CoreDispatcherPriority.Normal, async () =>
				{
					try
					{
						src = await c.CreateImageSourceAsync();
						Assert.IsNotNull(src);
						Assert.IsInstanceOfType(src, typeof(Windows.UI.Xaml.Media.Imaging.BitmapImage));
						var bmp = (Windows.UI.Xaml.Media.Imaging.BitmapImage)src;
						Assert.AreEqual(100, bmp.PixelWidth);
						Assert.AreEqual(150, bmp.PixelHeight);
						taskSource.SetResult(null);
					}
					catch (Exception e)
					{
						taskSource.SetException(e);
					}
				});
			await taskSource.Task;
		}
	}
}
