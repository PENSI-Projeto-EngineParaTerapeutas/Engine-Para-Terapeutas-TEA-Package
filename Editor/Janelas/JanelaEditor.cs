using System.IO;
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
        protected VisualTreeAsset template;
        protected StyleSheet style;

        protected StyleSheet defaultStyle;

        protected static EventoJogo eventoAbrirPopupAviso;
        protected static EventoJogo eventoFecharPopupAviso;

        protected virtual void CreateGUI() {
            eventoAbrirPopupAviso = AssetDatabase.LoadAssetAtPath<EventoJogo>(Path.Combine(ConstantesEditor.CaminhoPastaEventosEditor, "EventoAbrirPopupAviso" + ExtensoesEditor.ScriptableObject));
            eventoFecharPopupAviso = AssetDatabase.LoadAssetAtPath<EventoJogo>(Path.Combine(ConstantesEditor.CaminhoPastaEventosEditor, "EventoFecharPopupAviso" + ExtensoesEditor.ScriptableObject));

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