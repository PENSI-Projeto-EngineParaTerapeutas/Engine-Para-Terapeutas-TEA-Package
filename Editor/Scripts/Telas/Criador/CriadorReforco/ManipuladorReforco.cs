using UnityEngine;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Editor.Utils;
using Autis.Runtime.Constantes;
using Autis.Runtime.DTOs;

namespace Autis.Editor.Manipuladores {
    public class ManipuladorReforco : ManipuladorObjetos, IExcluir {
        private const string CAMINHO_PREFAB_REFORCO = "Reforcos/Reforco.prefab";

        #region .: Componentes :.

        public AudioSource ComponenteAudioSource { get => componenteAudioSource; }
        private AudioSource componenteAudioSource;

        public SpriteRenderer ComponenteSpriteRenderer { get => componenteSpriteRenderer; }
        private SpriteRenderer componenteSpriteRenderer;

        public Texto ComponenteTexto { get => componenteTexto; }
        private Texto componenteTexto;

        public Video ComponenteVideo { get => componenteVideo; }
        private Video componenteVideo;

        public ListenerEventosReforco ListenerEventos { get => listenerEventos; }
        private ListenerEventosReforco listenerEventos;

        public IdentificadorTipoReforco ComponenteIdentificadorTipoReforco { get => componenteIdentificadorTipoReforco; }
        private IdentificadorTipoReforco componenteIdentificadorTipoReforco;

        #endregion

        #region .: Manipuladores :.

        public ManipuladorAudioSource ManipuladorComponenteAudioSource { get => manipuladorComponenteAudioSource; }
        private ManipuladorAudioSource manipuladorComponenteAudioSource;

        public ManipuladorSpriteRenderer ManipuladorComponenteSpriteRenderer { get => manipuladorComponenteSpriteRenderer; }
        private ManipuladorSpriteRenderer manipuladorComponenteSpriteRenderer;

        public ManipuladorTexto ManipuladorComponenteTexto { get => manipuladorComponenteTexto; }
        private ManipuladorTexto manipuladorComponenteTexto;

        public ManipuladorVideo ManipuladorComponenteVideo { get => manipuladorComponenteVideo; }
        private ManipuladorVideo manipuladorComponenteVideo;

        #endregion

        public ManipuladorReforco() {
            prefabObjeto = Importador.ImportarPrefab(CAMINHO_PREFAB_REFORCO);
            return;
        }

        public override void Editar(GameObject objetoAlvo) {
            base.Editar(objetoAlvo);

            componenteAudioSource = objeto.GetComponent<AudioSource>();
            componenteSpriteRenderer = objeto.GetComponent<SpriteRenderer>();
            componenteTexto = objeto.GetComponent<Texto>();
            componenteVideo = objeto.GetComponent<Video>();
            listenerEventos = objeto.GetComponent<ListenerEventosReforco>();
            componenteIdentificadorTipoReforco = objeto.GetComponent<IdentificadorTipoReforco>();

            manipuladorComponenteAudioSource = new ManipuladorAudioSource(componenteAudioSource);
            manipuladorComponenteSpriteRenderer = new ManipuladorSpriteRenderer(componenteSpriteRenderer);
            manipuladorComponenteTexto = new ManipuladorTexto(componenteTexto);
            manipuladorComponenteVideo = new ManipuladorVideo(componenteVideo);

            componenteSpriteRenderer.sortingOrder = OrdemRenderizacao.EmCriacao;

            return;
        }

        public void Excluir() {
            GameObject.DestroyImmediate(objeto);
            RemoverVinculo();

            return;
        }

        protected void RemoverVinculo() {
            componenteAudioSource = null;
            componenteSpriteRenderer = null;
            componenteTexto = null;
            componenteVideo = null;
            listenerEventos = null;
            componenteIdentificadorTipoReforco = null;

            manipuladorComponenteAudioSource = null;
            manipuladorComponenteSpriteRenderer = null;
            manipuladorComponenteTexto = null;
            manipuladorComponenteVideo = null;

            return;
        }

        protected override void FinalizarInterno() {
            objeto.tag = NomesTags.Reforcos;
            objeto.layer = LayersProjeto.Default.Index;
            componenteSpriteRenderer.sortingOrder = OrdemRenderizacao.Reforco;

            objeto = null;

            return;
        }

        public void AlterarTipo(TiposReforcos tipo) {
            if(objeto == null) {
                return;
            }

            objeto.transform.position = Vector3.zero;
            DesabilitarComponentes();

            switch(tipo) {
                case(TiposReforcos.Audio): {
                    HabilitarComponentesAudio();
                    break;
                }
                case(TiposReforcos.Imagem): {
                    HabilitarComponentesImagem();
                    break;
                }
                case(TiposReforcos.Texto): {
                    HabilitarComponentesTexto();
                    break;
                }
                case(TiposReforcos.Video): {
                    HabilitarComponentesVideo();
                    break;
                }
            }

            return;
        }

        private void HabilitarComponentesAudio() {
            componenteAudioSource.enabled = true;
            listenerEventos.enabled = true;
            objeto.transform.position = POSICAO_PADRAO_ATOR_TIPO_AUDIO;

            componenteIdentificadorTipoReforco.tipo = TiposReforcos.Audio;

            return;
        }

        private void HabilitarComponentesImagem() {
            componenteSpriteRenderer.enabled = true;
            listenerEventos.enabled = true;

            componenteIdentificadorTipoReforco.tipo = TiposReforcos.Imagem;

            return;
        }

        private void HabilitarComponentesTexto() {
            componenteTexto.Habilitado = true;
            listenerEventos.enabled = true;

            componenteIdentificadorTipoReforco.tipo = TiposReforcos.Texto;

            return;
        }

        private void HabilitarComponentesVideo() {
            componenteVideo.Habilitado = true;
            listenerEventos.enabled = true;

            componenteIdentificadorTipoReforco.tipo= TiposReforcos.Video;

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
            listenerEventos.enabled = false;

            return;
        }

        public TiposReforcos GetTipo() {
            if(componenteVideo.Habilitado) {
                return TiposReforcos.Video;
            }

            if(componenteTexto.Habilitado) {
                return TiposReforcos.Texto;
            }

            if(componenteAudioSource.enabled) {
                return TiposReforcos.Audio;
            }

            return TiposReforcos.Imagem;
        }

        public void SetTipoAcionamento(TipoAcionamentoReforco tipoAcionamento) {
            if(objeto == null) {
                return;
            }

            listenerEventos.TipoAcionamento = tipoAcionamento;
            return;
        }

        public TipoAcionamentoReforco GetTipoAcionamento() {
            return listenerEventos.TipoAcionamento;
        }

        public void SetTempoExibicao(float tempo) {
            componenteIdentificadorTipoReforco.tempoEspera = tempo;
            return;
        }

        public float GetTempoExibicao() {
            return componenteIdentificadorTipoReforco.tempoEspera;
        }
    }
}