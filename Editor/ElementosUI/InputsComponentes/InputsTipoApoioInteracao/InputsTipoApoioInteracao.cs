using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Runtime.DTOs;
using Autis.Editor.Constantes;

namespace Autis.Editor.UI {
    public class InputsTipoApoioInteracao : ElementoInterfaceEditor, IVinculavel<IdentificadorTipoApoioObjetoInteracao>, IReiniciavel {
        protected override string CaminhoTemplate => "ElementosUI/InputsComponentes/InputsTipoApoioInteracao/InputsTipoApoioInteracaoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputsComponentes/InputsTipoApoioInteracao/InputsTipoApoioInteracaoStyle.uss";

        #region .: Elementos :.

        private const string NOME_LABEL_TIPO_ACAO_APOIO_OBJETO_INTERACAO = "label-tipo-acao-apoio-objeto-interacao";
        private const string NOME_INPUT_TIPO_ACAO_APOIO_OBJETO_INTERACAO = "input-tipo-acao-apoio-objeto-interacao";
        private readonly EnumField campoTipoApoioObjetoInteracao;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_IMAGEM = "regiao-carregamento-inputs-imagem";
        private readonly VisualElement regiaoCarregamentoInputsImagem;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_AUDIO = "regiao-carregamento-inputs-audio";
        private readonly VisualElement regiaoCarregamentoInputsAudio;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_TEXTO = "regiao-carregamento-inputs-texto";
        private readonly VisualElement regiaoCarregamentoInputsTexto;

        private readonly InputsComponenteImagem grupoInputsImagem;
        private readonly InputsComponenteAudio grupoInputsAudio;
        private readonly InputsComponenteTexto grupoInputsTexto;

        #endregion

        private readonly TiposApoiosObjetosInteracao tipoPadrao = TiposApoiosObjetosInteracao.Texto;

        private IdentificadorTipoApoioObjetoInteracao tipoApoioObjetoInteracao;

        private Texto texto;
        private AudioSource audioSource;
        private SpriteRenderer spriteRenderer;

        public InputsTipoApoioInteracao() {
            grupoInputsAudio = new InputsComponenteAudio();
            grupoInputsImagem = new InputsComponenteImagem();
            grupoInputsTexto = new InputsComponenteTexto();

            campoTipoApoioObjetoInteracao = Root.Query<EnumField>(NOME_INPUT_TIPO_ACAO_APOIO_OBJETO_INTERACAO);
            regiaoCarregamentoInputsImagem = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_IMAGEM);
            regiaoCarregamentoInputsAudio = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_AUDIO);
            regiaoCarregamentoInputsTexto = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_TEXTO);

            ConfigurarInputTipoApoioObjetoInteracao();
            CarregarRegiaoInputsAudio();
            CarregarRegiaoInputsImagem();
            CarregarRegiaoInputsTexto();

            AlterarVisibilidadeCamposComBaseTipo(tipoPadrao);

            return;
        }

        private void AlterarVisibilidadeCamposComBaseTipo(TiposApoiosObjetosInteracao tipo) {
            regiaoCarregamentoInputsAudio.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsImagem.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsTexto.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            switch(tipo) {
                case(TiposApoiosObjetosInteracao.Audio): {
                    regiaoCarregamentoInputsAudio.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                    break;
                }
                case(TiposApoiosObjetosInteracao.Seta): {
                    regiaoCarregamentoInputsImagem.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                    regiaoCarregamentoInputsAudio.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                    break;
                }
                case(TiposApoiosObjetosInteracao.Texto): {
                    regiaoCarregamentoInputsTexto.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                    break;
                }
            }

            return;
        }

        private void ConfigurarInputTipoApoioObjetoInteracao() {
            campoTipoApoioObjetoInteracao.labelElement.name = NOME_LABEL_TIPO_ACAO_APOIO_OBJETO_INTERACAO;
            campoTipoApoioObjetoInteracao.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            campoTipoApoioObjetoInteracao.Init(tipoPadrao);
            campoTipoApoioObjetoInteracao.SetValueWithoutNotify(tipoPadrao);

            return;
        }

        private void CarregarRegiaoInputsImagem() {
            regiaoCarregamentoInputsImagem.Add(grupoInputsImagem.Root);
            grupoInputsImagem.ReiniciarCampos();

            return;
        }

        private void CarregarRegiaoInputsAudio() {
            regiaoCarregamentoInputsAudio.Add(grupoInputsAudio.Root);
            grupoInputsAudio.ReiniciarCampos();

            return;
        }

        private void CarregarRegiaoInputsTexto() {
            regiaoCarregamentoInputsTexto.Add(grupoInputsTexto.Root);
            grupoInputsTexto.ReiniciarCampos();

            return;
        }

        public void ReiniciarCampos() {
            campoTipoApoioObjetoInteracao.SetValueWithoutNotify(tipoPadrao);
            grupoInputsAudio.ReiniciarCampos();
            grupoInputsImagem.ReiniciarCampos();
            grupoInputsTexto.ReiniciarCampos();

            return;
        }

        public void VincularDados(IdentificadorTipoApoioObjetoInteracao componente) {
            tipoApoioObjetoInteracao = componente;

            texto = tipoApoioObjetoInteracao.GetComponent<Texto>();
            audioSource = tipoApoioObjetoInteracao.GetComponent<AudioSource>();
            spriteRenderer = tipoApoioObjetoInteracao.GetComponent<SpriteRenderer>();

            campoTipoApoioObjetoInteracao.SetValueWithoutNotify(tipoApoioObjetoInteracao.Tipo);
            campoTipoApoioObjetoInteracao.RegisterCallback<ChangeEvent<Enum>>(evt => {
                TiposApoiosObjetosInteracao novoTipo = Enum.Parse<TiposApoiosObjetosInteracao>(campoTipoApoioObjetoInteracao.value.ToString());

                tipoApoioObjetoInteracao.AlterarTipo(novoTipo);
                AlterarVisibilidadeCamposComBaseTipo(novoTipo);
            });

            grupoInputsAudio.VincularDados(audioSource);
            grupoInputsTexto.VincularDados(texto);
            grupoInputsImagem.VincularDados(spriteRenderer);

            AlterarVisibilidadeCamposComBaseTipo(tipoApoioObjetoInteracao.Tipo);

            return;
        }
    }
}