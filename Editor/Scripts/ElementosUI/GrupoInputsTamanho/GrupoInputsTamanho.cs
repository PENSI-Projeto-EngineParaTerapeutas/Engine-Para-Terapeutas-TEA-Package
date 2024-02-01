using System;
using UnityEngine.UIElements;
using Autis.Editor.Constantes;
using Autis.Editor.Manipuladores;

namespace Autis.Editor.UI {
    public class GrupoInputsTamanho : ElementoInterfaceEditor, IReiniciavel, ICamposAtualizaveis, IVinculavel<ManipuladorObjetos> {
        protected override string CaminhoTemplate => "ElementosUI/GrupoInputsTamanho/GrupoInputsTamanhoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/GrupoInputsTamanho/GrupoInputsTamanhoStyle.uss";

        #region .: Mensagens :.

        private const string LABEL_TITULO = "Tamanho";
        private const string MENSAGEM_TOOLTIP_TITULO = "Dimensões da imagem (comprimento e largura)";

        #endregion

        #region .: Elementos :.

        public InputNumerico CampoTamanhoX { get => campoTamanhoX; }
        public InputNumerico CampoTamanhoY { get => campoTamanhoY; }

        private const string NOME_LABEL_TAMANHO_X = "label-tamanho-x";
        private InputNumerico campoTamanhoX;

        private const string NOME_LABEL_TAMANHO_Y = "label-tamanho-y";
        private InputNumerico campoTamanhoY;

        private const string NOME_REGIAO_CONTEUDO_PRINCIPAL = "regiao-conteudo";
        private VisualElement regiaoConteudoPrincipal;

        private const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-tooltip-titulo";
        private Tooltip tooltipTitulo;
        private VisualElement regiaoCarregamentoTooltipTitulo;

        private const string NOME_REGIAO_CARREGAMENTO_TITULO = "regiao-carregamento-titulo";
        private VisualElement regiaoCarregamentoTitulo;

        private const string NOME_LABEL = "label-inputs-tamanho";
        private Label labelTitulo;

        #endregion

        private ManipuladorObjetos manipulador;
        private bool isEditing = false;

        public GrupoInputsTamanho() {
            ConfigurarLabel(LABEL_TITULO, MENSAGEM_TOOLTIP_TITULO);
            ConfigurarCamposTamanho();
            return;
        }

        private void ConfigurarCamposTamanho() {
            regiaoConteudoPrincipal = root.Query<VisualElement>(NOME_REGIAO_CONTEUDO_PRINCIPAL);

            campoTamanhoX = new InputNumerico("Largura (%):");
            CampoTamanhoX.CampoNumerico.labelElement.name = NOME_LABEL_TAMANHO_X;
            CampoTamanhoX.CampoNumerico.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoTamanhoX.CampoNumerico.SetValueWithoutNotify(100f);

            regiaoConteudoPrincipal.Add(campoTamanhoX.Root);

            campoTamanhoY = new InputNumerico("Altura (%):");
            CampoTamanhoY.CampoNumerico.labelElement.name = NOME_LABEL_TAMANHO_Y;
            CampoTamanhoY.CampoNumerico.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoTamanhoY.CampoNumerico.SetValueWithoutNotify(100f);

            regiaoConteudoPrincipal.Add(campoTamanhoY.Root);

            root.Add(regiaoConteudoPrincipal);

            return;
        }

        private void ConfigurarLabel(string label, string tooltip) {
            tooltipTitulo = new Tooltip();

            CarregarTooltipTitulo(tooltip);

            labelTitulo = Root.Query<Label>(NOME_LABEL);

            labelTitulo.name = NOME_LABEL;
            labelTitulo.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            labelTitulo.text = label;

            regiaoCarregamentoTitulo = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TITULO);

            root.Add(regiaoCarregamentoTitulo);
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
            CampoTamanhoX.CampoNumerico.SetValueWithoutNotify(100f);
            CampoTamanhoY.CampoNumerico.SetValueWithoutNotify(100f);
            return;
        }

        public void VincularDados(ManipuladorObjetos manipulador) {
            this.manipulador = manipulador;

            campoTamanhoX.CampoNumerico.SetValueWithoutNotify(this.manipulador.GetTamanho().x * 100f);
            campoTamanhoY.CampoNumerico.SetValueWithoutNotify(this.manipulador.GetTamanho().y * 100f);

            campoTamanhoX.CampoNumerico.RegisterCallback<FocusInEvent>(evt => {
                isEditing = true;
            });

            campoTamanhoX.CampoNumerico.RegisterCallback<FocusOutEvent>(evt => {
                isEditing = false;
            });

            campoTamanhoX.CampoNumerico.RegisterCallback<ChangeEvent<float>>(evt => {
                this.manipulador.SetTamanhoX(evt.newValue / 100f);
            });

            campoTamanhoY.CampoNumerico.RegisterCallback<FocusInEvent>(evt => {
                isEditing = true;
            });

            campoTamanhoY.CampoNumerico.RegisterCallback<FocusOutEvent>(evt => {
                isEditing = false;
            });

            campoTamanhoY.CampoNumerico.RegisterCallback<ChangeEvent<float>>(evt => {
                this.manipulador.SetTamanhoY(evt.newValue / 100f);
            });

            return;
        }

        public void AtualizarCampos() {
            if(isEditing || manipulador == null || manipulador.ObjetoAtual == null) {
                return;
            }

            campoTamanhoX?.CampoNumerico.SetValueWithoutNotify(manipulador.GetTamanho().x * 100f);
            campoTamanhoY?.CampoNumerico.SetValueWithoutNotify(manipulador.GetTamanho().y * 100f);

            return;
        }
    }
}
