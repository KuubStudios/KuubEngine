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
        public Color4 ClearColor {
            get { return clearColor; }
            set {
                clearColor = value;
                GL.ClearColor(value);
            }
        }

        public GameWindow Window { get; private set; }
        public ContentManager Content { get; protected set; }

        protected Game(GameConfiguration config) {
            if(config == null) throw new ArgumentNullException("config");

            lastGameTime = new GameTime();

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

        protected Game() : this(GameConfiguration.Default) {}

        public void Dispose() {
            Window.Dispose();
        }

        private void Load(object sender, EventArgs eventArgs) {
            Window.VSync = VSyncMode.Adaptive;

            GraphicsDevice.Initialize();

            Log.Info("Initializing");
            Initialize();

            Content = new ContentManager();

            Log.Info("Loading content");
            LoadContent(Content);
        }

        private void Unload(object sender, EventArgs e) {
            Log.Info("Unloading content");
            UnloadContent();

            Texture2D.Blank.Dispose();
        }

        private void Resize(object sender, EventArgs eventArgs) {
            GL.Viewport(0, 0, Window.Width, Window.Height);
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
        ///     Takes a screenshot of the game
        /// </summary>
        /// <param name="flip">Flip along Y axis, used for saving as PNG instead of BMP (Very slow!)</param>
        /// <returns>Bitmap of screenshot</returns>
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