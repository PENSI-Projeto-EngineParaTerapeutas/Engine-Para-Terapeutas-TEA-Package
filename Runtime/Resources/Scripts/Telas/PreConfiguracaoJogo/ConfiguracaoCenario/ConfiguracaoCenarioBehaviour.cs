using UnityEngine;
using UnityEngine.UIElements;
using Autis.Runtime.Constantes;
using Autis.Runtime.ComponentesGameObjects;

namespace Autis.Runtime.UI {
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
        private readonly CenarioResize cenarioResize;

        public ConfiguracaoCenarioBehaviour() {
            cenario = GameObject.FindGameObjectWithTag(NomesTags.Cenario); // TODO: Garantir que um cenário sempre exista
            spriteCenario = cenario.GetComponent<SpriteRenderer>();
            cenarioResize = cenario.GetComponent<CenarioResize>();

            modificadorImagemDinamico = new ModificadorImagemDinamico(spriteCenario.sprite, HandleModificacaoSpriteCenario);

            regiaoCarregamentoModificadorCenario = Root.Query<VisualElement>(NOME_REIGAO_CARREGAMENTO_MODIFICADOR_CENARIO);
            regiaoCarregamentoModificadorCenario.Add(modificadorImagemDinamico.Root);
            
            return;
        }

        private void HandleModificacaoSpriteCenario(Texture2D novaImagem) {
            spriteCenario.sprite = Sprite.Create(novaImagem, new Rect(0.0f, 0.0f, novaImagem.width, novaImagem.height), new Vector2(0.5f, 0.5f));
            cenarioResize.Resize();

            return;
        }
    }
}