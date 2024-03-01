using System.Collections.Generic;
using UnityEngine;

namespace Autis.Runtime.ComponentesGameObjects {
    public class ControladorEventosAnimacao : MonoBehaviour {
        [SerializeField]
        private List<GameObject> roupasAnimacoes;

        public void EnableClothing(string roupa) {
            if(string.IsNullOrEmpty(roupa)) {
                return;
            }

            SetEnableClothing(roupa, true);
        }

        public void DisableClothing(string roupa) {
            if(string.IsNullOrEmpty(roupa)) {
                return;
            }

            SetEnableClothing(roupa, false);
        }

        private void SetEnableClothing(string roupa, bool estaAbilitado) {
            GameObject gameObjectRoupa = roupasAnimacoes.Find(gameObject =>  gameObject.name.ToLower() == roupa.ToLower());
            SpriteRenderer spriteRoupa = gameObjectRoupa.GetComponent<SpriteRenderer>();
            spriteRoupa.enabled = estaAbilitado;

            return;
        }
    }
}