using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.UI {
    public class InputsColisor : ElementoInterfaceEditor, IVinculavel<BoxCollider2D>, IReiniciavel {
        #region .: Elementos :.

        private const string NOME_LABEL_HABILITADO = "label-habilitado";
        private const string NOME_INPUT_HABILITADO = "input-habilitado";
        public Toggle CampoHabilitado { get => campoHabilitado; }
        private readonly Toggle campoHabilitado;

        private const string NOME_LABEL_OCUPA_ESPACO = "label-ocupa-espaco";
        private const string NOME_INPUT_OCUPA_ESPACO = "input-ocupa-espaco";
        public Toggle CampoOcupaEspaco { get => campoOcupaEspaco; }
        private readonly Toggle campoOcupaEspaco;

        private const string NOME_LABEL_TAMANHO = "label-tamanho";
        public Label LabelTamanho { get => labelTamanho; }
        private readonly Label labelTamanho;

        private const string NOME_LABEL_ALTURA = "label-altura";
        private const string NOME_INPUT_ALTURA = "input-altura";
        public FloatField CampoAltura { get => campoAltura; }
        private readonly FloatField campoAltura;

        private const string NOME_LABEL_LARGURA = "label-largura";
        private const string NOME_INPUT_LARGURA = "input-largura";
        public FloatField CampoLargura { get => campoLargura; }
        private readonly FloatField campoLargura;

        #endregion

        private BoxCollider2D colisorVinculado;

        public InputsColisor() {
            ImportarTemplate("Componentes/GruposInputs/InputsColisor/InputsColisorTemplate.uxml");
            ImportarStyle("Componentes/GruposInputs/InputsColisor/InputsColisorStyle.uss");

            campoHabilitado = Root.Query<Toggle>(NOME_INPUT_HABILITADO);
            campoOcupaEspaco = Root.Query<Toggle>(NOME_INPUT_OCUPA_ESPACO);
            labelTamanho = Root.Query<Label>(NOME_LABEL_TAMANHO);
            campoLargura = Root.Query<FloatField>(NOME_INPUT_LARGURA);
            campoAltura = Root.Query<FloatField>(NOME_INPUT_ALTURA);

            ConfigurarHabilitado();
            ConfigurarOcupaEspaco();
            ConfigurarCampoLargura();
            ConfigurarCampoAltura();

            return;
        }

        private void ConfigurarHabilitado() {
            CampoHabilitado.labelElement.name = NOME_LABEL_HABILITADO;
            CampoHabilitado.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            CampoHabilitado.SetValueWithoutNotify(true);
            CampoHabilitado.RegisterCallback<ChangeEvent<bool>>(evt => {
                AlterarVisibilidadeCamposDependentes(evt.newValue);
            });

            return;
        }

        private void AlterarVisibilidadeCamposDependentes(bool deveExibir) {
            if (deveExibir) {
                CampoOcupaEspaco.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                CampoLargura.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                CampoAltura.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                LabelTamanho.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            } else {
                CampoOcupaEspaco.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                CampoLargura.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                CampoAltura.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                LabelTamanho.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            }

            return;
        }

        private void ConfigurarOcupaEspaco() {
            CampoOcupaEspaco.labelElement.name = NOME_LABEL_OCUPA_ESPACO;
            CampoOcupaEspaco.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            CampoOcupaEspaco.SetValueWithoutNotify(true);

            return;
        }

        private void ConfigurarCampoLargura() {
            CampoLargura.labelElement.name = NOME_LABEL_LARGURA;
            CampoLargura.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            CampoLargura.SetValueWithoutNotify(0);
            CampoLargura.RegisterCallback<ChangeEvent<float>>(evt => {
                if (evt.newValue < 0) {
                    CampoLargura.value = 0;
                }
            });

            return;
        }

        private void ConfigurarCampoAltura() {
            CampoAltura.labelElement.name = NOME_LABEL_ALTURA;
            CampoAltura.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            CampoAltura.SetValueWithoutNotify(0);
            CampoAltura.RegisterCallback<ChangeEvent<float>>(evt => {
                if (evt.newValue < 0) {
                    CampoAltura.value = 0;
                }
            });

            return;
        }

        public void ReiniciarCampos() {
            CampoHabilitado.SetValueWithoutNotify(true);
            CampoOcupaEspaco.SetValueWithoutNotify(true);
            CampoAltura.SetValueWithoutNotify(0);
            CampoLargura.SetValueWithoutNotify(0);

            return;
        }

        public void VincularDados(BoxCollider2D componente) {
            colisorVinculado = componente;

            CampoHabilitado.SetValueWithoutNotify(colisorVinculado.enabled);
            CampoOcupaEspaco.SetValueWithoutNotify(!colisorVinculado.isTrigger);
            CampoLargura.SetValueWithoutNotify(colisorVinculado.size.x);
            CampoAltura.SetValueWithoutNotify(colisorVinculado.size.y);

            CampoHabilitado.RegisterCallback<ChangeEvent<bool>>(evt => {
                colisorVinculado.enabled = CampoHabilitado.value;
            });

            CampoOcupaEspaco.RegisterCallback<ChangeEvent<bool>>(evt => {
                colisorVinculado.isTrigger = !CampoOcupaEspaco.value;
            });

            CampoLargura.RegisterCallback<ChangeEvent<float>>(evt => {
                colisorVinculado.size = new Vector2(CampoLargura.value, colisorVinculado.size.y);
            });

            CampoAltura.RegisterCallback<ChangeEvent<float>>(evt => {
                colisorVinculado.size = new Vector2(colisorVinculado.size.x, CampoAltura.value);
            });

            AlterarVisibilidadeCamposDependentes(CampoHabilitado.value);

            return;
        }
    }
}