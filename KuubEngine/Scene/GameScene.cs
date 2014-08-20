using System.Collections.Generic;

using KuubEngine.Core;

namespace KuubEngine.Scene {
    public abstract class GameScene {
        protected Game Game { get; set; }

        public List<Entity> Entities { get; private set; }

        protected GameScene(Game game) {
            Game = game;
            Entities = new List<Entity>();
        }

        public virtual void Start() {}
        public virtual void End() {}
        public virtual void Pause() {}
        public virtual void Resume() {}

        public virtual void Update(GameTime gameTime) {}
        public virtual void Draw(GameTime gameTime, float interpolation) {}

        public void Add(Entity ent) {
            if(!Entities.Contains(ent)) Entities.Add(ent);
        }

        public void Remove(Entity ent) {
            if(Entities.Contains(ent)) Entities.Remove(ent);
        }
    }
}