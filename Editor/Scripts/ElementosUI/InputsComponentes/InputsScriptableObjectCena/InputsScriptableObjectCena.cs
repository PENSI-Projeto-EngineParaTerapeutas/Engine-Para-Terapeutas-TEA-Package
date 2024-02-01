using System;
using UnityEngine.UIElements;
using Autis.Editor.Constantes;
using Autis.Runtime.DTOs;
using Autis.Editor.Manipuladores;

namespace Autis.Editor.UI {
    public class InputsScriptableObjectCena : ElementoInterfaceEditor, IVinculavel<ManipuladorCena>, IReiniciavel {
        protected override string CaminhoTemplate => "ElementosUI/InputsComponentes/InputsScriptableObjectCena/InputsScriptableObjectCenaTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputsComponentes/InputsScriptableObjectCena/InputsScriptableObjectCenaStyle.uss";

        #region .: Elementos :.
        public TextField CampoNome { get => campoNome; }
        public EnumField CampoDificuldade { get => campoDificuldade; }
        public IntegerField CampoFaixaEtaria { get => campoFaixaEtaria; }

        private const string NOME_LABEL_NOME = "label-nome";
        private const string NOME_INPUT_NOME = "input-nome";
        private readonly TextField campoNome;

        private const string NOME_LABEL_DIFICULDADE = "label-dificuldade";
        private const string NOME_INPUT_DIFICULDADE = "input-dificuldade";
        private readonly EnumField campoDificuldade;

        private const string NOME_LABEL_FAIXA_ETARIA = "label-faixa-etaria";
        private const string NOME_INPUT_FAIXA_ETARIA  = "input-faixa-etaria";
        private readonly IntegerField campoFaixaEtaria;

        #endregion

        private ManipuladorCena manipuladorCena;

        public InputsScriptableObjectCena() {
            campoNome = Root.Query<TextField>(NOME_INPUT_NOME);
            campoDificuldade = Root.Query<EnumField>(NOME_INPUT_DIFICULDADE);
            campoFaixaEtaria = Root.Query<IntegerField>(NOME_INPUT_FAIXA_ETARIA);

            ConfigurarCampoNome();
            ConfigurarCampoDificuldade();
            ConfigurarCampoFaixaEtaria();

            return;
        }

        private void ConfigurarCampoNome() {
            CampoNome.labelElement.name = NOME_LABEL_NOME;
            CampoNome.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoNome.AddToClassList("input-texto");
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

        public void VincularDados(ManipuladorCena manipulador) {
            manipuladorCena = manipulador;

            CampoNome.SetValueWithoutNotify(manipuladorCena.GetNome());
            CampoFaixaEtaria.SetValueWithoutNotify(manipuladorCena.GetFaixaEtaria());
            CampoDificuldade.SetValueWithoutNotify(manipuladorCena.GetDificuldade());

            campoNome.RegisterCallback<ChangeEvent<string>>(evt => {
                manipuladorCena.SetNome(evt.newValue);
            });

            CampoFaixaEtaria.RegisterCallback<ChangeEvent<int>>(evt => {
                if(evt.newValue < 0) {
                    CampoFaixaEtaria.value = 0;
                }

                manipuladorCena.SetFaixaEtaria(campoFaixaEtaria.value);
            });

            CampoDificuldade.RegisterCallback<ChangeEvent<Enum>>(evt => {
                manipuladorCena.SetDificuldade((NiveisDificuldade) evt.newValue);
            });

            return;
        }

        public void ReiniciarCampos() {
            manipuladorCena = null;

            CampoNome.SetValueWithoutNotify(string.Empty);
            CampoFaixaEtaria.SetValueWithoutNotify(0);
            CampoDificuldade.SetValueWithoutNotify(NiveisDificuldade.Facil);

            return;
        }
    }
}