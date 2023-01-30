using UnityEngine;
using EngineParaTerapeutas.Eventos;
using UnityEngine.Video;

namespace EngineParaTerapeutas.ComponentesGameObjects {
    [AddComponentMenu("Engine Terapeutas TEA/Listener Eventos")]
    public class ListenerEventos : MonoBehaviour {
        public TipoAcionamento TipoAcionamento { get => tipoAcionamento; set { tipoAcionamento = value; } }
        [SerializeField]
        private TipoAcionamento tipoAcionamento;

        private EventoJogo eventoErro;
        private EventoJogo eventoAcerto;
        private EventoJogo eventoFimJogo;

        private SpriteRenderer spriteRenderer;
        private AudioSource audioSource;
        private Video video;

        private void Start() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
            video = GetComponent<Video>();

            spriteRenderer.enabled = false;
            audioSource.enabled = false;
            video.enabled = false;

            eventoErro = Resources.Load<EventoJogo>("ScriptableObjects/EventoErro");
            eventoAcerto = Resources.Load<EventoJogo>("ScriptableObjects/EventoAcerto");
            eventoFimJogo = Resources.Load<EventoJogo>("ScriptableObjects/EventoFimJogo");

            eventoErro.AdicionarCallback(HandleEventoErro);
            eventoAcerto.AdicionarCallback(HandleEventoAcerto);
            eventoFimJogo.AdicionarCallback(HandleEventoFimJogo);

            return;
        }

        public virtual void HandleEventoErro() {
            if(tipoAcionamento != TipoAcionamento.Erro) {
                return;
            }

            HabilitarComponentes();
            return;
        }

        private void HabilitarComponentes() {
            spriteRenderer.enabled = true;
            audioSource.enabled = true;
            video.enabled = true;

            // TODO: Identificar tipo de Apoio ou reforço

            if(audioSource.clip != null) {
                audioSource.Play();
            }

            if(!string.IsNullOrEmpty(video.nomeArquivoVideo)) {
                video.Player.Play();
            }

            return;
        }

        public virtual void HandleEventoAcerto() {
            if(tipoAcionamento != TipoAcionamento.Acerto) {
                return;
            }

            HabilitarComponentes();
            return;
        }

        public virtual void HandleEventoFimJogo() {
            if(tipoAcionamento != TipoAcionamento.FimJogo) {
                return;
            }

            HabilitarComponentes();
            return;
        }
    }
}