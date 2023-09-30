using UnityEngine;

namespace Autis.Runtime.ComponentesGameObjects {
    public class AcionadorAudioControleIndireto : MonoBehaviour {
        private AudioSource audioSource;

        private void Awake() {
            audioSource = GetComponent<AudioSource>();
            return;
        }

        public void AcionarAudio(AudioClip clip) {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();

            return;
        }

        public void StopAudio() {
            audioSource.Stop();
            return;
        }
    }
}