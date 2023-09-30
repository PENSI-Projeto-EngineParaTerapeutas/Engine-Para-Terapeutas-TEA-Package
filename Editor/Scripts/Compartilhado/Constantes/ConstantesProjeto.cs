using System.IO;
using UnityEditor;
using UnityEngine;

namespace Autis.Editor.Constantes {
    public static class ConstantesProjeto {
        public static string CaminhoDinamicoPacote {
            get {
                if(!string.IsNullOrWhiteSpace(caminhoDinamicoPacote)) {
                    return caminhoDinamicoPacote;
                }

                string[] assets = AssetDatabase.FindAssets($"t:Script {nameof(Inicializacao)}");
                string caminhoArquivoInicializacao = AssetDatabase.GUIDToAssetPath(assets[0]);

                string caminhoCompletoProjetoUnity = Directory.GetParent(Application.dataPath).FullName;
                string caminhoCompletoPacoteEngineTEA = Directory.GetParent(Path.GetDirectoryName(Path.GetFullPath(caminhoArquivoInicializacao))).Parent.FullName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                caminhoDinamicoPacote = Path.GetRelativePath(caminhoCompletoProjetoUnity, caminhoCompletoPacoteEngineTEA);

                return caminhoDinamicoPacote;
            }
        }
        private static string caminhoDinamicoPacote = string.Empty;

        public static string NomeProjeto => "Autis";
        public static string NomeOrganizacao => "PENSI";
        public static string NomePacote => "com.pensi.engine-para-terapeutas-tea";
        public static string CaminhoAssetDatabaseProjeto => "Packages/com.pensi.engine-para-terapeutas-tea/";
    }
}