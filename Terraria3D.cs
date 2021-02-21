using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace Terraria3D
{
    public class Terraria3D : Mod
    {
        public static bool Enabled { get; set; } = true;
		public static bool VrEnabled { get; set; } = true;
		public static bool VrRendering => VrEnabled && Instance.VrHandler.HMDConnected && Instance.VrHandler.VrInitalized;
		public static Terraria3D Instance { get; private set; }
        public Scene3D Scene { get; set; }
		public VrHandler VrHandler { get; set; }
		public LayerManager LayerManager { get; set; }

		public async void Toggle()
		{
			if (Scene.DollyController.DollyInProgress) return;
			if(Enabled)
			{
				await Scene.DollyController.TransitionOutAsync();
				Enabled = false;
			}
			else
			{
				Enabled = true;
				Scene.DollyController.TransitionIn();
			}

		}

		//Main.screenTarget background
		//public override void MidUpdateTimeWorld()
  //      {
		//	Main.graphics.GraphicsDevice.SetRenderTarget(a);

		//	Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
		//	Main.spriteBatch.Draw(Main.screenTarget, new Rectangle(0, 0, a.Width, a.Height), Color.White);
		//	Main.spriteBatch.End();

		//	Instance.DrawScene();

		//	Main.graphics.GraphicsDevice.SetRenderTarget(null);
		//}

  //      RenderTarget2D a = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
		
		public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
			if (VrRendering)
            {
				spriteBatch.Draw(VrHandler.leftEyeTarget, new Rectangle(0, Main.screenHeight / 2, Main.screenWidth / 2, Main.screenHeight / 2), Color.White);
				spriteBatch.Draw(VrHandler.rightEyeTarget, new Rectangle(Main.screenWidth / 2, Main.screenHeight / 2, Main.screenWidth / 2, Main.screenHeight / 2), Color.White);
			}
			base.PostDrawInterface(spriteBatch);
        }

		

		public override void Load()
        {
			//for (int i = 0; i < 3; i++)
			//{
			//	Debug.WriteLine("DEBUG1234");
			//}

			//FieldInfo[] fieldInfos = typeof(RenderTarget2D).GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			//Debug.WriteLine("TERRARIA 3D DEBUG");
			//Debug.WriteLine(fieldInfos.Length);
			//foreach (FieldInfo info in fieldInfos)
			//{
			//	Debug.WriteLine(info.ToString());
			//}

			if (Main.dedServ) return;
            Instance = this;
            Enabled = true;
            Loading.Load(this);
        }
        public override void Unload()
        {
            if (Main.dedServ) return;
            Instance = null;
            Enabled = false;
            Loading.Unload(this);
        }

        public override bool LoadResource(string path, int length, Func<Stream> getStream)
        {
            if (Main.dedServ || (!Renderers.SM3Enabled && path.StartsWith("Effects/HiDef"))) return false;
            return base.LoadResource(path, length, getStream);
        }

        // Drawing
        public void RenderLayersTargets() => Scene.RenderLayers(LayerManager.Layers);
        public void DrawScene() => Scene.DrawToScreen(LayerManager.Layers);

		// UI
		public override void UpdateUI(GameTime gameTime)
		{
			InputTerraria3D.Update(gameTime);
			UITerraria3D.Update(gameTime);
			Scene.Update(gameTime);
		}
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) => UITerraria3D.ModifyInterfaceLayers(layers);
    }

	public class PlayerHooks : ModPlayer
	{
		public override void ProcessTriggers(TriggersSet triggersSet) => InputTerraria3D.ProcessInput();
		public override void SetControls() => InputTerraria3D.SetControls(player);
		public override void OnEnterWorld(Player player)
		{
			// Hack for overhaul to stop black tiles from persisting.
			Settings.Load();
			if (Main.instance.blackTarget != null && !Main.instance.blackTarget.IsDisposed)
			{
				Main.graphics.GraphicsDevice.SetRenderTarget(Main.instance.blackTarget);
				Main.graphics.GraphicsDevice.Clear(Color.Transparent);
				Main.graphics.GraphicsDevice.SetRenderTarget(null);
			}
		}
		public override TagCompound Save()
		{
			Settings.Save();
			return base.Save();
		}
	}
}