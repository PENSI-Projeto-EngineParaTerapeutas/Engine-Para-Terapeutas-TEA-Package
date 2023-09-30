using UnityEngine;
using UnityEngine.UIElements;
using Autis.Editor.Constantes;

namespace Autis.Editor.UI {
    public class InputsComponentePosicao : ElementoInterfaceEditor, IVinculavel<Transform>, IReiniciavel, ICamposAtualizaveis {
        protected override string CaminhoTemplate => "ElementosUI/InputsComponentes/InputsComponentePosicao/InputsComponentePosicaoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputsComponentes/InputsComponentePosicao/InputsComponentePosicaoStyle.uss";

        #region .: Elementos :.
        public GrupoInputsPosicao GrupoInputsPosicao { get => grupoInputsPosicao; }
        public GrupoInputsTamanho GrupoInputsTamanho { get => grupoInputsTamanho; }
        public InputNumerico CampoRotacao { get => campoRotacao; }

        private readonly GrupoInputsPosicao grupoInputsPosicao;
        private readonly GrupoInputsTamanho grupoInputsTamanho;

        private const string GRUPO_INPUTS_POSICAO = "grupo-inputs-posicao";
        private readonly VisualElement campoGrupoInputsPosicao;

        private const string GRUPO_INPUTS_TAMANHO = "grupo-inputs-tamanho";
        private readonly VisualElement campoGrupoInputsTamanho;

        private const string NOME_LABEL_ROTACAO = "label-rotacao";
        private const string NOME_INPUT_ROTACAO = "input-rotacao";
        private readonly InputNumerico campoRotacao;



        #endregion

        private Transform transformVinculado;

        public InputsComponentePosicao() {

            campoGrupoInputsPosicao = Root.Query<VisualElement>(GRUPO_INPUTS_POSICAO);
            campoGrupoInputsTamanho = Root.Query<VisualElement>(GRUPO_INPUTS_TAMANHO);

            grupoInputsPosicao = new GrupoInputsPosicao();
            grupoInputsTamanho = new GrupoInputsTamanho();
            campoRotacao = new InputNumerico("Rotação", "Gira o ator no sentido anti-horário", 360, 0);

            campoGrupoInputsPosicao.Add(grupoInputsPosicao.Root);
            campoGrupoInputsTamanho.Add(grupoInputsTamanho.Root);
            Root.Add(campoRotacao.Root);

            ConfigurarCampoRotacao();

            return;
        }

        private void ConfigurarCampoRotacao() {
            CampoRotacao.CampoNumerico.labelElement.name = NOME_LABEL_ROTACAO;
            CampoRotacao.CampoNumerico.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoRotacao.CampoNumerico.SetValueWithoutNotify(0);

            return;
        }

        public void VincularDados(Transform componente) {
            transformVinculado = componente;

            GrupoInputsPosicao.CampoPosicaoX.CampoNumerico.SetValueWithoutNotify(transformVinculado.position.x);
            GrupoInputsPosicao.CampoPosicaoY.CampoNumerico.SetValueWithoutNotify(transformVinculado.position.y);

            GrupoInputsTamanho.CampoTamanhoX.CampoNumerico.SetValueWithoutNotify(transformVinculado.localScale.x);
            GrupoInputsTamanho.CampoTamanhoY.CampoNumerico.SetValueWithoutNotify(transformVinculado.localScale.y);

            CampoRotacao.CampoNumerico.SetValueWithoutNotify(transformVinculado.rotation.z);

            GrupoInputsPosicao.CampoPosicaoX.CampoNumerico.RegisterCallback<ChangeEvent<float>>(evt => {
                transformVinculado.position = new Vector3(grupoInputsPosicao.CampoPosicaoX.CampoNumerico.value, transformVinculado.position.y, 0);
            });

            GrupoInputsPosicao.CampoPosicaoY.CampoNumerico.RegisterCallback<ChangeEvent<float>>(evt => {
                transformVinculado.position = new Vector3(transformVinculado.position.x, grupoInputsPosicao.CampoPosicaoY.CampoNumerico.value, 0);
            });

            GrupoInputsTamanho.CampoTamanhoX.CampoNumerico.RegisterCallback<ChangeEvent<float>>(evt => {
                transformVinculado.localScale = new Vector3(GrupoInputsTamanho.CampoTamanhoX.CampoNumerico.value, transformVinculado.localScale.y, 0);
            });

            GrupoInputsTamanho.CampoTamanhoY.CampoNumerico.RegisterCallback<ChangeEvent<float>>(evt => {
                transformVinculado.localScale = new Vector3(transformVinculado.localScale.x, GrupoInputsTamanho.CampoTamanhoY.CampoNumerico.value, 0);
            });

            CampoRotacao.CampoNumerico.RegisterCallback<ChangeEvent<float>>(evt => {
                transformVinculado.rotation = new Quaternion(0, 0, campoRotacao.CampoNumerico.value, transformVinculado.rotation.w);
            });

            return;
        }

        public void ReiniciarCampos() {
            GrupoInputsPosicao.CampoPosicaoX.CampoNumerico.SetValueWithoutNotify(0);
            GrupoInputsPosicao.CampoPosicaoY.CampoNumerico.SetValueWithoutNotify(0);

            GrupoInputsTamanho.CampoTamanhoX.CampoNumerico.SetValueWithoutNotify(0);
            GrupoInputsTamanho.CampoTamanhoY.CampoNumerico.SetValueWithoutNotify(0);

            CampoRotacao.CampoNumerico.SetValueWithoutNotify(0);

            return;
        }

        public void AtualizarCampos() {
            if(transformVinculado == null) {
                return;
            }

            GrupoInputsPosicao.CampoPosicaoX?.CampoNumerico.SetValueWithoutNotify(transformVinculado.position.x);
            GrupoInputsPosicao.CampoPosicaoY?.CampoNumerico.SetValueWithoutNotify(transformVinculado.position.y);

            GrupoInputsTamanho.CampoTamanhoX?.CampoNumerico.SetValueWithoutNotify(transformVinculado.localScale.x);
            GrupoInputsTamanho.CampoTamanhoY?.CampoNumerico.SetValueWithoutNotify(transformVinculado.localScale.y);

            CampoRotacao.CampoNumerico?.SetValueWithoutNotify(transformVinculado.rotation.z);

            return;
        }
    }
}