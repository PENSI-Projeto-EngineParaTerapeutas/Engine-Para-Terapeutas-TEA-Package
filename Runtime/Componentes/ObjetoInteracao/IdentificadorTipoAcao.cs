using UnityEngine;
using EngineParaTerapeutas.DTOs;

namespace EngineParaTerapeutas.ComponentesGameObjects {
    [AddComponentMenu("Engine Terapeutas TEA/Identificador Tipo/Identificador Tipo Ação do Objeto de Interação")]
    public class IdentificadorTipoAcao : IdentificadorTipo<TiposAcoes> {
        private Arrastavel componenteArrastar;
        private Selecionavel componenteSelecao;

        private void Awake() {
            componenteArrastar = GetComponent<Arrastavel>();
            componenteSelecao = GetComponent<Selecionavel>();

            return;
        }

        #if UNITY_EDITOR
        public override void AlterarTipo(TiposAcoes novoTipo) {
            tipo = novoTipo;

            componenteArrastar = GetComponent<Arrastavel>();
            componenteSelecao = GetComponent<Selecionavel>();

            DesabilitarComponentes();
            HabilitarComponentes();

            return;
        }
        #endif

        public override void HabilitarComponentes(TiposAcoes tipo) {
            switch(tipo) {
                case(TiposAcoes.Arrastavel): {
                    componenteArrastar.enabled = true;
                    componenteArrastar.habilitado = true;
                    break;
                }
                case(TiposAcoes.Selecionavel): {
                    componenteSelecao.enabled = true;
                    componenteSelecao.habilitado = true;
                    break;
                }
            }

            return;
        }

        public override void DesabilitarComponentes() {
            componenteArrastar.enabled = false;
            componenteArrastar.habilitado = false;

            componenteSelecao.enabled = false;
            componenteSelecao.habilitado = false;

            return;
        }
    }
}
