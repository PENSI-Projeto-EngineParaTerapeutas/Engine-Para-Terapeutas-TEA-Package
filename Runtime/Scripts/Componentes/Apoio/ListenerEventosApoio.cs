using UnityEngine;
using Autis.Runtime.DTOs;
using Autis.Runtime.Eventos;

namespace Autis.Runtime.ComponentesGameObjects {
    [AddComponentMenu("AUTIS/Apoio/Listener Apoio")]
    public class ListenerEventosApoio : MonoBehaviour {
        [SerializeField]
        private EventoJogo eventoErroGeral;

        [SerializeField]
        private EventoInteiro eventoErroComOrdem;

        private IdentificadorTipoApoio tipoApoio;
        private AudioSource audioSource;
        private Video video;

        private void Awake() {
            tipoApoio = GetComponent<IdentificadorTipoApoio>();
            audioSource = GetComponent<AudioSource>();
            video = GetComponent<Video>();

            return;
        }

        private void Start() {
            eventoErroGeral.AdicionarCallback(HandleEventoErroGeral);
            eventoErroComOrdem.AdicionarCallback(HandleEventoErroComOrdem);
            tipoApoio.DesabilitarComponentes();

            return;
        }

        private void HandleEventoErroGeral() {
            AcionarComponentes();
            return;
        }

        private void HandleEventoErroComOrdem(int numeroPassoAtual) {
            AcionarComponentes();
            return;
        }

        private void AcionarComponentes() {
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
