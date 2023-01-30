using UnityEngine;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    public abstract class CustomEditorComponentes<T, U> : CustomEditorBase where T : MonoBehaviour where U : Component {
        protected T componente;
        protected U componenteOriginal;

        public override void OnEnable() {
            base.OnEnable();

            componente = target as T;
            componenteOriginal = componente.GetComponent<U>();

            AlterarVisibilidadeComponenteOriginal(HideFlags.HideInInspector);
            return;
        }

        protected virtual void AlterarVisibilidadeComponenteOriginal(HideFlags flag) {
            if(componenteOriginal == null) {
                return;
            }

            componenteOriginal.hideFlags = flag;
            return;
        }

        public virtual void OnDisable() {
            AlterarVisibilidadeComponenteOriginal(HideFlags.None);
            return;
        }

        public virtual void OnDestroy() {
            componenteOriginal = null;
            componente = null;

            return;
        }
    }
}