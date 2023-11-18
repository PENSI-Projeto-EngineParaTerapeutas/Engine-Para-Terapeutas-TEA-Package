using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using Autis.Editor.Constantes;
using Autis.Runtime.Eventos;

namespace Autis.Editor.Utils {
    public static class Importador {
        public static T ImportarAsset<T>(string caminho) where T : UnityEngine.Object {
            return AssetDatabase.LoadAssetAtPath<T>(Path.Combine(ConstantesEditor.CaminhoPastaEditor, caminho));
        }

        public static Texture ImportarImagem(string caminho) {
            return AssetDatabase.LoadAssetAtPath<Texture>(Path.Combine(ConstantesEditor.CaminhoPastaImagensEditor, caminho));
        }

        public static GameObject ImportarPrefab(string caminho) {
            return AssetDatabase.LoadAssetAtPath<GameObject>(Path.Combine(ConstantesEditor.CaminhoPastaRuntime, "Prefabs/", caminho));
        }

        public static VisualTreeAsset ImportarUXML(string caminho) {
            return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Path.Combine(ConstantesEditor.CaminhoPastaScriptsEditor, caminho));
        }

        public static StyleSheet ImportarUSS(string caminho) {
            return AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(ConstantesEditor.CaminhoPastaScriptsEditor, caminho));
        }

        public static List<Texture> ImportarSpriteCompletoPersonagensAvatar() {
            return ImportarSpriteCompletoPersonagens("Personagens/Avatar/");
        }

        public static List<Texture> ImportarSpriteCompletoPersonagensLudicos() {
            return ImportarSpriteCompletoPersonagens("Personagens/Ludicos/");
        }

        private static List<Texture> ImportarSpriteCompletoPersonagens(string caminhoPastaTipo) {
            List<Texture> imagensPersonagens = new();
            string[] pastasPersonagens = Directory.GetDirectories(Path.Combine(ConstantesEditor.CaminhoPastaAssetsRuntime, caminhoPastaTipo));

            foreach(string pasta in pastasPersonagens) {
                string caminhoFormatado = pasta.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                string nomePasta = caminhoFormatado.Split(Path.AltDirectorySeparatorChar).Last();

                string[] spritesPersonagem = Directory.GetFiles(caminhoFormatado);
                foreach(string sprite in spritesPersonagem) {
                    if(Path.GetExtension(sprite) == ExtensoesEditor.Meta || Path.GetFileNameWithoutExtension(sprite) != nomePasta) {
                        continue;
                    }

                    Texture spriteCompletoPersonagem = AssetDatabase.LoadAssetAtPath<Texture>(sprite);
                    imagensPersonagens.Add(spriteCompletoPersonagem);
                }
            }

            return imagensPersonagens;
        }

        public static EventoJogo ImportarEvento(string nomeEvento) {
            return AssetDatabase.LoadAssetAtPath<EventoJogo>(Path.Combine(ConstantesEditor.CaminhoPastaEventosEditor, nomeEvento + ExtensoesEditor.ScriptableObject));
        }
    }
}