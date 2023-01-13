using UnityEditor;
using UnityEngine.UIElements;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.UI {
    public abstract class ElementoInterfaceEditor : ElementoInterface {
        protected ElementoInterfaceEditor() {}

        protected override void ImportarDefaultStyle() {
            defaultStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>(ConstantesEditor.PastaRaiz + "Compartilhado/ClassesPadroesEditorStyle.uss");
            Root.styleSheets.Add(defaultStyle);

            return;
        }

        protected override void ImportarTemplate(string caminho) {
            template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ConstantesEditor.PastaRaiz + caminho);
            Root.Add(template.Instantiate());

            return;
        }

        protected override void ImportarStyle(string caminho) {
            style = AssetDatabase.LoadAssetAtPath<StyleSheet>(ConstantesEditor.PastaRaiz + caminho); // TODO: Utilizar path
            Root.styleSheets.Add(style);

            return;
        }
    }
}