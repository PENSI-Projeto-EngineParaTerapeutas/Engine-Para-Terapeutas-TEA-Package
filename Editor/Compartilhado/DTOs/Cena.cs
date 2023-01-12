using UnityEngine;
using UnityEngine.SceneManagement;
using EngineParaTerapeutas.DTOs;

namespace EngineParaTerapeutas.ScriptableObjects {
    [CreateAssetMenu(fileName = "Cena", menuName = "Engine Para Terapeutas/Scriptable Object/Cena")]
    public class Cena : ScriptableObject {
        public string Nome { get => nome; set { nome = value; } }
        [SerializeField]
        private string nome = "";

        public int FaixaEtaria { get => faixaEtaria; set { faixaEtaria = (value <= 0) ? 0 : value; } }
        [Min(0)]
        [SerializeField]
        private int faixaEtaria = 0;

        public NiveisDificuldade NivelDificuldade { get => nivelDificuldade; set { nivelDificuldade = value; } }
        [SerializeField]
        private NiveisDificuldade nivelDificuldade = NiveisDificuldade.Facil;

        public Scene Arquivo { get => arquivo; set { arquivo = value; } }
        private Scene arquivo;
    }
}