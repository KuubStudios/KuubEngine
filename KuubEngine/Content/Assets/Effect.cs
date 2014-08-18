using System;
using System.Collections.Generic;
using System.IO;

using KuubEngine.Annotations;
using KuubEngine.Diagnostics;
using KuubEngine.Graphics;

using Newtonsoft.Json;

using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Content.Assets {
    public class Effect : Asset {
        #region Serialization
        private class ShaderTypeEnumConverter : JsonConverter {
            private string GetTypeString(ShaderType type) {
                switch(type) {
                    case ShaderType.FragmentShader:
                        return "fragment";
                    case ShaderType.VertexShader:
                        return "vertex";
                    case ShaderType.GeometryShader:
                        return "geometry";
                    case ShaderType.TessEvaluationShader:
                        return "tesseval";
                    case ShaderType.TessControlShader:
                        return "tesscontrol";
                    case ShaderType.ComputeShader:
                        return "compute";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
                writer.WriteValue(GetTypeString((ShaderType)value));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
                string value = ((string)reader.Value).ToLower();

                if(value == "fragment") return ShaderType.FragmentShader;
                if(value == "vertex") return ShaderType.VertexShader;
                if(value == "geometry") return ShaderType.GeometryShader;
                if(value == "tesseval") return ShaderType.TessEvaluationShader;
                if(value == "tesscontrol") return ShaderType.TessControlShader;
                if(value == "compute") return ShaderType.ComputeShader;

                return null;
            }

            public override bool CanConvert(Type objectType) {
                return objectType == typeof(string);
            }
        }

        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        private class SerializedShader {
            [JsonProperty("type", Required = Required.Always), JsonConverter(typeof(ShaderTypeEnumConverter))]
            public ShaderType Type { get; set; }

            [JsonProperty("file", Required = Required.Always)]
            public string File { get; set; }

            [JsonProperty("fragdata", Required = Required.Default)]
            public Dictionary<string, int> FragData { get; set; }
        }

        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        private class SerializedShaderCollection {
            [JsonProperty("name", Required = Required.Always)]
            public string Name { get; set; }

            [JsonProperty("shaders", Required = Required.Always)]
            public SerializedShader[] Shaders { get; set; }

            [JsonProperty("attributes", Required = Required.Default)]
            public Dictionary<string, int> Attributes { get; set; }
        }
        #endregion

        public ShaderProgram Program { get; protected set; }
        private readonly List<Shader> shaders = new List<Shader>();

        public Effect() {
            Program = new ShaderProgram();
        }

        public override void Load(string path) { 
            base.Load(path + ".json");
        }

        public override void Load(Stream file, string root) {
            StreamReader reader = new StreamReader(file);
            var json = JsonConvert.DeserializeObject<SerializedShaderCollection>(reader.ReadToEnd());

            Log.Debug("Loading shader {0}", json.Name);

            for(int i = 0; i < json.Shaders.Length; i++) {
                Log.Debug("\tFound {0} {1}", json.Shaders[i].Type, json.Shaders[i].File);

                Shader shader = new Shader(json.Shaders[i].Type, File.ReadAllText(Path.Combine(root, json.Shaders[i].File)));

                shader.Attach(Program);
                shaders.Add(shader);

                if(json.Shaders[i].Type == ShaderType.FragmentShader && json.Shaders[i].FragData != null) foreach(var fragdata in json.Shaders[i].FragData) GL.BindFragDataLocation(Program.ID, fragdata.Value, fragdata.Key);
            }

            if(json.Attributes != null && json.Attributes.Count > 0) foreach(var attrib in json.Attributes) GL.BindAttribLocation(Program.ID, attrib.Value, attrib.Key);

            Program.Link();

            base.Load(file, root);
        }

        public override void Unload() {
            foreach(var shader in shaders) shader.Dispose();
            Program.Dispose();

            base.Unload();
        }

        public void Use() {
            Program.Bind();
        }
    }
}