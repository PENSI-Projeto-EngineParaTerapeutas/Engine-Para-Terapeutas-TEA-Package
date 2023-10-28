using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Autis.Editor.Telas;
using Autis.Editor.UI;
using Autis.Editor.Utils;
using Autis.Editor.Constantes;
using Autis.Editor.Manipuladores;

namespace Autis.Editor.Criadores {
    public class PersonalizacaoAvatarBehaviour : Tela, IReiniciavel {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorPersonagem/PersonalizacaoAvatar/PersonalizacaoAvatarTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorPersonagem/PersonalizacaoAvatar/PersonalizacaoAvatarStyle.uss";

        #region .: Mensagens :.

        private const string MENSAGEM_ERRO_CARREGAR_PREFAB_PERSONAGEM = "[ERROR]: Não foi possível carregar o prefab para o Avatar {nome}. Certifique-se de que o prefab está localizado em: <Pacote>/Prefabs/Personagens/Ludico_{nome}.prefab. Além disso, garanta que o nome do sprite completo para o Personagem Lúdico equivale ao final do nome no prefab.";

        private const string MENSAGEM_TOOLTIP_DROPDOWN_CABELOS = "[TODO]: Adicionar mensagem.";
        private const string MENSAGEM_TOOLTIP_DROPDOWN_ROUPAS = "[TODO]: Adicionar mensagem.";

        #endregion

        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_TIPOS_PERSONAGEM = "regiao-tipos-persoangens";
        private VisualElement regiaoCarregamentoTiposPersonagens;

        private const string NOME_REGIAO_CARREGAMENTO_DROPDOWN_CABELOS = "regiao-carregamento-dropdown-cabelos";
        private VisualElement regiaoCarregamentoDropdownCabelos;

        private const string NOME_REGIAO_CARREGAMENTO_INPUT_COR_CABELO = "regiao-carregamento-cor-cabelo";
        private VisualElement regiaoCarregamentoInputCorCabelo;

        private const string NOME_REGIAO_CARREGAMENTO_INPUT_COR_OLHOS = "regiao-carregamento-cor-olhos";
        private VisualElement regiaoCarregamentoInputCorOlhos;

        private const string NOME_REGIAO_CARREGAMENTO_INPUT_COR_PELE = "regiao-carregamento-cor-pele";
        private VisualElement regiaoCarregamentoInputCorPele;

        private const string NOME_REGIAO_CARREGAMENTO_DROPDOWN_ROUPAS = "regiao-carregamento-dropdown-roupas";
        private VisualElement regiaoCarregamentoDropdownRoupas;

        private const string NOME_REGIAO_CARREGAMENTO_INPUT_COR_ROUPA_SUPERIOR = "regiao-carregamento-cor-roupa-superior";
        private VisualElement regiaoCarregamentoInputCorRoupaSuperior;

        private const string NOME_REGIAO_CARREGAMENTO_INPUT_COR_ROUPA_INFERIOR = "regiao-carregamento-cor-roupa-inferior";
        private VisualElement regiaoCarregamentoInputCorRoupaInferior;

        private const string NOME_REGIAO_CARREGAMENTO_INPUT_IMAGEM_ROSTO = "regiao-carregamento-upload-imagem-rosto";
        private VisualElement regiaoCarregamentoInputImagemRosto;

        protected const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        protected VisualElement regiaoCarregamentoBotoesConfirmacao;

        private Dropdown dropdownCabelos;
        private InputCor inputCorCabelo;
        private InputCor inputCorOlhos;
        private InputCor inputCorPele;
        private Dropdown dropdownRoupas;
        private InputCor inputCorRoupaSuperior;
        private InputCor inputCorRoupaInferior;
        private InputImagem inputImagemRosto;
        protected BotoesConfirmacao botoesConfirmacao;

        #endregion

        public Action OnConfirmarCriacao { get; set; }

        private readonly GameObject avatarOriginal;
        private readonly ManipuladorAvatar manipuladorAvatar;

        private string nomePrefabAtual = string.Empty;

