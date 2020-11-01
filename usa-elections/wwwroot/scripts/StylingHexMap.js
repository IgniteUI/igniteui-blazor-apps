//console.log("MapStylingShapes v1.0.1")

function onHexShapeStyle(o, e) {

    // styling shapes based on data fields for all states in USA
    //var Name = e.item.getFieldValue("Name");
    var Code = e.item.getFieldValue("Code");
    var WinnerParty = e.item.getFieldValue("WinnerParty");
    var ElectionYear = e.item.getFieldValue("ElectionYear");
    var Statehood = e.item.getFieldValue("Statehood");

    var WinnerParty = e.item.getFieldValue("WinnerParty");

    //if (Code == "DC" || Code == "TX") {
    //    console.log("onHexShapeStyle " + Code + " " + WinnerParty + " " + StateCreation);
    //}

    // hiding shapes until map is zoomed in and all data fields are set
    if (WinnerParty == null) {
        e.shapeStroke = "transparent";
        e.shapeFill   = "transparent";
        return;
    }

    // styling states that did not exists in the election year
    if (Statehood > ElectionYear) {
        //console.log("onHexShapeStyle " + Code + " NoStatehood");
        var style = PartyStyles.NoStatehood;
        e.shapeFill = style.fill;
        e.shapeStroke = style.outline;

    } else {
        var style = PartyStyles[WinnerParty];
        if (style) {
            //if (Code == "NJ" || Code == "FL") {
            //    console.log("onStyleShapeScript " + Name + " " + WinnerParty + " Statehood=" + (ElectionYear > Statehood));
            //}
            e.shapeFill = style.fill;
            e.shapeStroke = style.outline;
        } else {
            //if (Code == "NJ" || Code == "FL") {
            //    console.log("onStyleShapeScript " + Name + " " + WinnerParty + " else ");
            //}
            console.log("onHexShapeStyle " + Code + " Pink");
            e.shapeFill = "Pink";
            e.shapeStroke = "black";
        }
    }

    var ShapeSelected = e.item.getFieldValue("ShapeSelected");
    if (ShapeSelected == "true") {
        e.shapeOpacity = 0.75;
    } else {
        e.shapeOpacity = 1.0;
    }
}

igRegisterScript("onHexShapeStyle", onHexShapeStyle, false);

function onHexShapeMouseDown(o, e) {
    console.log("onHexShapeMouseDown " + " ");

    if (e.item && e.item.setFieldValue) {
        //console.log("onShapeMouseMove ShapeSelected");

        var WinnerParty = e.item.getFieldValue("WinnerParty");

        var winnerEditable = e.item.getFieldValue("WinnerEditable");
        if (winnerEditable == "true") {
            if (WinnerParty == "Tossup") {
                WinnerParty = "Democrat";
            } else if (WinnerParty == "Democrat") {
                WinnerParty = "Republican";
            } else {
                WinnerParty = "Tossup";
            }
            e.item.setFieldValue("WinnerParty", WinnerParty);
        }
        //e.chart.notifyClearItems(e.series.shapefileDataSource);
        //e.series.outline = 'black';
        //e.series.markerType = 'Circle';
        //e.series.renderSeries(false);
        e.series.styleUpdated();
    }
}

igRegisterScript("onHexShapeMouseDown", onHexShapeMouseDown, false);

function onHexMarkerStyle(o, e) {
    //console.log("onStyleHexMarker " + e);

    var desiredSize = 46;

    return {
        measure: function (measureInfo) {
            var code = "DC";
            var data = measureInfo.data;
            //if (data != null && data.item != null) {
            //    //code = data.item.StateSymbol.toString();
            //    code = data.item.getFieldValue("Code").toString(); 
            //}
            //var context = measureInfo.context;
            //var height = context.measureText("M").height;
            //var width = context.measureText(code).width;
            //measureInfo.width = width;
            //measureInfo.height = height;
            measureInfo.width  = desiredSize;
            measureInfo.height = desiredSize;
        },

        render: function (renderInfo) {
            var ctx = renderInfo.context;
            if (renderInfo.isHitTestRender) {
                ctx.fillStyle = renderInfo.data.actualItemBrush.fill;
            } else {
                ctx.fillStyle = "black";
            }
             
            var halfWidth = renderInfo.availableWidth / 2.0;
            var halfHeight = renderInfo.availableHeight / 2.0;

            var cx = renderInfo.xPosition;
            var cy = renderInfo.yPosition;

            var x = renderInfo.xPosition - halfWidth;
            var y = renderInfo.yPosition - (halfHeight);

            if (renderInfo.isHitTestRender) {
                ctx.fillRect(x, y, renderInfo.availableWidth, renderInfo.availableHeight);
            } else {
                //ctx.fillStyle = "red";
                //ctx.fillRect(cx, cy, renderInfo.availableWidth, renderInfo.availableHeight);
                //ctx.fillStyle = "blue";
                //ctx.fillRect(x, y, renderInfo.availableWidth, renderInfo.availableHeight);

                var viewportHeight = renderInfo.passInfo.viewportHeight;
                var viewportRatio = viewportHeight / 500.0;

                var fontSize = Math.round(viewportRatio * 10);
                var lineSize = Math.round(viewportRatio * 6);

                ctx.font = "normal " + fontSize + "px Verdana";
                ctx.textBaseline = "middle";
                ctx.textAlign = "center";

                var item = renderInfo.data.item;
                var code = item.getFieldValue("Code").toString();
                ctx.fillStyle = "white";
                ctx.fillText(code, cx, cy - lineSize);

                var ElectionYear = item.getFieldValue("ElectionYear");
                var ElectionMode = item.getFieldValue("ElectionMode");
                var Statehood = item.getFieldValue("Statehood");

                var winnerValue = 0;
                var looserValue = 0;
                //var winnerElectors = 20;
                //var winnerValue = item.getFieldValue("WinnerElectors");
                
               
                //var winnerElectors = item.getFieldValue("WinnerElectors");
                var heldElection = Statehood < ElectionYear

                if (heldElection) {
                    if (ElectionMode == "Popular") {
                     //winnerValue = abbreviate(item.getFieldValue("WinnerVotes"));
                        winnerValue = item.getFieldValue("WinnerPercentage") + "%";
                       looserValue = 0; //abbreviate(data.item.LooserVotes);
                    } else if (ElectionMode == "Percent") {
                        winnerValue = item.getFieldValue("WinnerPercentage").toFixed(0) + "%";
                        looserValue = 0; //data.item.LooserPercentage;
                    } else {
                        winnerValue = item.getFieldValue("WinnerElectors");
                        looserValue = 0; //data.item.LooserElectors;
                    }
                }  

                if (code == "TX") {
                    console.log("onHexMarkerStyle " + code + " " + ElectionMode + " " + winnerValue );
                }

                if (winnerValue > 0 || winnerValue != "0.0") {
                    ctx.fillText(winnerValue, cx, cy + lineSize); 
                }
                  
            }
        }
    };
}

igRegisterScript("onHexMarkerStyle", onHexMarkerStyle, true);
