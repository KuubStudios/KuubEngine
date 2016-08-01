// <copyright file="Game.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Diagnostics;
using KuubEngine.Graphics;
using NLog;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Core
{
    /// <summary>
    /// Base class which should be inherited by your game
    /// </summary>
    public class Game : IDisposable
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        private readonly GameTime gameTime = new GameTime();
        private Stopwatch gameTimer;

        private Color4 clearColor;

        /// <summary>
        /// Gets or sets the <see cref="Color4"/> used to clear at the start of every Draw
        /// </summary>
        public Color4 ClearColor
        {
            get
            {
                return clearColor;
            }

            set
            {
                clearColor = value;
                GL.ClearColor(value);
            }
        }

        /// <summary>
        /// Gets the OpenTK <see cref="GameWindow"/>
        /// </summary>
        public GameWindow Window { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        protected Game()
        {
            GraphicsContextFlags flags = GraphicsContextFlags.ForwardCompatible;
#if DEBUG
            flags |= GraphicsContextFlags.Debug;
#endif

            Window = new GameWindow(1600, 900, GraphicsMode.Default, "KuubGame", GameWindowFlags.FixedWindow, DisplayDevice.Default, 3, 3, flags);

            Window.Load += Load;
            Window.Unload += Unload;
            Window.Resize += Resize;
            Window.UpdateFrame += UpdateFrame;
            Window.RenderFrame += RenderFrame;

#if DEBUG
            Window.Context.ErrorChecking = true;
#endif

            ClearColor = Color4.MidnightBlue;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Window.Dispose();
        }

        private void Load(object sender, EventArgs eventArgs)
        {
            Window.VSync = VSyncMode.Adaptive;

            GraphicsDevice.Initialize();

            /*
            GL.Enable(EnableCap.CullFace);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.CullFace(CullFaceMode.Back);
            */

            log.Info("Initializing");
            Initialize();

            log.Info("Loading content");
            LoadContent();
        }

        private void Unload(object sender, EventArgs e)
        {
            log.Info("Unloading content");
            UnloadContent();
        }

        private void Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, Window.Width, Window.Height);

            Resized();
        }

        private void UpdateFrame(object sender, FrameEventArgs e)
        {
            gameTime.Total = gameTimer.Elapsed;
            gameTime.Elapsed = TimeSpan.FromSeconds(e.Time);
            Update(gameTime);
        }

        private void RenderFrame(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            gameTime.Total = gameTimer.Elapsed;
            gameTime.Elapsed = TimeSpan.FromSeconds(e.Time);
            Draw(gameTime, 0);

            Window.SwapBuffers();
        }

        /// <summary>
        /// Called when the game starts
        /// </summary>
        protected virtual void Initialize()
        {
        }

        /// <summary>
        /// Called after the game starts and content should be loaded
        /// </summary>
        protected virtual void LoadContent()
        {
        }

        /// <summary>
        /// Called before the game closes and content should be disposed
        /// </summary>
        protected virtual void UnloadContent()
        {
        }

        /// <summary>
        /// Called when the game is resized
        /// Viewport is updated automatically
        /// </summary>
        protected virtual void Resized()
        {
        }

        /// <summary>
        /// Called at a constant rate of 60 times per second
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        protected virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Called once for every frame to draw
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        /// <param name="interpolation">Interpolation value when drawing between updates</param>
        protected virtual void Draw(GameTime gameTime, float interpolation)
        {
        }

        /// <summary>
        /// Start the game
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public void Run(string[] args)
        {
            gameTimer = Stopwatch.StartNew();
            gameTime.Elapsed = TimeSpan.FromMilliseconds(1000 / 60);
            Window.Run(60);
        }

        /// <summary>
        /// Exit the game
        /// </summary>
        public void Exit()
        {
            Window.Exit();
        }
    }
}