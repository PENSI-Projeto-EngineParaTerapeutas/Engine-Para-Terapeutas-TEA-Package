using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Autis.Editor.UI;
using Autis.Runtime.Constantes;
using Autis.Editor.Manipuladores;
using Autis.Editor.DTOs;
using Autis.Editor.Criadores;
using Autis.Runtime.Eventos;
using Autis.Editor.Utils;

namespace Autis.Editor.Telas {
    public class AdicionarAcaoBehaviour : Tela {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorPersonagem/AdicionarAcao/AdicionarAcaoTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorPersonagem/AdicionarAcao/AdicionarAcaoStyle.uss";

        #region .: Mensagens :.

        private const string MENSAGEM_TOOLTIP_DROPDOWN_ELEMENTOS_INTERACAO = "Elemento que ao ser selecionado fará com que uma animação com o personagem seja exibida.";
        private const string MENSAGEM_TOOLTIP_DROPDOWN_ANIMACOES = "Animação com o personagem que será apresentada quando um determinado Elemento for selecionado.";

        private const string MENSAGEM_AVISO_CONFIRMAR_SEM_SELECIONAR_CAMPOS_OBRIGATORIOS = "[AVISO]: É necessário que o campo {nome-campo} esteja preenchido antes de confirmar.";

        #endregion

        #region .: Eventos :.

        public Action<AcaoPersonagem> OnFinalizarCriacao { get; set; }
        
        protected static EventoJogo eventoIniciarCriacao;
        protected static EventoJogo eventoFinalizarCriacao;

        #endregion

        #region .: Elementos :.

        private const string NOME_RADIO_SELECIONAR_ELEMENTO_EXISTENTE = "radio-opcao-selecionar-objeto-existente";
        private RadioButton radioSelecionarElementoExistente;

        private const string NOME_REGIAO_CARREGAMENTO_INPUT_ELEMENTO_INTERACAO = "regiao-carregamento-selecao-elemento-existente";
        private VisualElement regiaoCarregamentoInputElementoInteracao;

        private const string NOME_RADIO_CRIAR_NOVO_ELEMENTO = "radio-opcao-criar-novo-objeto";
        private RadioButton radioCriarNovoElemento;

        private const string NOME_BOTAO_CRIAR_ELEMENTO = "botao-criar-elemento";
        private Button botaoCriarElemento;

        private const string NOME_REGIAO_CARREGAMENTO_INPUT_ANIMACOES = "regiao-carregamento-input-animacao";
        private VisualElement regiaoCarregamentoInputAnimacoes;

        protected const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        protected VisualElement regiaoCarregamentoBotoesConfirmacao;

        private Dropdown dropdownElementoInteracao;
        private Dropdown dropdownAnimacoes;
        protected BotoesConfirmacao botoesConfirmacao;

        private const string NOME_REGIAO_LABEL_DEFINICAO_ACAO = "regiao-label-definicao-acao";
        private VisualElement regiaoLabelDefinicaoAcao;
        private VisualElement regiaoCarregamentoTooltipDefinicaoAcao;
        private InterrogacaoToolTip tooltipDefinicaoAcao;
        private const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_DEFINICAO_ACAO = "regiao-tooltip-definicao-acao";

        private const string NOME_REGIAO_LABEL_DEFINICAO_ANIMACAO = "regiao-label-definicao-animacao";
        private VisualElement regiaoLabelDefinicaoAnimacao;
        private VisualElement regiaoCarregamentoTooltipDefinicaoAnimacao;
        private InterrogacaoToolTip tooltipDefinicaoAnimacao;
        private const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_DEFINICAO_ANIMACAO = "regiao-tooltip-definicao-animacao";

        #endregion

        private readonly ManipuladorPersonagens manipulador;
        
        private List<AnimationClip> clipsAnimacoes;
        private List<GameObject> elementosInteracao;

        private GameObject elementoInteracaoSelecionado = null;
        private AnimationClip animacaoSelecionada = null;

        private readonly List<DisplayAcao> acoesJaCriadas;

        public AdicionarAcaoBehaviour(ManipuladorPersonagens manipulador, List<DisplayAcao> acoesJaCriadas) {
            this.manipulador = manipulador;
            this.acoesJaCriadas = acoesJaCriadas;

            eventoIniciarCriacao = Importador.ImportarEvento("EventoIniciarCriacao");
            eventoFinalizarCriacao = Importador.ImportarEvento("EventoFinalizarCriacao");

            CarregarListas();
            ConfigurarDropdownElementoInteracao();
            ConfigurarBotaoCriarElemento();
            ConfigurarDropdownAnimacoes();
            ConfigurarRadioButtons();
            ConfigurarBotoesConfirmacao();
            
            return;
        }

        public AdicionarAcaoBehaviour(ManipuladorPersonagens manipulador, List<DisplayAcao> acoesJaCriadas, AcaoPersonagem acaoEditada) {
            this.manipulador = manipulador;
            this.acoesJaCriadas = acoesJaCriadas;

            CarregarListas();
            ConfigurarDropdownElementoInteracao();
            ConfigurarBotaoCriarElemento();
            ConfigurarDropdownAnimacoes();
            ConfigurarRadioButtons();
            ConfigurarBotoesConfirmacao();

            CarregarDados(acaoEditada);

            return;
        }

