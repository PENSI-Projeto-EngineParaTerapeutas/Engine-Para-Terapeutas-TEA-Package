using UnityEngine;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Runtime.Constantes;

namespace Autis.Editor.Manipuladores {
    public class ManipuladorContexto : ManipuladorObjetos {
        #region .: Mensagens :.

        protected const string MENSAGEM_ERRO_CONTEXTO_NAO_ENCONTRADO = "[ERROR]: Não foi possível carregar o GameObject de Contexto. Garanta que ele existe na hierarquia do projeto.";

        #endregion

        #region .: Componentes :.

        public AudioSource ComponenteAudioSource { get => componenteAudioSource; }
        private AudioSource componenteAudioSource;

        public SpriteRenderer ComponenteSpriteRenderer { get => componenteSpriteRenderer; }
        private SpriteRenderer componenteSpriteRenderer;

        public Video ComponenteVideo { get => componenteVideo; }
        private Video componenteVideo;

        public ListenerContexto ComponenteListenerContexto { get => componenteListenerContexto; }
        private ListenerContexto componenteListenerContexto;

        #endregion

        #region .: Manipuladores Componentes :.

        public ManipuladorAudioSource ManipuladorAudioSource { get => manipuladorAudioSource; }
        private ManipuladorAudioSource manipuladorAudioSource;

        public ManipuladorSpriteRenderer ManipuladorSpriteRenderer { get => manipuladorSpriteRenderer; }
        private ManipuladorSpriteRenderer manipuladorSpriteRenderer;

        public ManipuladorVideo ManipuladorVideo { get => manipuladorVideo; }
        private ManipuladorVideo manipuladorVideo;

        #endregion

        public ManipuladorContexto() {
            EncontrarObjetoContexto();
            return;
        }

        protected virtual void EncontrarObjetoContexto() {
            objeto = GameObject.FindGameObjectWithTag(NomesTags.Contexto);

            if(objeto == null) {
                Debug.LogError(MENSAGEM_ERRO_CONTEXTO_NAO_ENCONTRADO);
            }

            CarregarComponentes();
            return;
        }

        private void CarregarComponentes() {
            componenteAudioSource = objeto.GetComponent<AudioSource>();
            componenteSpriteRenderer = objeto.GetComponent<SpriteRenderer>();
            componenteVideo = objeto.GetComponent<Video>();
            componenteListenerContexto = objeto.GetComponent<ListenerContexto>();

            manipuladorAudioSource = new ManipuladorAudioSource(componenteAudioSource);
            manipuladorSpriteRenderer = new ManipuladorSpriteRenderer(componenteSpriteRenderer);
            manipuladorVideo = new ManipuladorVideo(componenteVideo);

            return;
        }

        public override void Criar() {
            EncontrarObjetoContexto();
            return;
        }

        public override void Editar(GameObject objetoAlvo) {
            if(!objetoAlvo.CompareTag(NomesTags.Contexto)) {
                return;
            }

            objeto = objetoAlvo;
            CarregarComponentes();

            return;
        }

        protected override void FinalizarInterno() {
            return;
        }

        public void SetNomeArquivoVideo(string nomeArquivoVideo) {
            componenteVideo.nomeArquivoVideo = nomeArquivoVideo;
            componenteListenerContexto.nomeArquivoVideoContexto = nomeArquivoVideo;

            return;
        }

        public string GetNomeArquivoVideo() {
            return componenteVideo.nomeArquivoVideo;
        }
    }
}