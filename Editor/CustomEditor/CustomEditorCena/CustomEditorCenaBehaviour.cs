using UnityEditor;
using UnityEngine.UIElements;
using EngineParaTerapeutas.ScriptableObjects;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(Cena))]
    public class CustomEditorCenaBehaviour : CustomEditorBase {
        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_PADROES_CENA = "regiao-carregamento-inputs-padroes-cena";
        private VisualElement regiaoCarregamentoInputsPadroesCena;

        private InputsScriptableObjectCena grupoInputsCena;

        #endregion

        private Cena cena;

        protected override void OnEnable() {
            base.OnEnable();
            cena = target as Cena;

            ImportarTemplate("CustomEditor/CustomEditorCena/CustomEditorCenaTemplate.uxml");
            ImportarStyle("CustomEditor/CustomEditorCena/CustomEditorCenaStyle.uss");

            grupoInputsCena = new InputsScriptableObjectCena();

            ConfigurarInputs();

            return;
        }

        protected override void ConfigurarInputs() {
            regiaoCarregamentoInputsPadroesCena = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_PADROES_CENA);
            regiaoCarregamentoInputsPadroesCena.Add(grupoInputsCena.Root);

            grupoInputsCena.VincularDados(cena);

            return;
        }
    }
}