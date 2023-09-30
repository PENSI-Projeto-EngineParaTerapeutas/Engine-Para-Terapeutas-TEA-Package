using UnityEngine;
using Autis.Runtime.Eventos;
using Autis.Runtime.Constantes;

namespace Autis.Runtime.ComponentesGameObjects {
    [AddComponentMenu("AUTIS/Objeto Interação/Acionador Ação Personagem")]
    public class AcionadorAcaoPersonagem : MonoBehaviour {
        public AnimationClip animacaoAcionada;

        [SerializeField]
        private EventoAnimationClip eventoAcionarAcaoPersonagem;

        private bool temPersonagem = false;

        private void Awake() {
            temPersonagem = GameObject.FindGameObjectWithTag(NomesTags.Personagem) != null;
            if(!temPersonagem || animacaoAcionada == null) {
                this.enabled = false;
                return;
            }

            return;
        }

        private void OnMouseUpAsButton() {
            if(!temPersonagem || animacaoAcionada == null) {
                return;
            }

            eventoAcionarAcaoPersonagem.AcionarCallbacks(animacaoAcionada);
            return;
        }
    }
}