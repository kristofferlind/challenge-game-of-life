(function(GAME, undefined) {
    'use strict';

    var indexedBoard = {};
    var potentialCells = {};

    var findCell = function (x, y) {
        return indexedBoard[x] && indexedBoard[x][y];
    };

    var findPotentialCell = function (x, y) {
        return potentialCells[x] && potentialCells[x][y];
    };

    var indexBoard = function (cells) {
        indexedBoard = {};
        cells.forEach(function (cell) {
            if (!indexedBoard[cell.x]) {
                indexedBoard[cell.x] = {};
            }
            indexedBoard[cell.x][cell.y] = cell;
        })
    };

    var countNeighbours = function(cell) {
        var neighbours = 0;
        for (var x = cell.x -1; x <= cell.x + 1; x += 1) {
            for (var y = cell.y -1; y <= cell.y + 1; y += 1) {
                if (!(x == cell.x && y == cell.y)) {
                    if (findCell(x, y)) {
                        neighbours += 1;
                    } else {
                        if (findPotentialCell(x, y)) {
                            potentialCells[x][y].neighbourCount += 1;
                        } else {
                            if (!potentialCells[x]) {
                                potentialCells[x] = {};
                            }
                            potentialCells[x][y] = { x: x, y: y, neighbourCount: 1 };
                        }
                    }
                }
            }
        }
        return neighbours;
    };

    var shouldStayAlive = function (neighbourCount) {
        return (neighbourCount === 2 || neighbourCount === 3);
    };

    var shouldComeToLife = function (neighbourCount) {
        return neighbourCount === 3;
    };

    var nextGeneration = function () {
        var nextGenCells = [],
            currentGenCells = GAME.Universe.cells;

        potentialCells = {};

        currentGenCells.forEach(function (cell) {
            var neighbourCount = countNeighbours(cell)
            if (shouldStayAlive(neighbourCount)) {
                nextGenCells.push(cell);
            }
        });

        for (var x in potentialCells) {
            for (var y in potentialCells[x]) {
                if (shouldComeToLife(potentialCells[x][y].neighbourCount)) {
                    var potentialCell = potentialCells[x][y];
                    var cell = {
                        x: potentialCell.x,
                        y: potentialCell.y
                    };
                    nextGenCells.push(cell);
                }
            }
        }

        return nextGenCells;
    };

    var killCell = function(cell) {
        var cellIndex = GAME.Universe.cells.indexOf(cell);
        if (cellIndex > -1) {
            GAME.Universe.cells.splice(cellIndex, 1);
        }
    };

    GAME.Universe = {
        generation: 0,
        cells: [],
        history: GAME.History,
        loadGeneration: function (generation) {
            var cells = GAME.History.get(generation);
            if (cells) {
                GAME.Universe.cells = cells;
                GAME.Universe.generation = generation;
                return true;
            } else {
                //load cells from api
                return false;
            }
        },
        toggleCell: function (cell) {
            indexBoard(GAME.Universe.cells);
            if (findCell(cell.x, cell.y)) {
                var theCell = findCell(cell.x, cell.y)
                killCell(theCell);
            } else {
                GAME.Universe.cells.push(cell);
            }
        },
        devolve: function () {
            GAME.Universe.generation -= 1;
            var success = GAME.Universe.loadGeneration(GAME.Universe.generation);
            if (!success) {
                GAME.Universe.generation += 1;
            }
            return success;
        },
        evolve: function () {
            GAME.History.set(GAME.Universe.generation, GAME.Universe.cells);
            GAME.Universe.generation += 1;
            //TODO: evolve universe one step forward
            indexBoard(GAME.Universe.cells);
            var nextGenCells = nextGeneration();
            GAME.Universe.cells = nextGenCells;
        }
    };

}(window.GAME = window.GAME || {}))
