using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.ComponentesGameObjects;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(Colisor))]
    public class CustomEditorColisorBehaviour : CustomEditorComponentes<Colisor, BoxCollider2D> {
        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_PADROES_COLISOR = "regiao-carregamento-inputs-padroes-colisor";
        private VisualElement regiaoCarregamentoInputsPadroesColisor;

        private InputsComponenteColisor grupoInputsColisor;

        #endregion

        public override void OnEnable() {
            base.OnEnable();
            grupoInputsColisor = new InputsComponenteColisor();

            ImportarTemplate("/CustomEditor/CustomEditorColisor/CustomEditorColisorTemplate.uxml");
            ImportarStyle("/CustomEditor/CustomEditorColisor/CustomEditorColisorStyle.uss");
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