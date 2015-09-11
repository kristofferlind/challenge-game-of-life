(function (GAME, undefined) {
    'use strict';

    var config = GAME.Config,
        events = config.events,
        Universe = GAME.Universe,
        View = GAME.View,
        noop = function () { };

    var state = {
        playing: false,
        rewinding: false
    };

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

    gameHub.client.updateUserList = function (users) {
        GAME.View.renderUserList(users);
    };

    $.connection.hub.start().done(function () {
        gameHub.server.getSeeds().done(function (seeds) {
            GAME.View.renderSeedList(seeds);
        });
    });

    GAME.View.registerPlay(function (cells) {
        state.playing = true;
        gameHub.server.startSimulation(cells, 0);
    });

    //controls
    var controls = config.controls,
        previous = document.querySelector(controls.previous),
        rewindElement = document.querySelector(controls.rewind),
        play = document.querySelector(controls.play),
        pause = document.querySelector(controls.pause),
        next = document.querySelector(controls.next),
        canvas = document.querySelector(config.elements.canvas);

    var handlePrevious = function () {
        if (!state.playing) {
            var status = Universe.devolve();
            View.render(Universe.cells);
            return status;
        }
    };

    var handlePlay = function () {
        if (!state.rewinding) {
            state.playing = true;
            gameHub.server.startSimulation(Universe.cells, Universe.generation);
        }
    };

    var handlePause = function () {
        gameHub.server.pauseSimulation();
        state.rewinding = false;
        state.playing = false;
    };

    var handleNext = function () {
        if (!(state.playing && state.rewinding)) {
            Universe.evolve();
            View.render(Universe.cells);
        }
    };

    var handleCellToggle = function (event) {
        if (!(state.playing && state.rewinding)) {
            var cell = View.getCellByPixelOffset(event.offsetX, event.offsetY);
            Universe.toggleCell(cell);
            View.render(Universe.cells);
        }
    };

    var handleRewind = function () {
        if (!state.playing) {
            state.rewinding = true;

            var generation = GAME.Universe.generation;

            if ((generation - 1000) > 0) {
                var from = generation - 2000,
                    to = generation - 1000;
                if (from < 0) {
                    from = 0;
                }

                console.log(from, to);

                var complete = {
                    getHistoryBatch: false,
                    rewind: false,
                    historyBatch: []
                };

                var nextHistoryBatch = function () {
                    if (complete.getHistoryBatch && complete.rewind) {
                        GAME.History.clear();
                        complete.historyBatch.forEach(function (generation) {
                            GAME.History.set(generation.generationNumber, generation.cells);
                        });
                        complete = null;
                        console.log('history replaced @ generation: ', GAME.Universe.generation);
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
                rewind(function () {
                    state.rewinding = false;
                });
            }
        }
    };

    var rewind = function (callback, end) {
        var success = false;
        var end = end || 0;

        success = handlePrevious();

        if (success && GAME.Universe.generation > end) {
            window.requestAnimationFrame(function () { rewind(callback, end); });
        } else {
            console.log(GAME.Universe.generation);
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
