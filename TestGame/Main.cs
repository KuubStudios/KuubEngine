using KuubEngine.Content;
using KuubEngine.Core;
using KuubEngine.Graphics;

using OpenTK.Graphics;
using OpenTK.Input;

namespace TestGame {
    public class Main : Game {
        private Texture2D texture;

        public Main() : base(new GameConfiguration("KuubTestGame").LoadFile("settings")) {}

        protected override void LoadContent(ContentManager content) {
            texture = new Texture2D("wut.png");
        }

        protected override void UnloadContent() {
            texture.Dispose();
        }

        protected override void Update(GameTime gameTime) {
            if(Keyboard.GetState()[Key.Escape]) Exit();
        }

        protected override void Draw(GameTime gameTime, float interpolation) {
            using(SpriteBatch.Use) {
                SpriteBatch.Draw(texture, 256, 256, Color4.White);
            }
        }
    }
}