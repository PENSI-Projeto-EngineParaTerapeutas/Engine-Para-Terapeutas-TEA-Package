using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.Telas {
    public class PopupSalvarLayoutBehaviour : JanelaEditor {
        protected override string CaminhoTemplate => "Layout/PopupSalvarLayout/PopupSalvarLayoutTemplate.uxml";
        protected override string CaminhoStyle => "Layout/PopupSalvarLayout/PopupSalvarLayoutStyle.uss";

        #region .: Elementos :.

        private const string NOME_INPUT_NOME_ARQUIVO = "input-nome-layout";
        private const string NOME_LABEL_NOME_ARQUIVO = "label-nome-layout";
        private TextField campoNomeArquivo;

        private const string NOME_BOTAO_SALVAR = "botao-salvar";
        private Button salvar;

        private const string NOME_BOTAO_CANCELAR = "botao-cancelar";
        private Button cancelar;

        #endregion

        string nomeArquivo = string.Empty;

        [MenuItem("Desenvolvimento/Salvar Layout Atual")]
        public static void ShowPopupSalvarLayout() {
            const string TITULO = "Salvar Layout";

            PopupSalvarLayoutBehaviour janela = GetWindow<PopupSalvarLayoutBehaviour>();

            janela.titleContent = new GUIContent(TITULO);

            janela.minSize = new Vector2(400, 150);
            janela.maxSize = new Vector2(400, 150);

            return;
        }

        protected override void OnRenderizarInterface() {
            ConfigurarCampoNomeArquivo();
            ConfigurarBotaoSalvar();
            ConfigurarBotaoCancelar();

            return;
        }

        private void ConfigurarCampoNomeArquivo() {
            campoNomeArquivo = root.Query<TextField>(NOME_INPUT_NOME_ARQUIVO);
            campoNomeArquivo.labelElement.name = NOME_LABEL_NOME_ARQUIVO;
            campoNomeArquivo.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            campoNomeArquivo.SetValueWithoutNotify(string.Empty);
            campoNomeArquivo.RegisterCallback<ChangeEvent<string>>(evt => {
                nomeArquivo = evt.newValue;
            });

            return;
        }

        private void ConfigurarBotaoSalvar() {
            salvar = root.Query<Button>(NOME_BOTAO_SALVAR);
            salvar.clicked += HandleBotaoSalvar;

            return;
        }

        private void HandleBotaoSalvar() {
            Close();
            LayoutManager.SalvarLayoutAtual(nomeArquivo);

            return;
        }

        private void ConfigurarBotaoCancelar() {
            cancelar = root.Query<Button>(NOME_BOTAO_CANCELAR);
            cancelar.clicked += HandleBotaoCancelar;

            return;
        }

        private void HandleBotaoCancelar() {
            nomeArquivo = string.Empty;
            Close();

            return;
        }
    }
}