using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.Utils {
    public static class ExploradorArquivos {
        private static Action<Sprite> callbackAoCarregarImagem;
        private static Action<AudioClip> callbackAoCarregarAudio;

        public static void SelecionarSprite(Action<Sprite> callback) {
            callbackAoCarregarImagem = callback;
            AdaptadorExploradorArquivosJS.RequisitarSelecaoArquivo(HandleSelecaoArquivoImagemConcluida, ConstantesRuntime.ExtensoesImagem);
            return;
        }

        private static void HandleSelecaoArquivoImagemConcluida(string caminho) {
            if(string.IsNullOrWhiteSpace(caminho)) {
                Debug.Log("[LOG]: Nenhum arquivo selecionado");
                return;
            }

            AdaptadorExploradorArquivosJS.ComponenteComunicacaoJS.StartCoroutine(CarregarSprite(caminho));
            return;
        }

        private static IEnumerator CarregarSprite(string caminho) {
            Texture2D texture;

            using(UnityWebRequest request = UnityWebRequestTexture.GetTexture(caminho)) {
                request.downloadHandler = new DownloadHandlerTexture();
                yield return request.SendWebRequest();

                texture = DownloadHandlerTexture.GetContent(request);
            }

            callbackAoCarregarImagem.Invoke(Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f)));
            yield break;
        }

        public static void SelecionarAudio(Action<AudioClip> callback) {
            callbackAoCarregarAudio = callback;
            AdaptadorExploradorArquivosJS.RequisitarSelecaoArquivo(HandleSelecaoArquivoAudioConcluida, ConstantesRuntime.ExtensoesAudio);
            return;
        }

        private static void HandleSelecaoArquivoAudioConcluida(string caminho) {
            if(string.IsNullOrWhiteSpace(caminho)) {
                Debug.Log("[LOG]: Nenhum arquivo selecionado");
                return;
            }

            AdaptadorExploradorArquivosJS.ComponenteComunicacaoJS.StartCoroutine(CarregarAudio(caminho));
            return;
        }

        private static IEnumerator CarregarAudio(string caminho) {
            AudioClip audio;

            using(UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(caminho, AudioType.WAV)) {
                request.downloadHandler = new DownloadHandlerAudioClip(caminho, AudioType.WAV);
                yield return request.SendWebRequest();

                audio = DownloadHandlerAudioClip.GetContent(request);
            }

            callbackAoCarregarAudio.Invoke(audio);
            yield break;
        }
    }
}