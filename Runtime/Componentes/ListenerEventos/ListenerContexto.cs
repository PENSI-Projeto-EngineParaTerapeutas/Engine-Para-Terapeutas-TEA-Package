using System.IO;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using EngineParaTerapeutas.Eventos;
using EngineParaTerapeutas.ScriptableObjects;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.ComponentesGameObjects {
    [AddComponentMenu("Engine Terapeutas TEA/Listener Contexto")]
    public class ListenerContexto : MonoBehaviour {
        private Video video;
        private EventoJogo eventoExibirContextualizacao;
        private EventoJogo eventoExibirInstrucao;

        private void Start() {
            video = GetComponent<Video>();
            CarregarVideo();
            
            eventoExibirContextualizacao = Resources.Load<EventoJogo>("ScriptableObjects/EventoApresentarContexto");
            eventoExibirInstrucao = Resources.Load<EventoJogo>("ScriptableObjects/EventoApresentarInstrucao");

            eventoExibirContextualizacao.AdicionarCallback(HandleEventoExibirContextualizacao);

            return;
        }

        private void CarregarVideo() {
            string caminhoInfoCenaAtual = Path.Combine(ConstantesRuntime.NomePastaCenas, SceneManager.GetActiveScene().name);
            Cena infoCenaAtual = Resources.Load<Cena>(caminhoInfoCenaAtual);

            video.AlterarVideo(infoCenaAtual.NomeArquivoVideoContexto);

            return;
        }

        private void HandleEventoExibirContextualizacao() {
            if(string.IsNullOrWhiteSpace(video.nomeArquivoVideo)) {
                EncerrarContextualizacao();
                return;
            }

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