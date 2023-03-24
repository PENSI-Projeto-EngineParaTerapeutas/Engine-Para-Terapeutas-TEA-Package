using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.ComponentesGameObjects;
using EngineParaTerapeutas.DTOs;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.Criadores {
    public class CriadorReforcoBehaviour : Criador {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorReforco/CriadorReforcoTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorReforco/CriadorReforcoStyle.uss";

        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_HEADER = "regiao-carregamento-header";
        private VisualElement regiaoCarregamentoHeader;

        private const string NOME_LABEL_TIPO_REFORCO = "label-tipo-reforco";
        private const string NOME_INPUT_TIPO_REFORCO = "input-tipo-reforco";
        private EnumField campoTipoReforco;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_IMAGEM = "regiao-carregamento-inputs-imagem";
        private VisualElement regiaoCarregamentoInputsImagem;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_AUDIO = "regiao-carregamento-inputs-audio";
        private VisualElement regiaoCarregamentoInputsAudio;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_TEXTO = "regiao-carregamento-inputs-texto";
        private VisualElement regiaoCarregamentoInputsTexto;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_VIDEO = "regiao-carregamento-inputs-video";
        private VisualElement regiaoCarregamentoInputsVideo;

        private const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        private VisualElement regiaoCarregamentoBotoesConfirmacao;

        private readonly InputsComponenteImagem grupoInputsImagem;
        private readonly InputsComponenteAudio grupoInputsAudio;
        private readonly InputsComponenteTexto grupoInputsTexto;
        private readonly InputsComponenteVideo grupoInputsVideo;

        #endregion

        private SpriteRenderer sprite;
        private AudioSource audioSource;
        private Texto texto;
        private Video video;

        private readonly TiposReforcos tipoPadrao = TiposReforcos.Imagem;

        public CriadorReforcoBehaviour() {
            grupoInputsImagem = new InputsComponenteImagem();
            grupoInputsAudio = new InputsComponenteAudio();
            grupoInputsTexto = new InputsComponenteTexto();
            grupoInputsVideo = new InputsComponenteVideo();

            ImportarPrefab("Reforcos/Reforco.prefab");

            CarregarRegiaoHeader();
            CarregarRegiaoInputsImagem();
            CarregarRegiaoInputsAudio();
            CarregarRegiaoInputsTexto();
            CarregarRegiaoInputsVideo();

            ConfigurarCampoTipoReforco();
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
            regiaoCarregamentoInputsImagem.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        private void CarregarRegiaoInputsAudio() {
            regiaoCarregamentoInputsAudio = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_AUDIO);
            regiaoCarregamentoInputsAudio.Add(grupoInputsAudio.Root);
            regiaoCarregamentoInputsAudio.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        private void CarregarRegiaoInputsTexto() {
            regiaoCarregamentoInputsTexto = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_TEXTO);
            regiaoCarregamentoInputsTexto.Add(grupoInputsTexto.Root);
            regiaoCarregamentoInputsTexto.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        private void CarregarRegiaoInputsVideo() {
            regiaoCarregamentoInputsVideo = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_VIDEO);
            regiaoCarregamentoInputsVideo.Add(grupoInputsVideo.Root);
            regiaoCarregamentoInputsVideo.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        private void ConfigurarCampoTipoReforco() {
            campoTipoReforco = Root.Query<EnumField>(NOME_INPUT_TIPO_REFORCO);

            campoTipoReforco.labelElement.name = NOME_LABEL_TIPO_REFORCO;
            campoTipoReforco.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            campoTipoReforco.Init(TiposReforcos.Imagem);
            campoTipoReforco.SetValueWithoutNotify(TiposReforcos.Imagem);

            campoTipoReforco.RegisterCallback<ChangeEvent<Enum>>(evt => {
                TiposReforcos novoTipo = Enum.Parse<TiposReforcos>(campoTipoReforco.value.ToString());
                IdentificadorTipoReforco tipoNovoObjeto = novoObjeto.GetComponent<IdentificadorTipoReforco>();

                tipoNovoObjeto.AlterarTipo(novoTipo);
                AlterarVisibilidadeCamposComBaseTipo(novoTipo);
            });

            return;
        }

        private void AlterarVisibilidadeCamposComBaseTipo(TiposReforcos tipo) {
            regiaoCarregamentoInputsAudio.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsImagem.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsTexto.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsVideo.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            switch(tipo) {
                case(TiposReforcos.Audio): {
                    regiaoCarregamentoInputsAudio.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                    break;
                }
                case(TiposReforcos.Imagem): {
                    regiaoCarregamentoInputsImagem.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                    break;
                }
                case(TiposReforcos.Texto): {
                    regiaoCarregamentoInputsTexto.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                    break;
                }
                case(TiposReforcos.Video): {
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

            texto = novoObjeto.GetComponent<Texto>();
            grupoInputsTexto.VincularDados(texto);

            video = novoObjeto.GetComponent<Video>();
            grupoInputsVideo.VincularDados(video);

            IdentificadorTipoReforco tipoReforco = novoObjeto.GetComponent<IdentificadorTipoReforco>();
            tipoReforco.AlterarTipo(tipoPadrao);

            return;
        }

        public override void FinalizarCriacao() {
            novoObjeto.tag = NomesTags.Reforcos;
            novoObjeto.layer = LayersProjeto.Default.Index;
            sprite.sortingOrder = OrdemRenderizacao.Reforco;
            
            base.FinalizarCriacao();

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

            campoTipoReforco.SetValueWithoutNotify(tipoPadrao);
            AlterarVisibilidadeCamposComBaseTipo(tipoPadrao);

            return;
        }
    }
}