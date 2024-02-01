using System;
using UnityEngine.UIElements;
using Autis.Editor.Constantes;

namespace Autis.Editor.UI {
    public class InputTexto : ElementoInterfaceEditor, IReiniciavel, IEstaVazio {
        protected override string CaminhoTemplate => "ElementosUI/InputTexto/InputTextoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputTexto/InputTextoStyle.uss";

        #region .: Elementos :.

        public Label LabelCampoTexto { get => labelTitulo; }
        public TextField CampoTexto { get => campoTexto; }

        private const string NOME_LABEL_INPUT_TEXTO = "label-input-texto";

        private const string NOME_INPUT_TEXTO = "input-texto";

        private const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-tooltip-titulo";

        private const string SEM_TOOLTIP = null;

        private readonly TextField campoTexto;
        private readonly Tooltip tooltipTitulo;
        private VisualElement regiaoCarregamentoTooltipTitulo;

        private const string NOME_REGIAO_CARREGAMENTO_TITULO = "regiao-carregamento-titulo";
        private VisualElement regiaoCarregamentoTitulo;

        private Label labelTitulo;

        #endregion

        public InputTexto(string label, string tooltipTexto = SEM_TOOLTIP) {
            labelTitulo = Root.Query<Label>(NOME_LABEL_INPUT_TEXTO);

            labelTitulo.name = NOME_LABEL_INPUT_TEXTO;
            labelTitulo.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            labelTitulo.text = label;

            regiaoCarregamentoTitulo = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TITULO); 

            tooltipTitulo = new Tooltip();
            CarregarTooltipTitulo(tooltipTexto);

            campoTexto = Root.Query<TextField>(NOME_INPUT_TEXTO);

            root.Add(regiaoCarregamentoTitulo);
            root.Add(campoTexto);
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

        public bool EstaVazio() {
            return campoTexto.value == string.Empty;
        }
    }
}