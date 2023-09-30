using UnityEngine.UIElements;
using Autis.Editor.Utils;

namespace Autis.Editor.UI {
    public class BotoesConfirmacao : ElementoInterfaceEditor {
        protected override string CaminhoTemplate => "ElementosUI/BotoesConfirmacao/BotoesConfirmacaoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/BotoesConfirmacao/BotoesConfirmacaoStyle.uss";

        #region .: Elementos :.

        public Button BotaoConfirmar { get => botaoConfirmar; }
        public Button BotaoCancelar { get => botaoCancelar; }

        private const string NOME_BOTAO_CONFIRMAR = "botao-confirmar";
        private Button botaoConfirmar;

        private const string NOME_IMAGEM_BOTAO_CONFIRMAR = "imagem-icone-confirmar";
        private Image imagemBotaoConfirmar;

        private const string NOME_BOTAO_CANCELAR = "botao-cancelar";
        private Button botaoCancelar;

        private const string NOME_IMAGEM_BOTAO_CANCELAR = "imagem-icone-cancelar";
        private Image imagemBotaoCancelar;

        #endregion

        public BotoesConfirmacao() {
            ConfigurarBotaoConfirmar();
            ConfigurarBotaoCancelar();

            return;
        }

        private void ConfigurarBotaoConfirmar() {
            botaoConfirmar = root.Query<Button>(NOME_BOTAO_CONFIRMAR);
            
            imagemBotaoConfirmar = root.Query<Image>(NOME_IMAGEM_BOTAO_CONFIRMAR);
            imagemBotaoConfirmar.image = Importador.ImportarImagem("icone-confirmar.png");

            return;
        }

        private void ConfigurarBotaoCancelar() {
            botaoCancelar = root.Query<Button>(NOME_BOTAO_CANCELAR);
            
            imagemBotaoCancelar = root.Query<Image>(NOME_IMAGEM_BOTAO_CANCELAR);
            imagemBotaoCancelar.image = Importador.ImportarImagem("icone-cancelar.png");

            return;
        }
    }
}