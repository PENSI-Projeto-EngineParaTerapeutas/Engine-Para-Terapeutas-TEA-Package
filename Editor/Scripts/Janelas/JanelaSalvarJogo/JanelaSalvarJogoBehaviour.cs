using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Autis.Editor.Telas {
    public class JanelaSalvarJogoBehaviour : JanelaEditor {
        protected override string CaminhoTemplate => "Janelas/JanelaSalvarJogo/JanelaSalvarJogoTemplate.uxml";
        protected override string CaminhoStyle => "Janelas/JanelaSalvarJogo/JanelaSalvarJogoStyle.uss";

        #region .: Elementos :.

        private const string NOME_BOTAO_SALVAR_JOGO = "botao-salvar-jogo";
        private Button botaoSalvarJogo;

        #endregion

        [MenuItem("AUTIS/Janela Salvar Jogo")]
        public static void ShowJanelaSalvarJogo() {
            string titulo = string.Empty;

            JanelaSalvarJogoBehaviour janela = GetWindow<JanelaSalvarJogoBehaviour>();
            janela.titleContent = new GUIContent(titulo);

            return;
        }

        protected override void OnRenderizarInterface() {
            ConfigurarBotaoSalvarJogo();
            return;
        }

        private void ConfigurarBotaoSalvarJogo() {
            botaoSalvarJogo = root.Query<Button>(NOME_BOTAO_SALVAR_JOGO);
            botaoSalvarJogo.clicked += HandleBotaoSalvarJogoClick;

            return;
        }

        private void HandleBotaoSalvarJogoClick() {
            Debug.Log("[TODO]: Implementar");
            return;
        }
    }
}