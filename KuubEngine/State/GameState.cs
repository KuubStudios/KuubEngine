using KuubEngine.Core;

namespace KuubEngine.State {
    public abstract class GameState : GameComponent {
        protected GameState(Game game) : base(game) {}

        public virtual void Start() {}
        public virtual void End() {}
        public virtual void Pause() {}
        public virtual void Resume() {}

        public virtual void Update(GameTime gameTime) {}
        public virtual void Draw(GameTime gameTime, float interpolation) {}
    }
}