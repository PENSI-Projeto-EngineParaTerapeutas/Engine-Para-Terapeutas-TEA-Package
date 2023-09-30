using System;
using UnityEngine;

namespace Autis.Runtime.ComponentesGameObjects {
    [Serializable]
    public class ParteControleDireto {
        public string NomeDisplay { get => nomeDisplay; }
        [SerializeField]
        private string nomeDisplay;

        public Transform ParteCorpo { get => parteCorpo; }
        [SerializeField]
        private Transform parteCorpo;
    }
}