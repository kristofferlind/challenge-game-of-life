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
            CLICK: 'click',
            RESIZE: 'resize'
        },
        controls: {
            previous: '#previous',
            play: '#play',
            pause: '#pause',
            next: '#next',
            rewind: '#rewind'
        }
    };

}(window.GAME = window.GAME || {}))
