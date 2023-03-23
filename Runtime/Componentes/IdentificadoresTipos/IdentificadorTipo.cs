using System;
using System.Collections;
using UnityEngine;

namespace EngineParaTerapeutas.ComponentesGameObjects {
    public abstract class IdentificadorTipo<T> : MonoBehaviour where T : Enum {
        private const float TEMPO_ESPERA_PADRAO = 2f;

        public T Tipo { get => tipo; }
        [SerializeField]
        protected T tipo;

        protected Coroutine corrotinaDesabilitarAutomatico;

        #if UNITY_EDITOR
        public abstract void AlterarTipo(T novoTipo);
        #endif

        public virtual void HabilitarComponentes() {
            HabilitarComponentes(Tipo);
            return;
        }

        public abstract void HabilitarComponentes(T tipo);

        public abstract void DesabilitarComponentes();

        public void IniciarCorrotinaDesabilitarComponentes(float tempoEspera = TEMPO_ESPERA_PADRAO) {
            if(corrotinaDesabilitarAutomatico != null) {
                FinalizarCorrotinaDesabilitarComponentes();
            }

            corrotinaDesabilitarAutomatico = StartCoroutine(DesabilitarComponentesAutomaticamente(tempoEspera));
            return;
        }

        private IEnumerator DesabilitarComponentesAutomaticamente(float tempoEspera = TEMPO_ESPERA_PADRAO) {
            yield return new WaitForSeconds(tempoEspera);

            DesabilitarComponentes();

            FinalizarCorrotinaDesabilitarComponentes();
            yield break;
        }

        public void FinalizarCorrotinaDesabilitarComponentes() {
            if(corrotinaDesabilitarAutomatico == null) {
                return;
            }

            StopCoroutine(corrotinaDesabilitarAutomatico);
            return;
        }
    }
}