//console.log("MapStylingMarkers loaded")

var styles = {
    Republican: { outline: "black", fill: "#FF0808" },
    Democrat: { outline: "black", fill: "#008DFF" },
    NoStatehood: { outline: "black", fill: "lightgray" },
    //NoStatehood: { outline: "gray", fill: "white" },
};

function onTemplateMarker(o, e) {
    //console.log("Map onTemplateMarker " + e);

    return {
        measure: function (measureInfo) {
            var cont = measureInfo.context;
            var data = measureInfo.data;
            var name = "null";
            if (data.item != null) {
                name = data.item.StateSymbol.toString();
            }
            var height = cont.measureText("M").width;
            var width  = cont.measureText(name).width;
            measureInfo.width  = width;
            measureInfo.height = height;
        },

        render: function (renderInfo) {
            //console.log("Map onTemplateMarker render ");
            var ctx = renderInfo.context;
            if (renderInfo.isHitTestRender) {
                ctx.fillStyle = renderInfo.data.actualItemBrush.fill;
            } else {
                ctx.fillStyle = "black";
            }

            var data = renderInfo.data;
            var name = data.item.StateSymbol.toString();
            var halfWidth  = renderInfo.availableWidth / 2.0;
            var halfHeight = renderInfo.availableHeight / 2.0;

            //console.log("Map onTemplateMarker name=" + name);

            var cx = renderInfo.xPosition;
            var cy = renderInfo.yPosition;

            var x = renderInfo.xPosition - halfWidth;
            var y = renderInfo.yPosition - (halfHeight);

            //if (y < 0) {
            //    y += (halfHeight * 4.0);
            //}

            if (renderInfo.isHitTestRender) {
                ctx.fillRect(x, y, renderInfo.availableWidth, renderInfo.availableHeight);
            } else {
                //console.log("Map onTemplateMarker " + data.item.StateWithBox);
                 
                var winnerParty = data.item.WinnerParty;
                var winnerStyle = styles[winnerParty];
                //if (style) {
                //    ctx.strokeStyle = style.outline;
                //    ctx.fillStyle = style.fill;
                //} else {
                //    ctx.strokeStyle = "black";
                //    ctx.fillStyle = "#929292";
                //}

                //if (data.item.StateWithBox == "true") {
                //    ctx.fillStyle = "#30A510";
                //    ctx.fillRect(x - 3, y - 3, renderInfo.availableWidth + 6, renderInfo.availableHeight + 6);
                //}

                ctx.font = "normal 8px Verdana";
                ctx.fillStyle = "black";

                if (data.item.StateHasLabelBox) {
                    //ctx.fillStyle = "green";
                    //ctx.textAlign = "right";
                    ctx.textAlign = "center";
                    ctx.textBaseline = "middle";
                    ctx.fillText(name, cx, cy + 2);
                    //console.log("Map onTemplateMarker " + name + " " + cy);

                } else {
                    ctx.textAlign = "center";
                    ctx.textBaseline = "top";
                    ctx.fillText(name, cx, cy);
                }
                //ctx.fillStyle = "#D402D4";
                //ctx.fillStyle = "white";
                //ctx.fillStyle = "black";
                //ctx.textAlign = "center";
                //ctx.fillText(name, x, y);
                
                if (data.item.StateHasLabelBox) {
                    ctx.fillStyle   = winnerStyle ? winnerStyle.fill : "orange";
                    ctx.strokeStyle = winnerStyle ? winnerStyle.fill : "red";
                    //ctx.fillRect(x + 16, y, 16, 16);
                    //ctx.strokeStyle = "red";
                    //ctx.strokeRect(cx + 10, cy - 3, 16, renderInfo.availableHeight + 6);
                    ctx.fillRect(cx + 10, cy - 2 - halfHeight, 16, 14);

                    ctx.lineWidth = 0.5;
                    ctx.strokeStyle = "black";
                    ctx.strokeRect(cx + 10, cy - 2 - halfHeight, 16, 14);

                    //ctx.fillStyle = "black";
                    //ctx.fillRect(cx + 10, cy, 16, 1);

                    var winnerElectors = data.item.WinnerElectors;
                    if (winnerElectors > 0) {
                        ctx.font = "normal 8px Verdana";
                        ctx.fillStyle = "white";
                        ctx.fillText(winnerElectors, cx + 10 + 8, cy + 2);
                    }
                }  
            }
        }
    }
}

//igRegisterScript("onTemplateMarker", onTemplateMarker, false);
igRegisterScript("onTemplateMarker", onTemplateMarker, true);
 