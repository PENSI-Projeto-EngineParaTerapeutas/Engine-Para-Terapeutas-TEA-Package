using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.Utils;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EngineParaTerapeutas.UI {
    public class ModificadorImagemDinamico : ElementoInterfaceJogo {
        protected override string CaminhoTemplate => "Scripts/ElementosUI/ModificadorImagemDinamico/ModificadorImagemDinamicoTemplate";
        protected override string CaminhoStyle => "Scripts/ElementosUI/ModificadorImagemDinamico/ModificadorImagemDinamicoStyle";

        #region .: Elementos :.

        public Label LabelPreviewImagem { get => labelPreviewImagem; }
        public Image PreviewImagem { get => previewImagem; }
        public Button BotaoAlterarImagem { get => botaoAlterarImagem; }

        private const string NOME_LABEL_PREVIEW_IMAGEM = "label-preview-imagem";
        private readonly Label labelPreviewImagem;

        private const string NOME_PREVIEW_IMAGEM = "preview-imagem";
        private readonly Image previewImagem;

        private const string NOME_BOTAO_ALTERAR_IMAGEM = "botao-alterar-imagem";
        private readonly Button botaoAlterarImagem;

        #endregion

        private readonly SpriteRenderer spriteRenderer;

        public ModificadorImagemDinamico(SpriteRenderer spriteRenderer) {
            this.spriteRenderer = spriteRenderer;

            labelPreviewImagem = Root.Query<Label>(NOME_LABEL_PREVIEW_IMAGEM);
            previewImagem = Root.Query<Image>(NOME_PREVIEW_IMAGEM);
            botaoAlterarImagem = Root.Query<Button>(NOME_BOTAO_ALTERAR_IMAGEM);

            ConfigurarElementos();

            return;
        }

        private void ConfigurarElementos() {
            #if UNITY_EDITOR
            botaoAlterarImagem.clicked += HandleAlterarImagemClickEditor;
            #endif

            #if !UNITY_EDITOR
            botaoAlterarImagem.clicked += HandleAlterarImagemClick;
            #endif

            previewImagem.sprite = spriteRenderer.sprite;
            return;
        }

        #if UNITY_EDITOR
        private void HandleAlterarImagemClickEditor() {
            string caminhoAqruivoSelecionado = EditorUtility.OpenFilePanel("Procurar Imagem", "", "png,jpg,jpeg");
            if (string.IsNullOrWhiteSpace(caminhoAqruivoSelecionado)) {
                Debug.Log("[LOG]: Nenhum arquivo selecionado");
                return;
            }

            byte[] imagemBytes = File.ReadAllBytes(caminhoAqruivoSelecionado);

            Texture2D texture = new(0, 0);
            texture.LoadImage(imagemBytes);
            Sprite imagem = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            spriteRenderer.sprite = imagem;
            previewImagem.sprite = spriteRenderer.sprite;
            return;
        }
        #endif

        private void HandleAlterarImagemClick() {
            ExploradorArquivos.SelecionarSprite(HandleCarregamentoImagemCompleto);
            return;
        }

        private void HandleCarregamentoImagemCompleto(Sprite imagem) {
            spriteRenderer.sprite = imagem;
            previewImagem.sprite = spriteRenderer.sprite;

            return;
        }
    }
}