using UnityEditor;
using UnityEngine.UIElements;
using EngineParaTerapeutas.ComponentesGameObjects;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(IdentificadorTipoApoioObjetoInteracao))]
    public class CustomEditorIdentificadorTipoApoioInteracaoBehaviour : CustomEditorBase {
        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_TIPO_ACAO = "regiao-carregamento-inputs-tipo-acao";
        private VisualElement regiaoCarregamentoInputsTipoAcao;

        private InputsIdentificadorTipoApoioInteracao grupoInputsTipoApoioObjetoInteracao;

        #endregion

        private IdentificadorTipoApoioObjetoInteracao componente;

        protected override void OnEnable() {
            base.OnEnable();

            componente = target as IdentificadorTipoApoioObjetoInteracao;
            
            ImportarTemplate("/CustomEditor/CustomEditorIdentificadorTipoApoioInteracao/CustomEditorIdentificadorTipoApoioInteracaoTemplate.uxml");
            ImportarStyle("/CustomEditor/CustomEditorIdentificadorTipoApoioInteracao/CustomEditorIdentificadorTipoApoioInteracaoStyle.uss");

            grupoInputsTipoApoioObjetoInteracao = new InputsIdentificadorTipoApoioInteracao();

            ConfigurarInputs();

            return;
        }

        protected override void ConfigurarInputs() {
            regiaoCarregamentoInputsTipoAcao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_TIPO_ACAO);
            regiaoCarregamentoInputsTipoAcao.Add(grupoInputsTipoApoioObjetoInteracao.Root);

            grupoInputsTipoApoioObjetoInteracao.VincularDados(componente);

            return;
        }
    }
}