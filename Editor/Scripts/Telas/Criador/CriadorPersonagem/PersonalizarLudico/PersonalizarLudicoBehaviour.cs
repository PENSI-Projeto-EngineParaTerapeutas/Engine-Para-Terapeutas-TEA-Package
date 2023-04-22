using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Autis.Editor.Constantes;
using Autis.Editor.Telas;
using Autis.Editor.Utils;
using Autis.Editor.UI;

namespace Autis.Editor.Criadores {
    public class PersonalizarLudicoBehaviour : Tela {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorPersonagem/PersonalizarLudico/PersonalizarLudicoTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorPersonagem/PersonalizarLudico/PersonalizarLudicoStyle.uss";

        #region .: Mensagens :.

        private const string MENSAGEM_ERRO_CARREGAR_PREFAB_PERSONAGEM = "[ERROR]: Não foi possível carregar o prefab para o Personagem Lúdico {nome}. Certifique-se de que o prefab está localizado em: <Pacote>/Prefabs/Personagens/Ludico_{nome}.prefab. Além disso, garanta que o nome do sprite completo para o Personagem Lúdico equivale ao final do nome no prefab.";

        #endregion

        #region .: Elementos :.

        private const string NOME_REGIAO_TIPOS_PERSONAGEM = "regiao-tipos-personagens";
        private readonly VisualElement regiaoTiposPersonagem;

        private const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        private readonly VisualElement regiaoCarregamentoBotoesConfirmacao;

        private readonly BotoesConfirmacao botoesConfirmacao;

        #endregion

        private readonly GameObject prefabOriginal;
        private readonly Action<GameObject> reiniciarCriacaoComPrefabPersonagem;

        private string nomePrefabAtual = string.Empty;

        public PersonalizarLudicoBehaviour(GameObject prefabAtual, Action<GameObject> callbackReinicarCriacao) {
            prefabOriginal = prefabAtual;
            nomePrefabAtual = prefabAtual.name;
            reiniciarCriacaoComPrefabPersonagem = callbackReinicarCriacao;

            botoesConfirmacao = new BotoesConfirmacao();

            regiaoTiposPersonagem = Root.Query<VisualElement>(NOME_REGIAO_TIPOS_PERSONAGEM);
            regiaoCarregamentoBotoesConfirmacao = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO);

            CarregarTiposPersonagem();
            ConfigurarBotoesConfirmacao();

            return;
        }

        private void CarregarTiposPersonagem() {
            List<Texture2D> imagensPersonagens = CarregarImagensPersonagens();

            foreach(Texture2D imagemPersonagem in imagensPersonagens) {
                Image displayImagemPersonagem = new() {
                    image = imagemPersonagem,
                };

                VisualElement botaoSelecaoPersonagem = new();
                botaoSelecaoPersonagem.Add(displayImagemPersonagem);

                botaoSelecaoPersonagem.RegisterCallback<ClickEvent>(evt => {
                    AlterarPersonagem(imagemPersonagem.name);
                });

                regiaoTiposPersonagem.Add(botaoSelecaoPersonagem);
            }

            return;
        }

        private List<Texture2D> CarregarImagensPersonagens() {
            List<Texture2D> imagensPersonagens = new();
            string[] pastasPersonagens = Directory.GetDirectories(Path.Combine(ConstantesEditor.CaminhoPastaAssetsRuntime, "Personagens/"));

            foreach(string pasta in pastasPersonagens) {
                string caminhoFormatado = pasta.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                string nomePasta = caminhoFormatado.Split(Path.AltDirectorySeparatorChar).Last();

                string[] spritesPersonagem = Directory.GetFiles(caminhoFormatado);
                foreach(string sprite in spritesPersonagem) {
                    if(Path.GetExtension(sprite) == ExtensoesEditor.Meta || Path.GetFileNameWithoutExtension(sprite) != nomePasta) {
                        continue;
                    }

                    Texture2D spriteCompletoPersonagem = AssetDatabase.LoadAssetAtPath<Texture2D>(sprite);
                    imagensPersonagens.Add(spriteCompletoPersonagem);
                }
            }

            return imagensPersonagens;
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

            reiniciarCriacaoComPrefabPersonagem.Invoke(prefabPersonagem);

            return;
        }

        private void ConfigurarBotoesConfirmacao() {
            regiaoCarregamentoBotoesConfirmacao.Add(botoesConfirmacao.Root);
            botoesConfirmacao.BotaoCancelar.clicked += HandleBotaoCancelarClick;

            return;
        }

        private void HandleBotaoCancelarClick() {
            if(nomePrefabAtual == prefabOriginal.name) {
                return;
            }

            reiniciarCriacaoComPrefabPersonagem.Invoke(prefabOriginal);
            return;
        }
    }
}