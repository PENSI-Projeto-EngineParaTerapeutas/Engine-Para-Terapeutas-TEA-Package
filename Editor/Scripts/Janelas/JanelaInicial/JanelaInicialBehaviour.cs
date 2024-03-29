using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.SceneManagement;
using Autis.Editor.Constantes;
using Autis.Editor.UI;
using Autis.Editor.Utils;
using Autis.Runtime.ScriptableObjects;
using Autis.Runtime.Constantes;

namespace Autis.Editor.Telas {
    public class JanelaInicialBehaviour : JanelaEditor {
        protected override string CaminhoTemplate => "Janelas/JanelaInicial/JanelaInicialTemplate.uxml";
        protected override string CaminhoStyle => "Janelas/JanelaInicial/JanelaInicialStyle.uss";

        #region .: Mensagens :.

        protected const string MENSAGEM_TOOLTIP_CAMPO_NOME = "Digite o nome do jogo.";
        protected const string MENSAGEM_AVISO_NOME_JOGO_NAO_DEFINIDO = "Defina um nome para o jogo no campo de \"Nome do jogo\" antes de clicar em \"Baixar Jogo\".";

        #endregion

        #region .: Elementos :.

        private const string NOME_REGIAO_AVISO_LISTA_CENAS_VAZIA = "aviso-lista-cenas-vazia";
        private VisualElement regiaoAvisoListaCenasVazia;

        private const string NOME_REGIAO_LISTA_CENAS = "lista-cenas";
        private VisualElement grupoListaCenas;

        private const string NOME_LABEL_DESCRICAO = "label-descricao-compilar";
        private Label labelDescricao;

        private const string NOME_BOTAO_BAIXAR_JOGO = "botao-baixar-jogo";
        private Button botaoBaixarJogo;

        private const string NOME_BOTAO_CRIAR_CENA = "botao-criar-cena";
        private Button botaoCriarCena;

        private const string NOME_INPUT_NOME_JOGO = "input-nome-jogo";
        private TextField inputNomeJogo;

        #endregion

        private List<Cena> cenas = new();

        [MenuItem("AUTIS/Janela Inicial")]
        public static void ShowJanelaInicial() {
            const string TITULO = "Janela Inicial";

            JanelaInicialBehaviour janela = GetWindow<JanelaInicialBehaviour>();
            janela.titleContent = new GUIContent(TITULO);

            return;
        }

        protected override void OnRenderizarInterface() {
            ConfigurarInputNomeJogo();
            ConfigurarBotaoBaixarJogo();
            ConfigurarListaCenas();
            ConfigurarBotaoCriarCena();
            ConfigurarTooltipCampoNome();

            AlterarVisibilidadeListaCenas();

            return;
        }

        private void ConfigurarInputNomeJogo() {
            inputNomeJogo = root.Query<TextField>(NOME_INPUT_NOME_JOGO);
            return;
        }

        private void ConfigurarBotaoBaixarJogo() {
            botaoBaixarJogo = root.Query<Button>(NOME_BOTAO_BAIXAR_JOGO);
            botaoBaixarJogo.clicked += HandleBotaoBaixarJogoClick;
            return;
        }

        private void HandleBotaoBaixarJogoClick() {
            if(string.IsNullOrEmpty(inputNomeJogo.value)) {
                PopupAvisoBehaviour.ShowPopupAviso(MENSAGEM_AVISO_NOME_JOGO_NAO_DEFINIDO);
                return;
            }

            // TODO: Chamar m�todo para compilar o jogo na plataforma atualmente selecionada
            Debug.Log("[LOG]: TODO: Implementar");

            return;
        }

        private void ConfigurarTooltipCampoNome() {
            const string NOME_REGIAO_LABEL_INPUT_NOME = "regiao-label-nome-jogo";
            const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_CAMPO_NOME = "regiao-carregamento-tooltip-nome-jogo";

            Tooltip tooltipCampoNome = new(MENSAGEM_TOOLTIP_CAMPO_NOME);

            VisualElement regiaoCarregamentoTooltipCampoNome = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_CAMPO_NOME);
            regiaoCarregamentoTooltipCampoNome.Add(tooltipCampoNome.Root);

            VisualElement regiaoCarregamentoLabelCampoNome = root.Query<VisualElement>(NOME_REGIAO_LABEL_INPUT_NOME);
            regiaoCarregamentoLabelCampoNome.Add(regiaoCarregamentoTooltipCampoNome);

            return;
        }

        private void ConfigurarListaCenas() {
            grupoListaCenas = root.Query<VisualElement>(NOME_REGIAO_LISTA_CENAS);
            regiaoAvisoListaCenasVazia = root.Query<VisualElement>(NOME_REGIAO_AVISO_LISTA_CENAS_VAZIA);
            
            labelDescricao = root.Query<Label>(NOME_LABEL_DESCRICAO);

            if(GerenciadorCenas.QuantidadeCenas <= 0) {
                OcultarCamposSemCena();
                return;
            }

            ExibirCamposCena();
            cenas = GerenciadorCenas.GetTodasCenasCriadas();

            foreach(Cena cena in cenas) {
                AdicionarDisplayCena(cena);
            }

            return;
        }

        private void OcultarCamposSemCena() {
            labelDescricao.SetEnabled(false);
            labelDescricao.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            botaoBaixarJogo.SetEnabled(false);

            return;
        }

        private void ExibirCamposCena() {
            labelDescricao.SetEnabled(true);
            labelDescricao.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            botaoBaixarJogo.SetEnabled(true);

            return;
        }

        private void AdicionarDisplayCena(Cena cena) {
            DisplayInformacoesCena informacoes = new(cena);
            informacoes.Root.name = cena.nomeExibicao;
            informacoes.CallbackExcluirCena = HandleExclusaoCena;

            grupoListaCenas.Add(informacoes.Root);

            return;
        }

        private void HandleExclusaoCena(DisplayInformacoesCena displayCena) {
            cenas.Remove(displayCena.InformacoesCena);
            List<VisualElement> elementos = grupoListaCenas.Children().ToList();

            foreach(VisualElement elemento in elementos) {
                if(elemento.name == displayCena.Root.name) {
                    grupoListaCenas.Remove(elemento);
                }
            }

            AlterarVisibilidadeListaCenas();
            return;
        }

        private void AlterarVisibilidadeListaCenas() {
            if(cenas.Count <= 0) {
                grupoListaCenas.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                regiaoAvisoListaCenasVazia.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                
                OcultarCamposSemCena();
                
                return;
            }

            grupoListaCenas.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoAvisoListaCenasVazia.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            ExibirCamposCena();

            return;
        }

        private Image CriarIconeMais() {
            return new() {
                image = Importador.ImportarImagem("mais.png"),
            };
        }

        private void ConfigurarBotaoCriarCena() {
            botaoCriarCena = root.Query<Button>(NOME_BOTAO_CRIAR_CENA);
            botaoCriarCena.clicked += HandleClickBotaoCriarCena;

            botaoCriarCena.Insert(0, CriarIconeMais());
            
            return;
        }

        private void HandleClickBotaoCriarCena() {
            Cena novaCena = GerenciadorCenas.CriarCena();
            cenas.Add(novaCena);

            AdicionarDisplayCena(novaCena);
            AlterarVisibilidadeListaCenas();
            ExibirCamposCena();

            EditorSceneManager.OpenScene(Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsCenas, novaCena.nomeArquivo + ExtensoesEditor.Cena));
            LayoutLoader.CarregarTelaEditor();

            return;
        }
    }
}