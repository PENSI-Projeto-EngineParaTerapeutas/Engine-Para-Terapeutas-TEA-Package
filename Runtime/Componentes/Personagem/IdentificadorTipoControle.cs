using UnityEngine;
using EngineParaTerapeutas.DTOs;

namespace EngineParaTerapeutas.ComponentesGameObjects {
    [RequireComponent(typeof(ControleDireto))]
    [RequireComponent(typeof(ControleIndireto))]
    [AddComponentMenu("Engine Terapeutas TEA/Controles Personagem/Identificador Tipo Controle")]
    public class IdentificadorTipoControle : IdentificadorTipo<TipoControle> {
        private ControleDireto controleDireto;
        private ControleIndireto controleIndireto;
        private Animator animator;

        private void Awake() {
            controleDireto = GetComponent<ControleDireto>();
            controleIndireto = GetComponent<ControleIndireto>();
            animator = GetComponent<Animator>();

            return;
        }

        #if UNITY_EDITOR
        public override void AlterarTipo(TipoControle novoTipo) {
            tipo = novoTipo;

            controleDireto = GetComponent<ControleDireto>();
            controleIndireto = GetComponent<ControleIndireto>();
            animator = GetComponent<Animator>();

            DesabilitarComponentes();
            HabilitarComponentes();

            return;
        }
        #endif

        public override void HabilitarComponentes(TipoControle tipo) {
            switch(tipo) {
                case(TipoControle.Direto): {
                    controleDireto.enabled = true;
                    break;
                }
                case(TipoControle.Indireto): {
                    controleIndireto.enabled = true;
                    animator.enabled = true;
                    break;
                }
            }

            return;
        }

        public override void DesabilitarComponentes() {
            controleDireto.enabled = false;
            controleIndireto.enabled = false;
            animator.enabled = false;

            return;
        }
    }
}