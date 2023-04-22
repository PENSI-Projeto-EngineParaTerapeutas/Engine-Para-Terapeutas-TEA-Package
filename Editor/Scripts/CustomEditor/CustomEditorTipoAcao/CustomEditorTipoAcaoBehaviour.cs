using UnityEditor;
using UnityEngine.UIElements;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Editor.UI;

namespace Autis.Editor.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(IdentificadorTipoAcao))]
    public class CustomEditorTipoAcaoBehaviour : CustomEditorBase {
        protected override string CaminhoTemplate => "CustomEditor/CustomEditorTipoAcao/CustomEditorTipoAcaoTemplate.uxml";
        protected override string CaminhoStyle => "CustomEditor/CustomEditorTipoAcao/CustomEditorTipoAcaoStyle.uss";

        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_TIPO_ACAO = "regiao-carregamento-inputs-tipo-acao";
        private VisualElement regiaoCarregamentoInputsTipoAcao;

        private InputsIdentificadorTipoAcao grupoInputsIdentificadorTipoAcao;

        #endregion

        private IdentificadorTipoAcao componente;

        protected override void OnRenderizarInterface() {
            componente = target as IdentificadorTipoAcao;

            grupoInputsIdentificadorTipoAcao = new InputsIdentificadorTipoAcao();
            ConfigurarInputTipoAcao();

            return;
        }

        private void ConfigurarInputTipoAcao() {
            regiaoCarregamentoInputsTipoAcao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_TIPO_ACAO);
            regiaoCarregamentoInputsTipoAcao.Add(grupoInputsIdentificadorTipoAcao.Root);

            grupoInputsIdentificadorTipoAcao.VincularDados(componente);

            return;
        }
    }
}