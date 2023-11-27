using UnityEngine;
using UnityEngine.UIElements;
using Autis.Runtime.Constantes;
using Autis.Editor.Criadores;
using Autis.Editor.Utils;
using Autis.Runtime.Eventos;
using UnityEditor;

namespace Autis.Editor.Telas {
    public class MenuPrincipalBehaviour : Tela {
        protected override string CaminhoTemplate => "Telas/MenuPrincipal/MenuPrincipalTemplate.uxml";
        protected override string CaminhoStyle => "Telas/MenuPrincipal/MenuPrincipalStyle.uss";

        #region .: Mensagens :.

        private const string MENSAGEM_AVISO_ATOR_SINGLETON = "Só pode haver um {tipo} por fase do jogo. Para editar o {tipo} já criado, clique na aba Edição.";

        #endregion

        #region .: Eventos :.

        protected static EventoJogo eventoIniciarCriacao;

        #endregion

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

        private const string NOME_REGIAO_AJUDA = "label-ajuda";
        private VisualElement regiaoAjuda;

        private const string NOME_ICONE_AJUDA = "imagem-icone-ajuda";
        private Image iconeAjuda;

        #endregion

        public MenuPrincipalBehaviour() {
            eventoIniciarCriacao = Importador.ImportarEvento("EventoIniciarCriacao");

            ConfigurarBotaoCarregarCenario();
            ConfigurarBotaoCarregarPersonagem();
            ConfigurarBotaoCarregarApoio();
            ConfigurarBotaoCarregarReforco();
            ConfigurarBotaoCarregarElementoInteracao();
            ConfigurarBotaoCarregarInstrucao();
            ConfigurarBotaoCarregarInformacoesFase();

            ConfigurarRegiaoAjuda();

            return;
        }

        public override void OnEditorUpdate() {
            DefinirFerramenta();
            return;
        }

        protected virtual void DefinirFerramenta() {
            if(Tools.current != Tool.Rect) {
                Tools.current = Tool.Rect;
                return;
            }

            return;
        }

        private void ConfigurarBotaoCarregarCenario() {
            botaoCarregarSecaoCenario = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_CENARIO);
            botaoCarregarSecaoCenario.Insert(0, CriarIconeMais());
            botaoCarregarSecaoCenario.clicked += HandleBotaoCarregarSecaoCenarioClick;

            return;
        }

        private Image CriarIconeMais() {
            return new() {
                image = Importador.ImportarImagem("mais.png"),
            };
        }

        private void ConfigurarBotaoCarregarPersonagem() {
            botaoCarregarSecaoPersonagem = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_PERSONAGEM);
            botaoCarregarSecaoPersonagem.Insert(0, CriarIconeMais());
            botaoCarregarSecaoPersonagem.clicked += HandleBotaoCarregarSecaoPersonagemClick;

            return;
        }

        private void ConfigurarBotaoCarregarReforco() {
            botaoCarregarSecaoReforco = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_REFORCO);
            botaoCarregarSecaoReforco.Insert(0, CriarIconeMais());
            botaoCarregarSecaoReforco.clicked += HandleBotaoCarregarSecaoReforcoClick;

            return;
        }

        private void ConfigurarBotaoCarregarApoio() {
            botaoCarregarSecaoApoio = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_APOIO);
            botaoCarregarSecaoApoio.Insert(0, CriarIconeMais());
            botaoCarregarSecaoApoio.clicked += HandleBotaoCarregarSecaoApoioClick;

            return;
        }

        private void ConfigurarBotaoCarregarElementoInteracao() {
            botaoCarregarSecaoObjetoIteracao = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_OBJETO_INTERACAO);
            botaoCarregarSecaoObjetoIteracao.Insert(0, CriarIconeMais());
            botaoCarregarSecaoObjetoIteracao.clicked += HandleBotaoCarregarSecaoObjetoInteracaoClick;

            return;
        }

        private void ConfigurarBotaoCarregarInstrucao() {
            botaoCarregarSecaoInstrucao = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_INSTRUCAO);
            botaoCarregarSecaoInstrucao.Insert(0, CriarIconeMais());
            botaoCarregarSecaoInstrucao.clicked += HandleBotaoCarregarSecaoInstrucaoClick;

            return;
        }

        private void ConfigurarBotaoCarregarInformacoesFase() {
            botaoCarregarSecaoInformacoesFase = Root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_INFORMACOES_FASE);
            botaoCarregarSecaoInformacoesFase.clicked += HandleBotaoCarregarSecaoInformacoesFaseClick;

            return;
        }

        private void ConfigurarRegiaoAjuda() {
            regiaoAjuda = root.Query<VisualElement>(NOME_REGIAO_AJUDA);
            iconeAjuda = root.Query<Image>(NOME_ICONE_AJUDA);
            iconeAjuda.image = Importador.ImportarImagem("interrogacao-azul.png");

            return;
        }

        private void HandleBotaoCarregarSecaoCenarioClick() {
            GameObject cenario = GameObject.FindGameObjectWithTag(NomesTags.Cenario);
            if(cenario != null) {
                PopupAvisoBehaviour.ShowPopupAviso(MENSAGEM_AVISO_ATOR_SINGLETON.Replace("{tipo}", "Cenário"));
                return;
            }

            eventoIniciarCriacao.AcionarCallbacks();
            Navigator.Instance.IrPara(new CriadorCenarioBehaviour());
            return;
        }

        private void HandleBotaoCarregarSecaoPersonagemClick() {
            GameObject personagem = GameObject.FindGameObjectWithTag(NomesTags.Personagem);
            if(personagem != null) {
                PopupAvisoBehaviour.ShowPopupAviso(MENSAGEM_AVISO_ATOR_SINGLETON.Replace("{tipo}", "Personagem"));
                return;
            }

            eventoIniciarCriacao.AcionarCallbacks();
            Navigator.Instance.IrPara(new CriadorPersonagemBehaviour());
            return;
        }

        private void HandleBotaoCarregarSecaoReforcoClick() {
            eventoIniciarCriacao.AcionarCallbacks();
            Navigator.Instance.IrPara(new CriadorReforcoBehaviour());
            return;
        }

        private void HandleBotaoCarregarSecaoApoioClick() {
            eventoIniciarCriacao.AcionarCallbacks();
            Navigator.Instance.IrPara(new CriadorApoioBehaviour());
            return;
        }

        private void HandleBotaoCarregarSecaoObjetoInteracaoClick() {
            eventoIniciarCriacao.AcionarCallbacks();
            Navigator.Instance.IrPara(new CriadorObjetoInteracaoBehaviour());
            return;
        }

        private void HandleBotaoCarregarSecaoInstrucaoClick() {
            eventoIniciarCriacao.AcionarCallbacks();
            Navigator.Instance.IrPara(new CriadorInstrucoesBehaviour());
            return;
        }

        private void HandleBotaoCarregarSecaoInformacoesFaseClick() {
            eventoIniciarCriacao.AcionarCallbacks();
            Navigator.Instance.IrPara(new InformacoesCenaBehaviour());
            return;
        }
    }
}