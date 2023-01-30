using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using TMPro;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.ComponentesGameObjects;

namespace EngineParaTerapeutas.UI {
    public class InputsComponenteTexto : ElementoInterfaceEditor, IVinculavel<Texto>, IReiniciavel {

        #region

        public Toggle CampoHabilitado { get => campoHabilitado; }
        public TextField CampoConteudoTexto { get => campoConteudoTexto; }
        public FloatField CampoTamanhoTexto { get => campoTamanhoTexto; }
        public Toggle CampoNegrito { get => campoNegrito; }
        public Toggle CampoItalico { get => campoItalico; }
        public Toggle CampoSublinhado { get => campoSublinhado; }
        public ColorField CampoCor { get => campoCor; }

        private const string NOME_LABEL_HABILITADO = "label-habilitado";
        private const string NOME_INPUT_HABILITADO = "input-habilitado";
        private readonly Toggle campoHabilitado;

        private const string NOME_LABEL_CONTEUDO_TEXTO = "label-texto";
        private const string NOME_INPUT_CONTEUDO_TEXTO = "input-texto";
        private readonly TextField campoConteudoTexto;

        private const string NOME_LABEL_TAMANHO_TEXTO = "label-tamanho-texto";
        private const string NOME_INPUT_TAMANHO_TEXTO = "input-tamanho-texto";
        private readonly FloatField campoTamanhoTexto;

        private const string NOME_LABEL_NEGRITIO = "label-negrito";
        private const string NOME_INPUT_NEGRITO= "input-negrito";
        private readonly Toggle campoNegrito;

        private const string NOME_LABEL_ITALICO = "label-italico";
        private const string NOME_INPUT_ITALICO = "input-italico";
        private readonly Toggle campoItalico;

        private const string NOME_LABEL_SUBLINHADO = "label-sublinhado";
        private const string NOME_INPUT_SUBLINHADO = "input-sublinhado";
        private readonly Toggle campoSublinhado;

        private const string NOME_LABEL_COR = "label-cor";
        private const string NOME_INPUT_COR = "input-cor";
        private readonly ColorField campoCor;

        #endregion

        private Texto componenteTexto;
        private TextMeshProUGUI componenteTextMesh;

        public InputsComponenteTexto() {
            ImportarTemplate("ElementosUI/InputsComponentes/InputsComponenteTexto/InputsComponenteTextoTemplate.uxml");
            ImportarStyle("ElementosUI/InputsComponentes/InputsComponenteTexto/InputsComponenteTextoStyle.uss");

            campoHabilitado = Root.Query<Toggle>(NOME_INPUT_HABILITADO);
            campoConteudoTexto = Root.Query<TextField>(NOME_INPUT_CONTEUDO_TEXTO);
            campoTamanhoTexto = Root.Query<FloatField>(NOME_INPUT_TAMANHO_TEXTO);
            campoNegrito = Root.Query<Toggle>(NOME_INPUT_NEGRITO);
            campoItalico = Root.Query<Toggle>(NOME_INPUT_ITALICO);
            campoSublinhado = Root.Query<Toggle>(NOME_INPUT_SUBLINHADO);
            campoCor = Root.Query<ColorField>(NOME_INPUT_COR);

            ConfigurarHabilitado();
            ConfigurarConteudoTexto();
            ConfigurarTamanhoTexto();
            ConfigurarNegrito();
            ConfigurarItalico();
            ConfigurarSublinhado();
            ConfigurarCor();

            return;
        }

        private void ConfigurarHabilitado() {
            CampoHabilitado.labelElement.name = NOME_LABEL_HABILITADO;
            CampoHabilitado.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoHabilitado.SetValueWithoutNotify(false);

            return;
        }

        private void ConfigurarConteudoTexto() {
            CampoConteudoTexto.labelElement.name = NOME_LABEL_CONTEUDO_TEXTO;
            CampoConteudoTexto.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoConteudoTexto.SetValueWithoutNotify("Texto");

            return;
        }

        private void ConfigurarTamanhoTexto() {
            CampoTamanhoTexto.labelElement.name = NOME_LABEL_TAMANHO_TEXTO;
            CampoTamanhoTexto.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoTamanhoTexto.SetValueWithoutNotify(1f);

            return;
        }

