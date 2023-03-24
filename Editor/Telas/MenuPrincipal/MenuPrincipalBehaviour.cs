using UnityEngine.UIElements;
using EngineParaTerapeutas.Criadores;

namespace EngineParaTerapeutas.Telas {
    public class MenuPrincipalBehaviour : Tela {
        protected override string CaminhoTemplate => "Telas/MenuPrincipal/MenuPrincipalTemplate.uxml";
        protected override string CaminhoStyle => "Telas/MenuPrincipal/MenuPrincipalStyle.uss";

        #region .: Elementos :.

        private const string NOME_BOTAO_CARREGAR_SECAO_CENARIO = "botao-carregar-secao-cenario";
        private Button botaoCarregarSecaoCenario;

        private const string NOME_BOTAO_CARREGAR_SECAO_PERSONAGEM = "botao-carregar-secao-personagem";
        private Button botaoCarregarSecaoPersonagem;

        private const string NOME_BOTAO_CARREGAR_SECAO_APOIO = "botao-carregar-secao-apoio";
        private Button botaoCarregarSecaoApoio;

        private const string NOME_BOTAO_CARREGAR_SECAO_REFORCO = "botao-carregar-secao-reforco";
        private Button botaoCarregarSecaoReforco;

        private const string NOME_BOTAO_CARREGAR_SECAO_OBJETO_INTERACAO = "botao-carregar-secao-objeto-interacao";
        private Button botaoCarregarSecaoObjetoIteracao;

        private const string NOME_BOTAO_CARREGAR_SECAO_INSTRUCAO = "botao-carregar-secao-instrucao";
        private Button botaoCarregarSecaoInstrucao;

        private const string NOME_BOTAO_CARREGAR_SECAO_INFORMACOES_FASE = "botao-carregar-secao-informacoes-fase";
        private Button botaoCarregarSecaoInformacoesFase;


        #endregion

        public MenuPrincipalBehaviour() {
            ConfigurarBotoesCarregamentoSecoes();
            return;
        }

        private void ConfigurarBotoesCarregamentoSecoes() {
            botaoCarregarSecaoCenario = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_CENARIO);
            botaoCarregarSecaoCenario.clicked += HandleBotaoCarregarSecaoCenarioClick;

            botaoCarregarSecaoPersonagem = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_PERSONAGEM);
            botaoCarregarSecaoPersonagem.clicked += HandleBotaoCarregarSecaoPersonagemClick;

            botaoCarregarSecaoReforco = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_REFORCO);
            botaoCarregarSecaoReforco.clicked += HandleBotaoCarregarSecaoReforcoClick;

            botaoCarregarSecaoApoio = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_APOIO);
            botaoCarregarSecaoApoio.clicked += HandleBotaoCarregarSecaoApoioClick;

            botaoCarregarSecaoObjetoIteracao = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_OBJETO_INTERACAO);
            botaoCarregarSecaoObjetoIteracao.clicked += HandleBotaoCarregarSecaoObjetoInteracaoClick;

            botaoCarregarSecaoInstrucao = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_INSTRUCAO);
            botaoCarregarSecaoInstrucao.clicked += HandleBotaoCarregarSecaoInstrucaoClick;

            botaoCarregarSecaoInformacoesFase = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_INFORMACOES_FASE);
            botaoCarregarSecaoInformacoesFase.clicked += HandleBotaoCarregarSecaoInformacoesFaseClick;

            return;
        }

        private void HandleBotaoCarregarSecaoCenarioClick() {
            Navigator.Instance.IrPara(new CriadorCenarioBehaviour());
            return;
        }

        private void HandleBotaoCarregarSecaoPersonagemClick() {
            Navigator.Instance.IrPara(new CriadorPersonagemBehaviour());
            return;
        }

        private void HandleBotaoCarregarSecaoReforcoClick() {
            Navigator.Instance.IrPara(new CriadorReforcoBehaviour());
            return;
        }

        private void HandleBotaoCarregarSecaoApoioClick() {
            Navigator.Instance.IrPara(new CriadorApoioBehaviour());
            return;
        }

        private void HandleBotaoCarregarSecaoObjetoInteracaoClick() {
            Navigator.Instance.IrPara(new CriadorObjetoInteracaoBehaviour());
            return;
        }

        private void HandleBotaoCarregarSecaoInstrucaoClick() {
            Navigator.Instance.IrPara(new CriadorInstrucoesBehaviour());
            return;
        }

        private void HandleBotaoCarregarSecaoInformacoesFaseClick() {
            Navigator.Instance.IrPara(new InformacoesCenaBehaviour());
            return;
        }
    }
}