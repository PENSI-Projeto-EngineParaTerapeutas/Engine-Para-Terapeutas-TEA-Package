using UnityEngine;
using UnityEngine.SceneManagement;
using EngineParaTerapeutas.DTOs;

namespace EngineParaTerapeutas.ScriptableObjects {
    [CreateAssetMenu(fileName = "Cena", menuName = "Engine Para Terapeutas/Scriptable Object/Cena")]
    public class Cena : ScriptableObject {

        public string NomeExibicao { get => nomeExibicao; set { nomeExibicao = value; } }
        [SerializeField]
        private string nomeExibicao = "";

        public string NomeArquivo { get => nomeArquivo; set { nomeArquivo = value; } }
        private string nomeArquivo = "";

        public string Caminho { get => caminho; set { caminho = value; } }
        private string caminho = "";

        public int BuildIndex { get => buildIndex; set { buildIndex = value; } }
        private int buildIndex = -1;

        public int FaixaEtaria { get => faixaEtaria; set { faixaEtaria = (value <= 0) ? 0 : value; } }
        [Min(0)]
        [SerializeField]
        private int faixaEtaria = 0;

        public NiveisDificuldade NivelDificuldade { get => nivelDificuldade; set { nivelDificuldade = value; } }
        [SerializeField]
        private NiveisDificuldade nivelDificuldade = NiveisDificuldade.Facil;

        public string NomeArquivoVideoContexto { get => nomeArquivoVideoContexto; set { nomeArquivoVideoContexto = value; } }
        private string nomeArquivoVideoContexto = "";

        public void AssociarValores(Scene arquivo) {
            nomeExibicao = arquivo.name;
            caminho = arquivo.path;
            buildIndex = arquivo.buildIndex;
            
            return;
        }
    }
}