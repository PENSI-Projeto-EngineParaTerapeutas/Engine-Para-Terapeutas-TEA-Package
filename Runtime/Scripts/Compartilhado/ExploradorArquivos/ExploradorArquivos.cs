using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Autis.Runtime.Constantes;

namespace Autis.Runtime.Utils {
    public static class ExploradorArquivos {
        private static Action<Texture2D> callbackAoCarregarImagem;
        private static Action<AudioClip> callbackAoCarregarAudio;

        public static void SelecionarSprite(Action<Texture2D> callback) {
            callbackAoCarregarImagem = callback;
            AdaptadorExploradorArquivosJS.RequisitarSelecaoArquivo(HandleSelecaoArquivoImagemConcluida, ExtensoesRuntime.Imagem);
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

            callbackAoCarregarImagem.Invoke(texture);
            yield break;
        }

        public static void SelecionarAudio(Action<AudioClip> callback) {
            callbackAoCarregarAudio = callback;
            AdaptadorExploradorArquivosJS.RequisitarSelecaoArquivo(HandleSelecaoArquivoAudioConcluida, ExtensoesRuntime.Audio);
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