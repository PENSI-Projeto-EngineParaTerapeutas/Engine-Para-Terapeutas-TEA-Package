using System;
using System.Collections.Generic;
using UnityEngine;

namespace Autis.Runtime.ComponentesGameObjects {
    [Serializable]
    public class ConjuntoRoupas {
        public List<SpriteRenderer> Roupas { get => roupas; }
        [SerializeField]
        private List<SpriteRenderer> roupas;

        public void SetActive(bool enable) {
            foreach(SpriteRenderer roupa in roupas) {
                roupa.gameObject.SetActive(enable);
            }

            return;
        }
    }
}