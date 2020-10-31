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
    //ctx.globalAlpha = 0.4;
    ctx.beginPath();
    ctx.arc(x, y, r, 0, 2 * Math.PI);
    ctx.stroke();
    ctx.fill();
    //ctx.globalAlpha = 1.0;
}

function onTemplateBubble(o, e) {
    //console.log("Bubble onTemplateBubble ");
     
    var desiredSize = 46; 

    return {
        measure: function (measureInfo) {
            var cont = measureInfo.context;
            var data = measureInfo.data;
            measureInfo.width = desiredSize;
            measureInfo.height = desiredSize;
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
            var halfWidth  = Math.round(renderInfo.availableWidth / 2.0);
            var halfHeight = Math.round(renderInfo.availableHeight / 2.0);

            var cx = renderInfo.xPosition;
            var cy = renderInfo.yPosition;

            var x = renderInfo.xPosition - halfWidth;
            var y = renderInfo.yPosition - halfHeight;

            if (renderInfo.isHitTestRender) {
                //ctx.fillRect(cx - rectHalf, cy - rectHalf, rectSize, rectSize);
                ctx.fillRect(x, y, renderInfo.availableWidth, renderInfo.availableHeight);

            } else {

                //ctx.globalAlpha = 0.4;
                //ctx.strokeStyle = "red";
                //ctx.strokeRect(x, y, renderInfo.availableWidth, renderInfo.availableHeight);
                //ctx.strokeRect(cx, cy, renderInfo.availableWidth, renderInfo.availableHeight);

                var viewportHeight = renderInfo.passInfo.viewportHeight;
                var viewportRatio = viewportHeight / 500.0;
                var markerSize = Math.round(viewportRatio * desiredSize);
                var markerHalf = Math.round(markerSize / 2.0);

                var mx = cx - markerHalf;
                var my = cy - markerHalf;
                var fontSize = Math.round(viewportRatio * 10);
                var lineSize = Math.round(viewportRatio * 6);

                ctx.font = "normal " + fontSize + "px Verdana";

                var winnerParty = data.item.WinnerParty;
                //renderCircle(ctx, cx, cy, 16, winnerParty);
                renderCircle(ctx, cx, cy, markerHalf, winnerParty);
               
                //ctx.fillStyle = "#30A510";
                //ctx.fillRect(x, y, renderInfo.availableWidth, renderInfo.availableHeight);
                                
                ctx.textBaseline = "middle";
                ctx.textAlign = "center";
                ctx.fillStyle = "white";
                ctx.fillText(name, cx, cy - lineSize);
                 
                var winnerElectors = data.item.WinnerElectors;
                if (winnerElectors > 0) { 
                    ctx.fillText(winnerElectors, cx, cy + lineSize);
                     
                    //ctx.strokeStyle = "#24C315"; 
                    //ctx.strokeRect(mx, my, markerSize, markerSize);
                }

                var runnerUpElectors = 2; // data.item.RunnerUpElectors;
                if (runnerUpElectors > 0) {

                    var smallSize = Math.round(markerHalf / 3);
                    var sx = cx + markerHalf - smallSize;
                    var sy = cy + markerHalf - smallSize;
                    var runnerUpParty = data.item.RunnerUpParty;
                    renderCircle(ctx, sx, sy, smallSize, runnerUpParty);
                  //renderCircle(ctx, cx + qw + 2, cy + 6, 6, runnerUpParty);
                     
                    //ctx.font = "normal 10px Verdana";
                    ctx.textBaseline = "middle";
                    ctx.textAlign = "center";
                    ctx.fillStyle = "white";
                    ctx.fillText(runnerUpElectors, sx, sy);
                  //ctx.fillText(runnerUpElectors, cx + qw + 2, cy + 6);
                }
            }
        }
    }
}
 
igRegisterScript("onTemplateBubble", onTemplateBubble, true);
 