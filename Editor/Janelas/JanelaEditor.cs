using UnityEditor;
using UnityEngine.UIElements;
using EngineParaTerapeutas.Utils;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.Telas {
    public abstract class JanelaEditor : EditorWindow {
        protected abstract string CaminhoTemplate { get; }
        protected abstract string CaminhoStyle { get; }

        protected VisualElement root;
        protected VisualTreeAsset template;
        protected StyleSheet style;

        protected StyleSheet defaultStyle;

        protected virtual void CreateGUI() {
            root = rootVisualElement;
            ImportarTemplate(CaminhoTemplate);

            ImportarDefaultStyle();
            ImportarStyle(CaminhoStyle);

            OnRenderizarInterface();

            return;
        }

        protected abstract void OnRenderizarInterface();

        protected virtual void ImportarDefaultStyle() {
            defaultStyle = Importador.ImportarUSS(ConstantesEditor.CaminhoArquivoClassesPadroesUSS);
            root.styleSheets.Add(defaultStyle);

            return;
        }

        protected virtual void ImportarTemplate(string caminho) {
            template = Importador.ImportarUXML(caminho);
            root.Add(template.Instantiate());

            return;
        }

        protected virtual void ImportarStyle(string caminho) {
            style = Importador.ImportarUSS(caminho);
            root.styleSheets.Add(style);

            return;
        }
    }
}