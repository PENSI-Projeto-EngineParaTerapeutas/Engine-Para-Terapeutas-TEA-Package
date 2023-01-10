using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.Telas {
    public class TelaPreConfiguracaoBehaviour : MonoBehaviour {
        private enum AbasTelaPreConfiguracao {
            Nenhuma,
            ConfigurarCenario,
            ConfigurarPersonagem,
            ConfigurarApoios,
            ConfigurarReforcos,
        }

        private UIDocument template;

        #region .: Elementos :.

        private VisualElement root;
        private StyleSheet style;

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

        private const string NOME_REGIAO_CARREGAMENTO = "regiao-configuracao-atual";
        private VisualElement regiaoCarregamento;

        private const string NOME_BOTAO_INICIAR_JOGO = "botao-iniciar-jogo";
        private Button botaoIniciarJogo;

        private ConfiguracaoCenarioBehaviour secaoConfiguracaoCenario;
        private ConfiguracaoApoioBehaviour secaoConfiguracaoApoios;
        private ConfiguracaoReforcoBehaviour secaoConfiguracaoReforcos;

        #endregion

        AbasTelaPreConfiguracao abaAtual = AbasTelaPreConfiguracao.Nenhuma;

        public void Awake() {
            ImportarTemplate();
            ImportarStyle();

            secaoConfiguracaoCenario = new();
            secaoConfiguracaoApoios = new();
            secaoConfiguracaoReforcos = new();

            return;
        }

        private void ImportarTemplate() {
            template = GetComponent<UIDocument>();
            root = template.rootVisualElement;

            return;
        }

        private void ImportarStyle() {
            style = Resources.Load<StyleSheet>("Scripts/Telas/PreConfiguracaoJogo/TelaPreConfiguracaoStyle");
            root.styleSheets.Add(style);

            return;
        }

        private void CarregarElementos() {
            grupoBotoesCarregamSecoes = root.Query<VisualElement>(NOME_REGIAO_BOTOES_CARREGAM_SECOES_CONFIGURACAO);
            regiaoCarregamento = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO);

            ConfigurarBotaoCarregaregamento();
            ConfigurarBotaoIniciarJogo();

            return;
        }

        private void ConfigurarBotaoCarregaregamento() {
            botaoCarregarSecaoApoio = root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_CONFIGURACAO_APOIO);
            botaoCarregarSecaoApoio.clicked += HandleBotaoCarregarSecaoApoio;

            botaoCarregarSecaoCenario = root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_CONFIGURACAO_CENARIO);
            botaoCarregarSecaoCenario.clicked += HandleBotaoCarregarSecaoCenario;

            botaoCarregarSecaoPersonagem = root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_CONFIGURACAO_PERSONAGEM);
            botaoCarregarSecaoPersonagem.clicked += HandleBotaoCarregarSecaoPersonagem;
        
            botaoCarregarSecaoReforco = root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_CONFIGURACAO_REFORCO);
            botaoCarregarSecaoReforco.clicked += HandleBotaoCarregarSecaoReforco;
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

        private void ConfigurarBotaoIniciarJogo() {
            botaoIniciarJogo = root.Query<Button>(NOME_BOTAO_INICIAR_JOGO);
            botaoIniciarJogo.clicked += HandleBotaoIniciarJogo;
            return;
        }

        private void HandleBotaoIniciarJogo() {
            return;
        }

        public void Start() {
            CarregarElementos();
            return;
        }
    }
}