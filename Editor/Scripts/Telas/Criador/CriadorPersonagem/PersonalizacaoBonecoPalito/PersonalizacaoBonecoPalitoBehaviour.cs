using UnityEngine;
using UnityEngine.UIElements;
using Autis.Editor.Telas;
using Autis.Editor.UI;
using Autis.Editor.Manipuladores;
using Autis.Runtime.Constantes;
using UnityEditor;

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

        public override void OnEditorUpdate() {
            DefinirFerramenta();
            return;
        }

        private void DefinirFerramenta() {
            if(Selection.activeTransform == null || !Selection.activeTransform.CompareTag(NomesTags.EditorOnly)) {
                return;
            }

            if(Tools.current != Tool.Move) {
                Tools.current = Tool.Move;
            }

            return;
        }

        private void ConfigurarInputCor() {
            inputCor = new InputCor("Cor do boneco palito:");
            inputCor.CampoCor.RegisterCallback<ChangeEvent<Color>>(evt => {
                manipuladorBonecoPalito.SetCor(evt.newValue);
            });

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