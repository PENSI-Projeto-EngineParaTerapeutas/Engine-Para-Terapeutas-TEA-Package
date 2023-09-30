using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Autis.Runtime.Constantes;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Editor.Constantes;

namespace Autis.Editor.Manipuladores {
    public class ManipuladorPersonagemLudico : ManipuladorPersonagens, IExcluir {
        public DadosPersonagemLudico DadosPersonagemLudico { get => dadosPersonagemLudico; }
        protected DadosPersonagemLudico dadosPersonagemLudico;

        public ManipuladorPersonagemLudico(GameObject prefabObjeto) : base(prefabObjeto) { }

        public override void Editar(GameObject objetoAlvo) {
            base.Editar(objetoAlvo);

            dadosPersonagemLudico = objeto.GetComponent<DadosPersonagemLudico>();

            return;
        }

        public void Excluir() {
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
            
            dadosPersonagemLudico = null;

            LimparAcoesControleIndireto();
            associacaoAcoesOriginal.Clear();

            return;
        }

        protected override void CarregarController() {
            string nomeTipoPersonagemLudico = dadosPersonagemLudico.tipoPersonagensLudicos.ToString();
            RuntimeAnimatorController controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesPersonagemLudico, nomeTipoPersonagemLudico, "ControllerPersonagemLudico.controller"));
            if(controller == null) {
                Debug.LogError(MENSAGEM_ERRO_CARREGAR_CONTROLLER_PERSONAGEM.Replace("{nome-controller}", "ControllerPersonagemLudico.controller").Replace("{local}", ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesPersonagemLudico + nomeTipoPersonagemLudico));
            }

            objeto.GetComponent<Animator>().runtimeAnimatorController = controller;
            return;
        }

        public override List<AnimationClip> GetAnimacoes() {
            List<AnimationClip> clipsAnimacoes = new();

            string tipoPersonagemLudico = DadosPersonagemLudico.tipoPersonagensLudicos.ToString();
            List<string> caminhoArquivosPastaAnimacao = Directory.GetFiles(Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesPersonagemLudico, tipoPersonagemLudico)).ToList();

            if(caminhoArquivosPastaAnimacao.Count <= 0) {
                Debug.LogError(MENSAGEM_ERRO_CARREGAR_ANIMACOES.Replace("{local}", ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesPersonagemLudico + tipoPersonagemLudico));
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