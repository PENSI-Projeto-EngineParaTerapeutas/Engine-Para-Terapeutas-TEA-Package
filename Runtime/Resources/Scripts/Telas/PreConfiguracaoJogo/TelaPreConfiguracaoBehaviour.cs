using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.UI;
using EngineParaTerapeutas.Eventos;

namespace EngineParaTerapeutas.Telas {
    public class TelaPreConfiguracaoBehaviour : TelaJogo {
        private enum AbasTelaPreConfiguracao {
            Nenhuma,
            ConfigurarCenario,
            ConfigurarPersonagem,
            ConfigurarApoios,
            ConfigurarReforcos,
            ConfigurarObjetosInteracao,
            ConfigurarInstrucoes,
        }

        #region .: Elementos :.

        private const string NOME_REGIAO_BOTOES_CARREGAM_SECOES_CONFIGURACAO = "grupo-secoes-carregaveis";
        private VisualElement grupoBotoesCarregamSecoes;

        private const string NOME_BOTAO_CARREGAR_SECAO_CONFIGURACAO_CENARIO = "botao-carregar-secao-configuracao-cenario";
        private Button botaoCarregarSecaoCenario;

        private const string NOME_BOTAO_CARREGAR_SECAO_CONFIGURACAO_PERSONAGEM = "botao-carregar-secao-configuracao-personagem";
        private Button botaoCarregarSecaoPersonagem;

        private const string NOME_BOTAO_CARREGAR_SECAO_CONFIGURACAO_APOIO = "botao-carregar-secao-configuracao-apoio";
        private Button botaoCarregarSecaoApoio;

        private const string NOME_BOTAO_CARREGAR_SECAO_CONFIGURACAO_REFORCO = "botao-carregar-secao-configuracao-reforco";
        private Button botaoCarregarSecaoReforco;

        private const string NOME_BOTAO_CARREGAR_SECAO_CONFIGURACAO_OBJETO_INTERACAO = "botao-carregar-secao-configuracao-objeto-interacao";
        private Button botaoCarregarSecaoObjetoInteracao;

        private const string NOME_BOTAO_CARREGAR_SECAO_CONFIGURACAO_INSTRUCOES = "botao-carregar-secao-configuracao-instrucoes";
        private Button botaoCarregarSecaoInstrucoes;

        private const string NOME_REGIAO_CARREGAMENTO = "regiao-configuracao-atual";
        private VisualElement regiaoCarregamento;

        private const string NOME_BOTAO_INICIAR_JOGO = "botao-iniciar-jogo";
        private Button botaoIniciarJogo;

        private ConfiguracaoCenarioBehaviour secaoConfiguracaoCenario;
        private ConfiguracaoApoioBehaviour secaoConfiguracaoApoios;
        private ConfiguracaoReforcoBehaviour secaoConfiguracaoReforcos;
        private ConfiguracaoInstrucao secaoConfiguracaoInstrucoes;
        private ConfiguracaoObjetoInteracao secaoConfiguracaoObjetoInteracao;

        #endregion

        AbasTelaPreConfiguracao abaAtual = AbasTelaPreConfiguracao.Nenhuma;

        private EventoJogo eventoExibirContextualizacao;

        private void Awake() {
            Root.styleSheets.Add(style);

            eventoExibirContextualizacao = Resources.Load<EventoJogo>("ScriptableObjects/EventoApresentarContexto");

            secaoConfiguracaoCenario = new ConfiguracaoCenarioBehaviour();
            secaoConfiguracaoApoios = new ConfiguracaoApoioBehaviour();
            secaoConfiguracaoReforcos = new ConfiguracaoReforcoBehaviour();
            secaoConfiguracaoInstrucoes = new ConfiguracaoInstrucao();
            secaoConfiguracaoObjetoInteracao = new ConfiguracaoObjetoInteracao();

            return;
        }

        private void Start() {
            CarregarElementos();
            return;
        }

        private void CarregarElementos() {
            grupoBotoesCarregamSecoes = Root.Query<VisualElement>(NOME_REGIAO_BOTOES_CARREGAM_SECOES_CONFIGURACAO);
            regiaoCarregamento = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO);

            ConfigurarBotaoCarregaregamento();
            ConfigurarBotaoIniciarJogo();

