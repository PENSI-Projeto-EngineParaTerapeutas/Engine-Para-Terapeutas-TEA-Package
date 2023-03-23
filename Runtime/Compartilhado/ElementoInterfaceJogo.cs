using UnityEngine;
using UnityEngine.UIElements;

namespace EngineParaTerapeutas.UI {
    public abstract class ElementoInterfaceJogo : ElementoInterface {
        private const string CAMINHO_CLASS_PADROES_USS = "Scripts/Compartilhado/ClassesPadroesStyle";

        protected override void ImportarDefaultStyle() {
            defaultStyle = Resources.Load<StyleSheet>(CAMINHO_CLASS_PADROES_USS);
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