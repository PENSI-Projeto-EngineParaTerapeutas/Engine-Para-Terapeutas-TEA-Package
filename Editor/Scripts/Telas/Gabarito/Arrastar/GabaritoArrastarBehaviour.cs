using UnityEngine.UIElements;
using Autis.Editor.UI;
using Autis.Editor.Manipuladores;
using System.Collections.Generic;

namespace Autis.Editor.Telas {
    public class GabaritoArrastarBehaviour : Tela {
        protected override string CaminhoTemplate => "Telas/Gabarito/Arrastar/GabaritoArrastarTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Gabarito/Arrastar/GabaritoArrastarStyle.uss";

        #region .: Mensagens :.

        protected const string MENSAGEM_TOOLTIP_TITULO = "TODO: Adicionar tooltip";

        #endregion

        #region .: Elementos :.

        protected const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-carregamento-tooltip-titulo";
        protected VisualElement regiaoCarregamentoTooltipTitulo;

        protected const string NOME_REIGAO_CARREGAMENTO_ASSOCIACOES = "regiao-carregamento-associacoes";
        protected ScrollView scrollviewAssociacoes;

        protected const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        protected VisualElement regiaoCarregamentoBotoesConfirmacao;

        protected InterrogacaoToolTip tooltipTitulo;
        protected readonly List<AssociacaoArrastavel> displaysAssociacoes = new();
        protected BotoesConfirmacao botoesConfirmacao;

        #endregion

        protected readonly ManipuladorGabaritoArrastar manipuladorGabaritoArrastar;

        public GabaritoArrastarBehaviour() {
            manipuladorGabaritoArrastar = new ManipuladorGabaritoArrastar();

            ConfigurarTooltipTitulo();
            ConfigurarScrollviewAssociacoes();
            ConfigurarBotoesConfirmacao();

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

        protected virtual void ConfigurarBotoesConfirmacao() {
            botoesConfirmacao = new();
            botoesConfirmacao.BotaoConfirmar.clicked += HandleBotaoConfirmarClick;
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