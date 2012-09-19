Server = (function () {
    var module = function () { };
    var server = sinon.fakeServer.create();
    var setResponse = function () { };

    module.stubContinuation = function (continuation) {
        setResponse = function (request) {
            server.respondWith([200, {
                'Content-Type': 'application/json',
                'X-Correlation-Id': request.correlationId
            }, JSON.stringify(continuation)
            ]);
        };
    };

    module.onStart = function (request) {
        module.reset();
        setResponse(request);
    };

    module.respond = function () {
        server.respond();
    };

    module.reset = function () {
        server.restore();
        server = sinon.fakeServer.create();
    };

    amplify.subscribe('AjaxStarted', module.onStart);

    return module;
} ());