        public PersonalizacaoAvatarBehaviour(ManipuladorAvatar manipuladorAvatar) {
            this.manipuladorAvatar = manipuladorAvatar;

            avatarOriginal = GameObject.Instantiate(manipuladorAvatar.ObjetoAtual);
            avatarOriginal.SetActive(false);

            CarregarTiposPersonagem();
            ConfigurarInputsTiposCabelos();
            ConfigurarInputCorCabelo();
            ConfigurarInputCorOlhos();
            ConfigurarInputCorPele();
            ConfigurarDropdownRoupas();
            ConfigurarInputCorRoupaSuperior();
            ConfigurarInputCorRoupaInferior();
            ConfigurarBotoesConfirmacao();

            if(manipuladorAvatar.PossuiPersonagemSelecionado()) {
                CarregarValoresJaDefinidos();
            }

            return;
        }

        private void CarregarValoresJaDefinidos() {
            inputCorCabelo.CampoCor.SetValueWithoutNotify(manipuladorAvatar.GetCorCabelo());
            inputCorPele.CampoCor.SetValueWithoutNotify(manipuladorAvatar.GetCorPele());
            inputCorRoupaSuperior.CampoCor.SetValueWithoutNotify(manipuladorAvatar.GetCorRoupaSuperior());
            inputCorRoupaInferior.CampoCor.SetValueWithoutNotify(manipuladorAvatar.GetCorRoupaInferior());

            return;
        }

        private void CarregarTiposPersonagem() {
            List<Texture> spritesAvatares = Importador.ImportarSpriteCompletoPersonagensAvatar();

            regiaoCarregamentoTiposPersonagens = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TIPOS_PERSONAGEM);
            
            foreach(Texture imagemPersonagem in spritesAvatares) {
                Image displayImagemPersonagem = new() {
                    image = imagemPersonagem,
                };

                Button botaoSelecaoPersonagem = new();
                botaoSelecaoPersonagem.Add(displayImagemPersonagem);

                botaoSelecaoPersonagem.clicked += (() => {
                    AlterarPesonagem(imagemPersonagem.name);
                });

                regiaoCarregamentoTiposPersonagens.Add(botaoSelecaoPersonagem);
            }

            return;
        }

        private void AlterarPesonagem(string nomeImagemPersonagem) {
            string nomePrefabNovo = "Avatar_" + nomeImagemPersonagem;
            if(nomePrefabNovo == nomePrefabAtual) {
                return;
            }

            nomePrefabAtual = nomePrefabNovo;
            GameObject prefabPersonagem = Importador.ImportarPrefab(Path.Combine("Personagens", nomePrefabAtual + ExtensoesEditor.Prefab));

            if(prefabPersonagem == null) {
                Debug.LogError(MENSAGEM_ERRO_CARREGAR_PREFAB_PERSONAGEM.Replace("{nome}", nomeImagemPersonagem));
            }

            manipuladorAvatar.AlterarPrefab(prefabPersonagem);
            ReiniciarCampos();

            botoesConfirmacao.BotaoConfirmar.SetEnabled(manipuladorAvatar.PossuiPersonagemSelecionado());

            return;
        }

        private void ConfigurarInputsTiposCabelos() {
            List<string> opcoesCabelos = new() {
                "Cabelo 1",
                "Cabelo 2",
                "Cabelo 3",
            };

            dropdownCabelos = new Dropdown("Cabelos:", MENSAGEM_TOOLTIP_DROPDOWN_CABELOS, opcoesCabelos);
            dropdownCabelos.Campo.RegisterCallback<ChangeEvent<string>>(evt => {
                manipuladorAvatar.SetCabelo(opcoesCabelos.FindIndex(opcao => opcao == evt.newValue) - 1);
            });

            regiaoCarregamentoDropdownCabelos = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_DROPDOWN_CABELOS);
            regiaoCarregamentoDropdownCabelos.Add(dropdownCabelos.Root);

