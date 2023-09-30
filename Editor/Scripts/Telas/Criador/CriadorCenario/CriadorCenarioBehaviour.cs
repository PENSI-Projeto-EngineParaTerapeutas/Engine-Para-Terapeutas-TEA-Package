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

        protected const string NOME_REGIAO_CARREGAMENTO_INPUT_NOME = "regiao-input-nome";
        protected VisualElement regiaoCarregamenteInputNome;

        protected const string NOME_REGIAO_CARREGAMENTO_INPUTS_IMAGEM = "regiao-carregamento-inputs-imagem";
        protected VisualElement regiaoCarregamentoInputsImagem;

        protected const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        protected VisualElement regiaoCarregamentoBotoesConfirmacao;

        protected InputTexto campoNome;
        protected readonly InputsComponenteImagemCenario grupoInputsImagem;
        protected BotoesConfirmacao botoesConfirmacao;

        #endregion

        protected readonly ManipuladorCenario manipulador;

        public CriadorCenarioBehaviour() {
            grupoInputsImagem = new InputsComponenteImagemCenario();
            
            manipulador = new ManipuladorCenario();
            manipulador.Criar();

            CarregarRegiaoInputsImagem();
            ConfigurarBotoesConfirmacao();
            VincularCamposAoNovoObjeto();

            return;
        }

        protected virtual void ConfigurarCampoNome() {
            campoNome = new InputTexto("Nome:");

            campoNome.CampoTexto.RegisterCallback<ChangeEvent<string>>(evt => {
                manipulador.SetNome(evt.newValue);
            });

            regiaoCarregamenteInputNome = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_NOME);
            regiaoCarregamenteInputNome.Add(campoNome.Root);

            return;
        }

        protected virtual void CarregarRegiaoInputsImagem() {
            regiaoCarregamentoInputsImagem = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_IMAGEM);
            regiaoCarregamentoInputsImagem.Add(grupoInputsImagem.Root);

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

        protected virtual void VincularCamposAoNovoObjeto() {
            grupoInputsImagem.InputCor.CampoCor.RegisterCallback<ChangeEvent<Color>>(evt => {
                manipulador.SetCorSolida(evt.newValue);
            });

            grupoInputsImagem.InputImagem.CampoImagem.RegisterCallback<ChangeEvent<Object>>(evt => {
                manipulador.SetImagem(evt.newValue as Sprite);
            });

            return;
        }

        public void ReiniciarCampos() {
            grupoInputsImagem.ReiniciarCampos();

            return;
        }
    }
}