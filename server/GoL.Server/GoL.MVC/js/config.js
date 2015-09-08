(function(GAME, undefined) {
    'use strict';

    GAME.Config = {
        DEBUG: false,
        cellSize: 3,
        cellColor: '#000000',
        elements: {
            canvas: '#game'
        },
        events: {
            CLICK: 'click'
        },
        controls: {
            previous: '#previous',
            play: '#play',
            pause: '#pause',
            next: '#next'
        }
    };

}(window.GAME = window.GAME || {}))
