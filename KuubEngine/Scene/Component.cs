using System;
using System.Reflection;

using KuubEngine.Core;

using OpenTK;

namespace KuubEngine.Scene {
    public abstract class Component {
        public readonly GameObject Entity;

        protected int ID {
            get { return Entity.ID; }
        }
        protected GameScene Scene {
            get { return Entity.Scene; }
        }
        protected Vector3 Position {
            get { return Entity.Position; }
        }
        protected Vector2 Position2D {
            get { return Entity.Position2D; }
        }

        protected Component(GameObject ent) {
            Entity = ent;
        }

        public static T Create<T>(GameObject ent) where T : Component {
            ConstructorInfo c = typeof(T).GetConstructor(new Type[] { typeof(GameObject) });
            if(c == null) throw new MissingMethodException(typeof(T).FullName + " has no valid constructor.");
            return (T)c.Invoke(new object[] { ent });
        }

        public virtual void OnSpawn() {}
        public virtual void OnDestroy() {}
        public virtual void OnUpdate(GameTime gameTime) {}
    }
}