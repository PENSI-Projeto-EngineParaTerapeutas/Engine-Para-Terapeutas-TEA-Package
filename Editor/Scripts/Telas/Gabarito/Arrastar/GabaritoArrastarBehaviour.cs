using System.Collections.Generic;
using UnityEngine.UIElements;
using Autis.Editor.UI;
using Autis.Editor.Manipuladores;

namespace Autis.Editor.Telas {
    public class GabaritoArrastarBehaviour : Tela {
        protected override string CaminhoTemplate => "Telas/Gabarito/Arrastar/GabaritoArrastarTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Gabarito/Arrastar/GabaritoArrastarStyle.uss";

        #region .: Mensagens :.

        protected const string MENSAGEM_TOOLTIP_TITULO = "Indicação do local que é esperado que cada Elemento arrastável seja posicionado.";
        protected const string MENSAGEM_TOOLTIP_DESFAZER_ACAO = "Permitir que o Elemento volte para sua posição inicial caso ele seja arrastado para um local incorreto..";

        #endregion

        #region .: Elementos :.

        protected const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-carregamento-tooltip-titulo";
        protected VisualElement regiaoCarregamentoTooltipTitulo;

        protected const string NOME_REIGAO_CARREGAMENTO_ASSOCIACOES = "regiao-carregamento-associacoes";
        protected ScrollView scrollviewAssociacoes;

        protected const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_DESFAZER_ACAO = "regiao-carregamento-tooltip-desfazer-acao";
        protected VisualElement regiaoCarregamentoTooltipDesfazerAcao;

        protected const string NOME_CHECKBOX_DESFAZER_ACAO = "input-desfazer-acao";
        protected Toggle checkboxDesfazerAcao;

        protected const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        protected VisualElement regiaoCarregamentoBotoesConfirmacao;

        protected InterrogacaoToolTip tooltipTitulo;
        protected InterrogacaoToolTip toolTipDesfazerAcao;

        protected readonly List<AssociacaoArrastavel> displaysAssociacoes = new();
        protected BotoesConfirmacao botoesConfirmacao;

        #endregion

        protected readonly ManipuladorGabaritoArrastar manipuladorGabaritoArrastar;

        public GabaritoArrastarBehaviour() {
            manipuladorGabaritoArrastar = new ManipuladorGabaritoArrastar();

            ConfigurarTooltipTitulo();
            ConfigurarScrollviewAssociacoes();
            ConfigurarCheckboxDesfazerAcao();
            ConfigurarBotoesConfirmacao();
            ConfigurarTooltipDesfazerAcao();

            return;
        }

        protected virtual void ConfigurarTooltipTitulo() {
            tooltipTitulo = new InterrogacaoToolTip();
            tooltipTitulo.SetTexto(MENSAGEM_TOOLTIP_TITULO);

            regiaoCarregamentoTooltipTitulo = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO);
            regiaoCarregamentoTooltipTitulo.Add(tooltipTitulo.Root);

            return;
        }

        protected virtual void ConfigurarScrollviewAssociacoes() {
            scrollviewAssociacoes = root.Query<ScrollView>(NOME_REIGAO_CARREGAMENTO_ASSOCIACOES);
            
            foreach(ManipuladorObjetoInteracao manipuladorElemento in manipuladorGabaritoArrastar.ElementosInteracaoArrastaveis) {
                AssociacaoArrastavel associacao = new(manipuladorGabaritoArrastar, manipuladorElemento);
                
                associacao.Root.AddToClassList("associacao-elemento");
                displaysAssociacoes.Add(associacao);

                scrollviewAssociacoes.Add(associacao.Root);
            }

            return;
        }

        protected virtual void ConfigurarCheckboxDesfazerAcao() {
            checkboxDesfazerAcao = root.Query<Toggle>(NOME_CHECKBOX_DESFAZER_ACAO);

            checkboxDesfazerAcao.SetValueWithoutNotify(true);
            checkboxDesfazerAcao.RegisterCallback<ChangeEvent<bool>>(evt => {
                foreach(ManipuladorObjetoInteracao manipuladorElemento in manipuladorGabaritoArrastar.ElementosInteracaoArrastaveis) {
                    manipuladorElemento.SetDeveDesfazerAcao(evt.newValue);
                }
            });

            return;
        }

        protected virtual void ConfigurarTooltipDesfazerAcao() {
            toolTipDesfazerAcao = new InterrogacaoToolTip();
            toolTipDesfazerAcao.SetTexto(MENSAGEM_TOOLTIP_DESFAZER_ACAO);

            regiaoCarregamentoTooltipDesfazerAcao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_DESFAZER_ACAO);
            regiaoCarregamentoTooltipDesfazerAcao.Add(toolTipDesfazerAcao.Root);

            return;
        }

        protected virtual void ConfigurarBotoesConfirmacao() {
            botoesConfirmacao = new();

            botoesConfirmacao.BotaoConfirmar.Clear();
            botoesConfirmacao.BotaoConfirmar.text = "Salvar Ações\r\nEsperadas";
            botoesConfirmacao.BotaoConfirmar.clicked += HandleBotaoConfirmarClick;

            botoesConfirmacao.BotaoCancelar.Clear();
            botoesConfirmacao.BotaoCancelar.text = "Cancelar Ações Esperadas";
            botoesConfirmacao.BotaoCancelar.clicked += HandleBotaoCancelarClick;

            regiaoCarregamentoBotoesConfirmacao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO);
            regiaoCarregamentoBotoesConfirmacao.Add(botoesConfirmacao.Root);

            return;
        }

        protected virtual void HandleBotaoConfirmarClick() {
            foreach(AssociacaoArrastavel displayAssociaco in displaysAssociacoes) {
                ManipuladorObjetoInteracao manipuladorElementoOrigem = displayAssociaco.ObjetoOrigem;
                ManipuladorObjetoInteracao manipuladorElementoDestino = displayAssociaco.ObjetoDestino;

                manipuladorGabaritoArrastar.AdicionarAssociacao(manipuladorElementoOrigem, manipuladorElementoDestino);
            }

            manipuladorGabaritoArrastar.Finalizar();
            foreach(AssociacaoArrastavel associacao in displaysAssociacoes) {
                associacao.ReiniciarCampos();
            }

            Navigator.Instance.Voltar();

            return;
        }

        protected virtual void HandleBotaoCancelarClick() {
            Navigator.Instance.Voltar();
            return;
        }
    }
}