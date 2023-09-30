using UnityEngine;
using Autis.Runtime.Constantes;
using Autis.Runtime.Eventos;

namespace Autis.Runtime.ComponentesGameObjects {
    [AddComponentMenu("AUTIS/Objeto Interação/Verificação Gabarito Seleção")]
    public class VerificacaoGabaritoSelecao : MonoBehaviour {
        public bool habilitado;

        public bool ehOpcaoCorreta = false;
        public bool ordemImporta = false;
        public int numeroOrdemSelecao = -1;

        #region .: Eventos :.

        [SerializeField]
        private EventoJogo eventoAcerto;

        [SerializeField]
        private EventoJogo eventoErro;

        [SerializeField]
        private EventoInteiro eventoErroComOrdem;

        #endregion

        private bool jaFoiSelecionado = false;
        private AcionadorApoios componenteSelecao;

        private GameObject objetoGabarito;
        private Gabarito gabarito;

        private void Awake() {
            objetoGabarito = GameObject.FindGameObjectWithTag(NomesTags.Gabarito);
            return;
        }

        private void Start() {
            gabarito = objetoGabarito.GetComponent<Gabarito>();
            componenteSelecao = GetComponent<AcionadorApoios>();

            return;
        }

        private void OnMouseUpAsButton() {
            if(!habilitado || jaFoiSelecionado) {
                return;
            }

            if(!ehOpcaoCorreta && !ordemImporta) {
                eventoErro.AcionarCallbacks();
                return;
            }
            
            if(!ehOpcaoCorreta && ordemImporta) {
                eventoErroComOrdem.AcionarCallbacks(gabarito.NumeroEtapaAtual);
                return;
            }

            if(ehOpcaoCorreta && ordemImporta && gabarito.NumeroEtapaAtual != numeroOrdemSelecao) {
                eventoErroComOrdem.AcionarCallbacks(gabarito.NumeroEtapaAtual);
                return;
            }

            jaFoiSelecionado = true;
            componenteSelecao.enabled = false;
            componenteSelecao.habilitado = false;

            eventoAcerto.AcionarCallbacks();

            return;
        }
    }
}