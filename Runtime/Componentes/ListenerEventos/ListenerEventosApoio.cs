using UnityEngine;
using Autis.Runtime.DTOs;

namespace Autis.Runtime.ComponentesGameObjects {
    [AddComponentMenu("AUTIS/Apoio/Listener Apoio")]
    public class ListenerEventosApoio : ListenerEventosBase {
        private IdentificadorTipoApoio tipoApoio;
        private AudioSource audioSource;
        private Video video;

        protected override void Awake() {
            base.Awake();

            tipoApoio = GetComponent<IdentificadorTipoApoio>();
            audioSource = GetComponent<AudioSource>();
            video = GetComponent<Video>();

            return;
        }

        protected override void Start() {
            base.Start();
            tipoApoio.DesabilitarComponentes();

            return;
        }

        protected override void AcionarComponentes() {
            tipoApoio.HabilitarComponentes();

            switch(tipoApoio.Tipo) {
                case(TiposApoios.Audio): {
                    audioSource.Play();
                    tipoApoio.IniciarCorrotinaDesabilitarComponentes(audioSource.clip.length);
                    break;
                }
                case(TiposApoios.Video): {
                    video.Player.Play();
                    video.Player.loopPointReached += HandleDesabilitarComponentesFimVideo;
                    break;
                }
                case(TiposApoios.Imagem): {
                    tipoApoio.IniciarCorrotinaDesabilitarComponentes();
                    break;
                }
            }

            return;
        }

        private void HandleDesabilitarComponentesFimVideo(UnityEngine.Video.VideoPlayer source) {
            tipoApoio.DesabilitarComponentes();
            return;
        }
    }
}
