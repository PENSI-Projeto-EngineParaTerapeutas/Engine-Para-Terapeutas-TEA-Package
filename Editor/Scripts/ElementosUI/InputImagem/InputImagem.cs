using System.IO;
using UnityEditor;
using UnityEngine;
using Autis.Editor.Utils;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Autis.Editor.Constantes;
using Autis.Runtime.Constantes;

namespace Autis.Editor.UI {
    public class InputImagem : ElementoInterfaceEditor, IReiniciavel, IVinculavel<Sprite>, IEstaVazio {
        protected override string CaminhoTemplate => "ElementosUI/InputImagem/InputImagemTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputImagem/InputImagemStyle.uss";

        private const string LABEL_TEXT = "Selecionar Imagem";
        private const string CLASS_ARQUIVO_SELECIONADO = "arquivo-selecionado";

        #region .: Elementos :.
        public ObjectField CampoImagem { get => campoImagem; }
        public Label LabelBotao { get => labelBotao; }
        public Button BotaoBuscarImagem { get => botaoBuscarImagem; }
        public Button BotaoCancelarImagem { get => botaoCancelarImagem; }

        private const string NOME_LABEL_BOTAO = "label-botao";
        private Label labelBotao;

        private const string NOME_INPUT_IMAGEM = "input-imagem";
        private ObjectField campoImagem;

        private const string NOME_BOTAO_BUSCAR_IMAGEM = "buscar-imagem";
        private Button botaoBuscarImagem;

        private const string NOME_BOTAO_CANCELAR_IMAGEM = "cancelar-imagem";
        private Button botaoCancelarImagem;

        private const string NOME_ICONE_ARQUIVO_IMAGEM = "imagem-icone-arquivo-imagem";
        private Image iconeArquivoImagem;

        private const string NOME_ICONE_FECHAR = "imagem-icone-fechar";
        private Image iconeFechar;

        #endregion

        public InputImagem() {
            ConfigurarCampoImagem();
            ConfigurarIconeArquivoImagem();
            ConfigurarBotaoBuscarImagem();
            ConfigurarIconeFechar();
            ConfigurarBotaoCancelarImagem();

            return;
        }

        private void ConfigurarCampoImagem() {
            campoImagem = Root.Query<ObjectField>(NOME_INPUT_IMAGEM);

            CampoImagem.objectType = typeof(Sprite);
            CampoImagem.SetValueWithoutNotify(null);
            CampoImagem.style.display = DisplayStyle.None;
            return;
        }

        private void ConfigurarIconeArquivoImagem() {
            iconeArquivoImagem = root.Query<Image>(NOME_ICONE_ARQUIVO_IMAGEM);
            iconeArquivoImagem.image = Importador.ImportarImagem("image-file.png");

            return;
        }

        private void ConfigurarIconeFechar() {
            iconeFechar = root.Query<Image>(NOME_ICONE_FECHAR);
            iconeFechar.image = Importador.ImportarImagem("fechar.png");

            return;
        }

        private void ConfigurarBotaoBuscarImagem() {
            botaoBuscarImagem = Root.Query<Button>(NOME_BOTAO_BUSCAR_IMAGEM);
            labelBotao = botaoBuscarImagem.Query<Label>(NOME_LABEL_BOTAO);

            LabelBotao.name = NOME_LABEL_BOTAO;
            LabelBotao.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            LabelBotao.text = LABEL_TEXT;

            BotaoBuscarImagem.clicked += HandleBotaoBuscarImagemClick;

            return;
        }

        private void HandleBotaoBuscarImagemClick() {
            string caminhoAqruivoSelecionado = EditorUtility.OpenFilePanel("Procurar Imagem", Path.Combine(ConstantesProjeto.CaminhoDinamicoPacote, "Runtime/Assets/Imagens"), ExtensoesEditor.Imagem);

            if(string.IsNullOrWhiteSpace(caminhoAqruivoSelecionado)) {
                Debug.Log("[LOG]: Nenhum arquivo selecionado");
                return;
            }

            CopiarArquivoSeNaoExistir(caminhoAqruivoSelecionado);

            string nomeArquivo = Path.GetFileName(caminhoAqruivoSelecionado);
            Sprite imagemCarregada = AssetDatabase.LoadAssetAtPath<Sprite>(Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsImagens, nomeArquivo));

            CampoImagem.value = imagemCarregada;
            CampoImagem.SendEvent(new ChangeEvent<Object>());

            AlterarEstadoImagemCarregada(nomeArquivo);
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

        private void AlterarEstadoImagemCarregada(string label) {
            LabelBotao.text = label;
            LabelBotao.AddToClassList(CLASS_ARQUIVO_SELECIONADO);

            iconeArquivoImagem.style.display = DisplayStyle.None;
            botaoCancelarImagem.style.display = DisplayStyle.Flex;
            botaoBuscarImagem.style.justifyContent = Justify.SpaceBetween;

            return;
        }

        private void ConfigurarBotaoCancelarImagem() {
            botaoCancelarImagem = Root.Query<Button>(NOME_BOTAO_CANCELAR_IMAGEM);
            botaoCancelarImagem.clicked += HandleBotaoCancelarImagemClick;
            botaoCancelarImagem.style.display = DisplayStyle.None;

            return;
        }

        private void HandleBotaoCancelarImagemClick() {
            CampoImagem.value = null;
            CampoImagem.SendEvent(new ChangeEvent<Object>());

            AlterarEstadoImagemNaoCarregada();

            return;
        }

        private void AlterarEstadoImagemNaoCarregada() {
            LabelBotao.text = LABEL_TEXT;
            LabelBotao.RemoveFromClassList(CLASS_ARQUIVO_SELECIONADO);

            iconeArquivoImagem.style.display = DisplayStyle.Flex;
            botaoCancelarImagem.style.display = DisplayStyle.None;
            botaoBuscarImagem.style.justifyContent = Justify.Center;

            return;
        }

        public void ReiniciarCampos() {
            CampoImagem.SetValueWithoutNotify(null);
            AlterarEstadoImagemNaoCarregada();
            return;
        }

        public void VincularDados(Sprite sprite) {
            if(sprite == null) {
                CampoImagem.SetValueWithoutNotify(null);
                AlterarEstadoImagemNaoCarregada();

                return;
            }

            CampoImagem.SetValueWithoutNotify(sprite);

            string nomeSprite = Path.GetFileName(AssetDatabase.GetAssetPath(sprite.GetInstanceID()));
            AlterarEstadoImagemCarregada(nomeSprite);

            return;
        }

        public bool EstaVazio() {
            return CampoImagem.value == null;
        }
    }
}