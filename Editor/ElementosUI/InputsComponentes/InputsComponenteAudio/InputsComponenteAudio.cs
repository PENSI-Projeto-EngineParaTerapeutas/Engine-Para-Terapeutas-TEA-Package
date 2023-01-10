using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.UI {
    public class InputsComponenteAudio : ElementoInterfaceEditor, IVinculavel<AudioSource>, IReiniciavel {
        #region .: Elementos :.
        public VisualElement RegiaoInputImagem { get => regiaoInputAudio; }
        public FloatField CampoVolume { get => campoVolume; }
        public Toggle CampoMudo { get => campoMudo; }
        public Toggle CampoTocarAoIniciar { get => campoTocarAoIniciar; }
        public InputAudio InputAudio { get => inputAudio; }

        private const string NOME_REGIAO_INPUT_AUDIO = "regiao-input-audio";
        private readonly VisualElement regiaoInputAudio;

        private const string NOME_LABEL_VOLUME = "label-volume";
        private const string NOME_INPUT_VOLUME = "input-volume";
        private readonly FloatField campoVolume;

        private const string NOME_LABEL_MUDO = "label-mudo";
        private const string NOME_INPUT_MUDO = "input-mudo";
        private readonly Toggle campoMudo;

        private const string NOME_LABEL_TOCAR_AO_INICIAR = "label-tocar-iniciar";
        private const string NOME_INPUT_TOCAR_AO_INICIAR = "input-tocar-iniciar";
        private readonly Toggle campoTocarAoIniciar;

        private readonly InputAudio inputAudio;

        #endregion

        private AudioSource audioSourceVinculado;

        public InputsComponenteAudio() {
            ImportarTemplate("ElementosUI/InputsComponentes/InputsComponenteAudio/InputsComponenteAudioTemplate.uxml");
            ImportarStyle("ElementosUI/InputsComponentes/InputsComponenteAudio/InputsComponenteAudioStyle.uss");

            regiaoInputAudio = Root.Query<VisualElement>(NOME_REGIAO_INPUT_AUDIO);
            campoVolume = Root.Query<FloatField>(NOME_INPUT_VOLUME);
            campoMudo = Root.Query<Toggle>(NOME_INPUT_MUDO);
            campoTocarAoIniciar = Root.Query<Toggle>(NOME_INPUT_TOCAR_AO_INICIAR);

            inputAudio = new InputAudio();

            ConfigurarCampoArquivoSom();
            ConfigurarCampoMudo();
            ConfigurarCamposVolume();
            ConfigurarCampoTocarAoIniciaor();

            return;
        }

        private void ConfigurarCampoArquivoSom() {
            regiaoInputAudio.Add(InputAudio.Root);
            return;
        }

        private void ConfigurarCamposVolume() {
            CampoVolume.labelElement.name = NOME_LABEL_VOLUME;
            CampoVolume.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            CampoVolume.SetValueWithoutNotify(0);

            CampoVolume.RegisterCallback<ChangeEvent<float>>(evt => {
                if(evt.newValue < 0) {
                    CampoVolume.value = 0;
                }
            });

            return;
        }

        private void ConfigurarCampoMudo() {
            CampoMudo.labelElement.name = NOME_LABEL_MUDO;
            CampoMudo.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            CampoMudo.SetValueWithoutNotify(false);
            CampoMudo.RegisterCallback<ChangeEvent<bool>>(evt => {
                AlterarVisibilidadeCamposDependentes(evt.newValue);
            });

            return;
        }

        private void AlterarVisibilidadeCamposDependentes(bool deveExibir) {
            if(deveExibir) {
                CampoTocarAoIniciar.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                InputAudio.CampoAudio.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                CampoVolume.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            }
            else {
                CampoTocarAoIniciar.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                InputAudio.CampoAudio.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                CampoVolume.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            }

            return;
        }

        private void ConfigurarCampoTocarAoIniciaor() {
            CampoTocarAoIniciar.labelElement.name = NOME_LABEL_TOCAR_AO_INICIAR;
            CampoTocarAoIniciar.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            CampoTocarAoIniciar.SetValueWithoutNotify(false);

            return;
        }

        public void ReiniciarCampos() {
            InputAudio.CampoAudio.SetValueWithoutNotify(null);
            CampoMudo.SetValueWithoutNotify(false);
            CampoTocarAoIniciar.SetValueWithoutNotify(false);
            CampoVolume.SetValueWithoutNotify(0);

            return;
        }

        public void VincularDados(AudioSource componente) {
            audioSourceVinculado = componente;

            InputAudio.CampoAudio.SetValueWithoutNotify(audioSourceVinculado.clip);
            CampoMudo.SetValueWithoutNotify(audioSourceVinculado.mute);
            CampoTocarAoIniciar.SetValueWithoutNotify(audioSourceVinculado.playOnAwake);
            CampoVolume.SetValueWithoutNotify(audioSourceVinculado.volume);

            InputAudio.CampoAudio.RegisterCallback<ChangeEvent<Object>>(evt => {
                audioSourceVinculado.clip = InputAudio.CampoAudio.value as AudioClip;
            });

            CampoMudo.RegisterCallback<ChangeEvent<bool>>(evt => {
                audioSourceVinculado.mute = CampoMudo.value;
            });

            CampoTocarAoIniciar.RegisterCallback<ChangeEvent<bool>>(evt => {
                audioSourceVinculado.playOnAwake = CampoTocarAoIniciar.value;
            });

            CampoVolume.RegisterCallback<ChangeEvent<float>>(evt => {
                audioSourceVinculado.volume = CampoVolume.value;
            });

            AlterarVisibilidadeCamposDependentes(CampoMudo.value);

            return;
        }
    }
}