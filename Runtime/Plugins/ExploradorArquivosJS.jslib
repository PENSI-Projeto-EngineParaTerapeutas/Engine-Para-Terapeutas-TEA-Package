mergeInto(LibraryManager.library, {
    InicializarExploradorArquivosJS: function(nomeGameObjectDestinatarioFormatoC, nomeMetodoDestinatarioFormatoC) {
        nomeGameObjectDestinatario = UTF8ToString(nomeGameObjectDestinatarioFormatoC);
        nomeMetodoDestinatario = UTF8ToString(nomeMetodoDestinatarioFormatoC);

        var exploradorArquivos = document.getElementById("exploradorArquivos");
        if(exploradorArquivos) {
            return;
        }

        exploradorArquivos = document.createElement("input");
        exploradorArquivos.setAttribute("style", "display: none;");
        exploradorArquivos.setAttribute("type", "file");
        exploradorArquivos.setAttribute("id", "exploradorArquivos");
        exploradorArquivos.setAttribute("class", "nonfocused");
        document.getElementsByTagName("body")[0].appendChild(exploradorArquivos);

        exploradorArquivos.onchange = function(evt) {
            const files = evt.target.files;

            if(files.length === 0) {
                ReiniciarExploradorArquivosJS();
                return;
            }

            SendMessage(nomeGameObjectDestinatario, nomeMetodoDestinatario, URL.createObjectURL(files[0]));
        };

        return;
    },

    RequisitarSelecaoArquivoExploradorJS: function(extensoesFormatoC) {
        const extensoes = UTF8ToString(extensoesFormatoC);
        const exploradorArquivos = document.getElementById("exploradorArquivos");

        if(!exploradorArquivos) {
            InicializarExploradorArquivosJS(nomeGameObjectDestinatario, nomeMetodoDestinatario);
        }

        if(extensoes || extensoes.match(/^ *$/) === null) {
            exploradorArquivos.setAttribute("accept", extensoes);
        }

        exploradorArquivos.setAttribute("class", "focused");
        exploradorArquivos.click();

        return;
    },

    ReiniciarExploradorArquivosJS: function() {
        const exploradorArquivos = document.getElementById("exploradorArquivos");
        if(!exploradorArquivos) {
            return;
        }

        exploradorArquivos.setAttribute("class", "nonfocused");

        return;
    },
});