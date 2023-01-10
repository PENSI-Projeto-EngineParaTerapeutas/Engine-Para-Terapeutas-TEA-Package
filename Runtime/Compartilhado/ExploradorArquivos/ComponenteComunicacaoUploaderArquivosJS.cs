using UnityEngine;

namespace EngineParaTerapeutas.Utils {
    public class ComponenteComunicacaoUploaderArquivosJS : MonoBehaviour {
        public void Start() {
            DontDestroyOnLoad(gameObject);
            return;
        }

        public void ReceberCallbackSelecaoArquivoJS(string caminho) {
            AdaptadorExploradorArquivosJS.IniciarUpload(caminho);
            return;
        }
    }
}
