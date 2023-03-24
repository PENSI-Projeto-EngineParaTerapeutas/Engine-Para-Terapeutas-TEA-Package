using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.UI {
    public class InputImagem : ElementoInterfaceEditor, IReiniciavel {
        protected override string CaminhoTemplate => "ElementosUI/InputImagem/InputImagemTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputImagem/InputImagemStyle.uss";

        #region .: Elementos :.
        public ObjectField CampoImagem { get => campoImagem; }
        public Label LabelCampoImagem { get => campoImagem.labelElement; }
        public Button BotaoBuscarImagem { get => botaoBuscarImagem; }

        private const string NOME_LABEL_IMAGEM = "label-imagem";
        private const string NOME_INPUT_IMAGEM = "input-imagem";
        private readonly ObjectField campoImagem;

        private const string NOME_BOTAO_BUSCAR_IMAGEM = "buscar-imagem";
        private readonly Button botaoBuscarImagem;

        #endregion

        public InputImagem() {
            campoImagem = Root.Query<ObjectField>(NOME_INPUT_IMAGEM);
            botaoBuscarImagem = Root.Query<Button>(NOME_BOTAO_BUSCAR_IMAGEM);

            ConfigurarCampoImagem();
            ConfigurarBotaoBuscarImagem();

            return;
        }

        private void ConfigurarCampoImagem() {
            LabelCampoImagem.name = NOME_LABEL_IMAGEM;
            LabelCampoImagem.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            CampoImagem.objectType = typeof(Sprite);
            CampoImagem.SetValueWithoutNotify(null);
            return;
        }

        private void ConfigurarBotaoBuscarImagem() {
            BotaoBuscarImagem.clicked += HandleBotaoBuscarImagemClick;
            return;
        }

        private void HandleBotaoBuscarImagemClick() {
            string caminhoAqruivoSelecionado = EditorUtility.OpenFilePanel("Procurar Imagem", string.Empty, ExtensoesEditor.Imagem);

            if(string.IsNullOrWhiteSpace(caminhoAqruivoSelecionado)) {
                Debug.Log("[LOG]: Nenhum arquivo selecionado");
                return;
            }

            CopiarArquivoSeNaoExistir(caminhoAqruivoSelecionado);

            string nomeArquivo = Path.GetFileName(caminhoAqruivoSelecionado);
            Sprite imagemCarregada = AssetDatabase.LoadAssetAtPath<Sprite>(Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsImagens, nomeArquivo));

            CampoImagem.value = imagemCarregada;
            CampoImagem.SendEvent(new ChangeEvent<Object>());
            return;
        }

        private void CopiarArquivoSeNaoExistir(string caminho) {
            if(!Directory.Exists(ConstantesProjetoUnity.CaminhoUnityAssetsImagens)) {
                Directory.CreateDirectory(ConstantesProjetoUnity.CaminhoUnityAssetsImagens);
            }

            string[] arquivos = Directory.GetFiles(ConstantesProjetoUnity.CaminhoUnityAssetsImagens);
            string nomeArquivo = Path.GetFileName(caminho);

            foreach(string arquivo in arquivos) {
                if(Path.GetFileName(arquivo) == nomeArquivo) {
                    return;
                }
            }

            FileUtil.CopyFileOrDirectory(caminho, Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsImagens, nomeArquivo));
            AssetDatabase.Refresh();
            return;
        }

        public void ReiniciarCampos() {
            CampoImagem.SetValueWithoutNotify(null);
            return;
        }
    }
}