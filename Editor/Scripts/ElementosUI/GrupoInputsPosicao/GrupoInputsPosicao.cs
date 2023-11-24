using UnityEngine.UIElements;
using Autis.Editor.Constantes;
using Autis.Editor.Manipuladores;

namespace Autis.Editor.UI {
    public class GrupoInputsPosicao : ElementoInterfaceEditor, IReiniciavel, ICamposAtualizaveis, IVinculavel<ManipuladorObjetos> {
        protected override string CaminhoTemplate => "ElementosUI/GrupoInputsPosicao/GrupoInputsPosicaoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/GrupoInputsPosicao/GrupoInputsPosicaoStyle.uss";

        #region .: Elementos :.

        public InputNumerico CampoPosicaoX { get => campoPosicaoX; }
        public InputNumerico CampoPosicaoY { get => campoPosicaoY; }

        private const string NOME_LABEL_POSICAO_X = "label-posicao-x";
        private InputNumerico campoPosicaoX;

        private const string NOME_LABEL_POSICAO_Y = "label-posicao-y";
        private InputNumerico campoPosicaoY;

        private const string NOME_REGIAO_CONTEUDO_PRINCIPAL = "regiao-conteudo";
        private VisualElement regiaoConteudoPrincipal;

        #endregion

        private ManipuladorObjetos manipulador;

        public GrupoInputsPosicao() {
            ConfigurarCamposPosicao();
            return;
        }

        private void ConfigurarCamposPosicao() {
            regiaoConteudoPrincipal = root.Query<VisualElement>(NOME_REGIAO_CONTEUDO_PRINCIPAL);

            campoPosicaoX = new InputNumerico("Horizontal");
            campoPosicaoX.CampoNumerico.labelElement.name = NOME_LABEL_POSICAO_X;
            campoPosicaoX.CampoNumerico.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            campoPosicaoX.CampoNumerico.SetValueWithoutNotify(0);

            regiaoConteudoPrincipal.Add(campoPosicaoX.Root);

            campoPosicaoY = new InputNumerico("Vertical");
            campoPosicaoY.CampoNumerico.labelElement.name = NOME_LABEL_POSICAO_Y;
            campoPosicaoY.CampoNumerico.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            campoPosicaoY.CampoNumerico.SetValueWithoutNotify(0);

            regiaoConteudoPrincipal.Add(campoPosicaoY.Root);

            return;
        }

        public void ReiniciarCampos() {
            campoPosicaoX.CampoNumerico.SetValueWithoutNotify(0);
            campoPosicaoY.CampoNumerico.SetValueWithoutNotify(0);
            return;
        }

        public void VincularDados(ManipuladorObjetos manipulador) {
            this.manipulador = manipulador;

            campoPosicaoX.CampoNumerico.SetValueWithoutNotify(this.manipulador.GetPosicao().x);
            campoPosicaoY.CampoNumerico.SetValueWithoutNotify(this.manipulador.GetPosicao().y);

            campoPosicaoX.CampoNumerico.RegisterCallback<ChangeEvent<float>>(evt => {
                manipulador.SetPosicaoX(evt.newValue);
            });

            campoPosicaoY.CampoNumerico.RegisterCallback<ChangeEvent<float>>(evt => {
                manipulador.SetPosicaoY(evt.newValue);
            });

            return;
        }

        public void AtualizarCampos() {
            if(manipulador == null || manipulador.ObjetoAtual == null) {
                return;
            }

            campoPosicaoX?.CampoNumerico.SetValueWithoutNotify(manipulador.GetPosicao().x);
            campoPosicaoY?.CampoNumerico.SetValueWithoutNotify(manipulador.GetPosicao().y);

            return;
        }
    }
}
