using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Autis.Runtime.Utils {
    public static class AdaptadorExploradorArquivosJS {
        private readonly static string NOME_OBJETO_DESTINATARIO_SEND_MESSAGE = typeof(ComponenteComunicacaoUploaderArquivosJS).Name;
        private readonly static string NOME_METODO_DESTINATARIO_SEND_MESSAGE = "ReceberCallbackSelecaoArquivoJS";

        public static ComponenteComunicacaoUploaderArquivosJS ComponenteComunicacaoJS { get => componenteComunicacaoJS; }
        private readonly static ComponenteComunicacaoUploaderArquivosJS componenteComunicacaoJS;

        public static GameObject ObjetoComunicacaoJS { get => objetoComunicacaoJS; }
        private readonly static GameObject objetoComunicacaoJS;

        private static Action<string> callbackAoSelecionarArquivo;

        #region .: Link com Funcoes JS :.

        [DllImport("__Internal")]
        private static extern void InicializarExploradorArquivosJS(string nomeGameObjectDestinatario, string nomeMetodoDestinatario);

        [DllImport("__Internal")]
        private static extern void RequisitarSelecaoArquivoExploradorJS(string extensoes);

        [DllImport("__Internal")]
        private static extern void ReiniciarExploradorArquivosJS();

        #endregion

        static AdaptadorExploradorArquivosJS() {
            objetoComunicacaoJS = new GameObject(NOME_OBJETO_DESTINATARIO_SEND_MESSAGE, typeof(ComponenteComunicacaoUploaderArquivosJS));
            componenteComunicacaoJS = objetoComunicacaoJS.GetComponent<ComponenteComunicacaoUploaderArquivosJS>();

            InicializarExploradorArquivosJS(NOME_OBJETO_DESTINATARIO_SEND_MESSAGE, NOME_METODO_DESTINATARIO_SEND_MESSAGE);

            return;
        }

        public static void RequisitarSelecaoArquivo(Action<string> callback, string extensoesPermitidas) {
            RequisitarSelecaoArquivoExploradorJS(extensoesPermitidas);
            callbackAoSelecionarArquivo = callback;

            return;
        }

        public static void IniciarUpload(string caminnho) {
            callbackAoSelecionarArquivo.Invoke(caminnho);
            Reiniciar();

            return;
        }

        private static void Reiniciar() {
            ReiniciarExploradorArquivosJS();
            callbackAoSelecionarArquivo = null;

            return;
        }
    }
}