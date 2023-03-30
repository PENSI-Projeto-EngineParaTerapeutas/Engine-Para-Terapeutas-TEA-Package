using UnityEngine;
using EngineParaTerapeutas.Eventos;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.ComponentesGameObjects {
    [AddComponentMenu("Engine Terapeutas TEA/Objeto Interação/Acionador Ação Personagem")]
    public class AcionadorAcaoPersonagem : MonoBehaviour {
        public AnimationClip animacaoAcionada;

        private EventoAcionarAcaoPersonagem eventoAcionarAcaoPersonagem;

        private bool temPersonagem = false;

        private void Awake() {
            temPersonagem = GameObject.FindGameObjectWithTag(NomesTags.Personagem) != null;
            if(!temPersonagem || animacaoAcionada == null) {
                this.enabled = false;
                return;
            }

            eventoAcionarAcaoPersonagem = Resources.Load<EventoAcionarAcaoPersonagem>("ScriptableObjects/EventoAcionarAcaoPersonagem");

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