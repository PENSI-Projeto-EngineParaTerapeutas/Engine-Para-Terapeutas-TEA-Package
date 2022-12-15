using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.UI {
    public class InputsAudio : ElementoInterfaceEditor, IVinculavel<AudioSource>, IReiniciavel {
        #region .: Elementos :.

        private const string NOME_LABEL_ARQUIVO_AUDIO = "label-arquivo-som";
        private const string NOME_INPUT_ARQUIVO_AUDIO = "input-arquivo-som";
        public ObjectField CampoArquivoAudio { get => campoArquivoAudio; }
        private readonly ObjectField campoArquivoAudio;

        private const string NOME_LABEL_VOLUME = "label-volume";
        private const string NOME_INPUT_VOLUME = "input-volume";
        public FloatField CampoVolume { get => campoVolume; }
        private readonly FloatField campoVolume;

        private const string NOME_LABEL_MUDO = "label-mudo";
        private const string NOME_INPUT_MUDO = "input-mudo";
        public Toggle CampoMudo { get => campoMudo; }
        private readonly Toggle campoMudo;

        private const string NOME_LABEL_TOCAR_AO_INICIAR = "label-tocar-iniciar";
        private const string NOME_INPUT_TOCAR_AO_INICIAR = "input-tocar-iniciar";
        public Toggle CampoTocarAoIniciar { get => campoTocarAoIniciar; }
        private readonly Toggle campoTocarAoIniciar;

        #endregion

        private AudioSource audioSourceVinculado;

        public InputsAudio() {
            ImportarTemplate("Componentes/GruposInputs/InputsAudio/InputsAudioTemplate.uxml");
            ImportarStyle("Componentes/GruposInputs/InputsAudio/InputsAudioStyle.uss");

            campoArquivoAudio = Root.Query<ObjectField>(NOME_INPUT_ARQUIVO_AUDIO);
            campoVolume = Root.Query<FloatField>(NOME_INPUT_VOLUME);
            campoMudo = Root.Query<Toggle>(NOME_INPUT_MUDO);
            campoTocarAoIniciar = Root.Query<Toggle>(NOME_INPUT_TOCAR_AO_INICIAR);

            ConfigurarCampoArquivoSom();
            ConfigurarCampoMudo();
            ConfigurarCamposVolume();
            ConfigurarCampoTocarAoIniciaor();

            return;
        }

        private void ConfigurarCampoArquivoSom() {
            CampoArquivoAudio.labelElement.name = NOME_LABEL_ARQUIVO_AUDIO;
            CampoArquivoAudio.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            CampoArquivoAudio.objectType = typeof(AudioClip);
            CampoArquivoAudio.SetValueWithoutNotify(null);

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
            if (deveExibir) {
                CampoTocarAoIniciar.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                CampoArquivoAudio.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                CampoVolume.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            } else {
                CampoTocarAoIniciar.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                CampoArquivoAudio.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
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
            CampoArquivoAudio.SetValueWithoutNotify(null);
            CampoMudo.SetValueWithoutNotify(false);
            CampoTocarAoIniciar.SetValueWithoutNotify(false);
            CampoVolume.SetValueWithoutNotify(0);

            return;
        }

        public void VincularDados(AudioSource componente) {
            audioSourceVinculado = componente;

            CampoArquivoAudio.SetValueWithoutNotify(audioSourceVinculado.clip);
            CampoMudo.SetValueWithoutNotify(audioSourceVinculado.mute);
            CampoTocarAoIniciar.SetValueWithoutNotify(audioSourceVinculado.playOnAwake);
            CampoVolume.SetValueWithoutNotify(audioSourceVinculado.volume);

            CampoArquivoAudio.RegisterCallback<ChangeEvent<Object>>(evt => {
                audioSourceVinculado.clip = CampoArquivoAudio.value as AudioClip;
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