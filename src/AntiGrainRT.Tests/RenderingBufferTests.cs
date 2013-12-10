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
			AntiGrainRT.RenderingBuffer c = new RenderingBuffer(100, 200);
			Assert.AreEqual((uint)100, c.PixelWidth);
			Assert.AreEqual((uint)200, c.PixelHeight);
		}
	}
}
