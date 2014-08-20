using System;
using System.Collections;
using System.Collections.Generic;

using KuubEngine.Core;

using OpenTK;

namespace KuubEngine.Scene {
    public delegate void EntityBuilderDelegate(Entity ent);

    public struct EntityBuilder {
        public readonly string Name;
        public readonly string Base;
        public readonly EntityBuilderDelegate Builder;

        public EntityBuilder(string name, EntityBuilderDelegate builder) {
            Name = name;
            Base = null;
            Builder = builder;
        }

        public EntityBuilder(string name, string baseName, EntityBuilderDelegate builder) {
            Name = name;
            Base = baseName;
            Builder = builder;
        }
    }

    public sealed class Entity : IEnumerable<Component> {
        private static int nextID;
        private static readonly Dictionary<string, EntityBuilder> builders = new Dictionary<string, EntityBuilder>();

        private readonly Dictionary<Type, Component> componentsDictionary = new Dictionary<Type, Component>();

        public readonly int ID;
        public readonly GameScene Scene;

        private Vector3 position;
        public Vector3 Position {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Position2D {
            get { return new Vector2(position.X, position.Z); }
            set {
                position.X = value.X;
                position.Z = value.Y;
            }
        }

        public bool Spawned { get; private set; }

        private Entity(GameScene scene) {
            ID = nextID++;
            Scene = scene;
            Scene.Add(this);
            Spawned = false;
        }

        public static void Register(string name, EntityBuilderDelegate builder) {
            builders[name] = new EntityBuilder(name, builder);
        }

        public static void Register(string name, string baseName, EntityBuilderDelegate builder) {
            builders[name] = new EntityBuilder(name, baseName, builder);
        }

        public static Entity Create(GameScene scene) {
            return new Entity(scene);
        }

        public static Entity Create(GameScene scene, string type) {
            EntityBuilder builder = builders[type];
            Entity ent = builder.Base != null ? Create(scene, builder.Base) : Create(scene);
            builder.Builder(ent);
            return ent;
        }

        public IEnumerator<Component> GetEnumerator() {
            return componentsDictionary.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public T AddComponent<T>() where T : Component {
            T comp = Component.Create<T>(this);
            componentsDictionary.Add(typeof(T), comp);

            return comp;
        }

        public void RemoveComponent<T>() where T : Component {
            componentsDictionary.Remove(typeof(T));
        }

        public bool HasComponent<T>() where T : Component {
            return componentsDictionary.ContainsKey(typeof(T));
        }

        public T GetComponent<T>() where T : Component {
            Component result;
            componentsDictionary.TryGetValue(typeof(T), out result);
            return (T)result;
        }

        public void Spawn() {
            if(Spawned) return;

            Spawned = true;
            foreach(Component comp in this) comp.OnSpawn();
        }

        public void Destroy() {
            if(!Spawned) return;

            foreach(Component comp in this) comp.OnDestroy();
            Spawned = false;
        }

        public void Update(GameTime gameTime) {
            if(!Spawned) return;

            foreach(Component comp in this) comp.OnUpdate(gameTime);
        }
    }
}