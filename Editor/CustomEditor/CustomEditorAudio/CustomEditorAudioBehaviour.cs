using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.ComponentesGameObjects;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(Audio))]
    public class CustomEditorAudioBehaviour : CustomEditor<Audio, AudioSource> {
        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_PADROES_AUDIO = "regiao-carregamento-inputs-padroes-audio";
        private VisualElement regiaoCarregamentoInputsPadroesAudio;

        private InputsAudio grupoInputsAudio;

        #endregion

        public override void OnEnable() {
            base.OnEnable();
            grupoInputsAudio = new InputsAudio();

            ImportarTemplate("/CustomEditor/CustomEditorAudio/CustomEditorAudioTemplate.uxml");
            ImportarStyle("/CustomEditor/CustomEditorAudio/CustomEditorAudioStyle.uss");
            ConfigurarInputs();

            return;
        }

        protected override void ConfigurarInputs() {
            regiaoCarregamentoInputsPadroesAudio = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_PADROES_AUDIO);
            regiaoCarregamentoInputsPadroesAudio.Add(grupoInputsAudio.Root);

            grupoInputsAudio.VincularDados(componenteOriginal);

            return;
        }
    }
}