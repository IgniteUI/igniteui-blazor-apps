var PartyStyles = {
    Unknown:    { outline: "black", fill: "#6F6E6E" },
    //Republican: { outline: "black", fill: "#CD433C" },
    //Democrat:   { outline: "black", fill: "#465F98" },
    //Tossup:     { outline: "black", fill: "#6F6E6E" },
//    Republican:  { outline: "black", fill: "#FF0808" },
//    Democrat:    { outline: "black", fill: "#008DFF" },
    Republican:     { outline: "black", fill: "#DE5E58" },
    Democrat:       { outline: "black", fill: "#5885EC" },
    Tossup:         { outline: "black", fill: "#959494" },
    NoStatehood:    { outline: "black", fill: "#CB52FA" },
}; 


function abbreviate(num) {
    if (num < 1e3) return n;
    if (num >= 1e3 && num < 1e5) return +(num / 1e3).toFixed(1) + "K";
    if (num >= 1e5 && num < 1e9) return +(num / 1e6).toFixed(1) + "M";
    if (num >= 1e9 && num < 1e12) return +(num / 1e9).toFixed(1) + "B";
    if (num >= 1e12) return +(num / 1e12).toFixed(1) + "T";

    //var str = num;
    //if (num >= 1000) {
    //    var suffixes = ["", "k", "m", "b", "t"];
    //    var suffixNum = Math.floor(("" + num).length / 3);
    //    var shortValue = '';
    //    for (var precision = 2; precision >= 1; precision--) {
    //        shortValue = parseFloat((suffixNum != 0 ? (num / Math.pow(1000, suffixNum)) : num).toPrecision(precision));
    //        var dotLessShortValue = (shortValue + '').replace(/[^a-zA-Z 0-9]+/g, '');
    //        if (dotLessShortValue.length <= 2) { break; }
    //    }
    //    if (shortValue % 1 != 0) shortValue = shortValue.toFixed(1);
    //    var suffix = suffixes[suffixNum];
    //    if (suffix == undefined)
    //        suffix = '';
    //    str = shortValue + suffix;
    //}
    //return str;
}