using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.ComponentesGameObjects;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(Colisor))]
    public class CustomEditorColisorBehaviour : CustomEditor<Colisor, BoxCollider2D> {
        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_PADROES_COLISOR = "regiao-carregamento-inputs-padroes-colisor";
        private VisualElement regiaoCarregamentoInputsPadroesColisor;

        private InputsColisor grupoInputsColisor;

        #endregion

        public override void OnEnable() {
            base.OnEnable();
            grupoInputsColisor = new InputsColisor();

            ImportarTemplate("Packages/com.pensi.engine-para-terapeutas-tea/Editor/CustomEditor/CustomEditorColisor/CustomEditorColisorTemplate.uxml");
            ImportarStyle("Packages/com.pensi.engine-para-terapeutas-tea/Editor/CustomEditor/CustomEditorColisor/CustomEditorColisorStyle.uss");
            ConfigurarInputs();

            return;
        }

        protected override void ConfigurarInputs() {
            regiaoCarregamentoInputsPadroesColisor = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_PADROES_COLISOR);
            regiaoCarregamentoInputsPadroesColisor.Add(grupoInputsColisor.Root);

            grupoInputsColisor.VincularDados(componenteOriginal);

            return;
        }
    }
}