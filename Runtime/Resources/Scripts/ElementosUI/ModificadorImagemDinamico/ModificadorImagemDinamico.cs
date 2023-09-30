using System;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using Autis.Runtime.Utils;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Autis.Runtime.UI {
    public class ModificadorImagemDinamico : ElementoInterfaceJogo {
        protected override string CaminhoTemplate => "Scripts/ElementosUI/ModificadorImagemDinamico/ModificadorImagemDinamicoTemplate";
        protected override string CaminhoStyle => "Scripts/ElementosUI/ModificadorImagemDinamico/ModificadorImagemDinamicoStyle";

        private readonly Action<Texture2D> handleLoadTexture;

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

        public ModificadorImagemDinamico(Sprite imagemAtualAtor, Action<Texture2D> handleLoadTexture) {
            this.handleLoadTexture = handleLoadTexture;

            labelPreviewImagem = Root.Query<Label>(NOME_LABEL_PREVIEW_IMAGEM);
            previewImagem = Root.Query<Image>(NOME_PREVIEW_IMAGEM);
            botaoAlterarImagem = Root.Query<Button>(NOME_BOTAO_ALTERAR_IMAGEM);

            previewImagem.sprite = imagemAtualAtor;
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

            return;
        }

        #if UNITY_EDITOR
        private void HandleAlterarImagemClickEditor() {
            string caminhoAqruivoSelecionado = EditorUtility.OpenFilePanel("Procurar Imagem", string.Empty, "png,jpg,jpeg");
            if(string.IsNullOrWhiteSpace(caminhoAqruivoSelecionado)) {
                Debug.Log("[LOG]: Nenhum arquivo selecionado");
                return;
            }

            byte[] imagemBytes = File.ReadAllBytes(caminhoAqruivoSelecionado);

            Texture2D texture = new(0, 0);
            texture.LoadImage(imagemBytes);

            HandleCarregamentoImagemCompleto(texture);
            return;
        }
        #endif

        private void HandleAlterarImagemClick() {
            ExploradorArquivos.SelecionarSprite(HandleCarregamentoImagemCompleto);
            return;
        }

        private void HandleCarregamentoImagemCompleto(Texture2D texture) {
            previewImagem.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            handleLoadTexture.Invoke(texture);

            return;
        }
    }
}