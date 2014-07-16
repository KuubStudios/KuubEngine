using System;

using KuubEngine.Content;
using KuubEngine.Content.Assets;
using KuubEngine.Core;
using KuubEngine.Graphics;

using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace TestGame {
    public class Main : Game {
        private GraphicsBuffer buffer, indexBuffer, normalBuffer;
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
                //new Vector3(-0.5f, 0.5f, 0.0f),
                //new Vector3(0.5f, -0.5f, 0.0f),
                new Vector3(-0.5f, -0.5f, 0.0f)
            });

            indexBuffer = new GraphicsBuffer(BufferTarget.ElementArrayBuffer);
            indexBuffer.SetData(new int[] {
                0, 1, 2,
                0, 2, 3
            });

            normalBuffer = new GraphicsBuffer(BufferTarget.ArrayBuffer);
            normalBuffer.SetData(new Vector3[] {
                Vector3.UnitZ,
                Vector3.UnitZ,
                Vector3.UnitZ,
                Vector3.UnitZ
            });

            vertexArray = new VertexArray();
            vertexArray.BindBuffer(buffer, 0);
            vertexArray.BindBuffer(normalBuffer, 1);
            vertexArray.BindBuffer(indexBuffer);
            VertexArray.Unbind();

            texture = new Texture2D("wut.png");

            basicShader = Content.Load<ShaderCollection>("shaders/basic");
        }

        protected override void UnloadContent() {
            spriteBatch.Dispose();

            buffer.Dispose();
            indexBuffer.Dispose();
            normalBuffer.Dispose();
            vertexArray.Dispose();

            texture.Dispose();

        }

        protected override void Update(GameTime gameTime) {
            if(Keyboard.GetState()[Key.Escape]) Exit();
        }

        protected override void Draw(GameTime gameTime, float interpolation) {
            basicShader.Use();
            vertexArray.Bind();
            //GL.DrawArrays(PrimitiveType.Triangles, 0, buffer.Length);
            GL.DrawElements(PrimitiveType.Triangles, indexBuffer.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);

            //using(spriteBatch.Use) spriteBatch.Draw(texture, -0.5f, -0.5f, 1, 1, Color4.Black);
        }
    }
}