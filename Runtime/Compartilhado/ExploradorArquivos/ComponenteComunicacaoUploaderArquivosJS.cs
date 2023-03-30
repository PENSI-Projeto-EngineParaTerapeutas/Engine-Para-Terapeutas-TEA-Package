using UnityEngine;

namespace Autis.Runtime.Utils {
    public class ComponenteComunicacaoUploaderArquivosJS : MonoBehaviour {
        private void Start() {
            DontDestroyOnLoad(gameObject);
            return;
        }

        public void ReceberCallbackSelecaoArquivoJS(string caminho) {
            AdaptadorExploradorArquivosJS.IniciarUpload(caminho);
            return;
        }
    }
}
