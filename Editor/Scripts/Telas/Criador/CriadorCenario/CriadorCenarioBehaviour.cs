using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using Autis.Editor.UI;
using Autis.Editor.Telas;
using Autis.Editor.Manipuladores;
using Autis.Editor.Constantes;
using Autis.Editor.Excecoes;
using Autis.Runtime.Eventos;
using Autis.Editor.Utils;

namespace Autis.Editor.Criadores {
    public class CriadorCenarioBehaviour : Tela, IReiniciavel {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorCenario/CriadorCenarioTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorCenario/CriadorCenarioStyle.uss";

        #region .: Mensagem :.

        private const string MENSAGEM_ERRO_TIPO_CENARIO_NAO_SELECIONADO = "Escolha uma opção dentre as abaixo para o tipo de Cenário:\n";
        private const string MENSAGEM_ERRO_CENARIO_IMAGEM_NAO_SELECIONADO = "Escolha uma imagem para o Cenário.\n";

        protected const string MENSAGEM_TOOLTIP_TITULO = "O cenário é o plano de fundo da fase do jogo.";
        #endregion

        #region .: Eventos :.

        protected static EventoJogo eventoFinalizarCriacao;

        #endregion

        #region .: Elementos :.

        protected readonly string NOME_RADIO_IMAGEM = "radio-opcao-imagem";
        protected RadioButton radioButtonImagem;

        protected const string NOME_REGIAO_INPUT_IMAGEM = "regiao-input-imagem";
        protected VisualElement regiaoInputImagem;

        protected const string NOME_REGIAO_INPUT_COR = "regiao-input-cor";
        protected VisualElement regiaoInputCor;

        protected readonly string NOME_RADIO_COR_UNICA = "radio-opcao-cor-unica";
        protected RadioButton radioButtonCorUnica;

        protected const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        protected VisualElement regiaoCarregamentoBotoesConfirmacao;

        protected InputImagem inputImagem;
        protected InputCor inputCor;

        protected BotoesConfirmacao botoesConfirmacao;

        protected const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-tooltip-titulo";
        protected VisualElement regiaoCarregamentoTooltipTitulo;

        protected InterrogacaoToolTip tooltipTitulo;

        #endregion

        protected readonly ManipuladorCenario manipulador;

        public CriadorCenarioBehaviour() {
            manipulador = new ManipuladorCenario();
            manipulador.Criar();

            eventoFinalizarCriacao = Importador.ImportarEvento("EventoFinalizarCriacao");

            ConfigurarTooltipTitulo();
            CarregarRegiaoInputImagem();
            CarregarRegiaoInputsCor();

            ConfigurarBotoesConfirmacao();

            return;
        }

        protected virtual void ConfigurarTooltipTitulo() {
            tooltipTitulo = new InterrogacaoToolTip(MENSAGEM_TOOLTIP_TITULO);

            regiaoCarregamentoTooltipTitulo = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO);
            regiaoCarregamentoTooltipTitulo.Add(tooltipTitulo.Root);

            return;
        }

        protected virtual void CarregarRegiaoInputImagem() {
            inputImagem = new InputImagem();
            inputImagem.Root.SetEnabled(false);

            radioButtonImagem = Root.Query<RadioButton>(NOME_RADIO_IMAGEM);
            radioButtonImagem.RegisterCallback<ChangeEvent<bool>>(evt => {
                if(!evt.newValue) {
                    return;
                }

                inputCor.Root.SetEnabled(false);
                inputImagem.Root.SetEnabled(true);

                if(!inputImagem.EstaVazio()) {
                    manipulador.SetImagem(inputImagem.CampoImagem.value as Sprite);
                }
            });

            inputImagem.CampoImagem.RegisterCallback<ChangeEvent<Object>>(evt => {
                if(evt.newValue == null) {
                    manipulador.SetCorSolida(inputCor.CampoCor.value);
                    return;
                }

                manipulador.SetImagem(evt.newValue as Sprite);
            });

            regiaoInputImagem = Root.Query<VisualElement>(NOME_REGIAO_INPUT_IMAGEM);
            regiaoInputImagem.Add(inputImagem.Root);

            return;
        }

        protected virtual void CarregarRegiaoInputsCor() {
            inputCor = new InputCor();
            inputCor.Root.SetEnabled(false);

            radioButtonCorUnica = root.Query<RadioButton>(NOME_RADIO_COR_UNICA);
            radioButtonCorUnica.RegisterCallback<ChangeEvent<bool>>(evt => {
                if(!evt.newValue) {
                    return;
                }

                inputImagem.Root.SetEnabled(false);
                inputCor.Root.SetEnabled(true);
                
                manipulador.SetCorSolida(inputCor.CampoCor.value);
            });

            inputCor.CampoCor.RegisterCallback<ChangeEvent<Color>>(evt => {
                manipulador.SetCorSolida(evt.newValue);
            });

            regiaoInputCor = Root.Query<VisualElement>(NOME_REGIAO_INPUT_COR);
            regiaoInputCor.Add(inputCor.Root);

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
            try {
                VerificarCamposObrigatorios();
            }
            catch(ExcecaoCamposObrigatoriosVazios error) {
                PopupAvisoBehaviour.ShowPopupAviso(error.Message);
                return;
            }

            try {
                manipulador.Finalizar();
            }
            catch(ExcecaoObjetoDuplicado excecao) {
                PopupAvisoBehaviour.ShowPopupAviso(MensagensGerais.MENSAGEM_ATOR_DUPLICADO.Replace("{nome}", excecao.NomeObjetoDuplicado));
                return;
            }

            eventoFinalizarCriacao.AcionarCallbacks();
            Navigator.Instance.Voltar();
            
            return;
        }

        protected virtual void VerificarCamposObrigatorios() {
            if(!radioButtonCorUnica.value && !radioButtonImagem.value) {
                string mensagem = MENSAGEM_ERRO_TIPO_CENARIO_NAO_SELECIONADO;
                mensagem += "\t" + radioButtonCorUnica.label + "\n";
                mensagem += "\t" + radioButtonImagem.label + "\n";

                throw new ExcecaoCamposObrigatoriosVazios(mensagem);
            }

            if(radioButtonImagem.value && inputImagem.EstaVazio()) {
                throw new ExcecaoCamposObrigatoriosVazios(MENSAGEM_ERRO_CENARIO_IMAGEM_NAO_SELECIONADO);
            }

            return;
        }

        protected virtual void HandleBotaoCancelarClick() {
            manipulador.Cancelar();

            eventoFinalizarCriacao.AcionarCallbacks();
            Navigator.Instance.Voltar();

            return;
        }

        public void ReiniciarCampos() {
            radioButtonImagem.SetValueWithoutNotify(false);
            inputImagem.ReiniciarCampos();
            inputImagem.Root.SetEnabled(false);

            radioButtonCorUnica.SetValueWithoutNotify(false);
            inputCor.ReiniciarCampos();
            inputCor.Root.SetEnabled(false);

            return;
        }
    }
}