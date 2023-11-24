using UnityEngine.UIElements;
using Autis.Editor.Constantes;
using Autis.Editor.Manipuladores;
using System;
using Codice.CM.SEIDInfo;

namespace Autis.Editor.UI {
    public class GrupoInputsTamanho : ElementoInterfaceEditor, IReiniciavel, ICamposAtualizaveis, IVinculavel<ManipuladorObjetos> {
        protected override string CaminhoTemplate => "ElementosUI/GrupoInputsTamanho/GrupoInputsTamanhoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/GrupoInputsTamanho/GrupoInputsTamanhoStyle.uss";

        #region .: Mensagens :.

        private const string MENSAGEM_TOOLTIP_TITULO = "A porcentagem se refere ao tamanho do personagem. Obs: Nos casos que o personagem for controlado por controle indireto, através de animações, o tamanho do personagem pode ser diminuído na animação.";

        #endregion

        #region .: Elementos :.

        public InputNumerico CampoTamanhoX { get => campoTamanhoX; }
        public InputNumerico CampoTamanhoY { get => campoTamanhoY; }

        private const string NOME_LABEL_TAMANHO_X = "label-tamanho-x";
        private InputNumerico campoTamanhoX;

        private const string NOME_LABEL_TAMANHO_Y = "label-tamanho-y";
        private InputNumerico campoTamanhoY;

        private const string NOME_REGIAO_CONTEUDO_PRINCIPAL = "regiao-conteudo";
        private VisualElement regiaoConteudoPrincipal;

        private const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-tooltip-titulo";

        #endregion

        private ManipuladorObjetos manipulador;

        public GrupoInputsTamanho() {
            ConfigurarCamposTamanho();
            return;
        }

        private void ConfigurarCamposTamanho() {
            regiaoConteudoPrincipal = root.Query<VisualElement>(NOME_REGIAO_CONTEUDO_PRINCIPAL);

            campoTamanhoX = new InputNumerico("Largura (%):");
            CampoTamanhoX.CampoNumerico.labelElement.name = NOME_LABEL_TAMANHO_X;
            CampoTamanhoX.CampoNumerico.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoTamanhoX.CampoNumerico.SetValueWithoutNotify(100f);

            regiaoConteudoPrincipal.Add(campoTamanhoX.Root);

            campoTamanhoY = new InputNumerico("Altura (%):");
            CampoTamanhoY.CampoNumerico.labelElement.name = NOME_LABEL_TAMANHO_Y;
            CampoTamanhoY.CampoNumerico.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoTamanhoY.CampoNumerico.SetValueWithoutNotify(100f);

            regiaoConteudoPrincipal.Add(campoTamanhoY.Root);

            return;
        }

        public void ReiniciarCampos() {
            CampoTamanhoX.CampoNumerico.SetValueWithoutNotify(100f);
            CampoTamanhoY.CampoNumerico.SetValueWithoutNotify(100f);
            return;
        }

        public void VincularDados(ManipuladorObjetos manipulador) {
            this.manipulador = manipulador;

            campoTamanhoX.CampoNumerico.SetValueWithoutNotify(this.manipulador.GetTamanho().x * 100f);
            campoTamanhoY.CampoNumerico.SetValueWithoutNotify(this.manipulador.GetTamanho().y * 100f);

            campoTamanhoX.CampoNumerico.RegisterCallback<ChangeEvent<float>>(evt => {
                this.manipulador.SetTamanhoX(evt.newValue / 100f);
            });

            campoTamanhoY.CampoNumerico.RegisterCallback<ChangeEvent<float>>(evt => {
                this.manipulador.SetTamanhoY(evt.newValue / 100f);
            });

            return;
        }

        public void AtualizarCampos() {
            if(manipulador == null || manipulador.ObjetoAtual == null) {
                return;
            }

            campoTamanhoX?.CampoNumerico.SetValueWithoutNotify(manipulador.GetTamanho().x * 100f);
            campoTamanhoY?.CampoNumerico.SetValueWithoutNotify(manipulador.GetTamanho().y * 100f);

            return;
        }
    }
}
