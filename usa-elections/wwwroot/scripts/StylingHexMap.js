//console.log("MapStylingShapes v1.0.1")

function onHexImportCompleted(o, e) {
    console.log("Map onHexImportCompleted " + e);
}

igRegisterScript("onHexImportCompleted", onHexImportCompleted, false);

function onHexShapeStyle(o, e) {

    // styling shapes based on data fields for all states in USA
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
            //console.log("ElectionHex ShapeStyle " + Code + " Pink");
            //e.shapeFill = "Pink";
            //e.shapeStroke = "black";
            var style = PartyStyles.NoStatehood;
            e.shapeFill = style.fill;
            e.shapeStroke = style.outline;

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

function onHexMarkerStyle(o, e) {
    //console.log("onStyleHexMarker " + e);

    var desiredSize = 46;

    return {
        measure: function (measureInfo) {
            //var stateCode = "DC";
            //var data = measureInfo.data;
            //if (data != null && data.item != null) {
            //    //stateCode = data.item.StateSymbol.toString();
            //    stateCode = data.item.getFieldValue("Code").toString(); 
            //}
            //var context = measureInfo.context;
            //var height = context.measureText("M").height;
            //var width = context.measureText(stateCode).width;
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
                
                var viewportHeight = renderInfo.passInfo.viewportHeight;
                var viewportRatio = viewportHeight / 500.0;

                var fontSize = Math.round(viewportRatio * 11);
                var lineSize = Math.round(viewportRatio * 7);

                ctx.font = "normal " + fontSize + "px Verdana";
                ctx.textBaseline = "middle";
                ctx.textAlign = "center";

                var item = renderInfo.data.item;
                var stateCode = item.getFieldValue("Code").toString();
                ctx.fillStyle = "white";
                ctx.fillText(stateCode, cx, cy - lineSize);

                var ElectionYear = item.getFieldValue("ElectionYear");
                var ElectionMode = item.getFieldValue("ElectionMode");
                var Statehood    = item.getFieldValue("Statehood");

                var winnerValue = 0;              
                var heldElection = Statehood < ElectionYear

                if (heldElection) {
                    if (ElectionMode == "Popular") {
                     //winnerValue = abbreviate(item.getFieldValue("WinnerVotes"));
                        winnerValue = item.getFieldValue("WinnerPercentage");
                        winnerValue = (Math.round(winnerValue)) + "%";  //* 10) / 10
                    } else if (ElectionMode == "Percent") {
                        winnerValue = item.getFieldValue("WinnerPercentage");
                        winnerValue = (Math.round(winnerValue)) + "%";
                    } else {
                        winnerValue = item.getFieldValue("WinnerElectors");
                    }
                }

                //if (stateCode == "TX") {
                //    console.log("ElectionHex Marker " + stateCode + " " + ElectionMode + " " + winnerValue );
                //}

                if (winnerValue > 0 || winnerValue != "0.0") {
                    ctx.fillText(winnerValue, cx, cy + lineSize); 
                }
                  
            }
        }
    };
}

igRegisterScript("onHexMarkerStyle", onHexMarkerStyle, true);

function onMapMouseEnter(o, e) {
    //console.log("Map onMapMouseEnter ");

    if (e.series.tooltipTemplate == null ||
        e.series.tooltipTemplate == undefined) {
        e.series.tooltipTemplate = createTooltip;
        console.log("Map onMapMouseEnter createTooltip ");
    }
}

igRegisterScript("onMapMouseEnter", onMapMouseEnter, false);

