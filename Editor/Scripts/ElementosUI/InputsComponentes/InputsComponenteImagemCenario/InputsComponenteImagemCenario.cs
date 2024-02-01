using UnityEngine;
using UnityEngine.UIElements;

namespace Autis.Editor.UI {
    public class InputsComponenteImagemCenario : ElementoInterfaceEditor, IVinculavel<SpriteRenderer>, IReiniciavel {
        protected override string CaminhoTemplate => "ElementosUI/InputsComponentes/InputsComponenteImagemCenario/InputsComponenteImagemCenarioTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputsComponentes/InputsComponenteImagemCenario/InputsComponenteImagemCenarioStyle.uss";

        #region .: Elementos :.
        public VisualElement RegiaoInputImagem { get => regiaoInputImagem; }
        public VisualElement RegiaoInputCor { get => regiaoInputCor; }
        //public Toggle CampoEspelharHorizontal { get => campoEspelharHorizontal; }
        //public Toggle CampoEspelharVertical { get => campoEspelharVertical; }
        public InputImagem InputImagem { get => inputImagem; }
        public InputCor InputCor { get => inputCor; }

        private const string NOME_REGIAO_INPUT_IMAGEM = "regiao-input-imagem";
        private readonly VisualElement regiaoInputImagem;

        private const string NOME_REGIAO_INPUT_COR = "regiao-input-cor";
        private readonly VisualElement regiaoInputCor;

        //private const string NOME_LABEL_ESPELHAR_HORIZONTAL = "label-espelhar-horizontal";
        //private const string NOME_INPUT_ESPELHAR_HORIZONTAL = "input-espelhar-horizontal";
        //private readonly Toggle campoEspelharHorizontal;

        //private const string NOME_LABEL_ESPELHAR_VERTICAL = "label-espelhar-vertical";
        //private const string NOME_INPUT_ESPELHAR_VERTICAL = "input-espelhar-vertical";
        //private readonly Toggle campoEspelharVertical;

        private readonly InputImagem inputImagem;

        private readonly InputCor inputCor;

        private readonly string NOME_RADIO_COR_UNICA = "radio-opcao-cor-unica";
        private RadioButton radioButtonCorUnica;

        private readonly string NOME_RADIO_IMAGEM = "radio-opcao-imagem";
        private RadioButton radioButtonImagem;

        #endregion

        private SpriteRenderer spriteRendererVinculado;

        public InputsComponenteImagemCenario() {
            regiaoInputImagem = Root.Query<VisualElement>(NOME_REGIAO_INPUT_IMAGEM);
            regiaoInputCor = Root.Query<VisualElement>(NOME_REGIAO_INPUT_COR);
            //campoEspelharHorizontal = Root.Query<Toggle>(NOME_INPUT_ESPELHAR_HORIZONTAL);
            //campoEspelharVertical = Root.Query<Toggle>(NOME_INPUT_ESPELHAR_VERTICAL);
            inputImagem = new InputImagem();
            inputCor = new InputCor();

            ConfigurarInputCor();
            ConfigurarInputImagem();
            
            //ConfigurarInputEspelharVertical();
            //ConfigurarInputEspelharHorizontal();

            return;
        }

        private void ConfigurarInputImagem() {
            radioButtonImagem = root.Query<RadioButton>(NOME_RADIO_IMAGEM);
            radioButtonImagem.RegisterCallback<ChangeEvent<bool>>(evt => {
                inputImagem.Root.SetEnabled(evt.newValue);
            });

            regiaoInputImagem.Add(inputImagem.Root);
            return;
        }

        private void ConfigurarInputCor() {
            radioButtonCorUnica = root.Query<RadioButton>(NOME_RADIO_COR_UNICA);
            radioButtonCorUnica.RegisterCallback<ChangeEvent<bool>>(evt => {
                inputCor.Root.SetEnabled(evt.newValue);
            });

            regiaoInputCor.Add(inputCor.Root);
            return;
        }

        //private void ConfigurarInputEspelharHorizontal() {
        //    CampoEspelharHorizontal.labelElement.name = NOME_LABEL_ESPELHAR_HORIZONTAL;
        //    CampoEspelharHorizontal.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
        //    CampoEspelharHorizontal.SetValueWithoutNotify(false);

        //    return;
        //}

        //private void ConfigurarInputEspelharVertical() {
        //    CampoEspelharVertical.labelElement.name = NOME_LABEL_ESPELHAR_VERTICAL;
        //    CampoEspelharVertical.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
        //    CampoEspelharVertical.SetValueWithoutNotify(false);

        //    return;
        //}

        public void VincularDados(SpriteRenderer componente) {
            spriteRendererVinculado = componente;

            InputImagem.VincularDados(spriteRendererVinculado.sprite);
            InputCor.CampoCor.SetValueWithoutNotify(spriteRendererVinculado.color);
            //CampoEspelharHorizontal.SetValueWithoutNotify(spriteRendererVinculado.flipX);
            //CampoEspelharVertical.SetValueWithoutNotify(spriteRendererVinculado.flipY);

            InputImagem.CampoImagem.RegisterCallback<ChangeEvent<Object>>(evt => {
                spriteRendererVinculado.sprite = InputImagem.CampoImagem.value as Sprite;
            });

            InputCor.CampoCor.RegisterCallback<ChangeEvent<Color>>(evt => {
                spriteRendererVinculado.color = InputCor.CampoCor.value;
            });

            //CampoEspelharHorizontal.RegisterCallback<ChangeEvent<bool>>(evt => {
            //    spriteRendererVinculado.flipX = CampoEspelharHorizontal.value;
            //});

            //CampoEspelharVertical.RegisterCallback<ChangeEvent<bool>>(evt => {
            //    spriteRendererVinculado.flipY = CampoEspelharVertical.value;
            //});

            return;
        }

        public void ReiniciarCampos() {
            InputImagem.CampoImagem.SetValueWithoutNotify(null);
            InputCor.CampoCor.SetValueWithoutNotify(Color.white);

            //CampoEspelharHorizontal.SetValueWithoutNotify(false);
            //CampoEspelharVertical.SetValueWithoutNotify(false);

            return;
        }
    }
}