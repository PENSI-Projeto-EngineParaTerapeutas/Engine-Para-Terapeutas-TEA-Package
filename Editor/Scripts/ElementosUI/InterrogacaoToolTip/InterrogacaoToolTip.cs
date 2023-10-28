using UnityEngine.UIElements;
using Autis.Editor.Utils;

namespace Autis.Editor.UI {

    public class InterrogacaoToolTip : ElementoInterfaceEditor {
        protected override string CaminhoTemplate => "ElementosUI/InterrogacaoToolTip/InterrogacaoToolTipTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InterrogacaoToolTip/InterrogacaoToolTipStyle.uss";

        #region .: Elementos :.

        public Image ImagemTooltip { get => imagemTooltip; }

        private const string NOME_IMAGEM_TOOLTIP = "imagem-interrogacao";
        private Image imagemTooltip;

        #endregion

        public InterrogacaoToolTip() {
            ConfigurarImagemToolTip();
            return;
        }

        public InterrogacaoToolTip(string conteudo) {
            ConfigurarImagemToolTip();
            SetTexto(conteudo);

            return;
        }

        private void ConfigurarImagemToolTip() {
            imagemTooltip = Root.Query<Image>(NOME_IMAGEM_TOOLTIP);
            imagemTooltip.image = Importador.ImportarImagem("interrogacao.png");

            return;
        }

        public void SetTexto(string conteudo) {
            imagemTooltip.tooltip = conteudo;
            return;
        }
    }
}