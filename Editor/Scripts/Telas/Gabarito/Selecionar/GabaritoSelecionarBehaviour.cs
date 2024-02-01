using System.Collections.Generic;
using UnityEngine.UIElements;
using Autis.Editor.UI;
using Autis.Editor.Constantes;
using Autis.Editor.Manipuladores;

namespace Autis.Editor.Telas {
    public class GabaritoSelecionarBehaviour : Tela {
        protected override string CaminhoTemplate => "Telas/Gabarito/Selecionar/GabaritoSelecionarTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Gabarito/Selecionar/GabaritoSelecionarStyle.uss";

        #region .: Mensagens :.

        protected const string MENSAGEM_TOOLTIP_TITULO = "Indicação do conjunto de Elementos que é esperado que o usuário selecione durante o jogo";
        protected const string MENSAGEM_TOOLTIP_TOGGLE_ORDEM = "Indica que durante o jogo é esperado que o jogador selecione os Elementos em uma ordem específica.";
        protected const string MENSAGEM_TOOLTIP_DROPDOWN = "Elemento que é esperado que o usuário selecione durante o jogo";

        #endregion

        #region .: Elementos :.

        protected const string NOME_TOGGLE_ORDEM = "input-ordem-importa";
        protected Toggle toggleOrdem;

        protected const string NOME_REGIAO_CARREGAMENTO_DROPDOWN = "regiao-dropdown-objetos-selecionaveis";
        protected VisualElement regiaoCarregamentoDropdownObjetos;

        protected const string NOME_BOTAO_ADICIONAR_OBJETO = "botao-adicionar-objeto";
        protected Button botaoAdicionarObjeto;

        protected const string NOME_LISTA_OBJETOS_SELECIONAVEIS = "lista-objetos-selecionaveis";
        protected ListView listViewObjetosSelecionaveis;

        protected const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-carregamento-tooltip-titulo";
        protected VisualElement regiaoCarregamentoTooltipTitulo;

        protected const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TOGGLE_ORDEM = "regiao-carregamento-tooltip-ordem-selecao";
        protected VisualElement regiaoCarregamentoTooltipToggleOrdem;

        protected const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        protected VisualElement regiaoCarregamentoBotoesConfirmacao;

        protected Dropdown dropdownObjetos;
        protected Tooltip tooltipTitulo;
        protected Tooltip tooltipToogleOrdem;
        protected BotoesConfirmacao botoesConfirmacao;

        #endregion

        protected readonly List<string> ordemObjetosInteracao;
        protected readonly ManipuladorGabritoSelecao manipuladorGabarito;

        public GabaritoSelecionarBehaviour() {
            manipuladorGabarito = new ManipuladorGabritoSelecao();
            ordemObjetosInteracao = manipuladorGabarito.OrdemSelecaoElementos;

            ConfigurarTooltipTitulo();
            ConfigurarToggleOrdem();
            ConfigurarDropdowObjetos();
            ConfigurarBotaoAdicionarObjeto();
            ConfigurarListViewObjetosSelecionaveis();
            ConfigurarBotoesConfirmacao();

            return;
        }

        protected virtual void ConfigurarTooltipTitulo() {
            regiaoCarregamentoTooltipTitulo = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO);

            tooltipTitulo = new Tooltip();
            tooltipTitulo.SetTexto(MENSAGEM_TOOLTIP_TITULO);
            regiaoCarregamentoTooltipTitulo.Add(tooltipTitulo.Root);

            return;
        }

        protected virtual void ConfigurarToggleOrdem() {
            toggleOrdem = root.Query<Toggle>(NOME_TOGGLE_ORDEM);
            toggleOrdem.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            toggleOrdem.SetValueWithoutNotify(false);

            toggleOrdem.RegisterCallback<ChangeEvent<bool>>(evt => {
                listViewObjetosSelecionaveis.reorderable = evt.newValue;
                manipuladorGabarito.OrdemEhRelevante = evt.newValue;
            });

            tooltipToogleOrdem = new Tooltip();
            tooltipToogleOrdem.SetTexto(MENSAGEM_TOOLTIP_TOGGLE_ORDEM);

            regiaoCarregamentoTooltipToggleOrdem = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_TOGGLE_ORDEM);
            regiaoCarregamentoTooltipToggleOrdem.Add(tooltipToogleOrdem.Root);

