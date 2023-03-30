using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using Autis.Editor.Constantes;

namespace Autis.Editor.Utils {
    public static class Importador {
        public static T ImportarAsset<T>(string caminho) where T : UnityEngine.Object {
            return AssetDatabase.LoadAssetAtPath<T>(Path.Combine(ConstantesEditor.CaminhoPastaEditor, caminho));
        }

        public static Texture ImportarImagem(string caminho) {
            return AssetDatabase.LoadAssetAtPath<Texture>(Path.Combine(ConstantesEditor.CaminhoPastaImagensEditor, caminho));
        }

        public static GameObject ImportarPrefab(string caminho) {
            return AssetDatabase.LoadAssetAtPath<GameObject>(Path.Combine(ConstantesEditor.CaminhoPastaResourcesRuntime, "Prefabs/", caminho));
        }

        public static VisualTreeAsset ImportarUXML(string caminho) {
            return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Path.Combine(ConstantesEditor.CaminhoPastaEditor, caminho));
        }

        public static StyleSheet ImportarUSS(string caminho) {
            return AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(ConstantesEditor.CaminhoPastaEditor, caminho));
        }
    }
}