using System.IO;
using System.Reflection;
using Type = System.Type;
using UnityEngine;
using UnityEditor;
using Autis.Editor.Constantes;

namespace Autis.Editor.UI {
    public static class LayoutManager {
        #region .: Mensagens :.

        private const string MENSAGEM_ERRO_CARREGAR_TIPO_WINDOW_LAYOUT = "[ERROR]: Não foi possível obter o Tipo WindowLayout";
        private const string MENSAGEM_ERRO_CARREGAR_METODO = "[ERROR]: Não foi possível encontrar método de {nome} layouts";

        #endregion

        private enum TipoMetodo { Salvar, Carregar };
        private static string CaminhoPastaLayoutsSalvos { get => Path.Combine(ConstantesEditor.CaminhoPastaEditor, "Assets/Layouts/"); }

        private static string GetCaminhoDiretorioAtual {
            get {
                string[] assets  = AssetDatabase.FindAssets($"t:Script {nameof(Inicializacao)}");
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
                Debug.LogError(MENSAGEM_ERRO_CARREGAR_TIPO_WINDOW_LAYOUT);
                return null;
            }

            MethodInfo save = null;
            MethodInfo load = null;

            load = WindowLayout.GetMethod("LoadWindowLayout", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string), typeof(bool) }, null);
            if(load == null) {
                Debug.LogError(MENSAGEM_ERRO_CARREGAR_METODO.Replace("{nome}", "Carregar"));
            }

            save = WindowLayout.GetMethod("SaveWindowLayout", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string) }, null);
            if(save == null) {
                Debug.LogError(MENSAGEM_ERRO_CARREGAR_METODO.Replace("{nome}", "Salvar"));
            }

            if(tipoMetodo == TipoMetodo.Salvar) {
                return save;
            } 
            else {
                return load;
            }
        }

        public static void CarregarLayout(string path) {
            path = Path.Combine(CaminhoPastaLayoutsSalvos, path);
            CarregarMetodo(TipoMetodo.Carregar).Invoke(null, new object[] { path, false });

            return;
        }

        public static void SalvarLayoutAtual(string nomeLayout) {
            SalvarLayout(CaminhoPastaLayoutsSalvos + nomeLayout + ExtensoesEditor.Layout);
            return;
        }
    }
}