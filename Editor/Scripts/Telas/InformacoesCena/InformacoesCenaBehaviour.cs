using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Autis.Editor.Constantes;
using Autis.Editor.UI;
using Autis.Editor.Manipuladores;
using Autis.Runtime.DTOs;

namespace Autis.Editor.Telas {
    public class InformacoesCenaBehaviour : Tela {
        protected override string CaminhoTemplate => "Telas/InformacoesCena/InformacoesCenaTemplate.uxml";
        protected override string CaminhoStyle => "Telas/InformacoesCena/InformacoesCenaStyle.uss";

        #region .: Mensagens :.

        protected const string MENSAGEM_TOOLTIP_TITULO = "[TODO]: Adicionar.";
        protected const string MENSAGEM_TOOLTIP_CAMPO_NOME = "[TODO]: Adicionar.";
        protected const string MENSAGEM_TOOLTIP_INPUT_VIDEO_CONTEXTO = "[TODO]: Adicionar.";
        protected const string MENSAGEM_TOOLTIP_DROPDOWN_DIFICULDADE = "[TODO]: Adicionar.";
        protected const string MENSAGEM_TOOLTIP_INPUT_FAIXA_ETARIA = "[TODO]: Adicionar.";
        protected const string MENSAGEM_TOOLTIP_ACAO_ESPERADA = "[TODO]: Adicionar.";

        #endregion

        #region .: Elementos :.

        protected const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-tooltip-titulo";
        protected VisualElement regiaoTooltipTitulo;

        protected const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_ACAO_ESPERADA = "regiao-carregamento-tooltip-acao-esperada";
        protected VisualElement regiaoTooltipAcaoEsperada;

        protected const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_INPUT_VIDEO = "regiao-carregamento-tooltip-input-video";
        protected VisualElement regiaoTooltipInputVideo;

        protected const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_FAIXA_ETARIA = "regiao-carregamento-tooltip-faixa-etaria";
        protected VisualElement regiaoTooltipFaixaEtaria;

        protected const string NOME_REGIAO_CARREGAMENTO_CAMPO_NOME = "regiao-carregamento-input-nome";
        protected VisualElement regiaoCarregamentoCampoNome;

        protected const string NOME_REGIAO_CARREGAMENTO_INPUT_VIDEO_CONTEXTO = "regiao-carregamento-inputs-video";
        protected VisualElement regiaoInputVideoContexto;

        protected const string NOME_REGIAO_CARREGAMENTO_DROPDOWN_NIVEL_DIFICULDADE = "regiao-carregamento-dropdown-nivel-dificuldade";
        protected VisualElement regiaoDropdownNivelDificuldade;

        protected const string NOME_REGIAO_CARREGAMENTO_INPUT_FAIXA_INFERIOR = "regiao-input-faixa-etaria-inferior";
        protected VisualElement regiaoInputFaixaInferior;

        protected const string NOME_REGIAO_CARREGAMENTO_INPUT_FAIXA_SUPERIOR = "regiao-input-faixa-etaria-superior";
        protected VisualElement regiaoInputFaixaSuperior;

        protected const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_GABARITO = "regiao-tooltip-tipo-gabarito";
        protected VisualElement regiaoCarregamentoTooltipTipoGabarito;

        protected const string NOME_OPCAO_RADIO_SELECIONAR = "radio-opcao-selecionar";
        protected RadioButton opcaoRadioSelecionar;

        protected const string NOME_OPCAO_RADIO_ARRASTAR = "radio-opcao-arrastar";
        protected RadioButton opcaoRadioArrastar;

        protected const string NOME_BOTAO_CRIAR_GABARITO = "botao-criar-gabarito";
        protected Button botaoCriarGabarito;

        protected const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        protected VisualElement regiaoCarregamentoBotoesConfirmacao;

        protected InputTexto campoNome;
        protected InputVideo campoVideoContexto;
        protected Dropdown dropdownDificuldade;
        protected InputNumerico faixaEtariaInferior;
        protected InputNumerico faixaEtariaSuperior;

        protected InterrogacaoToolTip tooltipTitulo;
        protected InterrogacaoToolTip tooltipInputVideoContexto;
        protected InterrogacaoToolTip tooltipFaixaEtaria;
        protected InterrogacaoToolTip tooltipAcaoEsperada;
        protected BotoesConfirmacao botoesConfirmacao;

        #endregion

        protected readonly ManipuladorCena manipuladorCena;
        protected readonly ManipuladorContexto manipuladorContexto;

