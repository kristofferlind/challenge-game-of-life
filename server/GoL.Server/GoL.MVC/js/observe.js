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

    gameHub.server.watch(GAME.ObserveUser);

    $.connection.hub.start();

}(window.GAME = window.GAME || {}))
