using UnityEditor;
using UnityEngine.UIElements;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    public abstract class CustomEditorBase : Editor {
        protected VisualElement root;
        protected VisualTreeAsset template;
        protected StyleSheet style;

        protected StyleSheet defaultStyle;

        public virtual void OnEnable() {
            root = new VisualElement();

            ImportarDefaultStyle();

            return;
        }

        protected virtual void ImportarDefaultStyle() {
            defaultStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>(ConstantesEditor.PastaRaiz + "/Compartilhado/ClassesPadroesEditorStyle.uss"); // TODO: Utilizar path
            root.styleSheets.Add(defaultStyle);

            return;
        }

        protected virtual void ImportarTemplate(string caminho) {
            template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ConstantesEditor.PastaRaiz + caminho); // TODO: Utilizar path
            root.Add(template.Instantiate());

            return;
        }

        protected virtual void ImportarStyle(string caminho) {
            style = AssetDatabase.LoadAssetAtPath<StyleSheet>(ConstantesEditor.PastaRaiz + caminho); // TODO: Utilizar path
            root.styleSheets.Add(style);

            return;
        }

        public override VisualElement CreateInspectorGUI() {
            return root;
        }

        protected abstract void ConfigurarInputs();
    }
}