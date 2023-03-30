using UnityEngine;
using Autis.Runtime.DTOs;

namespace Autis.Runtime.ComponentesGameObjects {
    [AddComponentMenu("Engine Terapeutas TEA/Identificador Tipo/Identificador Tipo Reforço")]
    public class IdentificadorTipoReforco : IdentificadorTipo<TiposReforcos> {
        private SpriteRenderer spriteRenderer;
        private AudioSource audioSource;
        private Video video;
        private Texto texto;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
            video = GetComponent<Video>();
            texto = GetComponent<Texto>();

            return;
        }

        #if UNITY_EDITOR
        public override void AlterarTipo(TiposReforcos novoTipo) {
            tipo = novoTipo;

            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
            video = GetComponent<Video>();
            texto = GetComponent<Texto>();

            DesabilitarComponentes();
            HabilitarComponentes();
            
            return;
        }
        #endif

        public override void HabilitarComponentes(TiposReforcos tipo) {
            switch(tipo) {
                case(TiposReforcos.Audio): {
                    audioSource.enabled = true;
                    break;
                }
                case(TiposReforcos.Imagem): {
                    spriteRenderer.enabled = true;
                    break;
                }
                case(TiposReforcos.Texto): {
                    texto.enabled = true;
                    texto.Habilitar();

                    break;
                }
                case(TiposReforcos.Video): {
                    spriteRenderer.enabled = true;
                    audioSource.enabled = true;
                    video.enabled = true;
                    video.Player.enabled = true;

                    break;
                }
            }

            return;
        }

        public override void DesabilitarComponentes() {
            spriteRenderer.enabled = false;
            audioSource.enabled = false;

            video.Player.enabled = false;
            video.enabled = false;

            texto.Desabilitar();
            texto.enabled = false;

            return;
        }
    }
}