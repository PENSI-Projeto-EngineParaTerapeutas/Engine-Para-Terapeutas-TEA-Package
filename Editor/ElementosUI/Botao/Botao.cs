using UnityEngine.UIElements;

namespace EngineParaTerapeutas.UI {
    public class Botao : ElementoInterfaceEditor {
        protected override string CaminhoTemplate => "ElementosUI/Botao/BotaoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/Botao/BotaoStyle.uss";

        #region .: Elementos :.

        public Button ComponenteOriginal { get => botaoOriginal; }

        private const string NOME_BOTAO_ORIGINAL = "botao-original";
        private readonly Button botaoOriginal;

        private const string CLASSE_HABILITADO = "botao-habilitado";
        private const string CLASSE_DESABILITADO = "botao-desabilitado";

        #endregion

        public bool Habilitado { get => habilitado; }

        private bool habilitado = true;

        public Botao(string conteudo) {
            botaoOriginal = Root.Query<Button>(NOME_BOTAO_ORIGINAL);
            botaoOriginal.text = conteudo;
            botaoOriginal.AddToClassList(CLASSE_HABILITADO);

            return;
        }

        public void Habilitar() {
            habilitado = true;

            botaoOriginal.RemoveFromClassList(CLASSE_DESABILITADO);
            botaoOriginal.AddToClassList(CLASSE_HABILITADO);

            return;
        }

        public void Desabilitar() {
            habilitado = false;

            botaoOriginal.RemoveFromClassList(CLASSE_HABILITADO);
            botaoOriginal.AddToClassList(CLASSE_DESABILITADO);

            return;
        }
    }
}