using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EngineParaTerapeutas.ComponentesGameObjects;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.DTOs;

namespace EngineParaTerapeutas.UI {
    public class InputsIdentificadorTipoAcao : ElementoInterfaceEditor, IVinculavel<IdentificadorTipoAcao>, IReiniciavel {
        private const string NOME_GAME_OBJECT_VIDEO_OBJETO_INTERACAO = "Video";

        protected override string CaminhoTemplate => "ElementosUI/InputsComponentes/InputsIdentificadorTipoAcao/InputsIdentificadorTipoAcaoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputsComponentes/InputsIdentificadorTipoAcao/InputsIdentificadorTipoAcaoStyle.uss";

        #region .: Elementos :.

        private const string NOME_LABEL_TIPO_ACAO = "label-tipo-acao";
        private const string NOME_INPUT_TIPO_ACAO = "input-tipo-acao";
        private readonly EnumField campoTipoAcao;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_AUDIO = "regiao-carregamento-inputs-audio";
        private readonly VisualElement regiaoCarregamentoInputsAudio;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_VIDEO = "regiao-carregamento-inputs-video";
        private readonly VisualElement regiaoCarregamentoInputsVideo;

        private readonly InputAudio inputAudio;
        private readonly InputVideo inputVideo;

        #endregion

        private IdentificadorTipoAcao componenteTipoAcao;
        private AudioSource audioSource;

        private Transform gameObjectVideo;
        private Video componenteVideo;

        public InputsIdentificadorTipoAcao() {
            inputAudio = new InputAudio();
            inputVideo = new InputVideo();

            campoTipoAcao = Root.Query<EnumField>(NOME_INPUT_TIPO_ACAO);
            regiaoCarregamentoInputsAudio = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_AUDIO);
            regiaoCarregamentoInputsVideo = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_VIDEO);

            ConfigurarInputTipoAcao();
            ConfigurarInputAudio();
            ConfigurarInputVideo();

            return;
        }

        private void ConfigurarInputTipoAcao() {
            campoTipoAcao.labelElement.name = NOME_LABEL_TIPO_ACAO;
            campoTipoAcao.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            campoTipoAcao.Init(TiposAcoes.Nenhuma);
            campoTipoAcao.SetValueWithoutNotify(TiposAcoes.Nenhuma);

            return;
        }

        private void ConfigurarInputAudio() {
            regiaoCarregamentoInputsAudio.Add(inputAudio.Root);
            inputAudio.CampoAudio.SetValueWithoutNotify(null);

            return;
        }

        private void ConfigurarInputVideo() {
            regiaoCarregamentoInputsVideo.Add(inputVideo.Root);
            inputVideo.CampoVideo.SetValueWithoutNotify(string.Empty);

            return;
        }

        public void ReiniciarCampos() {
            campoTipoAcao.SetValueWithoutNotify(TiposAcoes.Nenhuma);
            inputAudio.ReiniciarCampos();
            inputVideo.ReiniciarCampos();

            return;
        }

        public void VincularDados(IdentificadorTipoAcao componente) {
            componenteTipoAcao = componente;

            audioSource = componenteTipoAcao.GetComponent<AudioSource>();
            gameObjectVideo = componenteTipoAcao.transform.Find(NOME_GAME_OBJECT_VIDEO_OBJETO_INTERACAO);
            componenteVideo = gameObjectVideo.GetComponent<Video>();

            campoTipoAcao.Init(componenteTipoAcao.Tipo);
            campoTipoAcao.SetValueWithoutNotify(componenteTipoAcao.Tipo);
            campoTipoAcao.RegisterCallback<ChangeEvent<Enum>>(evt => {
                componenteTipoAcao.AlterarTipo(Enum.Parse<TiposAcoes>(campoTipoAcao.value.ToString()));
            });

            inputAudio.CampoAudio.SetValueWithoutNotify(audioSource.clip);
            inputAudio.CampoAudio.RegisterCallback<ChangeEvent<UnityEngine.Object>>(evt => {
                audioSource.clip = inputAudio.CampoAudio.value as AudioClip;
            });

            inputVideo.CampoVideo.SetValueWithoutNotify(componenteVideo.nomeArquivoVideo);
            inputVideo.CampoVideo.RegisterCallback<ChangeEvent<string>>(evt => {
                componenteVideo.nomeArquivoVideo = inputVideo.CampoVideo.value;
            });

            return;
        }
    }
}