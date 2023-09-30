using UnityEngine;
using UnityEngine.Video;
using Autis.Runtime.Eventos;

namespace Autis.Runtime.ComponentesGameObjects {
    [AddComponentMenu("AUTIS/Contexto/Listener Contexto")]
    public class ListenerContexto : MonoBehaviour {
        public string nomeArquivoVideoContexto;

        [SerializeField]
        private EventoJogo eventoExibirContextualizacao;
        [SerializeField]
        private EventoJogo eventoExibirInstrucao;

        private SpriteRenderer spriteRenderer;
        private AudioSource audioSource;
        private VideoPlayer videoPlayer;
        private Video video;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
            videoPlayer = GetComponent<VideoPlayer>();
            video = GetComponent<Video>();

            return;
        }

        private void Start() {
            eventoExibirContextualizacao.AdicionarCallback(HandleEventoExibirContextualizacao);
            return;
        }

        private void HandleEventoExibirContextualizacao() {
            if(string.IsNullOrWhiteSpace(video.nomeArquivoVideo)) {
                EncerrarContextualizacao();
                return;
            }

            spriteRenderer.enabled = true;
            audioSource.enabled = true;
            videoPlayer.enabled = true;
            video.enabled = true;

            video.Player.loopPointReached += HandleFimApresentacaoContexto;
            video.Player.Play();

            return;
        }

        private void HandleFimApresentacaoContexto(VideoPlayer player) {
            EncerrarContextualizacao();
            return;
        }

        private void EncerrarContextualizacao() {
            Debug.Log("[LOG]: Iniciar Instrução");
            eventoExibirInstrucao.AcionarCallbacks();

            // TODO: Identificar tipo de instrução e se for video ou audio despausar o jogo ao iniciar

            gameObject.SetActive(false);
            return;
        }
    }
}