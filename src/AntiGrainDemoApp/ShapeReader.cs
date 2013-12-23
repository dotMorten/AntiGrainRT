using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace AntiGrainDemoApp
{
	internal class ShapeReader
	{
		public class Drawing
		{
			public Color Color { get; set; }
			public PathGeometry Geometry { get; set; }
		}

		public static IEnumerable<Drawing> Parse(string shapeString)
		{
			PathGeometry geom = null;
			Color color = Colors.Black;
			using (System.IO.StringReader reader = new System.IO.StringReader(shapeString))
			{
				string LineString = reader.ReadLine();
				while (LineString != null)
				{
					int c;
					if (!LineString.StartsWith("//"))
					{
						if (LineString.Length > 0
							&& LineString[0] != 'M'
							&& Int32.TryParse(LineString, NumberStyles.HexNumber, null, out c))
						{
							// New color. Every new color creates new path in the path object.
							if (geom != null && geom.Figures.Count > 0)
							{
								yield return new Drawing() { Color = color, Geometry = geom };
								geom = null;
							}
							color = IntToColor(c);
							color.A = 255;
						}
						else
						{
							bool startedPoly = false;
							string[] splitOnSpace = LineString.Split(' ');
							PathFigure path = null;
							for (int i = 0; i < splitOnSpace.Length; i++)
							{
								string[] splitOnComma = splitOnSpace[i].Split(',');
								if (splitOnComma.Length > 1)
								{
									double x = 0.0;
									double y = 0.0;
									double.TryParse(splitOnComma[0], NumberStyles.Number, CultureInfo.InvariantCulture, out x);
									double.TryParse(splitOnComma[1], NumberStyles.Number, CultureInfo.InvariantCulture, out y);

									if (!startedPoly)
									{
										startedPoly = true;
										path = new PathFigure()
										{
											IsClosed = true,
											StartPoint = new Windows.Foundation.Point(x, y)
										};
									}
									else
									{
										path.Segments.Add(new LineSegment() { Point = new Windows.Foundation.Point(x, y) });
									}
								}
							}
							if (path.Segments.Count > 0)
							{
								if (geom == null)
									geom = new PathGeometry();
								geom.Figures.Add(path);
							}
						}
					}
					LineString = reader.ReadLine();
				}
				if (geom != null && geom.Figures.Count > 0)
					yield return new Drawing() { Color = color, Geometry = geom };
			}
		}
		private static Color IntToColor(int colorAsInt)
		{
			return Color.FromArgb((byte)((colorAsInt >> 0x18) & 0xff),
						  (byte)((colorAsInt >> 0x10) & 0xff),
						  (byte)((colorAsInt >> 8) & 0xff),
						  (byte)(colorAsInt & 0xff));
		}
	}
}