function createTooltip(context) {
         
    if (!context) return null;

    var item = context.item; 
    if (!item) return null;

    if (!item.getFieldValue) return null;

    var ElectionYear = item.getFieldValue("ElectionYear");
    //var ElectionMode = item.getFieldValue("ElectionMode");
    var Statehood = item.getFieldValue("Statehood");
    var heldElection = Statehood < ElectionYear

    //console.log("Map createTooltip '" + ElectionMode + "'");

    var winnerValue = 0;
    var votes = "";
    var percent = "";

    var stateCode = item.getFieldValue("Code").toString();
    var stateName = item.getFieldValue("Name").toString();
    var stateElement = document.createElement("div");
    stateElement.innerHTML = stateName + " (" + stateCode + ")";

    var tooltip = document.createElement("div");
    tooltip.className = "ui-tooltip-content";
    tooltip.appendChild(stateElement);

    var winnerParty = item.getFieldValue("WinnerParty").toString();
    if (winnerParty == "Tossup") {
        winnerValue = Math.round(item.getFieldValue("WinnerElectors")) + " Electors";
        var winnerLine = document.createElement("div");
        winnerLine.innerHTML = "Tossup: " + winnerValue;
        //winnerLine.style.color = "black";

        tooltip.appendChild(winnerLine);
    }
    else if (heldElection) {
        //if (ElectionMode == "Popular") {
        var winnerElectors = Math.round(item.getFieldValue("WinnerElectors")) + " Electors";
        var looserElectors = Math.round(item.getFieldValue("LooserElectors")) + " Electors";  

        votes   = abbreviate(item.getFieldValue("WinnerVotes"));
        percent = Math.round(item.getFieldValue("WinnerPercentage")) + "%";
        winnerValue = votes + " (" + percent + ") "; // + winnerElectors;
        votes   = abbreviate(item.getFieldValue("LooserVotes"));
        percent = Math.round(item.getFieldValue("LooserPercentage")) + "%";
        looserValue = votes + " (" + percent + ") "; // + looserElectors;

        //} else { // if (ElectionMode == "Percent") {
        //    winnerValue = Math.round(item.getFieldValue("WinnerElectors")) + " Electors";
        //    looserValue = Math.round(item.getFieldValue("LooserElectors")) + " Electors";  
        //}

        //var winnerName = item.getFieldValue("WinnerName").toString();
        var winnerColor = GetColor(winnerParty)
       
        var winnerLineInfo = document.createElement("div");
        winnerLineInfo.innerHTML = winnerParty + ":";
        winnerLineInfo.style.width = "4rem";
        var winnerLineV = document.createElement("div");
        winnerLineV.innerHTML = winnerValue;
        var winnerLineE = document.createElement("div");
        winnerLineE.style.textAlign = "right";
        winnerLineE.style.width = "4rem";
        winnerLineE.innerHTML = winnerElectors; 

        var winnerLine = document.createElement("div");
        winnerLine.style.color = winnerColor;
        winnerLine.className = "igTooltipRow";
        winnerLine.appendChild(winnerLineInfo);
        winnerLine.appendChild(winnerLineV);
        winnerLine.appendChild(winnerLineE);
            
        //var looserName  = item.getFieldValue("LooserName").toString();
        var looserParty = item.getFieldValue("LooserParty").toString();
        var looserColor = GetColor(looserParty)

        var looserLineInfo = document.createElement("div");
        looserLineInfo.innerHTML = looserParty + ":";
        looserLineInfo.style.width = "4rem";
        var looserLineV = document.createElement("div");
        looserLineV.innerHTML = looserValue;
        var looserLineE = document.createElement("div");
        looserLineE.style.textAlign = "right";
        looserLineE.style.width = "4rem";
        looserLineE.innerHTML = looserElectors; 

        var looserLine = document.createElement("div");
        looserLine.style.color = looserColor;
        looserLine.className = "igTooltipRow";
        looserLine.appendChild(looserLineInfo);
        looserLine.appendChild(looserLineV);
        looserLine.appendChild(looserLineE);

        if (winnerParty == "Democrat") {
            tooltip.appendChild(winnerLine);
            tooltip.appendChild(looserLine);
        } else {
            tooltip.appendChild(looserLine);
            tooltip.appendChild(winnerLine);
        }
    }
     
    return tooltip;
}

//function onHexShapeMouseDown(o, e) {
//    console.log("onHexShapeMouseDown " + " ");

//    if (e.item && e.item.setFieldValue) {
//        //console.log("onShapeMouseMove ShapeSelected");

//        var WinnerParty = e.item.getFieldValue("WinnerParty");

//        var winnerEditable = e.item.getFieldValue("WinnerEditable");
//        if (winnerEditable == "true") {
//            if (WinnerParty == "Tossup") {
//                WinnerParty = "Democrat";
//            } else if (WinnerParty == "Democrat") {
//                WinnerParty = "Republican";
//            } else {
//                WinnerParty = "Tossup";
//            }
//            e.item.setFieldValue("WinnerParty", WinnerParty);
//        }
//        //e.chart.notifyClearItems(e.series.shapefileDataSource);
//        //e.series.outline = 'black';
//        //e.series.markerType = 'Circle';
//        //e.series.renderSeries(false);
//        e.series.styleUpdated();
//    }
//}

//igRegisterScript("onHexShapeMouseDown", onHexShapeMouseDown, false);
