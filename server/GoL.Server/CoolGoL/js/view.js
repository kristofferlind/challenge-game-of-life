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
        xOffset = Math.floor(width / 2),
        yOffset = Math.floor(height /2),
        cellXOffset = Math.floor((width / config.cellSize) / 2),
        cellYOffset = Math.floor((height / config.cellSize) / 2),
        userList = document.querySelector(config.elements.userList),
        seedList = document.querySelector(config.elements.seedList),
        //Render cell on canvas
        renderCell = function(cell) {
            context.beginPath();
            context.rect(cell.x * config.cellSize + xOffset, cell.y * config.cellSize + yOffset, config.cellSize, config.cellSize);
            context.fill();
        },
        //Check if cell is within our window to the universe
        isInsideContext = function(cell) {
            var isInsideLeftBorder = cell.x >= 0 -cellXOffset,
                isInsideRightBorder = cell.x < cellsWidth -cellXOffset,
                isInsideTopBorder = cell.y >= 0 -cellYOffset,
                isInsideBottomBorder = cell.y < cellsHeight -cellYOffset;

            return isInsideLeftBorder && isInsideTopBorder && isInsideRightBorder && isInsideBottomBorder;
        };


    canvas.height = height;
    canvas.width = width;
    context.fillStyle = config.cellColor;
    context.strokeStyle = config.cellColor;

    var getCellByPixelOffset = function (pixelOffsetX, pixelOffsetY) {
        var cell = {
            x: Math.floor((pixelOffsetX - xOffset) / config.cellSize),
            y: Math.floor((pixelOffsetY - yOffset) / config.cellSize)
        };

        return cell;
    };

    var handleResize = function () {
        width = canvas.clientWidth;
        height = canvas.clientHeight;
        cellsWidth = width / config.cellSize;
        cellsHeight = height / config.cellSize;
        xOffset = Math.floor(width / 2);
        yOffset = Math.floor(height /2);
        cellXOffset = Math.floor((width / config.cellSize) / 2);
        cellYOffset = Math.floor((height / config.cellSize) / 2);
        canvas.height = height;
        canvas.width = width;

        console.log(canvas.width);
    };

    var renderUserList = function (users) {
        userList.innerHtml = "";
        users.forEach(function(user) {
            var userElement = createUserElement(user);
            userList.appendChild(userElement);
        });
    };

    var createUserElement = function (user) {
        var userElement = document.createElement("li"),
            userLink = document.createElement("a");
        
        userLink.href = "https://localhost:44306/home/observe/?" + user.username;
        userLink.text = user.username;

        userElement.appendChild(userLink);
        return userElement;
    }

    var renderSeedList = function (seeds, callback) {
        seedList.innerHtml = "";

        for (name in seeds) {
            var seed = {
                name: name,
                cells: seeds[name]
            };

            var seedElement = createSeedElement(seed);
            seedList.appendChild(seedElement);
        }

        //seeds.forEach(function (seed) {
        //    var seedElement = createSeedElement(seed);
        //    seedList.appendChild(seedElement);
        //});
    };
    
    var createSeedElement = function (seed, callback) {
        var seedElement = document.createElement("li"),
        seedLink = document.createElement("a");
        seedLink.href = "#";
        seedLink.text = seed.name;

        seedLink.onclick = function (event) {
            event.preventDefault();
            GAME.View.play(seed.cells);
            //callback(seed.cells);
        };

        seedElement.appendChild(seedLink);
        return seedElement;
    };

    GAME.View = {
        getCellByPixelOffset: getCellByPixelOffset,
        handleResize: handleResize,
        play: null,
        registerPlay: function (func) {
            GAME.View.play = func;
        },
        //Render window into universe
        renderUserList: renderUserList,
        renderSeedList: renderSeedList,
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
