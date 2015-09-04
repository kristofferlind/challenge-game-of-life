(function(GAME, undefined) {
    'use strict';

    //signalr
    var gameHub = $.connection.gameHub;
    gameHub.logging = true;
    $.connection.hub.logging = true;
    $.connection.hub.error(function(error) {
        console.log('signalr error', error);
    });

    //Next universe step (from signalr)
    gameHub.client.nextUniverseStep = function(cells) {
        GAME.View.render(cells);
    };

    $.connection.hub.start();

}(window.GAME = window.GAME || {}))
