using UnityEngine;
using Autis.Runtime.DTOs;
using System.Collections.Generic;

namespace Autis.Runtime.ComponentesGameObjects {
    [AddComponentMenu("AUTIS/Personagem/Controle Direto")]
    public class ControleDireto : MonoBehaviour {
        [SerializeField]
        private float PASSO_ROTACAO = 0.15f;

        public List<Transform> PartesCorpo { get => partesCorpo; }

        [SerializeField]
        private List<Transform> partesCorpo;

        private IdentificadorTipoControle tipoControle;

        private void Awake() {
            tipoControle = GetComponent<IdentificadorTipoControle>();
            if(tipoControle.Tipo != TipoControle.Direto) {
                this.enabled = false;
            }


            return;
        }

        public void RotacionarSentidoHorario(Transform parte) {
            if(parte == null) {
                return;
            }

            parte.rotation = new Quaternion(parte.rotation.x, parte.rotation.y, parte.rotation.z + PASSO_ROTACAO, parte.rotation.w);
            return;
        }

        public void RotacionarSentidoAntiHorario(Transform parte) {
            if(parte == null) {
                return;
            }

            parte.rotation = new Quaternion(parte.rotation.x, parte.rotation.y, parte.rotation.z - PASSO_ROTACAO, parte.rotation.w);
            return;
        }
    }
}