using System;

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

        private static readonly Shader VertexShader, FragShader;
        private static readonly ShaderProgram ShaderProgram;
        private static int ShaderReferences;

        private readonly GraphicsBuffer vertexBuffer, colorBuffer, texBuffer;
        private readonly VertexArray vertexArray;
        private readonly Vector3[] vertices = new Vector3[MaxSprites * 6];
        private readonly Color4[] colors = new Color4[MaxSprites * 6];
        private readonly Vector2[] texCoords = new Vector2[MaxSprites * 6];
        private int cacheSize;

        public IDisposable Use { get; private set; }

        static SpriteBatch() {
            ShaderProgram = new ShaderProgram();

            VertexShader = new Shader(ShaderType.VertexShader, @"
                #version 130

                in vec3 vertex_pos;
                in vec4 vertex_color;

                out vec4 frag_color;

                void main () {
                    gl_Position = vec4(vertex_pos, 1);
                    frag_color = vertex_color;
                }
            ");
            VertexShader.Attach(ShaderProgram);

            FragShader = new Shader(ShaderType.FragmentShader, @"
                #version 130

                in vec4 frag_color;
                out vec4 color_out;

                void main () {
                    color_out = frag_color;
                }
            ");
            FragShader.Attach(ShaderProgram);

            GL.BindAttribLocation(ShaderProgram.ID, 0, "vertex_pos");
            GL.BindAttribLocation(ShaderProgram.ID, 1, "vertex_color");

            ShaderProgram.Link();
        }

        public SpriteBatch() {
            ShaderReferences++;

            Use = new SpriteBatchUse(this);

            vertexBuffer = new GraphicsBuffer(BufferTarget.ArrayBuffer);
            vertexBuffer.SetData(vertices);
            colorBuffer = new GraphicsBuffer(BufferTarget.ArrayBuffer);
            colorBuffer.SetData(colors);
            texBuffer = new GraphicsBuffer(BufferTarget.ArrayBuffer);
            texBuffer.SetData(texCoords);

            vertexArray = new VertexArray();
            vertexArray.BindBuffer(vertexBuffer, 0);
            vertexArray.BindBuffer(colorBuffer, 1);
            vertexArray.BindBuffer(texBuffer, 2);
        }

        public void Dispose() {
            vertexBuffer.Dispose();
            colorBuffer.Dispose();
            texBuffer.Dispose();
            vertexArray.Dispose();

            ShaderReferences--;

            if(ShaderReferences < 1) { // this is broken if you dispose all spritebatches and then make a new one
                VertexShader.Dispose();
                FragShader.Dispose();
                ShaderProgram.Dispose();
            }
        }

        public void Flush() {
            if(cacheSize == 0) return;

            vertexBuffer.SetData(vertices);
            colorBuffer.SetData(colors);
            texBuffer.SetData(texCoords);

            ShaderProgram.Use();
            vertexArray.Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, cacheSize * 6);

            cacheSize = 0;
        }

        public void Draw(Texture2D texture, float x, float y, float width, float height, Color4 color, Vector2 origin, float rotation, SpriteEffects effects = SpriteEffects.None) {
            if(cacheSize + 1 > MaxSprites) {
                Console.WriteLine("Spritebatch cache overflowed, forcing flush");
                Flush();
            }

            if(Texture.Current != texture) {
                Console.WriteLine("Changing textures");
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

                //Console.WriteLine(verts[i, 0]);
                //Console.WriteLine(verts[i, 1]);
                //Console.WriteLine("=====");
            }

            int vOffset = cacheSize * 6;

            vertices[vOffset] = new Vector3(verts[0, 0], verts[0, 1], 0);
            texCoords[vOffset] = new Vector2(xMin, yMin);

            vertices[vOffset + 1] = new Vector3(x + width, y + height, 0);
            texCoords[vOffset + 1] = new Vector2(xMax, yMax);

            vertices[vOffset + 2] = new Vector3(x, y + height, 0);
            texCoords[vOffset + 2] = new Vector2(xMin, yMax);

            vertices[vOffset + 3] = new Vector3(verts[0, 0], verts[0, 1], 0);
            texCoords[vOffset + 3] = new Vector2(xMin, yMin);

            vertices[vOffset + 4] = new Vector3(x + width, y + height, 0);
            texCoords[vOffset + 4] = new Vector2(xMax, yMax);

            vertices[vOffset + 5] = new Vector3(x + width, y, 0);
            texCoords[vOffset + 5] = new Vector2(xMax, yMin);

            for(int i = 0; i < 6; i++) colors[vOffset + i] = color;

            cacheSize++;
        }

        public void Draw(Texture2D texture, float x, float y, float width, float height, Color4 color, SpriteEffects effects = SpriteEffects.None) {
            Draw(texture, x, y, width, height, Color4.White, Vector2.Zero, 0, effects);
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