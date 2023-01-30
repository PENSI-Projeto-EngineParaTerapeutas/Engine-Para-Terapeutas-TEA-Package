using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using EngineParaTerapeutas.UI;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.ScriptableObjects;
using EngineParaTerapeutas.Utils;

namespace EngineParaTerapeutas.Telas {
    public class InformacoesCenaBehaviour : TelaEditor {
        private const string TITULO = "Informações da Cena";

        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_CENA = "regiao-carregamento-inputs-cena";
        private VisualElement regiaoCarregamentoInputsCena;

        private InputsScriptableObjectCena grupoInputsCena;

        #endregion

        private Cena cenaAtual;

        [MenuItem("Engine Para Terapeutas/Tela Informações da Cena")]
        public static void ShowInformacoesCena() {
            InformacoesCenaBehaviour janela = GetWindow<InformacoesCenaBehaviour>();
            janela.titleContent = new GUIContent(TITULO);

            return;
        }

        public override void CreateGUI() {
            base.CreateGUI();

            CarregarCenaAtual();
            grupoInputsCena = new InputsScriptableObjectCena();

            ImportarTemplate("Telas/InformacoesCena/InformacoesCenaTemplate.uxml");
            ImportarStyle("Telas/InformacoesCena/InformacoesCenaStyle.uss");

            CarregarInputsCena();

            return;
        }

        private void CarregarCenaAtual() {
            string nomeCenaAtual = SceneManager.GetActiveScene().name;
            cenaAtual = AssetDatabase.LoadAssetAtPath<Cena>(Path.Combine(ConstantesRuntime.CaminhoPastaScriptableObjectsCenas, nomeCenaAtual + Extensoes.ScriptableObject));
            
            return;
        }

        private void CarregarInputsCena() {
            regiaoCarregamentoInputsCena = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_CENA);
            regiaoCarregamentoInputsCena.Add(grupoInputsCena.Root);

            grupoInputsCena.VincularDados(cenaAtual);

            return;
        }
    }
}