using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Autis.Editor.Constantes;
using Autis.Runtime.Constantes;

namespace Autis.Editor.UI {
    public class InputVideo : ElementoInterfaceEditor, IReiniciavel {
        protected override string CaminhoTemplate => "ElementosUI/InputVideo/InputVideoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputVideo/InputVideoStyle.uss";

        #region .: Elementos :.
        public TextField CampoVideo { get => campoVideo; }
        public Label LabelCampoVideo { get => campoVideo.labelElement; }
        public Button BotaoBuscarVideo { get => botaoBuscarVideo; }

        private const string NOME_LABEL_VIDEO = "label-video";
        private const string NOME_INPUT_VIDEO = "input-video";
        private readonly TextField campoVideo;

        private const string NOME_BOTAO_BUSCAR_VIDEO = "buscar-video";
        private readonly Button botaoBuscarVideo;

        #endregion

        public InputVideo() {
            campoVideo = Root.Query<TextField>(NOME_INPUT_VIDEO);
            botaoBuscarVideo = Root.Query<Button>(NOME_BOTAO_BUSCAR_VIDEO);

            ConfigurarCampoVideo();
            ConfigurarBotaoBuscarVideo();

            return;
        }

        private void ConfigurarCampoVideo() {
            LabelCampoVideo.name = NOME_LABEL_VIDEO;
            LabelCampoVideo.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            CampoVideo.isReadOnly = true;
            CampoVideo.SetValueWithoutNotify(string.Empty);
            return;
        }

        private void ConfigurarBotaoBuscarVideo() {
            botaoBuscarVideo.clicked += HandleBotaoBuscarVideoClick;
            return;
        }

        private void HandleBotaoBuscarVideoClick() {
            string caminhoAqruivoSelecionado = EditorUtility.OpenFilePanel("Procurar Video", string.Empty, ExtensoesEditor.Video);

            if (string.IsNullOrWhiteSpace(caminhoAqruivoSelecionado)) {
                Debug.Log("[LOG]: Nenhum arquivo selecionado");
                return;
            }

            CopiarArquivoSeNaoExistir(caminhoAqruivoSelecionado);

            string nomeArquivo = Path.GetFileName(caminhoAqruivoSelecionado);

            CampoVideo.value = nomeArquivo;
            CampoVideo.SendEvent(new ChangeEvent<string>());

            return;
        }

        private void CopiarArquivoSeNaoExistir(string caminho) {
            if(!Directory.Exists(ConstantesProjetoUnity.CaminhoUnityAssetsStreamingAssets)) {
                Directory.CreateDirectory(ConstantesProjetoUnity.CaminhoUnityAssetsStreamingAssets);
            }

            string[] arquivos = Directory.GetFiles(ConstantesProjetoUnity.CaminhoUnityAssetsStreamingAssets);
            string nomeArquivo = Path.GetFileName(caminho);

            foreach(string arquivo in arquivos) {
                if(Path.GetFileName(arquivo) == nomeArquivo) {
                    return;
                }
            }

            FileUtil.CopyFileOrDirectory(caminho, Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsStreamingAssets, nomeArquivo));
            AssetDatabase.Refresh();
            return;
        }

        public void ReiniciarCampos() {
            CampoVideo.SetValueWithoutNotify(null);
            return;
        }
    }
}