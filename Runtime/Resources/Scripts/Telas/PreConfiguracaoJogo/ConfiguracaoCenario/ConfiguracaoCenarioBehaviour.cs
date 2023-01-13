using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.Utils;
using EngineParaTerapeutas.Constantes;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EngineParaTerapeutas.UI {
    public class ConfiguracaoCenarioBehaviour : ElementoInterfaceJogo {
        #region .: Elementos :.

        private const string NOME_LABEL_PREVIEW_CENARIO = "label-preview-cenario";
        private readonly Label labelPreviewCenario;

        private const string NOME_PREVIEW_CENARIO = "preview-cenario";
        private readonly Image previewCenario;

        private const string NOME_BOTAO_SELECIONAR_CENARIO = "botao-selecionar-cenario";
        private readonly Button botaoSelecionarCenario;

        #endregion

        private readonly GameObject cenario;
        private readonly SpriteRenderer spriteCenario;

        public ConfiguracaoCenarioBehaviour() {
            cenario = GameObject.FindGameObjectWithTag(NomesTags.Cenario); // TODO: Garantir que um cenário sempre exista
            spriteCenario = cenario.GetComponent<SpriteRenderer>();

            ImportarTemplate("Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoCenario/ConfiguracaoCenarioTemplate");
            ImportarStyle("Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoCenario/ConfiguracaoCenarioStyle");

            labelPreviewCenario = Root.Query<Label>(NOME_LABEL_PREVIEW_CENARIO);
            previewCenario = Root.Query<Image>(NOME_PREVIEW_CENARIO);
            botaoSelecionarCenario = Root.Query<Button>(NOME_BOTAO_SELECIONAR_CENARIO);

            ConfigurarElementos();
            return;
        }

        private void ConfigurarElementos() {
            #if UNITY_EDITOR
            botaoSelecionarCenario.clicked += HandleSelecionarCenarioClickEditor;
            #endif

            #if !UNITY_EDITOR
            botaoSelecionarCenario.clicked += HandleSelecionarCenarioClick;
            #endif

            previewCenario.sprite = spriteCenario.sprite;
            return;
        }

        #if UNITY_EDITOR
        private void HandleSelecionarCenarioClickEditor() {
            string caminhoAqruivoSelecionado = EditorUtility.OpenFilePanel("Procurar Imagem", "", "png,jpg,jpeg");
            if (string.IsNullOrWhiteSpace(caminhoAqruivoSelecionado)) {
                Debug.Log("[LOG]: Nenhum arquivo selecionado");
                return;
            }

            byte[] imagemBytes = File.ReadAllBytes(caminhoAqruivoSelecionado);

            Texture2D texture = new(0, 0);
            texture.LoadImage(imagemBytes);
            Sprite imagem = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            spriteCenario.sprite = imagem;
            previewCenario.sprite = spriteCenario.sprite;
            return;
        }
        #endif

        private void HandleSelecionarCenarioClick() {
            ExploradorArquivos.SelecionarSprite(HandleCarregamentoImagemCompleto);
            return;
        }

        private void HandleCarregamentoImagemCompleto(Sprite imagem) {
            spriteCenario.sprite = imagem;
            previewCenario.sprite = spriteCenario.sprite;

            return;
        }
    }
}