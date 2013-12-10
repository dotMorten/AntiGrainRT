using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI;

namespace AntiGrainRT.Tests
{
	[TestClass]
	public class RenderingBufferTests
	{
		[TestMethod]
		public void ConstructorTest()
		{
			AntiGrainRT.RenderingBuffer c = new RenderingBuffer(100, 200, PixelFormat.Rgb24);
			Assert.AreEqual((uint)100, c.PixelWidth);
			Assert.AreEqual((uint)200, c.PixelHeight);
			Assert.AreEqual(PixelFormat.Rgb24, c.PixelFormat);
		}

		[TestMethod]
		public void SetPixelTestRgb()
		{
			AntiGrainRT.RenderingBuffer c = new RenderingBuffer(1, 1, PixelFormat.Rgb24);
			c.SetPixel(0, 0, Color.FromArgb(1, 2, 3, 4));
			var color = c.GetPixel(0, 0);
			Assert.AreEqual((byte)2, color.R);
			Assert.AreEqual((byte)3, color.G);
			Assert.AreEqual((byte)4, color.B);
		}

		[TestMethod]
		public void SetPixelTestBgr()
		{
			AntiGrainRT.RenderingBuffer c = new RenderingBuffer(1, 1, PixelFormat.Bgr24);
			c.SetPixel(0, 0, Color.FromArgb(1, 2, 3, 4));
			var color = c.GetPixel(0, 0);
			Assert.AreEqual((byte)2, color.R);
			Assert.AreEqual((byte)3, color.G);
			Assert.AreEqual((byte)4, color.B);
		}
	}
}
