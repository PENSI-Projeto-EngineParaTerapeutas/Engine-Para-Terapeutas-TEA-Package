using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EngineParaTerapeutas.ComponentesGameObjects;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.DTOs;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.Criadores {
    public class CriadorInstrucoesBehaviour : Criador {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorInstrucoes/CriadorInstrucoesTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorInstrucoes/CriadorInstrucoesStyle.uss";

        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_HEADER = "regiao-carregamento-header";
        private VisualElement regiaoCarregamentoHeader;

        private const string NOME_LABEL_TIPO_INSTRUCAO = "label-tipo-instrucao";
        private const string NOME_INPUT_TIPO_INSTRUCAO = "input-tipo-instrucao";
        private EnumField campoTipoInstrucao;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_VIDEO = "regiao-carregamento-inputs-video";
        private VisualElement regiaoCarregamentoInputsVideo;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_AUDIO = "regiao-carregamento-inputs-audio";
        private VisualElement regiaoCarregamentoInputsAudio;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_TEXTO = "regiao-carregamento-inputs-texto";
        private VisualElement regiaoCarregamentoInputsTexto;

        private const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        private VisualElement regiaoCarregamentoBotoesConfirmacao;

        private readonly InputsComponenteVideo grupoInputsVideo;
        private readonly InputsComponenteAudio grupoInputsAudio;
        private readonly InputsComponenteTexto grupoInputsTexto;

        #endregion

        private Video video;
        private AudioSource audio;
        private Texto texto;
        private SpriteRenderer spriteRenderer;

        private readonly TiposIntrucoes tipoPadrao = TiposIntrucoes.Texto;

        public CriadorInstrucoesBehaviour() {
            grupoInputsVideo = new InputsComponenteVideo();
            grupoInputsAudio = new InputsComponenteAudio();
            grupoInputsTexto = new InputsComponenteTexto();

            ImportarPrefab("Instrucao/Instrucao.prefab");

            CarregarRegiaoHeader();
            CarregarRegiaoInputsVideo();
            CarregarRegiaoInputsAudio();
            CarregarRegiaoInputsTexto();

            ConfigurarCampoTipoInstrucao();
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

        private void ConfigurarCampoTipoInstrucao() {
            campoTipoInstrucao = Root.Query<EnumField>(NOME_INPUT_TIPO_INSTRUCAO);

            campoTipoInstrucao.labelElement.name = NOME_LABEL_TIPO_INSTRUCAO;
            campoTipoInstrucao.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            campoTipoInstrucao.Init(TiposIntrucoes.Texto);
            campoTipoInstrucao.SetValueWithoutNotify(TiposIntrucoes.Texto);

            campoTipoInstrucao.RegisterCallback<ChangeEvent<Enum>>(evt => {
                TiposIntrucoes novoTipo = Enum.Parse<TiposIntrucoes>(campoTipoInstrucao.value.ToString());
                IdentificadorTipoInstrucao tipoNovoObjeto = novoObjeto.GetComponent<IdentificadorTipoInstrucao>();

                tipoNovoObjeto.AlterarTipo(novoTipo);
                AlterarVisibilidadeCamposComBaseTipo(novoTipo);
            });

            return;
        }

        private void AlterarVisibilidadeCamposComBaseTipo(TiposIntrucoes tipo) {
            regiaoCarregamentoInputsAudio.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsTexto.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsVideo.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            
            switch(tipo) {
                case(TiposIntrucoes.Audio): {
                    regiaoCarregamentoInputsAudio.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                    break;
                }
                case(TiposIntrucoes.Texto): {
                    regiaoCarregamentoInputsTexto.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                    break;
                }
                case(TiposIntrucoes.Video): {
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
            video = novoObjeto.GetComponent<Video>();
            grupoInputsVideo.VincularDados(video);

            audio = novoObjeto.GetComponent<AudioSource>();
            grupoInputsAudio.VincularDados(audio);

            texto = novoObjeto.GetComponent<Texto>();
            grupoInputsTexto.VincularDados(texto);

            spriteRenderer = novoObjeto.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = OrdemRenderizacao.EmCriacao;

            IdentificadorTipoInstrucao tipoInstrucao = novoObjeto.GetComponent<IdentificadorTipoInstrucao>();
            tipoInstrucao.AlterarTipo(tipoPadrao);

            return;
        }

        public override void FinalizarCriacao() {
            novoObjeto.tag = NomesTags.Instrucoes;
            novoObjeto.layer = LayersProjeto.Default.Index;
            spriteRenderer.sortingOrder = OrdemRenderizacao.Instrucao;
            
            base.FinalizarCriacao();

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

            campoTipoInstrucao.SetValueWithoutNotify(tipoPadrao);
            AlterarVisibilidadeCamposComBaseTipo(tipoPadrao);

            return;
        }
    }
}