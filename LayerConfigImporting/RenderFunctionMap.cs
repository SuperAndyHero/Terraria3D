using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Terraria3D
{
	public static class RenderFunctionMap
	{
		public static ReadOnlyDictionary<string, Action> Map { get; } = new ReadOnlyDictionary<string, Action>(new Dictionary<string, Action>()
		{
			{ "Black", () => Rendering.DrawBlack() },
			{ "BackgroundWater", () => Rendering.DrawBackgroundWater() },
			{ "SceneBackground", () => Rendering.DrawSceneBackground() },
			{ "Walls", () => Rendering.DrawWalls() },
			{ "SolidTiles", () => Rendering.DrawSolidTiles() },
			{ "DrawTiles", () => Rendering.PostDrawTiles() },
			{ "NonSolidTiles", () => Rendering.DrawNonSolidTiles() },
			{ "WaterFalls", () => Rendering.DrawWaterFalls() },
			{ "MoonMoon", () => Rendering.DrawMoonMoon() },
			{ "NPCsBehindTiles", () => Rendering.DrawNPCsBehindTiles() },
			{ "DrawCacheWorm", () => Rendering.SortDrawCacheWorm() },
			{ "WallOfFlesh", () => Rendering.DrawWallOfFlesh() },
			{ "NPCsBehindNonSoldTiles", () => Rendering.DrawNPCsBehindNonSoldTiles() },
			{ "NPCsInfrontOfTiles", () => Rendering.DrawNPCsInfrontOfTiles() },
			{ "Players", () => Rendering.DrawPlayers() },
			{ "NPCsOverPlayer", () => Rendering.DrawNPCsOverPlayer() },
			{ "ProjsBehindNPCsAndTiles", () => Rendering.DrawProjsBehindNPCsAndTiles() },
			{ "ProjsBehindNPCs", () => Rendering.DrawProjsBehindNPCs() },
			{ "ProjsBehindProjectiles", () => Rendering.DrawProjsBehindProjectiles() },
			{ "Projectiles", () => Rendering.DrawProjectiles() },
			{ "InfernoRings", () => Rendering.DrawInfernoRings() },
			{ "ProjsOverWireUI", () => Rendering.DrawProjsOverWireUI() },
			{ "NPCProjectiles", () => Rendering.DrawNPCProjectiles() },
			{ "GoreBehind", () => Rendering.DrawGoreBehind() },
			{ "Gore", () => Rendering.DrawGore() },
			{ "Dust", () => Rendering.DrawDust() },
			{ "Rain", () => Rendering.DrawRain() },
			{ "Sandstorm", () => Rendering.DrawSandstorm() },
			{ "MoonLordDeath", () => Rendering.DrawMoonLordDeath() },
			{ "MoonlordDeathFront", () => Rendering.DrawMoonlordDeathFront() },
			{ "Items", () => Rendering.DrawItems() },
			{ "ForegroundWater", () => Rendering.DrawForegroundWater() },
			{ "Wires", () => Rendering.DrawWires() },
			{ "HitTileAnimation", () => Rendering.DrawHitTileAnimation() },
			{ "ItemText", () => Rendering.DrawItemText() },
			{ "CombatText", () => Rendering.DrawCombatText() },
			{ "ChatOverPlayerHeads", () => Rendering.DrawChatOverPlayerHeads() },
			{ "GameInterfaces", () => InterfaceRendering.RenderGameInterfaces() }
		});
	}
}