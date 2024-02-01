using UnityEngine.UIElements;
using Autis.Editor.Utils;

namespace Autis.Editor.UI {
    public class Tooltip : ElementoInterfaceEditor {
        protected override string CaminhoTemplate => "ElementosUI/ToolTip/ToolTipTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/ToolTip/ToolTipStyle.uss";

        #region .: Elementos :.

        public Image ImagemTooltip { get => imagemTooltip; }

        private const string NOME_IMAGEM_TOOLTIP = "imagem-interrogacao";
        private Image imagemTooltip;

        #endregion

        public Tooltip() {
            ConfigurarImagemToolTip();
            return;
        }

        public Tooltip(string conteudo) {
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