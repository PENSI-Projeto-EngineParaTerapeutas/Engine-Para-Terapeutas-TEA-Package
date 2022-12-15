using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.ComponentesGameObjects;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(Posicao))]
    public class CustomEditorPosicaoBehaviour : CustomEditor<Posicao, Transform> {
        #region .: Elementos :.

        private const string REGIAO_CARREGAMENTO_INPUTS_PADROES_POSICAO = "regiao-carregamento-inputs-padroes-posicao";
        private VisualElement regiaoCarregamentosInputsPadroesPosicao;

        private InputsPosicao grupoInputsPosicao;

        #endregion

        public override void OnEnable() {
            base.OnEnable();
            grupoInputsPosicao = new InputsPosicao();

            ImportarTemplate("Packages/com.pensi.engine-para-terapeutas-tea/Editor/CustomEditor/CustomEditorPosicao/CustomEditorPosicaoTemplate.uxml");
            ImportarStyle("Packages/com.pensi.engine-para-terapeutas-tea/Editor/CustomEditor/CustomEditorPosicao/CustomEditorPosicaoStyle.uss");

            ConfigurarInputs();

            return;
        }

        protected override void ConfigurarInputs() {
            regiaoCarregamentosInputsPadroesPosicao = root.Query<VisualElement>(REGIAO_CARREGAMENTO_INPUTS_PADROES_POSICAO);
            regiaoCarregamentosInputsPadroesPosicao.Add(grupoInputsPosicao.Root);

            grupoInputsPosicao.VincularDados(componenteOriginal);
        
            return;
        }
    }
}