using UnityEngine;
using UnityEngine.Video;
using Autis.Editor.Utils;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Runtime.Constantes;
using Autis.Runtime.DTOs;

namespace Autis.Editor.Manipuladores {
    public class ManipuladorInstrucoes : ManipuladorObjetos, IExcluir {
        private const string CAMINHO_PREFAB_INSTRUCAO = "Instrucao/Instrucao.prefab";

        #region .: Componentes :.

        public AudioSource ComponenteAudioSource { get => componenteAudioSource; }
        private AudioSource componenteAudioSource;

        public SpriteRenderer ComponenteSpriteRenderer { get => componenteSpriteRenderer; }
        private SpriteRenderer componenteSpriteRenderer;

        public Texto ComponenteTexto { get => componenteTexto; }
        private Texto componenteTexto;

        public VideoPlayer ComponenteVideoPlayer { get => componenteVideoPlayer; }
        private VideoPlayer componenteVideoPlayer;

        public Video ComponenteVideo { get => componenteVideo; }
        private Video componenteVideo;

        #endregion

        #region .: Manipuladores Componentes :.

        public ManipuladorAudioSource ManipuladorAudioSource { get => manipuladorAudioSource; }
        private ManipuladorAudioSource manipuladorAudioSource;

        public ManipuladorSpriteRenderer ManipuladorSpriteRenderer { get => manipuladorSpriteRenderer; }
        private ManipuladorSpriteRenderer manipuladorSpriteRenderer;

        public ManipuladorTexto ManipuladorTexto { get => manipuladorTexto; }
        private ManipuladorTexto manipuladorTexto;

        public ManipuladorVideo ManipuladorVideo { get => manipuladorVideo; }
        private ManipuladorVideo manipuladorVideo;

        #endregion

        public ManipuladorInstrucoes() {
            prefabObjeto = Importador.ImportarPrefab(CAMINHO_PREFAB_INSTRUCAO);
            return;
        }

        public ManipuladorInstrucoes(GameObject prefabAtor) : base(prefabAtor) {}

        public override void Editar(GameObject objetoAlvo) {
            base.Editar(objetoAlvo);

            componenteAudioSource = objeto.GetComponent<AudioSource>();
            componenteSpriteRenderer = objeto.GetComponent<SpriteRenderer>();
            componenteTexto = objeto.GetComponent<Texto>();
            componenteVideoPlayer = objeto.GetComponent<VideoPlayer>();
            componenteVideo = objeto.GetComponent<Video>();

            manipuladorAudioSource = new ManipuladorAudioSource(componenteAudioSource);
            manipuladorSpriteRenderer = new ManipuladorSpriteRenderer(componenteSpriteRenderer);
            manipuladorTexto = new ManipuladorTexto(componenteTexto);
            manipuladorVideo = new ManipuladorVideo(componenteVideo);

            componenteSpriteRenderer.sortingOrder = OrdemRenderizacao.EmCriacao;

            return;
        }

        protected override void FinalizarInterno() {
            objeto.tag = NomesTags.Instrucoes;
            objeto.layer = LayersProjeto.Default.Index;
            componenteSpriteRenderer.sortingOrder = OrdemRenderizacao.Instrucao;

            RemoverVinculo();

            return;
        }

        public void Excluir() {
            GameObject.DestroyImmediate(objeto);
            RemoverVinculo();

            return;
        }

        protected void RemoverVinculo() {
            objeto = null;

            componenteAudioSource = null;
            componenteSpriteRenderer = null;
            componenteTexto = null;
            componenteVideoPlayer = null;
            componenteVideo = null;

            manipuladorAudioSource = null;
            manipuladorSpriteRenderer = null;
            manipuladorTexto = null;
            manipuladorVideo = null;

            return;
        }

        public void AlterarTipo(TiposIntrucoes tipo) {
            if(objeto == null) {
                return;
            }

            DesabilitarComponentes();

            switch(tipo) {
                case(TiposIntrucoes.Audio): {
                    HabilitarComponentesAudio();
                    break;
                }
                case(TiposIntrucoes.Texto): {
                    HabilitarComponentesTexto();
                    break;
                }
                case(TiposIntrucoes.Video): {
                    HabilitarComponentesVideo();
                    break;
                }
            }

            return;
        }

        public void DesabilitarComponentes() {
            if(objeto == null) {
                return;
            }

            componenteAudioSource.enabled = false;
            componenteSpriteRenderer.enabled = false;
            componenteTexto.Habilitado = false;
            componenteVideo.Habilitado = false;

            return;
        }

        private void HabilitarComponentesTexto() {
            componenteTexto.Habilitado = true;
            return;
        }

        private void HabilitarComponentesAudio() {
            componenteAudioSource.enabled = true;
            objeto.transform.position = POSICAO_PADRAO_ATOR_TIPO_AUDIO;

            return;
        }

        private void HabilitarComponentesVideo() {
            componenteVideo.Habilitado = true;
            return;
        }

        public TiposIntrucoes GetTipo() {
            if(componenteVideo.Habilitado) {
                return TiposIntrucoes.Video;
            }

            if(componenteTexto.Habilitado) {
                return TiposIntrucoes.Texto;
            }

            return TiposIntrucoes.Audio;
        }
    }
}