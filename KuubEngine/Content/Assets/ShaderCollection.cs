using System;
using System.Collections.Generic;
using System.IO;

using KuubEngine.Annotations;
using KuubEngine.Diagnostics;
using KuubEngine.Graphics;

using Newtonsoft.Json;

using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Content.Assets {
    public class ShaderCollection : Asset {
        private class ShaderTypeEnumConverter : JsonConverter {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
                ShaderType type = (ShaderType)value;
                switch(type) {
                    case ShaderType.FragmentShader:
                        writer.WriteValue("fragment");
                        break;
                    case ShaderType.VertexShader:
                        writer.WriteValue("vertex");
                        break;
                    case ShaderType.GeometryShader:
                        writer.WriteValue("geometry");
                        break;
                    case ShaderType.TessEvaluationShader:
                        writer.WriteValue("tesseval");
                        break;
                    case ShaderType.TessControlShader:
                        writer.WriteValue("tesscontrol");
                        break;
                    case ShaderType.ComputeShader:
                        writer.WriteValue("compute");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
                string value = (string)reader.Value;

                if(value.ToLower() == "fragment") return ShaderType.FragmentShader;
                if(value.ToLower() == "vertex") return ShaderType.VertexShader;
                if(value.ToLower() == "geometry") return ShaderType.GeometryShader;
                if(value.ToLower() == "tesseval") return ShaderType.TessEvaluationShader;
                if(value.ToLower() == "tesscontrol") return ShaderType.TessControlShader;
                if(value.ToLower() == "compute") return ShaderType.ComputeShader;

                return null;
            }

            public override bool CanConvert(Type objectType) {
                return objectType == typeof(string);
            }
        }

        [UsedImplicitly]
        private class SerializedShader {
            [JsonProperty("type", Required = Required.Always), JsonConverter(typeof(ShaderTypeEnumConverter))]
            public ShaderType Type { get; set; }

            [JsonProperty("file", Required = Required.Always)]
            public string File { get; set; }

            [JsonProperty("fragdata", Required = Required.Default)]
            public Dictionary<string, int> FragData { get; set; }
        }

        [UsedImplicitly]
        private class SerializedShaderCollection {
            [JsonProperty("name", Required = Required.Always)]
            public string Name { get; set; }

            [JsonProperty("shaders", Required = Required.Always)]
            public SerializedShader[] Shaders { get; set; }

            [JsonProperty("attributes", Required = Required.Default)]
            public Dictionary<string, int> Attributes { get; set; }
        }

        public ShaderProgram Program { get; protected set; }
        private readonly List<Shader> shaders = new List<Shader>();

        // TODO: do some fool proof path handling because this is retarded
        public override void Load(string path) {
            Log.Debug("Attempting to load ShaderCollection {0}.json", path);

            var json = JsonConvert.DeserializeObject<SerializedShaderCollection>(File.ReadAllText(path + ".json"));

            Log.Debug("Loading shader {0}", json.Name);

            Program = new ShaderProgram();

            for(int i = 0; i < json.Shaders.Length; i++) {
                Shader shader = new Shader(json.Shaders[i].Type, File.ReadAllText(Path.GetDirectoryName(path + ".json") + "/" + json.Shaders[i].File));
                shader.Attach(Program);
                shaders.Add(shader);

                if(json.Shaders[i].Type == ShaderType.FragmentShader && json.Shaders[i].FragData != null) foreach(var fragdata in json.Shaders[i].FragData) GL.BindFragDataLocation(Program.ID, fragdata.Value, fragdata.Key);

                Log.Debug("\tFound {0} {1}", json.Shaders[i].Type, json.Shaders[i].File);
            }

            if(json.Attributes != null && json.Attributes.Count > 0) foreach(var attrib in json.Attributes) GL.BindAttribLocation(Program.ID, attrib.Value, attrib.Key);

            Program.Link();

            base.Load(path);
        }

        public override void Unload() {
            foreach(var shader in shaders) shader.Dispose();
            Program.Dispose();

            base.Unload();
        }

        public void Use() {
            Program.Use();
        }
    }
}