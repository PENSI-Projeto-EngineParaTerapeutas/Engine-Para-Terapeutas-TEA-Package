using System;
using Autis.Editor.Utils;
using Autis.Editor.Constantes;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

namespace Autis.Editor.UI {
    public class Dropdown : ElementoInterfaceEditor, IReiniciavel {
        public const string VALOR_PADRAO_DROPDOWN = "Selecione";
        private const string CLASSE_STYLE_PADRAO = "estilo-dropdown";

        protected override string CaminhoTemplate => "ElementosUI/Dropdown/DropdownTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/Dropdown/DropdownStyle.uss";

        #region .: Elementos :.

        public Label Label { get => label; }
        public DropdownField Campo { get => campo; }

        private const string NOME_LABEL = "label-campo-dropdown";
        private readonly Label label;

        private const string NOME_DROPDOWN = "campo-dropdown";
        private readonly DropdownField campo;

        private const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-tooltip-titulo";
        private readonly InterrogacaoToolTip tooltipTitulo;
        private VisualElement regiaoCarregamentoTooltipTitulo;

        #endregion

        public Dropdown(string label) {
            List<string> opcoes = new() {
                VALOR_PADRAO_DROPDOWN,
            };

            campo = new(label, opcoes, 0) {
                name = NOME_DROPDOWN,
            };
            campo.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            campo.AddToClassList(CLASSE_STYLE_PADRAO);

            campo.labelElement.name = NOME_LABEL;
            campo.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            root.Add(campo);

            return;
        }

        public Dropdown(string label, List<string> opcoes) {
            List<string> opcoesDropdown = opcoes;
            opcoesDropdown.Insert(0, VALOR_PADRAO_DROPDOWN);

            campo = new(label, opcoesDropdown, 0) {
                name = NOME_DROPDOWN,
            };
            campo.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            campo.AddToClassList(CLASSE_STYLE_PADRAO);

            campo.labelElement.name = NOME_LABEL;
            campo.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            root.Add(campo);

            return;
        }

        public Dropdown(string label, string tooltip, List<string> opcoes) {
            List<string> opcoesDropdown = opcoes;
            opcoesDropdown.Insert(0, VALOR_PADRAO_DROPDOWN);

            campo = new(label, opcoesDropdown, 0) {
                name = NOME_DROPDOWN,
                tooltip = tooltip,
            };

            tooltipTitulo = new InterrogacaoToolTip();

            CarregarTooltipTitulo(tooltip);

            campo.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            campo.AddToClassList(CLASSE_STYLE_PADRAO);

            campo.labelElement.name = NOME_LABEL;
            campo.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            root.Add(campo);

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
            campo.SetValueWithoutNotify(VALOR_PADRAO_DROPDOWN);
            campo.SendEvent(new ChangeEvent<string>());

            return;
        }
    }
}