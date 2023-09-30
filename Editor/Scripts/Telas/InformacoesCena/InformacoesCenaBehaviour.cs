using UnityEngine.UIElements;
using Autis.Editor.Constantes;
using Autis.Editor.UI;
using Autis.Editor.Manipuladores;
using Autis.Runtime.DTOs;

namespace Autis.Editor.Telas {
    public class InformacoesCenaBehaviour : Tela {
        protected override string CaminhoTemplate => "Telas/InformacoesCena/InformacoesCenaTemplate.uxml";
        protected override string CaminhoStyle => "Telas/InformacoesCena/InformacoesCenaStyle.uss";

        #region .: Elementos :.

        protected const string NOME_REGIAO_CARREGAMENTO_INPUTS_CENA = "regiao-carregamento-inputs-cena";
        protected VisualElement regiaoCarregamentoInputsCena;

        protected const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        protected VisualElement regiaoCarregamentoBotoesConfirmacao;

        protected const string NOME_REGIAO_CARREGAMENTO_INPUTS_VIDEO = "regiao-carregamento-inputs-video";
        protected VisualElement regiaoCarregamentoInputsVideo;

        protected const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_GABARITO = "regiao-tooltip-tipo-gabarito";
        protected VisualElement regiaoCarregamentoTooltipTipoGabarito;

        protected const string NOME_OPCAO_RADIO_SELECIONAR = "radio-opcao-selecionar";
        protected RadioButton opcaoRadioSelecionar;

        protected const string NOME_OPCAO_RADIO_ARRASTAR = "radio-opcao-arrastar";
        protected RadioButton opcaoRadioArrastar;

        protected const string NOME_BOTAO_CRIAR_GABARITO = "botao-criar-gabarito";
        protected Button botaoCriarGabarito;

        protected readonly InputsScriptableObjectCena grupoInputsCena;
        protected readonly InputVideo inputVideo;
        protected readonly InterrogacaoToolTip tooltipTipoGabarito;
        protected BotoesConfirmacao botoesConfirmacao;

        #endregion

        protected readonly ManipuladorCena manipuladorCena;
        protected readonly ManipuladorContexto manipuladorContexto;

        public InformacoesCenaBehaviour() {
            manipuladorCena = new ManipuladorCena();
            manipuladorContexto = new ManipuladorContexto();

            tooltipTipoGabarito = new InterrogacaoToolTip();
            grupoInputsCena = new InputsScriptableObjectCena();
            inputVideo = new InputVideo();

            CarregarInputsCena();
            CarregarInputVideo();
            CarregarBotoesConfirmacao();
            CarregarOpcoesRadioButtons();
            CarregarTooltips();
            ConfigurarBotaoCriarGabarito();

            return;
        }

        protected virtual void CarregarInputsCena() {
            regiaoCarregamentoInputsCena = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_CENA);
            regiaoCarregamentoInputsCena.Add(grupoInputsCena.Root);

            grupoInputsCena.VincularDados(manipuladorCena);

            return;
        }

        protected virtual void CarregarInputVideo() {
            regiaoCarregamentoInputsVideo = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_VIDEO);
            regiaoCarregamentoInputsVideo.Add(inputVideo.Root);

            inputVideo.CampoVideo.SetValueWithoutNotify(manipuladorContexto.GetNomeArquivoVideo());
            inputVideo.CampoVideo.RegisterCallback<ChangeEvent<string>>(evt => {
                manipuladorContexto.SetNomeArquivoVideo(evt.newValue);
            });

            return;
        }

        protected virtual void CarregarBotoesConfirmacao() {
            botoesConfirmacao = new();
            botoesConfirmacao.BotaoConfirmar.clicked += HandleBotaoConfirmarClick;
            botoesConfirmacao.BotaoCancelar.clicked += HandleBotaoCancelarClick;

            regiaoCarregamentoBotoesConfirmacao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO);
            regiaoCarregamentoBotoesConfirmacao.Add(botoesConfirmacao.Root);

            return;
        }

        protected virtual void HandleBotaoConfirmarClick() {
            Navigator.Instance.Voltar();
            return;
        }

        protected virtual void HandleBotaoCancelarClick() {
            Navigator.Instance.Voltar();
            return;
        }

        protected virtual void CarregarOpcoesRadioButtons() {
            opcaoRadioArrastar = root.Query<RadioButton>(NOME_OPCAO_RADIO_ARRASTAR);
            opcaoRadioArrastar.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            opcaoRadioArrastar.SetValueWithoutNotify(false);
            opcaoRadioArrastar.RegisterCallback<ChangeEvent<bool>>(evt => {
                if(!evt.newValue) {
                    return;
                }

                botaoCriarGabarito.SetEnabled(true);
                manipuladorCena.SetTipoGabarito(TipoGabarito.Arrastar);
            });

            opcaoRadioSelecionar = root.Query<RadioButton>(NOME_OPCAO_RADIO_SELECIONAR);
            opcaoRadioSelecionar.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            opcaoRadioSelecionar.SetValueWithoutNotify(false);
            opcaoRadioSelecionar.RegisterCallback<ChangeEvent<bool>>(evt => {
                if(!evt.newValue) {
                    return;
                }

                botaoCriarGabarito.SetEnabled(true);
                manipuladorCena.SetTipoGabarito(TipoGabarito.Selecionar);
            });

            return;
        }

        protected virtual void CarregarTooltips() {
            regiaoCarregamentoTooltipTipoGabarito = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_GABARITO);
            regiaoCarregamentoTooltipTipoGabarito.Add(tooltipTipoGabarito.Root);

            return;
        }

        protected virtual void ConfigurarBotaoCriarGabarito() {
            botaoCriarGabarito = root.Query<Button>(NOME_BOTAO_CRIAR_GABARITO);
            botaoCriarGabarito.clicked += HandleBotaoCriarGabaritoClick;
            botaoCriarGabarito.SetEnabled(false);

            return;
        }

        protected virtual void HandleBotaoCriarGabaritoClick() {
            if(opcaoRadioArrastar.value) {
                Navigator.Instance.IrPara(new GabaritoArrastarBehaviour());
            }

            if(opcaoRadioSelecionar.value) {
                Navigator.Instance.IrPara(new GabaritoSelecionarBehaviour());
            }

            return;
        }
    }
}