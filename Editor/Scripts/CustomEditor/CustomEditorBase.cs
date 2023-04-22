using UnityEngine.UIElements;
using Autis.Editor.Utils;
using Autis.Editor.Constantes;

namespace Autis.Editor.CustomEditorComponentesGameObjects {
    public abstract class CustomEditorBase : UnityEditor.Editor {
        protected abstract string CaminhoTemplate { get; }
        protected abstract string CaminhoStyle { get; }

        protected VisualElement root;
        protected VisualTreeAsset template;

        protected StyleSheet style;
        protected StyleSheet defaultStyle;

        protected virtual void OnEnable() {
            ImportarTemplate(CaminhoTemplate);
            root = template.Instantiate();

            ImportarDefaultStyle();
            ImportarStyle(CaminhoStyle);

            OnRenderizarInterface();

            return;
        }

        protected abstract void OnRenderizarInterface();

        protected virtual void ImportarTemplate(string caminho) {
            template = Importador.ImportarUXML(caminho);
            return;
        }

        protected virtual void ImportarDefaultStyle() {
            defaultStyle = Importador.ImportarUSS(ConstantesEditor.CaminhoArquivoClassesPadroesUSS);
            root.styleSheets.Add(defaultStyle);

            return;
        }

        protected virtual void ImportarStyle(string caminho) {
            style = Importador.ImportarUSS(caminho);
            root.styleSheets.Add(style);

            return;
        }

        public override VisualElement CreateInspectorGUI() {
            return root;
        }
    }
}