using UnityEngine;

namespace EngineParaTerapeutas.ComponentesGameObjects {
    [AddComponentMenu("Engine Terapeutas TEA/Selecion�vel")]
    public class Selecionavel : MonoBehaviour {
        private const string NOME_GAME_OBJECT_VIDEO_OBJETO_INTERACAO = "Video";

        public bool habilitado = true;

        private AudioSource audioSource;

        private GameObject gameObjectVideo;
        private Video video;

        private void Awake() {
            audioSource = GetComponent<AudioSource>();
            gameObjectVideo = transform.Find(NOME_GAME_OBJECT_VIDEO_OBJETO_INTERACAO).gameObject;

            return;
        }

        private void Start() {
            video = gameObjectVideo.GetComponent<Video>();
            return;
        }

        private void OnMouseUpAsButton() {
            if(!habilitado) {
                return;
            }

            if(audioSource.clip != null) {
                audioSource.Play();
            }

            if(!string.IsNullOrEmpty(video.nomeArquivoVideo)) {
                gameObjectVideo.SetActive(true);
                video.Player.Play();
                video.Player.loopPointReached += HandleFimVideo;
            }

            return;
        }

        private void HandleFimVideo(UnityEngine.Video.VideoPlayer source) {
            gameObjectVideo.SetActive(false);
            return;
        }
    }
}