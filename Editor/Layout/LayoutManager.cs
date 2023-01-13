using System.IO;
using System.Reflection;
using Type = System.Type;
using UnityEngine;
using UnityEditor;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.Utils;

namespace EngineParaTerapeutas.UI {
    public static class LayoutManager {
        private enum TipoMetodo { Salvar, Carregar };

        private static string GetCaminhoDiretorioAtual {
            get {
                string[] assets  = AssetDatabase.FindAssets($"t:Script {nameof(LayoutManager)}");
                string caminhoLayoutManager = AssetDatabase.GUIDToAssetPath(assets[0]);

                return Path.GetDirectoryName(Path.GetFullPath(caminhoLayoutManager));
            }
        }

        public static void SalvarLayout(string path) {
            path = Path.Combine(GetCaminhoDiretorioAtual, path);
            CarregarMetodo(TipoMetodo.Salvar).Invoke(null, new object[] { path });

            return;
        }

        private static MethodInfo CarregarMetodo(TipoMetodo tipoMetodo) {
            Type WindowLayout = Type.GetType("UnityEditor.WindowLayout,UnityEditor");
            if(WindowLayout == null) {
                Debug.LogError("[ERRO]: Não foi possível obter o tipo WindowLayout");
                return null;
            }

            MethodInfo save = null;
            MethodInfo load = null;

            load = WindowLayout.GetMethod("LoadWindowLayout", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string), typeof(bool) }, null);
            if(load == null) {
                Debug.LogError("[ERRO]: Não foi possível carregar método de carregar layouts");
            }

            save = WindowLayout.GetMethod("SaveWindowLayout", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string) }, null);
            if(save == null) {
                Debug.LogError("[ERRO]: Não foi possível carregar método de salvar layouts");
            }

            if(tipoMetodo == TipoMetodo.Salvar) {
                return save;
            } 
            else {
                return load;
            }
        }

        public static void CarregarLayout(string path) {
            path = Path.Combine(GetCaminhoDiretorioAtual, path);
            CarregarMetodo(TipoMetodo.Carregar).Invoke(null, new object[] { path, false });

            return;
        }

        public static void SalvarLayoutAtual(string nomeLayout) {
            SalvarLayout(Path.Combine(GetCaminhoDiretorioAtual, ConstantesEditor.NomePastaLayouts) + nomeLayout + Extensoes.Layout);
            return;
        }
    }
}