using System.Collections.Generic;
using UnityEngine;

namespace Autis.Runtime.ComponentesGameObjects {
    public class PersonalizacaoCores : MonoBehaviour {
        public List<SpriteRenderer> Olhos { get => olhos; }
        [SerializeField]
        private List<SpriteRenderer> olhos;

        public Color CorOlhos { get => corOlhos; }
        [SerializeField]
        private Color corOlhos;

        public List<SpriteRenderer> Cabelos { get => cabelos; }
        [SerializeField]
        private List<SpriteRenderer> cabelos;

        public Color CorCabelo { get => corCabelo; }
        [SerializeField]
        private Color corCabelo;

        public List<SpriteRenderer> PartesCorpo { get => partesCorpo; }
        [SerializeField]
        private List<SpriteRenderer> partesCorpo;

        public Color CorPele { get => corPele; }
        [SerializeField]
        private Color corPele;

        public List<SpriteRenderer> RoupasSuperiores { get => roupasSuperiores; }
        [SerializeField]
        private List<SpriteRenderer> roupasSuperiores;

        public Color CorRoupaSuperior { get => corRoupaSuperior; }
        [SerializeField]
        private Color corRoupaSuperior;

        public List<SpriteRenderer> RoupasInferiores { get => roupasInferiores; }
        [SerializeField]
        private List<SpriteRenderer> roupasInferiores;

        public Color CorRoupaInferior { get => corRoupaInferior; }
        [SerializeField]
        private Color corRoupaInferior;

        public void SetCorOlhos(Color cor) {
            corOlhos = cor;

            foreach(SpriteRenderer spriteRendererOlhos in olhos) {
                spriteRendererOlhos.color = corOlhos;
            }

            return;
        }

        public void SetCorCabelo(Color cor) {
            corCabelo = cor;

            foreach(SpriteRenderer spriteRendererCabelo in cabelos) {
                spriteRendererCabelo.color = corCabelo;
            }

            return;
        }

        public void SetCorPele(Color cor) {
            corPele = cor;

            foreach(SpriteRenderer spriteRendererParte in partesCorpo) {
                spriteRendererParte.color = corPele;
            }

            return;
        }

        public void SetCorRoupaSuperior(Color cor) {
            corRoupaSuperior = cor;

            foreach(SpriteRenderer spriteRendererRoupa in roupasSuperiores) {
                spriteRendererRoupa.color = corRoupaSuperior;
            }

            return;
        }

        public void SetCorRoupaInferior(Color cor) {
            corRoupaInferior = cor;

            foreach(SpriteRenderer spriteRendererRoupa in roupasInferiores) {
                spriteRendererRoupa.color = corRoupaInferior;
            }

            return;
        }
    }
}