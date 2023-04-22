using Autis.Runtime.UI;
using Autis.Editor.Utils;
using Autis.Editor.Constantes;

namespace Autis.Editor.UI {
    public abstract class ElementoInterfaceEditor : ElementoInterface {
        protected override void ImportarTemplate(string caminho) {
            template = Importador.ImportarUXML(caminho);
            return;
        }

        protected override void ImportarDefaultStyle() {
            defaultStyle = Importador.ImportarUSS(ConstantesEditor.CaminhoArquivoClassesPadroesUSS);
            Root.styleSheets.Add(defaultStyle);

            return;
        }

        protected override void ImportarStyle(string caminho) {
            style = Importador.ImportarUSS(caminho);
            Root.styleSheets.Add(style);

            return;
        }
    }
}