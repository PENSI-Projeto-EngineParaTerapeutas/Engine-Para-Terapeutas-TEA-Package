using UnityEngine;
using UnityEngine.UIElements;
using Autis.Editor.Utils;
using Autis.Editor.Constantes;

namespace Autis.Editor.UI {

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

        private bool labelAtivada = false;

        #endregion

        public InterrogacaoToolTip() {
            imagemTooltip = Root.Query<Image>(NOME_IMAGEM_TOOLTIP);
            regiaoTextoTooltip = Root.Query<VisualElement>(NOME_REGIAO_TEXTO_TOOLTIP);
            textoTooltip = Root.Query<Label>(NOME_TEXTO_TOOLTIP);

            regiaoTextoTooltip.AddToClassList("regiao-tooltip");

            ConfigurarEventosMouse();
            ConfigurarImagemToolTip();
            OcultarTooltip();

            root.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        private void ConfigurarEventosMouse() {
            imagemTooltip.RegisterCallback<ClickEvent>(EventoClicar);

            SetTexto("Teste da tooltip"); // DEBUG - DELETAR DEPOIS
        }
        private void ConfigurarImagemToolTip() {
            imagemTooltip.image = Importador.ImportarImagem("interrogacao.png");
            return;
        }

        public void SetTexto(string conteudo) {
            imagemTooltip.tooltip = conteudo;
            return;
        }

        void EventoClicar(ClickEvent evt) {
            return;

            /*if(labelAtivada) {
                OcultarTooltip();
                labelAtivada = false;
            }
            else {
                ExibirTooltip();
                labelAtivada = true;
            }*/
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