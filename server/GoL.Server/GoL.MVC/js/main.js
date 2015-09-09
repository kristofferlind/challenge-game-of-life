(function(GAME, undefined) {
    'use strict';

    var config = GAME.Config,
        events = config.events,
        Universe = GAME.Universe,
        View = GAME.View,
        paused = false;

    //signalr
    var gameHub = $.connection.gameHub;
    $.connection.hub.logging = config.DEBUG;
    $.connection.hub.error(function(error) {
        console.log('signalr error', error);
    });

    //Next universe step (from signalr)
    gameHub.client.nextUniverseStep = function (cells, generation) {
        Universe.cells = cells;
        Universe.history.set(generation, cells);
        Universe.generation = generation || 0;
        View.render(cells);
    };

    $.connection.hub.start();

    //controls
    var controls = config.controls,
        previous = document.querySelector(controls.previous),
        play = document.querySelector(controls.play),
        pause = document.querySelector(controls.pause),
        next = document.querySelector(controls.next),
        canvas = document.querySelector(config.elements.canvas);

    var handlePrevious = function () {
        Universe.devolve();
        View.render(Universe.cells);
    };

    var handlePlay = function () {
        gameHub.server.startSimulation(Universe.cells, Universe.generation);
        paused = false;
    };

    var handlePause = function () {
        gameHub.server.pauseSimulation();
        paused = true;
    };

    var handleNext = function () {
        Universe.evolve();
        View.render(Universe.cells);
    };

    var handleCellToggle = function (event) {
        var cell = View.getCellByPixelOffset(event.offsetX, event.offsetY);
        Universe.toggleCell(cell);
        View.render(Universe.cells);
    };

    //hooks
    previous.addEventListener(events.CLICK, handlePrevious, false);
    play.addEventListener(events.CLICK, handlePlay, false);
    pause.addEventListener(events.CLICK, handlePause, false);
    next.addEventListener(events.CLICK, handleNext, false);
    canvas.addEventListener(events.CLICK, handleCellToggle, false);
    document.addEventListener(events.RESIZE, GAME.View.handleResize, false);

}(window.GAME = window.GAME || {}))
