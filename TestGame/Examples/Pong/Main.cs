using KuubEngine.Core;
using KuubEngine.Scene;
using KuubEngine.Scene.Components;

namespace TestGame.Examples.Pong {
    [Example("Pong")]
    public class Main : GameScene {
        private Entity leftPaddle, rightPaddle, ball;

        public Main(Game game) : base(game) {
            Entity.Register("paddle", delegate(Entity ent) {
                ent.AddComponent<PaddleController>();
                ent.AddComponent<Collider2D>();
            });

            leftPaddle = Entity.Create(this, "paddle");
            rightPaddle = Entity.Create(this, "paddle");

            ball = Entity.Create(this, "ball");
        }
    }
}
