using UnityEngine;

namespace EngineParaTerapeutas.ComponentesGameObjects {
    public class ControleDireto : MonoBehaviour {
        [SerializeField]
        private float PASSO_ROTACAO = 0.15f;

        public Transform BracoEsquerdo { get => bracoEsquerdo; }
        public Transform AntebracoEsquerdo { get => antebracoEsquerdo; }
        public Transform BracoDireito { get => bracoDireito; }
        public Transform AntebracoDireito { get => antebracoDireito; }
        public Transform PernaEsquerda { get => pernaEsquerda; }
        public Transform PernaInferiorEsquerda { get => pernaInferiorEsquerda; }
        public Transform PernaDireita { get => pernaDireita; }
        public Transform PernaInferiorDireita { get => pernaInferiorDireita; }

        [SerializeField]
        private Transform bracoEsquerdo;
        [SerializeField]
        private Transform antebracoEsquerdo;

        [SerializeField]
        private Transform bracoDireito;
        [SerializeField]
        private Transform antebracoDireito;

        [SerializeField]
        private Transform pernaEsquerda;
        [SerializeField]
        private Transform pernaInferiorEsquerda;

        [SerializeField]
        private Transform pernaDireita;
        [SerializeField]
        private Transform pernaInferiorDireita;

        public void RotacionarSentidoHorario(Transform parte) {
            parte.rotation = new Quaternion(parte.rotation.x, parte.rotation.y, parte.rotation.z + PASSO_ROTACAO, parte.rotation.w);
            return;
        }

        public void RotacionarSentidoAntiHorario(Transform parte) {
            parte.rotation = new Quaternion(parte.rotation.x, parte.rotation.y, parte.rotation.z - PASSO_ROTACAO, parte.rotation.w);
            return;
        }
    }
}