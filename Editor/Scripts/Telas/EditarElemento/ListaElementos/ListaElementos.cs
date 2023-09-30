using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Autis.Editor.UI {
    public class ListaElementos : ElementoInterfaceEditor {
        protected override string CaminhoTemplate => "Telas/EditarElemento/ListaElementos/ListaElementosTemplate.uxml";
        protected override string CaminhoStyle => "Telas/EditarElemento/ListaElementos/ListaElementosStyle.uss";

        #region .: Elementos :.

        public Foldout Foldout { get => foldout; }
        public ListView ListView { get => listView; }

        private const string NOME_FOLDOUT = "foldout-lista";
        private Foldout foldout;

        private const string NOME_LISTA = "lista-elementos";
        private ListView listView;

        #endregion

        public List<GameObject> Objetos { get => objetos; }
        private readonly List<GameObject> objetos;

        public ListaElementos(string label, List<GameObject> objetos) {
            this.objetos = objetos;

            ConfigurarFoldout(label);
            ConfigurarListView();

            return;
        }

        private void ConfigurarFoldout(string label) {
            foldout = root.Query<Foldout>(NOME_FOLDOUT);
            foldout.text = label;

            return;
        }

        private void ConfigurarListView() {
            listView = root.Query<ListView>(NOME_LISTA);

            listView.reorderable = false;
            listView.showAddRemoveFooter = false;
            listView.showFoldoutHeader = false;

            static VisualElement makeItem() {
                VisualElement elemento = new();
                elemento.AddToClassList("item-lista-elementos");

                return elemento;
            };

            listView.makeItem = makeItem;
            listView.itemsSource = objetos;
            listView.selectionType = SelectionType.Single;

            return;
        }

        public void RemoverItem(GameObject item) {
            listView.itemsSource.Remove(item);
            listView.Rebuild();

            return;
        }

        public void Atualizar() {
            listView.Rebuild();
            return;
        }
    }
}