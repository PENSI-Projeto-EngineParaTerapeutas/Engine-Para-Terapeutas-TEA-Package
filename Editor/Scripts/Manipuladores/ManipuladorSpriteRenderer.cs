using UnityEngine;

namespace Autis.Editor.Manipuladores {
    public class ManipuladorSpriteRenderer {
        #region .: Componentes :.

        public SpriteRenderer ComponenteSpriteRenderer { get => componenteSpriteRenderer; set { componenteSpriteRenderer = value; } }
        protected SpriteRenderer componenteSpriteRenderer;

        #endregion

        public ManipuladorSpriteRenderer(SpriteRenderer componenteSpriteRenderer) {
            this.componenteSpriteRenderer = componenteSpriteRenderer;
            return;
        }

        public void SetCor(Color cor) {
            if(componenteSpriteRenderer == null) {
                return;
            }

            componenteSpriteRenderer.color = cor;
            return;
        }

        public Color GetCor() {
            return componenteSpriteRenderer.color;
        }

        public void SetImagem(Sprite sprite) {
            if(componenteSpriteRenderer == null) {
                return;
            }

            componenteSpriteRenderer.sprite = sprite;
            return;
        }

        public void SetImagem(Texture2D texture) {
            if(componenteSpriteRenderer == null) {
                return;
            }

            componenteSpriteRenderer.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return;
        }

        public Sprite GetImagem() {
            return componenteSpriteRenderer.sprite;
        }

        public void SetEspelhar(bool deveEspelhar) {
            if(componenteSpriteRenderer == null) {
                return;
            }

            componenteSpriteRenderer.flipX = deveEspelhar;
            return;
        }

        public bool EstaEspelhado() {
            return componenteSpriteRenderer.flipX;
        }
    }
}