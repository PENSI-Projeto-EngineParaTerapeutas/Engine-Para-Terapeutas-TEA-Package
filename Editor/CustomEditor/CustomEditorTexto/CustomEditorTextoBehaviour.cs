using UnityEditor;
using UnityEngine.UIElements;
using EngineParaTerapeutas.UI;
using EngineParaTerapeutas.ComponentesGameObjects;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(Texto))]
    public class CustomEditorTextoBehaviour : CustomEditorBase {
        #region .: Elementos :.

        private const string REGIAO_CARREGAMENTO_INPUTS_PADROES_TEXTO = "regiao-carregamento-inputs-padroes-texto";
        private VisualElement regiaoCarregamentosInputsPadroesTexto;

        private InputsComponenteTexto grupoInputsTexto;

        #endregion

        private Texto componente;

        protected override void OnEnable() {
            base.OnEnable();

            componente = target as Texto;
            root = new VisualElement();
            grupoInputsTexto = new InputsComponenteTexto();

            ImportarTemplate("/CustomEditor/CustomEditorTexto/CustomEditorTextoTemplate.uxml");
            ImportarStyle("/CustomEditor/CustomEditorTexto/CustomEditorTextoStyle.uss");

            ConfigurarInputs();

            return;
        }

        protected override void ConfigurarInputs() {
            regiaoCarregamentosInputsPadroesTexto = root.Query<VisualElement>(REGIAO_CARREGAMENTO_INPUTS_PADROES_TEXTO);
            regiaoCarregamentosInputsPadroesTexto.Add(grupoInputsTexto.Root);

            grupoInputsTexto.VincularDados(componente);

            return;
        }
    }
}