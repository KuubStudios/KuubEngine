// <copyright file="Main.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using KuubEngine.Core;
using KuubEngine.Graphics;
using KuubEngine.Graphics.Structures;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace TestGame
{
    /// <summary>
    /// Game entry point
    /// </summary>
    public class Main : Game
    {
        private ShaderProgram program;
        private VertexArray vao;
        private Texture2D texture;

        /// <inheritdoc/>
        protected override void Initialize()
        {
            Shader vertShader = new Shader(ShaderType.VertexShader);
            using (StreamReader reader = new StreamReader(Assembly.GetCallingAssembly().GetManifestResourceStream("KuubEngine.resources.shaders.test.vert")))
            {
                vertShader.Load(reader.ReadToEnd());
            }

            Shader fragShader = new Shader(ShaderType.FragmentShader);
            using (StreamReader reader = new StreamReader(Assembly.GetCallingAssembly().GetManifestResourceStream("KuubEngine.resources.shaders.test.frag")))
            {
                fragShader.Load(reader.ReadToEnd());
            }

            program = new ShaderProgram();
            vertShader.Attach(program);
            fragShader.Attach(program);
            program.Link();

            texture = new Texture2D("resources/textures/choa.jpg");
            Vector2 actual = texture.GetCoords(texture.Width, texture.Height);

            VertexPositionColorTexture[] vertices =
            {
                new VertexPositionColorTexture(new Vector3(0.5f,  0.5f, 0.0f), new Color4(1.0f, 0.0f, 0.0f, 1.0f), new Vector2(1.0f * actual.X, 0.0f)), // Top Right
                new VertexPositionColorTexture(new Vector3(0.5f, -0.5f, 0.0f), new Color4(1.0f, 1.0f, 0.0f, 1.0f), new Vector2(1.0f * actual.X, actual.Y)), // Bottom Right
                new VertexPositionColorTexture(new Vector3(-0.5f, -0.5f, 0.0f), new Color4(0.0f, 1.0f, 0.0f, 1.0f), new Vector2(0.0f, actual.Y)), // Bottom Left
                new VertexPositionColorTexture(new Vector3(-0.5f,  0.5f, 0.0f), new Color4(0.0f, 0.0f, 1.0f, 1.0f), new Vector2(0.0f, 0.0f)) // Top Left
            };

            /*
            VertexPositionTexture[] vertices =
            {
                new VertexPositionTexture(new Vector3(0.5f,  0.5f, 0.0f), new Vector2(1.0f, 1.0f)), // Top Right
                new VertexPositionTexture(new Vector3(0.5f, -0.5f, 0.0f), new Vector2(1.0f, 0.0f)), // Bottom Right
                new VertexPositionTexture(new Vector3(-0.5f, -0.5f, 0.0f), new Vector2(0.0f, 0.0f)), // Bottom Left
                new VertexPositionTexture(new Vector3(-0.5f,  0.5f, 0.0f), new Vector2(0.0f, 1.0f)) // Top Left
            };
            */

            int[] indices =
            {
                0, 1, 3, // First Triangle
                1, 2, 3 // Second Triangle
            };

            vao = new VertexArray();

            GraphicsBuffer vbo = new GraphicsBuffer(BufferTarget.ArrayBuffer);
            vbo.SetData(vertices);
            /*
            vao.BindBuffer(vbo, 0, 3, VertexAttribPointerType.Float, false, 6 * Marshal.SizeOf<float>(), 0);
            vao.BindBuffer(vbo, 1, 3, VertexAttribPointerType.Float, false, 6 * Marshal.SizeOf<float>(), 3 * Marshal.SizeOf<float>());
            */
            vao.BindBuffer(vbo, VertexPositionColorTexture.VertexDeclaration);

            GraphicsBuffer ebo = new GraphicsBuffer(BufferTarget.ElementArrayBuffer);
            ebo.SetData(indices);
            vao.BindBuffer(ebo);

            vao.Unbind();

            // GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
        }

        /// <inheritdoc/>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState()[Key.Escape])
            {
                Exit();
            }
        }

        /// <inheritdoc/>
        protected override void Draw(GameTime gameTime, float interpolation)
        {
            program.Bind();
            vao.Bind();
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            vao.Unbind();
        }
    }
}