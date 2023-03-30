using UnityEngine;
using UnityEngine.UIElements;

namespace Autis.Runtime.Telas {
    public abstract class TelaJogo : MonoBehaviour {
        [SerializeField]
        protected UIDocument uiDocument;

        [SerializeField]
        protected StyleSheet style;

        public VisualElement Root { 
            get {
                if(uiDocument == null) {
                    return null;
                }

                if(root == null) {
                    root = uiDocument.rootVisualElement;
                }

                return root;
            }
        }
        private VisualElement root;
    }
}