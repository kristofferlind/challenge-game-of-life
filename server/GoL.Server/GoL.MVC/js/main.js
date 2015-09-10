(function (GAME, undefined) {
    'use strict';

    var config = GAME.Config,
        events = config.events,
        Universe = GAME.Universe,
        View = GAME.View,
        paused = false,
        rewinding = false,
        noop = function () { };

    //signalr
    var gameHub = $.connection.gameHub;
    $.connection.hub.logging = config.DEBUG;
    $.connection.hub.error(function (error) {
        console.log('signalr error', error);
    });

    //Next universe step (from signalr)
    gameHub.client.nextUniverseStep = function (cells, generation) {
        Universe.cells = cells;
        Universe.generation = generation || 0;
        Universe.history.set(generation, cells);
        View.render(cells);
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
        rewinding = true;

        var currentGeneration = GAME.Universe.generation;

        if ((currentGeneration - 1000) > 0) {
            var from = currentGeneration - 2000,
                to = currentGeneration - 1000;
            if (from < 0) {
                from = 0;
            }

            var complete = {
                getHistoryBatch: false,
                rewind: false,
                historyBatch: []
            };

            var nextHistoryBatch = function () {
                if (complete.getHistoryBatch && complete.rewind) {
                    console.log('fetched history from ', from, ' to ', to, 'batch: ', complete.historyBatch);
                    GAME.History.clear();
                    complete.historyBatch.forEach(function (generation) {
                        GAME.History.set(generation.generationNumber, generation.cells);
                    });
                    complete = null;
                    handleRewind();
                }
            };

            gameHub.server.getHistoryBatch(from, to).done(function (historyBatch) {
                complete.historyBatch = historyBatch;
                complete.getHistoryBatch = true;
                nextHistoryBatch();
            });

            rewind(function () {
                complete.rewind = true;
                nextHistoryBatch();
            }, to);
        } else {
            rewind();
        }
    };

    var rewind = function (callback, end) {
        var success = false;
        var end = end || 0;

        success = handlePrevious();

        if (success && GAME.Universe.generation > end) {
            window.requestAnimationFrame(function () { rewind(callback, end); });
        } else {
            if (callback) {
                callback();
            }
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
