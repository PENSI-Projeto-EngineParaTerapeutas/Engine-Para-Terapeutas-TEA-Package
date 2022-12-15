using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    public abstract class CustomEditor<T, U> : Editor where T : MonoBehaviour where U : Component {
        protected VisualElement root;
        protected VisualTreeAsset template;
        protected StyleSheet style;

        protected StyleSheet defaultStyle;

        protected T componente;
        protected U componenteOriginal;

        public virtual void OnEnable() {
            componente = target as T;
            componenteOriginal = componente.GetComponent<U>();
            root = new VisualElement();

            ImportarDefaultStyle();
            AlterarVisibilidadeComponenteOriginal(HideFlags.HideInInspector);
            return;
        }

        protected virtual void ImportarDefaultStyle() {
            defaultStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.pensi.engine-para-terapeutas-tea/Editor/Compartilhado/ClassesPadroesEditorStyle.uss");
            root.styleSheets.Add(defaultStyle);

            return;
        }

        protected virtual void ImportarTemplate(string caminho) {
            template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(caminho);
            root.Add(template.Instantiate());

            return;
        }

        protected virtual void ImportarStyle(string caminho) {
            style = AssetDatabase.LoadAssetAtPath<StyleSheet>(caminho);
            root.styleSheets.Add(style);

            return;
        }

        protected virtual void AlterarVisibilidadeComponenteOriginal(HideFlags flag) {
            if(componenteOriginal == null) {
                return;
            }

            componenteOriginal.hideFlags = flag;
            return;
        }

        public override VisualElement CreateInspectorGUI() {
            return root;
        }

        public virtual void OnDisable() {
            AlterarVisibilidadeComponenteOriginal(HideFlags.None);
            return;
        }

        public void OnDestroy() {
            componenteOriginal = null;
            componente = null;

            return;
        }

        protected abstract void ConfigurarInputs();
    }
}