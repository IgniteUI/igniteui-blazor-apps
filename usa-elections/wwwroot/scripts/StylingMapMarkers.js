//console.log("MapStylingMarkers loaded")

//var styles = {
//    Republican: { outline: "black", fill: "#FF0808" },
//    Democrat: { outline: "black", fill: "#008DFF" },
//    NoStatehood: { outline: "black", fill: "lightgray" },
//    //NoStatehood: { outline: "gray", fill: "white" },
//};

function onShapeMarker(o, e) {
    //console.log("Map onShapeMarker " + e);

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
            //console.log("Map onShapeMarker render ");
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

            //console.log("Map onShapeMarker name=" + name);

            var cx = renderInfo.xPosition;
            var cy = renderInfo.yPosition;

            var x = renderInfo.xPosition - halfWidth;
            var y = renderInfo.yPosition - (halfHeight);
            
            if (renderInfo.isHitTestRender) {
                ctx.fillRect(x, y, renderInfo.availableWidth, renderInfo.availableHeight);
            } else {
                //console.log("Map onShapeMarker " + data.item.StateWithBox);
                 
                var winnerParty = data.item.WinnerParty;
                var winnerStyle = PartyStyles[winnerParty];
               

                //if (data.item.StateWithBox == "true") {
                //    ctx.fillStyle = "#30A510";
                //    ctx.fillRect(x - 3, y - 3, renderInfo.availableWidth + 6, renderInfo.availableHeight + 6);
                //}
                        
                if (data.item.StateHasLabelBox) {
                    ctx.fillStyle   = winnerStyle ? winnerStyle.fill : "orange";
                    ctx.strokeStyle = winnerStyle ? winnerStyle.fill : "red";

                    //cx = cx + halfHeight + 10;
                    ctx.fillRect(cx, cy - 2 - halfHeight, 20, 14);
                    //ctx.fillRect(cx + 10, cy - 2 - halfHeight, 16, 14);

                    ctx.lineWidth = 0.5;
                    ctx.strokeStyle = "white";
                    ctx.strokeRect(cx, cy - 2 - halfHeight, 20, 14);
                    //ctx.strokeRect(cx + 10, cy - 2 - halfHeight, 16, 14);

                    //var winnerElectors = data.item.WinnerElectors;
                    //if (winnerElectors > 0) {
                    //    ctx.font = "normal 8px Verdana";
                    //    ctx.fillStyle = "white";
                    //    ctx.fillText(winnerElectors, cx + 10 + 8, cy + 2);
                    //}
                }  

                ctx.font = "normal 8px Verdana";
                ctx.fillStyle = data.item.StateSymbol == "HI" ? "black" : "white";
                if (data.item.StateSymbol == "AK") {
                    cy = cy - 3;
                }
                else if (data.item.StateSymbol == "TN") {
                    cy = cy - 2;
                }
                else if (data.item.StateSymbol == "ME") {
                    cy = cy - 4;
                }
                else if (data.item.StateSymbol == "PA") {
                    cy = cy - 4;
                }
                else if (data.item.StateSymbol == "KY") {
                    cy = cy - 4;
                }
                else if (data.item.StateSymbol == "WV" ||
                    data.item.StateSymbol == "SD" ||
                    data.item.StateSymbol == "ND" ||
                    data.item.StateSymbol == "NE") {
                    cy = cy - 4;
                }
                else if (data.item.StateSymbol == "FL") {
                    cy = cy - 2;
                }

                if (data.item.StateHasLabelBox) {
                    cx = cx + 10;
                    //ctx.fillStyle = "green";
                    //ctx.textAlign = "right";
                    ctx.textAlign = "center";
                    ctx.textBaseline = "middle";
                    ctx.fillText(name, cx, cy + 2);
                    //console.log("Map onShapeMarker " + name + " " + cy);

                } else {
                    ctx.textAlign = "center";
                    ctx.textBaseline = "top";
                    ctx.fillText(name, cx, cy);
                } 
            }
        }
    }
}

//igRegisterScript("onShapeMarker", onShapeMarker, false);
igRegisterScript("onShapeMarker", onShapeMarker, true);






