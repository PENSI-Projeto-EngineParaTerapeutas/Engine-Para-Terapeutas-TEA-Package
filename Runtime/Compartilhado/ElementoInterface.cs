using UnityEngine.UIElements;

namespace EngineParaTerapeutas.UI {
    public abstract class ElementoInterface {
        public VisualElement Root { get; }

        protected StyleSheet defaultStyle;

        protected VisualTreeAsset template;
        protected StyleSheet style;

        protected ElementoInterface() {
            Root = new VisualElement();
            ImportarDefaultStyle();

            return;
        }

        protected abstract void ImportarDefaultStyle();

        protected abstract void ImportarTemplate(string caminho);

        protected abstract void ImportarStyle(string caminho);
    }
}