        private void CarregarDados(AcaoPersonagem acaoEditada) {
            elementoInteracaoSelecionado = acaoEditada.ObjetoGatilho;
            animacaoSelecionada = acaoEditada.Animacao;

            elementosInteracao.Add(elementoInteracaoSelecionado);
            dropdownElementoInteracao.Campo.choices.Add(elementoInteracaoSelecionado.name);

            radioSelecionarElementoExistente.SetValueWithoutNotify(true);
            dropdownElementoInteracao.Root.SetEnabled(true);
            dropdownElementoInteracao.Campo.SetValueWithoutNotify(elementoInteracaoSelecionado.name);
            dropdownAnimacoes.Campo.SetValueWithoutNotify(animacaoSelecionada.name);

            return;
        }

        private void CarregarListas() {
            clipsAnimacoes = this.manipulador.GetAnimacoes();
            elementosInteracao = new();

            List<GameObject> todosElementosInteracao = GameObject.FindGameObjectsWithTag(NomesTags.ObjetosInteracao).ToList();
            foreach(GameObject elementoInteracao in todosElementosInteracao) {
                if(acoesJaCriadas.Find(acao => acao.AcaoVinculada.ObjetoGatilho == elementoInteracao) != null) {
                    continue;
                }

                elementosInteracao.Add(elementoInteracao);
            }

            return;
        }

        private void ConfigurarDropdownElementoInteracao() {
            regiaoLabelDefinicaoAcao = root.Query<VisualElement>(NOME_REGIAO_LABEL_DEFINICAO_ACAO);

            List<string> nomesElementosInteracao = new();
            foreach(GameObject elementoInteracao in elementosInteracao) {
                nomesElementosInteracao.Add(elementoInteracao.name);
            }
            
            dropdownElementoInteracao = new Dropdown(string.Empty, nomesElementosInteracao);
            dropdownElementoInteracao.Root.SetEnabled(false);

            dropdownElementoInteracao.Campo.RegisterCallback<ChangeEvent<string>>(evt => {
                if(evt.newValue == Dropdown.VALOR_PADRAO_DROPDOWN) {
                    elementoInteracaoSelecionado = null;
                    return;
                }

                elementoInteracaoSelecionado = elementosInteracao.Find(elemento => elemento.name == evt.newValue);
            });

            regiaoCarregamentoInputElementoInteracao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_ELEMENTO_INTERACAO);
            regiaoCarregamentoInputElementoInteracao.Add(dropdownElementoInteracao.Root);

            ConfigurarTooltipDefinicaoAcao();
            
            return;
        }

        private void ConfigurarTooltipDefinicaoAcao() {
            tooltipDefinicaoAcao = new InterrogacaoToolTip();
            tooltipDefinicaoAcao.SetTexto(MENSAGEM_TOOLTIP_DROPDOWN_ELEMENTOS_INTERACAO);

            regiaoCarregamentoTooltipDefinicaoAcao = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_DEFINICAO_ACAO);
            regiaoCarregamentoTooltipDefinicaoAcao.Add(tooltipDefinicaoAcao.Root);

