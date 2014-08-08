﻿using System;

using KuubEngine.Content.Assets;
using KuubEngine.Diagnostics;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics {
    [Flags]
    public enum SpriteEffects {
        None = 0,
        FlipHorizontally = 1,
        FlipVertically = 2,
        FlipBoth = FlipHorizontally | FlipVertically
    }

    public class SpriteBatch : IDisposable {
        private const int MaxSprites = 8192;
        private readonly Vector2 centerOrigin = new Vector2(0.5f, 0.5f);

        private struct SpriteBatchUse : IDisposable {
            private readonly SpriteBatch spriteBatch;

            public SpriteBatchUse(SpriteBatch sb) {
                spriteBatch = sb;
            }

            public void Dispose() {
                spriteBatch.Flush();
            }
        }

        private static readonly ShaderCollection shaderCollection;
        private static int shaderReferences;
        private static Matrix4 mvp;

        private readonly GraphicsBuffer vertexBuffer, indexBuffer, colorBuffer, texBuffer;
        private readonly VertexArray vertexArray;
        private readonly Vector2[] vertices = new Vector2[MaxSprites * 4];
        private readonly uint[] indices = new uint[MaxSprites * 6];
        private readonly Color4[] colors = new Color4[MaxSprites * 4];
        private readonly Vector2[] texCoords = new Vector2[MaxSprites * 6];

        private readonly uint[] iValues = { 0, 1, 2, 1, 3, 2 };

        private int cacheSize;

        public IDisposable Use { get; private set; }

        static SpriteBatch() {
            shaderCollection = new ShaderCollection();
            shaderCollection.Load("resources/shaders/sprite");
        }

        public static void Resize(int width, int height) {
            mvp = Matrix4.CreateOrthographicOffCenter(0, width, height, 0, -10, 10);
            shaderCollection.Program.SetUniformMatrix4("mvp", mvp);            
        }

        public SpriteBatch() {
            shaderReferences++;

            Use = new SpriteBatchUse(this);

            vertexBuffer = new GraphicsBuffer(BufferTarget.ArrayBuffer, BufferUsageHint.StreamDraw);
            vertexBuffer.SetData(vertices);
            indexBuffer = new GraphicsBuffer(BufferTarget.ElementArrayBuffer, BufferUsageHint.StreamDraw);
            indexBuffer.SetData(indices);

            colorBuffer = new GraphicsBuffer(BufferTarget.ArrayBuffer, BufferUsageHint.StreamDraw);
            colorBuffer.SetData(colors);
            texBuffer = new GraphicsBuffer(BufferTarget.ArrayBuffer, BufferUsageHint.StreamDraw);
            texBuffer.SetData(texCoords);

            vertexArray = new VertexArray();
            vertexArray.BindBuffer(vertexBuffer, shaderCollection.Program, "in_pos");
            vertexArray.BindBuffer(indexBuffer);
            vertexArray.BindBuffer(colorBuffer, shaderCollection.Program, "in_color");
            vertexArray.BindBuffer(texBuffer, shaderCollection.Program, "in_texcoord");          
        }

        public void Dispose() {
            vertexBuffer.Dispose();
            indexBuffer.Dispose();
            colorBuffer.Dispose();
            texBuffer.Dispose();
            vertexArray.Dispose();

            shaderReferences--;
            if(shaderReferences < 1) { // this is broken if you dispose all spritebatches and then make a new one
                shaderCollection.Dispose();
            }
        }

        public void Flush() {
            if(cacheSize == 0) return;

            vertexBuffer.SetData(vertices);
            indexBuffer.SetData(indices);
            vertexArray.BindBuffer(indexBuffer);

            colorBuffer.SetData(colors);
            texBuffer.SetData(texCoords);

            shaderCollection.Use();

            /*
            if(Texture.Current != null) {
                GL.ActiveTexture(TextureUnit.Texture0);
                shaderCollection.Program.SetUniform1("tex", Texture.Current.ID);
            }
            */

            vertexArray.Bind();
            GL.DrawElements(PrimitiveType.Triangles, cacheSize * 6, DrawElementsType.UnsignedInt, IntPtr.Zero);

            Array.Clear(vertices, 0, vertices.Length);
            Array.Clear(indices, 0, indices.Length);
            Array.Clear(colors, 0, colors.Length);

            cacheSize = 0;
        }

        public void Draw(Texture2D texture, float x, float y, float width, float height, Color4 color, Vector2 origin, float rotation, SpriteEffects effects = SpriteEffects.None) {
            if(cacheSize + 1 > MaxSprites) {
                Log.Warn("Spritebatch cache overflowed, forcing flush");
                Flush();
            }

            if(Texture.Current != texture) {
                Flush();
                texture.Bind();
            }

            Vector2 tMin = texture.GetCoords(0, 0);
            Vector2 tMax = texture.GetCoords(texture.Width, texture.Height);

            bool flipH = (effects & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally;
            bool flipV = (effects & SpriteEffects.FlipVertically) == SpriteEffects.FlipVertically;
            float xMax = flipH ? tMin.X : tMax.X;
            float xMin = flipH ? tMax.X : tMin.X;
            float yMax = flipV ? tMax.Y : tMin.Y;
            float yMin = flipV ? tMin.Y : tMax.Y;

            float cos = (float)Math.Cos(rotation);
            float sin = (float)Math.Sin(rotation);

            float originX = x + origin.X;
            float originY = y + origin.Y;

            float[,] verts = {
                { x, y },
                { x + width, y },
                { x, y + height },
                { x + width, y + height }
            };

            for(int i = 0; i < 4; ++i) {
                float oldX = verts[i, 0];
                float oldY = verts[i, 1];
                verts[i, 0] = originX + (cos * (oldX - originX) + sin * (oldY - originY));
                verts[i, 1] = originY + (-sin * (oldX - originX) + cos * (oldY - originY));
            }
            

            uint vOffset = (uint)(cacheSize * 4);

            /*
            vertices[vOffset] = new Vector2(x, y);
            vertices[vOffset + 1] = new Vector2(x + width, y);
            vertices[vOffset + 2] = new Vector2(x, y + height);
            vertices[vOffset + 3] = new Vector2(x + width, y + height);
            */

            vertices[vOffset] = new Vector2(verts[0, 0], verts[0, 1]);
            vertices[vOffset + 1] = new Vector2(verts[1, 0], verts[1, 1]);
            vertices[vOffset + 2] = new Vector2(verts[2, 0], verts[2, 1]);
            vertices[vOffset + 3] = new Vector2(verts[3, 0], verts[3, 1]);

            int iOffset = cacheSize * 6;
            for(int i = 0; i < 6; i++) indices[iOffset + i] = vOffset + iValues[i];

            texCoords[vOffset] = new Vector2(xMin, yMax);
            texCoords[vOffset + 1] = new Vector2(xMax, yMax);
            texCoords[vOffset + 2] = new Vector2(xMin, yMin);
            texCoords[vOffset + 3] = new Vector2(xMax, yMin);

            for(int i = 0; i < 4; i++) colors[vOffset + i] = color;

            cacheSize++;
        }

        public void Draw(Texture2D texture, float x, float y, float width, float height, Color4 color, float rotation, SpriteEffects effects = SpriteEffects.None) {
            Draw(texture, x, y, width, height, color, centerOrigin, rotation, effects);
        } 

        public void Draw(Texture2D texture, float x, float y, float width, float height, Color4 color, SpriteEffects effects = SpriteEffects.None) {
            Draw(texture, x, y, width, height, color, Vector2.Zero, 0, effects);
        }

        public void Draw(Texture2D texture, float x, float y, float width, float height) {
            Draw(texture, x, y, width, height, Color4.White);
        }

        public void Draw(Texture2D texture, float x, float y, Color4 color) {
            Draw(texture, x, y, texture.Width, texture.Height, color);
        }

        public void Draw(Texture2D texture, float x, float y) {
            Draw(texture, x, y, texture.Width, texture.Height, Color4.White);
        }
    }
}