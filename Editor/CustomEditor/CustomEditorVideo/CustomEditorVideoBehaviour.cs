using UnityEditor;
using UnityEngine.UIElements;
using EngineParaTerapeutas.ComponentesGameObjects;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(Video))]
    public class CustomEditorVideoBehaviour : CustomEditorBase {
        protected override string CaminhoTemplate => "CustomEditor/CustomEditorVideo/CustomEditorVideoTemplate.uxml";
        protected override string CaminhoStyle => "CustomEditor/CustomEditorVideo/CustomEditorVideoStyle.uss";

        #region .: Elementos :.

        private const string REGIAO_CARREGAMENTO_INPUTS_PADROES_VIDEO = "regiao-carregamento-inputs-padroes-video";
        private VisualElement regiaoCarregamentosInputsPadroesVideo;

        private InputsComponenteVideo grupoInputsVideo;

        #endregion

        private Video componente;

        protected override void OnRenderizarInterface() {
            componente = target as Video;

            grupoInputsVideo = new InputsComponenteVideo();
            ConfigurarInputVideo();

            return;
        }

        private void ConfigurarInputVideo() {
            regiaoCarregamentosInputsPadroesVideo = root.Query<VisualElement>(REGIAO_CARREGAMENTO_INPUTS_PADROES_VIDEO);
            regiaoCarregamentosInputsPadroesVideo.Add(grupoInputsVideo.Root);

            grupoInputsVideo.VincularDados(componente);

            return;
        }
    }
}