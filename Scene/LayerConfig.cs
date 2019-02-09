namespace Terraria3D
{
	public class LayerConfig
	{
		public string Name { get; set; } = "Un-named";
		public string Author { get; set; } = "Unknown";
		public TileOverrides TileOverrides { get; set; } = new TileOverrides();

		public Layer3D[] Layers { get; set; }

		public void Dispose()
		{
			if (Layers != null)
			{
				foreach (var layer in Layers)
					layer.Dispose();
			}
		}
	}
}
