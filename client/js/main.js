(function(GAME, undefined) {
    'use strict';

    //signalr
    // var gameHub = $.connection.gameHub;

    // gameHub.client.nextUniverseStep = function(cells) {
    //     GAME.View.render(cells);
    // };

    //test
    var testCells = [{x: 5, y: 10}, {x: 3, y: 10}, {x: 5, y: 13}, {x: 15, y: 20}];
    GAME.View.render(testCells);

}(window.GAME = window.GAME || {}))
