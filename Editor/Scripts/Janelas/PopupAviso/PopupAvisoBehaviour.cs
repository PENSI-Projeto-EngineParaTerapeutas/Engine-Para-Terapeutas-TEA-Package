using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Autis.Editor.Telas {
    public class PopupAvisoBehaviour : JanelaEditor {
        protected override string CaminhoTemplate => "Janelas/PopupAviso/PopupAvisoTemplate.uxml";
        protected override string CaminhoStyle => "Janelas/PopupAviso/PopupAvisoStyle.uss";

        #region .: Elementos :.

        private const string NOME_LABEL_MENSAGEM = "mensagem-aviso";
        private Label labelMensagem;

        private const string NOME_BOTAO_CONFIRMACAO = "botao-confirmacao";
        private Button botaoConfirmacao;

        #endregion

        private static string mensagem = string.Empty;

        [MenuItem("AUTIS/Popup Aviso")]
        public static void ShowPopupAviso() {
            const string TITULO = "AVISO!";

            Vector2 tamanhoJanela = new(600, 250);
            Vector2 posicaoJenela = new((EditorGUIUtility.GetMainWindowPosition().width - tamanhoJanela.x) / 2, (EditorGUIUtility.GetMainWindowPosition().height - tamanhoJanela.y) / 2);

            PopupAvisoBehaviour janela = GetWindow<PopupAvisoBehaviour>();
            janela.titleContent = new GUIContent(TITULO);

            janela.minSize = tamanhoJanela;
            janela.maxSize = tamanhoJanela;
            
            janela.position = new Rect(posicaoJenela, tamanhoJanela);

            eventoAbrirPopupAviso.AcionarCallbacks();

            return;
        }

        public static void ShowPopupAviso(string mensagem) {
            PopupAvisoBehaviour.mensagem = mensagem;

            ShowPopupAviso();

            return;
        }

        protected override void OnRenderizarInterface() {
            ConfigurarTemplateBackground();
            ConfigurarLabel();
            ConfigurarBotaoConfirmacao();

            return;
        }

        private void ConfigurarTemplateBackground() {
            rootVisualElement.Children().First().AddToClassList("regiao-background-popup");
            return;
        }

        private void ConfigurarLabel() {
            labelMensagem = root.Query<Label>(NOME_LABEL_MENSAGEM);
            labelMensagem.text = mensagem;
            
            return;
        }

        private void ConfigurarBotaoConfirmacao() {
            botaoConfirmacao = root.Query<Button>(NOME_BOTAO_CONFIRMACAO);
            botaoConfirmacao.clicked += HandleBotaoConfirmacaoClick;

            return;
        }

        private void OnDisable() {
            eventoFecharPopupAviso.AcionarCallbacks();
            return;
        }

        private void HandleBotaoConfirmacaoClick() {
            eventoFecharPopupAviso.AcionarCallbacks();
            Close();

            return;
        }

        protected override void HandleAbrirPopupAviso() {
            return;
        }

        protected override void HandleFecharPopupAviso() {
            return;
        }
    }
}