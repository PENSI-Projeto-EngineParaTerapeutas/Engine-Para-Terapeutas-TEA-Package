using UnityEngine;
using Autis.Runtime.Eventos;
using System.Collections.Generic;

namespace Autis.Runtime.ComponentesGameObjects {
    [AddComponentMenu("AUTIS/Objeto Interação/Verificação Gabarito Arrastar")]
    public class VerificacaoGabaritoArrastar : MonoBehaviour {
        public bool habilitado;
        public bool deveRetornarPosicaoInicial;
        public Transform objetoDestinoCorreto;

        #region .: Eventos :.

        [SerializeField]
        private EventoJogo eventoAcerto;

        [SerializeField]
        private EventoJogo eventoErro;

        #endregion

        private Rigidbody2D body2d;
        private BoxCollider2D boxCollider2d;
        private Arrastavel arrastavel;
        private Vector3 posicaoInicial = Vector3.zero;

        private void Awake() {
            body2d = GetComponent<Rigidbody2D>();
            boxCollider2d = GetComponent<BoxCollider2D>();
            arrastavel = GetComponent<Arrastavel>();

            posicaoInicial = transform.position;

            return;
        }

        private void OnMouseUpAsButton() {
            if(!habilitado) {
                return;
            }

            ContactFilter2D filtro = new() {
                useTriggers = true,
            };
            List<Collider2D> colisoesAtuais = new();
            boxCollider2d.OverlapCollider(filtro, colisoesAtuais);

            if(colisoesAtuais.Count <= 0) {
                RetornarPosicaoOriginal();
                return;
            }

            Collider2D colisiorObjetoDestino = colisoesAtuais.Find(collider => collider.transform == objetoDestinoCorreto);
            if(colisiorObjetoDestino == null) {
                eventoErro.AcionarCallbacks();
                RetornarPosicaoOriginal();
                return;
            }

            body2d.MovePosition(objetoDestinoCorreto.position);
            arrastavel.habilitado = false;
            habilitado = false;

            eventoAcerto.AcionarCallbacks();
            return;
        }

        private void RetornarPosicaoOriginal() {
            if(!deveRetornarPosicaoInicial) {
                return;
            }

            body2d.MovePosition(posicaoInicial);
            return;
        }
    }
}