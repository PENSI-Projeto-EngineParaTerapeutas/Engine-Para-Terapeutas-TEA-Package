using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.Utils;

namespace EngineParaTerapeutas.UI {

    public class InterrogacaoToolTip : ElementoInterfaceEditor {
        protected override string CaminhoTemplate => "ElementosUI/InterrogacaoToolTip/InterrogacaoToolTipTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InterrogacaoToolTip/InterrogacaoToolTipStyle.uss";

        #region .: Elementos :.

        public Image ImagemTooltip { get => imagemTooltip; }
        public VisualElement RegiaoTextoTooltip { get => regiaoTextoTooltip; }
        public Label TextoTooltip { get => textoTooltip; }

        private const string NOME_IMAGEM_TOOLTIP = "imagem-interrogacao";
        private readonly Image imagemTooltip;

        private const string NOME_REGIAO_TEXTO_TOOLTIP = "regiao-texto-tooltip";
        private readonly VisualElement regiaoTextoTooltip;

        private const string NOME_TEXTO_TOOLTIP = "texto-tooltip";
        private readonly Label textoTooltip;

        #endregion

        public InterrogacaoToolTip() {
            imagemTooltip = Root.Query<Image>(NOME_IMAGEM_TOOLTIP);
            regiaoTextoTooltip = Root.Query<VisualElement>(NOME_REGIAO_TEXTO_TOOLTIP);
            textoTooltip = Root.Query<Label>(NOME_TEXTO_TOOLTIP);

            ConfigurarImagemToolTip();
            OcultarTooltip();
            return;
        }

        private void ConfigurarImagemToolTip() {
            imagemTooltip.image = Importador.ImportarImagem("interrogacao.png");
            return;
        }

        public void SetTexto(string conteudo) {
            imagemTooltip.tooltip = conteudo;
            //TODO: TextoTooltip.text = conteudo;
            return;
        }

        public void ExibirTooltip() {
            regiaoTextoTooltip.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            return;
        }

        public void OcultarTooltip() {
            regiaoTextoTooltip.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            return;
        }
    }
}