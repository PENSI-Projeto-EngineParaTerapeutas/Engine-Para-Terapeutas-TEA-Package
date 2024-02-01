using UnityEditor;
using UnityEngine.UIElements;
using Autis.Editor.Utils;
using Autis.Editor.Constantes;
using Autis.Runtime.Eventos;

namespace Autis.Editor.Telas {
    public abstract class JanelaEditor : EditorWindow {
        protected abstract string CaminhoTemplate { get; }
        protected abstract string CaminhoStyle { get; }

        protected VisualElement root;
        protected TemplateContainer templateInstace;
        protected VisualTreeAsset template;
        protected StyleSheet style;

        protected StyleSheet defaultStyle;

        protected static EventoJogo eventoAbrirPopupAviso;
        protected static EventoJogo eventoFecharPopupAviso;

        protected virtual void CreateGUI() {
            eventoAbrirPopupAviso = Importador.ImportarEvento("EventoAbrirPopupAviso");
            eventoFecharPopupAviso = Importador.ImportarEvento("EventoFecharPopupAviso");

            eventoAbrirPopupAviso.AdicionarCallback(HandleAbrirPopupAviso);
            eventoFecharPopupAviso.AdicionarCallback(HandleFecharPopupAviso);

            root = rootVisualElement;
            ImportarTemplate(CaminhoTemplate);

            ImportarDefaultStyle();
            ImportarStyle(CaminhoStyle);

            OnRenderizarInterface();

            return;
        }

        protected virtual void HandleAbrirPopupAviso() {
            root.SetEnabled(false);
            return;
        }

        protected virtual void HandleFecharPopupAviso() {
            root.SetEnabled(true);
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
            
            templateInstace = template.Instantiate();
            templateInstace.AddToClassList(NomesClassesPadroesEditorStyle.TemplateContainerPadrao);

            root.Add(templateInstace);

            return;
        }

        protected virtual void ImportarStyle(string caminho) {
            style = Importador.ImportarUSS(caminho);
            root.styleSheets.Add(style);

            return;
        }
    }
}