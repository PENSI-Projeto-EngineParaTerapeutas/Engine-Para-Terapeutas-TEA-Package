using System.IO;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using EngineParaTerapeutas.UI;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.ScriptableObjects;
using EngineParaTerapeutas.Utils;

namespace EngineParaTerapeutas.Telas {
    public class InformacoesCenaBehaviour : Tela {
        protected override string CaminhoTemplate => "Telas/InformacoesCena/InformacoesCenaTemplate.uxml";
        protected override string CaminhoStyle => "Telas/InformacoesCena/InformacoesCenaStyle.uss";

        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_CENA = "regiao-carregamento-inputs-cena";
        private VisualElement regiaoCarregamentoInputsCena;

        private const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        private VisualElement regiaoCarregamentoBotoesConfirmacao;

        private readonly InputsScriptableObjectCena grupoInputsCena;
        private readonly BotoesConfirmacao botoesConfirmacao;

        #endregion

        private Cena cenaAtual;

        public InformacoesCenaBehaviour() {
            CarregarCenaAtual();
            grupoInputsCena = new InputsScriptableObjectCena();
            botoesConfirmacao = new BotoesConfirmacao();

            CarregarInputsCena();
            CarregarBotoesConfirmacao();

            return;
        }

        private void CarregarCenaAtual() {
            string nomeCenaAtual = SceneManager.GetActiveScene().name;
            cenaAtual = AssetDatabase.LoadAssetAtPath<Cena>(Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsScriptableObjectsCenas, nomeCenaAtual + Extensoes.ScriptableObject));
            
            return;
        }

        private void CarregarInputsCena() {
            regiaoCarregamentoInputsCena = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_CENA);
            regiaoCarregamentoInputsCena.Add(grupoInputsCena.Root);

            grupoInputsCena.VincularDados(cenaAtual);

            return;
        }

        private void CarregarBotoesConfirmacao() {
            regiaoCarregamentoBotoesConfirmacao = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO);
            regiaoCarregamentoBotoesConfirmacao.Add(botoesConfirmacao.Root);

            return;
        }
    }
}