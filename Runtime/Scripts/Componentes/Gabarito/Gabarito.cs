using UnityEngine;
using Autis.Runtime.Eventos;

namespace Autis.Runtime.ComponentesGameObjects {
    public class Gabarito : MonoBehaviour {
        #region .: Eventos :.

        [SerializeField]
        private EventoJogo eventoAcerto;

        [SerializeField]
        private EventoJogo eventoFimJogo;

        #endregion

        public int maximoAcertos = 0;
        public bool possuiLimiteAcertos = true;

        public int NumeroEtapaAtual { get => contadorAcertos; }
        private int contadorAcertos = 0;

        private void Awake() {
            eventoAcerto.AdicionarCallback(IncrementarContadorAcertos);
            return;
        }

        private void IncrementarContadorAcertos() {
            contadorAcertos++;

            if(contadorAcertos >= maximoAcertos && possuiLimiteAcertos) {
                eventoFimJogo.AcionarCallbacks();
            }

            return;
        }
    }
}