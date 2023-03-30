using UnityEditor;
using Autis.Editor.Constantes;

namespace Autis.Editor.UI {
    public static class LayoutLoader {
        [MenuItem("Engine Para Terapeutas/Carregar Tela Inical")]
        public static void CarregarTelaInicial() {
            LayoutManager.CarregarLayout(ConstantesEditor.NomePastaLayouts + ConstantesLayouts.NomeLayoutTelaInicial);
            return;
        }

        [MenuItem("Engine Para Terapeutas/Carregar Tela Editor")]
        public static void CarregarTelaEditor() {
            LayoutManager.CarregarLayout(ConstantesEditor.NomePastaLayouts + ConstantesLayouts.NomeLayoutTelaEditor);
            return;
        }
    }
}
