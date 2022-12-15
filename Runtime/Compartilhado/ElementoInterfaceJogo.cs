using UnityEngine;
using UnityEngine.UIElements;

namespace EngineParaTerapeutas.UI {
    public abstract class ElementoInterfaceJogo : ElementoInterface {
        protected ElementoInterfaceJogo() {}

        protected override void ImportarDefaultStyle() {
            defaultStyle = Resources.Load<StyleSheet>("Scripts/Compartilhado/ClassesPadroesStyle");
            Root.styleSheets.Add(defaultStyle);

            return;
        }

        protected override void ImportarTemplate(string caminho) {
            template = Resources.Load<VisualTreeAsset>(caminho);
            Root.Add(template.Instantiate());

            return;
        }

        protected override void ImportarStyle(string caminho) {
            style = Resources.Load<StyleSheet>(caminho);
            Root.styleSheets.Add(style);

            return;
        }
    }
}