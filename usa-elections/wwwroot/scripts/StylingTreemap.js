//console.log("StylingTreemap v1.0.1")

var styles = {
    Republican: { outline: "black", fill: "#FF0808" },
    Democrat: { outline: "black", fill: "#008DFF" },
    NoStatehood: { outline: "black", fill: "lightgray" },
    //NoStatehood: { outline: "gray", fill: "white" },
}; 

function onStyleTreeNode(o, e) {
    //console.log("StylingTreemap onStyleTreeNode ");

    if (e.item == null) {
        e.style.fill = "#414141";

    } else {
        var style = styles[e.item.WinnerParty];
        if (style) {
            e.style.fill = style.fill;
        } else {
            e.style.fill = "#414141";
        }
    }
    

    //if (e.parentLabel != null) {
    //    var style = styles[e.item.Party];
    //    if (style) {
    //        e.style.fill = style.fill;
    //    } else {
    //        e.style.fill = "#00BEFF";
    //    }
    //    //e.style.backgrund = "#00BEFF";

    //    //e.style.color = "#00BEFF";
    //} else {
    //    //e.style.backgrund = "#DEA51C";
    //    e.style.fill = "#DEA51C";
    //    //e.style.color = "#DEA51C";
    //}

};
igRegisterScript("onStyleTreeNode", onStyleTreeNode, false);
 