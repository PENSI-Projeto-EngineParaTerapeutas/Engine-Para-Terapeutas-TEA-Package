using System;
using UnityEngine.UIElements;
using Autis.Editor.Constantes;
using Autis.Editor.Manipuladores;

namespace Autis.Editor.UI {
    public class GrupoInputsPosicao : ElementoInterfaceEditor, IReiniciavel, ICamposAtualizaveis, IVinculavel<ManipuladorObjetos> {
        protected override string CaminhoTemplate => "ElementosUI/GrupoInputsPosicao/GrupoInputsPosicaoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/GrupoInputsPosicao/GrupoInputsPosicaoStyle.uss";

        #region .: Mensagens :.

        private const string LABEL_TITULO = "Posição";
        private const string MENSAGEM_TOOLTIP_TITULO = "Posicionamento da imagem na tela (posição horizontal e vertical)";

        #endregion

        #region .: Elementos :.

        public InputNumerico CampoPosicaoX { get => campoPosicaoX; }
        public InputNumerico CampoPosicaoY { get => campoPosicaoY; }

        private const string NOME_LABEL_POSICAO_X = "label-posicao-x";
        private InputNumerico campoPosicaoX;

        private const string NOME_LABEL_POSICAO_Y = "label-posicao-y";
        private InputNumerico campoPosicaoY;

        private const string NOME_REGIAO_CONTEUDO_PRINCIPAL = "regiao-conteudo";
        private VisualElement regiaoConteudoPrincipal;

        private const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-tooltip-titulo";
        private Tooltip tooltipTitulo;
        private VisualElement regiaoCarregamentoTooltipTitulo;

        private const string NOME_REGIAO_CARREGAMENTO_TITULO = "regiao-carregamento-titulo";
        private VisualElement regiaoCarregamentoTitulo;

        private const string NOME_LABEL = "label-inputs-posicao";
        private Label labelTitulo;

        #endregion

        private ManipuladorObjetos manipulador;
        private bool isEditing = false;

        public GrupoInputsPosicao() {
            ConfigurarLabel(LABEL_TITULO, MENSAGEM_TOOLTIP_TITULO);
            ConfigurarCamposPosicao();
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

        private void ConfigurarCamposPosicao() {
            regiaoConteudoPrincipal = root.Query<VisualElement>(NOME_REGIAO_CONTEUDO_PRINCIPAL);

            campoPosicaoX = new InputNumerico("Horizontal");
            campoPosicaoX.CampoNumerico.labelElement.name = NOME_LABEL_POSICAO_X;
            campoPosicaoX.CampoNumerico.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            campoPosicaoX.CampoNumerico.SetValueWithoutNotify(0);

            regiaoConteudoPrincipal.Add(campoPosicaoX.Root);

            campoPosicaoY = new InputNumerico("Vertical");
            campoPosicaoY.CampoNumerico.labelElement.name = NOME_LABEL_POSICAO_Y;
            campoPosicaoY.CampoNumerico.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            campoPosicaoY.CampoNumerico.SetValueWithoutNotify(0);

            regiaoConteudoPrincipal.Add(campoPosicaoY.Root);
            root.Add(regiaoConteudoPrincipal);

            return;
        }

        public void ReiniciarCampos() {
            campoPosicaoX.CampoNumerico.SetValueWithoutNotify(0);
            campoPosicaoY.CampoNumerico.SetValueWithoutNotify(0);
            return;
        }

        public void VincularDados(ManipuladorObjetos manipulador) {
            this.manipulador = manipulador;

            campoPosicaoX.CampoNumerico.SetValueWithoutNotify(this.manipulador.GetPosicao().x);
            campoPosicaoY.CampoNumerico.SetValueWithoutNotify(this.manipulador.GetPosicao().y);

            campoPosicaoX.CampoNumerico.RegisterCallback<FocusInEvent>(evt => {
                isEditing = true;
            });

            campoPosicaoX.CampoNumerico.RegisterCallback<FocusOutEvent>(evt => {
                isEditing = false;
            });

            campoPosicaoX.CampoNumerico.RegisterCallback<ChangeEvent<float>>(evt => {
                manipulador.SetPosicaoX(evt.newValue);
            });

            campoPosicaoY.CampoNumerico.RegisterCallback<FocusInEvent>(evt => {
                isEditing = true;
            });

            campoPosicaoY.CampoNumerico.RegisterCallback<FocusOutEvent>(evt => {
                isEditing = false;
            });

            campoPosicaoY.CampoNumerico.RegisterCallback<ChangeEvent<float>>(evt => {
                manipulador.SetPosicaoY(evt.newValue);
            });

            return;
        }

        public void AtualizarCampos() {
            if(isEditing || manipulador == null || manipulador.ObjetoAtual == null) {
                return;
            }

            campoPosicaoX?.CampoNumerico.SetValueWithoutNotify(manipulador.GetPosicao().x);
            campoPosicaoY?.CampoNumerico.SetValueWithoutNotify(manipulador.GetPosicao().y);

            return;
        }
    }
}
