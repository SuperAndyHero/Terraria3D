namespace Terraria3D
{
    public class LayerManager
    {
		public LayerConfig LayerConfig { get; private set; }

        public LayerManager()
        {
            Rebuild();
        }

        public void Rebuild()
        {
			LayerConfig?.Dispose();
			LayerConfig = LayerConfigImporter.GetDefaultConfig();
        }

        public void Dispose()
        {
			LayerConfig?.Dispose();
        }
    }
}