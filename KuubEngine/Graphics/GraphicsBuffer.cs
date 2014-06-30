using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics {
    public class GraphicsBuffer : IDisposable {
        private uint id;
        public uint ID {
            get {
                if(id == 0) GL.GenBuffers(1, out id);
                return id;
            }
        }

        public int Length { get; private set; }
        public int Stride { get; private set; }
        public VertexAttribPointerType Type { get; private set; }

        public BufferTarget Target { get; protected set; }
        public BufferUsageHint Usage { get; protected set; }

        public GraphicsBuffer(BufferTarget target, BufferUsageHint usage = BufferUsageHint.StaticDraw) {
            Target = target;
            Usage = usage;
        }

#if DEBUG
        ~GraphicsBuffer() {
            Debug.Assert(id == 0, this + " leaked!");
        }
#endif

        public void Dispose() {
            if(id != 0) {
                GL.DeleteBuffer(id);
                id = 0;
            }
        }

        public void SetData<T>(T[] data, int stride, VertexAttribPointerType type) where T : struct {
            if(data == null) throw new ArgumentNullException("data");

            Length = data.Length;
            Stride = stride;
            Type = type;

            GL.BindBuffer(Target, ID);
            GL.BufferData(Target, new IntPtr(Length * Marshal.SizeOf(typeof(T))), data, Usage);
        }

        public void SetData(Vector3[] data) {
            SetData(data, 3, VertexAttribPointerType.Float);
        }

        public void SetData(Vector2[] data) {
            SetData(data, 2, VertexAttribPointerType.Float);
        }

        public void SetData(Color4[] data) {
            SetData(data, 4, VertexAttribPointerType.Float);
        }

        public void Bind() {
            if(Length == 0) return;

            GL.BindBuffer(Target, ID);
        }
    }
}