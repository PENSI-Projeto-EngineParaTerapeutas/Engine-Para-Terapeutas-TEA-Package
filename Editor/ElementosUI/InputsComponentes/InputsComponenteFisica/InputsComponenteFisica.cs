using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.UI {
    public class InputsComponenteFisica : ElementoInterfaceEditor, IVinculavel<Rigidbody2D>, IReiniciavel {
        protected override string CaminhoTemplate => "ElementosUI/InputsComponentes/InputsComponenteFisica/InputsComponenteFisicaTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputsComponentes/InputsComponenteFisica/InputsComponenteFisicaStyle.uss";

        #region .: Elementos :.
        public Toggle CampoPodeMover { get => campoPodeMover; }
        public FloatField CampoGravidade { get => campoGravidade; }
        public FloatField CampoMassa { get => campoMassa; }

        private const string NOME_LABEL_PODE_MOVER = "label-pode-mover";
        private const string NOME_INPUT_PODE_MOVER = "input-pode-mover";
        private readonly Toggle campoPodeMover;

        private const string NOME_LABEL_GRAVIDADE = "label-gravidade";
        private const string NOME_INPUT_GRAVIDADE = "input-gravidade";
        private readonly FloatField campoGravidade;

        private const string NOME_LABEL_MASSA = "label-massa";
        private const string NOME_INPUT_MASSA = "input-massa";
        private readonly FloatField campoMassa;

        #endregion

        private Rigidbody2D rigidbody2DVinculado;

        public InputsComponenteFisica() {
            campoPodeMover = Root.Query<Toggle>(NOME_INPUT_PODE_MOVER);
            campoGravidade = Root.Query<FloatField>(NOME_INPUT_GRAVIDADE);
            campoMassa = Root.Query<FloatField>(NOME_INPUT_MASSA);

            ConfigurarCampoPodeMover();
            ConfigurarCampoGravidade();
            ConfigurarCampoMassa();

            return;
        }

        private void ConfigurarCampoPodeMover() {
            CampoPodeMover.labelElement.name = NOME_LABEL_PODE_MOVER;
            CampoPodeMover.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            CampoPodeMover.SetValueWithoutNotify(true);
            CampoPodeMover.RegisterCallback<ChangeEvent<bool>>(evt => {
                AlterarVisibilidadeCamposDependentes(evt.newValue);
            });

            return;
        }

        private void AlterarVisibilidadeCamposDependentes(bool deveExibir) {
            if(deveExibir) {
                CampoGravidade.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                CampoMassa.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            }
            else {
                CampoGravidade.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                CampoMassa.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            }
        }

        private void ConfigurarCampoGravidade() {
            CampoGravidade.labelElement.name = NOME_LABEL_GRAVIDADE;
            CampoGravidade.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            CampoGravidade.SetValueWithoutNotify(0);

            campoGravidade.RegisterCallback<ChangeEvent<float>>(evt => {
                if(evt.newValue < 0) {
                    campoGravidade.value = 0;
                }
            });

            return;
        }

        private void ConfigurarCampoMassa() {
            CampoMassa.labelElement.name = NOME_LABEL_MASSA;
            CampoMassa.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            CampoMassa.SetValueWithoutNotify(0);

            campoMassa.RegisterCallback<ChangeEvent<float>>(evt => {
                if(evt.newValue < 0) {
                    campoMassa.value = 0;
                }
            });

            return;
        }

        public void VincularDados(Rigidbody2D componente) {
            rigidbody2DVinculado = componente;

            CampoPodeMover.SetValueWithoutNotify(rigidbody2DVinculado.bodyType == RigidbodyType2D.Dynamic);
            CampoGravidade.SetValueWithoutNotify(rigidbody2DVinculado.gravityScale);
            CampoMassa.SetValueWithoutNotify(rigidbody2DVinculado.mass);

            campoPodeMover.RegisterCallback<ChangeEvent<bool>>(evt => {
                if(CampoPodeMover.value) {
                    rigidbody2DVinculado.bodyType = RigidbodyType2D.Dynamic;
                }
                else {
                    rigidbody2DVinculado.bodyType = RigidbodyType2D.Static;
                }
            });

            campoGravidade.RegisterCallback<ChangeEvent<float>>(evt => {
                rigidbody2DVinculado.gravityScale = CampoGravidade.value;
            });

            campoMassa.RegisterCallback<ChangeEvent<float>>(evt => {
                rigidbody2DVinculado.mass = CampoMassa.value;
            });

            AlterarVisibilidadeCamposDependentes(CampoPodeMover.value);

            return;
        }

        public void ReiniciarCampos() {
            CampoPodeMover.SetValueWithoutNotify(true);
            CampoGravidade.SetValueWithoutNotify(0);
            CampoMassa.SetValueWithoutNotify(0);

            return;
        }
    }
}