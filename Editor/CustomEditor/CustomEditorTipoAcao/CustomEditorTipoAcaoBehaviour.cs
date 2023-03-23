using UnityEditor;
using UnityEngine.UIElements;
using EngineParaTerapeutas.ComponentesGameObjects;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(IdentificadorTipoAcao))]
    public class CustomEditorTipoAcaoBehaviour : CustomEditorBase {
        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_TIPO_ACAO = "regiao-carregamento-inputs-tipo-acao";
        private VisualElement regiaoCarregamentoInputsTipoAcao;

        private InputsIdentificadorTipoAcao grupoInputsIdentificaadorTipoAcao;

        #endregion

        private IdentificadorTipoAcao componente;

        protected override void OnEnable() {
            base.OnEnable();

            componente = target as IdentificadorTipoAcao;

            ImportarTemplate("/CustomEditor/CustomEditorTipoAcao/CustomEditorTipoAcaoTemplate.uxml");
            ImportarStyle("/CustomEditor/CustomEditorTipoAcao/CustomEditorTipoAcaoStyle.uss");

            grupoInputsIdentificaadorTipoAcao = new InputsIdentificadorTipoAcao();

            ConfigurarInputs();

            return;
        }

        protected override void ConfigurarInputs() {
            regiaoCarregamentoInputsTipoAcao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_TIPO_ACAO);
            regiaoCarregamentoInputsTipoAcao.Add(grupoInputsIdentificaadorTipoAcao.Root);

            grupoInputsIdentificaadorTipoAcao.VincularDados(componente);

            return;
        }
    }
}