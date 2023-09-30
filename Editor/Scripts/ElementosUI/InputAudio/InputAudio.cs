using System.IO;
using UnityEditor;
using UnityEngine;
using Autis.Editor.Utils;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Autis.Editor.Constantes;
using Autis.Runtime.Constantes;

namespace Autis.Editor.UI {
    public class InputAudio : ElementoInterfaceEditor, IReiniciavel, IVinculavel<AudioClip> {
        protected override string CaminhoTemplate => "ElementosUI/InputAudio/InputAudioTemplate.uxml";
        protected override string CaminhoStyle => "ElementosUI/InputAudio/InputAudioStyle.uss";

        private const string LABEL_TEXT = "Selecionar áudio";

        #region .: Elementos :.
        public ObjectField CampoAudio { get => campoAudio; }
        public Label LabelBotao { get => labelBotao; }
        public Button BotaoBuscarAudio { get => botaoBuscarAudio; }
        public Button BotaoCancelarAudio { get => botaoCancelarAudio; }

        private const string NOME_INPUT_AUDIO = "input-audio";
        private ObjectField campoAudio;

        private const string NOME_LABEL_BOTAO = "label-botao";
        private Label labelBotao;

        private const string NOME_BOTAO_BUSCAR_AUDIO = "buscar-audio";
        private Button botaoBuscarAudio;

        private const string NOME_BOTAO_CANCELAR_AUDIO = "cancelar-audio";
        private Button botaoCancelarAudio;

        private const string NOME_ICONE_ARQUIVO_AUDIO = "imagem-icone-arquivo-audio";
        private Image iconeArquivoAudio;

        private const string NOME_ICONE_FECHAR = "imagem-icone-fechar";
        private Image iconeFechar;

        #endregion

        public InputAudio() {
            ConfigurarCampoAudio();
            ConfigurarIconeArquivoAudio();
            ConfigurarBotaoBuscarAudio();
            ConfigurarIconeFechar();
            ConfigurarBotaoCancelarAudio();

            return;
        }

        private void ConfigurarCampoAudio() {
            campoAudio = Root.Query<ObjectField>(NOME_INPUT_AUDIO);

            CampoAudio.objectType = typeof(AudioClip);
            CampoAudio.SetValueWithoutNotify(null);
            CampoAudio.style.display = DisplayStyle.None;

            return;
        }

        private void ConfigurarIconeArquivoAudio() {
            iconeArquivoAudio = root.Query<Image>(NOME_ICONE_ARQUIVO_AUDIO);
            iconeArquivoAudio.image = Importador.ImportarImagem("audio-file.png");

            return;
        }

        private void ConfigurarIconeFechar() {
            iconeFechar = root.Query<Image>(NOME_ICONE_FECHAR);
            iconeFechar.image = Importador.ImportarImagem("fechar.png");

            return;
        }

        private void ConfigurarBotaoBuscarAudio() {
            botaoBuscarAudio = Root.Query<Button>(NOME_BOTAO_BUSCAR_AUDIO);
            labelBotao = botaoBuscarAudio.Query<Label>(NOME_LABEL_BOTAO);

            LabelBotao.name = NOME_LABEL_BOTAO;
            LabelBotao.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            LabelBotao.text = LABEL_TEXT;
            BotaoBuscarAudio.clicked += HandleBotaoBuscarAudioClick;

            return;
        }

        private void HandleBotaoBuscarAudioClick() {
            string caminhoAqruivoSelecionado = EditorUtility.OpenFilePanel("Procurar Audio", Path.Combine(ConstantesProjeto.CaminhoDinamicoPacote, "Runtime/Assets/Sons"), ExtensoesEditor.Audio);

            if(string.IsNullOrWhiteSpace(caminhoAqruivoSelecionado)) {
                Debug.Log("[LOG]: Nenhum arquivo selecionado");
                return;
            }

            CopiarArquivoSeNaoExistir(caminhoAqruivoSelecionado);

            string nomeArquivo = Path.GetFileName(caminhoAqruivoSelecionado);
            AudioClip audioCarregado = AssetDatabase.LoadAssetAtPath<AudioClip>(Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsSons, nomeArquivo));

            CampoAudio.value = audioCarregado;
            CampoAudio.SendEvent(new ChangeEvent<Object>());

            AlterarEstadoAudioCarregado();
            return;
        }

        private void CopiarArquivoSeNaoExistir(string caminho) {
            if(!Directory.Exists(ConstantesProjetoUnity.CaminhoUnityAssetsSons)) {
                Directory.CreateDirectory(ConstantesProjetoUnity.CaminhoUnityAssetsSons);
            }

            string[] arquivos = Directory.GetFiles(ConstantesProjetoUnity.CaminhoUnityAssetsSons);
            string nomeArquivo = Path.GetFileName(caminho);

            foreach(string arquivo in arquivos) {
                if(Path.GetFileName(arquivo) == nomeArquivo) {
                    return;
                }
            }

            FileUtil.CopyFileOrDirectory(caminho, Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsSons, nomeArquivo));
            AssetDatabase.Refresh();
            return;
        }

        private void AlterarEstadoAudioCarregado() {
            LabelBotao.text = campoAudio.value.name;

            iconeArquivoAudio.style.display = DisplayStyle.None;
            botaoCancelarAudio.style.display = DisplayStyle.Flex;
            botaoBuscarAudio.style.justifyContent = Justify.SpaceBetween;

            return;
        }

        private void ConfigurarBotaoCancelarAudio() {
            botaoCancelarAudio = Root.Query<Button>(NOME_BOTAO_CANCELAR_AUDIO);

            botaoCancelarAudio.clicked += HandleBotaoCancelarAudioClick;
            botaoCancelarAudio.style.display = DisplayStyle.None;

            return;
        }

        private void HandleBotaoCancelarAudioClick() {
            CampoAudio.value = null;
            CampoAudio.SendEvent(new ChangeEvent<Object>());

            AlterarEstadoAudioNaoCarregado();
            return;
        }

        private void AlterarEstadoAudioNaoCarregado() {
            LabelBotao.text = LABEL_TEXT;
            iconeArquivoAudio.style.display = DisplayStyle.Flex;
            botaoCancelarAudio.style.display = DisplayStyle.None;
            botaoBuscarAudio.style.justifyContent = Justify.Center;

            return;
        }

        public void VincularDados(AudioClip clip) {
            if(clip == null) {
                CampoAudio.SetValueWithoutNotify(null);
                AlterarEstadoAudioNaoCarregado();

                return;
            }

            CampoAudio.SetValueWithoutNotify(clip);
            AlterarEstadoAudioCarregado();

            return;
        }

        public void ReiniciarCampos() {
            CampoAudio.SetValueWithoutNotify(null);
            AlterarEstadoAudioNaoCarregado();
            return;
        }
    }
}