            regiaoLabelDefinicaoAcao.Add(regiaoCarregamentoTooltipDefinicaoAcao);
        }

        private void ConfigurarBotaoCriarElemento() {
            botaoCriarElemento = root.Query<Button>(NOME_BOTAO_CRIAR_ELEMENTO);
            botaoCriarElemento.SetEnabled(false);

            botaoCriarElemento.clicked += HandleBotaoCriarElementoClick;

            return;
        }

        private void HandleBotaoCriarElementoClick() {
            if(!eventoFinalizarCriacao.ContemCallback(HandleEventoFinalizarCriacaoObjetoIntercacao)) {
                eventoFinalizarCriacao.AdicionarCallback(HandleEventoFinalizarCriacaoObjetoIntercacao);
            }

            Navigator.Instance.IrPara(new CriadorObjetoInteracaoBehaviour() {
                OnFinalizarCriacao = HandleFinalizarCriacao,
            });

            return;
        }

        private void HandleEventoFinalizarCriacaoObjetoIntercacao() {
            eventoIniciarCriacao.AcionarCallbacks();
            return;
        }

        private void HandleFinalizarCriacao(GameObject novoElementoInteracao) {
            radioCriarNovoElemento.SetValueWithoutNotify(false);
            radioSelecionarElementoExistente.SetValueWithoutNotify(true);

            botaoCriarElemento.SetEnabled(false);
            dropdownElementoInteracao.Root.SetEnabled(true);

            elementosInteracao.Add(novoElementoInteracao);
            dropdownElementoInteracao.Campo.choices.Add(novoElementoInteracao.name);
            dropdownElementoInteracao.Campo.SetValueWithoutNotify(novoElementoInteracao.name);

            elementoInteracaoSelecionado = novoElementoInteracao;

            return;
        }

        private void ConfigurarDropdownAnimacoes() {
            regiaoLabelDefinicaoAnimacao = root.Query<VisualElement>(NOME_REGIAO_LABEL_DEFINICAO_ANIMACAO);

            List<string> nomesClipsAnimacoes = new();
            foreach(AnimationClip clip in clipsAnimacoes) {
                nomesClipsAnimacoes.Add(clip.name);
            }

            dropdownAnimacoes = new Dropdown(string.Empty, nomesClipsAnimacoes);
            dropdownAnimacoes.Campo.RegisterCallback<ChangeEvent<string>>(evt => {
                if(evt.newValue == Dropdown.VALOR_PADRAO_DROPDOWN) {
                    animacaoSelecionada = null;
                    return;
                }

                animacaoSelecionada = clipsAnimacoes.Find(clip => clip.name == evt.newValue);
            });

            regiaoCarregamentoInputAnimacoes = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_ANIMACOES);
            regiaoCarregamentoInputAnimacoes.Add(dropdownAnimacoes.Root);

            ConfigurarTooltipDefinicaoAnimacao();

            return;
        }

        private void ConfigurarTooltipDefinicaoAnimacao() {
            tooltipDefinicaoAnimacao = new InterrogacaoToolTip();
            tooltipDefinicaoAnimacao.SetTexto(MENSAGEM_TOOLTIP_DROPDOWN_ANIMACOES);

            regiaoCarregamentoTooltipDefinicaoAnimacao = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_DEFINICAO_ANIMACAO);
            regiaoCarregamentoTooltipDefinicaoAnimacao.Add(tooltipDefinicaoAnimacao.Root);

            regiaoLabelDefinicaoAnimacao.Add(regiaoCarregamentoTooltipDefinicaoAnimacao);
        }

        private void ConfigurarRadioButtons() {
            radioSelecionarElementoExistente = root.Query<RadioButton>(NOME_RADIO_SELECIONAR_ELEMENTO_EXISTENTE);
            radioSelecionarElementoExistente.SetValueWithoutNotify(false);
            radioSelecionarElementoExistente.RegisterCallback<ChangeEvent<bool>>(evt => {
                dropdownElementoInteracao.Root.SetEnabled(evt.newValue);
            });

            radioCriarNovoElemento = root.Query<RadioButton>(NOME_RADIO_CRIAR_NOVO_ELEMENTO);
            radioCriarNovoElemento.SetValueWithoutNotify(false);
            radioCriarNovoElemento.RegisterCallback<ChangeEvent<bool>>(evt => {
                botaoCriarElemento.SetEnabled(evt.newValue);
            });

            return;
        }

        private void ConfigurarBotoesConfirmacao() {
            botoesConfirmacao = new();

            botoesConfirmacao.BotaoConfirmar.Clear();
            botoesConfirmacao.BotaoConfirmar.text = "Salvar A��o";
            botoesConfirmacao.BotaoConfirmar.clicked += HandleBotaoConfirmarClick;

            botoesConfirmacao.BotaoCancelar.Clear();
            botoesConfirmacao.BotaoCancelar.text = "Cancelar A��o";
            botoesConfirmacao.BotaoCancelar.clicked += HandleBotaoCancelarClick;

            regiaoCarregamentoBotoesConfirmacao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO);
            regiaoCarregamentoBotoesConfirmacao.Add(botoesConfirmacao.Root);

            return;
        }

        protected virtual void HandleBotaoConfirmarClick() {
            if(elementoInteracaoSelecionado == null) {
                Debug.Log(MENSAGEM_AVISO_CONFIRMAR_SEM_SELECIONAR_CAMPOS_OBRIGATORIOS.Replace("{nome-campo}", "Elemento Interação"));
                return;
            }

            if(animacaoSelecionada == null) {
                Debug.Log(MENSAGEM_AVISO_CONFIRMAR_SEM_SELECIONAR_CAMPOS_OBRIGATORIOS.Replace("{nome-campo}", "Animação"));
                return;
            }

            OnFinalizarCriacao?.Invoke(new AcaoPersonagem() {
                Animacao = animacaoSelecionada,
                ObjetoGatilho = elementoInteracaoSelecionado,
            });

            if(eventoFinalizarCriacao.ContemCallback(HandleEventoFinalizarCriacaoObjetoIntercacao)) {
                eventoFinalizarCriacao.RemoverCallback(HandleEventoFinalizarCriacaoObjetoIntercacao);
            }
            Navigator.Instance.Voltar();

            return;
        }

        protected virtual void HandleBotaoCancelarClick() {
            if(eventoFinalizarCriacao.ContemCallback(HandleEventoFinalizarCriacaoObjetoIntercacao)) {
                eventoFinalizarCriacao.RemoverCallback(HandleEventoFinalizarCriacaoObjetoIntercacao);
            }
            Navigator.Instance.Voltar();

            return;
        }
    }
}