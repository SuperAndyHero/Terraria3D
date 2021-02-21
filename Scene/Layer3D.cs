﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace Terraria3D
{
    public class Layer3D
    {
        public string Name { get; set; } = "Un-named";
        public float ZPos { get; set; } = 0;
        public float Depth { get; set; } = 16;
        public float NoiseAmount { get; set; } = 1;
        public bool Enabled { get; set; } = true;
        public bool UseInnerPixel { get; set; } = true;
        public Action RenderFunction { get; set; } = null;
        public InputPlaneType InputPlane { get; set; } = InputPlaneType.None;

        public int TargetWidth => _renderTarget.Width;
        public int TargetHeght => _renderTarget.Height;

        private RenderTarget2D _renderTarget;
        private RenderTarget2D _innerPixelTarget;

        public Layer3D()
        {
            UpdateRenderTarget();
            RTManager.ResolutionChanged += (w, h, rtW, rtH) => UpdateRenderTarget();
        }

        public void RenderToTarget()
        {
            Main.graphics.GraphicsDevice.SetRenderTarget(_renderTarget);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
            RenderFunction();
            Main.spriteBatch.End();
            if(UseInnerPixel)
                Renderers.InnerPixelRenderer.Draw(_innerPixelTarget, _renderTarget);
            Main.graphics.GraphicsDevice.SetRenderTarget(null);
        }

        public void Dispose()
        {
            _renderTarget?.Dispose();
            _innerPixelTarget?.Dispose();
        }

        public void DrawExtrusion(Matrix view, Matrix projection, bool ao, Matrix worldMatrix)
        {
            if (!Enabled) return;
            worldMatrix = Matrix.CreateScale(1, 1, Depth) * Matrix.CreateTranslation(0, 0, Depth - ZPos) * worldMatrix;
            Renderers.GridRenderer.Draw(UseInnerPixel ? _innerPixelTarget : _renderTarget, view, projection, Depth, NoiseAmount, ao, worldMatrix);
        }
        public void DrawCap(Matrix view, Matrix projection, Matrix worldMatrix)
        {
            if (!Enabled) return;
            worldMatrix = Matrix.CreateTranslation(0, 0, Depth - ZPos) * worldMatrix;
            Renderers.CapRenderer.Draw(_renderTarget, view, projection, worldMatrix);
        }

        public void DrawExtrusion(Camera camera, bool ao, Matrix worldMatrix)
        {
            if (!Enabled) return;
            worldMatrix = Matrix.CreateScale(1, 1, Depth) * Matrix.CreateTranslation(0, 0, Depth - ZPos) * worldMatrix;
            Renderers.GridRenderer.Draw(UseInnerPixel ? _innerPixelTarget : _renderTarget, camera, Depth, NoiseAmount, ao, worldMatrix);
        }
        public void DrawCap(Camera camera, Matrix worldMatrix)
        {
            if (!Enabled) return;
            worldMatrix = Matrix.CreateTranslation(0, 0, Depth - ZPos) * worldMatrix;
            Renderers.CapRenderer.Draw(_renderTarget, camera, worldMatrix);
        }

        private void UpdateRenderTarget()
        {
            Dispose();
            _renderTarget = Utils.CreateRenderTarget(RTManager.UsingPoT);
            _innerPixelTarget = Utils.CreateRenderTarget(RTManager.UsingPoT);
        }

        public enum InputPlaneType
        {
            None,
            SolidTiles,
            NoneSolidTiles,
        }
    }
}
