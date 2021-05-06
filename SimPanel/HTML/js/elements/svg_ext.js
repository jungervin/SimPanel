
// SVG.extend(SVG.Line, {

//     clockface: function (x, y, r1, r2, deg) {
//         return this.plot(SVG.shapes.clockface(x, y, r1, r2, deg))
//         // var r = Math.PI / 180.0 * deg;
//         // var p1 = Math.cos(r) * r1;
//         // var p2 = Math.sin(r) * r1;
//         // var p3 = Math.cos(r) * r2;
//         // var p4 = Math.sin(r) * r2;
//         // return this.line(x + p1, y + p2, x + p3, y + p4);
//     },

// })

// SVG.shapes = {

//     clockface: function (x, y, r1, r2, deg) {
//         var points = [];
//         var r = Math.PI / 180.0 * deg;
//         var p1 = Math.cos(r) * r1;
//         var p2 = Math.sin(r) * r1;
//         var p3 = Math.cos(r) * r2;
//         var p4 = Math.sin(r) * r2;
//         points.push([x + p1, y + p2]);
//         points.push([x + p3, y + p4]);
//         return new SVG.PointArray(points);
//     },
//     // constructor(x, y, r1, r2, deg) {
//     //     super();

//     //     var r = Math.PI / 180.0 * deg;
//     //     var p1 = Math.cos(r) * r1;
//     //     var p2 = Math.sin(r) * r1;
//     //     var p3 = Math.cos(r) * r2;
//     //     var p4 = Math.sin(r) * r2;
//     //     this.line(x + p1, y + p2, x + p3, y + p4);

//     // }
// }


SVG.extend(SVG.Container, {

    clockface: function (x, y, r1, r2, deg) {

        let r = Math.PI / 180.0 * deg;
        let p1 = Math.cos(r) * r1;
        let p2 = Math.sin(r) * r1;
        let p3 = Math.cos(r) * r2;
        let p4 = Math.sin(r) * r2;
        return this.put(new SVG.Line(x + p1, y + p2, x + p3, y + p4))
    },

    boxed_text: function(w, h, text) {
        return this.put(new SVG.BoxedText(w, h, text))
    },

    button: function (w, h, title, options, callback) {
        return this.put(new SVG.Button(w, h, title, options, callback));
    },

    indicator_button: function (w, h, title, options, callback) {
        return this.put(new SVG.IndicatorButton(w, h, title, options, callback));
    },

    circle_button: function (owner, diam, text, callback) {
        return this.put(new SVG.CircleButton(owner, diam, text, callback));
    },

    rect_button: function (width, height, text, callback, enabled) {
        return this.put(new SVG.RectButton(width, height, text, callback, enabled));
    },

    image_button: function (owner, text, image_on, image_off, callback) {
        return this.put(new SVG.ImageButton(owner, text, image_on, image_off, callback));
    },

    icon_button: function (width, height, path) {
        return this.put(new SVG.IconButton(width, height, path));
    },


    rotary: function () {
        return this.put(new SVG.Rotary());
    },

    lcdDisplay: function (width, height, text, options) {
        return this.put(new SVG.LCDDisplay(width, height, text, options))
    },

    compass: function (diam, options) {
        return this.put(new SVG.Compass(diam, options));
    },

    compass_bug: function (diam, options) {
        return this.put(new SVG.CompassBug(diam, options));
    },

    compass_needle: function (diam, options) {
        return this.put(new SVG.CompassNeedle(diam, options));
    },

    compass_gauge: function (w, h, options) {
        return this.put(new SVG.CompassGauge(w, h, options));
    },

    altimeter: function (w, h, step, div) {
        return this.put(new Altimeter(w, h));
    },

    verticalspeed: function (w, h, step, div) {
        return this.put(new VerticalSpeed(w, h));
    },

    gsindicator: function (w, h, step, div) {
        return this.put(new GSIndicator(w, h));
    },

    altimeter_gauge: function (w, h, options) {
        return this.put(new SVG.AltimeterGauge(w, h, options));
    },

    speedindicator: function (w, h, step, div) {
        return this.put(new SpeedIndicator(w, h));
    },

    speedindicator_gauge: function (w, h, options) {
        return this.put(new SVG.SpeedIndicatorGauge(w, h, options));
    },

    attitude_indicator: function (w, h, cx, cy) {
        return this.put(new AttitudeIndicator(w, h, cx, cy));
    },

    rpm_gauge: function (w, h, r, opt) {
        return this.put(new SVG.RPMGauge(w, h, r, opt));
    },

    horizontal_gauge: function (w, h, opt) {
        return this.put(new SVG.HorizontalGauge(w, h, opt));
    },

    fuelquantity_gauge: function (w, h, opt) {
        return this.put(new SVG.FuelQuantityGauge(w, h, opt));
    },

    magneto: function (w, h, opt = null, callback) {
        return this.put(new SVG.Magneto(w, h, opt, callback));
    },

    trim_indicator:function (w, h, opt = null) {
        return this.put(new SVG.TrimIndicator(w, h, opt));
    },

    flaps_control:function (w, h, opt = null) {
        return this.put(new SVG.FlapsControl(w, h, opt));
    },

    wind_gauge:function (w, h, opt = null) {
        return this.put(new SVG.WindGauge(w, h, opt));
    },
});