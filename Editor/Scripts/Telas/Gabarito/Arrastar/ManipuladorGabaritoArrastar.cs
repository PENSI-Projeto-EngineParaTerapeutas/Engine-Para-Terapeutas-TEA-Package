using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Autis.Runtime.Constantes;
using Autis.Runtime.DTOs;

namespace Autis.Editor.Manipuladores {
    public class ManipuladorGabaritoArrastar : ManipuladorGabarito {
        public List<ManipuladorObjetoInteracao> ElementosInteracaoArrastaveis { get => elementosInteracaoArrastaveis; }
        private readonly List<ManipuladorObjetoInteracao> elementosInteracaoArrastaveis = new();

        public Dictionary<ManipuladorObjetoInteracao, ManipuladorObjetoInteracao> AssociacoesOrigemDestino { get => associacoesOrigemDestino; }
        private readonly Dictionary<ManipuladorObjetoInteracao, ManipuladorObjetoInteracao> associacoesOrigemDestino = new();

        public ManipuladorGabaritoArrastar() { 
            foreach(ManipuladorObjetoInteracao manipulador in elementosInteracaoArrastaveis) {
                associacoesOrigemDestino.Add(manipulador, null);
            }

            return;
        }

        protected override void EncontrarObjetoGabarito() {
            base.EncontrarObjetoGabarito();

            List<GameObject> elementosInteracaoEncontrados = GameObject.FindGameObjectsWithTag(NomesTags.ObjetosInteracao).ToList();
            foreach(GameObject elemento in elementosInteracaoEncontrados) {
                ManipuladorObjetoInteracao manipulador = new();
                manipulador.SetObjeto(elemento);

                elementosInteracao.Add(manipulador);

                if(manipulador.GetTipoInteracao() == TiposAcoes.Arrastavel) {
                    elementosInteracaoArrastaveis.Add(manipulador);
                }
            }

            return;
        }

        public override void Finalizar() {
            int quantidadeAcertos = 0;

            foreach(KeyValuePair<ManipuladorObjetoInteracao, ManipuladorObjetoInteracao> associacao in associacoesOrigemDestino) {
                ManipuladorObjetoInteracao manipuladorElementoOrigem = associacao.Key;
                ManipuladorObjetoInteracao manipuladorElementoDestino = associacao.Value;
                
                if(manipuladorElementoDestino == null) {
                    manipuladorElementoOrigem.SetObjetoDestino(null);
                    continue;
                }

                manipuladorElementoOrigem.SetObjetoDestino(manipuladorElementoDestino.ObjetoAtual.transform);

                quantidadeAcertos++;
            }
            
            SetLimiteAcertos(true);
            SetQuantidadeAcertos(quantidadeAcertos);

            foreach(ManipuladorObjetoInteracao manipulador in elementosInteracao) {
                manipulador.Finalizar();
            }

            return;
        }

        public override void Cancelar() {
            associacoesOrigemDestino.Clear();
            foreach(ManipuladorObjetoInteracao manipulador in elementosInteracaoArrastaveis) {
                associacoesOrigemDestino.Add(manipulador, null);
            }

            return;
        }

        public void AdicionarAssociacao(GameObject elementoOrigem, GameObject elementoDestino) {
            ManipuladorObjetoInteracao manipuladorElementoOrigem = elementosInteracaoArrastaveis.Find(manipualdorElemento => manipualdorElemento.ObjetoAtual == elementoOrigem);
            ManipuladorObjetoInteracao manipuladorElementoDestino = elementosInteracao.Find(manipuladorElemento => manipuladorElemento.ObjetoAtual == elementoDestino);

            associacoesOrigemDestino[manipuladorElementoOrigem] = manipuladorElementoDestino;

            return;
        }

        public void AdicionarAssociacao(ManipuladorObjetoInteracao manipuladorElementoOrigem, ManipuladorObjetoInteracao manipuladorElementoDestino) {
            associacoesOrigemDestino[manipuladorElementoOrigem] = manipuladorElementoDestino;
            return;
        }

        public void RemoverAssociacao(ManipuladorObjetoInteracao manipuladorElementoOrigem) {
            associacoesOrigemDestino[manipuladorElementoOrigem] = null;
            return;
        }
    }
}