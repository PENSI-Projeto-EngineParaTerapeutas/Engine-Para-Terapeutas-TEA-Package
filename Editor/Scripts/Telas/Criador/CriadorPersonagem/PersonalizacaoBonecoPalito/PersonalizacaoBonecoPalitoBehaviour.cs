using UnityEngine;
using UnityEngine.UIElements;
using Autis.Editor.Telas;
using Autis.Editor.UI;
using Autis.Editor.Manipuladores;

namespace Autis.Editor.Criadores {
    public class PersonalizacaoBonecoPalitoBehaviour : Tela, IReiniciavel {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorPersonagem/PersonalizacaoBonecoPalito/PersonalizacaoBonecoPalitoTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorPersonagem/PersonalizacaoBonecoPalito/PersonalizacaoBonecoPalitoStyle.uss";

        #region .: Elementos :.

        public InputCor InputCor { get => inputCor; }
        private InputCor inputCor;

        protected const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        protected VisualElement regiaoCarregamentoBotoesConfirmacao;

        public VisualElement RegiaoInputCor { get => regiaoInputCor; }
        private const string NOME_REGIAO_INPUT_COR = "regiao-input-cor";
        private VisualElement regiaoInputCor;

        protected BotoesConfirmacao botoesConfirmacao;

        #endregion

        private readonly ManipuladorBonecoPalito manipuladorBonecoPalito;
        private readonly Color corInicial;

        public PersonalizacaoBonecoPalitoBehaviour(ManipuladorBonecoPalito manipuladorBonecoPalito) {
            this.manipuladorBonecoPalito = manipuladorBonecoPalito;
            corInicial = manipuladorBonecoPalito.Cor;

            ConfigurarInputCor();
            ConfigurarBotoesConfirmacao();

            return;
        }

        private void ConfigurarInputCor() {
            inputCor = new InputCor("Cor do boneco palito:");
            inputCor.CampoCor.tooltip = "Cor que sobrepõe a imagem do ator";

            regiaoInputCor = Root.Query<VisualElement>(NOME_REGIAO_INPUT_COR);
            regiaoInputCor.Add(inputCor.Root);

            return;
        }

        private void ConfigurarBotoesConfirmacao() {
            botoesConfirmacao = new();
            botoesConfirmacao.BotaoConfirmar.clicked += HandleBotaoConfirmarClick;
            botoesConfirmacao.BotaoCancelar.clicked += HandleBotaoCancelarClick;

            regiaoCarregamentoBotoesConfirmacao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO);
            regiaoCarregamentoBotoesConfirmacao.Add(botoesConfirmacao.Root);

            return;
        }

        private void HandleBotaoConfirmarClick() {
            Navigator.Instance.Voltar();
            return;
        }

        private void HandleBotaoCancelarClick() {
            manipuladorBonecoPalito.SetCor(corInicial);

            ReiniciarCampos();
            Navigator.Instance.Voltar();
            
            return;
        }

        public void ReiniciarCampos() {
            inputCor.CampoCor.SetValueWithoutNotify(Color.white);
            return;
        }
    }
}