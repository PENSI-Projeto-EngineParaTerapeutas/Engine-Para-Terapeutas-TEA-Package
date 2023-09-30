using System.Collections.Generic;
using UnityEngine;
using Autis.Runtime.Constantes;
using Autis.Runtime.DTOs;

namespace Autis.Runtime.ComponentesGameObjects {
    [AddComponentMenu("AUTIS/Objeto Interação/Acionador Apoios")]
    public class AcionadorApoios : MonoBehaviour {
        private const float DISTANCIA_LIMITE_DETECCAO_CLICK = 1.0f;
        
        public bool habilitado = true;

        private Vector3 clickStartPosition = Vector3.zero;
        private readonly List<ListenerEventosApoioInteracao> listenerApoiosSelecao = new();

        private void Awake() {
            foreach(Transform child in transform) {
                if(!child.CompareTag(NomesTags.Apoios)) {
                    continue;
                }

                ListenerEventosApoioInteracao listener = child.GetComponent<ListenerEventosApoioInteracao>();
                if(listener.TipoAcionamento != TipoAcionamentoApoioObjetoInteracao.Selecao) {
                    continue;
                }

                listenerApoiosSelecao.Add(listener);
            }

            return;
        }

        private void OnMouseDown() {
            clickStartPosition = Input.mousePosition;
            return;
        }

        private void OnMouseUpAsButton() {
            if(!habilitado || Vector3.Distance(clickStartPosition, Input.mousePosition) > DISTANCIA_LIMITE_DETECCAO_CLICK) {
                return;
            }

            foreach(ListenerEventosApoioInteracao listenerSelecao in listenerApoiosSelecao) {
                listenerSelecao.AcionarComponentes();
            }

            return;
        }
    }
}