            return;
        }

        protected virtual void ConfigurarDropdowObjetos() {
            List<string> nomesObjetosInteracao = new();
            foreach(ManipuladorObjetoInteracao manipualdor in manipuladorGabarito.ElementosInteracaoSelecionaveis) {
                nomesObjetosInteracao.Add(manipualdor.GetNome());
            }

            dropdownObjetos = new Dropdown("Selecione objeto", MENSAGEM_TOOLTIP_DROPDOWN, nomesObjetosInteracao);
            
            regiaoCarregamentoDropdownObjetos = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_DROPDOWN);
            regiaoCarregamentoDropdownObjetos.Add(dropdownObjetos.Root);

            return;
        }

        protected virtual void ConfigurarBotaoAdicionarObjeto() {
            botaoAdicionarObjeto = root.Query<Button>(NOME_BOTAO_ADICIONAR_OBJETO);
            botaoAdicionarObjeto.clicked += HandleBotaoAdicionarObjetoClick;

            return;
        }

        protected virtual void HandleBotaoAdicionarObjetoClick() {
            if(dropdownObjetos.Campo.value == Dropdown.VALOR_PADRAO_DROPDOWN) {
                return;
            }

            string objetoAdicionado = dropdownObjetos.Campo.value;

            listViewObjetosSelecionaveis.itemsSource.Add(objetoAdicionado);
            listViewObjetosSelecionaveis.Rebuild();

            dropdownObjetos.Campo.choices.Remove(objetoAdicionado);
            dropdownObjetos.Campo.SetValueWithoutNotify(Dropdown.VALOR_PADRAO_DROPDOWN);

            return;
        }

        protected virtual void ConfigurarListViewObjetosSelecionaveis() {
            listViewObjetosSelecionaveis = root.Query<ListView>(NOME_LISTA_OBJETOS_SELECIONAVEIS);
            listViewObjetosSelecionaveis.reorderMode = ListViewReorderMode.Animated;
            listViewObjetosSelecionaveis.reorderable = false;
            listViewObjetosSelecionaveis.showAddRemoveFooter = false;
            listViewObjetosSelecionaveis.showFoldoutHeader = false;

            VisualElement makeItem() { 
                VisualElement container = new();
                container.AddToClassList("container-list-view");

                return container;
            };

            void bindItem(VisualElement elemento, int index) {
                elemento.Clear();

                ObjetoSelecionavelGabarito objetoSelecionavelGabarito = new(ordemObjetosInteracao[index]) {
                    CallbackExcluirObjeto = HandleExcluirObjeto,
                };

                elemento.Add(objetoSelecionavelGabarito.Root);
            };

            listViewObjetosSelecionaveis.makeItem = makeItem;
            listViewObjetosSelecionaveis.bindItem = bindItem;
            listViewObjetosSelecionaveis.itemsSource = ordemObjetosInteracao;
            listViewObjetosSelecionaveis.selectionType = SelectionType.None;

            return;
        }

        protected virtual void HandleExcluirObjeto(ObjetoSelecionavelGabarito elementoObjetoExcluido) {
            string nomeObjetoExcluido = elementoObjetoExcluido.NomeObjetoAssociado;

            listViewObjetosSelecionaveis.itemsSource.Remove(nomeObjetoExcluido);
            listViewObjetosSelecionaveis.Rebuild();

            dropdownObjetos.Campo.choices.Add(nomeObjetoExcluido);

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
            manipuladorGabarito.Finalizar();
            Navigator.Instance.Voltar();

            return;
        }

        protected virtual void HandleBotaoCancelarClick() {
            Navigator.Instance.Voltar();
            return;
        }
    }
}