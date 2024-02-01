using System;
using System.IO;
using UnityEngine.UIElements;
using UnityEditor.SceneManagement;
using Autis.Runtime.Constantes;
using Autis.Runtime.ScriptableObjects;
using Autis.Editor.Constantes;
using Autis.Editor.Utils;

namespace Autis.Editor.UI {
    public class DisplayInformacoesCena : ElementoInterfaceEditor {
        protected override string CaminhoTemplate => "Janelas/JanelaInicial/DisplayInformacoesCena/DisplayInformacoesCenaTemplate.uxml";
        protected override string CaminhoStyle => "Janelas/JanelaInicial/DisplayInformacoesCena/DisplayInformacoesCenaStyle.uss";

        #region .: Elementos :.

        public Label LabelNome { get => labelNome; }
        public Label LabelDificuldade { get => labelDificuldade; }
        public Label LabelFaixaEtaria { get => labelFaixaEtaria; }
        public Button BotaoAbrirCena { get => botaoAbrirCena; }
        public Button BotaoExcluirCena { get => botaoExcluirCena; }

        private const string NOME_LABEL_NOME_CENA = "nome-cena";
        private readonly Label labelNome;

        private const string NOME_LABEL_DIFICULDADE_CENA = "nivel-dificuldade";
        private readonly Label labelDificuldade;

        private const string NOME_LABEL_FAIXA_ETARIA_CENA = "faixa-etaria";
        private readonly Label labelFaixaEtaria;

        private const string NOME_BOTAO_ABRIR_CENA = "botao-abrir-cena";
        private Button botaoAbrirCena;

        private const string NOME_BOTAO_EXCLUIR_CENA = "botao-excluir-cena";
        private Button botaoExcluirCena;

        public Action<DisplayInformacoesCena> CallbackExcluirCena { set; get; } = null;

        #endregion

        public Cena InformacoesCena { get => informacoesCena; }

        private readonly Cena informacoesCena;

        public DisplayInformacoesCena(Cena cena) {
            informacoesCena = cena;

            labelNome = Root.Query<Label>(NOME_LABEL_NOME_CENA);
            //labelDificuldade = Root.Query<Label>(NOME_LABEL_DIFICULDADE_CENA);
            //labelFaixaEtaria = Root.Query<Label>(NOME_LABEL_FAIXA_ETARIA_CENA);

            botaoAbrirCena = Root.Query<Button>(NOME_BOTAO_ABRIR_CENA);
            botaoExcluirCena = Root.Query<Button>(NOME_BOTAO_EXCLUIR_CENA);

            ConfigurarLabels();
            ConfigurarBotoes();

            return;
        }

        private void ConfigurarLabels() {
            labelNome.text = informacoesCena.nomeExibicao;
            //labelDificuldade.text = informacoesCena.nivelDificuldade.ToString();
            //labelFaixaEtaria.text = informacoesCena.faixaEtaria.ToString();

            return;
        }

        private Image CriarIconeEditar() {
            return new() {
                image = Importador.ImportarImagem("icone-editar.png"),
            };
        }

        private Image CriarIconeLixeira() {
            return new() {
                image = Importador.ImportarImagem("icone-lixeira.png"),
            };
        }

        private void ConfigurarBotoes() {
            botaoAbrirCena.clicked += HandleClickBotaoAbrirCena;
            botaoAbrirCena.Insert(0, CriarIconeEditar());

            botaoExcluirCena.clicked += HandleClickBotaoExcluirCena;
            botaoExcluirCena.Insert(0, CriarIconeLixeira());

            return;
        }

        private void HandleClickBotaoAbrirCena() {
            EditorSceneManager.OpenScene(Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsCenas, informacoesCena.nomeArquivo + ExtensoesEditor.Cena));
            LayoutLoader.CarregarTelaEditor();
            return;
        }

        private void HandleClickBotaoExcluirCena() {
            GerenciadorCenas.DeletarCena(informacoesCena);
            CallbackExcluirCena?.Invoke(this);

            return;
        }
    }
}