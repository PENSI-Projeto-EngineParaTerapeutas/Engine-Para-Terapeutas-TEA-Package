using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using EngineParaTerapeutas.Utils;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.ScriptableObjects;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.Telas {
    public class JanelaInicialBehaviour : JanelaEditor {
        protected override string CaminhoTemplate => "Janelas/JanelaInicial/JanelaInicialTemplate.uxml";
        protected override string CaminhoStyle => "Janelas/JanelaInicial/JanelaInicialStyle.uss";

        #region .: Elementos :.

        private const string NOME_REGIAO_AVISO_LISTA_CENAS_VAZIA = "aviso-lista-cenas-vazia";
        private VisualElement regiaoAvisoListaCenasVazia;

        private const string NOME_REGIAO_LISTA_CENAS = "lista-cenas";
        private VisualElement grupoListaCenas;

        private const string NOME_BOTAO_CRIAR_CENA = "botao-criar-cena";
        private Button botaoCriarCena;

        #endregion

        private List<Cena> cenas = new();

        [MenuItem("Engine Para Terapeutas/Janela Inicial")]
        public static void ShowJanelaInicial() {
            const string TITULO = "Janela Inicial";

            JanelaInicialBehaviour janela = GetWindow<JanelaInicialBehaviour>();
            janela.titleContent = new GUIContent(TITULO);

            return;
        }

        protected override void OnRenderizarInterface() {
            ConfigurarListaCenas();
            ConfigurarBotaoCriarCena();

            AlterarVisibilidadeListaCenas();

            return;
        }

        private void ConfigurarListaCenas() {
            grupoListaCenas = root.Query<VisualElement>(NOME_REGIAO_LISTA_CENAS);
            regiaoAvisoListaCenasVazia = root.Query<VisualElement>(NOME_REGIAO_AVISO_LISTA_CENAS_VAZIA);

            if(GerenciadorCenas.QuantidadeCenas <= 0) {
                return;
            }

            cenas = GerenciadorCenas.GetTodasCenasCriadas();
            foreach(Cena cena in cenas) {
                AdicionarDisplayCena(cena);
            }

            return;
        }

        private void AdicionarDisplayCena(Cena cena) {
            DisplayInformacoesCena informacoes = new(cena);
            informacoes.Root.name = cena.NomeExibicao;
            informacoes.CallbackExcluirCena = HandleExclusaoCena;

            grupoListaCenas.Add(informacoes.Root);

            return;
        }

        private void HandleExclusaoCena(DisplayInformacoesCena displayCena) {
            cenas.Remove(displayCena.InformacoesCena);
            List<VisualElement> elementos = grupoListaCenas.Children().ToList();

            foreach(VisualElement elemento in elementos) {
                if(elemento.name == displayCena.Root.name) {
                    grupoListaCenas.Remove(elemento);
                }
            }

            AlterarVisibilidadeListaCenas();
            return;
        }

        private void AlterarVisibilidadeListaCenas() {
            if(cenas.Count <= 0) {
                grupoListaCenas.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                regiaoAvisoListaCenasVazia.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            }
            else {
                grupoListaCenas.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                regiaoAvisoListaCenasVazia.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            }

            return;
        }

        private void ConfigurarBotaoCriarCena() {
            botaoCriarCena = root.Query<Button>(NOME_BOTAO_CRIAR_CENA);
            botaoCriarCena.clicked += HandleClickBotaoCriarCena;
            
            return;
        }

        private void HandleClickBotaoCriarCena() {
            Cena novaCena = GerenciadorCenas.CriarCena();
            cenas.Add(novaCena);

            AdicionarDisplayCena(novaCena);
            AlterarVisibilidadeListaCenas();

            return;
        }
    }
}