            return;
        }

        private void ConfigurarBotaoCarregaregamento() {
            botaoCarregarSecaoApoio = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_CONFIGURACAO_APOIO);
            botaoCarregarSecaoApoio.clicked += HandleBotaoCarregarSecaoApoio;

            botaoCarregarSecaoCenario = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_CONFIGURACAO_CENARIO);
            botaoCarregarSecaoCenario.clicked += HandleBotaoCarregarSecaoCenario;

            botaoCarregarSecaoPersonagem = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_CONFIGURACAO_PERSONAGEM);
            botaoCarregarSecaoPersonagem.clicked += HandleBotaoCarregarSecaoPersonagem;
        
            botaoCarregarSecaoReforco = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_CONFIGURACAO_REFORCO);
            botaoCarregarSecaoReforco.clicked += HandleBotaoCarregarSecaoReforco;

            botaoCarregarSecaoObjetoInteracao = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_CONFIGURACAO_OBJETO_INTERACAO);
            botaoCarregarSecaoObjetoInteracao.clicked += HandleBotaoCarregarSecaoObjetoInteracao;

            botaoCarregarSecaoInstrucoes = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_CONFIGURACAO_INSTRUCOES);
            botaoCarregarSecaoInstrucoes.clicked += HandleBotaoCarregarSecaoInstrucoes;
            return;
        }

        private void HandleBotaoCarregarSecaoApoio() {
            if(abaAtual == AbasTelaPreConfiguracao.ConfigurarApoios) {
                return;
            }

            abaAtual = AbasTelaPreConfiguracao.ConfigurarApoios;
            CarregarSecaoConfiguracao(secaoConfiguracaoApoios);

            return;
        }

        private void CarregarSecaoConfiguracao(ElementoInterface secao) {
            regiaoCarregamento.Clear();
            regiaoCarregamento.Add(secao.Root);

            return;
        }

        private void HandleBotaoCarregarSecaoCenario() {
            if(abaAtual == AbasTelaPreConfiguracao.ConfigurarCenario) {
                return;
            }

            abaAtual = AbasTelaPreConfiguracao.ConfigurarCenario;
            CarregarSecaoConfiguracao(secaoConfiguracaoCenario);

            return;
        }

        private void HandleBotaoCarregarSecaoPersonagem() {
            if(abaAtual == AbasTelaPreConfiguracao.ConfigurarPersonagem) {
                return;
            }

            abaAtual = AbasTelaPreConfiguracao.ConfigurarPersonagem;
            regiaoCarregamento.Clear();
            Debug.Log("[LOG]: Carregar configurações do personagem");

            return;
        }

        private void HandleBotaoCarregarSecaoReforco() {
            if(abaAtual == AbasTelaPreConfiguracao.ConfigurarReforcos) {
                return;
            }

            abaAtual = AbasTelaPreConfiguracao.ConfigurarReforcos;
            CarregarSecaoConfiguracao(secaoConfiguracaoReforcos);

            return;
        }

        private void HandleBotaoCarregarSecaoObjetoInteracao() {
            if(abaAtual == AbasTelaPreConfiguracao.ConfigurarObjetosInteracao) {
                return;
            }

            abaAtual = AbasTelaPreConfiguracao.ConfigurarObjetosInteracao;
            CarregarSecaoConfiguracao(secaoConfiguracaoObjetoInteracao);

            return;
        }

        private void HandleBotaoCarregarSecaoInstrucoes() {
            if(abaAtual == AbasTelaPreConfiguracao.ConfigurarInstrucoes) {
                return;
            }

            abaAtual = AbasTelaPreConfiguracao.ConfigurarInstrucoes;
            CarregarSecaoConfiguracao(secaoConfiguracaoInstrucoes);

            return;
        }

        private void ConfigurarBotaoIniciarJogo() {
            botaoIniciarJogo = Root.Query<Button>(NOME_BOTAO_INICIAR_JOGO);
            botaoIniciarJogo.clicked += HandleBotaoIniciarJogo;
            return;
        }

        private void HandleBotaoIniciarJogo() {
            eventoExibirContextualizacao.AcionarCallbacks();
            gameObject.SetActive(false);
            
            return;
        }
    }
}