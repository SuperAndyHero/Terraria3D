using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraria3D
{
	public class JsonLayerConfig
	{
		public string Name { get; set; }
		public string Author { get; set; }
		public JsonLayer[] Layers { get; set; }
	}

	public class JsonLayer
	{
		public string Name { get; set; } = "Un-named";
		public float Depth { get; set; } = 16;
		public float ZPos { get; set; } = 0;
		public float Noise { get; set; } = 1;
		public bool InnerPixel { get; set; } = true;
		public Layer3D.InputPlaneType InputPlane { get; set; } = Layer3D.InputPlaneType.None;

		public string[] RenderFunctions { get; set; }
		public string[] TileOverrides { get; set; }
	}
}
