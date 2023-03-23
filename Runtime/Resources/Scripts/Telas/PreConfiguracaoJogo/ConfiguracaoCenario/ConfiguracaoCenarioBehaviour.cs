using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.UI {
    public class ConfiguracaoCenarioBehaviour : ElementoInterfaceJogo {
        protected override string CaminhoTemplate => "Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoCenario/ConfiguracaoCenarioTemplate";
        protected override string CaminhoStyle => "Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoCenario/ConfiguracaoCenarioStyle";


        #region .: Elementos :.

        private const string NOME_REIGAO_CARREGAMENTO_MODIFICADOR_CENARIO = "regiao-carregamento-modificador-cenario";
        private readonly VisualElement regiaoCarregamentoModificadorCenario;

        private readonly ModificadorImagemDinamico modificadorImagemDinamico;

        #endregion

        private readonly GameObject cenario;
        private readonly SpriteRenderer spriteCenario;

        public ConfiguracaoCenarioBehaviour() {
            cenario = GameObject.FindGameObjectWithTag(NomesTags.Cenario); // TODO: Garantir que um cenário sempre exista
            spriteCenario = cenario.GetComponent<SpriteRenderer>();
            modificadorImagemDinamico = new ModificadorImagemDinamico(spriteCenario);

            regiaoCarregamentoModificadorCenario = Root.Query<VisualElement>(NOME_REIGAO_CARREGAMENTO_MODIFICADOR_CENARIO);
            regiaoCarregamentoModificadorCenario.Add(modificadorImagemDinamico.Root);
            
            return;
        }
    }
}