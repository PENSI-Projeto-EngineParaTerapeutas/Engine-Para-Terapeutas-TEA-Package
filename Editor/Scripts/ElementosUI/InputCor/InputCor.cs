using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Autis.Editor.Constantes;

namespace Autis.Editor.UI {
    public class InputCor : ElementoInterfaceEditor, IReiniciavel {
        protected override string CaminhoTemplate => "ElementosUI/InputCor/InputCorTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputCor/InputCorStyle.uss";

        #region .: Elementos :.
 
        public ColorField CampoCor { get => campoCor; }
        public Label LabelCampoCor { get => label; }

        private const string NOME_INPUT_COR = "input-cor";
        private readonly ColorField campoCor;

        private const string NOME_LABEL_INPUT = "label-input-cor";
        private readonly Label label;

        #endregion

        public InputCor() {
            campoCor = Root.Query<ColorField>(NOME_INPUT_COR);
            label = Root.Query<Label>(NOME_LABEL_INPUT);

            ConfigurarInputCor();
            label.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        public InputCor(string label) {
            campoCor = Root.Query<ColorField>(NOME_INPUT_COR);
            this.label = Root.Query<Label>(NOME_LABEL_INPUT);

            ConfigurarInputCor();
            this.label.text = label;

            return;
        }

        private void ConfigurarInputCor() {
            CampoCor.SetValueWithoutNotify(Color.blue);
            CampoCor.showEyeDropper = false;
            CampoCor.showAlpha = false;
            CampoCor.AddToClassList("color-field__input");

            return;
        }

        public void ReiniciarCampos() {
            CampoCor.SetValueWithoutNotify(Color.blue);
            return;
        }
    }
}