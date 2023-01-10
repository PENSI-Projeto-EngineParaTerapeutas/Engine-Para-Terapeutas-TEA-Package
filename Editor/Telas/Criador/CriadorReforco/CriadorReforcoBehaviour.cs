using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.Criadores {
    public class CriadorReforcoBehaviour : Criador {
        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_HEADER = "regiao-carregamento-header";
        private VisualElement regiaoCarregamentoHeader;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_IMAGEM = "regiao-carregamento-inputs-imagem";
        private VisualElement regiaoCarregamentoInputsImagem;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_AUDIO = "regiao-carregamento-inputs-audio";
        private VisualElement regiaoCarregamentoInputsAudio;

        private readonly InputsComponenteImagem grupoInputsImagem;
        private readonly InputsComponenteAudio grupoInputsAudio;

        #endregion

        private SpriteRenderer sprite;
        private AudioSource audioSource;

        public CriadorReforcoBehaviour() {
            grupoInputsImagem = new InputsComponenteImagem();
            grupoInputsAudio = new InputsComponenteAudio();

            ImportarPrefab("Prefabs/Reforcos/Reforco.prefab");

            ImportarTemplate("Telas/Criador/CriadorReforco/CriadorReforcoTemplate.uxml");
            ImportarStyle("Telas/Criador/CriadorReforco/CriadorReforcoStyle.uss");

            CarregarRegiaoHeader();
            CarregarRegiaoInputsImagem();
            CarregarRegiaoInputsAudio();

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

        private void CarregarRegiaoInputsAudio() {
            regiaoCarregamentoInputsAudio = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_AUDIO);
            regiaoCarregamentoInputsAudio.Add(grupoInputsAudio.Root);

            return;
        }

        protected override void VincularCamposAoNovoObjeto() {
            sprite = novoObjeto.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = OrdemRenderizacao.EmCriacao;
            grupoInputsImagem.VincularDados(sprite);

            audioSource = novoObjeto.GetComponent<AudioSource>();
            grupoInputsAudio.VincularDados(audioSource);
            return;
        }

        protected override void ReiniciarPropriedadesNovoObjeto() {
            sprite = null;
            audioSource = null;

            return;
        }

        public override void ReiniciarCampos() {
            grupoInputsImagem.ReiniciarCampos();
            grupoInputsAudio.ReiniciarCampos();

            return;
        }

        public override void FinalizarCriacao() {
            novoObjeto.tag = NomesTags.Reforcos;
            novoObjeto.layer = LayersProjeto.Default.Index;
            sprite.sortingOrder = OrdemRenderizacao.Reforco;
            novoObjeto = null;

            ReiniciarPropriedadesNovoObjeto();

            header.ReiniciarCampos();
            ReiniciarCampos();

            return;
        }
    }
}