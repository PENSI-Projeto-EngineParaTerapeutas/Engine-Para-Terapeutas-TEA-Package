using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EngineParaTerapeutas.UI;
using EngineParaTerapeutas.Telas;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.Criadores {
    public class PersonalizacaoBonecoPalitoBehaviour : Tela, IReiniciavel {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorPersonagem/PersonalizacaoBonecoPalito/PersonalizacaoBonecoPalitoTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorPersonagem/PersonalizacaoBonecoPalito/PersonalizacaoBonecoPalitoStyle.uss";

        #region .: Elementos :.

        private const string NOME_LABEL_COR = "label-cor";
        private const string NOME_INPUT_COR = "input-cor";
        private readonly ColorField inputCor;

        private const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        private readonly VisualElement regiaoBotoesConfirmacao;

        private readonly BotoesConfirmacao botoesConfirmacao;

        #endregion

        private readonly GameObject personagemAtual;
        private readonly SpriteRenderer[] spriteRenderers;

        private readonly Color corInicial;

        public PersonalizacaoBonecoPalitoBehaviour(GameObject personagemAtual) {
            this.personagemAtual = personagemAtual;
            spriteRenderers = this.personagemAtual.GetComponentsInChildren<SpriteRenderer>();
            corInicial = spriteRenderers.First().color;

            botoesConfirmacao = new BotoesConfirmacao();

            inputCor = Root.Query<ColorField>(NOME_INPUT_COR);
            regiaoBotoesConfirmacao = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO);
            regiaoBotoesConfirmacao.Add(botoesConfirmacao.Root);

            ConfigurarInputCor();
            ConfigurarBotoesConfirmacao();

            return;
        }

        private void ConfigurarInputCor() {
            inputCor.labelElement.name = NOME_LABEL_COR;
            inputCor.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            inputCor.SetValueWithoutNotify(corInicial);

            inputCor.RegisterCallback<ChangeEvent<Color>>(evt => {
                foreach(SpriteRenderer spriteRenderer in spriteRenderers) {
                    spriteRenderer.color = inputCor.value;
                }
            });

            return;
        }

        private void ConfigurarBotoesConfirmacao() {
            botoesConfirmacao.BotaoCancelar.clicked += HandleBotaoCancelarClick;
            return;
        }

        private void HandleBotaoCancelarClick() {
            foreach(SpriteRenderer spriteRenderer in spriteRenderers) {
                spriteRenderer.color = corInicial;
            }

            ReiniciarCampos();
            return;
        }

        public void ReiniciarCampos() {
            inputCor.SetValueWithoutNotify(Color.white);
            return;
        }
    }
}