using KuubEngine.Core;

using OpenTK;

namespace KuubEngine.Scene.Components {
    public class Camera : Component {
        public Matrix4 ProjectionMatrix { get; set; }
        public Matrix4 ViewMatrix { get; set; }

        public Camera(GameObject ent, float aspect) : base(ent) {
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect, 0.5f, 10000f);
            ViewMatrix = Matrix4.LookAt(Position, Vector3.Zero, Vector3.UnitY);
        }

        public override void OnUpdate(GameTime gameTime) {}
    }
}