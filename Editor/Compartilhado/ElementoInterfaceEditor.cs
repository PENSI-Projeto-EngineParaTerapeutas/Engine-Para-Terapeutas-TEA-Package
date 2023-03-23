using EngineParaTerapeutas.Utils;

namespace EngineParaTerapeutas.UI {
    public abstract class ElementoInterfaceEditor : ElementoInterface {
        private const string CAMINHO_CLASSES_PADROES_USS = "Compartilhado/ClassesPadroesEditorStyle.uss";

        protected override void ImportarDefaultStyle() {
            defaultStyle = Importador.ImportarUSS(CAMINHO_CLASSES_PADROES_USS);
            Root.styleSheets.Add(defaultStyle);

            return;
        }

        protected override void ImportarTemplate(string caminho) {
            template = Importador.ImportarUXML(caminho);
            return;
        }

        protected override void ImportarStyle(string caminho) {
            style = Importador.ImportarUSS(caminho);
            Root.styleSheets.Add(style);

            return;
        }
    }
}