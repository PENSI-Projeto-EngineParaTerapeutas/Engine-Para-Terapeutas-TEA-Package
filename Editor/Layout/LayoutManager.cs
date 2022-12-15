using UnityEngine;
using System.IO;
using System.Reflection;
using Type = System.Type;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.UI {
    public static class LayoutManager {
        private enum TipoMetodo { Salvar, Carregar };

        public static void SalvarLayout(string path) {
            path = Path.Combine(Directory.GetCurrentDirectory(), path);
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
            path = Path.Combine(Directory.GetCurrentDirectory(), path);
            CarregarMetodo(TipoMetodo.Carregar).Invoke(null, new object[] { path, false });

            return;
        }

        public static void SalvarLayoutAtual(string nomeLayout) {
            SalvarLayout(ConstantesProjeto.PastaLayouts + nomeLayout + ConstantesLayouts.ExtensaoArquivoLayout);
            return;
        }
    }
}