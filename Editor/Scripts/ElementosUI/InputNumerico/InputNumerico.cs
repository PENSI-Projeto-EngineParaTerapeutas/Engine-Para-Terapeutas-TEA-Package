using System;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Autis.Editor.Constantes;

namespace Autis.Editor.UI {
    public class InputNumerico : ElementoInterfaceEditor, IReiniciavel {
        protected override string CaminhoTemplate => "ElementosUI/InputNumerico/InputNumericoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputNumerico/InputNumericoStyle.uss";

        #region .: Elementos :.

        public Label LabelCampoNumerico { get => campoNumerico.labelElement; }
        public FloatField CampoNumerico { get => campoNumerico; }

        private readonly FloatField campoNumerico;
        private readonly InterrogacaoToolTip tooltipTitulo;
        private VisualElement regiaoCarregamentoTooltipTitulo;

        private const string NOME_LABEL_INPUT_NUMERICO = "label-input-numerico";
        private const string NOME_INPUT_NUMERICO = "input-numerico";
        private const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-tooltip-titulo";
        private const string SEM_TOOLTIP = null;

        #endregion

        public InputNumerico(string label, string tooltipTexto = SEM_TOOLTIP, float max = float.MaxValue, float min = float.MinValue) {

            campoNumerico = Root.Query<FloatField>(NOME_INPUT_NUMERICO);
            tooltipTitulo = new InterrogacaoToolTip();
            
            ConfigurarCampoNumerico(label, max, min);
            CarregarTooltipTitulo(tooltipTexto);

            return;
        }

        private void ConfigurarCampoNumerico(string label, float max, float min) {

            CampoNumerico.label = label;
            CampoNumerico.labelElement.name = NOME_LABEL_INPUT_NUMERICO;
            CampoNumerico.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoNumerico.SetValueWithoutNotify(0);

            CampoNumerico.RegisterCallback<ChangeEvent<float>>(evt => {
                if(evt.newValue < min) {
                    CampoNumerico.value = min;
                }
                if(evt.newValue > max) {
                    CampoNumerico.value = max;
                }

            });

            return;
        }

        private void CarregarTooltipTitulo(string tooltipTexto) {
            if (!String.IsNullOrEmpty(tooltipTexto)) {
                regiaoCarregamentoTooltipTitulo = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO);
                regiaoCarregamentoTooltipTitulo.Add(tooltipTitulo.Root);

                tooltipTitulo.SetTexto(tooltipTexto);
            }

            return;
        }

        public void ReiniciarCampos() {
            CampoNumerico.SetValueWithoutNotify(0);
            return;
        }
    }
}
