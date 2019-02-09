using System.Collections.Generic;
using System.Reflection;
using Terraria.ID;

namespace Terraria3D
{
	public class TileOverrides
	{
		public Layer3D SolidTopTilesLayer { get; set; }

		public Dictionary<ushort, Layer3D> Overrides { get; set; } = new Dictionary<ushort, Layer3D>();

		public void Add(string tileName, Layer3D layer)
		{
			var fieldInfo = typeof(TileID).GetField(tileName, BindingFlags.Public | BindingFlags.Static);
			if (fieldInfo != null && fieldInfo.FieldType == typeof(ushort))
			{
				var id = (ushort)fieldInfo.GetValue(null);
				Add(id, layer);
			}
			// TODO: check for mod tiles?
		}

		public void Add(ushort id, Layer3D layer)
		{
			if (!Overrides.ContainsKey(id))
				Overrides.Add(id, layer);
		}
	}
}
