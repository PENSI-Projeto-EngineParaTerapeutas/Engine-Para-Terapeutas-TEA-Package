using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Runtime.Constantes;
using Autis.Editor.Constantes;

namespace Autis.Editor.Manipuladores {
    public class ManipuladorAvatar : ManipuladorPersonagens {
        private const string NOME_ANIMATOR_CONTROLLER = "ControllerPersonagemAvatarFeminino.controller";

        private PersonalizacaoPartes componentePersonalizacaoPartes;
        private PersonalizacaoCores componentePersonalizacaoCores;

        public ManipuladorAvatar() : base() {}

        public ManipuladorAvatar(GameObject prefab) : base(prefab) {}

        public override void Editar(GameObject objetoEditado) {
            base.Editar(objetoEditado);

            componentePersonalizacaoPartes = objeto.GetComponent<PersonalizacaoPartes>();
            componentePersonalizacaoCores = objeto.GetComponent<PersonalizacaoCores>();

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
            
            componentePersonalizacaoPartes = null;

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

            componentePersonalizacaoCores.SetCorOlhos(cor);
            return;
        }

        public Color GetCorOlhos() {
            return componentePersonalizacaoCores.CorOlhos;
        }

        public void SetCorPele(Color cor) {
            if(!PossuiPersonagemSelecionado()) {
                return;
            }

            componentePersonalizacaoCores.SetCorPele(cor);
            return;
        }

        public Color GetCorPele() {
            return componentePersonalizacaoCores.CorPele;
        }

        public void SetCorCabelo(Color cor) {
            if(!PossuiPersonagemSelecionado()) {
                return;
            }

            componentePersonalizacaoCores.SetCorCabelo(cor);
            return;
        }

        public Color GetCorCabelo() {
            return componentePersonalizacaoCores.CorCabelo;
        }

        public void SetCorRoupaSuperior(Color cor) {
            if(!PossuiPersonagemSelecionado()) {
                return;
            }

            componentePersonalizacaoCores.SetCorRoupaSuperior(cor);
            return;
        }

        public Color GetCorRoupaSuperior() {
            return componentePersonalizacaoCores.CorRoupaSuperior;
        }

        public void SetCorRoupaInferior(Color cor) {
            if(!PossuiPersonagemSelecionado()) {
                return;
            }

            componentePersonalizacaoCores.SetCorRoupaInferior(cor);
            return;
        }

        public Color GetCorRoupaInferior() {
            return componentePersonalizacaoCores.CorRoupaInferior;
        }

        public void SetConjuntoRoupas(int indexConjunto) {
            if(!PossuiPersonagemSelecionado() || indexConjunto >= componentePersonalizacaoPartes.Roupas.Count) {
                return;
            }

            componentePersonalizacaoPartes.SetConjuntoRoupa(indexConjunto);
            return;
        }

        public void SetCabelo(int indexCabelo) {
            if(!PossuiPersonagemSelecionado() || indexCabelo < 0 || indexCabelo >= componentePersonalizacaoPartes.Cabelos.Count) {
                return;
            }

            componentePersonalizacaoPartes.SetCabelo(indexCabelo);
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