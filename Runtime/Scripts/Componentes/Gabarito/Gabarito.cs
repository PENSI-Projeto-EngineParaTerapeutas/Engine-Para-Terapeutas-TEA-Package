using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Autis.Runtime.Eventos;
using Autis.Runtime.Constantes;
using Autis.Runtime.DTOs;

namespace Autis.Runtime.ComponentesGameObjects {
    public class Gabarito : MonoBehaviour {
        #region .: Eventos :.

        [SerializeField]
        private EventoJogo eventoAcerto;

        [SerializeField]
        private EventoJogo eventoAcionarReforcosAcerto;

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

            if(contadorAcertos < maximoAcertos) {
                eventoAcionarReforcosAcerto.AcionarCallbacks();
                return;
            } 

            if(contadorAcertos >= maximoAcertos && possuiLimiteAcertos) {
                List<GameObject> reforcos = GameObject.FindGameObjectsWithTag(NomesTags.Reforcos).ToList();

                bool possuiReforcoFimJogo = reforcos.Any(objeto => objeto.GetComponent<ListenerEventosReforco>().TipoAcionamento == TipoAcionamentoReforco.FimJogo);
                if(possuiReforcoFimJogo) {
                    eventoFimJogo.AcionarCallbacks();
                }
                else {
                    eventoAcionarReforcosAcerto.AcionarCallbacks();
                }
            }

            return;
        }
    }
}