using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Autis.Editor.Manipuladores;

namespace Autis.Editor.UI {
    public class AssociacaoArrastavel : ElementoInterfaceEditor, IReiniciavel {
        protected override string CaminhoTemplate => "Telas/Gabarito/Arrastar/AssociacaoArrastavel/AssociacaoArrastavelTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Gabarito/Arrastar/AssociacaoArrastavel/AssociacaoArrastavelStyle.uss";

        #region .: Mensagens :.

        private const string MENSAGEM_TOOLTIP_ELEMENTO_ORIGEM = "Elemento que o usuário poderá arrastar durante o jogo.";
        private const string MENSAGEM_TOOLTIP_ELEMENTO_DESTINO = "Elemento para onde um outro Elemento poderá ser arrastado.";

        #endregion

        #region .: Elementos :.

        private const string NOME_REGIAO_TOOLTIP_ELEMENTO_ORIGEM = "regiao-carregamento-tooltip-elemento-origem";
        private VisualElement regiaoTootipElementoOrigem;

        private const string NOME_LABEL_NOME_ELEMENTO_ORIGEM = "label-nome-elemento-origem";
        private Label labelNomeElemento;

        private const string NOME_REGIAO_CARREGAMENTO_DROPDOWN_ELEMENTO_DESTINO = "regiao-carregamento-dropdown-elemento-destino";
        private VisualElement regiaoCarregamentoDropdownElementoDestino;

        private Tooltip tooltipElementoOrigem;
        private Dropdown dropdownElementoDestino;

        #endregion

        public ManipuladorObjetoInteracao ObjetoOrigem { get => manipuladorElementoVinculado; }

        public ManipuladorObjetoInteracao ObjetoDestino { get => objetoDestino; }
        private ManipuladorObjetoInteracao objetoDestino;

        private readonly ManipuladorGabaritoArrastar manipuladorGabarito;
        private readonly ManipuladorObjetoInteracao manipuladorElementoVinculado;

        private readonly static List<Action<ChangeEvent<string>, string>> onSelecaoValorDropdown = new();
        private readonly List<string> nomesElementosDestino = new();

        public AssociacaoArrastavel(ManipuladorGabaritoArrastar manipuladorGabarito, ManipuladorObjetoInteracao manipuladorElementoVinculado) {
            this.manipuladorGabarito = manipuladorGabarito;
            this.manipuladorElementoVinculado = manipuladorElementoVinculado;
            
            ConfigurarTooltipElementoOrigem();
            ConfigurarLabelNomeElemento();
            ConfigurarDropdownElementoDestino();

            onSelecaoValorDropdown.Add(HandleSelecaoValorDropdown);

            return;
        }

        private void ConfigurarTooltipElementoOrigem() {
            tooltipElementoOrigem = new Tooltip();
            tooltipElementoOrigem.SetTexto(MENSAGEM_TOOLTIP_ELEMENTO_ORIGEM);

            regiaoTootipElementoOrigem = root.Query<VisualElement>(NOME_REGIAO_TOOLTIP_ELEMENTO_ORIGEM);
            regiaoTootipElementoOrigem.Add(tooltipElementoOrigem.Root);

            return;
        }

        private void ConfigurarLabelNomeElemento() {
            labelNomeElemento = root.Query<Label>(NOME_LABEL_NOME_ELEMENTO_ORIGEM);
            labelNomeElemento.text = manipuladorElementoVinculado.GetNome();

            return;
        }

        private void ConfigurarDropdownElementoDestino() {
            foreach(ManipuladorObjetoInteracao manipulador in manipuladorGabarito.ElementosInteracao) {
                if(manipulador.ObjetoAtual == manipuladorElementoVinculado.ObjetoAtual) {
                    continue;
                }

                nomesElementosDestino.Add(manipulador.GetNome());
            }

            dropdownElementoDestino = new("Elemento destino:", MENSAGEM_TOOLTIP_ELEMENTO_DESTINO, nomesElementosDestino);
            dropdownElementoDestino.Campo.RegisterCallback<ChangeEvent<string>>(evt => {
                if(evt.newValue == Dropdown.VALOR_PADRAO_DROPDOWN) {
                    objetoDestino = null;
                    AcionarEventos(evt, manipuladorElementoVinculado.GetNome());
                    return;
                }

                objetoDestino = manipuladorGabarito.ElementosInteracao.Find(elemento => elemento.GetNome() == evt.newValue);
                AcionarEventos(evt, manipuladorElementoVinculado.GetNome());
            });

            regiaoCarregamentoDropdownElementoDestino = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_DROPDOWN_ELEMENTO_DESTINO);
            regiaoCarregamentoDropdownElementoDestino.Add(dropdownElementoDestino.Root);

            return;
        }

        private void AcionarEventos(ChangeEvent<string> evt, string elementoOrigem) {
            foreach(Action<ChangeEvent<string>, string> action in onSelecaoValorDropdown) {
                action?.Invoke(evt, elementoOrigem);
            }

            return;
        }

        private void HandleSelecaoValorDropdown(ChangeEvent<string> evt, string elementoOrigem) {
            if(manipuladorElementoVinculado.GetNome() == elementoOrigem) {
                return;
            }

            if(evt.newValue == Dropdown.VALOR_PADRAO_DROPDOWN) {
                nomesElementosDestino.Add(evt.previousValue);
                return;
            }

            nomesElementosDestino.Remove(evt.newValue);
            nomesElementosDestino.Add(evt.previousValue);

            return;
        }

        public void SetElementoDestino(ManipuladorObjetoInteracao manipulador) {
            if(objetoDestino != null) {
                nomesElementosDestino.Add(objetoDestino.GetNome());
            }

            objetoDestino = manipulador;
            dropdownElementoDestino.Campo.SetValueWithoutNotify(objetoDestino.GetNome());
            AcionarEventos(ChangeEvent<string>.GetPooled(Dropdown.VALOR_PADRAO_DROPDOWN, objetoDestino.GetNome()), manipuladorElementoVinculado.GetNome());

            return;
        }

        public void ReiniciarCampos() {
            objetoDestino = null;

            onSelecaoValorDropdown.Remove(HandleSelecaoValorDropdown);
            dropdownElementoDestino.ReiniciarCampos();

            return;
        }
    }
}