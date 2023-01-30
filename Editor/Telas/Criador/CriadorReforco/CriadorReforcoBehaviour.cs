using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.UI;
using EngineParaTerapeutas.ComponentesGameObjects;

namespace EngineParaTerapeutas.Criadores {
    public class CriadorReforcoBehaviour : Criador {
        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_HEADER = "regiao-carregamento-header";
        private VisualElement regiaoCarregamentoHeader;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_IMAGEM = "regiao-carregamento-inputs-imagem";
        private VisualElement regiaoCarregamentoInputsImagem;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_AUDIO = "regiao-carregamento-inputs-audio";
        private VisualElement regiaoCarregamentoInputsAudio;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_TEXTO = "regiao-carregamento-inputs-texto";
        private VisualElement regiaoCarregamentoInputsTexto;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_VIDEO = "regiao-carregamento-inputs-video";
        private VisualElement regiaoCarregamentoInputsVideo;

        private readonly InputsComponenteImagem grupoInputsImagem;
        private readonly InputsComponenteAudio grupoInputsAudio;
        private readonly InputsComponenteTexto grupoInputsTexto;
        private readonly InputsComponenteVideo grupoInputsVideo;

        #endregion

        private SpriteRenderer sprite;
        private AudioSource audioSource;
        private Texto texto;
        private Video video;

        public CriadorReforcoBehaviour() {
            grupoInputsImagem = new InputsComponenteImagem();
            grupoInputsAudio = new InputsComponenteAudio();
            grupoInputsTexto = new InputsComponenteTexto();
            grupoInputsVideo = new InputsComponenteVideo();

            ImportarPrefab("Prefabs/Reforcos/Reforco.prefab");

            ImportarTemplate("Telas/Criador/CriadorReforco/CriadorReforcoTemplate.uxml");
            ImportarStyle("Telas/Criador/CriadorReforco/CriadorReforcoStyle.uss");

            CarregarRegiaoHeader();
            CarregarRegiaoInputsImagem();
            CarregarRegiaoInputsAudio();
            CarregarRegiaoInputsTexto();
            CarregarRegiaoInputsVideo();

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

        private void CarregarRegiaoInputsTexto() {
            regiaoCarregamentoInputsTexto = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_TEXTO);
            regiaoCarregamentoInputsTexto.Add(grupoInputsTexto.Root);

            return;
        }

        private void CarregarRegiaoInputsVideo() {
            regiaoCarregamentoInputsVideo = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_VIDEO);
            regiaoCarregamentoInputsVideo.Add(grupoInputsVideo.Root);

            return;
        }

        protected override void VincularCamposAoNovoObjeto() {
            sprite = novoObjeto.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = OrdemRenderizacao.EmCriacao;
            grupoInputsImagem.VincularDados(sprite);

            audioSource = novoObjeto.GetComponent<AudioSource>();
            grupoInputsAudio.VincularDados(audioSource);

            texto = novoObjeto.GetComponent<Texto>();
            grupoInputsTexto.VincularDados(texto);

            video = novoObjeto.GetComponent<Video>();
            grupoInputsVideo.VincularDados(video);
            return;
        }

        protected override void ReiniciarPropriedadesNovoObjeto() {
            sprite = null;
            audioSource = null;
            texto = null;
            video = null;

            return;
        }

        public override void ReiniciarCampos() {
            grupoInputsImagem.ReiniciarCampos();
            grupoInputsAudio.ReiniciarCampos();
            grupoInputsTexto.ReiniciarCampos();
            grupoInputsVideo.ReiniciarCampos();

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