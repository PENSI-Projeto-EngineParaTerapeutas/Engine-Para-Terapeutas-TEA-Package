using UnityEngine;
using UnityEngine.UIElements;
using Autis.Editor.UI;
using Autis.Editor.Telas;
using Autis.Editor.Manipuladores;

namespace Autis.Editor.Criadores {
    public class CriadorCenarioBehaviour : Tela, IReiniciavel {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorCenario/CriadorCenarioTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorCenario/CriadorCenarioStyle.uss";

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

        #endregion

        protected readonly ManipuladorCenario manipulador;

        public CriadorCenarioBehaviour() {
            manipulador = new ManipuladorCenario();
            manipulador.Criar();

            CarregarRegiaoInputImagem();
            CarregarRegiaoInputsCor();

            ConfigurarBotoesConfirmacao();

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

                inputImagem.Root.SetEnabled(true);
                inputCor.Root.SetEnabled(false);
            });

            inputImagem.CampoImagem.RegisterCallback<ChangeEvent<Object>>(evt => {
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
            manipulador.Finalizar();
            Navigator.Instance.Voltar();
            
            return;
        }

        protected virtual void HandleBotaoCancelarClick() {
            manipulador.Cancelar();
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