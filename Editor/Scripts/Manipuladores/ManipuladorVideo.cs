using UnityEngine;
using UnityEngine.Video;
using Autis.Runtime.ComponentesGameObjects;

namespace Autis.Editor.Manipuladores {
    public class ManipuladorVideo {
        #region .: Componentes :.

        public Video ComponenteVideo { get => componenteVideo; set {  componenteVideo = value; } }
        protected Video componenteVideo;

        public VideoPlayer ComponenteVideoPlayer { get => componenteVideo.Player; }

        public AudioSource ComponenteAudioSource { get => componenteVideo.PlayerAudio; }

        #endregion

        public ManipuladorVideo(Video componenteVideo) { 
            this.componenteVideo = componenteVideo;
            return;
        }

        public void SetVideo(string caminho) {
            if(componenteVideo == null) {
                return;
            }

            componenteVideo.nomeArquivoVideo = caminho;
            return;
        }

        public string GetVideo() {
            return componenteVideo.nomeArquivoVideo;
        }

        public void SetVolume(float volume) {
            if(componenteVideo == null) {
                return;
            }

            ComponenteAudioSource.volume = volume;
            return;
        }

        public float GetVolume() {
            return ComponenteAudioSource.volume;
        }
    }
}