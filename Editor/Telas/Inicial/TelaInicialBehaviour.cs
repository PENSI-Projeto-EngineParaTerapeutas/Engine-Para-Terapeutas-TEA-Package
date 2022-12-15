using UnityEditor;
using UnityEngine;

namespace EngineParaTerapeutas.Telas {
    public class TelaInicialBehaviour : TelaEditor {
        private const string TITULO = "Tela Inicial";

        [MenuItem("Engine Para Terapeutas/Tela Inicial")]
        public static void ShowInicial() {
            TelaInicialBehaviour janela = GetWindow<TelaInicialBehaviour>();
            janela.titleContent = new GUIContent(TITULO);

            return;
        }

        public override void CreateGUI() {
            base.CreateGUI();

            ImportarTemplate("Telas/Inicial/TelaInicialTemplate.uxml");
            ImportarStyle("Telas/Inicial/TelaInicialStyle.uss");

            ConfigurarElementos();

            return;
        }

        private void ConfigurarElementos() {
            return;
        }
    }
}