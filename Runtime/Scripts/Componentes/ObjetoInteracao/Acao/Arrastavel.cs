using UnityEngine;

namespace Autis.Runtime.ComponentesGameObjects {
    [AddComponentMenu("AUTIS/Objeto Interação/Arrastável")]
    public class Arrastavel : MonoBehaviour {
        public bool habilitado = true;

        private Rigidbody2D body2D;

        private void Awake() {
            body2D = GetComponent<Rigidbody2D>();
            return;
        }

        private void OnMouseDrag() {
            if(!habilitado) {
                return;
            }

            Vector3 posicaoMouse = Input.mousePosition;
            posicaoMouse.z = Camera.main.nearClipPlane;
            Vector2 novaPosicaoObjeto = Camera.main.ScreenToWorldPoint(posicaoMouse);

            body2D.MovePosition(novaPosicaoObjeto);

            return;
        }
    }
}