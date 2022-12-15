using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.UI {
    public class InputsImagem : ElementoInterfaceEditor, IVinculavel<SpriteRenderer>, IReiniciavel {
        #region .: Elementos :.

        private const string NOME_LABEL_IMAGEM = "label-imagem";
        private const string NOME_INPUT_IMAGEM = "input-imagem";
        public ObjectField CampoImagem { get => campoImagem; }
        private readonly ObjectField campoImagem;

        private const string NOME_LABEL_COR = "label-cor";
        private const string NOME_INPUT_COR = "input-cor";
        public ColorField CampoCor { get => campoCor; }
        private readonly ColorField campoCor;

        private const string NOME_LABEL_ESPELHAR_HORIZONTAL = "label-espelhar-horizontal";
        private const string NOME_INPUT_ESPELHAR_HORIZONTAL = "input-espelhar-horizontal";
        public Toggle CampoEspelharHorizontal { get => campoEspelharHorizontal; }
        private readonly Toggle campoEspelharHorizontal;

        private const string NOME_LABEL_ESPELHAR_VERTICAL = "label-espelhar-vertical";
        private const string NOME_INPUT_ESPELHAR_VERTICAL = "input-espelhar-vertical";
        public Toggle CampoEspelharVertical { get => campoEspelharVertical; }
        private readonly Toggle campoEspelharVertical;

        #endregion

        private SpriteRenderer spriteRendererVinculado;

        public InputsImagem() {
            ImportarTemplate("Componentes/GruposInputs/InputsImagem/InputsImagemTemplate.uxml");
            ImportarStyle("Componentes/GruposInputs/InputsImagem/InputsImagemStyle.uss");

            campoImagem = Root.Query<ObjectField>(NOME_INPUT_IMAGEM);
            campoCor = Root.Query<ColorField>(NOME_INPUT_COR);
            campoEspelharHorizontal = Root.Query<Toggle>(NOME_INPUT_ESPELHAR_HORIZONTAL);
            campoEspelharVertical = Root.Query<Toggle>(NOME_INPUT_ESPELHAR_VERTICAL);

            ConfigurarInputImagem();
            ConfigurarInputCor();
            ConfigurarInputEspelharVertical();
            ConfigurarInputEspelharHorizontal();

            return;
        }

        private void ConfigurarInputImagem() {
            CampoImagem.labelElement.name = NOME_LABEL_IMAGEM;
            CampoImagem.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            CampoImagem.objectType = typeof(Sprite);

            return;
        }

        private void ConfigurarInputCor() {
            CampoCor.labelElement.name = NOME_LABEL_COR;
            CampoCor.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            return;
        }

        private void ConfigurarInputEspelharHorizontal() {
            CampoEspelharHorizontal.labelElement.name = NOME_LABEL_ESPELHAR_HORIZONTAL;
            CampoEspelharHorizontal.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoEspelharHorizontal.SetValueWithoutNotify(false);

            return;
        }

        private void ConfigurarInputEspelharVertical() {
            CampoEspelharVertical.labelElement.name = NOME_LABEL_ESPELHAR_VERTICAL;
            CampoEspelharVertical.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoEspelharVertical.SetValueWithoutNotify(false);

            return;
        }

        public void VincularDados(SpriteRenderer componente) {
            spriteRendererVinculado = componente;

            CampoImagem.SetValueWithoutNotify(spriteRendererVinculado.sprite);
            CampoCor.SetValueWithoutNotify(spriteRendererVinculado.color);
            CampoEspelharHorizontal.SetValueWithoutNotify(spriteRendererVinculado.flipX);
            CampoEspelharVertical.SetValueWithoutNotify(spriteRendererVinculado.flipY);

            CampoImagem.RegisterCallback<ChangeEvent<Object>>(evt => {
                spriteRendererVinculado.sprite = CampoImagem.value as Sprite;
            });

            CampoCor.RegisterCallback<ChangeEvent<Color>>(evt => {
                spriteRendererVinculado.color = CampoCor.value;
            });

            CampoEspelharHorizontal.RegisterCallback<ChangeEvent<bool>>(evt => {
                spriteRendererVinculado.flipX = CampoEspelharHorizontal.value;
            });

            CampoEspelharVertical.RegisterCallback<ChangeEvent<bool>>(evt => {
                spriteRendererVinculado.flipY = CampoEspelharVertical.value;
            });

            return;
        }

        public void ReiniciarCampos() {
            CampoImagem.SetValueWithoutNotify(null);
            CampoCor.SetValueWithoutNotify(Color.white);

            CampoEspelharHorizontal.SetValueWithoutNotify(false);
            CampoEspelharVertical.SetValueWithoutNotify(false);
            return;
        }
    }
}