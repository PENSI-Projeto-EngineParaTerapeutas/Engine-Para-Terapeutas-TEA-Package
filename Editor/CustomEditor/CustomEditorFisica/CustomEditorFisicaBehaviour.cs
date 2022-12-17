using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.ComponentesGameObjects;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(Fisica))]
    public class CustomEditorFisicaBehaviour : CustomEditor<Fisica, Rigidbody2D> {
        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_PADROES_FISICA = "regiao-carregamento-inputs-padroes-fisica";
        private VisualElement regiaoCarregamentoInputsPadroesFisica;

        private InputsFisica grupoInputFisica;

        #endregion

        public override void OnEnable() {
            base.OnEnable();
            grupoInputFisica = new InputsFisica();

            ImportarTemplate("/CustomEditor/CustomEditorFisica/CustomEditorFisicaTemplate.uxml");
            ImportarStyle("/CustomEditor/CustomEditorFisica/CustomEditorFisicaStyle.uss");
            ConfigurarInputs();

            return;
        }

        protected override void ConfigurarInputs() {
            regiaoCarregamentoInputsPadroesFisica = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_PADROES_FISICA);
            regiaoCarregamentoInputsPadroesFisica.Add(grupoInputFisica.Root);

            grupoInputFisica.VincularDados(componenteOriginal);

            return;
        }
    }
}