using Autis.Runtime.DTOs;

namespace Autis.Runtime.Constantes {
    public static class LayersProjeto {
        public readonly static LayerInfo Default = new() { Nome = "Default", Index = 0 };

        public readonly static LayerInfo IgnoreRaycast = new() { Nome = "IgnoreRaycast", Index = 2 };

        public readonly static LayerInfo UI = new() { Nome = "UI", Index = 5 };

        public readonly static LayerInfo EditorOnly = new() { Nome = "EditorOnly", Index = 31 };
    }
}
