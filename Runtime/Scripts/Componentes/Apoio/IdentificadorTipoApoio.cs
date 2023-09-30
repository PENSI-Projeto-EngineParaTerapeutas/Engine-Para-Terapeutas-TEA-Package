using UnityEngine;
using Autis.Runtime.DTOs;

namespace Autis.Runtime.ComponentesGameObjects {
    [AddComponentMenu("AUTIS/Apoio/Identificador Tipo Apoio")]
    public class IdentificadorTipoApoio : IdentificadorTipo<TiposApoios> {
        private SpriteRenderer spriteRenderer;
        private AudioSource audioSource;
        private Video video;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
            video = GetComponent<Video>();

            return;
        }

        #if UNITY_EDITOR
        public override void AlterarTipo(TiposApoios novoTipo) {
            tipo = novoTipo;

            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
            video = GetComponent<Video>();

            DesabilitarComponentes();
            HabilitarComponentes();

            return;
        }
        #endif

        public override void HabilitarComponentes(TiposApoios tipo) {
            switch(tipo) {
                case(TiposApoios.Audio): {
                    audioSource.enabled = true;
                    transform.position = POSICAO_PADRAO_ATOR_AUDIO;
                    break;
                }
                case(TiposApoios.Imagem): {
                    spriteRenderer.enabled = true;
                    break;
                }
                case(TiposApoios.Video): {
                    spriteRenderer.enabled = true;
                    audioSource.enabled = true;
                    
                    video.Player.enabled = true;
                    video.enabled = true;

                    break;
                }
            }

            return;
        }

        public override void DesabilitarComponentes() {
            transform.position = Vector3.zero;

            spriteRenderer.enabled = false;
            audioSource.enabled = false;
            video.Player.enabled = false;
            video.enabled = false;

            return;
        }
    }
}