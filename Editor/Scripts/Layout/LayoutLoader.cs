using UnityEditor;
using Autis.Editor.Constantes;

namespace Autis.Editor.UI {
    public static class LayoutLoader {
        [MenuItem("AUTIS/Carregar Tela Inical")]
        public static void CarregarTelaInicial() {
            LayoutManager.CarregarLayout(ConstantesLayouts.NomeLayoutTelaInicial);
            return;
        }

        [MenuItem("AUTIS/Carregar Tela Editor")]
        public static void CarregarTelaEditor() {
            LayoutManager.CarregarLayout(ConstantesLayouts.NomeLayoutTelaEditor);
            return;
        }

        [MenuItem("AUTIS/Carregar Boas Vindas")]
        public static void CarregarTelaBoasVindas() {
            LayoutManager.CarregarLayout(ConstantesLayouts.NomeLayoutTelaBemVindo);
            return;
        }
    }
}
