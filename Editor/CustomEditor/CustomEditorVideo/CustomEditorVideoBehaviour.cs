using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;
using EngineParaTerapeutas.ComponentesGameObjects;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(Video))]
    public class CustomEditorVideoBehaviour : CustomEditorComponentes<Video, VideoPlayer> {
        #region .: Elementos :.

        private const string REGIAO_CARREGAMENTO_INPUTS_PADROES_VIDEO = "regiao-carregamento-inputs-padroes-video";
        private VisualElement regiaoCarregamentosInputsPadroesVideo;

        private InputsComponenteVideo grupoInputsVideo;

        #endregion

        public override void OnEnable() {
            componente = target as Video;
            componenteOriginal = componente.Player;
            root = new VisualElement();

            ImportarDefaultStyle();
            AlterarVisibilidadeComponenteOriginal(HideFlags.HideInInspector);

            grupoInputsVideo = new InputsComponenteVideo();

            ImportarTemplate("/CustomEditor/CustomEditorVideo/CustomEditorVideoTemplate.uxml");
            ImportarStyle("/CustomEditor/CustomEditorVideo/CustomEditorVideoStyle.uss");

            ConfigurarInputs();

            return;
        }

        protected override void ConfigurarInputs() {
            regiaoCarregamentosInputsPadroesVideo = root.Query<VisualElement>(REGIAO_CARREGAMENTO_INPUTS_PADROES_VIDEO);
            regiaoCarregamentosInputsPadroesVideo.Add(grupoInputsVideo.Root);

            grupoInputsVideo.VincularDados(componente);

            return;
        }
    }
}