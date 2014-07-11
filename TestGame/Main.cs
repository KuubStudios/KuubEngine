using KuubEngine.Content;
using KuubEngine.Content.Assets;
using KuubEngine.Core;
using KuubEngine.Graphics;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace TestGame {
    public class Main : Game {
        private GraphicsBuffer buffer;
        private VertexArray vertexArray;
        private ShaderCollection basicShader;
        private SpriteBatch spriteBatch;
        private Texture2D texture;

        public Main() : base(new GameConfiguration("KuubTestGame").LoadFile("settings")) {}

        protected override void LoadContent(ContentManager content) {
            spriteBatch = new SpriteBatch();

            buffer = new GraphicsBuffer(BufferTarget.ArrayBuffer);
            buffer.SetData(new Vector3[] {
                new Vector3(-0.5f, 0.5f, 0.0f),
                new Vector3(0.5f, 0.5f, 0.0f),
                new Vector3(0.5f, -0.5f, 0.0f),
                new Vector3(-0.5f, 0.5f, 0.0f),
                new Vector3(0.5f, -0.5f, 0.0f),
                new Vector3(-0.5f, -0.5f, 0.0f)
            });

            vertexArray = new VertexArray();
            vertexArray.BindBuffer(buffer, 0);

            texture = new Texture2D("wut.png");

            basicShader = Content.Load<ShaderCollection>("shaders/basic");
        }

        protected override void UnloadContent() {
            spriteBatch.Dispose();

            buffer.Dispose();
            vertexArray.Dispose();

            texture.Dispose();
        }

        protected override void Draw(GameTime gameTime, float interpolation) {
            //basicShader.Program.Use();
            //vertexArray.Bind();
            //GL.DrawArrays(PrimitiveType.Triangles, 0, buffer.Length);

            using(spriteBatch.Use) spriteBatch.Draw(texture, -0.5f, -0.5f, 1, 1, Color4.Black);
        }
    }
}