using UnityEngine;
using Autis.Runtime.DTOs;
using Autis.Runtime.Eventos;

namespace Autis.Runtime.ComponentesGameObjects {
    [RequireComponent(typeof(Animator))]
    [AddComponentMenu("AUTIS/Personagem/Controle Indireto")]
    public class ControleIndireto : MonoBehaviour {
        public EventoAnimationClip eventoAcionarAcaoPersonagem;

        private Animator animator;
        private DadosPersonagem dados;

        private void Awake() {
            dados = GetComponent<DadosPersonagem>();
            if(dados.tipoControle != TipoControle.Indireto) {
                this.enabled = false;
                return;
            }

            animator = GetComponent<Animator>();
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