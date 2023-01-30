using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using EngineParaTerapeutas.UI;
using EngineParaTerapeutas.ComponentesGameObjects;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(Texto))]
    public class CustomEditorTextoBehaviour : CustomEditorComponentes<Texto, TextMeshProUGUI> {
        #region .: Elementos :.

        private const string REGIAO_CARREGAMENTO_INPUTS_PADROES_TEXTO = "regiao-carregamento-inputs-padroes-texto";
        private VisualElement regiaoCarregamentosInputsPadroesTexto;

        private InputsComponenteTexto grupoInputsTexto;

        #endregion

        public override void OnEnable() {
            componente = target as Texto;
            componenteOriginal = componente.TextMesh;
            root = new VisualElement();

            ImportarDefaultStyle();
            AlterarVisibilidadeComponenteOriginal(HideFlags.HideInInspector);

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