        public InformacoesCenaBehaviour() {
            manipuladorCena = new ManipuladorCena();
            manipuladorContexto = new ManipuladorContexto();

            ConfigurarCampoNome();
            ConfigurarCampoInputVideoContexto();
            ConfigurarCampoDropdownDificuldade();
            ConfigurarCampoFaixaEtaria();
            CarregarBotoesConfirmacao();
            CarregarOpcoesRadioButtons();
            ConfigurarBotaoCriarGabarito();

            ConfigurarTooltipTitulo();
            ConfigurarTooltipAcoesEsperadas();
            ConfigurarTooltipInputVideo();
            ConfigurarTooltipFaixaEtaria();

            return;
        }

        protected virtual void ConfigurarCampoNome() {
            campoNome = new InputTexto("Nome");
            
            regiaoCarregamentoCampoNome = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_CAMPO_NOME);
            regiaoCarregamentoCampoNome.Add(campoNome.Root);

            return;
        }

        protected virtual void ConfigurarCampoInputVideoContexto() {
            campoVideoContexto = new InputVideo();

            regiaoInputVideoContexto = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_VIDEO_CONTEXTO);
            regiaoInputVideoContexto.Add(campoVideoContexto.Root);

            return;
        }

        protected virtual void ConfigurarCampoDropdownDificuldade() {
            List<string> opcoes = new() {
                NiveisDificuldade.Facil.ToString(),
                NiveisDificuldade.Medio.ToString(),
                NiveisDificuldade.Dificil.ToString(),
            };

            dropdownDificuldade = new Dropdown("Nível de dificuldade: (opcional)", MENSAGEM_TOOLTIP_DROPDOWN_DIFICULDADE, opcoes);
            dropdownDificuldade.Campo.RegisterCallback<ChangeEvent<string>>(evt => {
                if(evt.newValue == Dropdown.VALOR_PADRAO_DROPDOWN) {
                    return;
                }

                manipuladorCena.SetDificuldade(Enum.Parse<NiveisDificuldade>(evt.newValue));
            });

            regiaoDropdownNivelDificuldade = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_DROPDOWN_NIVEL_DIFICULDADE);
            regiaoDropdownNivelDificuldade.Add(dropdownDificuldade.Root);

            return;
        }

        protected virtual void ConfigurarCampoFaixaEtaria() {
            faixaEtariaInferior = new InputNumerico(string.Empty);
            regiaoInputFaixaInferior = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_FAIXA_INFERIOR);
            regiaoInputFaixaInferior.Add(faixaEtariaInferior.Root);

            faixaEtariaSuperior = new InputNumerico(string.Empty);
            regiaoInputFaixaSuperior = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_FAIXA_SUPERIOR);
            regiaoInputFaixaSuperior.Add(faixaEtariaSuperior.Root);

            return;
        }

        protected virtual void ConfigurarTooltipTitulo() {
            tooltipTitulo = new InterrogacaoToolTip();
            tooltipTitulo.SetTexto(MENSAGEM_TOOLTIP_TITULO);

            regiaoTooltipTitulo = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO);
            regiaoTooltipTitulo.Add(tooltipTitulo.Root);

            return;
        }

        protected virtual void ConfigurarTooltipAcoesEsperadas() {
            tooltipAcaoEsperada = new InterrogacaoToolTip();
            tooltipAcaoEsperada.SetTexto(MENSAGEM_TOOLTIP_ACAO_ESPERADA);

            regiaoTooltipAcaoEsperada = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_ACAO_ESPERADA);
            regiaoTooltipAcaoEsperada.Add(tooltipAcaoEsperada.Root);

            return;
        }

        protected virtual void ConfigurarTooltipInputVideo() {
            tooltipInputVideoContexto = new InterrogacaoToolTip();
            tooltipInputVideoContexto.SetTexto(MENSAGEM_TOOLTIP_INPUT_VIDEO_CONTEXTO);

            regiaoTooltipInputVideo = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_INPUT_VIDEO);
            regiaoTooltipInputVideo.Add(tooltipInputVideoContexto.Root);

            return;
        }

        protected virtual void ConfigurarTooltipFaixaEtaria() {
            tooltipFaixaEtaria = new InterrogacaoToolTip();
            tooltipFaixaEtaria.SetTexto(MENSAGEM_TOOLTIP_INPUT_FAIXA_ETARIA);

            regiaoTooltipFaixaEtaria = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_FAIXA_ETARIA);
            regiaoTooltipFaixaEtaria.Add(tooltipFaixaEtaria.Root);

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