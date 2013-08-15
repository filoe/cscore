//#define rotation

using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D9;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Visualization3D.Core.Graphics
{
    public class CubeItem : IVisualisationItem
    {
        private const string techniquekey = "technique";
        private const string effectkey = "effect";
        private const string vertexbufferkey = "vertexbuffer";
        private const string vertexdeclkey = "vertexdecl";

        private static readonly VertexElement[] vertexElements = new VertexElement[]
            {
                new VertexElement(0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, 16, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                VertexElement.VertexDeclarationEnd
            };

        private static bool isprepared = false;
        private static ResourceManager resourceManager;

        protected static ResourceManager ResourceManager
        {
            get { return resourceManager; }
        }

        public Context Context { get; set; }

        public float Value { get; set; }

        private Matrix _world = Matrix.Identity;

        public void BeginItemRendering()
        {
            var effect = ResourceManager.GetResource<Effect>(effectkey);
            effect.Technique = ResourceManager.GetResource<EffectHandle>(techniquekey);
            effect.Begin();

            var vertexBuffer = ResourceManager.GetResource<VertexBuffer>(vertexbufferkey);

            Context.Device.SetStreamSource(0, vertexBuffer, 0, Utilities.SizeOf<Vector4>() * 2);
            Context.Device.VertexDeclaration = ResourceManager.GetResource<VertexDeclaration>(vertexdeclkey);

            isprepared = true;
        }

        public void EndItemRendering()
        {
            ResourceManager.GetResource<Effect>(effectkey).End();
            isprepared = false;
        }

        public void Load()
        {
            if (ResourceManager == null)
                resourceManager = new ResourceManager();

            if (ResourceManager.GetResource(effectkey) == null)
            {
                var effect = Effect.FromFile(Context.Device, "Cube.fx", ShaderFlags.None);
                effect.SetValue("heightFactor", 25f);
                effect.SetValue("baseHeight", 1f);

                /*var texture = Texture.FromFile(Context.Device, "default_color.dds");

                var effect = Effect.FromFile(Context.Device, "BumpFX.fx", ShaderFlags.None);
                var texhandle = effect.GetParameterBySemantic(null, "DIFFUSE");
                effect.SetTexture(texhandle, texture);
                */

                ResourceManager.AddResource(techniquekey, effect.GetTechnique(0));
                ResourceManager.AddResource(effectkey, effect);
            }

            if (ResourceManager.GetResource(vertexbufferkey) == null)
            {
                var vertices = new VertexBuffer(Context.Device, Utilities.SizeOf<Vector4>() * 2 * 36, Usage.WriteOnly, VertexFormat.None, Pool.Managed);
                var ptr = vertices.Lock(0, 0, LockFlags.None);
                ptr.WriteRange(CreateVertices());
                vertices.Unlock();

                ResourceManager.AddResource(vertexbufferkey, vertices);
                ResourceManager.AddResource(vertexdeclkey, new VertexDeclaration(Context.Device, vertexElements));
            }
        }

        public void Update(float time)
        {
#if rotation
            _rotation += time * 2;
            _rotation %= 360;

            _world = Matrix.RotationY(_rotation);
#endif
        }

        public void Render(Matrix worldViewProj, float time)
        {
            if (!isprepared)
                throw new InvalidOperationException("not rendering");

            var effect = ResourceManager.GetResource<Effect>(effectkey);
            effect.BeginPass(0);

            effect.SetValue("worldViewProj", _world * worldViewProj);
            effect.SetValue("value", Value);
            Context.Device.DrawPrimitives(PrimitiveType.TriangleList, 0, 12);

            effect.EndPass();
        }

        public void Dispose()
        {
            ResourceManager.FreeAll();
        }

        private static Vector4[] CreateVertices()
        {
            Vector4 color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            Vector4 colorfront = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
            Vector4 colorback = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
            Vector4 colortop = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
            Vector4 colorbottom = new Vector4(1.0f, 1.0f, 0.0f, 1.0f);
            Vector4 colorleft = new Vector4(1.0f, 0.0f, 1.0f, 1.0f);
            Vector4 colorright = new Vector4(0.0f, 1.0f, 1.0f, 1.0f);

            return new Vector4[]
            {
                new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), colorfront, // Front
                new Vector4(-1.0f,  1.0f, -1.0f, 1.0f), colorfront,
                new Vector4( 1.0f,  1.0f, -1.0f, 1.0f), colorfront,
                new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), colorfront,
                new Vector4( 1.0f,  1.0f, -1.0f, 1.0f), colorfront,
                new Vector4( 1.0f, -1.0f, -1.0f, 1.0f), colorfront,

                new Vector4(-1.0f, -1.0f,  1.0f, 1.0f), colorback, // BACK
                new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), colorback,
                new Vector4(-1.0f,  1.0f,  1.0f, 1.0f), colorback,
                new Vector4(-1.0f, -1.0f,  1.0f, 1.0f), colorback,
                new Vector4( 1.0f, -1.0f,  1.0f, 1.0f), colorback,
                new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), colorback,

                new Vector4(-1.0f, 1.0f, -1.0f,  1.0f), colortop, // Top
                new Vector4(-1.0f, 1.0f,  1.0f,  1.0f), colortop,
                new Vector4( 1.0f, 1.0f,  1.0f,  1.0f), colortop,
                new Vector4(-1.0f, 1.0f, -1.0f,  1.0f), colortop,
                new Vector4( 1.0f, 1.0f,  1.0f,  1.0f), colortop,
                new Vector4( 1.0f, 1.0f, -1.0f,  1.0f), colortop,

                new Vector4(-1.0f,-1.0f, -1.0f,  1.0f), colorbottom, // Bottom
                new Vector4( 1.0f,-1.0f,  1.0f,  1.0f), colorbottom,
                new Vector4(-1.0f,-1.0f,  1.0f,  1.0f), colorbottom,
                new Vector4(-1.0f,-1.0f, -1.0f,  1.0f), colorbottom,
                new Vector4( 1.0f,-1.0f, -1.0f,  1.0f), colorbottom,
                new Vector4( 1.0f,-1.0f,  1.0f,  1.0f), colorbottom,

                new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), colorleft, // Left
                new Vector4(-1.0f, -1.0f,  1.0f, 1.0f), colorleft,
                new Vector4(-1.0f,  1.0f,  1.0f, 1.0f), colorleft,
                new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), colorleft,
                new Vector4(-1.0f,  1.0f,  1.0f, 1.0f), colorleft,
                new Vector4(-1.0f,  1.0f, -1.0f, 1.0f), colorleft,

                new Vector4( 1.0f, -1.0f, -1.0f, 1.0f), colorright, // Right
                new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), colorright,
                new Vector4( 1.0f, -1.0f,  1.0f, 1.0f), colorright,
                new Vector4( 1.0f, -1.0f, -1.0f, 1.0f), colorright,
                new Vector4( 1.0f,  1.0f, -1.0f, 1.0f), colorright,
                new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), colorright
            };
        }
    }
}