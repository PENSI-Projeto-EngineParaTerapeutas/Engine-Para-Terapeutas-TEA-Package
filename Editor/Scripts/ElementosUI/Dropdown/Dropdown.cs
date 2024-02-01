using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Autis.Editor.Constantes;

namespace Autis.Editor.UI {
    public class Dropdown : ElementoInterfaceEditor, IReiniciavel, IEstaVazio {
        public const string VALOR_PADRAO_DROPDOWN = "Selecione";
        private const string CLASSE_STYLE_PADRAO = "estilo-dropdown";

        protected override string CaminhoTemplate => "ElementosUI/Dropdown/DropdownTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/Dropdown/DropdownStyle.uss";

        #region .: Elementos :.

        public Label Label { get => labelTitulo; }
        public DropdownField Campo { get => campo; } 

        private const string NOME_DROPDOWN = "campo-dropdown";
        private readonly DropdownField campo;

        private const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-tooltip-titulo";
        private readonly Tooltip tooltipTitulo;
        private VisualElement regiaoCarregamentoTooltipTitulo;

        private const string NOME_REGIAO_CARREGAMENTO_TITULO = "regiao-carregamento-titulo";
        private VisualElement regiaoCarregamentoTitulo;

        private const string NOME_LABEL = "label-campo-dropdown";
        private Label labelTitulo;

        #endregion

        public Dropdown(string label) {
            List<string> opcoes = new() {
                VALOR_PADRAO_DROPDOWN,
            };

            campo = new(opcoes, 0)
            {
                name = NOME_DROPDOWN,
            };
            campo.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            campo.AddToClassList(CLASSE_STYLE_PADRAO);

            labelTitulo = Root.Query<Label>(NOME_LABEL);

            labelTitulo.name = NOME_LABEL;
            labelTitulo.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            labelTitulo.text = label;

            EsconderTituloSeVazio(label);

            root.Add(labelTitulo);
            root.Add(campo);

            return;
        }

        public Dropdown(string label, List<string> opcoes) {
            List<string> opcoesDropdown = opcoes;
            opcoesDropdown.Insert(0, VALOR_PADRAO_DROPDOWN);

            campo = new(opcoesDropdown, 0)
            {
                name = NOME_DROPDOWN,
            };
            campo.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            campo.AddToClassList(CLASSE_STYLE_PADRAO);

            labelTitulo = Root.Query<Label>(NOME_LABEL);

            labelTitulo.name = NOME_LABEL;
            labelTitulo.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            labelTitulo.text = label;

            EsconderTituloSeVazio(label);

            root.Add(labelTitulo);
            root.Add(campo);

            return;
        }

        public Dropdown(string label, string tooltip, List<string> opcoes) {
            List<string> opcoesDropdown = opcoes;
            opcoesDropdown.Insert(0, VALOR_PADRAO_DROPDOWN);

            campo = new(opcoesDropdown, 0) {
                name = NOME_DROPDOWN,
                tooltip = tooltip,
            };

            tooltipTitulo = new Tooltip();

            CarregarTooltipTitulo(tooltip);

            campo.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            campo.AddToClassList(CLASSE_STYLE_PADRAO);

            labelTitulo = Root.Query<Label>(NOME_LABEL);

            labelTitulo.name = NOME_LABEL;
            labelTitulo.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            labelTitulo.text = label;

            regiaoCarregamentoTitulo = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TITULO);

            root.Add(regiaoCarregamentoTitulo);
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

        private void EsconderTituloSeVazio(string label) {
            if (String.IsNullOrEmpty(label)) {
                regiaoCarregamentoTitulo = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TITULO);
                regiaoCarregamentoTitulo.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

                labelTitulo.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            }
        }

        public void ReiniciarCampos() {
            campo.SetValueWithoutNotify(VALOR_PADRAO_DROPDOWN);
            campo.SendEvent(new ChangeEvent<string>());

            return;
        }

        public bool EstaVazio() {
            return Campo.value == Dropdown.VALOR_PADRAO_DROPDOWN;
        }
    }
}