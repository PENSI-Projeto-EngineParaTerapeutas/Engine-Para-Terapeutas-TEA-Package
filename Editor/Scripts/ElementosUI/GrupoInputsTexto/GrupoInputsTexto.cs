using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using TMPro;
using Autis.Editor.Constantes;
using Autis.Editor.Manipuladores;

namespace Autis.Editor.UI {
    public class GrupoInputsTexto : ElementoInterfaceEditor, IVinculavel<ManipuladorTexto>, IReiniciavel {
        protected override string CaminhoTemplate => "ElementosUI/GrupoInputsTexto/GrupoInputsTextoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/GrupoInputsTexto/GrupoInputsTextoStyle.uss";

        #region .: Mensagens :.

        private const string MENSAGEM_TOOLTIP_CONFIGURACAO_TEXTO = "Formatação da fonte do texto.";

        #endregion

        #region .: Elementos :.
        public TextField CampoConteudoTexto { get => campoConteudoTexto; }
        public VisualElement RegiaoCampoTamanhoTexto { get => regiaoCampoTamanhoTexto; }
        public FloatField CampoTamanhoTexto { get => campoTamanhoTexto; }
        public Toggle CampoNegrito { get => campoNegrito; }
        public Toggle CampoItalico { get => campoItalico; }
        public Toggle CampoSublinhado { get => campoSublinhado; }
        public VisualElement RegiaoInputCor { get => regiaoInputCor; }
        public InputCor InputCor { get => inputCor; }

        private const string NOME_LABEL_CONTEUDO_TEXTO = "label-texto";
        private const string NOME_INPUT_CONTEUDO_TEXTO = "input-texto";
        private TextField campoConteudoTexto;

        private const string NOME_REGIAO_TAMANHO_TEXTO = "regiao-campo-tamanho-texto";
        private VisualElement regiaoCampoTamanhoTexto;

        private const string NOME_LABEL_TAMANHO_TEXTO = "label-tamanho-texto";
        private const string NOME_INPUT_TAMANHO_TEXTO = "input-tamanho-texto";
        private FloatField campoTamanhoTexto;

        private const string NOME_LABEL_NEGRITIO = "label-negrito";
        private const string NOME_INPUT_NEGRITO= "input-negrito";
        private Toggle campoNegrito;

        private const string NOME_LABEL_ITALICO = "label-italico";
        private const string NOME_INPUT_ITALICO = "input-italico";
        private Toggle campoItalico;

        private const string NOME_LABEL_SUBLINHADO = "label-sublinhado";
        private const string NOME_INPUT_SUBLINHADO = "input-sublinhado";
        private Toggle campoSublinhado;

        private const string NOME_REGIAO_INPUT_COR = "regiao-input-cor";
        private VisualElement regiaoInputCor;

        private const string NOME_LABEL_COR = "label-cor";
        private const string NOME_INPUT_COR = "input-cor";
        private InputCor inputCor;

        private const string NOME_LABEL_CONFIGURACAO_TEXTO = "label-configuracao-texto";
        private Label labelConfiguracaoTexto;

        private const string NOME_REGIAO_LABEL_CONFIGURACAO_TEXTO = "regiao-label-configuracao-texto";
        private VisualElement regiaoLabelConfiguracaoTexto;
        private VisualElement regiaoCarregamentoTooltipConfiguracaoTexto;
        private InterrogacaoToolTip tooltipConfiguracaoTexto;
        private const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_CONFIGURACAO_TEXTO = "regiao-tooltip-configuracao-texto";

        #endregion

        private ManipuladorTexto manipulador;

        public GrupoInputsTexto() {
            ConfigurarTooltipLabelConfiguracaoTexto();
            ConfigurarConteudoTexto();
            ConfigurarTamanhoTexto();
            ConfigurarNegrito();
            ConfigurarItalico();
            ConfigurarSublinhado();
            ConfigurarInputCor();

            return;
        }

        private void ConfigurarTooltipLabelConfiguracaoTexto() {
            tooltipConfiguracaoTexto = new InterrogacaoToolTip();
            tooltipConfiguracaoTexto.SetTexto(MENSAGEM_TOOLTIP_CONFIGURACAO_TEXTO);

            regiaoCarregamentoTooltipConfiguracaoTexto = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_CONFIGURACAO_TEXTO);
            regiaoCarregamentoTooltipConfiguracaoTexto.Add(tooltipConfiguracaoTexto.Root);

            return;
        }

