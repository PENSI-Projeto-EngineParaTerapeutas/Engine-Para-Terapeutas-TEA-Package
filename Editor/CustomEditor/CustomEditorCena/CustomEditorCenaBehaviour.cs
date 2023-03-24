using UnityEditor;
using UnityEngine.UIElements;
using EngineParaTerapeutas.ScriptableObjects;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(Cena))]
    public class CustomEditorCenaBehaviour : CustomEditorBase {
        protected override string CaminhoTemplate => "CustomEditor/CustomEditorCena/CustomEditorCenaTemplate.uxml";
        protected override string CaminhoStyle => "CustomEditor/CustomEditorCena/CustomEditorCenaStyle.uss";

        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_PADROES_CENA = "regiao-carregamento-inputs-padroes-cena";
        private VisualElement regiaoCarregamentoInputsPadroesCena;

        private InputsScriptableObjectCena grupoInputsCena;

        #endregion

        private Cena cena;

        protected override void OnRenderizarInterface() {
            cena = target as Cena;

            grupoInputsCena = new InputsScriptableObjectCena();
            ConfigurarInputsCena();

            return;
        }

        private void ConfigurarInputsCena() {
            regiaoCarregamentoInputsPadroesCena = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_PADROES_CENA);
            regiaoCarregamentoInputsPadroesCena.Add(grupoInputsCena.Root);

            grupoInputsCena.VincularDados(cena);

            return;
        }
    }
}