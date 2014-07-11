using System;
using System.Runtime.Serialization;

namespace KuubEngine.Utility {
	[Serializable]
	public class ContentLoadException : Exception {
		public ContentLoadException() {}
		public ContentLoadException(string message) : base(message) {}
		public ContentLoadException(string message, Exception inner) : base(message, inner) {}
		protected ContentLoadException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}
	[Serializable]
	public class ShaderCompileException : Exception {
		public ShaderCompileException() {}
		public ShaderCompileException(string message) : base(message) {}
		public ShaderCompileException(string message, Exception inner) : base(message, inner) {}
		protected ShaderCompileException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}
	[Serializable]
	public class ShaderLinkException : Exception {
		public ShaderLinkException() {}
		public ShaderLinkException(string message) : base(message) {}
		public ShaderLinkException(string message, Exception inner) : base(message, inner) {}
		protected ShaderLinkException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}
	[Serializable]
	public class ShaderValidateException : Exception {
		public ShaderValidateException() {}
		public ShaderValidateException(string message) : base(message) {}
		public ShaderValidateException(string message, Exception inner) : base(message, inner) {}
		protected ShaderValidateException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}
}
