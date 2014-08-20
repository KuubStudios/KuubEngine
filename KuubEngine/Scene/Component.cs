using System;
using System.Reflection;

using KuubEngine.Core;

using OpenTK;

namespace KuubEngine.Scene {
    public abstract class Component {
        public readonly Entity Entity;

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

        protected Component(Entity ent) {
            Entity = ent;
        }

        public static T Create<T>(Entity ent) where T : Component {
            ConstructorInfo c = typeof(T).GetConstructor(new Type[] { typeof(Entity) });
            if(c == null) throw new MissingMethodException(typeof(T).FullName + " has no valid constructor.");
            return (T)c.Invoke(new object[] { ent });
        }

        public virtual void OnSpawn() {}
        public virtual void OnDestroy() {}
        public virtual void OnUpdate(GameTime gameTime) {}
    }
}