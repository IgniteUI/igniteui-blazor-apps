//console.log("MapStylingShapes v1.0.1")
 
//var styles = {
//    Republican:  { outline: "black", fill: "#FF0808" },
//    Democrat:    { outline: "black", fill: "#008DFF" },
//    NoStatehood: { outline: "black", fill: "lightgray" },
//    //NoStatehood: { outline: "gray", fill: "white" },
//}; 
 
function onShapeStyle(o, e) {
    
    // styling shapes based on data fields for all states in USA
    //var Name = e.item.getFieldValue("Name");
    var Code = e.item.getFieldValue("Code");
    var WinnerParty  = e.item.getFieldValue("WinnerParty");
    var ElectionYear = e.item.getFieldValue("ElectionYear");
    var StateCreation = e.item.getFieldValue("Statehood"); 

    //if (Code == "CA" || Code == "TX") {
    //    console.log("Map onShapeStyle " + Code + " " + ElectionYear + " " + StateCreation);
    //}

    // hiding shapes until map is zoomed in and all data fields are set
    var WinnerParty = e.item.getFieldValue("WinnerParty");
    if (WinnerParty == null) {
        e.shapeStroke = "transparent";
        e.shapeFill   = "transparent";
        return;
    } 

    // styling states that did not exists in the election year
    if (StateCreation > ElectionYear) {
        var style = PartyStyles.NoStatehood;
        e.shapeFill   = style.fill;
        e.shapeStroke = style.outline;

    } else {
        var style = PartyStyles[WinnerParty];
        if (style) {
            //if (Code == "NJ" || Code == "FL") {
            //    console.log("onShapeStyleScript " + Name + " " + WinnerParty + " Statehood=" + (ElectionYear > Statehood));
            //}
            e.shapeFill = style.fill;
            e.shapeStroke = style.outline;
        } else {
            //if (Code == "NJ" || Code == "FL") {
            //    console.log("onShapeStyleScript " + Name + " " + WinnerParty + " else ");
            //}
            e.shapeFill = "gray";
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

igRegisterScript("onShapeStyle", onShapeStyle, false);

var isHighlightingShape = false;
var isEnteringShape = false;
function onShapeMouseEnter(o, e) {
    console.log("onShapeMouseEnter " + isEnteringShape + " " + isHighlightingShape);
    if (isHighlightingShape == true) return;
    //var dataSource = e.series.dataSource; 
    //for (var i = 0; i < e.series.dataSource.length; i++) {
    //    e.series.dataSource[i].setFieldValue("ShapeSelected", "false");
    //}
    isEnteringShape = true;
    //e.item.setFieldValue("ShapeSelected", "true");
    //e.series.outline = 'Black';
}
igRegisterScript("onShapeMouseEnter", onShapeMouseEnter, false);

function onShapeMouseLeave(o, e) {
    console.log("onShapeMouseLeave " + isEnteringShape + " " + isHighlightingShape);
    if (isHighlightingShape == true) return; 

    isEnteringShape = false;
    //window.isMouseInShape = false;
    ////e.shapeOpacity = 1.0;
    ////e.item.fieldValues.ShapeSelected = "false";
    //e.item.setFieldValue("ShapeSelected", "false");
    //e.series.outline = 'Red';
    //deselectShape(e.series);
}
igRegisterScript("onShapeMouseLeave", onShapeMouseLeave, false);

function onShapeMouseMove(o, e) {
    console.log("onShapeMouseMove " + isEnteringShape + " " + isHighlightingShape);
    if (isHighlightingShape == true) return;
    //var dataSource = e.series.dataSource; 

    isHighlightingShape = true;
    deselectShape(e.series);

    if (e.item && e.item.setFieldValue) {
        //console.log("onShapeMouseMove ShapeSelected");
        e.item.setFieldValue("ShapeSelected", "true");
        e.series.outline = 'Black';
    }
    isHighlightingShape = false;
}
igRegisterScript("onShapeMouseMove", onShapeMouseMove, false);


function deselectShape(series) {

    if (series && series.dataSource) {
        for (var i = 0; i < series.dataSource.length; i++) {
            var item = series.dataSource[i];
            if (item && item.setFieldValue) {
                //console.log("onShapeMouseMove " + item);
                series.dataSource[i].setFieldValue("ShapeSelected", "false");
            }
        }
        if (isHighlightingShape == true) return;
        series.outline = 'gray';
    }
}