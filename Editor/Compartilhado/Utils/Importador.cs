using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.Utils {
    public static class Importador {
        public static T ImportarAsset<T>(string caminho) where T : UnityEngine.Object {
            return AssetDatabase.LoadAssetAtPath<T>(Path.Combine(ConstantesEditor.PastaRaiz, caminho));
        }

        public static Texture ImportarImagem(string caminho) {
            return AssetDatabase.LoadAssetAtPath<Texture>(Path.Combine(ConstantesEditor.CaminhoCompletoPastaImagens, caminho));
        }

        public static GameObject ImportarPrefab(string caminho) {
            return AssetDatabase.LoadAssetAtPath<GameObject>(Path.Combine(ConstantesRuntime.CaminhoCompletoPastaResources, "Prefabs/", caminho));
        }

        public static VisualTreeAsset ImportarUXML(string caminho) {
            return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Path.Combine(ConstantesEditor.PastaRaiz, caminho));
        }

        public static StyleSheet ImportarUSS(string caminho) {
            return AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(ConstantesEditor.PastaRaiz, caminho));
        }
    }
}