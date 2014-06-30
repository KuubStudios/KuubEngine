using System;
using System.Diagnostics;

using KuubEngine.Diagnostics;

using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics {
    public class ShaderLinkException : Exception {
        public ShaderLinkException(string message) : base(message) {}
    }

    public class ShaderProgram : IDisposable {
        public static ShaderProgram Current;

        private int id;
        public int ID {
            get {
                if(id == 0) id = GL.CreateProgram();
                return id;
            }
        }

#if DEBUG
        ~ShaderProgram() {
            Debug.Assert(id == 0, this + " leaked!");
        }
#endif

        public void Dispose() {
            if(Current == this) Clear();
            if(id != 0) {
                GL.DeleteProgram(ID);
                id = 0;
            }
        }

        public static void Clear() {
            GL.UseProgram(0);
            Current = null;
        }

        public void Link() {
            GL.LinkProgram(ID);

            int status;
            GL.GetProgram(ID, GetProgramParameterName.ValidateStatus, out status);
            if(status != 1) throw new ShaderLinkException(GL.GetProgramInfoLog(ID));

            GL.ValidateProgram(ID);

            int length;
            GL.GetProgram(ID, GetProgramParameterName.InfoLogLength, out length);
            if(length > 1) Log.Warn("ShaderProgram {0} info log not empty:\n\t{1}", ID, GL.GetProgramInfoLog(ID));
        }

        public void Use() {
            if(Current == this) return;

            GL.UseProgram(ID);
            Current = this;
        }
    }
}