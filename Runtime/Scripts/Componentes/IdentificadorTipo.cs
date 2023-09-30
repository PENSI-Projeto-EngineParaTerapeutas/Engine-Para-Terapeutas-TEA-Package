using System;
using System.Collections;
using UnityEngine;

namespace Autis.Runtime.ComponentesGameObjects {
    public abstract class IdentificadorTipo<T> : MonoBehaviour where T : Enum {
        private const float TEMPO_ESPERA_PADRAO = 2f;
        protected static readonly Vector2 POSICAO_PADRAO_ATOR_AUDIO = new(-9, 4);

        public float tempoEspera = 2f;

        public T Tipo { get => tipo; }
        public T tipo;

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

        public void IniciarCorrotinaDesabilitarComponentes() {
            IniciarCorrotinaDesabilitarComponentes(tempoEspera);
            return;
        }

        public void IniciarCorrotinaDesabilitarComponentes(float tempoEspera) {
            if(corrotinaDesabilitarAutomatico != null) {
                FinalizarCorrotinaDesabilitarComponentes();
            }

            corrotinaDesabilitarAutomatico = StartCoroutine(DesabilitarComponentesAutomaticamente(tempoEspera));
            return;
        }

        private IEnumerator DesabilitarComponentesAutomaticamente(float tempoEspera) {
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