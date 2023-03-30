using UnityEngine;
using UnityEngine.UIElements;
using Autis.Runtime.Constantes;

namespace Autis.Runtime.UI {
    public abstract class ElementoInterfaceJogo : ElementoInterface {
        protected override void ImportarDefaultStyle() {
            defaultStyle = Resources.Load<StyleSheet>(ConstantesRuntime.CaminhoClassesPadroesUSS);
            Root.styleSheets.Add(defaultStyle);

            return;
        }

        protected override void ImportarTemplate(string caminho) {
            template = Resources.Load<VisualTreeAsset>(caminho);
            return;
        }

        protected override void ImportarStyle(string caminho) {
            style = Resources.Load<StyleSheet>(caminho);
            Root.styleSheets.Add(style);

            return;
        }
    }
}