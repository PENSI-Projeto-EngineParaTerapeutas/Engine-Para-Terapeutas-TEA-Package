using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Autis.Runtime.Constantes;
using Autis.Editor.Constantes;

namespace Autis.Editor.Manipuladores {
    public class ManipuladorBonecoPalito : ManipuladorPersonagens {
        public Color Cor { get => spritesPersonagem.First().color; }

        public ManipuladorBonecoPalito() : base() {}

        public ManipuladorBonecoPalito(GameObject prefab) : base(prefab) {}

        public override void Editar(GameObject objetoAlvo) {
            base.Editar(objetoAlvo);
            spritesPersonagem = objeto.GetComponentsInChildren<SpriteRenderer>().ToList();

            return;
        }

        protected override void ExcluirInterno() {
            GameObject.DestroyImmediate(objeto);

            objeto = null;
            RemoverVinculo();

            return;
        }

        protected void RemoverVinculo() {
            dados = null;
            spritesPersonagem = null;
            animationComponente = null;
            animatorComponente = null;
            sortingGroupComponente = null;
            controleDiretoComponente = null;
            controleIndiretoComponente = null;

            LimparAcoesControleIndireto();
            associacaoAcoesOriginal.Clear();

            return;
        }

        protected override void CarregarController() {
            RuntimeAnimatorController controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesBonecoPalito, "ControllerPersonagemPalito.controller"));
            if(controller == null) {
                Debug.LogError(MENSAGEM_ERRO_CARREGAR_CONTROLLER_PERSONAGEM.Replace("{nome-controller}", "ControllerPersonagemPalito.controller").Replace("{local}", ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesBonecoPalito));
            }

            objeto.GetComponent<Animator>().runtimeAnimatorController = controller;

            return;
        }

        public void SetCor(Color cor) {
            if(!PossuiPersonagemSelecionado()) {
                return;
            }

            foreach(SpriteRenderer membro in spritesPersonagem) {
                membro.color = cor;
            }

            return;
        }

        public override List<AnimationClip> GetAnimacoes() {
            List<AnimationClip> clipsAnimacoes = new();
            List<string> caminhoArquivosPastaAnimacao = Directory.GetFiles(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesBonecoPalito).ToList();

            if(caminhoArquivosPastaAnimacao.Count <= 0) {
                Debug.LogError(MENSAGEM_ERRO_CARREGAR_ANIMACOES.Replace("{local}", ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesBonecoPalito));
            }

            foreach(string caminhoArquivo in caminhoArquivosPastaAnimacao) {
                if(Path.GetExtension(caminhoArquivo) == ExtensoesEditor.ClipAnimacao) {
                    AnimationClip clipAnimacao = AssetDatabase.LoadAssetAtPath<AnimationClip>(caminhoArquivo);
                    clipsAnimacoes.Add(clipAnimacao);
                }
            }

            return clipsAnimacoes;
        }
    }
}