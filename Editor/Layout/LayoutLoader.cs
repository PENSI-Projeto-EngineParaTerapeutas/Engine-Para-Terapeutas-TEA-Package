using UnityEditor;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.UI {
    public class LayoutLoader {
        [MenuItem("Engine Para Terapeutas/Carregar Tela Inical")]
        public static void CarregarTelaInicial() {
            LayoutManager.CarregarLayout(ConstantesProjeto.PastaLayouts + ConstantesLayouts.NomeLayoutTelaInicial);
            return;
        }

        [MenuItem("Engine Para Terapeutas/Carregar Tela Editor")]
        public static void CarregarTelaEditor() {
            LayoutManager.CarregarLayout(ConstantesProjeto.PastaLayouts + ConstantesLayouts.NomeLayoutTelaEditor);
            return;
        }
    }
}
