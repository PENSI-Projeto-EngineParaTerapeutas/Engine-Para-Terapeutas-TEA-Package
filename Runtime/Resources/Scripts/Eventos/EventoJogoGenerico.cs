using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace EngineParaTerapeutas.Eventos {
    [CreateAssetMenu(fileName = "EventoJogoGenerico", menuName = "Engine Para Terapeutas/Scriptable Object/Eventos/Evento Jogo Genérico")]
    public class EventoJogoGenerico<T> : ScriptableObject {
        public int QuantidadeCallbacks { get => callbacks.Count; }
        
        private readonly List<UnityAction<T>> callbacks = new();

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

        public void AdicionarCallback(UnityAction<T> callback) {
            callbacks.Add(callback);
            return;
        }

        public void RemoverCallback(UnityAction<T> callback) {
            callbacks.Remove(callback);
            return;
        }

        public void AcionarCallbacks(T valor) {
            foreach(UnityAction<T> callback in callbacks) {
                callback(valor);
            }

            return;
        }
    }
}