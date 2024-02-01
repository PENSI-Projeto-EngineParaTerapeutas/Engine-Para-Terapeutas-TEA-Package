using UnityEditor;
using UnityEngine;
using Autis.Runtime.Constantes;

namespace Autis.Editor.Utils {
    public static class ControladorFoco {
        public static void VerificarSelecao() {
            if(Selection.objects.Length <= 0) {
                return;
            }

            SelecionarGameObjectRaiz(Selection.activeTransform);
            SelecionarFerramentaPorTipo(Selection.activeTransform);

            return;
        }

        public static void FixarSelecao(GameObject gameObject) {
            if(gameObject == null) {
                return;
            }

            FixarSelecao(gameObject.transform);
            
            return;
        }

        public static void FixarSelecao(Transform gameObject) {
            if(gameObject == null) {
                return;
            }

            Selection.activeObject = gameObject;

            return;
        }

        private static void SelecionarGameObjectRaiz(Transform objetoSelecionado) {
            if(objetoSelecionado == null) {
                return;
            }

            Transform rootGameObject = objetoSelecionado.root;
            if(objetoSelecionado != rootGameObject) {
                Selection.activeObject = rootGameObject;
            }

            return;
        }

        private static void SelecionarFerramentaPorTipo(Transform objetoSelecionado) {
            if(objetoSelecionado == null && Tools.current != Tool.Rect) {
                Tools.current = Tool.Rect;
                return;
            }
            else if(objetoSelecionado == null) {
                return;
            }

            if(objetoSelecionado.CompareTag(NomesTags.Personagem)) {
                Tools.current = Tool.Move;
            }
            else {
                Tools.current = Tool.Rect;
            }

            return;
        }
    }
}