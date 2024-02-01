using System;
using UnityEngine.UIElements;
using Autis.Editor.Manipuladores;

namespace Autis.Editor.UI {
    public class GrupoInputsVideo : ElementoInterfaceEditor, IReiniciavel, IVinculavel<ManipuladorVideo> {
        protected override string CaminhoTemplate => "ElementosUI/GrupoInputsVideo/GrupoInputsVideoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/GrupoInputsVideo/GrupoInputsVideoStyle.uss";

        #region .: Mensagens :.

        private const string MENSAGEM_TOOLTIP_VOLUME = "Volume que o vídeo será reproduzido no jogo.";

        #endregion

        #region .: Elementos :.

        public Slider CampoVolume { get => campoVolume; }
        public InputVideo CampoArquivoVideo { get => inputVideo; }

        private const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_VOLUME = "regiao-tooltip-slider-volume";

        private const string NOME_SLIDER_VOLUME = "input-volume";
        private Slider campoVolume;

        private const string NOME_REGIAO_CARREGAMENTO_INPUT_VIDEO = "regiao-input-video";
        private VisualElement regiaoCarregamentoInputVideo;

        private InputVideo inputVideo;

        #endregion

        private ManipuladorVideo manipulador;

        public GrupoInputsVideo() {
            CarregarTooltipTitulo(MENSAGEM_TOOLTIP_VOLUME);
            ConfigurarCampoVideo();
            ConfigurarCampoVolume();

            return;
        }

        private void CarregarTooltipTitulo(string tooltipTexto) {
            if(!String.IsNullOrEmpty(tooltipTexto)) {
                VisualElement regiaoCarregamentoTooltipTitulo = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_VOLUME); ;
                Tooltip tooltipTitulo = new Tooltip();
                regiaoCarregamentoTooltipTitulo.Add(tooltipTitulo.Root);

                tooltipTitulo.SetTexto(tooltipTexto);
            }

            return;
        }

        private void ConfigurarCampoVideo() {
            inputVideo = new InputVideo();

            inputVideo.BotaoCancelarVideo.clicked += HandleBotaoCancelarVideoClick;

            regiaoCarregamentoInputVideo = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_VIDEO);
            regiaoCarregamentoInputVideo.Add(inputVideo.Root);

            return;
        }

        private void HandleBotaoCancelarVideoClick() {
            manipulador?.SetVideo(string.Empty);
            return;
        }

        private void ConfigurarCampoVolume() {
            campoVolume = root.Query<Slider>(NOME_SLIDER_VOLUME);
            campoVolume.lowValue = 0;
            campoVolume.highValue = 1;

            return;
        }

        public void ReiniciarCampos() {
            inputVideo.ReiniciarCampos();
            campoVolume.value = campoVolume.lowValue;

            return;
        }

        public void VincularDados(ManipuladorVideo manipulador) {
            this.manipulador = manipulador;

            inputVideo.VincularDados(this.manipulador.GetVideo());
            campoVolume.SetValueWithoutNotify(this.manipulador.GetVolume());

            inputVideo.CampoVideo.RegisterCallback<ChangeEvent<string>>(evt => {
                this.manipulador.SetVideo(evt.newValue);
            });

            campoVolume.RegisterCallback<ChangeEvent<float>>(evt => {
                this.manipulador.SetVolume(evt.newValue);
            });

            return;
        }
    }
}