            return;
        }

        private void ConfigurarInputCorCabelo() {
            inputCorCabelo = new InputCor("Cor do cabelo:");

            inputCorCabelo.CampoCor.RegisterCallback<ChangeEvent<Color>>(evt => {
                manipuladorAvatar.SetCorCabelo(evt.newValue);
            });

            regiaoCarregamentoInputCorCabelo = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_COR_CABELO);
            regiaoCarregamentoInputCorCabelo.Add(inputCorCabelo.Root);

            return;
        }

        private void ConfigurarInputCorOlhos() {
            inputCorOlhos = new InputCor("Cor dos olhos:");

            inputCorOlhos.CampoCor.RegisterCallback<ChangeEvent<Color>>(evt => {
                manipuladorAvatar.SetCorOlhos(evt.newValue);
            });

            regiaoCarregamentoInputCorOlhos = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_COR_OLHOS);
            regiaoCarregamentoInputCorOlhos.Add(inputCorOlhos.Root);

            return;
        }

        private void ConfigurarInputCorPele() {
            inputCorPele = new InputCor("Cor da pele:");

            inputCorPele.CampoCor.RegisterCallback<ChangeEvent<Color>>(evt => {
                manipuladorAvatar.SetCorPele(evt.newValue);
            });

            regiaoCarregamentoInputCorPele = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_COR_PELE);
            regiaoCarregamentoInputCorPele.Add(inputCorPele.Root);

            return;
        }

        private void ConfigurarDropdownRoupas() {
            List<string> opcoesRoupas = new() {
                "Conjunto 1",
                "Conjunto 2",
            };

            dropdownRoupas = new Dropdown("Roupas:", MENSAGEM_TOOLTIP_DROPDOWN_ROUPAS, opcoesRoupas);
            dropdownRoupas.Campo.RegisterCallback<ChangeEvent<string>>(evt => {
                manipuladorAvatar.SetConjuntoRoupas(dropdownRoupas.Campo.choices.FindIndex(opcao => opcao == evt.newValue) - 1);
            });

            regiaoCarregamentoDropdownRoupas = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_DROPDOWN_ROUPAS);
            regiaoCarregamentoDropdownRoupas.Add(dropdownRoupas.Root);

            return;
        }

        private void ConfigurarInputCorRoupaSuperior() {
            inputCorRoupaSuperior = new InputCor("Cor da parte superior da roupa:");

            inputCorRoupaSuperior.CampoCor.RegisterCallback<ChangeEvent<Color>>(evt => {
                manipuladorAvatar.SetCorRoupaSuperior(evt.newValue);
            });

            regiaoCarregamentoInputCorRoupaSuperior = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_COR_ROUPA_SUPERIOR);
            regiaoCarregamentoInputCorRoupaSuperior.Add(inputCorRoupaSuperior.Root);

            return;
        }

        private void ConfigurarInputCorRoupaInferior() {
            inputCorRoupaInferior = new InputCor("Cor da parte inferior da roupa:");

            inputCorRoupaInferior.CampoCor.RegisterCallback<ChangeEvent<Color>>(evt => {
                manipuladorAvatar.SetCorRoupaInferior(evt.newValue);
            });

            regiaoCarregamentoInputCorRoupaInferior = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_COR_ROUPA_INFERIOR);
            regiaoCarregamentoInputCorRoupaInferior.Add(inputCorRoupaInferior.Root);

            return;
        }

        private void ConfigurarInputImagemRosto() {
            inputImagemRosto = new InputImagem();

            regiaoCarregamentoInputImagemRosto = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_IMAGEM_ROSTO);
            regiaoCarregamentoInputImagemRosto.Add(inputImagemRosto.Root);
            regiaoCarregamentoInputImagemRosto.AddToClassList("input-com-label");

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
            GameObject.DestroyImmediate(avatarOriginal);
            OnConfirmarCriacao?.Invoke();

            Navigator.Instance.Voltar();

            return;
        }

        private void HandleBotaoCancelarClick() {
            manipuladorAvatar.Cancelar();

            avatarOriginal.SetActive(true);
            manipuladorAvatar.Editar(avatarOriginal);

            Navigator.Instance.Voltar();

            return;
        }

        public void ReiniciarCampos() {
            inputCorCabelo.ReiniciarCampos();
            inputCorOlhos.ReiniciarCampos();
            inputCorPele.ReiniciarCampos();
            inputCorRoupaInferior.ReiniciarCampos();
            inputCorRoupaSuperior.ReiniciarCampos();
            inputImagemRosto.ReiniciarCampos();
            dropdownRoupas.ReiniciarCampos();

            return;
        }
    }
}