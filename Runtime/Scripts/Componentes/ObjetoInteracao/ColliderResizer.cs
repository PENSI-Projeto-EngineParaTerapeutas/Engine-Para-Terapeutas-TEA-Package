using UnityEngine;
using Autis.Runtime.DTOs;

namespace Autis.Runtime.ComponentesGameObjects {
    [AddComponentMenu("AUTIS/Objeto Interação/Collider Resizer")]
    public class ColliderResizer : MonoBehaviour {
        private BoxCollider2D boxCollider2D;

        private void Awake() {
            boxCollider2D = GetComponent<BoxCollider2D>();
            return;
        }

        private void Start() {
            IdentificadorTipoObjetoInteracao identificadorTipo = GetComponent<IdentificadorTipoObjetoInteracao>();

            if(identificadorTipo.Tipo == TiposObjetosInteracao.Imagem) {
                AjustarTamanhoColliderObjetoComSprite();
            }
            else {
                AjustarTamanhoColliderObjetoComTexto();
            }

            return;
        }

        private void AjustarTamanhoColliderObjetoComSprite() {
            Renderer renderer = GetComponent<Renderer>();
            boxCollider2D.size = renderer.localBounds.size;

            return;
        }

        private void AjustarTamanhoColliderObjetoComTexto() {
            Texto componenteTexto = GetComponent<Texto>();
            Vector2 tamanhoRenderizadoTexto = new(componenteTexto.TextMesh.preferredWidth, componenteTexto.TextMesh.preferredHeight);

            boxCollider2D.size = tamanhoRenderizadoTexto;

            return;
        }
    }
}