using System;
using UnityEngine.UIElements;
using Autis.Editor.Constantes;

namespace Autis.Editor.UI {
    public class InputTexto : ElementoInterfaceEditor, IReiniciavel {
        protected override string CaminhoTemplate => "ElementosUI/InputTexto/InputTextoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputTexto/InputTextoStyle.uss";

        #region .: Elementos :.

        public Label LabelCampoTexto { get => campoTexto.labelElement; }
        public TextField CampoTexto { get => campoTexto; }

        private const string NOME_LABEL_INPUT_TEXTO = "label-input-texto";

        private const string NOME_INPUT_TEXTO = "input-texto";

        private const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-tooltip-titulo";

        private const string SEM_TOOLTIP = null;

        private readonly TextField campoTexto;
        private readonly InterrogacaoToolTip tooltipTitulo;
        private VisualElement regiaoCarregamentoTooltipTitulo;

        #endregion

        public InputTexto(string label, string tooltipTexto = SEM_TOOLTIP) {
            campoTexto = Root.Query<TextField>(NOME_INPUT_TEXTO);

            campoTexto.label = label;
            campoTexto.labelElement.name = NOME_LABEL_INPUT_TEXTO;
            campoTexto.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            tooltipTitulo = new InterrogacaoToolTip();
            CarregarTooltipTitulo(tooltipTexto);

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
            campoTexto.value = string.Empty;
            return;
        }
    }
}