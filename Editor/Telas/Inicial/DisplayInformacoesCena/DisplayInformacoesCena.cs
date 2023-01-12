using System;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.SceneManagement;
using EngineParaTerapeutas.ScriptableObjects;
using EngineParaTerapeutas.Utils;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.UI {
    public class DisplayInformacoesCena : ElementoInterfaceEditor {
        #region .: Elementos :.

        public Label LabelNome { get => labelNome; }
        public Label LabelDificuldade { get => labelDificuldade; }
        public Label LabelFaixaEtaria { get => labelFaixaEtaria; }
        public Button BotaoAbrirCena { get => botaoAbrirCena; }
        public Button BotaoEditarCena { get => botaoEditarCena; }
        public Button BotaoExcluirCena { get => botaoExcluirCena; }

        private const string NOME_LABEL_NOME_CENA = "nome-cena";
        private readonly Label labelNome;

        private const string NOME_LABEL_DIFICULDADE_CENA = "nivel-dificuldade";
        private readonly Label labelDificuldade;

        private const string NOME_LABEL_FAIXA_ETARIA_CENA = "faixa-etaria";
        private readonly Label labelFaixaEtaria;

        private const string NOME_BOTAO_ABRIR_CENA = "botao-abrir-cena";
        private readonly Button botaoAbrirCena;

        private const string NOME_BOTAO_EDITAR_CENA = "botao-editar-cena";
        private readonly Button botaoEditarCena;

        private const string NOME_BOTAO_EXCLUIR_CENA = "botao-excluir-cena";
        private readonly Button botaoExcluirCena;

        public Action<DisplayInformacoesCena> CallbackExcluirCena { set; get; } = null;

        #endregion

        public Cena InformacoesCena { get => informacoesCena; }
        private readonly Cena informacoesCena;

        public DisplayInformacoesCena(Cena cena) {
            informacoesCena = cena;

            ImportarTemplate("Telas/Inicial/DisplayInformacoesCena/DisplayInformacoesCenaStyle.uxml");
            ImportarStyle("Telas/Inicial/DisplayInformacoesCena/DisplayInformacoesCenaTemplate.uss");

            labelNome = Root.Query<Label>(NOME_LABEL_NOME_CENA);
            labelDificuldade = Root.Query<Label>(NOME_LABEL_DIFICULDADE_CENA);
            labelFaixaEtaria = Root.Query<Label>(NOME_LABEL_FAIXA_ETARIA_CENA);

            botaoAbrirCena = Root.Query<Button>(NOME_BOTAO_ABRIR_CENA);
            botaoEditarCena = Root.Query<Button>(NOME_BOTAO_EDITAR_CENA);
            botaoExcluirCena = Root.Query<Button>(NOME_BOTAO_EXCLUIR_CENA);

            ConfigurarLabels();
            ConfigurarBotoes();

            return;
        }

        private void ConfigurarLabels() {
            labelNome.text = informacoesCena.Nome;
            labelDificuldade.text = informacoesCena.NivelDificuldade.ToString();
            labelFaixaEtaria.text = informacoesCena.FaixaEtaria.ToString();

            return;
        }

        private void ConfigurarBotoes() {
            botaoAbrirCena.clicked += HandleClickBotaoAbrirCena;
            botaoEditarCena.clicked += HandleClickBotaoEditarCena;
            botaoExcluirCena.clicked += HandleClickBotaoExcluirCena;

            return;
        }

        private void HandleClickBotaoAbrirCena() {
            EditorSceneManager.OpenScene(Path.Combine(ConstantesProjeto.PastaCenasAssets, informacoesCena.Nome + Extensoes.Cena));
            LayoutLoader.CarregarTelaEditor();
            return;
        }

        private void HandleClickBotaoEditarCena() {
            Debug.Log("Editar cena: " + informacoesCena.Nome);
            return;
        }

        private void HandleClickBotaoExcluirCena() {
            GerenciadorCenas.DeletarCena(informacoesCena);

            if(CallbackExcluirCena != null) {
                CallbackExcluirCena.Invoke(this);
            }
            return;
        }
    }
}