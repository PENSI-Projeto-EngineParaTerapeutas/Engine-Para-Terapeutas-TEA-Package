using UnityEditor;
using UnityEngine.UIElements;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Editor.UI;

namespace Autis.Editor.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(IdentificadorTipoApoioObjetoInteracao))]
    public class CustomEditorTipoApoioInteracaoBehaviour : CustomEditorBase {
        protected override string CaminhoTemplate => "CustomEditor/CustomEditorTipoApoioInteracao/CustomEditorTipoApoioInteracaoTemplate.uxml";
        protected override string CaminhoStyle => "CustomEditor/CustomEditorTipoApoioInteracao/CustomEditorTipoApoioInteracaoStyle.uss";

        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_TIPO_ACAO = "regiao-carregamento-inputs-tipo-acao";
        private VisualElement regiaoCarregamentoInputsTipoAcao;

        private InputsTipoApoioInteracao grupoInputsTipoApoioObjetoInteracao;

        #endregion

        private IdentificadorTipoApoioObjetoInteracao componente;

        protected override void OnRenderizarInterface() {
            componente = target as IdentificadorTipoApoioObjetoInteracao;

            grupoInputsTipoApoioObjetoInteracao = new InputsTipoApoioInteracao();
            ConfigurarTipoApoioObjetoInteracao();

            return;
        }

        private void ConfigurarTipoApoioObjetoInteracao() {
            regiaoCarregamentoInputsTipoAcao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_TIPO_ACAO);
            regiaoCarregamentoInputsTipoAcao.Add(grupoInputsTipoApoioObjetoInteracao.Root);

            grupoInputsTipoApoioObjetoInteracao.VincularDados(componente);

            return;
        }
    }
}