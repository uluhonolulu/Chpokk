(function () {
    "use strict";

    var appView = Windows.UI.ViewManagement.ApplicationView;
    var nav = WinJS.Navigation;

    WinJS.Namespace.define("Application", {
        PageControlNavigator: WinJS.Class.define(
            //Defina a função de construtor para o PageControlNavigator.
            function PageControlNavigator(element, options) {
                this._element = element || document.createElement("div");
                this._element.appendChild(this._createPageElement());

                this.home = options.home;
                this._lastViewstate = appView.value;

                nav.onnavigated = this._navigated.bind(this);
                window.onresize = this._resized.bind(this);

                document.body.onkeyup = this._keyupHandler.bind(this);
                document.body.onkeypress = this._keypressHandler.bind(this);
                document.body.onmspointerup = this._mspointerupHandler.bind(this);

                Application.navigator = this;
            }, {
                home: "",
                /// <field domElement="true" />
                _element: null,
                _lastNavigationPromise: WinJS.Promise.as(),
                _lastViewstate: 0,

                // Este é o objeto Page carregado atualmente.
                pageControl: {
                    get: function () { return this.pageElement && this.pageElement.winControl; }
                },

                // Este é o elemento raiz da página atual.
                pageElement: {
                    get: function () { return this._element.firstElementChild; }
                },

                // Cria um contêiner no qual carregar uma nova página.
                _createPageElement: function () {
                    var element = document.createElement("div");
                    element.setAttribute("dir", window.getComputedStyle(this._element, null).direction);
                    element.style.width = "100%";
                    element.style.height = "100%";
                    return element;
                },

                // Recupera uma lista dos elementos de animação da página atual.
                // Se a página não definir uma lista, anime toda a página.
                _getAnimationElements: function () {
                    if (this.pageControl && this.pageControl.getAnimationElements) {
                        return this.pageControl.getAnimationElements();
                    }
                    return this.pageElement;
                },

                // Navega de volta sempre que a tecla backspace é pressionada e
                // não capturada por um campo de entrada.
                _keypressHandler: function (args) {
                    if (args.key === "Backspace") {
                        nav.back();
                    }
                },

                // Navega de volta ou avança quando as combinações de teclas alt + seta para esquerda ou
                // alt + seta para direita são pressionadas.
                _keyupHandler: function (args) {
                    if ((args.key === "Left" && args.altKey) || (args.key === "BrowserBack")) {
                        nav.back();
                    } else if ((args.key === "Right" && args.altKey) || (args.key === "BrowserForward")) {
                        nav.forward();
                    }
                },

                // Essa função responde a cliques para permitir a navegação usando
                // os botões voltar e avançar do mouse.
                _mspointerupHandler: function (args) {
                    if (args.button === 3) {
                        nav.back();
                    } else if (args.button === 4) {
                        nav.forward();
                    }
                },

                // Responde à navegação com a adição de novas páginas ao DOM.
                _navigated: function (args) {
                    var newElement = this._createPageElement();
                    var parentedComplete;
                    var parented = new WinJS.Promise(function (c) { parentedComplete = c; });

                    this._lastNavigationPromise.cancel();

                    this._lastNavigationPromise = WinJS.Promise.timeout().then(function () {
                        return WinJS.UI.Pages.render(args.detail.location, newElement, args.detail.state, parented);
                    }).then(function parentElement(control) {
                        var oldElement = this.pageElement;
                        if (oldElement.winControl && oldElement.winControl.unload) {
                            oldElement.winControl.unload();
                        }
                        this._element.appendChild(newElement);
                        this._element.removeChild(oldElement);
                        oldElement.innerText = "";
                        this._updateBackButton();
                        parentedComplete();
                        WinJS.UI.Animation.enterPage(this._getAnimationElements()).done();
                    }.bind(this));

                    args.detail.setPromise(this._lastNavigationPromise);
                },

                // Responde a eventos de redimensionamento e chama a função updateLayout
                // na página carregada atualmente.
                _resized: function (args) {
                    if (this.pageControl && this.pageControl.updateLayout) {
                        this.pageControl.updateLayout.call(this.pageControl, this.pageElement, appView.value, this._lastViewstate);
                    }
                    this._lastViewstate = appView.value;
                },

                // Atualiza o estado do botão Voltar. Chamado após a conclusão
                // da navegação.
                _updateBackButton: function () {
                    var backButton = this.pageElement.querySelector("header[role=banner] .win-backbutton");
                    if (backButton) {
                        backButton.onclick = function () { nav.back(); };

                        if (nav.canGoBack) {
                            backButton.removeAttribute("disabled");
                        } else {
                            backButton.setAttribute("disabled", "disabled");
                        }
                    }
                },
            }
        )
    });
})();