        private void ConfigurarConteudoTexto() {
            regiaoLabelConfiguracaoTexto = root.Query<VisualElement>(NOME_REGIAO_LABEL_CONFIGURACAO_TEXTO);
            campoConteudoTexto = Root.Query<TextField>(NOME_INPUT_CONTEUDO_TEXTO);

            CampoConteudoTexto.labelElement.name = NOME_LABEL_CONTEUDO_TEXTO;
            CampoConteudoTexto.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoConteudoTexto.SetValueWithoutNotify("Digite o texto aqui");
            CampoConteudoTexto.multiline = true;

            return;
        }

        private void ConfigurarTamanhoTexto() {
            regiaoCampoTamanhoTexto = Root.Query<VisualElement>(NOME_REGIAO_TAMANHO_TEXTO);
            campoTamanhoTexto = Root.Query<FloatField>(NOME_INPUT_TAMANHO_TEXTO);

            CampoTamanhoTexto.labelElement.name = NOME_LABEL_TAMANHO_TEXTO;
            CampoTamanhoTexto.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoTamanhoTexto.SetValueWithoutNotify(1f);

            return;
        }

        private void ConfigurarNegrito() {
            campoNegrito = Root.Query<Toggle>(NOME_INPUT_NEGRITO);

            CampoNegrito.labelElement.name = NOME_LABEL_NEGRITIO;
            CampoNegrito.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoNegrito.SetValueWithoutNotify(false);

            return;
        }

        private void ConfigurarItalico() {
            campoItalico = Root.Query<Toggle>(NOME_INPUT_ITALICO);

            CampoItalico.labelElement.name = NOME_LABEL_ITALICO;
            CampoItalico.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoItalico.SetValueWithoutNotify(false);

            return;
        }

        private void ConfigurarSublinhado() {
            campoSublinhado = Root.Query<Toggle>(NOME_INPUT_SUBLINHADO);

            CampoSublinhado.labelElement.name = NOME_LABEL_SUBLINHADO;
            CampoSublinhado.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoSublinhado.SetValueWithoutNotify(false);

            return;
        }

        private void ConfigurarInputCor() {
            regiaoInputCor = Root.Query<VisualElement>(NOME_REGIAO_INPUT_COR);
            inputCor = new InputCor();

            InputCor.CampoCor.SetValueWithoutNotify(Color.blue);
            InputCor.CampoCor.name = NOME_INPUT_COR;
            inputCor.LabelCampoCor.name = NOME_LABEL_COR;

            regiaoInputCor.Add(inputCor.Root);

            return;
        }

        public void VincularDados(ManipuladorTexto manipulador) {
            this.manipulador = manipulador;

            CampoConteudoTexto.SetValueWithoutNotify(this.manipulador.GetTexto());
            CampoTamanhoTexto.SetValueWithoutNotify(this.manipulador.GetFontSize());
            CampoNegrito.SetValueWithoutNotify(this.manipulador.FontStyleEstaAtivo(FontStyles.Bold));
            CampoItalico.SetValueWithoutNotify(this.manipulador.FontStyleEstaAtivo(FontStyles.Italic));
            CampoSublinhado.SetValueWithoutNotify(this.manipulador.FontStyleEstaAtivo(FontStyles.Underline));
            InputCor.CampoCor.SetValueWithoutNotify(this.manipulador.GetCor());

            CampoConteudoTexto.RegisterCallback<ChangeEvent<string>>(evt => {
                this.manipulador.SetTexto(evt.newValue);
            });

            CampoTamanhoTexto.RegisterCallback<ChangeEvent<float>>(evt => {
                this.manipulador.SetFontSize(evt.newValue);
            });

            CampoNegrito.RegisterCallback<ChangeEvent<bool>>(evt => {
                this.manipulador.SetFontStyle(evt.newValue, FontStyles.Bold);
            });

            CampoItalico.RegisterCallback<ChangeEvent<bool>>(evt => {
                this.manipulador.SetFontStyle(evt.newValue, FontStyles.Italic);
            });

            CampoSublinhado.RegisterCallback<ChangeEvent<bool>>(evt => {
                this.manipulador.SetFontStyle(evt.newValue, FontStyles.Underline);
            });

            InputCor.CampoCor.RegisterCallback<ChangeEvent<Color>>(evt => {
                this.manipulador.SetCor(evt.newValue);
            });

            return;
        }

        public void ReiniciarCampos() {
            CampoConteudoTexto.SetValueWithoutNotify("Digite o texto aqui");
            CampoTamanhoTexto.SetValueWithoutNotify(1f);
            CampoNegrito.SetValueWithoutNotify(false);
            CampoItalico.SetValueWithoutNotify(false);
            CampoSublinhado.SetValueWithoutNotify(false);
            InputCor.CampoCor.SetValueWithoutNotify(Color.blue);

            return;
        }
    }
}