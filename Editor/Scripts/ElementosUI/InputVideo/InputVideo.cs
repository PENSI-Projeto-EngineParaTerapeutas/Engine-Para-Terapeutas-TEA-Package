using System.IO;
using UnityEditor;
using UnityEngine;
using Autis.Editor.Utils;
using UnityEngine.UIElements;
using Autis.Editor.Constantes;
using Autis.Runtime.Constantes;

namespace Autis.Editor.UI {
    public class InputVideo : ElementoInterfaceEditor, IReiniciavel, IVinculavel<string> {
        protected override string CaminhoTemplate => "ElementosUI/InputVideo/InputVideoTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputVideo/InputVideoStyle.uss";

        private const string LABEL_TEXT = "Selecionar vídeo";

        #region .: Elementos :.
        public TextField CampoVideo { get => campoVideo; }
        public Label LabelBotao { get => labelBotao; }
        public Button BotaoBuscarVideo { get => botaoBuscarVideo; }
        public Button BotaoCancelarVideo { get => botaoCancelarVideo; }

        private const string NOME_INPUT_VIDEO = "input-video";
        private readonly TextField campoVideo;

        private const string NOME_BOTAO_BUSCAR_VIDEO = "buscar-video";
        private readonly Button botaoBuscarVideo;

        private const string NOME_LABEL_BOTAO = "label-botao";
        private readonly Label labelBotao;

        private const string NOME_ICONE_ARQUIVO_VIDEO = "imagem-icone-arquivo-video";
        private Image iconeArquivoVideo;

        private const string NOME_BOTAO_CANCELAR_VIDEO = "cancelar-video";
        private readonly Button botaoCancelarVideo;

        private const string NOME_ICONE_FECHAR = "imagem-icone-fechar";
        private Image iconeFechar;

        #endregion

        public InputVideo() {
            campoVideo = Root.Query<TextField>(NOME_INPUT_VIDEO);
            botaoBuscarVideo = Root.Query<Button>(NOME_BOTAO_BUSCAR_VIDEO);
            botaoCancelarVideo = Root.Query<Button>(NOME_BOTAO_CANCELAR_VIDEO);
            labelBotao = botaoBuscarVideo.Query<Label>(NOME_LABEL_BOTAO);

            ConfigurarCampoVideo();
            ConfigurarIconeArquivoVideo();
            ConfigurarBotaoBuscarVideo();
            ConfigurarIconeFechar();
            ConfigurarBotaoCancelarVideo();

            return;
        }

        private void ConfigurarCampoVideo() {
            CampoVideo.isReadOnly = true;
            CampoVideo.SetValueWithoutNotify(string.Empty);
            CampoVideo.style.display = DisplayStyle.None;
            return;
        }

        private void ConfigurarIconeArquivoVideo() {
            iconeArquivoVideo = root.Query<Image>(NOME_ICONE_ARQUIVO_VIDEO);
            iconeArquivoVideo.image = Importador.ImportarImagem("video-file.png");

            return;
        }

        private void ConfigurarIconeFechar() {
            iconeFechar = root.Query<Image>(NOME_ICONE_FECHAR);
            iconeFechar.image = Importador.ImportarImagem("fechar.png");

            return;
        }

        private void ConfigurarBotaoBuscarVideo() {
            LabelBotao.name = NOME_LABEL_BOTAO;
            LabelBotao.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            LabelBotao.text = LABEL_TEXT;
            botaoBuscarVideo.clicked += HandleBotaoBuscarVideoClick;
            return;
        }

        private void ConfigurarBotaoCancelarVideo() {
            botaoCancelarVideo.clicked += HandleBotaoCancelarVideoClick;
            botaoCancelarVideo.style.display = DisplayStyle.None;
            return;
        }

        private void HandleBotaoBuscarVideoClick() {
            string caminhoAqruivoSelecionado = EditorUtility.OpenFilePanel("Procurar Video", Path.Combine(ConstantesProjeto.CaminhoDinamicoPacote, "Runtime/Assets/Imagens"), ExtensoesEditor.Video);

            if (string.IsNullOrWhiteSpace(caminhoAqruivoSelecionado)) {
                Debug.Log("[LOG]: Nenhum arquivo selecionado");
                return;
            }

            CopiarArquivoSeNaoExistir(caminhoAqruivoSelecionado);

            string nomeArquivo = Path.GetFileName(caminhoAqruivoSelecionado);

            CampoVideo.value = nomeArquivo;
            CampoVideo.SendEvent(new ChangeEvent<string>());

            AlterarEstadoVideoCarregado();
            return;
        }

        private void AlterarEstadoVideoCarregado() {
            LabelBotao.text = CampoVideo.value;
            iconeArquivoVideo.style.display = DisplayStyle.None;
            botaoCancelarVideo.style.display = DisplayStyle.Flex;
            botaoBuscarVideo.style.justifyContent = Justify.SpaceBetween;

            return;
        }

        private void HandleBotaoCancelarVideoClick() {
            CampoVideo.value = null;
            CampoVideo.SendEvent(new ChangeEvent<Object>());

            AlterarEstadoVideoNaoCarregado();
            return;
        }

        private void AlterarEstadoVideoNaoCarregado() {
            LabelBotao.text = LABEL_TEXT;
            iconeArquivoVideo.style.display = DisplayStyle.Flex;
            botaoCancelarVideo.style.display = DisplayStyle.None;
            botaoBuscarVideo.style.justifyContent = Justify.Center;

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

        public void VincularDados(string nomeVideo) {
            if(string.IsNullOrEmpty(nomeVideo)) {
                CampoVideo.SetValueWithoutNotify(string.Empty);
                AlterarEstadoVideoNaoCarregado();

                return;
            }

            campoVideo.SetValueWithoutNotify(nomeVideo);
            AlterarEstadoVideoCarregado();

            return;
        }
    }
}