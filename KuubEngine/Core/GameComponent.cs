namespace KuubEngine.Core {
    public abstract class GameComponent {
        protected Game Game { get; set; }

        protected GameComponent(Game game) {
            Game = game;
        }
    }
}