(function(GAME, undefined) {
    'use strict';

    var config = GAME.Config,
        elements = config.elements,
        canvas = document.querySelector(elements.canvas),
        context = canvas.getContext('2d'),
        width = canvas.clientWidth,
        height = canvas.clientHeight,
        cellsWidth = width / config.cellSize,
        cellsHeight = height / config.cellSize,
        //Render cell on canvas
        renderCell = function(cell) {
            context.beginPath();
            context.rect(cell.x * config.cellSize, cell.y * config.cellSize, config.cellSize, config.cellSize);
            context.fill();
        },
        //Check if cell is within our window to the universe
        isInsideContext = function(cell) {
            var isInsideLeftBorder = cell.x >= 0,
                isInsideRightBorder = cell.x < cellsWidth,
                isInsideTopBorder = cell.y >= 0,
                isInsideBottomBorder = cell.y < cellsHeight;

            return isInsideLeftBorder && isInsideTopBorder && isInsideRightBorder && isInsideBottomBorder;
        };


    canvas.height = height;
    canvas.width = width;
    context.fillStyle = config.cellColor;
    context.strokeStyle = config.cellColor;

    GAME.View = {
        //Render window into universe
        render: function(cells) {
            context.clearRect(0, 0, width, height);
            cells.forEach(function(cell) {
                if (isInsideContext(cell)) {
                    renderCell(cell);
                }
            });
        }
    };

}(window.GAME = window.GAME || {}))
