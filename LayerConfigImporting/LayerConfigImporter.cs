using System;
using System.Linq;
using Newtonsoft.Json;

namespace Terraria3D
{
	public static class LayerConfigImporter
	{

		public static LayerConfig GetDefaultConfig()
		{
			var json = FileUtils.GetTextFile("IncludedLayerConfigs/Default.json");
			return ImportLayerConfig(json);
		}

		public static LayerConfig ImportLayerConfig(string json)
		{
			try
			{
				var config = GetJsonLayerConfig(json);
				var result = new LayerConfig()
				{
					Name = config.Name,
					Author = config.Author,
					Layers = GetLayersFromJsonLayers(config.Layers)
				};
				return result;
			}
			catch (JsonSerializationException exception)
			{
				throw exception;
				//Console.WriteLine(exception.Message);
			}
		}

		public static Layer3D[] GetLayersFromJsonLayers(JsonLayer[] jsonLayers)
		{
			var result = new Layer3D[jsonLayers.Length];
			for (int i = 0; i < jsonLayers.Length; i++)
				result[i] = JsonLayerToLayer3D(jsonLayers[i]);
			return result;
		}

		public static JsonLayerConfig GetJsonLayerConfig(string json)
			=> JsonConvert.DeserializeObject<JsonLayerConfig>(json);

		private static Layer3D JsonLayerToLayer3D(JsonLayer jl)
		{
			var result = new Layer3D()
			{
				Name = jl.Name,
				Depth = jl.Depth,
				ZPos = jl.ZPos,
				NoiseAmount = jl.Noise,
				UseInnerPixel = jl.InnerPixel,
				InputPlane = jl.InputPlane
			};
			var renderFunctions = GetRenderFunctions(jl.RenderFunctions);
			result.RenderFunction = () =>
			{
				foreach (var func in renderFunctions)
					func();
			};
			return result;
		}

		private static Action[] GetRenderFunctions(string[] functionNames)
			=> functionNames.Select(fn => GetRenderFunction(fn)).ToArray();

		private static Action GetRenderFunction(string functionName)
		{
			if (!RenderFunctionMap.Map.ContainsKey(functionName))
				throw new Exception(string.Format("Could not find render function '{0}'", functionName));
			return RenderFunctionMap.Map[functionName];
		}
	}
}