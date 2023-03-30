using System;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Autis.Editor.Constantes;
using Autis.Runtime.ScriptableObjects;
using Autis.Runtime.DTOs;

namespace Autis.Editor.UI {
    public class InputsScriptableObjectCena : ElementoInterfaceEditor, IVinculavel<Cena>, IReiniciavel {
        protected override string CaminhoTemplate => "ElementosUI/InputsComponentes/InputsScriptableObjectCena/InputsScriptableObjectCenaTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputsComponentes/InputsScriptableObjectCena/InputsScriptableObjectCenaStyle.uss";

        #region .: Elementos :.
        public TextField CampoNome { get => campoNome; }
        public EnumField CampoDificuldade { get => campoDificuldade; }
        public IntegerField CampoFaixaEtaria { get => campoFaixaEtaria; }
        public VisualElement RegiaoInputVideo { get => regiaoInputVideo; }
        public InputVideo InputVideo { get => inputVideo; }

        private const string NOME_LABEL_NOME = "label-nome";
        private const string NOME_INPUT_NOME = "input-nome";
        private readonly TextField campoNome;

        private const string NOME_LABEL_DIFICULDADE = "label-dificuldade";
        private const string NOME_INPUT_DIFICULDADE = "input-dificuldade";
        private readonly EnumField campoDificuldade;

        private const string NOME_LABEL_FAIXA_ETARIA = "label-faixa-etaria";
        private const string NOME_INPUT_FAIXA_ETARIA  = "input-faixa-etaria";
        private readonly IntegerField campoFaixaEtaria;

        private const string NOME_REGIAO_INPUT_VIDEO = "regiao-input-video";
        private readonly VisualElement regiaoInputVideo;

        private readonly InputVideo inputVideo;

        #endregion

        private Cena cena;

        public InputsScriptableObjectCena() {
            campoNome = Root.Query<TextField>(NOME_INPUT_NOME);
            campoDificuldade = Root.Query<EnumField>(NOME_INPUT_DIFICULDADE);
            campoFaixaEtaria = Root.Query<IntegerField>(NOME_INPUT_FAIXA_ETARIA);
            regiaoInputVideo = Root.Query<VisualElement>(NOME_REGIAO_INPUT_VIDEO);

            inputVideo = new InputVideo();

            ConfigurarCampoNome();
            ConfigurarCampoDificuldade();
            ConfigurarCampoFaixaEtaria();
            ConfigurarInputVideo();

            return;
        }

        private void ConfigurarCampoNome() {
            CampoNome.labelElement.name = NOME_LABEL_NOME;
            CampoNome.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoNome.SetValueWithoutNotify(string.Empty);

            return;
        }

        private void ConfigurarCampoDificuldade() {
            CampoDificuldade.labelElement.name = NOME_LABEL_DIFICULDADE;
            CampoDificuldade.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoDificuldade.Init(NiveisDificuldade.Facil);
            CampoDificuldade.SetValueWithoutNotify(NiveisDificuldade.Facil);

            return;
        }

        private void ConfigurarCampoFaixaEtaria() {
            CampoFaixaEtaria.labelElement.name = NOME_LABEL_FAIXA_ETARIA;
            CampoFaixaEtaria.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            return;
        }

        private void ConfigurarInputVideo() {
            RegiaoInputVideo.Add(InputVideo.Root);
            return;
        }


        public void VincularDados(Cena componente) {
            cena = componente;

            CampoNome.SetValueWithoutNotify(cena.NomeExibicao);
            CampoFaixaEtaria.SetValueWithoutNotify(cena.FaixaEtaria);
            CampoDificuldade.SetValueWithoutNotify(cena.NivelDificuldade);
            InputVideo.CampoVideo.SetValueWithoutNotify(cena.NomeArquivoVideoContexto);

            campoNome.RegisterCallback<ChangeEvent<string>>(evt => {
                cena.NomeExibicao = CampoNome.value;
            });

            CampoFaixaEtaria.RegisterCallback<ChangeEvent<int>>(evt => {
                if(evt.newValue < 0) {
                    CampoFaixaEtaria.value = 0;
                }

                cena.FaixaEtaria = CampoFaixaEtaria.value;
            });

            CampoDificuldade.RegisterCallback<ChangeEvent<Enum>>(evt => {
                switch(CampoDificuldade.value) {
                    case(NiveisDificuldade.Facil): {
                        cena.NivelDificuldade = NiveisDificuldade.Facil;
                        break;
                    }
                    case(NiveisDificuldade.Medio): {
                        cena.NivelDificuldade = NiveisDificuldade.Medio;
                        break;
                    }
                    case(NiveisDificuldade.Dificil): {
                        cena.NivelDificuldade = NiveisDificuldade.Dificil;
                        break;
                    }
                }
            });

            InputVideo.CampoVideo.RegisterCallback<ChangeEvent<string>>(evt => {
                cena.NomeArquivoVideoContexto = InputVideo.CampoVideo.value;
            });

            return;
        }

        public void ReiniciarCampos() {
            cena = null;

            CampoNome.SetValueWithoutNotify(string.Empty);
            CampoFaixaEtaria.SetValueWithoutNotify(0);
            CampoDificuldade.SetValueWithoutNotify(NiveisDificuldade.Facil);
            InputVideo.ReiniciarCampos();

            return;
        }
    }
}