using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.UI {
    public class InputAudio : ElementoInterfaceEditor, IReiniciavel {
        private string CaminhoCompletoPastaSons { get => "Assets/Resources/Sons"; }

        #region .: Elementos :.
        public ObjectField CampoAudio { get => campoAudio; }
        public Label LabelCampoAudio { get => campoAudio.labelElement; }
        public Button BotaoBuscarAudio { get => botaoBuscarAudio; }

        private const string NOME_LABEL_AUDIO = "label-audio";
        private const string NOME_INPUT_AUDIO = "input-audio";
        private readonly ObjectField campoAudio;

        private const string NOME_BOTAO_BUSCAR_AUDIO = "buscar-audio";
        private readonly Button botaoBuscarAudio;

        #endregion

        public InputAudio() {
            ImportarTemplate("ElementosUI/InputAudio/InputAudioTemplate.uxml");
            ImportarStyle("ElementosUI/InputAudio/InputAudioStyle.uss");

            campoAudio = Root.Query<ObjectField>(NOME_INPUT_AUDIO);
            botaoBuscarAudio = Root.Query<Button>(NOME_BOTAO_BUSCAR_AUDIO);

            ConfigurarCampoAudio();
            ConfigurarBotaoBuscarAudio();

            return;
        }

        private void ConfigurarCampoAudio() {
            LabelCampoAudio.name = NOME_LABEL_AUDIO;
            LabelCampoAudio.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            CampoAudio.objectType = typeof(AudioClip);
            CampoAudio.SetValueWithoutNotify(null);
            return;
        }

        private void ConfigurarBotaoBuscarAudio() {
            BotaoBuscarAudio.clicked += HandleBotaoBuscarAudioClick;
            return;
        }

        private void HandleBotaoBuscarAudioClick() {
            string caminhoAqruivoSelecionado = EditorUtility.OpenFilePanel("Procurar Audio", "", ConstantesEditor.ExtensoesAudio);

            if(string.IsNullOrWhiteSpace(caminhoAqruivoSelecionado)) {
                Debug.Log("[LOG]: Nenhum arquivo selecionado");
                return;
            }

            CopiarArquivoSeNaoExistir(caminhoAqruivoSelecionado);

            string nomeArquivo = Path.GetFileName(caminhoAqruivoSelecionado);
            AudioClip audioCarregado = AssetDatabase.LoadAssetAtPath<AudioClip>(Path.Combine(CaminhoCompletoPastaSons, nomeArquivo));

            CampoAudio.value = audioCarregado;
            CampoAudio.SendEvent(new ChangeEvent<Object>());
            return;
        }

        private void CopiarArquivoSeNaoExistir(string caminho) {
            if(!Directory.Exists(CaminhoCompletoPastaSons)) {
                Directory.CreateDirectory(CaminhoCompletoPastaSons);
            }

            string[] arquivos = Directory.GetFiles(CaminhoCompletoPastaSons);
            string nomeArquivo = Path.GetFileName(caminho);

            foreach(string arquivo in arquivos) {
                if(Path.GetFileName(arquivo) == nomeArquivo) {
                    return;
                }
            }

            FileUtil.CopyFileOrDirectory(caminho, Path.Combine(CaminhoCompletoPastaSons, nomeArquivo));
            AssetDatabase.Refresh();
            return;
        }

        public void ReiniciarCampos() {
            CampoAudio.SetValueWithoutNotify(null);
            return;
        }
    }
}