        private void ConfigurarNegrito() {
            CampoNegrito.labelElement.name = NOME_LABEL_NEGRITIO;
            CampoNegrito.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoNegrito.SetValueWithoutNotify(false);

            return;
        }

        private void ConfigurarItalico() {
            CampoItalico.labelElement.name = NOME_LABEL_ITALICO;
            CampoItalico.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoItalico.SetValueWithoutNotify(false);

            return;
        }

        private void ConfigurarSublinhado() {
            CampoSublinhado.labelElement.name = NOME_LABEL_SUBLINHADO;
            CampoSublinhado.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoSublinhado.SetValueWithoutNotify(false);

            return;
        }

        private void ConfigurarCor() {
            CampoCor.labelElement.name = NOME_LABEL_COR;
            CampoCor.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            CampoCor.SetValueWithoutNotify(Color.white);

            return;
        }

        public void VincularDados(Texto componente) {
            componenteTexto = componente;
            componenteTextMesh = componenteTexto.TextMesh;

            CampoHabilitado.SetValueWithoutNotify(componenteTexto.Canvas.gameObject.activeInHierarchy);
            CampoConteudoTexto.SetValueWithoutNotify(componenteTextMesh.text);
            CampoTamanhoTexto.SetValueWithoutNotify(componenteTextMesh.fontSize);
            CampoNegrito.SetValueWithoutNotify((componenteTextMesh.fontStyle & FontStyles.Bold) != 0);
            CampoItalico.SetValueWithoutNotify((componenteTextMesh.fontStyle & FontStyles.Italic) != 0);
            CampoSublinhado.SetValueWithoutNotify((componenteTextMesh.fontStyle & FontStyles.Underline) != 0);
            CampoCor.SetValueWithoutNotify(componenteTextMesh.color);

            CampoHabilitado.RegisterCallback<ChangeEvent<bool>>(evt => {
                componenteTexto.Canvas.gameObject.SetActive(CampoHabilitado.value);
                // TODO: Ocultar/exibir outros campos
            });

            CampoConteudoTexto.RegisterCallback<ChangeEvent<string>>(evt => {
                componenteTextMesh.text = CampoConteudoTexto.value;
            });

            CampoTamanhoTexto.RegisterCallback<ChangeEvent<float>>(evt => {
                if(evt.newValue < 0) {
                    CampoTamanhoTexto.value = 0;
                }

                componenteTextMesh.fontSize = CampoTamanhoTexto.value;
            });

            CampoNegrito.RegisterCallback<ChangeEvent<bool>>(evt => {
                if(CampoNegrito.value) {
                    componenteTextMesh.fontStyle |= FontStyles.Bold;
                }
                else {
                    componenteTextMesh.fontStyle ^= FontStyles.Bold;
                }
            });

            CampoItalico.RegisterCallback<ChangeEvent<bool>>(evt => {
                if(CampoItalico.value) {
                    componenteTextMesh.fontStyle |= FontStyles.Italic;
                } 
                else {
                    componenteTextMesh.fontStyle ^= FontStyles.Italic;
                }
            });

            CampoSublinhado.RegisterCallback<ChangeEvent<bool>>(evt => {
                if(CampoSublinhado.value) {
                    componenteTextMesh.fontStyle |= FontStyles.Underline;
                } 
                else {
                    componenteTextMesh.fontStyle ^= FontStyles.Underline;
                }
            });

            CampoCor.RegisterCallback<ChangeEvent<Color>>(evt => {
                componenteTextMesh.color = CampoCor.value;
            });

            return;
        }

        public void ReiniciarCampos() {
            componenteTexto = null;

            CampoHabilitado.SetValueWithoutNotify(false);
            CampoConteudoTexto.SetValueWithoutNotify("Texto");
            CampoTamanhoTexto.SetValueWithoutNotify(1f);
            CampoNegrito.SetValueWithoutNotify(false);
            CampoItalico.SetValueWithoutNotify(false);
            CampoSublinhado.SetValueWithoutNotify(false);
            CampoCor.SetValueWithoutNotify(Color.white);

            return;
        }
    }
}