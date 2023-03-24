using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.ComponentesGameObjects;
using EngineParaTerapeutas.DTOs;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.Criadores {
    public class CriadorApoioBehaviour : Criador {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorApoio/CriadorApoioTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorApoio/CriadorApoioStyle.uss";

        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_HEADER = "regiao-carregamento-header";
        private VisualElement regiaoCarregamentoHeader;

        private const string NOME_LABEL_TIPO_APOIO = "label-tipo-apoio";
        private const string NOME_INPUT_TIPO_APOIO = "input-tipo-apoio";
        private EnumField campoTipoApoio;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_IMAGEM = "regiao-carregamento-inputs-imagem";
        private VisualElement regiaoCarregamentoInputsImagem;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_AUDIO = "regiao-carregamento-inputs-audio";
        private VisualElement regiaoCarregamentoInputsAudio;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_VIDEO = "regiao-carregamento-inputs-video";
        private VisualElement regiaoCarregamentoInputsVideo;

        private const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        private VisualElement regiaoCarregamentoBotoesConfirmacao;

        private readonly InputsComponenteImagem grupoInputsImagem;
        private readonly InputsComponenteAudio grupoInputsAudio;
        private readonly InputsComponenteVideo grupoInputsVideo;

        #endregion

        private SpriteRenderer sprite;
        private AudioSource audioSource;
        private Video video;

        private readonly TiposApoios tipoPadrao = TiposApoios.Imagem;

        public CriadorApoioBehaviour() {
            grupoInputsImagem = new InputsComponenteImagem();
            grupoInputsAudio = new InputsComponenteAudio();
            grupoInputsVideo = new InputsComponenteVideo();

            ImportarPrefab("Apoios/Apoio.prefab");

            CarregarRegiaoHeader();
            CarregarRegiaoInputsImagem();
            CarregarRegiaoInputsAudio();
            CarregarRegiaoInputsVideo();

            ConfigurarCampoTipoApoio();
            AlterarVisibilidadeCamposComBaseTipo(tipoPadrao);

            ConfigurarBotoesConfirmacao();

            IniciarCriacao();

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

        private void CarregarRegiaoInputsVideo() {
            regiaoCarregamentoInputsVideo = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_VIDEO);
            regiaoCarregamentoInputsVideo.Add(grupoInputsVideo.Root);

            return;
        }

        private void ConfigurarCampoTipoApoio() {
            campoTipoApoio = Root.Query<EnumField>(NOME_INPUT_TIPO_APOIO);

            campoTipoApoio.labelElement.name = NOME_LABEL_TIPO_APOIO;
            campoTipoApoio.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            campoTipoApoio.Init(tipoPadrao);
            campoTipoApoio.SetValueWithoutNotify(tipoPadrao);

            campoTipoApoio.RegisterCallback<ChangeEvent<Enum>>(evt => {
                TiposApoios novoTipo = Enum.Parse<TiposApoios>(campoTipoApoio.value.ToString());
                IdentificadorTipoApoio tipoNovoObjeto = novoObjeto.GetComponent<IdentificadorTipoApoio>();

                tipoNovoObjeto.AlterarTipo(novoTipo);
                AlterarVisibilidadeCamposComBaseTipo(novoTipo);
            });

            return;
        }

        private void AlterarVisibilidadeCamposComBaseTipo(TiposApoios tipo) {
            regiaoCarregamentoInputsAudio.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsImagem.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsVideo.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            switch(tipo) {
                case(TiposApoios.Audio): {
                    regiaoCarregamentoInputsAudio.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                    break;
                }
                case(TiposApoios.Imagem): {
                    regiaoCarregamentoInputsImagem.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                    break;
                }
                case(TiposApoios.Video): {
                    regiaoCarregamentoInputsVideo.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                    break;
                }
            }

            return;
        }

        private void ConfigurarBotoesConfirmacao() {
            regiaoCarregamentoBotoesConfirmacao = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO);
            regiaoCarregamentoBotoesConfirmacao.Add(botoesConfirmacao.Root);

            botoesConfirmacao.BotaoConfirmar.clicked += HandleBotaoConfirmarClick;
            botoesConfirmacao.BotaoCancelar.clicked += HandleBotaoCancelarClick;

            return;
        }

        protected override void VincularCamposAoNovoObjeto() {
            sprite = novoObjeto.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = OrdemRenderizacao.EmCriacao;
            grupoInputsImagem.VincularDados(sprite);

            audioSource = novoObjeto.GetComponent<AudioSource>();
            grupoInputsAudio.VincularDados(audioSource);

            video = novoObjeto.GetComponent<Video>();
            grupoInputsVideo.VincularDados(video);

            IdentificadorTipoApoio tipo = novoObjeto.GetComponent<IdentificadorTipoApoio>();
            tipo.AlterarTipo(tipoPadrao);

            return;
        }

        public override void FinalizarCriacao() {
            novoObjeto.tag = NomesTags.Apoios;
            novoObjeto.layer = LayersProjeto.Default.Index;
            sprite.sortingOrder = OrdemRenderizacao.Apoio;
            
            base.FinalizarCriacao();

            return;
        }

        protected override void ReiniciarPropriedadesNovoObjeto() {
            sprite = null;
            audioSource = null;
            video = null;

            return;
        }

        public override void ReiniciarCampos() {
            grupoInputsImagem.ReiniciarCampos();
            grupoInputsAudio.ReiniciarCampos();
            grupoInputsVideo.ReiniciarCampos();

            campoTipoApoio.SetValueWithoutNotify(tipoPadrao);
            AlterarVisibilidadeCamposComBaseTipo(tipoPadrao);

            return;
        }
    }
}