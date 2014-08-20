using KuubEngine.Core;

using OpenTK.Input;

namespace TestGame {
    public class Main : Game {
        public Main() : base(new GameConfiguration("KuubTestGame").LoadFile("settings")) {}

        protected override void Update(GameTime gameTime) {
            if(Keyboard.GetState()[Key.Escape]) Exit();
        }
    }
}