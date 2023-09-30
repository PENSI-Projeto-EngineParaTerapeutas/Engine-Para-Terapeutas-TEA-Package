using UnityEngine;

namespace Autis.Editor.Manipuladores {
    public class ManipuladorAudioSource {
        #region .: Componentes :.

        public AudioSource ComponenteAudioSource { get => componenteAudioSource; set { componenteAudioSource = value; } }
        protected AudioSource componenteAudioSource;

        #endregion

        public ManipuladorAudioSource(AudioSource componenteAudioSource) {
            this.componenteAudioSource = componenteAudioSource;
            return;
        }

        public void SetVolume(float volume) {
            if(componenteAudioSource == null) {
                return;
            }

            if(volume < 0) {
                componenteAudioSource.volume = 0;
                return;
            }

            componenteAudioSource.volume = volume;
            return;
        }

        public float GetVolume() {
            return componenteAudioSource.volume;
        }

        public void SetAudioClip(AudioClip clip) {
            if(componenteAudioSource == null) {
                return;
            }

            componenteAudioSource.clip = clip;
            return;
        }

        public AudioClip GetAudioClip() {
            return componenteAudioSource.clip;
        }
    }
}