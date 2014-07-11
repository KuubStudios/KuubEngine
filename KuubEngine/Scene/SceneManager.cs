using System;
using System.Collections.Generic;

using KuubEngine.Core;

namespace KuubEngine.Scene {
    public class SceneManager : IDisposable {
        public GameScene ActiveGameState { get; private set; }
        public List<GameScene> GameStates { get; private set; }

        public SceneManager() {
            GameStates = new List<GameScene>();
        }

        public void Dispose() {
            while(ActiveGameState != null) Pop();
        }

        public GameScene Push(GameScene gameState) {
            if(gameState == null) throw new ArgumentNullException("gameState");
            if(ActiveGameState != null) ActiveGameState.Pause();

            GameStates.Add(gameState);
            ActiveGameState = gameState;
            gameState.Start();

            return gameState;
        }

        public GameScene Pop() {
            if(ActiveGameState == null) throw new InvalidOperationException("There are no GameStates on the stack to pop");

            ActiveGameState.End();
            GameStates.Remove(ActiveGameState);

            if(GameStates.Count > 0) {
                ActiveGameState = GameStates[GameStates.Count - 1];
                ActiveGameState.Resume();
            } else ActiveGameState = null;

            return ActiveGameState;
        }

        public void Switch(GameScene gameState) {
            if(gameState == null) throw new ArgumentNullException("gameState");

            if(ActiveGameState != null) ActiveGameState.Pause();
            if(!GameStates.Remove(gameState)) throw new ArgumentException(gameState + " is not on the stack");

            GameStates.Add(gameState);

            gameState.Resume();
            ActiveGameState = gameState;
        }

        public void Update(GameTime gameTime) {
            for(int i = 0; i < GameStates.Count; i++) GameStates[i].Update(gameTime);
        }

        public void Draw(GameTime gameTime, float interpolation) {
            for(int i = 0; i < GameStates.Count; i++) GameStates[i].Draw(gameTime, interpolation);
        }
    }
}