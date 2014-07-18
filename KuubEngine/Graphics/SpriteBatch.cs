using System;
using System.Drawing;
using System.Drawing.Imaging;

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

        private readonly GraphicsBuffer vertexBuffer, indexBuffer, colorBuffer, texBuffer;
        private readonly VertexArray vertexArray;
        private readonly Vector2[] vertices = new Vector2[MaxSprites * 4];
        private readonly uint[] indices = new uint[MaxSprites * 6];
        private readonly Color4[] colors = new Color4[MaxSprites * 4];
        private readonly Vector2[] texCoords = new Vector2[MaxSprites * 6];

        private int cacheSize;

        public IDisposable Use { get; private set; }

        private Bitmap bitmap = new Bitmap("wut.png");
        private int texture;

        static SpriteBatch() {
            shaderCollection = new ShaderCollection();
            shaderCollection.Load("resources/shaders/sprite");
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

            
            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);
            
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

            GL.DeleteTextures(1, ref texture);
        }

        public void Flush() {
            if(cacheSize == 0) return;

            vertexBuffer.SetData(vertices);
            indexBuffer.SetData(indices);
            vertexArray.BindBuffer(indexBuffer);

            colorBuffer.SetData(colors);
            texBuffer.SetData(texCoords);

            shaderCollection.Use();

            //GL.BindTexture(TextureTarget.Texture2D, texture);

            /*
            if(Texture.Current != null) {
                GL.ActiveTexture(TextureUnit.Texture0);
                shaderCollection.Program.SetUniform1("tex", Texture.Current.ID);
            }
            */
            vertexArray.Bind();
            //GL.DrawArrays(PrimitiveType.Triangles, 0, cacheSize * 6);
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
            float xMin = flipH ? tMax.X : tMin.X;
            float yMin = flipV ? tMax.Y : tMin.Y;
            float xMax = flipH ? tMin.X : tMax.X;
            float yMax = flipV ? tMin.Y : tMax.Y;

            float cos = (float)Math.Cos(rotation);
            float sin = (float)Math.Sin(rotation);

            float originW = width * origin.X;
            float originH = height * origin.Y;

            float[,] verts = {
                { -originW, -originH },
                { originW, -originH },
                { originW, originH },
                { -originW, originH }
            };

            for(int i = 0; i < 4; ++i) {
                float newX = verts[i, 0];
                float newY = verts[i, 1];
                verts[i, 0] = x + cos * newX - sin * newY;
                verts[i, 1] = y + sin * newX + cos * newY;
            }

            uint vOffset = (uint)(cacheSize * 4);

            vertices[vOffset] = new Vector2(x, y);
            vertices[vOffset + 1] = new Vector2(x + width, y);
            vertices[vOffset + 2] = new Vector2(x, y + height);
            vertices[vOffset + 3] = new Vector2(x + width, y + height);

            int iOffset = cacheSize * 6;
            indices[iOffset] = vOffset;
            indices[iOffset + 1] = vOffset + 2;
            indices[iOffset + 2] = vOffset + 1;
            indices[iOffset + 3] = vOffset + 1;
            indices[iOffset + 4] = vOffset + 2;
            indices[iOffset + 5] = vOffset + 3;

            texCoords[vOffset] = new Vector2(xMin, yMax);
            texCoords[vOffset + 1] = new Vector2(xMax, yMax);
            texCoords[vOffset + 2] = new Vector2(xMin, yMin);
            texCoords[vOffset + 3] = new Vector2(xMax, yMin);

            for(int i = 0; i < 4; i++) colors[vOffset + i] = color;

            cacheSize++;
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