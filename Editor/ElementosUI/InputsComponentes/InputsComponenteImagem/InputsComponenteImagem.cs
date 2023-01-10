using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.UI {
    public class InputsComponenteImagem : ElementoInterfaceEditor, IVinculavel<SpriteRenderer>, IReiniciavel {
        #region .: Elementos :.
        public VisualElement RegiaoInputImagem { get => regiaoInputImagem; }
        public ColorField CampoCor { get => campoCor; }
        public Toggle CampoEspelharHorizontal { get => campoEspelharHorizontal; }
        public Toggle CampoEspelharVertical { get => campoEspelharVertical; }
        public InputImagem InputImagem { get => inputImagem; }

        private const string NOME_REGIAO_INPUT_IMAGEM = "regiao-input-imagem";
        private readonly VisualElement regiaoInputImagem;

        private const string NOME_LABEL_COR = "label-cor";
        private const string NOME_INPUT_COR = "input-cor";
        private readonly ColorField campoCor;

        private const string NOME_LABEL_ESPELHAR_HORIZONTAL = "label-espelhar-horizontal";
        private const string NOME_INPUT_ESPELHAR_HORIZONTAL = "input-espelhar-horizontal";
        private readonly Toggle campoEspelharHorizontal;

        private const string NOME_LABEL_ESPELHAR_VERTICAL = "label-espelhar-vertical";
        private const string NOME_INPUT_ESPELHAR_VERTICAL = "input-espelhar-vertical";
        private readonly Toggle campoEspelharVertical;

        private readonly InputImagem inputImagem;

        #endregion

        private SpriteRenderer spriteRendererVinculado;

        public InputsComponenteImagem() {
            ImportarTemplate("ElementosUI/InputsComponentes/InputsComponenteImagem/InputsComponenteImagemTemplate.uxml");
            ImportarStyle("ElementosUI/InputsComponentes/InputsComponenteImagem/InputsComponenteImagemStyle.uss");

            regiaoInputImagem = Root.Query<VisualElement>(NOME_REGIAO_INPUT_IMAGEM);
            campoCor = Root.Query<ColorField>(NOME_INPUT_COR);
            campoEspelharHorizontal = Root.Query<Toggle>(NOME_INPUT_ESPELHAR_HORIZONTAL);
            campoEspelharVertical = Root.Query<Toggle>(NOME_INPUT_ESPELHAR_VERTICAL);

            inputImagem = new InputImagem();

            ConfigurarInputImagem();
            ConfigurarInputCor();
            ConfigurarInputEspelharVertical();
            ConfigurarInputEspelharHorizontal();

            return;
        }

        private void ConfigurarInputImagem() {
            regiaoInputImagem.Add(inputImagem.Root);
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

            InputImagem.CampoImagem.SetValueWithoutNotify(spriteRendererVinculado.sprite);
            CampoCor.SetValueWithoutNotify(spriteRendererVinculado.color);
            CampoEspelharHorizontal.SetValueWithoutNotify(spriteRendererVinculado.flipX);
            CampoEspelharVertical.SetValueWithoutNotify(spriteRendererVinculado.flipY);

            InputImagem.CampoImagem.RegisterCallback<ChangeEvent<Object>>(evt => {
                spriteRendererVinculado.sprite = InputImagem.CampoImagem.value as Sprite;
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
            InputImagem.CampoImagem.SetValueWithoutNotify(null);
            CampoCor.SetValueWithoutNotify(Color.white);

            CampoEspelharHorizontal.SetValueWithoutNotify(false);
            CampoEspelharVertical.SetValueWithoutNotify(false);

            return;
        }
    }
}