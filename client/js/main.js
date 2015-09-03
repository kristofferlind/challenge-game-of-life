(function(GAME, undefined) {
    'use strict';

    //signalr
    var gameHub = $.connection.gameHub;
    console.log(gameHub);
    gameHub.logging = true;

    gameHub.client.nextUniverseStep = function(cells) {
        console.log(cells);
        GAME.View.render(cells);
    };

    $.connection.hub.start().done(function() {
        console.log(gameHub);
    });

    // //test
    // var testCells = [{x: 5, y: 10}, {x: 3, y: 10}, {x: 5, y: 13}, {x: 15, y: 20}];
    // GAME.View.render(testCells);

}(window.GAME = window.GAME || {}))
