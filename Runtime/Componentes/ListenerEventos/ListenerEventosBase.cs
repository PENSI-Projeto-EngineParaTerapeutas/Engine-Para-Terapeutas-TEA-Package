using UnityEngine;
using EngineParaTerapeutas.Eventos;
using EngineParaTerapeutas.DTOs;

namespace EngineParaTerapeutas.ComponentesGameObjects {
    public abstract class ListenerEventosBase : MonoBehaviour {
        public TipoAcionamento TipoAcionamento { get => tipoAcionamento; set { tipoAcionamento = value; } }
        [SerializeField]
        protected TipoAcionamento tipoAcionamento;

        protected EventoJogo eventoErro;
        protected EventoJogo eventoAcerto;
        protected EventoJogo eventoFimJogo;

        protected virtual void Awake() {
            eventoErro = Resources.Load<EventoJogo>("ScriptableObjects/EventoErro");
            eventoAcerto = Resources.Load<EventoJogo>("ScriptableObjects/EventoAcerto");
            eventoFimJogo = Resources.Load<EventoJogo>("ScriptableObjects/EventoFimJogo");

            return;
        }

        protected virtual void Start() {
            eventoErro.AdicionarCallback(HandleEventoErro);
            eventoAcerto.AdicionarCallback(HandleEventoAcerto);
            eventoFimJogo.AdicionarCallback(HandleEventoFimJogo);

            return;
        }

        public virtual void HandleEventoErro() {
            if(tipoAcionamento != TipoAcionamento.Erro || !gameObject.activeInHierarchy) {
                return;
            }

            AcionarComponentes();
            return;
        }

        protected abstract void AcionarComponentes();

        public virtual void HandleEventoAcerto() {
            if(tipoAcionamento != TipoAcionamento.Acerto || !gameObject.activeInHierarchy) {
                return;
            }

            AcionarComponentes();
            return;
        }

        public virtual void HandleEventoFimJogo() {
            if(tipoAcionamento != TipoAcionamento.FimJogo || !gameObject.activeInHierarchy) {
                return;
            }

            AcionarComponentes();
            return;
        }
    }
}