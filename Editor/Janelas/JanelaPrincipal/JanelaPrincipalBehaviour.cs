using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Autis.Editor.Constantes;

namespace Autis.Editor.Telas {
    public class JanelaPrincipalBehaviour : JanelaEditor {
        protected override string CaminhoTemplate => "Janelas/JanelaPrincipal/JanelaPrincipalTemplate.uxml";
        protected override string CaminhoStyle => "Janelas/JanelaPrincipal/JanelaPrincipalStyle.uss";

        #region .: Elementos :.

        private const string NOME_BOTAO_CRIAR_ELEMENTOS = "criador-elementos";
        private Button botaoCriarElementos;

        private const string NOME_BOTAO_EDITAR_ELEMENTOS = "editor-elementos";
        private Button botaoEditarElementos;

        private const string NOME_REGIAO_CARREGAMENTO_TELAS = "regiao-carregamento-telas";
        private VisualElement regiaoCarregamentoTelas;

        #endregion

        private Tela telaAtual;

        [MenuItem("AUTIS/Janela Principal")]
        public static void ShowJanelaPrincipal() {
            string titulo = ConstantesProjeto.NomeProjeto;

            JanelaPrincipalBehaviour janela = GetWindow<JanelaPrincipalBehaviour>();
            janela.titleContent = new GUIContent(titulo);

            return;
        }

        private void OnEnable() {
            EditorApplication.playModeStateChanged += HandlePlayModeIniciado;
            return;
        }

        private void HandlePlayModeIniciado(PlayModeStateChange state) {
            if(state != PlayModeStateChange.ExitingEditMode || telaAtual == null) {
                return;
            }

            telaAtual.OnEditorPlay();
            return;
        }

        protected override void OnRenderizarInterface() {
            Navigator.Instance.IrPara(new MenuPrincipalBehaviour());

            ConfigurarBotaoCriarElementos();
            ConfigurarBotaoEditarElementos();
            ConfigurarRegiaoCarregamentoTelas();

            return;
        }

        private void ConfigurarBotaoCriarElementos() {
            botaoCriarElementos = root.Query<Button>(NOME_BOTAO_CRIAR_ELEMENTOS);
            return;
        }

        private void ConfigurarBotaoEditarElementos() {
            botaoEditarElementos = root.Query<Button>(NOME_BOTAO_EDITAR_ELEMENTOS);
            return;
        }

        private void ConfigurarRegiaoCarregamentoTelas() {
            regiaoCarregamentoTelas = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TELAS);
            return;
        }

        private void OnInspectorUpdate() {
            telaAtual?.OnEditorUpdate();

            if(Navigator.Instance.TelaAtual == telaAtual) {
                return;
            }

            telaAtual = Navigator.Instance.TelaAtual;
            regiaoCarregamentoTelas.Clear();
            regiaoCarregamentoTelas.Add(telaAtual.Root);

            return;
        }

        private void OnDestroy() {
            // TODO: Limpar Navigator (???)
            return;
        }
    }
}