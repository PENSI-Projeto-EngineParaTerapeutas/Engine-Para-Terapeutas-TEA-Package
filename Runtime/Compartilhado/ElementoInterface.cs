using UnityEngine.UIElements;

namespace EngineParaTerapeutas.UI {
    public abstract class ElementoInterface {
        protected abstract string CaminhoTemplate { get; }
        protected abstract string CaminhoStyle { get; }

        public VisualElement Root { get => root; }
        protected VisualElement root;

        protected StyleSheet defaultStyle;

        protected VisualTreeAsset template;
        protected StyleSheet style;

        protected ElementoInterface() {
            ImportarTemplate(CaminhoTemplate);
            root = template.Instantiate();

            ImportarDefaultStyle();
            ImportarStyle(CaminhoStyle);

            return;
        }

        protected abstract void ImportarTemplate(string caminho);
        protected abstract void ImportarDefaultStyle();
        protected abstract void ImportarStyle(string caminho);
    }
}