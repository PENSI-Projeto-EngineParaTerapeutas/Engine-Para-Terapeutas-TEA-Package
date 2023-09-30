using UnityEngine;
using System.Collections.Generic;

namespace Autis.Runtime.ComponentesGameObjects {
    public class PartesPersonalizaveis : MonoBehaviour {
        public SpriteRenderer Olhos { get => olhos; }
        [SerializeField]
        private SpriteRenderer olhos;
        
        public List<SpriteRenderer> Cabelos { get => cabelos; }
        [SerializeField]
        private List<SpriteRenderer> cabelos;

        public List<SpriteRenderer> PartesCorpo { get => partesCorpo; }
        [SerializeField]
        private List<SpriteRenderer> partesCorpo;

        public List<SpriteRenderer> RoupasSuperiores { get => roupasSuperiores; }
        [SerializeField]
        private List<SpriteRenderer> roupasSuperiores;

        public List<SpriteRenderer> RoupasInferiores { get => roupasInferiores; }
        [SerializeField]
        private List<SpriteRenderer> roupasInferiores;

        public List<ConjuntoRoupas> Roupas { get => roupas; }
        [SerializeField]
        private List<ConjuntoRoupas> roupas;
    }
}