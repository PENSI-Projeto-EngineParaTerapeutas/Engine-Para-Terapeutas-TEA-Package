using UnityEngine;
using EngineParaTerapeutas.DTOs;

namespace EngineParaTerapeutas.ComponentesGameObjects {
    public class ListenerEventosReforco : ListenerEventosBase {
        [AddComponentMenu("Engine Terapeutas TEA/Listener Eventos Reforço")]
        private IdentificadorTipoReforco tipoReforco;
        private AudioSource audioSource;
        private Video video;

        protected override void Awake() {
            base.Awake();

            tipoReforco = GetComponent<IdentificadorTipoReforco>();
            audioSource = GetComponent<AudioSource>();
            video = GetComponent<Video>();

            return;
        }

        protected override void Start() {
            base.Start();
            tipoReforco.DesabilitarComponentes();

            return;
        }

        protected override void AcionarComponentes() {
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
