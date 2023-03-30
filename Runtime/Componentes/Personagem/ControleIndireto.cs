using UnityEngine;
using EngineParaTerapeutas.Eventos;
using EngineParaTerapeutas.DTOs;

namespace EngineParaTerapeutas.ComponentesGameObjects {
    [RequireComponent(typeof(Animator))]
    [AddComponentMenu("Engine Terapeutas TEA/Controles Personagem/Controle Indireto")]
    public class ControleIndireto : MonoBehaviour {
        private Animator animator;
        private EventoAcionarAcaoPersonagem eventoAcionarAcaoPersonagem;

        private IdentificadorTipoControle tipoControle;

        private void Awake() {
            tipoControle = GetComponent<IdentificadorTipoControle>();
            if(tipoControle.Tipo != TipoControle.Indireto) {
                this.enabled = false;
                return;
            }

            animator = GetComponent<Animator>();
            eventoAcionarAcaoPersonagem = Resources.Load<EventoAcionarAcaoPersonagem>("ScriptableObjects/EventoAcionarAcaoPersonagem");

            return;
        }

        private void Start() {
            eventoAcionarAcaoPersonagem.AdicionarCallback(HandleEventoAcionarAcaoPersonagem);
            return;
        }

        private void HandleEventoAcionarAcaoPersonagem(AnimationClip animacaoAcionada) {
            animator.Play(animacaoAcionada.name);
            return;
        }
    }
}