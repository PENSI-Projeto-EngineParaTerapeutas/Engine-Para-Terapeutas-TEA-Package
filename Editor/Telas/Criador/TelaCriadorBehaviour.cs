using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.Criadores;
using EngineParaTerapeutas.Utils;

namespace EngineParaTerapeutas.Telas {
    public class TelaCriadorBehaviour : TelaEditor {
        private const string TITULO = "Criação de Atores";

        #region .: Elementos :.

        private const string NOME_REGIAO_BOTOES_CARREGAM_SECOES = "grupo-secoes-carregaveis";
        private VisualElement grupoBotoesCarregamSecoes;

        private const string NOME_BOTAO_CARREGAR_SECAO_CENARIO = "botao-carregar-secao-cenario";
        private Button botaoCarregarSecaoCenario;

        private const string NOME_BOTAO_CARREGAR_SECAO_PERSONAGEM = "botao-carregar-secao-personagem";
        private Button botaoCarregarSecaoPersonagem;

        private const string NOME_BOTAO_CARREGAR_SECAO_APOIO = "botao-carregar-secao-apoio";
        private Button botaoCarregarSecaoApoio;

        private const string NOME_BOTAO_CARREGAR_SECAO_REFORCO = "botao-carregar-secao-reforco";
        private Button botaoCarregarSecaoReforco;

        private const string NOME_REGIAO_CARREGAMENTO = "regiao-carregamento-criadores";
        private VisualElement regiaoCarregamento;

        private const string NOME_GRUPO_BOTOES_CONFIRMACAO = "grupo-botoes-confirmacao";
        private VisualElement grupoBotoesConfirmacao;

        private const string NOME_BOTAO_CONFIRMAR_CRIACAO = "confimar-criacao";
        private Button confirmarCricao;

        private const string NOME_BOTAO_CANCELAR_CRIACAO = "cancelar-criacao";
        private Button cancelarCricao;

        private CriadorCenarioBehaviour criadorCenario;
        private CriadorPersonagemBehaviour criadorPersonagem;
        private CriadorApoioBehaviour criadorApoio;
        private CriadorReforcoBehaviour criadorReforco;

        #endregion

        private Criador criadorAtual = null;

        [MenuItem("Engine Para Terapeutas/Tela Criação de Atores")]
        public static void ShowCriador() {
            TelaCriadorBehaviour janela = GetWindow<TelaCriadorBehaviour>();
            janela.titleContent = new GUIContent(TITULO);

            return;
        }

        public void OnEnable() {
            CarregarCriadores();
            return;
        }

        private void CarregarCriadores() {
            criadorCenario = new CriadorCenarioBehaviour();
            criadorPersonagem = new CriadorPersonagemBehaviour();
            criadorApoio = new CriadorApoioBehaviour();
            criadorReforco = new CriadorReforcoBehaviour();

            return;
        }

        public override void CreateGUI() {
            base.CreateGUI();

            ImportarTemplate("Telas/Criador/TelaCriadorTemplate.uxml");
            ImportarStyle("Telas/Criador/TelaCriadorStyle.uss");

            ConfigurarElementos();

            return;
        }

        private void ConfigurarElementos() {
            ConfigurarBotoesCarregamentoSecoes();
            ConfigurarSecaoCarregamento();
            ConfigurarBotoesConfirmacao();

            return;
        }

        private void ConfigurarBotoesCarregamentoSecoes() {
            grupoBotoesCarregamSecoes = root.Query<VisualElement>(NOME_REGIAO_BOTOES_CARREGAM_SECOES);

            botaoCarregarSecaoCenario = root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_CENARIO);
            botaoCarregarSecaoCenario.clicked += HandleBotaoCarregarSecaoCenarioClick;

            botaoCarregarSecaoPersonagem = root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_PERSONAGEM);
            botaoCarregarSecaoPersonagem.clicked += HandleBotaoCarregarSecaoPersonagemClick;

            botaoCarregarSecaoReforco = root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_REFORCO);
            botaoCarregarSecaoReforco.clicked += HandleBotaoCarregarSecaoReforcoClick;

            botaoCarregarSecaoApoio = root.Query<Button>(NOME_BOTAO_CARREGAR_SECAO_APOIO);
            botaoCarregarSecaoApoio.clicked += HandleBotaoCarregarSecaoApoioClick;

            return;
        }

        private void HandleBotaoCarregarSecaoCenarioClick() {
            if(criadorAtual != null) {
                return;
            }

            criadorAtual = criadorCenario;
            CarregarCriador(criadorCenario);
            return;
        }

        private void CarregarCriador(Criador criador) {
            regiaoCarregamento.Clear();
            regiaoCarregamento.Add(criador.Root);

            grupoBotoesCarregamSecoes.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            grupoBotoesConfirmacao.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            criador.IniciarCriacao();
            return;
        }

        private void HandleBotaoCarregarSecaoPersonagemClick() {
            if(criadorAtual != null) {
                return;
            }

            criadorAtual = criadorPersonagem;
            CarregarCriador(criadorPersonagem);
            return;
        }

        private void HandleBotaoCarregarSecaoReforcoClick() {
            if(criadorAtual != null) {
                return;
            }

            criadorAtual = criadorReforco;
            CarregarCriador(criadorReforco);
            return;
        }

        private void HandleBotaoCarregarSecaoApoioClick() {
            if(criadorAtual != null) {
                return;
            }

            criadorAtual = criadorApoio;
            CarregarCriador(criadorApoio);
            return;
        }

        private void ConfigurarSecaoCarregamento() {
            regiaoCarregamento = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO);
            return;
        }

        private void ConfigurarBotoesConfirmacao() {
            grupoBotoesConfirmacao = root.Query<VisualElement>(NOME_GRUPO_BOTOES_CONFIRMACAO);
            grupoBotoesConfirmacao.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            confirmarCricao = root.Query<Button>(NOME_BOTAO_CONFIRMAR_CRIACAO);
            confirmarCricao.clicked += HandleConfimarCricaoClick;

            cancelarCricao = root.Query<Button>(NOME_BOTAO_CANCELAR_CRIACAO);
            cancelarCricao.clicked += HandleCancelarCricaoClick;

            return;
        }

        private void HandleConfimarCricaoClick() {
            Salvamento.SalvarCenas();

            criadorAtual.FinalizarCriacao();
            ReiniciarEstado();

            return;
        }

        private void ReiniciarEstado() {
            criadorAtual = null;

            regiaoCarregamento.Clear();

            grupoBotoesConfirmacao.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            grupoBotoesCarregamSecoes.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        private void HandleCancelarCricaoClick() {
            criadorAtual.CancelarCriacao();
            ReiniciarEstado();

            return;
        }

        public void OnDestroy() {
            criadorAtual = null;

            criadorCenario.CancelarCriacao();
            criadorPersonagem.CancelarCriacao();
            criadorApoio.CancelarCriacao();

            return;
        }
    }
}