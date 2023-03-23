using UnityEngine.UIElements;
using EngineParaTerapeutas.Telas;

namespace EngineParaTerapeutas.UI {
    public class BotoesConfirmacao : ElementoInterfaceEditor {
        protected override string CaminhoTemplate => "ElementosUI/BotoesConfirmacao/BotoesConfirmacaoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/BotoesConfirmacao/BotoesConfirmacaoStyle.uss";

        #region .: Elementos :.

        public Button BotaoConfirmar { get => botaoConfirmar; }
        public Button BotaoCancelar { get => botaoCancelar; }

        private const string NOME_BOTAO_CONFIRMAR = "botao-confirmar";
        private readonly Button botaoConfirmar;

        private const string NOME_BOTAO_CANCELAR = "botao-cancelar";
        private readonly Button botaoCancelar;

        #endregion

        public BotoesConfirmacao() {
            botaoConfirmar = Root.Query<Button>(NOME_BOTAO_CONFIRMAR);
            botaoCancelar = Root.Query<Button>(NOME_BOTAO_CANCELAR);

            ConfigurarBotoesConfirmacao();

            return;
        }

        private void ConfigurarBotoesConfirmacao() {
            botaoConfirmar.clicked += HandleBotaoConfirmarClick;
            botaoCancelar.clicked += HandleBotaoCancelarClick;

            return;
        }

        private void HandleBotaoCancelarClick() {
            Navigator.Instance.Voltar();
            return;
        }

        private void HandleBotaoConfirmarClick() {
            Navigator.Instance.Voltar();
            return;
        }
    }
}