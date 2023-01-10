using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(ComponentesGameObjects.Camera))]
    public class CustomEditorCameraBehaviour : CustomEditor<ComponentesGameObjects.Camera, UnityEngine.Camera> {
        private const int POSICAO_PADRAO_EIXO_Z = -10;

        #region .: Elementos :.

        private const string NOME_LABEL_POSICAO_X = "label-posicao-x";
        private const string NOME_INPUT_POSICAO_X = "input-posicao-x";
        private FloatField campoPosicaoX;

        private const string NOME_LABEL_POSICAO_Y = "label-posicao-y";
        private const string NOME_INPUT_POSICAO_Y = "input-posicao-y";
        private FloatField campoPosicaoY;

        private const string NOMR_LABEL_COR_FUNDO = "label-cor-fundo";
        private const string NOME_INPUT_COR_FUNDO = "input-cor-fundo";
        private ColorField campoCorFundo;

        private const string NOMR_LABEL_ZOOM = "label-zoom";
        private const string NOME_INPUT_ZOOM = "input-zoom";
        private FloatField campoZoom;

        #endregion

        private AudioListener audioListener;
        private Transform transform;

        public override void OnEnable() {
            base.OnEnable();
            audioListener = componente.GetComponent<AudioListener>();
            transform = componente.transform;

            AlterarVisibilidadeComponenteOriginal(HideFlags.HideInInspector);

            ImportarTemplate("/CustomEditor/CustomEditorCamera/CustomEditorCameraTemplate.uxml");
            ImportarStyle("/CustomEditor/CustomEditorCamera/CustomEditorCameraStyle.uss");
            ConfigurarInputs();

            return;
        }

        protected override void ConfigurarInputs() {
            ConfigurarInputsPosicao();
            ConfigurarInputZoom();
            ConfigurarInputCorFundo();

            return;
        }

        private void ConfigurarInputsPosicao() {
            campoPosicaoX = root.Query<FloatField>(NOME_INPUT_POSICAO_X);
            campoPosicaoX.labelElement.name = NOME_LABEL_POSICAO_X;
            campoPosicaoX.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            campoPosicaoX.SetValueWithoutNotify(transform.position.x);

            campoPosicaoX.RegisterCallback<ChangeEvent<float>>(evt => {
                transform.position = new Vector3(campoPosicaoX.value, transform.position.y, POSICAO_PADRAO_EIXO_Z);
            });

            campoPosicaoY = root.Query<FloatField>(NOME_INPUT_POSICAO_Y);
            campoPosicaoY.labelElement.name = NOME_LABEL_POSICAO_Y;
            campoPosicaoY.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            campoPosicaoY.SetValueWithoutNotify(transform.position.y);

            campoPosicaoY.RegisterCallback<ChangeEvent<float>>(evt => {
                transform.position = new Vector3(transform.position.x, campoPosicaoY.value, POSICAO_PADRAO_EIXO_Z);
            });

            return;
        }

        private void ConfigurarInputZoom() {
            campoZoom = root.Query<FloatField>(NOME_INPUT_ZOOM);

            campoZoom.labelElement.name = NOMR_LABEL_ZOOM;
            campoZoom.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            campoZoom.SetValueWithoutNotify(componenteOriginal.orthographicSize);
            campoZoom.RegisterCallback<ChangeEvent<float>>(evt => {
                componenteOriginal.orthographicSize = (evt.newValue < 0) ? 0 : evt.newValue;
            });

            return;
        }

        private void ConfigurarInputCorFundo() {
            campoCorFundo = root.Query<ColorField>(NOME_INPUT_COR_FUNDO);

            campoCorFundo.labelElement.name = NOMR_LABEL_COR_FUNDO;
            campoCorFundo.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            campoCorFundo.SetValueWithoutNotify(componenteOriginal.backgroundColor);
            campoCorFundo.RegisterCallback<ChangeEvent<Color>>(evt => {
                componenteOriginal.backgroundColor = evt.newValue;
            });

            return;
        }

        protected override void AlterarVisibilidadeComponenteOriginal(HideFlags flag) {
            base.AlterarVisibilidadeComponenteOriginal(flag);

            if(transform != null) {
                transform.hideFlags = flag;
            }

            if(audioListener != null) {
                audioListener.hideFlags = flag;
            }

            return;
        }
    }
}