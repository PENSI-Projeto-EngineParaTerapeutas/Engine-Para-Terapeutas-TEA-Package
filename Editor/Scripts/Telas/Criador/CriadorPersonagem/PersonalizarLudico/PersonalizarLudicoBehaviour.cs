using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Autis.Editor.Constantes;
using Autis.Editor.Telas;
using Autis.Editor.Utils;
using Autis.Editor.Manipuladores;
using Autis.Editor.UI;
using Autis.Editor.Excecoes;
using Autis.Runtime.Constantes;

namespace Autis.Editor.Criadores {
    public class PersonalizarLudicoBehaviour : Tela {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorPersonagem/PersonalizarLudico/PersonalizarLudicoTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorPersonagem/PersonalizarLudico/PersonalizarLudicoStyle.uss";

        #region .: Mensagens :.

        private const string MENSAGEM_ERRO_CARREGAR_PREFAB_PERSONAGEM = "[ERROR]: Não foi possível carregar o prefab para o Personagem Lúdico {nome}. Certifique-se de que o prefab está localizado em: <Pacote>/Prefabs/Personagens/Ludico_{nome}.prefab. Além disso, garanta que o nome do sprite completo para o Personagem Lúdico equivale ao final do nome no prefab.";

        private const string MENSAGEM_ERRO_TIPO_PERSONAGEM_NAO_SELECIONADO = "Selecione um tipo de Personagem Lúdico.\n";

        #endregion

        #region .: Elementos :.

        private const string NOME_REGIAO_TIPOS_PERSONAGEM = "regiao-tipos-personagens";
        private readonly VisualElement regiaoTiposPersonagem;

        protected const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        protected VisualElement regiaoCarregamentoBotoesConfirmacao;
        
        protected BotoesConfirmacao botoesConfirmacao;

        #endregion

        public Action OnConfirmarCriacao { get; set; }

        private readonly GameObject prefabOriginal;
        private readonly ManipuladorPersonagemLudico manipuladorPersonagemLudico;

        private string nomePrefabAtual = string.Empty;
        private bool subtipoSelecionado = false;

        public PersonalizarLudicoBehaviour(ManipuladorPersonagemLudico manipuladorPersonagemLudico) {
            this.manipuladorPersonagemLudico = manipuladorPersonagemLudico;
            prefabOriginal = manipuladorPersonagemLudico.PrefabObjeto;
            nomePrefabAtual = manipuladorPersonagemLudico.PrefabObjeto.name;

            regiaoTiposPersonagem = Root.Query<VisualElement>(NOME_REGIAO_TIPOS_PERSONAGEM);

            CarregarTiposPersonagem();
            ConfigurarBotoesConfirmacao();

            return;
        }

        public override void OnEditorUpdate() {
            DefinirFerramenta();
            return;
        }

        private void DefinirFerramenta() {
            if(Selection.activeTransform == null || !Selection.activeTransform.CompareTag(NomesTags.EditorOnly)) {
                return;
            }

            if(!subtipoSelecionado) {
                Tools.current = Tool.Rect;
            }
            else if(Tools.current != Tool.Move) {
                Tools.current = Tool.Move;
            }

            return;
        }

        private void CarregarTiposPersonagem() {
            List<Texture> imagensPersonagens = Importador.ImportarSpriteCompletoPersonagensLudicos();

            foreach(Texture imagemPersonagem in imagensPersonagens) {
                Image displayImagemPersonagem = new() {
                    image = imagemPersonagem,
                };

                Button botaoSelecaoPersonagem = new();
                botaoSelecaoPersonagem.Add(displayImagemPersonagem);

                botaoSelecaoPersonagem.RegisterCallback<ClickEvent>(evt => {
                    AlterarPersonagem(imagemPersonagem.name);
                    subtipoSelecionado = true;
                });

                regiaoTiposPersonagem.Add(botaoSelecaoPersonagem);
            }

            return;
        }

        private void AlterarPersonagem(string nomePersonagem) {
            string nomePrefabPersonagem = "Ludico_" + nomePersonagem;
            if(nomePrefabAtual == nomePrefabPersonagem) {
                return;
            }

            nomePrefabAtual = nomePrefabPersonagem;
            GameObject prefabPersonagem = Importador.ImportarPrefab(Path.Combine("Personagens", nomePrefabAtual + ExtensoesEditor.Prefab));

            if(prefabPersonagem == null) {
                Debug.LogError(MENSAGEM_ERRO_CARREGAR_PREFAB_PERSONAGEM.Replace("{nome}", nomePersonagem));
            }

            manipuladorPersonagemLudico.AlterarPrefab(prefabPersonagem);
            manipuladorPersonagemLudico.SetPosicao(new Vector3(5.0f, 0.0f));
            botoesConfirmacao.BotaoConfirmar.SetEnabled(manipuladorPersonagemLudico.PossuiPersonagemSelecionado());

            return;
        }

        private void ConfigurarBotoesConfirmacao() {
            botoesConfirmacao = new();
            botoesConfirmacao.BotaoConfirmar.clicked += HandleBotaoConfirmarClick;
            botoesConfirmacao.BotaoCancelar.clicked += HandleBotaoCancelarClick;

            regiaoCarregamentoBotoesConfirmacao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO);
            regiaoCarregamentoBotoesConfirmacao.Add(botoesConfirmacao.Root);

            return;
        }

        private void HandleBotaoConfirmarClick() {
            try {
                VerificarCamposObrigatorios();
            }
            catch(ExcecaoCamposObrigatoriosVazios excecoes) {
                PopupAvisoBehaviour.ShowPopupAviso(excecoes.Message);
                return;
            }

            OnConfirmarCriacao?.Invoke();

            Navigator.Instance.Voltar();

            return;
        }

        private void VerificarCamposObrigatorios() {
            string mensagem = string.Empty;

            if(!manipuladorPersonagemLudico.PossuiPersonagemSelecionado()) {
                mensagem += MENSAGEM_ERRO_TIPO_PERSONAGEM_NAO_SELECIONADO;
                throw new ExcecaoCamposObrigatoriosVazios(mensagem);
            }

            return;
        }

        private void HandleBotaoCancelarClick() {
            if(nomePrefabAtual == prefabOriginal.name) {
                Navigator.Instance.Voltar();
                return;
            }

            manipuladorPersonagemLudico.AlterarPrefab(prefabOriginal);
            Navigator.Instance.Voltar();

            return;
        }
    }
}