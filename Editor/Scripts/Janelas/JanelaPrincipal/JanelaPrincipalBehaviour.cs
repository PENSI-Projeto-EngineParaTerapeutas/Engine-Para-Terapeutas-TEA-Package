using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Autis.Editor.Constantes;
using Autis.Runtime.Eventos;
using Autis.Editor.Utils;

namespace Autis.Editor.Telas {
    public class JanelaPrincipalBehaviour : JanelaEditor {
        protected override string CaminhoTemplate => "Janelas/JanelaPrincipal/JanelaPrincipalTemplate.uxml";
        protected override string CaminhoStyle => "Janelas/JanelaPrincipal/JanelaPrincipalStyle.uss";

        #region .: Classes USS :.

        private const string CLASSE_ABA_ATIVA = "aba-ativa";
        private const string CLASSE_ABA_INATIVA = "aba-inativa";

        #endregion

        #region .: Eventos :.

        protected static EventoJogo eventoIniciarCriacao;
        protected static EventoJogo eventoFinalizarCriacao;

        protected static EventoJogo eventoIniciarEdicao;
        protected static EventoJogo eventoFinalizarEdicao;

        #endregion

        #region .: Elementos :.

        private const string NOME_BOTAO_CRIAR_ELEMENTOS = "criador-elementos";
        private Button botaoCriarElementos;

        private const string NOME_BOTAO_EDITAR_ELEMENTOS = "editor-elementos";
        private Button botaoEditarElementos;

        private const string NOME_REGIAO_CARREGAMENTO_TELAS = "regiao-carregamento-telas";
        private VisualElement regiaoCarregamentoTelas;

        #endregion

        private Tela telaAtual;

        private enum Estado { Criar, Editar, }
        private Estado estadoAtual = Estado.Criar;

        [MenuItem("AUTIS/Janela Principal")]
        public static void ShowJanelaPrincipal() {
            string titulo = ConstantesProjeto.NomeProjeto;

            JanelaPrincipalBehaviour janela = GetWindow<JanelaPrincipalBehaviour>();
            janela.titleContent = new GUIContent(titulo);
            janela.minSize = new Vector2(350, 680);
            janela.maxSize = janela.minSize;
           
            return;
        }

        private void OnEnable() {
            EditorApplication.playModeStateChanged += HandlePlayModeIniciado;
            return;
        }

        private void HandlePlayModeIniciado(PlayModeStateChange state) {
            if(state == PlayModeStateChange.EnteredPlayMode) {
                MaximizarTelaJogo();
            }
            else if(state == PlayModeStateChange.ExitingPlayMode) {
                MinimizarTelaJogo();
            }

            if(state != PlayModeStateChange.ExitingEditMode || telaAtual == null) {
                return;
            }

            telaAtual.OnEditorPlay();
            return;
        }

        [MenuItem("Desenvolvimento/Maximizar Tela Jogo")]
        private static void MaximizarTelaJogo() {
            EditorWindow[] windows = (UnityEditor.EditorWindow[]) Resources.FindObjectsOfTypeAll(typeof(UnityEditor.EditorWindow));
            foreach (EditorWindow window in windows) {
                if (window != null && window.GetType().FullName == "UnityEditor.GameView") {
                    window.maximized = true;
                    break;
                }
            }

            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();

            return;
        }

        [MenuItem("Desenvolvimento/Minimizar Tela Jogo")]
        private static void MinimizarTelaJogo() {
            EditorWindow[] windows = (UnityEditor.EditorWindow[]) Resources.FindObjectsOfTypeAll(typeof(UnityEditor.EditorWindow));
            foreach (EditorWindow window in windows) {
                if (window != null && window.GetType().FullName == "UnityEditor.GameView") {
                    window.maximized = false;
                    break;
                }
            }

            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();

            return;
        }

        protected override void OnRenderizarInterface() {
            Navigator.Instance.IrPara(new MenuPrincipalBehaviour());
            estadoAtual = Estado.Criar;

            ConfigurarEventos();

            ConfigurarBotaoCriarElementos();
            ConfigurarBotaoEditarElementos();
            ConfigurarRegiaoCarregamentoTelas();

            return;
        }

        private void ConfigurarEventos() {
            eventoIniciarCriacao = Importador.ImportarEvento("EventoIniciarCriacao");
            eventoFinalizarCriacao = Importador.ImportarEvento("EventoFinalizarCriacao");

            eventoIniciarEdicao = Importador.ImportarEvento("EventoIniciarEdicao");
            eventoFinalizarEdicao = Importador.ImportarEvento("EventoFinalizarEdicao");

            eventoIniciarCriacao.AdicionarCallback(HandleIniciarCriacao);
            eventoFinalizarCriacao.AdicionarCallback(HandleFinalizarCriacao);

            eventoIniciarEdicao.AdicionarCallback(HandleIniciarEdicao);
            eventoFinalizarEdicao.AdicionarCallback(HandleFinalizarEdicao);

            return;
        }

        private void HandleIniciarCriacao() {
            botaoEditarElementos.SetEnabled(false);
            return;
        }

        private void HandleFinalizarCriacao() {
            botaoEditarElementos.SetEnabled(true);
            return;
        }

        private void HandleIniciarEdicao() {
            botaoCriarElementos.SetEnabled(false);
            return;
        }

        private void HandleFinalizarEdicao() {
            botaoCriarElementos.SetEnabled(true);
            return;
        }

        private void ConfigurarBotaoCriarElementos() {
            botaoCriarElementos = root.Query<Button>(NOME_BOTAO_CRIAR_ELEMENTOS);
            botaoCriarElementos.clicked += HandleBotaoCriarElementosClick;

            return;
        }

        private void HandleBotaoCriarElementosClick() {
            if(estadoAtual == Estado.Criar) {
                return;
            }

            botaoCriarElementos.AddToClassList(CLASSE_ABA_ATIVA);
            botaoCriarElementos.RemoveFromClassList(CLASSE_ABA_INATIVA);

            botaoEditarElementos.AddToClassList(CLASSE_ABA_INATIVA);
            botaoEditarElementos.RemoveFromClassList(CLASSE_ABA_ATIVA);

            Navigator.Instance.VoltarParaTelaInicial();
            estadoAtual = Estado.Criar;

            return;
        }

        private void ConfigurarBotaoEditarElementos() {
            botaoEditarElementos = root.Query<Button>(NOME_BOTAO_EDITAR_ELEMENTOS);
            botaoEditarElementos.clicked += HandleBotaoEditarElementosClick;

            return;
        }

        private void HandleBotaoEditarElementosClick() {
            if(estadoAtual == Estado.Editar) {
                return;
            }

            botaoCriarElementos.AddToClassList(CLASSE_ABA_INATIVA);
            botaoCriarElementos.RemoveFromClassList(CLASSE_ABA_ATIVA);

            botaoEditarElementos.AddToClassList(CLASSE_ABA_ATIVA);
            botaoEditarElementos.RemoveFromClassList(CLASSE_ABA_INATIVA);

            Navigator.Instance.VoltarParaTelaInicial();
            Navigator.Instance.IrPara(new EditarElementoBehaviour());
            estadoAtual = Estado.Editar;

            return;
        }

        private void ConfigurarRegiaoCarregamentoTelas() {
            regiaoCarregamentoTelas = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TELAS);
            return;
        }

        private void OnInspectorUpdate() {
            ControladorFoco.VerificarSelecao();
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