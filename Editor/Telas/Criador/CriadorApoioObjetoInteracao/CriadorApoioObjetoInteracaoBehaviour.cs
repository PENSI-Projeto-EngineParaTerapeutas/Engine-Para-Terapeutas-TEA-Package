using UnityEngine;
using UnityEngine.UIElements;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Runtime.Constantes;
using Autis.Editor.UI;

namespace Autis.Editor.Criadores {
    public class CriadorApoioObjetoInteracaoBehaviour : Criador {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorApoioObjetoInteracao/CriadorApoioObjetoInteracaoTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorApoioObjetoInteracao/CriadorApoioObjetoInteracaoStyle.uss";

        private readonly static Vector3 POSICAO_PADRAO_EM_RELACAO_PAI = new(1.5f, 1.5f, 0);

        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_HEADER = "regiao-carregamento-header";
        private VisualElement regiaoCarregamentoHeader;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_TIPO_APOIO_OBJETO_INTERACAO = "regiao-carregamento-inputs-tipo-apoio-objeto-interacao";
        private VisualElement regiaoCarregamentoInputsTipoApoioObjetoInteracao;

        private const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        private VisualElement regiaoCarregamentoBotoesConfirmacao;

        private readonly InputsTipoApoioInteracao inputsTipoApoioInteracao;

        #endregion

        private readonly GameObject objetoInteracaoOrigem;
        private IdentificadorTipoApoioObjetoInteracao tipoApoioObjetoInteracao;

        public CriadorApoioObjetoInteracaoBehaviour(GameObject objetoInteracaoOrigem) {
            this.objetoInteracaoOrigem = objetoInteracaoOrigem;
            inputsTipoApoioInteracao = new InputsTipoApoioInteracao();

            ImportarPrefab("ObjetosInteracao/ApoioObjetoInteracao.prefab");

            CarregarRegiaoHeader();
            CarregarInputsTipoApoioObjetoInteracao();

            ConfigurarBotoesConfirmacao();

            IniciarCriacao();
            novoObjeto.transform.SetParent(this.objetoInteracaoOrigem.transform);
            novoObjeto.transform.localPosition = POSICAO_PADRAO_EM_RELACAO_PAI;

            return;
        }

        private void CarregarRegiaoHeader() {
            regiaoCarregamentoHeader = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_HEADER);
            regiaoCarregamentoHeader.Add(header.Root);

            return;
        }

        private void CarregarInputsTipoApoioObjetoInteracao() {
            regiaoCarregamentoInputsTipoApoioObjetoInteracao = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_TIPO_APOIO_OBJETO_INTERACAO);
            regiaoCarregamentoInputsTipoApoioObjetoInteracao.Add(inputsTipoApoioInteracao.Root);

            return;
        }

        private void ConfigurarBotoesConfirmacao() {
            regiaoCarregamentoBotoesConfirmacao = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO);
            regiaoCarregamentoBotoesConfirmacao.Add(botoesConfirmacao.Root);

            botoesConfirmacao.BotaoConfirmar.clicked += HandleBotaoConfirmarClick;
            botoesConfirmacao.BotaoCancelar.clicked += HandleBotaoCancelarClick;

            return;
        }

        protected override void VincularCamposAoNovoObjeto() {
            tipoApoioObjetoInteracao = novoObjeto.GetComponent<IdentificadorTipoApoioObjetoInteracao>();
            inputsTipoApoioInteracao.VincularDados(tipoApoioObjetoInteracao);

            return;
        }

        public override void FinalizarCriacao() {
            novoObjeto.tag = NomesTags.Apoios;
            novoObjeto.layer = LayersProjeto.Default.Index;
            
            base.FinalizarCriacao();

            return;
        }

        protected override void ReiniciarPropriedadesNovoObjeto() {
            tipoApoioObjetoInteracao = null;
            return;
        }

        public override void ReiniciarCampos() {
            header.ReiniciarCampos();
            inputsTipoApoioInteracao.ReiniciarCampos();
            return;
        }
    }
}