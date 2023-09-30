using UnityEngine;
using Autis.Runtime.DTOs;
using Autis.Runtime.Eventos;

namespace Autis.Runtime.ComponentesGameObjects {
    [AddComponentMenu("AUTIS/Reforço/Listener Reforço")]
    public class ListenerEventosReforco : MonoBehaviour {
        public TipoAcionamentoReforco TipoAcionamento { get => tipoAcionamento; set { tipoAcionamento = value; } }
        [SerializeField]
        private TipoAcionamentoReforco tipoAcionamento;

        [SerializeField]
        protected EventoJogo eventoAcerto;
        [SerializeField]
        protected EventoJogo eventoFimJogo;
        [SerializeField]
        protected EventoJogo eventoErro;

        private IdentificadorTipoReforco tipoReforco;
        private AudioSource audioSource;
        private Video video;

        private void Awake() {
            tipoReforco = GetComponent<IdentificadorTipoReforco>();
            audioSource = GetComponent<AudioSource>();
            video = GetComponent<Video>();

            return;
        }

        private void Start() {
            switch(tipoAcionamento) {
                case(TipoAcionamentoReforco.Acerto): {
                    eventoAcerto.AdicionarCallback(AcionarComponentes);
                    break;
                }
                case(TipoAcionamentoReforco.Erro): {
                    eventoErro.AdicionarCallback(AcionarComponentes);
                    break;
                }
                case(TipoAcionamentoReforco.FimJogo): {
                    eventoFimJogo.AdicionarCallback(AcionarComponentes);
                    break;
                }
            }

            tipoReforco.DesabilitarComponentes();

            return;
        }

        private void AcionarComponentes() {
            tipoReforco.HabilitarComponentes();

            switch(tipoReforco.Tipo) {
                case(TiposReforcos.Audio): {
                    audioSource.Play();
                    tipoReforco.IniciarCorrotinaDesabilitarComponentes(audioSource.clip.length);
                    break;
                }
                case(TiposReforcos.Video): {
                    video.Player.Play();
                    video.Player.loopPointReached += HandleDesabilitarComponentesFimVideo;
                    break;
                }
                default: {
                    tipoReforco.IniciarCorrotinaDesabilitarComponentes();
                    break;
                }
            }

            return;
        }

        private void HandleDesabilitarComponentesFimVideo(UnityEngine.Video.VideoPlayer source) {
            tipoReforco.DesabilitarComponentes();
            return;
        }
    }
}