using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.Video;
using EngineParaTerapeutas.ComponentesGameObjects;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.Utils;

namespace EngineParaTerapeutas.UI {
    public class InputsComponenteVideo : ElementoInterfaceEditor, IVinculavel<Video>, IReiniciavel {
        protected override string CaminhoTemplate => "ElementosUI/InputsComponentes/InputsComponenteVideo/InputsComponenteVideoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputsComponentes/InputsComponenteVideo/InputsComponenteVideoStyle.uss";

        #region .: Elementos :.
        public VisualElement RegiaoInputVideo { get => regiaoInputVideo; }
        public Toggle CampoReproduzirIniciar { get => campoReproduzirIniciar; }
        public Toggle CampoReproduzirLoop { get => campoReproduzirLoop; }
        public Toggle CampoReproduzirSom { get => campoReproduzirSom; }
        public FloatField CampoVelocidade { get => campoVelocidade; }
        public InputVideo InputVideo { get => inputVideo; }

        private const string NOME_REGIAO_INPUT_VIDEO = "regiao-input-video";
        private readonly VisualElement regiaoInputVideo;

        private const string NOME_LABEL_REPRODUZIR_INICIAR = "label-reproduzir-ao-iniciar";
        private const string NOME_INPUT_REPRODUZIR_INICIAR = "input-reproduzir-ao-iniciar";
        private readonly Toggle campoReproduzirIniciar;

        private const string NOME_LABEL_REPRODUZIR_LOOP = "label-reproduzir-em-loop";
        private const string NOME_INPUT_REPRODUZIR_LOOP = "input-reproduzir-em-loop";
        private readonly Toggle campoReproduzirLoop;

        private const string NOME_LABEL_REPRODUZIR_SOM = "label-reproduzir-som";
        private const string NOME_INPUT_REPRODUZIR_SOM = "input-reproduzir-som";
        private readonly Toggle campoReproduzirSom;

        private const string NOME_LABEL_VELOCIDADE = "label-velocidade";
        private const string NOME_INPUT_VELOCIDADE = "input-velocidade";
        private readonly FloatField campoVelocidade;

        private readonly InputVideo inputVideo;

        #endregion

        private Video componenteVideo;
        private VideoPlayer componentePlayer;

        public InputsComponenteVideo() {
            regiaoInputVideo = Root.Query<VisualElement>(NOME_REGIAO_INPUT_VIDEO);
            campoReproduzirIniciar = Root.Query<Toggle>(NOME_INPUT_REPRODUZIR_INICIAR);
            campoReproduzirLoop = Root.Query<Toggle>(NOME_INPUT_REPRODUZIR_LOOP);
            campoReproduzirSom = Root.Query<Toggle>(NOME_INPUT_REPRODUZIR_SOM);
            campoVelocidade = Root.Query<FloatField>(NOME_INPUT_VELOCIDADE);

            inputVideo = new InputVideo();

            ConfigurarInputVideo();
            ConfigurarCampoReproduzirIniciar();
            ConfigurarCampoReproduzirLoop();
            ConfigurarCampoReproduzirSom();
            ConfigurarCampoVelocidade();

            return;
        }

        private void ConfigurarInputVideo() {
            RegiaoInputVideo.Add(InputVideo.Root);
            return;
        }

        private void ConfigurarCampoReproduzirIniciar() {
            CampoReproduzirIniciar.labelElement.name = NOME_LABEL_REPRODUZIR_INICIAR;
            CampoReproduzirIniciar.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoReproduzirIniciar.SetValueWithoutNotify(false);

            return;
        }

        private void ConfigurarCampoReproduzirLoop() {
            CampoReproduzirLoop.labelElement.name = NOME_LABEL_REPRODUZIR_LOOP;
            CampoReproduzirLoop.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoReproduzirLoop.SetValueWithoutNotify(false);

            return;
        }

        private void ConfigurarCampoReproduzirSom() {
            CampoReproduzirSom.labelElement.name = NOME_LABEL_REPRODUZIR_SOM;
            CampoReproduzirSom.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoReproduzirSom.SetValueWithoutNotify(false);

            return;
        }

        private void ConfigurarCampoVelocidade() {
            CampoVelocidade.labelElement.name = NOME_LABEL_VELOCIDADE;
            CampoVelocidade.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoVelocidade.SetValueWithoutNotify(1f);

            return;
        }

        public void VincularDados(Video componente) {
            componenteVideo = componente;
            componentePlayer = componenteVideo.Player;

            InputVideo.CampoVideo.SetValueWithoutNotify(componenteVideo.nomeArquivoVideo);
            CampoReproduzirIniciar.SetValueWithoutNotify(componentePlayer.playOnAwake);
            CampoReproduzirLoop.SetValueWithoutNotify(componentePlayer.isLooping);
            CampoReproduzirSom.SetValueWithoutNotify(!componenteVideo.PlayerAudio.mute);
            CampoVelocidade.SetValueWithoutNotify(componentePlayer.playbackSpeed);

            InputVideo.CampoVideo.RegisterCallback<ChangeEvent<string>>(evt => {
                componenteVideo.nomeArquivoVideo = InputVideo.CampoVideo.value; 
                Salvamento.SalvarProjeto();
            });

            CampoReproduzirIniciar.RegisterCallback<ChangeEvent<bool>>(evt => {
                componentePlayer.playOnAwake = CampoReproduzirIniciar.value;
            });

            CampoReproduzirLoop.RegisterCallback<ChangeEvent<bool>>(evt => {
                componentePlayer.isLooping = CampoReproduzirLoop.value;
            });

            CampoReproduzirSom.RegisterCallback<ChangeEvent<bool>>(evt => {
                componenteVideo.PlayerAudio.mute = CampoReproduzirSom.value;
            });

            CampoVelocidade.RegisterCallback<ChangeEvent<float>>(evt => {
                if(evt.newValue < 0f) {
                    campoVelocidade.value = 0f;
                }
                else if(evt.newValue > 10f) {
                    campoVelocidade.value = 10f;
                }

                componentePlayer.playbackSpeed = campoVelocidade.value;
            });

            return;
        }

        public void ReiniciarCampos() {
            componenteVideo = null;
            componentePlayer = null;

            InputVideo.ReiniciarCampos();
            CampoReproduzirIniciar.SetValueWithoutNotify(false);
            CampoReproduzirLoop.SetValueWithoutNotify(false);
            CampoReproduzirSom.SetValueWithoutNotify(false);
            CampoVelocidade.SetValueWithoutNotify(1f);

            return;
        }
    }
}