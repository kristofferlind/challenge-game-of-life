(function(GAME, undefined) {
    'use strict';

    var config = GAME.Config,
        events = config.events,
        Universe = GAME.Universe,
        View = GAME.View,
        paused = false,
        rewinding = false;

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
        if (generation == 1000) {
            console.log('local rewind to start no longer possible');
        }
    };

    $.connection.hub.start();

    //controls
    var controls = config.controls,
        previous = document.querySelector(controls.previous),
        rewindElement = document.querySelector(controls.rewind),
        play = document.querySelector(controls.play),
        pause = document.querySelector(controls.pause),
        next = document.querySelector(controls.next),
        canvas = document.querySelector(config.elements.canvas);

    var handlePrevious = function () {
        var status = Universe.devolve();
        View.render(Universe.cells);
        return status;
    };

    var handlePlay = function () {
        gameHub.server.startSimulation(Universe.cells, Universe.generation);
        paused = false;
        rewinding = false;
    };

    var handlePause = function () {
        gameHub.server.pauseSimulation();
        paused = true;
        rewinding = false;
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

    var handleRewind = function () {
        console.log('current generation: ', GAME.Universe.generation);
        rewinding = true;
        rewind();
    };

    var rewind = function () {
        var success = false;
        if (rewinding) {
            success = handlePrevious();
        }
        if (success) {
            window.requestAnimationFrame(rewind);
        }
    };

    //hooks
    previous.addEventListener(events.CLICK, handlePrevious, false);
    rewindElement.addEventListener(events.CLICK, handleRewind, false);
    play.addEventListener(events.CLICK, handlePlay, false);
    pause.addEventListener(events.CLICK, handlePause, false);
    next.addEventListener(events.CLICK, handleNext, false);
    canvas.addEventListener(events.CLICK, handleCellToggle, false);
    document.addEventListener(events.RESIZE, GAME.View.handleResize, false);

}(window.GAME = window.GAME || {}))
