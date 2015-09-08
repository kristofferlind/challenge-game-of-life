(function (GAME, undefined) {
    'use strict';

    var historyBuffer = function (size) {
        var array = new Array(size),
            length = 0;


        var buffer = {
            get: function (generation) {
                if (generation < 0 || generation < length - array.length) {
                    return undefined;
                }
                return array[generation % array.length];
            },
            set: function (generation, cells) {
                if (generation < 0 || generation < length - array.length) {
                    console.error('indexError', generation);
                }
                while (generation > length) {
                    array[length % array.length] = undefined;
                    length += 1;
                }
                array[generation % array.length] = cells;
                if (generation == length) {
                    length += 1;
                }
                console.log(array, length);
            }
        };

        return buffer;
    };

    var history = historyBuffer(100);

    GAME.History = {
        get: history.get,
        set: history.set
    };

}(window.GAME = window.GAME || {}))
