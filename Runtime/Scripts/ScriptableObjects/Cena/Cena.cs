using UnityEngine;
using UnityEngine.SceneManagement;
using Autis.Runtime.DTOs;

namespace Autis.Runtime.ScriptableObjects {
    [CreateAssetMenu(fileName = "Cena", menuName = "AUTIS/Scriptable Object/Cena")]
    public class Cena : ScriptableObject {

        public string nomeExibicao;

        [HideInInspector]
        public string nomeArquivo;

        [HideInInspector]
        public string caminho;

        [HideInInspector]
        public int buildIndex;

        [Min(0)]
        public int faixaEtaria;

        public NiveisDificuldade nivelDificuldade;

        [HideInInspector]
        public TipoGabarito tipoGabarito;

        public void AssociarValores(Scene arquivo) {
            nomeExibicao = arquivo.name;
            caminho = arquivo.path;
            buildIndex = arquivo.buildIndex;
            
            return;
        }
    }
}