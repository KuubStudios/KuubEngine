﻿using System.Collections.Generic;

using KuubEngine.Core;

namespace KuubEngine.Scene {
    public abstract class GameScene {
        protected Game Game { get; set; }

        public List<Entity> GameObjects { get; private set; }

        protected GameScene(Game game) {
            Game = game;
            GameObjects = new List<Entity>();
        }

        public virtual void Start() {}
        public virtual void End() {}
        public virtual void Pause() {}
        public virtual void Resume() {}

        public virtual void Update(GameTime gameTime) {}
        public virtual void Draw(GameTime gameTime, float interpolation) {}

        public void AddGameObject(Entity gameObject) {
            if(!GameObjects.Contains(gameObject)) GameObjects.Add(gameObject);
        }

        public void RemoveGameObject(Entity gameObject) {
            if(GameObjects.Contains(gameObject)) GameObjects.Remove(gameObject);
        }
    }
}