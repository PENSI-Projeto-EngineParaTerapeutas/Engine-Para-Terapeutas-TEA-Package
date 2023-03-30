using System.IO;
using UnityEngine;
using UnityEngine.Video;

namespace Autis.Runtime.ComponentesGameObjects {
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(VideoPlayer))]
    [AddComponentMenu("Engine Terapeutas TEA/Componentes Básicos/Texto")]
    public class Video : MonoBehaviour {
        public string nomeArquivoVideo;

        public string CaminhoCompletoArquivoVideo { get => caminhoCompletoArquivoVideo; set { caminhoCompletoArquivoVideo = value; } }
        private string caminhoCompletoArquivoVideo;

        public VideoPlayer Player { 
            get {
                if(player != null) {
                    return player;
                }

                player = GetComponent<VideoPlayer>();
                return player;
            }
        }
        private VideoPlayer player = null;

        public AudioSource PlayerAudio {
            get {
                if(playerAudio != null) {
                    return playerAudio;
                }

                playerAudio = GetComponent<AudioSource>();
                return playerAudio;
            }
        }
        private AudioSource playerAudio = null;

        private void Awake() {
            CaminhoCompletoArquivoVideo = Path.Combine(Application.streamingAssetsPath, nomeArquivoVideo);
            Player.url = CaminhoCompletoArquivoVideo;

            if(Player.playOnAwake && gameObject.activeInHierarchy) {
                Player.Play();
            }

            return;
        }

        public void AlterarVideo(string nomeNovoVideo) {
            nomeArquivoVideo = nomeNovoVideo;

            CaminhoCompletoArquivoVideo = Path.Combine(Application.streamingAssetsPath, nomeArquivoVideo);
            Player.url = CaminhoCompletoArquivoVideo;

            return;
        }
    }
}