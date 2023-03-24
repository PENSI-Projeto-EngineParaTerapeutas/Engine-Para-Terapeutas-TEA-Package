using UnityEditor;
using UnityEngine.UIElements;
using EngineParaTerapeutas.UI;
using EngineParaTerapeutas.ComponentesGameObjects;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(Texto))]
    public class CustomEditorTextoBehaviour : CustomEditorBase {
        protected override string CaminhoTemplate => "CustomEditor/CustomEditorTexto/CustomEditorTextoTemplate.uxml";
        protected override string CaminhoStyle => "CustomEditor/CustomEditorTexto/CustomEditorTextoStyle.uss";

        #region .: Elementos :.

        private const string REGIAO_CARREGAMENTO_INPUTS_PADROES_TEXTO = "regiao-carregamento-inputs-padroes-texto";
        private VisualElement regiaoCarregamentosInputsPadroesTexto;

        private InputsComponenteTexto grupoInputsTexto;

        #endregion

        private Texto componente;

        protected override void OnRenderizarInterface() {
            componente = target as Texto;

            grupoInputsTexto = new InputsComponenteTexto();
            ConfigurarInputsTexto();

            return;
        }

        private void ConfigurarInputsTexto() {
            regiaoCarregamentosInputsPadroesTexto = root.Query<VisualElement>(REGIAO_CARREGAMENTO_INPUTS_PADROES_TEXTO);
            regiaoCarregamentosInputsPadroesTexto.Add(grupoInputsTexto.Root);

            grupoInputsTexto.VincularDados(componente);

            return;
        }
    }
}