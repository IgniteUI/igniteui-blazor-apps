var PartyStyles = {
    Unknown:    { outline: "white", fill: "#6F6E6E" },
    Democrat:           { outline: "white", fill: "#465F98" },
    Republican:         { outline: "white", fill: "#CD433C" },
    Democrat_Leaning:   { outline: "white", fill: "#7D9BE1" },
    Republican_Leaning: { outline: "white", fill: "#D6726D" },
    //Tossup:     { outline: "black", fill: "#6F6E6E" },
//    Republican:  { outline: "black", fill: "#FF0808" },
//    Democrat:    { outline: "black", fill: "#008DFF" },
    //Republican:     { outline: "black", fill: "#DE5E58" },
    //Democrat:       { outline: "black", fill: "#5885EC" },
    Other:       { outline: "white", fill: "#959494" },
    Tossup:      { outline: "white", fill: "#959494" },
    NoStatehood: { outline: "white", fill: "#959494" },
    
    //NationalRepublican:      { outline: "white", fill: "#CD433C" },
    //Democratic_Republican_1: { outline: "white", fill: "#7A3E98" },
    //Democratic_Republican_2: { outline: "white", fill: "#465F98" },
    //Democratic_Republican_3: { outline: "white", fill: "#CD433C" },
    //Democratic_Republican_4: { outline: "white", fill: "#FD9B50" },
    //Libertarian:          { outline: "white", fill: "#FD9B50" },
    //Green:                { outline: "white", fill: "#69AA67" },
    //Independent:          { outline: "white", fill: "#7A3E98" },
    //Reform:               { outline: "white", fill: "#B4B4B4" }, 
    //StatesRights:         { outline: "white", fill: "#FD9B50" }, // 1948
    //Progressive:          { outline: "white", fill: "#69AA67" }, // 1948
    //Socialist:            { outline: "white", fill: "#99222A" }, // 1932
    //Populist:             { outline: "white", fill: "#FD9B50" }, // 1892
    //Prohibition:          { outline: "white", fill: "#69AA67" }, // 1892
    //Greenback:            { outline: "white", fill: "#69AA67" }, // 1880
    //Southern_Democrat:    { outline: "white", fill: "#465F98" }, // 1860
    //Constitutional_Union: { outline: "white", fill: "#FD9B50" }, // 1860 
    //Whig:                 { outline: "white", fill: "#7A3E98" }, // 1852
    //FreeSoil:             { outline: "white", fill: "#FD9B50" }, // 1852
    //Liberty:              { outline: "white", fill: "#FD9B50" }, // 1844
    //Anti_Masonic:         { outline: "white", fill: "#FD9B50" }, // 1832
    //Federalist_1:         { outline: "white", fill: "#69AA67" }, // 1804
    //Federalist_2:         { outline: "white", fill: "#7A3E98" }, // 1804

    NationalRepublican:      { outline: "white", fill: "#959494" },
    Democratic_Republican_1: { outline: "white", fill: "#959494" },
    Democratic_Republican_2: { outline: "white", fill: "#959494" },
    Democratic_Republican_3: { outline: "white", fill: "#959494" },
    Democratic_Republican_4: { outline: "white", fill: "#959494" },
    Libertarian:          { outline: "white", fill: "#959494" },
    Green:                { outline: "white", fill: "#959494" },
    Independent:          { outline: "white", fill: "#959494" },
    Reform:               { outline: "white", fill: "#959494" }, 
    StatesRights:         { outline: "white", fill: "#959494" }, // 1948
    Progressive:          { outline: "white", fill: "#959494" }, // 1948
    Socialist:            { outline: "white", fill: "#959494" }, // 1932
    Populist:             { outline: "white", fill: "#959494" }, // 1892
    Prohibition:          { outline: "white", fill: "#959494" }, // 1892
    Greenback:            { outline: "white", fill: "#959494" }, // 1880
    Southern_Democrat:    { outline: "white", fill: "#465F98" }, // 1860
    Constitutional_Union: { outline: "white", fill: "#959494" }, // 1860 
    Whig:                 { outline: "white", fill: "#7A3E98" }, // 1852
    FreeSoil:             { outline: "white", fill: "#959494" }, // 1852
    Liberty:              { outline: "white", fill: "#959494" }, // 1844
    Anti_Masonic:         { outline: "white", fill: "#959494" }, // 1832
    Federalist_1:         { outline: "white", fill: "#69AA67" }, // 1804
    Federalist_2:         { outline: "white", fill: "#7A3E98" }, // 1804
}; 
//GetBounds = (element, parm) => { return element.getBoundingClientRect(); };

function GetColor(partyName) {
    var defaultColor = PartyStyles.NoStatehood.fill;

    if (partyName == undefined) return defaultColor;

    var style = PartyStyles[partyName];
    if (style == undefined) return defaultColor; 

    return style.fill; 
}

function GetBounds(element, parm) {
    return element.getBoundingClientRect(); 
}

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