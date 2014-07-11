using System;
using System.Diagnostics;

using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics {
    public class VertexArray : IDisposable {
        private int id;
        public int ID {
            get {
                if(id == 0) GL.GenVertexArrays(1, out id);
                return id;
            }
        }

#if DEBUG
        ~VertexArray() {
            Debug.Assert(id == 0, this + " leaked!");
        }
#endif

        public void Dispose() {
            if(id != 0) {
                GL.DeleteVertexArray(id);
                id = 0;
            }
        }

        public void BindBuffer(GraphicsBuffer buffer, int index) {
            Bind();
            GL.EnableVertexAttribArray(index);
            buffer.Bind();
            GL.VertexAttribPointer(index, buffer.Stride, buffer.Type, false, 0, 0);
        }

        public void Bind() {
            GL.BindVertexArray(ID);
        }
    }
}