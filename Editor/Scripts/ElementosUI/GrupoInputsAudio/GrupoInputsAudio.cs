using UnityEngine;
using UnityEngine.UIElements;
using Autis.Editor.Manipuladores;

namespace Autis.Editor.UI {
    public class GrupoInputsAudio : ElementoInterfaceEditor, IReiniciavel, IVinculavel<ManipuladorAudioSource> {
        protected override string CaminhoTemplate => "ElementosUI/GrupoInputsAudio/GrupoInputsAudioTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/GrupoInputsAudio/GrupoInputsAudioStyle.uss";

        #region .: Elementos :.

        public Slider CampoVolume { get => campoVolume; }
        public InputAudio CampoArquivoAudio { get => inputAudio; }

        private const string NOME_SLIDER_VOLUME = "input-volume";
        private Slider campoVolume;

        private const string NOME_REGIAO_CARREGAMENTO_INPUT_AUDIO = "regiao-input-audio";
        private VisualElement regiaoCarregamentoInputAudio;

        private InputAudio inputAudio;

        #endregion

        private ManipuladorAudioSource manipulador;

        public GrupoInputsAudio() {
            ConfigurarCampoVolume();
            ConfigurarCampoInputAudio();

            return;
        }

        private void ConfigurarCampoVolume() {
            campoVolume = root.Query<Slider>(NOME_SLIDER_VOLUME);
            campoVolume.lowValue = 0;
            campoVolume.highValue = 1;

            campoVolume.SetValueWithoutNotify(campoVolume.highValue);

            return;
        }

        private void ConfigurarCampoInputAudio() {
            inputAudio = new InputAudio();

            inputAudio.BotaoCancelarAudio.clicked += (() => {
                manipulador.SetAudioClip(null);
            });

            regiaoCarregamentoInputAudio = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_AUDIO);
            regiaoCarregamentoInputAudio.Add(inputAudio.Root);

            return;
        }

        public void ReiniciarCampos() {
            inputAudio.ReiniciarCampos();
            campoVolume.value = campoVolume.highValue;
            
            return;
        }

        public void VincularDados(ManipuladorAudioSource manipulador) {
            this.manipulador = manipulador;

            inputAudio.VincularDados(this.manipulador.GetAudioClip());
            campoVolume.SetValueWithoutNotify(this.manipulador.GetVolume());

            inputAudio.CampoAudio.RegisterCallback<ChangeEvent<Object>>(evt => {
                this.manipulador.SetAudioClip(evt.newValue as AudioClip);
            });

            campoVolume.RegisterCallback<ChangeEvent<float>>(evt => {
                this.manipulador.SetVolume(evt.newValue);
            });

            return;
        }
    }
}