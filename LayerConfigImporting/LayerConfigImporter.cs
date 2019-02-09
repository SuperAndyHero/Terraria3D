using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Terraria3D
{
	public static class LayerConfigImporter
	{
		private static string defaultLayersJson = @"
[
	{
		'name': 'Background',
		'zPos': 4,
		'depth': 4,
		'renderFunctions':
		[
			'Black',
			'BackgroundWater',
			'SceneBackground',
			'Walls'
		]
	},
	{
		'name': 'Solid Tiles',
		'depth': 32,
		'inputPlane': 'SolidTiles',
		'renderFunctions':
		[
			'SolidTiles',
			'DrawTiles'
		]
},
	{
		'name': 'Non Solid Tiles',
		'depth': 8,
		'inputPlane': 'NonSolidTiles',
		'renderFunctions':
		[
			'NonSolidTiles',
			'WaterFalls'
		]
	},
	{
		'name': 'Characters',
		'zPos': -18,
		'depth': 6,
		'noise': 0,
		'renderFunctions':
		[
			'MoonMoon',
			'NPCsBehindTiles',
			'DrawCacheWorm',
			'WallOfFlesh',
			'NPCsBehindNonSoldTiles',
			'NPCsInfrontOfTiles',
			'Players',
			'NPCsOverPlayer'
		]
	},
	{
		'name': 'Projectiles',
		'zPos': -20,
		'depth': 2,
		'noise': 0,
		'renderFunctions':
		[
			'ProjsBehindNPCsAndTiles',
			'ProjsBehindNPCs',
			'ProjsBehindProjectiles',
			'Projectiles',
			'InfernoRings',
			'ProjsOverWireUI',
			'NPCProjectiles'
		]
	},
	{
		'name': 'Gore - Weather - Items',
		'zPos': -12,
		'depth': 6,
		'noise': 0,
		'renderFunctions':
		[
			'GoreBehind',
			'Gore',
			'Dust',
			'Rain',
			'Sandstorm',
			'MoonLordDeath',
			'MoonlordDeathFront',
			'Items'
		]
	},
	{
		'name': 'Water Foreground',
		'depth': 32,
		'renderFunctions':
		[
			'ForegroundWater'
		]
	},
	{
		'name': 'Wires - UI',
		'zPos': -32,
		'depth': 4,
		'renderFunctions':
		[
			'Wires',
			'HitTileAnimation',
			'ItemText',
			'CombatText',
			'ChatOverPlayerHeads',
			'GameInterfaces'
		]
	}
]
";
		
		public static Layer3D[] GetDefaultLayers()
		{
			try
			{
				var jsonLayers = GetJsonLayers(defaultLayersJson);
				var result = new Layer3D[jsonLayers.Length];
				for (int i = 0; i < jsonLayers.Length; i++)
					result[i] = JsonLayerToLayer3D(jsonLayers[i]);
				return result;
			}
			catch(JsonSerializationException exception)
			{
				Console.WriteLine(exception.Message);
			}
			return null;
		}

		public static JsonLayer[] GetJsonLayers(string json)
			=> JsonConvert.DeserializeObject<JsonLayer[]>(json);
		
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

	public class JsonLayer
	{
		public string Name { get; set; } = "Un-named";
		public float Depth { get; set; } = 0;
		public float ZPos { get; set; } = 16;
		public float Noise { get; set; } = 1;
		public bool InnerPixel { get; set; } = true;
		public Layer3D.InputPlaneType InputPlane { get; set; } = Layer3D.InputPlaneType.None;

		public string[] RenderFunctions { get; set; }
		public string[] TileOverrides { get; set; }
	}
}