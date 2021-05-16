var codes = new Array(36);
codes["a"] = [".-", "Alfa"];
codes["b"] = ["-...", "Bravo"];
codes["c"] = ["-.-.", "Charlie"];
codes["d"] = ["-..", "Delta"];
codes["e"] = [".", "Echo"];
codes["f"] = ["..-.", "Foxtrot"];
codes["g"] = ["--.", "Golf"];
codes["h"] = ["....", "Hotel"];
codes["i"] = ["..", "India"];
codes["j"] = [".---", "Juliet"];
codes["k"] = ["-.-", "Kilo"];
codes["l"] = [".-..", "Lima"];
codes["m"] = ["--", "Mike"];
codes["n"] = ["-.", "November"];
codes["o"] = ["---", "Oscar"];
codes["p"] = [".--.", "Papa"];
codes["q"] = ["--.-", "Quebec"];
codes["r"] = [".-.", "Romeo"];
codes["s"] = ["...", "Sierra"];
codes["t"] = ["-", "Tango"];
codes["u"] = ["..-", "Uniform"];
codes["v"] = ["...-", "Victor"]
codes["w"] = [".--", "Whisky"];
codes["x"] = ["-..-", "X-ray"];
codes["y"] = ["-.--", "Yankee"];
codes["z"] = ["--..", "Zulu"];
codes["0"] = ["-----", "Zero"];
codes["1"] = [".----", "One"];
codes["2"] = ["..---", "Two"];
codes["3"] = ["...--", "Three"];
codes["4"] = ["....-", "Four"];
codes["5"] = [".....", "Five"];
codes["6"] = ["-....", "Six"];
codes["7"] = ["--...", "Seven"];
codes["8"] = ["---..", "Eight"];
codes["9"] = ["----.", "Nine"];


function encode(text) {
    text = text.toLowerCase();
    var chars = text.split("");
    var morse = '';
    var phonetic = "";
    for (a = 0; a < chars.length; a++) {
        if (chars[a] != " ") {
            if (codes[chars[a]]) {
                //document.morsecode.codebox.value += charCodes[chars[a]] + "    ";
                morse += codes[chars[a]][0] + " ";
                phonetic += codes[chars[a]][1] + " ";
                //temp += chars[a] + "=" + codes[chars[a]] + "\n";
            }
            else {
                morse += "(None) ";
                phonetic += "(None) ";
            }
        }
        else {
            morse += " ";
            phonetic += " ";
        }
    }
    return { morse: morse, phonetic: phonetic };
}

function m2feet(m) {
    return m * 3.2808399
}
// 1 meter = 0.000539956803 nautical miles
function m2nm(meter) {
    return meter * 0.000539956803;
}

// 1 nautical miles = 1852 meters
function nm2m(nm) {
    return nm * 1852;
}

function padLeft(text, len, char = "\u00A0") {
    while (String(text).length < len) {
        text = char + text;
    }
    return text;
}

function padRight(text, len, char = "\u00A0") {
    while (String(text).length < len) {
        text += char;
    }
    return text;
}


// function getDistance(origin, destination) {
//     // return distance in meters
//     var lon1 = toRadian(origin[1]),
//         lat1 = toRadian(origin[0]),
//         lon2 = toRadian(destination[1]),
//         lat2 = toRadian(destination[0]);

//     var deltaLat = lat2 - lat1;
//     var deltaLon = lon2 - lon1;

//     var a = Math.pow(Math.sin(deltaLat / 2), 2) + Math.cos(lat1) * Math.cos(lat2) * Math.pow(Math.sin(deltaLon / 2), 2);
//     var c = 2 * Math.asin(Math.sqrt(a));
//     var EARTH_RADIUS = 6371;
//     return c * EARTH_RADIUS * 1000;
// }

function toRadian(degree) {
    return degree * Math.PI / 180;
}

function test() {
    alert("OK");
}

var getJSON = function (url, callback) {
    var xhr = new XMLHttpRequest();
    xhr.open('GET', url, true);
    xhr.responseType = 'json';
    xhr.onload = function () {
        var status = xhr.status;
        if (status === 200) {
            callback(null, xhr.response);
        } else {
            callback(status, xhr.response);
        }
    };
    xhr.send();
};

function HgToMilibar(hg) {
    return hg * 33.863886666667;
}
function GetDiffAndStore(prev_v, current_v, diff) {
    if (Math.abs(prev_v - current_v) > diff) {
        prev_v = current_v;
        return true;
    }
    return false;
}

// function getDistanceFromLatLonInKm(lat1, lon1, lat2, lon2) {
//     var R = 6371; // Radius of the earth in km
//     var dLat = deg2rad(lat2 - lat1);  // deg2rad below
//     var dLon = deg2rad(lon2 - lon1);
//     var a =
//         Math.sin(dLat / 2) * Math.sin(dLat / 2) +
//         Math.cos(deg2rad(lat1)) * Math.cos(deg2rad(lat2)) *
//         Math.sin(dLon / 2) * Math.sin(dLon / 2)
//         ;
//     var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
//     var d = R * c; // Distance in km
//     return d;
// }


function angleFromCoordinate(lat1, lon1, lat2, lon2) {
    var p1 = {
        x: lat1,
        y: lon1
    };

    var p2 = {
        x: lat2,
        y: lon2
    };
    // angle in radians
    var angleRadians = Math.atan2(p2.y - p1.y, p2.x - p1.x);
    // angle in degrees
    var angleDeg = Math.atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Math.PI;


    return angleDeg;

}
function deg2rad(deg) {
    return deg * (Math.PI / 180)
}

function todaySecs() {
    let dt = new Date()
    return dt.getHours() * 3600 + dt.getMinutes() * 60 + dt.getSeconds()
}

function secsToTime(secs) {
    secs = Math.round(secs);
    var hours = Math.floor(secs / (60 * 60));

    var divisor_for_minutes = secs % (60 * 60);
    var minutes = Math.floor(divisor_for_minutes / 60);

    var divisor_for_seconds = divisor_for_minutes % 60;
    var seconds = Math.ceil(divisor_for_seconds);

    var obj = {
        "h": hours,
        "m": minutes,
        "s": seconds
    };
    return obj;
}

function setCharAt(str, index, chr) {
    if (index > str.length - 1) return str;
    return str.substring(0, index) + chr + str.substring(index + 1);
}

function decimalToBCD(str) {
    return parseInt("0x" + str.replace(".", ""));
}

function setCookie(cname, cvalue, exdays = 100) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function getZIndex() {
    var divs = document.getElementsByTagName('div');
    var highest = 1000;
    for (var i = 0; i < divs.length; i++) {
        var zindex = divs[i].style.zIndex;
        if (zindex > highest) {
            highest = zindex;
        }
    }
    return highest
}