using UnityEngine.UIElements;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.UI {
    public class InputTexto : ElementoInterfaceEditor, IReiniciavel {
        protected override string CaminhoTemplate => "ElementosUI/InputTexto/InputTextoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputTexto/InputTextoStyle.uss";

        #region .: Elementos :.

        public Label LabelCampoTexto { get => campoTexto.labelElement; }
        public TextField CampoTexto { get => campoTexto; }

        private const string NOME_LABEL_INPUT_TEXTO = "label-input-texto";

        private const string NOME_INPUT_TEXTO = "input-texto";
        private readonly TextField campoTexto;

        #endregion

        public InputTexto(string label) {
            campoTexto = Root.Query<TextField>(NOME_INPUT_TEXTO);

            campoTexto.label = label;
            campoTexto.labelElement.name = NOME_LABEL_INPUT_TEXTO;
            campoTexto.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            return;
        }

        public void ReiniciarCampos() {
            campoTexto.value = string.Empty;
            return;
        }
    }
}