using UnityEngine;

namespace Autis.Runtime.ComponentesGameObjects {
    public class InformacoesParteControle : MonoBehaviour {
        public string NomeDisplay { get => nomeDisplay; }

        [SerializeField]
        private string nomeDisplay;

        [SerializeField]
        private SpriteRenderer sprite;

        [SerializeField]
        private int ordemRenderizacao;

        private void Start() {
            if(sprite != null) {
                sprite.sortingOrder = ordemRenderizacao;
            }

            return;
        }
    }
}