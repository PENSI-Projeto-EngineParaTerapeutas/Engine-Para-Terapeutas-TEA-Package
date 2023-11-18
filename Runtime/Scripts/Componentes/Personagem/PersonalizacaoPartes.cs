using UnityEngine;
using System.Collections.Generic;

namespace Autis.Runtime.ComponentesGameObjects {
    public class PersonalizacaoPartes : MonoBehaviour {
        public List<SpriteRenderer> Cabelos { get => cabelos; }
        [SerializeField]
        private List<SpriteRenderer> cabelos;

        public List<ConjuntoRoupas> Roupas { get => roupas; }
        [SerializeField]
        private List<ConjuntoRoupas> roupas;

        public void SetCabelo(int indice) {
            for(int i = 0; i < cabelos.Count; i++) {
                bool estaAtivo = i == indice;
                cabelos[i].gameObject.SetActive(estaAtivo);
            }

            return;
        }

        public void SetConjuntoRoupa(int indice) {
            for(int i = 0; i < roupas.Count; i++) {
                bool estaAtivo = i == indice;
                roupas[i].SetActive(estaAtivo);
            }

            return;
        }
    }
}