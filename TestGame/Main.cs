using KuubEngine.Content;
using KuubEngine.Core;
using KuubEngine.Graphics;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace TestGame {
    public class Main : Game {
        private GraphicsBuffer buffer;
        private VertexArray vertexArray;
        private Shader vertexShader, fragShader;
        private ShaderProgram testShaderProgram;
        private SpriteBatch spriteBatch;
        private Texture2D texture;

        public Main() : base(new GameConfiguration(1280, 720, "KuubTestGame")) {}
            
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

            testShaderProgram = new ShaderProgram();

            vertexShader = new Shader(ShaderType.VertexShader, @"
                #version 130

                in vec3 pos;
                out vec4 posColor;

                void main () {
                    gl_Position = vec4(pos, 1);

                    posColor = clamp(gl_Position, 0, 1);
                }
            ");
            vertexShader.Attach(testShaderProgram);

            fragShader = new Shader(ShaderType.FragmentShader, @"
                #version 130

                in vec4 posColor;
                out vec4 fragColor;

                void main () {
                    fragColor = posColor;
                }
            ");
            fragShader.Attach(testShaderProgram);

            testShaderProgram.Link();

            texture = new Texture2D("wut.png");
        }

        protected override void UnloadContent() {
            spriteBatch.Dispose();

            buffer.Dispose();
            vertexArray.Dispose();

            vertexShader.Dispose();
            fragShader.Dispose();
            testShaderProgram.Dispose();

            texture.Dispose();
        }

        protected override void Draw(GameTime gameTime, float interpolation) {
            //testShaderProgram.Use();
            //vertexArray.Bind();
            //GL.DrawArrays(PrimitiveType.Triangles, 0, buffer.Length);

            using(spriteBatch.Use) {
                spriteBatch.Draw(texture, -0.5f, -0.5f, 1, 1, Color4.Red);
            }
        }
    }
}