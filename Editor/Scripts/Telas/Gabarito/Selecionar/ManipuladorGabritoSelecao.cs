using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Autis.Runtime.Constantes;
using Autis.Runtime.DTOs;

namespace Autis.Editor.Manipuladores {
    public class ManipuladorGabritoSelecao : ManipuladorGabarito {
        public List<ManipuladorObjetoInteracao> ElementosInteracaoSelecionaveis { get => elementosInteracaoSelecionaveis; }
        private readonly List<ManipuladorObjetoInteracao> elementosInteracaoSelecionaveis = new();

        public bool OrdemEhRelevante { get; set; }

        public List<string> OrdemSelecaoElementos { get => ordemSelecaoElementos; }
        private readonly List<string> ordemSelecaoElementos = new();

        public ManipuladorGabritoSelecao() { }

        protected override void EncontrarObjetoGabarito() {
            base.EncontrarObjetoGabarito();

            List<GameObject> elementosInteracaoEncontrados = GameObject.FindGameObjectsWithTag(NomesTags.ObjetosInteracao).ToList();
            foreach(GameObject elemento in elementosInteracaoEncontrados) {
                ManipuladorObjetoInteracao manipulador = new();
                manipulador.SetObjeto(elemento);

                elementosInteracao.Add(manipulador);

                if(manipulador.GetTipoInteracao() == TiposAcoes.Selecionavel) {
                    elementosInteracaoSelecionaveis.Add(manipulador);
                }
            }

            return;
        }

        protected override void FinalizarInterno() {
            int quantidadeOpcoesCorretas = 0;
            
            foreach(ManipuladorObjetoInteracao manipulador in elementosInteracaoSelecionaveis) {
                int indexObjeto = ordemSelecaoElementos.FindIndex(nomeObjeto => manipulador.GetNome() == nomeObjeto);
                if(indexObjeto == -1) {
                    manipulador.SetOpcaoCorretaGabaritoSelecao(false);
                    continue;
                }

                quantidadeOpcoesCorretas++;

                manipulador.SetOpcaoCorretaGabaritoSelecao(true);
                manipulador.SetOrdemSelecaoEhRelevante(OrdemEhRelevante);
                manipulador.SetOrdemSelecao(indexObjeto);
            }

            SetLimiteAcertos(true);
            SetQuantidadeAcertos(quantidadeOpcoesCorretas);

            foreach(ManipuladorObjetoInteracao manipulador in elementosInteracao) {
                manipulador.Finalizar();
            }

            return;
        }

        public override void Cancelar() {
            ordemSelecaoElementos.Clear();
            OrdemEhRelevante = false;

            return;
        }

        public void SetOrdemSelecaoEhRelevante(bool possuiOrdem) {
            foreach(ManipuladorObjetoInteracao manipulador in elementosInteracaoSelecionaveis) {
                manipulador.SetOrdemSelecaoEhRelevante(possuiOrdem);
            }

            return;
        }

        public bool GetOrdemSelecaoEhRelevante() {
            ManipuladorObjetoInteracao manipulador = elementosInteracaoSelecionaveis.Find(elemento => elemento.EhOpcaoCorretaSelecao());
            if(manipulador == null) {
                return false;
            }

            return manipulador.GetOrdemSelecaoEhRelevante();
        }

        public void SetOpcaoCorretaElemeto(GameObject elemento, bool ehCorreto) {
            ManipuladorObjetoInteracao manipulador = elementosInteracaoSelecionaveis.Find(manipualdorElemento => manipualdorElemento.ObjetoAtual == elemento);
            manipulador.SetOpcaoCorretaGabaritoSelecao(ehCorreto);

            return;
        }

        public bool ElementoEhOpcaoCorreta(GameObject elemento) {
            ManipuladorObjetoInteracao manipulador = elementosInteracaoSelecionaveis.Find(manipuladorElemento => manipuladorElemento.ObjetoAtual == elemento);
            return manipulador.EhOpcaoCorretaSelecao();
        }

        public List<ManipuladorObjetoInteracao> GetElementosOpcoesCorretas() {
            List<ManipuladorObjetoInteracao> elementosCorretos = new();
            
            foreach(ManipuladorObjetoInteracao manipulador in elementosInteracaoSelecionaveis) {
                if(!manipulador.EhOpcaoCorretaSelecao()) {
                    continue;
                }

                elementosCorretos.Add(manipulador);
            }

            return elementosCorretos;
        }

        public void SetOrdemSelecaoElemento(GameObject elemento, int ordem) {
            ManipuladorObjetoInteracao manipulador = elementosInteracaoSelecionaveis.Find(manipuladorElemento => manipuladorElemento.ObjetoAtual == elemento);
            manipulador.SetOrdemSelecao(ordem);

            return;
        }

        public int GetOrdemSelecaoElemento(GameObject elemento) {
            ManipuladorObjetoInteracao manipulador = elementosInteracaoSelecionaveis.Find(manipuladorElemento => manipuladorElemento.ObjetoAtual == elemento);
            return manipulador.GetOrdemSelecao();
        }
    }
}