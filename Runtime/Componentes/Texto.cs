using TMPro;
using UnityEngine;

namespace EngineParaTerapeutas.ComponentesGameObjects {
    [AddComponentMenu("Engine Terapeutas TEA/Texto")]
    public class Texto : MonoBehaviour {
        private const string NOME_CANVAS = "Canvas";

        public TextMeshProUGUI TextMesh { 
            get {
                if(textMesh != null) {
                    return textMesh;
                }

                if(canvas == null) {
                    canvas = transform.Find(NOME_CANVAS);
                }

                textMesh = canvas.GetComponentInChildren<TextMeshProUGUI>();
                return textMesh;
            }
        }
        private TextMeshProUGUI textMesh = null;

        public Transform Canvas {
            get {
                if(canvas != null) {
                    return canvas;
                }

                canvas = transform.Find(NOME_CANVAS);
                return canvas;
            }
        }
        private Transform canvas = null;
    }
}