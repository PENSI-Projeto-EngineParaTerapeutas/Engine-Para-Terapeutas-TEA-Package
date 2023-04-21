using UnityEngine;
using Autis.Runtime.DTOs;
using Autis.Runtime.Eventos;

namespace Autis.Runtime.ComponentesGameObjects {
    [RequireComponent(typeof(Animator))]
    [AddComponentMenu("AUTIS/Personagem/Controle Indireto")]
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