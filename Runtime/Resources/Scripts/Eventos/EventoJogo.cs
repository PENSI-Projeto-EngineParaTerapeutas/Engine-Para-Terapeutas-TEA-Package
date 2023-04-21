using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Autis.Runtime.Eventos {
    [CreateAssetMenu(fileName = "EventoJogo", menuName = "AUTIS/Scriptable Object/Eventos/Evento Jogo")]
    public class EventoJogo : ScriptableObject {
        public int QuantidadeCallbacks { get => callbacks.Count; }

        private readonly List<UnityAction> callbacks = new();

        private void OnEnable() {
            SceneManager.sceneLoaded += HandleSceneLoad;
            SceneManager.sceneUnloaded += HandleSceneUnload;

            return;
        }

        private void HandleSceneLoad(Scene scene, LoadSceneMode mode) {
            LimparListaCallbacks();
            return;
        }

        private void HandleSceneUnload(Scene scene) {
            LimparListaCallbacks();

            SceneManager.sceneLoaded -= HandleSceneLoad;
            SceneManager.sceneUnloaded -= HandleSceneUnload;

            return;
        }

        public void LimparListaCallbacks() {
            callbacks.Clear();
            return;
        }

        public void AdicionarCallback(UnityAction callback) {
            callbacks.Add(callback);
            return;
        }

        public void RemoverCallback(UnityAction callback) {
            callbacks.Remove(callback);
            return;
        }

        public void AcionarCallbacks() {
            foreach(UnityAction callback in callbacks) {
                callback();
            }

            return;
        }
    }
}