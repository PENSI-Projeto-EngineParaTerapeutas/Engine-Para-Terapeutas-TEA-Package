using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.UI {
    public class InputsComponentePosicao : ElementoInterfaceEditor, IVinculavel<Transform>, IReiniciavel, ICamposAtualizaveis {
        protected override string CaminhoTemplate => "ElementosUI/InputsComponentes/InputsComponentePosicao/InputsComponentePosicaoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputsComponentes/InputsComponentePosicao/InputsComponentePosicaoStyle.uss";

        #region .: Elementos :.
        public FloatField CampoPosicaoX { get => campoPosicaoX; }
        public FloatField CampoPosicaoY { get => campoPosicaoY; }
        public FloatField CampoTamanhoX { get => campoTamanhoX; }
        public FloatField CampoTamanhoY { get => campoTamanhoY; }
        public FloatField CampoRotacao { get => campoRotacao; }

        private const string NOME_LABEL_POSICAO_X = "label-posicao-x";
        private const string NOME_INPUT_POSICAO_X = "input-posicao-x";
        private readonly FloatField campoPosicaoX;

        private const string NOME_LABEL_POSICAO_Y = "label-posicao-y";
        private const string NOME_INPUT_POSICAO_Y = "input-posicao-y";
        private readonly FloatField campoPosicaoY;

        private const string NOME_LABEL_TAMANHO_X = "label-tamanho-x";
        private const string NOME_INPUT_TAMANHO_X = "input-tamanho-x";
        private readonly FloatField campoTamanhoX;

        private const string NOME_LABEL_TAMANHO_Y = "label-tamanho-y";
        private const string NOME_INPUT_TAMANHO_Y = "input-tamanho-y";
        private readonly FloatField campoTamanhoY;

        private const string NOME_LABEL_ROTACAO = "label-rotacao";
        private const string NOME_INPUT_ROTACAO = "input-rotacao";
        private readonly FloatField campoRotacao;

        #endregion

        private Transform transformVinculado;

        public InputsComponentePosicao() {
            campoPosicaoX = Root.Query<FloatField>(NOME_INPUT_POSICAO_X);
            campoPosicaoY = Root.Query<FloatField>(NOME_INPUT_POSICAO_Y);

            campoTamanhoX = Root.Query<FloatField>(NOME_INPUT_TAMANHO_X);
            campoTamanhoY = Root.Query<FloatField>(NOME_INPUT_TAMANHO_Y);

            campoRotacao = Root.Query<FloatField>(NOME_INPUT_ROTACAO);

            ConfigurarCamposPosicao();
            ConfigurarCampoTamanho();
            ConfigurarCampoRotacao();

            return;
        }

        private void ConfigurarCamposPosicao() {
            CampoPosicaoX.labelElement.name = NOME_LABEL_POSICAO_X;
            CampoPosicaoX.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoPosicaoX.SetValueWithoutNotify(0);

            CampoPosicaoY.labelElement.name = NOME_LABEL_POSICAO_Y;
            CampoPosicaoY.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoPosicaoY.SetValueWithoutNotify(0);

            return;
        }

        private void ConfigurarCampoTamanho() {
            CampoTamanhoX.labelElement.name = NOME_LABEL_TAMANHO_X;
            CampoTamanhoX.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoTamanhoX.SetValueWithoutNotify(0);

            CampoTamanhoY.labelElement.name = NOME_LABEL_TAMANHO_Y;
            CampoTamanhoY.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoTamanhoY.SetValueWithoutNotify(0);

            return;
        }

        private void ConfigurarCampoRotacao() {
            CampoRotacao.labelElement.name = NOME_LABEL_ROTACAO;
            CampoRotacao.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoRotacao.SetValueWithoutNotify(0);

            return;
        }

        public void VincularDados(Transform componente) {
            transformVinculado = componente;

            CampoPosicaoX.SetValueWithoutNotify(transformVinculado.position.x);
            CampoPosicaoY.SetValueWithoutNotify(transformVinculado.position.y);

            CampoTamanhoX.SetValueWithoutNotify(transformVinculado.localScale.x);
            CampoTamanhoY.SetValueWithoutNotify(transformVinculado.localScale.y);

            CampoRotacao.SetValueWithoutNotify(transformVinculado.rotation.z);

            CampoPosicaoX.RegisterCallback<ChangeEvent<float>>(evt => {
                transformVinculado.position = new Vector3(CampoPosicaoX.value, transformVinculado.position.y, 0);
            });

            CampoPosicaoY.RegisterCallback<ChangeEvent<float>>(evt => {
                transformVinculado.position = new Vector3(transformVinculado.position.x, CampoPosicaoY.value, 0);
            });

            CampoTamanhoX.RegisterCallback<ChangeEvent<float>>(evt => {
                transformVinculado.localScale = new Vector3(CampoTamanhoX.value, transformVinculado.localScale.y, 0);
            });

            CampoTamanhoY.RegisterCallback<ChangeEvent<float>>(evt => {
                transformVinculado.localScale = new Vector3(transformVinculado.localScale.x, CampoTamanhoY.value, 0);
            });

            CampoRotacao.RegisterCallback<ChangeEvent<float>>(evt => {
                transformVinculado.rotation = new Quaternion(0, 0, campoRotacao.value, transformVinculado.rotation.w);
            });

            return;
        }

        public void ReiniciarCampos() {
            CampoPosicaoX.SetValueWithoutNotify(0);
            CampoPosicaoY.SetValueWithoutNotify(0);

            CampoTamanhoX.SetValueWithoutNotify(0);
            CampoTamanhoY.SetValueWithoutNotify(0);

            CampoRotacao.SetValueWithoutNotify(0);

            return;
        }

        public void AtualizarCampos() {
            if(transformVinculado == null) {
                return;
            }

            CampoPosicaoX?.SetValueWithoutNotify(transformVinculado.position.x);
            CampoPosicaoY?.SetValueWithoutNotify(transformVinculado.position.y);

            CampoTamanhoX?.SetValueWithoutNotify(transformVinculado.localScale.x);
            CampoTamanhoY?.SetValueWithoutNotify(transformVinculado.localScale.y);

            CampoRotacao?.SetValueWithoutNotify(transformVinculado.rotation.z);

            return;
        }
    }
}