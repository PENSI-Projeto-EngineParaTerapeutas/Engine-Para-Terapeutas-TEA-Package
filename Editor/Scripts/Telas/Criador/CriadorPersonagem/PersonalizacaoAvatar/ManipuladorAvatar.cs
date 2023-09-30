using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Runtime.Constantes;
using Autis.Editor.Constantes;

namespace Autis.Editor.Manipuladores {
    public class ManipuladorAvatar : ManipuladorPersonagens, IExcluir {
        private const string NOME_ANIMATOR_CONTROLLER = "ControllerPersonagemAvatarFeminino.controller";

        private PartesPersonalizaveis partesAvatar;

        public ManipuladorAvatar(GameObject prefab) : base(prefab) {}

        public override void Editar(GameObject objetoEditado) {
            base.Editar(objetoEditado);
            partesAvatar = objeto.GetComponent<PartesPersonalizaveis>();

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
            
            partesAvatar = null;

            LimparAcoesControleIndireto();
            associacaoAcoesOriginal.Clear();

            return;
        }

        protected override void CarregarController() {
            // TODO: Alterar controller com base em ser masculino ou feminino
            RuntimeAnimatorController controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesAvatar, NOME_ANIMATOR_CONTROLLER));
            if(controller == null) {
                Debug.LogError(MENSAGEM_ERRO_CARREGAR_CONTROLLER_PERSONAGEM.Replace("{nome-controller}", NOME_ANIMATOR_CONTROLLER).Replace("{local}", ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesAvatar));
            }

            objeto.GetComponent<Animator>().runtimeAnimatorController = controller;

            return;
        }

        public void SetCorOlhos(Color cor) {
            if(!PossuiPersonagemSelecionado()) {
                return;
            }

            partesAvatar.Olhos.color = cor;
            return;
        }

        public Color GetCorOlhos() {
            return partesAvatar.Olhos.color;
        }

        public void SetCorPele(Color cor) {
            if(!PossuiPersonagemSelecionado()) {
                return;
            }

            foreach(SpriteRenderer parteCorpo in partesAvatar.PartesCorpo) {;
                parteCorpo.color = cor;
            }

            return;
        }

        public Color GetCorPele() {
            return partesAvatar.PartesCorpo.First().color;
        }

        public void SetCorCabelo(Color cor) {
            if(!PossuiPersonagemSelecionado()) {
                return;
            }

            foreach(SpriteRenderer cabelo in partesAvatar.Cabelos) {
                cabelo.color = cor;
            }

            return;
        }

        public Color GetCorCabelo() {
            return partesAvatar.Cabelos.First().color;
        }

        public void SetCorRoupaSuperior(Color cor) {
            if(!PossuiPersonagemSelecionado()) {
                return;
            }

            foreach(SpriteRenderer roupa in partesAvatar.RoupasSuperiores) {
                roupa.color = cor;
            }

            return;
        }

        public Color GetCorRoupaSuperior() {
            return partesAvatar.RoupasSuperiores.First().color;
        }

        public void SetCorRoupaInferior(Color cor) {
            if(!PossuiPersonagemSelecionado()) {
                return;
            }

            foreach(SpriteRenderer roupa in partesAvatar.RoupasInferiores) {
                roupa.color = cor;
            }

            return;
        }

        public Color GetCorRoupaInferior() {
            return partesAvatar.RoupasInferiores.First().color;
        }

        public void SetConjuntoRoupas(int indexConjunto) {
            if(!PossuiPersonagemSelecionado() || indexConjunto >= partesAvatar.Roupas.Count) {
                return;
            }

            for(int i = 0; i < partesAvatar.Roupas.Count; i++) {
                bool estaAtivo = i == indexConjunto;
                partesAvatar.Roupas[i].SetActive(estaAtivo);
            }

            return;
        }

        public void SetCabelo(int indexCabelo) {
            if(!PossuiPersonagemSelecionado() || indexCabelo < 0 || indexCabelo >= partesAvatar.Cabelos.Count) {
                return;
            }

            for(int i = 0; i < partesAvatar.Cabelos.Count; i++) {
                bool estaAtivo = i == indexCabelo;
                partesAvatar.Cabelos[i].gameObject.SetActive(estaAtivo);
            }

            return;
        }

        public override List<AnimationClip> GetAnimacoes() {
            List<AnimationClip> clipsAnimacoes = new();
            List<string> caminhoArquivosPastaAnimacao = Directory.GetFiles(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesAvatar).ToList();

            if(caminhoArquivosPastaAnimacao.Count <= 0) {
                Debug.LogError(MENSAGEM_ERRO_CARREGAR_ANIMACOES.Replace("{local}", ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesAvatar));
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