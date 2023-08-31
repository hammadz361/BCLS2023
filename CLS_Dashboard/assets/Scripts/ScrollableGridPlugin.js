﻿(function ($) {
    $.fn.Scrollable = function (options) {
        var defaults = {
            ScrollHeight: 400,
            Width: 0
        };
        var options = $.extend(defaults, options);
        return this.each(function () {
            var grid = $(this).get(0);
            var gridWidth = grid.offsetWidth;
            var headerCellWidths = new Array();
            for (var i = 0; i < grid.getElementsByTagName("th").length; i++) {
                headerCellWidths[i] = grid.getElementsByTagName("th")[i].offsetWidth;
            }

            

            grid.parentNode.appendChild(document.createElement("div"));
            var parentDiv = grid.parentNode;

            var table = document.createElement("table");
            for (i = 0; i < grid.attributes.length; i++) {
                if (grid.attributes[i].specified && grid.attributes[i].name != "id") {
                    table.setAttribute(grid.attributes[i].name, grid.attributes[i].value);
                }
            }
            table.style.cssText = grid.style.cssText;
            table.style.width = gridWidth + "px";
            table.appendChild(document.createElement("tbody"));
            table.getElementsByTagName("tbody")[0].appendChild(grid.getElementsByTagName("TR")[0]);
            var cells = table.getElementsByTagName("TH");

            var gridRow = grid.getElementsByTagName("TR")[0];
            for (var i = 0; i < cells.length; i++) {
                var width;
               
                width = headerCellWidths[i];
               
                cells[i].style.width = parseInt(width) + "px";
                gridRow.getElementsByTagName("TD")[i].style.width = parseInt(width) + "px";
            }
            parentDiv.removeChild(grid);

            var dummyHeader = document.createElement("div");
            dummyHeader.appendChild(table);
            parentDiv.appendChild(dummyHeader);
            if (options.Width > 0) {
                gridWidth = options.Width;
            }
            var scrollableDiv = document.createElement("div");
            
            var rowcount = grid.getElementsByTagName("tr").length;
            if( rowcount > 8){
            gridWidth = parseInt(gridWidth+16);
            }
            else {
            gridWidth = parseInt(gridWidth);
            }
            scrollableDiv.style.cssText =  "overflow:auto;height:" + options.ScrollHeight + "px;width:" + gridWidth + "px";
            scrollableDiv.appendChild(grid);
            parentDiv.appendChild(scrollableDiv);
        });
    };
})(jQuery);