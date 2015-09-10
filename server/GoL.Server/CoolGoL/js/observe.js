(function (GAME, undefined) {
    'use strict';

    var config = GAME.Config,
        View = GAME.View;

    //signalr
    var gameHub = $.connection.gameHub;
    $.connection.hub.logging = config.DEBUG;
    $.connection.hub.error(function (error) {
        console.log('signalr error', error);
    });

    //Next universe step (from signalr)
    gameHub.client.nextUniverseStep = function (cells, generation) {
        View.render(cells);
    };


    $.connection.hub.start().done(function () {
        gameHub.server.watch(GAME.ObserveUser);
    });

}(window.GAME = window.GAME || {}))
