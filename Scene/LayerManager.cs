namespace Terraria3D
{
    public class LayerManager
    {
        public Layer3D[] Layers => _layers;
        private Layer3D[] _layers;

        public LayerManager()
        {
            Rebuild();
        }

        public void Rebuild()
        {
			if (_layers != null)
			{
				foreach (var layer in _layers)
					layer?.Dispose();
			}
			_layers = LayerConfigImporter.GetDefaultLayers();
            //LayerBuilder.PopulateLayers(ref _layers);
        }

        public void Dispose()
        {
            foreach (var layer in _layers)
                layer?.Dispose();
        }
    }
}