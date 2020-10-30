//console.log("MapStylingBubbles loaded")

//var styles = {
//    Republican: { outline: "black", fill: "#FF0808" },
//    Democrat: { outline: "black", fill: "#008DFF" },
//    NoStatehood: { outline: "black", fill: "lightgray" },
//    //NoStatehood: { outline: "gray", fill: "white" },
//};
function renderCircle(ctx, x, y, r, party) {
    var style = PartyStyles[party];
    if (style) {
        ctx.strokeStyle = style.outline;
        ctx.fillStyle = style.fill;
    } else {
        ctx.strokeStyle = "black";
        ctx.fillStyle = "#929292";
    }

    ctx.beginPath();
    ctx.arc(x, y, r, 0, 2 * Math.PI);
    ctx.stroke();
    ctx.fill();
}

function onTemplateBubble(o, e) {
    //console.log("Bubble onTemplateBubble ");

    var markerSize = 14;
    return {
        measure: function (measureInfo) {
            var cont = measureInfo.context;
            var data = measureInfo.data;
            var name = "null";
            if (data.item != null) {
                name = data.item.StateSymbol.toString();
            }
            var height = markerSize; //cont.measureText("M").width;
            var width = markerSize; //ont.measureText(name).width;
            measureInfo.width  = width;
            measureInfo.height = height;
        },

        render: function (renderInfo) {
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
                //ctx.globalAlpha = 0.4;
                //ctx.strokeStyle = "red";
                //ctx.strokeRect(x, y, renderInfo.availableWidth, renderInfo.availableHeight);
                //ctx.strokeRect(cx, cy, renderInfo.availableWidth, renderInfo.availableHeight);

                var winnerParty = data.item.WinnerParty;
                renderCircle(ctx, cx, cy, 16, winnerParty);
                
                //ctx.fillStyle = "#30A510";
                //ctx.fillRect(x, y, renderInfo.availableWidth, renderInfo.availableHeight);

                var runnerUpElectors = 2; // data.item.RunnerUpElectors;
                if (runnerUpElectors > 0) {
                    //cx = cx - 2;
                    //ctx.textAlign = "right";
                } else {
                    //ctx.textAlign = "center";
                }
                //ctx.font = "bold 10px Verdana";
                ctx.font = "normal 10px Verdana";
                //ctx.textBaseline = "top";
                ctx.textBaseline = "middle";
                ctx.textAlign = "center";
                ctx.fillStyle = "white";
                ctx.fillText(name, cx, cy - 4);

                var winnerElectors = data.item.WinnerElectors;
                if (winnerElectors > 0) { 
                    ctx.font = "normal 10px Verdana";
                    ctx.fillText(winnerElectors, cx, cy + 6); 
                }
                
                if (runnerUpElectors > 0) {
                    var qw = renderInfo.availableWidth / 2.5;
                    var qh = renderInfo.availableHeight / 3.0;
                    cx = cx + qw + 6;
                    cy = cy + qh + 6;
                    var runnerUpParty = data.item.RunnerUpParty;
                    renderCircle(ctx, cx, cy, 6, runnerUpParty);
                    //renderCircle(ctx, cx + qw + 2, cy + 6, 6, runnerUpParty);
                     
                    ctx.font = "normal 10px Verdana";
                    ctx.textBaseline = "middle";
                    ctx.textAlign = "center";
                    ctx.fillStyle = "white";
                    ctx.fillText(runnerUpElectors, cx, cy);
                    //ctx.fillText(runnerUpElectors, cx + qw + 2, cy + 6);
                }
            }
        }
    }
}
 
igRegisterScript("onTemplateBubble", onTemplateBubble, true);
 