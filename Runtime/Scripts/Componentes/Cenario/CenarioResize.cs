using UnityEngine;

namespace Autis.Runtime.ComponentesGameObjects {
    public class CenarioResize : MonoBehaviour {
        private SpriteRenderer ComponenteSpriteRenderer { 
            get {
                if(componenteSpriteRenderer != null) {
                    return componenteSpriteRenderer;
                }

                componenteSpriteRenderer = GetComponent<SpriteRenderer>();
                return componenteSpriteRenderer;
            }
        }
        private SpriteRenderer componenteSpriteRenderer;

        private Vector2 dimensoesTela;

        private void Awake() {
            componenteSpriteRenderer = GetComponent<SpriteRenderer>();
            return;
        }

        private void Start() {
            dimensoesTela = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
            Resize();

            return;
        }

        private void Update() {
            if(dimensoesTela.x != Camera.main.pixelWidth || dimensoesTela.y != Camera.main.pixelHeight) {
                dimensoesTela = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
                Resize();
            }

            return;
        }

        public void Resize() {
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);

            Vector3 cantoSuperiorDireito = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, Camera.main.transform.position.z));
            float larguraEmMundo = cantoSuperiorDireito.x * 2;
            float alturaEmMundo = cantoSuperiorDireito.y * 2;

            Vector3 tamanhoSprite = ComponenteSpriteRenderer.sprite.bounds.size;

            float escalaEixoX = larguraEmMundo / tamanhoSprite.x;
            float escalaEixoY = Mathf.Abs(alturaEmMundo / tamanhoSprite.y);

            transform.localScale = new Vector3(escalaEixoX, escalaEixoY, -1);

            return;
        }
    }
}