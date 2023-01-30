using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.UI;
using EngineParaTerapeutas.ComponentesGameObjects;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.Criadores {
    public class CriadorInstrucoesBehaviour : Criador {
        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_HEADER = "regiao-carregamento-header";
        private VisualElement regiaoCarregamentoHeader;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_VIDEO = "regiao-carregamento-inputs-video";
        private VisualElement regiaoCarregamentoInputsVideo;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_AUDIO = "regiao-carregamento-inputs-audio";
        private VisualElement regiaoCarregamentoInputsAudio;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_TEXTO = "regiao-carregamento-inputs-texto";
        private VisualElement regiaoCarregamentoInputsTexto;

        private readonly InputsComponenteVideo grupoInputsVideo;
        private readonly InputsComponenteAudio grupoInputsAudio;
        private readonly InputsComponenteTexto grupoInputsTexto;

        #endregion

        private Video video;
        private AudioSource audio;
        private Texto texto;
        private SpriteRenderer spriteRenderer;

        public CriadorInstrucoesBehaviour() {
            grupoInputsVideo = new InputsComponenteVideo();
            grupoInputsAudio = new InputsComponenteAudio();
            grupoInputsTexto = new InputsComponenteTexto();

            ImportarPrefab("Prefabs/Instrucao/Instrucao.prefab");

            ImportarTemplate("Telas/Criador/CriadorInstrucoes/CriadorInstrucoesTemplate.uxml");
            ImportarStyle("Telas/Criador/CriadorInstrucoes/CriadorInstrucoesStyle.uss");

            CarregarRegiaoHeader();
            CarregarRegiaoInputsVideo();
            CarregarRegiaoInputsAudio();
            CarregarRegiaoInputsTexto();

            return;
        }

        private void CarregarRegiaoHeader() {
            regiaoCarregamentoHeader = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_HEADER);
            regiaoCarregamentoHeader.Add(header.Root);

            return;
        }

        private void CarregarRegiaoInputsVideo() {
            regiaoCarregamentoInputsVideo = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_VIDEO);
            regiaoCarregamentoInputsVideo.Add(grupoInputsVideo.Root);

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

        protected override void VincularCamposAoNovoObjeto() {
            video = novoObjeto.GetComponent<Video>();
            grupoInputsVideo.VincularDados(video);

            audio = novoObjeto.GetComponent<AudioSource>();
            grupoInputsAudio.VincularDados(audio);

            texto = novoObjeto.GetComponent<Texto>();
            grupoInputsTexto.VincularDados(texto);

            spriteRenderer = novoObjeto.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = OrdemRenderizacao.EmCriacao;

            return;
        }

        protected override void ReiniciarPropriedadesNovoObjeto() {
            video = null;
            audio = null;
            texto = null;
            spriteRenderer = null;

            return;
        }

        public override void ReiniciarCampos() {
            grupoInputsVideo.ReiniciarCampos();
            grupoInputsAudio.ReiniciarCampos();
            grupoInputsTexto.ReiniciarCampos();

            return;
        }

        public override void FinalizarCriacao() {
            // TODO: novoObjeto.tag = NomesTags;
            novoObjeto.layer = LayersProjeto.Default.Index;
            spriteRenderer.sortingOrder = OrdemRenderizacao.Instrucao;
            novoObjeto = null;

            ReiniciarPropriedadesNovoObjeto();

            header.ReiniciarCampos();
            ReiniciarCampos();

            return;
        }
    }
}