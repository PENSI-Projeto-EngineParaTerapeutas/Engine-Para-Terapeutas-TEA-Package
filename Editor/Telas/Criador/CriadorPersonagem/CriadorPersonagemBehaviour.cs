using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.UI;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.Criadores {
    public class CriadorPersonagemBehaviour : Criador { 
        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_HEADER = "regiao-carregamento-header";
        private VisualElement regiaoCarregamentoHeader;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_IMAGEM = "regiao-carregamento-inputs-imagem";
        private VisualElement regiaoCarregamentoInputsImagem;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_FISICA = "regiao-carregamento-inputs-fisica";
        private VisualElement regiaoCarregamentoInputsFisica;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_COLISOR = "regiao-carregamento-inputs-colisor";
        private VisualElement regiaoCarregamentoInputsColisor;

        private readonly InputsImagem grupoInputsImagem;
        private readonly InputsFisica grupoInputsFisica;
        private readonly InputsColisor grupoInputsColisor;

        #endregion

        private SpriteRenderer sprite;
        private Rigidbody2D rigidbody2D;
        private BoxCollider2D colisor;

        public CriadorPersonagemBehaviour() {
            grupoInputsImagem = new InputsImagem();
            grupoInputsFisica = new InputsFisica();
            grupoInputsColisor = new InputsColisor();

            ImportarPrefab("Prefabs/Personagens/Personagem.prefab");

            ImportarTemplate("Telas/Criador/CriadorPersonagem/CriadorPersonagemTemplate.uxml");
            ImportarStyle("Telas/Criador/CriadorPersonagem/CriadorPersonagemStyle.uss");

            CarregarRegiaoHeader();
            CarregarRegiaoInputsImagem();
            CarregarRegiaoInputsFisica();
            CarregarRegiaoInputsColisor();

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

        private void CarregarRegiaoInputsFisica() {
            regiaoCarregamentoInputsFisica = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_FISICA);
            regiaoCarregamentoInputsFisica.Add(grupoInputsFisica.Root);

            return;
        }

        private void CarregarRegiaoInputsColisor() {
            regiaoCarregamentoInputsColisor = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_COLISOR);
            regiaoCarregamentoInputsColisor.Add(grupoInputsColisor.Root);

            return;
        }

        protected override void VincularCamposAoNovoObjeto() {
            sprite = novoObjeto.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = OrdemRenderizacao.EmCriacao;
            grupoInputsImagem.VincularDados(sprite);

            rigidbody2D = novoObjeto.GetComponent<Rigidbody2D>();
            grupoInputsFisica.VincularDados(rigidbody2D);

            colisor = novoObjeto.GetComponent<BoxCollider2D>();
            grupoInputsColisor.VincularDados(colisor);

            return;
        }

        protected override void ReiniciarPropriedadesNovoObjeto() {
            sprite = null;
            rigidbody2D = null;
            colisor = null;

            return;
        }

        public override void ReiniciarCampos() {
            grupoInputsImagem.ReiniciarCampos();
            grupoInputsFisica.ReiniciarCampos();
            grupoInputsColisor.ReiniciarCampos();

            return;
        }

        public override void FinalizarCriacao() {
            novoObjeto.tag = NomesTags.Untagged;
            novoObjeto.layer = LayersProjeto.Default.Index;
            sprite.sortingOrder = OrdemRenderizacao.Personagem;
            novoObjeto = null;

            ReiniciarPropriedadesNovoObjeto();

            header.ReiniciarCampos();
            ReiniciarCampos();

            return;
        }
    }
}