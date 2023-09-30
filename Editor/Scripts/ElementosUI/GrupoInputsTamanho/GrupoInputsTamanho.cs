using UnityEngine.UIElements;
using Autis.Editor.Constantes;
using Autis.Editor.Manipuladores;

namespace Autis.Editor.UI {
    public class GrupoInputsTamanho : ElementoInterfaceEditor, IReiniciavel, ICamposAtualizaveis, IVinculavel<ManipuladorObjetos> {
        protected override string CaminhoTemplate => "ElementosUI/GrupoInputsTamanho/GrupoInputsTamanhoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/GrupoInputsTamanho/GrupoInputsTamanhoStyle.uss";

        #region .: Elementos :.

        public InputNumerico CampoTamanhoX { get => campoTamanhoX; }
        public InputNumerico CampoTamanhoY { get => campoTamanhoY; }

        private const string NOME_LABEL_TAMANHO_X = "label-tamanho-x";
        private const string NOME_INPUT_TAMANHO_X = "input-tamanho-x";
        private readonly InputNumerico campoTamanhoX;

        private const string NOME_LABEL_TAMANHO_Y = "label-tamanho-y";
        private const string NOME_INPUT_TAMANHO_Y = "input-tamanho-y";
        private readonly InputNumerico campoTamanhoY;

        #endregion

        private ManipuladorObjetos manipulador;

        public GrupoInputsTamanho() {
            campoTamanhoX = new InputNumerico("Largura");
            campoTamanhoY = new InputNumerico("Altura");

            root.Add(campoTamanhoX.Root);
            root.Add(campoTamanhoY.Root);

            ConfigurarCamposTamanho();

            return;
        }
        
        private void ConfigurarCamposTamanho() {
            CampoTamanhoX.CampoNumerico.labelElement.name = NOME_LABEL_TAMANHO_X;
            CampoTamanhoX.CampoNumerico.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoTamanhoX.CampoNumerico.SetValueWithoutNotify(1);

            CampoTamanhoY.CampoNumerico.labelElement.name = NOME_LABEL_TAMANHO_Y;
            CampoTamanhoY.CampoNumerico.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoTamanhoY.CampoNumerico.SetValueWithoutNotify(1);

            return;
        }

        public void ReiniciarCampos() {
            CampoTamanhoX.CampoNumerico.SetValueWithoutNotify(1);
            CampoTamanhoY.CampoNumerico.SetValueWithoutNotify(1);
            return;
        }

        public void VincularDados(ManipuladorObjetos manipulador) {
            this.manipulador = manipulador;

            campoTamanhoX.CampoNumerico.SetValueWithoutNotify(this.manipulador.GetTamanho().x);
            campoTamanhoY.CampoNumerico.SetValueWithoutNotify(this.manipulador.GetTamanho().y);

            campoTamanhoX.CampoNumerico.RegisterCallback<ChangeEvent<float>>(evt => {
                this.manipulador.SetTamanhoX(evt.newValue);
            });

            campoTamanhoY.CampoNumerico.RegisterCallback<ChangeEvent<float>>(evt => {
                this.manipulador.SetTamanhoY(evt.newValue);
            });

            return;
        }

        public void AtualizarCampos() {
            if(manipulador == null) {
                return;
            }

            campoTamanhoX?.CampoNumerico.SetValueWithoutNotify(manipulador.GetTamanho().x);
            campoTamanhoY?.CampoNumerico.SetValueWithoutNotify(manipulador.GetTamanho().y);

            return;
        }
    }
}
