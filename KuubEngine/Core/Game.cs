/*
 * KuubEngine
 * Copyright 2012-2014 Kuub Studios (http://kuubstudios.com)
 * 
 * This entire project is licensed under the MIT license.
 * 
 * All other rights reserved.
 * 
 * THIS CODE IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND
 */

using System;
using System.Drawing;

using KuubEngine.Content;
using KuubEngine.Diagnostics;
using KuubEngine.Graphics;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Core {
    /// <summary>
    ///     Base class which should be inherited by your game
    /// </summary>
    public abstract class Game : IDisposable {
        private readonly GameTime lastGameTime;

        private Color4 clearColor;
        /// <summary>
        ///     Color used to clear at the start of every Draw
        /// </summary>
        public Color4 ClearColor {
            get { return clearColor; }
            set {
                clearColor = value;
                GL.ClearColor(value);
            }
        }

        /// <summary>
        ///     OpenTK GameWindow
        /// </summary>
        public GameWindow Window { get; private set; }

        /// <summary>
        ///     ContentManager to load resources that need special processing
        /// </summary>
        public ContentManager Content { get; protected set; }

        /// <summary>
        ///     SpriteBatch for drawing 2D quads efficiently
        /// </summary>
        public SpriteBatch SpriteBatch { get; protected set; }

        /// <summary>
        ///     Create a game with the specified configuration
        /// </summary>
        /// <param name="config"></param>
        protected Game(GameConfiguration config) {
            if(config == null) throw new ArgumentNullException("config");

            config.SaveFile("settings");

            lastGameTime = new GameTime();

#if DEBUG
            config.GraphicsContextFlags = config.GraphicsContextFlags | GraphicsContextFlags.Debug;
#endif

            Window = new GameWindow(config.Width, config.Height, config.GraphicsMode, config.Caption, config.GameWindowFlags, config.DisplayDevice, config.Major, config.Minor, config.GraphicsContextFlags);

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

        /// <summary>
        ///     Create a new game with the default configuration
        /// </summary>
        protected Game() : this(GameConfiguration.Default) {}

        public void Dispose() {
            Window.Dispose();
        }

        private void Load(object sender, EventArgs eventArgs) {
            Window.VSync = VSyncMode.Adaptive;

            GraphicsDevice.Initialize();

            GL.Enable(EnableCap.CullFace);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.CullFace(CullFaceMode.Back);

            Log.Info("Initializing");
            Initialize();

            Content = new ContentManager("resources/");

            SpriteBatch = new SpriteBatch();

            Log.Info("Loading content");
            LoadContent(Content);
        }

        private void Unload(object sender, EventArgs e) {
            Log.Info("Unloading content");
            UnloadContent();

            Content.Unload();
            SpriteBatch.Dispose();
            Texture2D.Blank.Dispose();
        }

        private void Resize(object sender, EventArgs e) {
            GL.Viewport(0, 0, Window.Width, Window.Height);
            SpriteBatch.Resize(Window.Width, Window.Height);

            Resized();
        }

        private void UpdateFrame(object sender, FrameEventArgs e) {
            lastGameTime.Update(e.Time);

            Update(lastGameTime);
        }

        private void RenderFrame(object sender, FrameEventArgs e) {
            lastGameTime.Update(e.Time);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Draw(lastGameTime, 0);

            Window.SwapBuffers();
        }

        /// <summary>
        ///     Called when the game starts
        /// </summary>
        protected virtual void Initialize() {}

        /// <summary>
        ///     Called after the game starts and content should be loaded
        /// </summary>
        protected virtual void LoadContent(ContentManager content) {}

        /// <summary>
        ///     Called before the game closes and content should be disposed
        /// </summary>
        protected virtual void UnloadContent() {}

        /// <summary>
        ///     Called when the game is resized, GL.Viewport is set automatically
        /// </summary>
        protected virtual void Resized() {}

        /// <summary>
        ///     Called at a constant rate of 60 times per second
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        protected virtual void Update(GameTime gameTime) {}

        /// <summary>
        ///     Called once for every frame to draw
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        /// <param name="interpolation">Interpolation value when drawing between updates</param>
        protected virtual void Draw(GameTime gameTime, float interpolation) {}

        /// <summary>
        ///     Start the game
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public void Run(string[] args) {
            Window.Run(60);
        }

        /// <summary>
        ///     Exit the game
        /// </summary>
        public void Exit() {
            Window.Exit();
        }

        /// <summary>
        ///     Takes a screenshot of the game
        /// </summary>
        /// <param name="flip">Flip along Y axis, used for saving as PNG instead of BMP (Very slow!)</param>
        /// <returns>Bitmap of screenshot</returns>
        /// <exception cref="GraphicsContextMissingException"></exception>
        public Bitmap Screenshot(bool flip = false) {
            if(GraphicsContext.CurrentContext == null) throw new GraphicsContextMissingException();

            Bitmap bmp = new Bitmap(Window.ClientSize.Width, Window.ClientSize.Height);
            System.Drawing.Imaging.BitmapData data = bmp.LockBits(Window.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.ReadPixels(0, 0, Window.ClientSize.Width, Window.ClientSize.Height, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            if(flip) bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
        }
    }
}