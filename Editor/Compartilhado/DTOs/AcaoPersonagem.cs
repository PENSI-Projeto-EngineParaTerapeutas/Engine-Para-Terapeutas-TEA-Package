using UnityEngine;

namespace EngineParaTerapeutas.DTOs {
    public class AcaoPersonagem {
        public AnimationClip Animacao { get => animacao; set => animacao = value; }
        private AnimationClip animacao;

        public GameObject ObjetoGatilho { get => objetoGatilho; set => objetoGatilho = value; }
        private GameObject objetoGatilho;
    }
}