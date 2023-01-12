using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.UI;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.Criadores {
    public class CriadorCenarioBehaviour : Criador {
        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_HEADER = "regiao-carregamento-header";
        private VisualElement regiaoCarregamentoHeader;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_IMAGEM = "regiao-carregamento-inputs-imagem";
        private VisualElement regiaoCarregamentoInputsImagem;

        private readonly InputsComponenteImagem grupoInputsImagem;

        #endregion

        private SpriteRenderer spriteCenario;

        public CriadorCenarioBehaviour() {
            grupoInputsImagem = new InputsComponenteImagem();

            ImportarPrefab("Prefabs/Cenarios/Cenario.prefab");

            ImportarTemplate("Telas/Criador/CriadorCenario/CriadorCenarioTemplate.uxml");
            ImportarStyle("Telas/Criador/CriadorCenario/CriadorCenarioStyle.uss");

            CarregarRegiaoHeader();
            CarregarRegiaoInputsImagem();

            return;
        }

        private void CarregarRegiaoHeader() {
            regiaoCarregamentoHeader = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_HEADER);
            regiaoCarregamentoHeader.Add(header.Root);

            return;
        }

        private void CarregarRegiaoInputsImagem() {
            regiaoCarregamentoInputsImagem = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_IMAGEM);
            regiaoCarregamentoInputsImagem.Add(grupoInputsImagem.Root);

            return;
        }

        protected override void VincularCamposAoNovoObjeto() {
            spriteCenario = novoObjeto.GetComponent<SpriteRenderer>();
            spriteCenario.sortingOrder = OrdemRenderizacao.EmCriacao;
            grupoInputsImagem.VincularDados(spriteCenario);

            return;
        }

        protected override void ReiniciarPropriedadesNovoObjeto() {
            spriteCenario = null;

            return;
        }

        public override void ReiniciarCampos() {
            grupoInputsImagem.ReiniciarCampos();

            return;
        }

        public override void FinalizarCriacao() {
            novoObjeto.tag = NomesTags.Cenario;
            novoObjeto.layer = LayersProjeto.Default.Index;
            spriteCenario.sortingOrder = OrdemRenderizacao.Cenario;
            novoObjeto = null;

            ReiniciarPropriedadesNovoObjeto();

            header.ReiniciarCampos();
            ReiniciarCampos();

            return;
        }
    }
}