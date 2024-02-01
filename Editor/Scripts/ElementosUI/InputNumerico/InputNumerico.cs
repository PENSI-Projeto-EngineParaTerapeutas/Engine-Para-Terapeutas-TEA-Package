using System;
using UnityEngine.UIElements;
using Autis.Editor.Constantes;

namespace Autis.Editor.UI {
    public class InputNumerico : ElementoInterfaceEditor, IReiniciavel {
        protected override string CaminhoTemplate => "ElementosUI/InputNumerico/InputNumericoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputNumerico/InputNumericoStyle.uss";

        #region .: Elementos :.

        public Label LabelCampoNumerico { get => labelTitulo; }
        public FloatField CampoNumerico { get => campoNumerico; }

        private readonly FloatField campoNumerico;

        private const string NOME_LABEL_INPUT_NUMERICO = "label-input-numerico";
        private const string NOME_INPUT_NUMERICO = "input-numerico";
        private const string SEM_TOOLTIP = null;

        private const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-tooltip-titulo";
        private Tooltip tooltipTitulo;
        private VisualElement regiaoCarregamentoTooltipTitulo;

        private const string NOME_REGIAO_CARREGAMENTO_TITULO = "regiao-carregamento-titulo";
        private VisualElement regiaoCarregamentoTitulo;

        private const string NOME_LABEL = "label-input-numerico";
        private Label labelTitulo;

        #endregion

        public InputNumerico(string label, string tooltipTexto = SEM_TOOLTIP, float max = float.MaxValue, float min = float.MinValue) {

            campoNumerico = Root.Query<FloatField>(NOME_INPUT_NUMERICO);
            tooltipTitulo = new Tooltip();
            
            ConfigurarCampoNumerico(label, max, min);
            CarregarTooltipTitulo(tooltipTexto);

            return;
        }

        private void ConfigurarCampoNumerico(string label, float max, float min) {
            labelTitulo = Root.Query<Label>(NOME_LABEL);

            labelTitulo.name = NOME_LABEL;
            labelTitulo.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            labelTitulo.text = label;
            EsconderTituloSeVazio(label);

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
                tooltipTitulo = new Tooltip(tooltipTexto);

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

        private void EsconderTituloSeVazio(string label) {
            if (String.IsNullOrEmpty(label)) {
                regiaoCarregamentoTitulo = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TITULO);
                regiaoCarregamentoTitulo.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

                labelTitulo.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            }
        }
    }
}
