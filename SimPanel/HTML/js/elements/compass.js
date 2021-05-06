SVG.Compass = class extends SVG.G {
    constructor(d, options) {
        super();
        var opt = options || {};
        this.bg = opt.bg || "#00000040";
        this.fg = opt.fg || "white";
        this.font1 = opt.font || { fill: "white", 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 28, leading: '0em' }
        this.font2 = opt.font || { fill: "white", 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 20, leading: '0em' }
        this.anim = opt.animation || { duration: 250, delay: 0, when: 'now', swing: true, times: 1, wait: 0 }

        this.circle(d).fill(this.bg);
        var r0 = d / 2;
        var r1 = r0 - 10;
        var r2 = r0 - 20;
        var div1 = 5;
        var div2 = 30;
        for (var i = 0; i < 360; i = i + div1) {
            if ((i / div2) != Math.round(i / div2)) {
                var r = Math.PI / 180.0 * i;
                var p1 = Math.cos(r) * r0;
                var p2 = Math.sin(r) * r0;
                var p3 = Math.cos(r) * r1;
                var p4 = Math.sin(r) * r1;
                this.line(r0 + p1, r0 + p2, r0 + p3, r0 + p4).stroke({ width: 3, color: "white" });
            }
        }

        var items = ["N", "3", "6", "E", "12", "15", "S", "21", "24", "W", "30", "33"]
        var step = 360 / items.length;
        for (var i = 0; i < items.length; i++) {
            var r = Math.PI / 180.0 * i * step;
            var p1 = Math.cos(r) * r0;
            var p2 = Math.sin(r) * r0;
            var p3 = Math.cos(r) * r2;
            var p4 = Math.sin(r) * r2;
            this.line(r0 + p1, r0 + p2, r0 + p3, r0 + p4).stroke({ width: 3, color: "white" });

            var t = i % 3 == 0 ? this.text(items[i]).font(this.font1) : this.text(items[i]).font(this.font2)
            t.center(r0, 33).rotate(i * step, r0, r0);
        }

        this.currentAngle = 0;
    }

    rotate(deg) {
        if (this.currentAngle != deg) {
            super.rotate(deg - this.currentAngle);
            this.currentAngle = deg
        }
    }

}

SVG.CompassBug = class extends SVG.G {
    constructor(d, options) {
        super();
        var opt = options || {};
        this.bg = opt.bg || "black";
        this.fg = opt.fg || "white";
        this.font1 = opt.font || { fill: "white", 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 28, leading: '0em' }
        this.font2 = opt.font || { fill: "white", 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 20, leading: '0em' }
        this.anim = opt.animation || { duration: 250, delay: 0, when: 'now', swing: true, times: 1, wait: 0 }

        //this.circle(d).fill("#ffffff80");
        this.circle(d).fill("transparent");
        var r0 = d / 2;
        var r1 = r0 - 10;
        var r2 = r0 - 20;

        this.heading_bug = this.polygon('-12,-10 -6,-10 0,-2 6,-10 12,-10 12,0 -12,0 -12,-10').fill('cyan')
            .stroke({ width: 2, color: 'cyan' }).center(r0, 5);
        this.currentAngle = 0;
    }

    rotate(deg) {
        if (this.currentAngle != deg) {
            super.rotate(deg - this.currentAngle);
            this.currentAngle = deg
        }
    }

    // rotate2(deg) {
    //     var a = deg < this.currentAngle ? deg + 360 : deg;
    //     var p1 = (a - this.currentAngle) % 360;
    //     var p2 = (-360 + a - this.currentAngle) % 360; 

    //     var a = Math.abs(p2) < 180 ? p2 : p1 

    //     if(a != 0) {
    //         super.animate(this.anim).rotate(a);
    //         this.currentAngle = deg;
    //     }
    // }


}

SVG.CompassNeedle = class extends SVG.G {
    constructor(diam, options) {
        super();
        var opt = options || {};
        this.anim = opt.animation || { duration: 250, delay: 0, when: 'now', swing: true, times: 1, wait: 0 }

        var d = diam;
        var c = this.circle(d).fill("#00000010");
        var m = 10;
        var l = d * 0.33;
        var a = [
            [0, m],
            [15, m + 30],
            [3, m + 30],
            [3, l],
            [-3, l],
            [-3, m + 30],
            [-15, m + 30],
        ]
        this.p1 = this.polygon(a);
        this.p1.fill("black").stroke({ width: 2, color: "white" }).center(c.bbox().cx);

        var l = d * 0.33;
        var a = [
            [-3, d - m],
            [3, d - m],
            [3, d - l],
            [-3, d - l],
        ]
        this.p2 = this.polygon(a);
        this.p2.fill("black").stroke({ width: 2, color: "white" }).center(c.bbox().cx);

        var l = d * 0.33 / 2;
        var a = [
            [-3, d + l],
            [3, d + l],
            [3, d - l],
            [-3, d - l],
        ]
        this.p3 = this.polygon(a)
        this.p3.fill("black").stroke({ width: 2, color: "white" }).center(c.bbox().cx, c.bbox().cy);


        var a = [
            [0, l + 28],
            [10, l + 44],
            [-10, l + 44],
            // [5, l],
            // [-5, l],
            // [-5, m + 30],
            // [-15, m + 30],
        ]
        this.t1 = this.polygon(a);
        this.t1.fill("black").stroke({ width: 2, color: "white" }).center(c.bbox().cx);
        var step = 30;
        this.max_dist = 2 * step;
        this.circle(10).fill('#00000040').stroke({ width: 1, color: 'white' }).move(c.bbox().cx - step - 3, c.bbox().cy - 3);
        this.circle(10).fill('#00000040').stroke({ width: 1, color: 'white' }).move(c.bbox().cx - step * 2 - 3, c.bbox().cy - 3);
        this.circle(10).fill('#00000040').stroke({ width: 1, color: 'white' }).move(c.bbox().cx + step - 3, c.bbox().cy - 3);
        this.circle(10).fill('#00000040').stroke({ width: 1, color: 'white' }).move(c.bbox().cx + step * 2 - 3, c.bbox().cy - 3);

        this.currentAngle = 0;
        this.currentDist = 0;

    }

    fill(color) {
        this.p1.fill(color);
        this.p2.fill(color);
        this.p3.fill(color);
        this.t1.fill(color);
        return this;
    }
    stroke(str) {
        this.p1.stroke(str);
        this.p2.stroke(str);
        this.p3.stroke(str);
        this.t1.stroke(str);
        return this;

    }

    // Percent!!
    dist(d) {
        var d = Math.min(1, Math.max(-1, (this.currentDist + d - this.currentDist)));
        //console.log(d);
        if (d != this.currentDist) {
            var dx = d - this.currentDist;
            this.currentDist = d;
            this.p3.animate(this.anim).translate(dx * this.max_dist);
        }
        return this;
    }

    rotate(deg) {
        if (this.currentAngle != deg) {
            super.rotate(deg - this.currentAngle);
            this.currentAngle = deg
        }
    }

    // rotate2(deg) {
    //     var a = deg < this.currentAngle ? deg + 360 : deg;
    //     var p1 = (a - this.currentAngle) % 360;
    //     var p2 = (-360 + a - this.currentAngle) % 360; 

    //     var a = Math.abs(p2) < 180 ? p2 : p1 

    //     if(a != 0) {
    //         super.animate(this.anim).rotate(a);
    //         this.currentAngle = deg;
    //     }   
    // }



    setHasNav(b) {
        if (b) {
            this.p3.show();
        } else {
            this.p3.hide();
        }
    }
    fromto(b) {
        if (b == 0) {
            this.t1.hide();
        } else {
            this.t1.show();
        }

        this.t1.rotate(this.prevFromToAngle || 0, this.bbox().cx, this.bbox().cy);
        var a = 0;
        switch (b) {
            case 1: a = 0;
                break;
            case 2: a = 180;
                break;
        }
        this.t1.rotate(a, this.bbox().cx, this.bbox().cy);
        this.prevFromToAngle = a;
        this.FromTo = b;
    }

}

SVG.CompassGauge = class extends SVG.G {
    constructor(w, h) {
        super();

        this.Rose = this.compass(w, h).center(w / 2, h / 2)
        this.Bug = this.compass_bug(w, h).center(w / 2, h / 2)
        this.Needle = this.compass_needle(w, h).center(w / 2, h / 2)
        this.Needle.fill("magenta").stroke({ width: 1, color: "black" })

        let cx = this.Rose.cx()
        let cy = this.Rose.cy()
        // AIR PLANE SYMBOL
        this.path("M44 50 L49 50 L49 53 L48 54 L48 55 L52 55 L52 54 L51 53 L51 50 L56 50 L56 49 L51 48 L51 46 Q50 44 49 46 L49 48 L44 49 Z")
            .center(cx, cy).scale(3).fill("white");

        let r0 = w / 2 + 5
        let r1 = r0 + 24
        for (var i = 0; i < 360; i += 30) {
            var r = Math.PI / 180.0 * i;
            var p1 = Math.cos(r) * r0;
            var p2 = Math.sin(r) * r0;
            var p3 = Math.cos(r) * r1;
            var p4 = Math.sin(r) * r1;
            this.line(cx + p1, cy + p2, cx + p3, cy + p4).stroke({ width: 3, color: "white" });

        }

        let font1 = { 'fill': 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 24, leading: '0em' }
        let font2 = { 'fill': 'cyan', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 24, leading: '0em' }
        let font3 = { 'fill': 'magenta', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 24, leading: '0em' }
        let font4 = { 'fill': 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 38, leading: '0em' }


        var r = w / 2 + 20;
        let p = polarToCartesian(cx, cy, r, 0)
        this.DIR = this.boxed_text(100, 38, "000°").font(font4).center(p.x, p.y)

        p = polarToCartesian(cx, cy, r, -35)
        this.HDGBox = this.rect(110, 32).center(p.x - 70, p.y)
        this.text("HDG").font(font1).move(this.HDGBox.bbox().x + 5, p.y - 14)
        this.HDGText = this.text("000°").font(font2)
        this.HDGText.move(this.HDGBox.bbox().x2 - this.HDGText.bbox().w - 5, p.y - 14)

        p = polarToCartesian(cx, cy, r, 35)
        this.CRSBox = this.rect(110, 32).center(p.x + 70, p.y)
        this.text("CRS").font(font1).move(this.CRSBox.bbox().x + 5, p.y - 14)
        this.CRSText = this.text("000°").font(font3)
        this.CRSText.move(this.CRSBox.bbox().x2 - this.CRSText.bbox().w - 5, p.y - 14)

        let font5 = { 'fill': 'magenta', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 18, leading: '0em' }

        r0 = w / 2 - 70;
        p = polarToCartesian(cx, cy, r0, -60)
        this.NAV = this.boxed_text(50, 22, "LOC1").font(font5).center(p.x, p.y).fg("green")

        p = polarToCartesian(cx, cy, r0, 60)
        this.ENR = this.boxed_text(50, 22, "ENRR").font(font5).center(p.x, p.y)

        p = polarToCartesian(cx, cy, r0 - 10, 180)
        this.XTK = this.boxed_text(120, 22, "XTK 99.99NM").font(font5).center(p.x, p.y)

        this.setColors(3)
    }

    update2(data) {
        this.Rose.rotate(-data.PLANE_HEADING_DEGREES_GYRO);
        this.Bug.rotate(-data.PLANE_HEADING_DEGREES_GYRO + data.AUTOPILOT_HEADING_LOCK_DIR)
        this.Needle.rotate(-data.PLANE_HEADING_DEGREES_GYRO + data.GPS_WP_DESIRED_TRACK)
    }

    update(data) {

        // PRESS D on the keyboard to syncronize the correct value
        data.WISKEY_COMPASS_INDICATION_DEGREES = data.PLANE_HEADING_DEGREES_GYRO;

        this.data = data;
        this.NAVMode = data.GPS_DRIVES_NAV1 == 1 ? 3 : data.AUTOPILOT_NAV_SELECTED;
        this.setColors(this.NAVMode);



        this.Rose.rotate(-data.PLANE_HEADING_DEGREES_GYRO);
        this.Bug.rotate(-data.PLANE_HEADING_DEGREES_GYRO + data.AUTOPILOT_HEADING_LOCK_DIR)
        this.Needle.rotate(-data.PLANE_HEADING_DEGREES_GYRO + data.GPS_WP_DESIRED_TRACK)

        this.setDIR(data.PLANE_HEADING_DEGREES_GYRO);
        this.setHDG(data.AUTOPILOT_HEADING_LOCK_DIR);
       
        if (this.L_GPS_CURRENT_PHASE != data.L_GPS_CURRENT_PHASE) {
            switch (data.L_GPS_CURRENT_PHASE) {
                case 1: this.ENR.text("DPRT")
                    break;
                case 2: this.ENR.text("TERM")
                    break;
                case 4: this.ENR.text("OCN")
                    break;
                default: this.ENR.text("ENR")
                    break;
            }
            this.L_GPS_CURRENT_PHASE = data.L_GPS_CURRENT_PHASE;
        }

        if (this.NAVMode == 3) {
            var a = data.PLANE_HEADING_DEGREES_GYRO - data.GPS_WP_DESIRED_TRACK;
            this.Needle.rotate(-a);
            var nm = m2nm(data.GPS_WP_CROSS_TRK);
            var mmax = m2nm(4000); //2000 * 0.000539956803;
            this.Needle.dist(nm / mmax);

            if (Math.abs(nm) < 2) {
                this.XTK.hide();
            }
            else {
                this.XTK.text("XTK " + (nm).toFixed(2).toString() + "NM");
                this.XTK.show();
            }

            this.Needle.fromto(1);
            this.Needle.setHasNav(data.GPS_WP_NEXT_ID != "");
            this.setCRS(data.GPS_WP_DESIRED_TRACK);
            this.XTK.text("XTK " + nm.toFixed(2) + 'NM');
            this.NAV.text("GPS");
        }

        else if (this.NAVMode == 1) {
            var b = Number(data.NAV_CODES_1) & 0x80;
            if (b == 0x80) {

                var a = data.PLANE_HEADING_DEGREES_GYRO - data.NAV_LOCALIZER_1; // - data.GPS_MAGVAR;
                this.Needle.setHasNav(data.NAV_HAS_NAV_1);
                this.Needle.fromto(data.NAV_HAS_NAV_1 ? data.NAV_TOFROM_1 : 0);
                this.Needle.rotate(-a);
                this.Needle.dist(data.HSI_CDI_NEEDLE / 127);
                this.NAV.text("LOC1");
                //this.setHDG(data.AUTOPILOT_HEADING_LOCK_DIR);
                this.setCRS(data.NAV_LOCALIZER_1);
                this.XTK.text("CDI " + (data.HSI_CDI_NEEDLE).toFixed(2).toString() + '°');

            } else {
                var a = data.PLANE_HEADING_DEGREES_GYRO - data.NAV_OBS_1;
                this.Needle.dist(data.NAV_CDI_1 / 127);
                this.Needle.setHasNav(data.NAV_HAS_NAV_1);
                this.Needle.fromto(data.NAV_HAS_NAV_1 ? data.NAV_TOFROM_1 : 0);
                this.setCRS(data.NAV_OBS_1);
                this.XTK.text("CDI " + (data.NAV_CDI_1).toFixed(2).toString() + '°');
                //this.setHDG(data.AUTOPILOT_HEADING_LOCK_DIR);
                this.setCRS(data.NAV_LOCALIZER_1);

                this.Needle.rotate(-a);
                this.Needle.dist(data.NAV_CDI_1 / 127);
                this.NAV.text("VOR1");
            }


        }
        else if (this.NAVMode == 2) {
            var b = Number(data.NAV_CODES_2) & 0x80;
            if (b == 0x80) {

                var a = data.PLANE_HEADING_DEGREES_GYRO - data.NAV_LOCALIZER_1; // - data.GPS_MAGVAR;
                this.Needle.setHasNav(data.NAV_HAS_NAV_2);
                this.Needle.fromto(data.NAV_HAS_NAV_2 ? data.NAV_TOFROM_2 : 0);
                this.Needle.rotate(-a);
                this.Needle.dist(data.HSI_CDI_NEEDLE / 127);
                this.NAV.text("LOC2");
                this.setHDG(data.AUTOPILOT_HEADING_LOCK_DIR);
                this.setCRS(data.NAV_LOCALIZER_1);
                this.XTK.text("CDI " + (data.HSI_CDI_NEEDLE).toFixed(2).toString() + '°');

            } else {
                var a = data.PLANE_HEADING_DEGREES_GYRO - data.NAV_OBS_2;
                this.Needle.dist(data.NAV_CDI_2 / 127);
                this.Needle.setHasNav(data.NAV_HAS_NAV_2);
                this.Needle.fromto(data.NAV_HAS_NAV_2 ? data.NAV_TOFROM_2 : 0);
                this.setCRS(data.NAV_OBS_2);
                this.XTK.text("CDI " + (data.NAV_CDI_2).toFixed(2).toString() + '°');

                this.Needle.rotate(-a);
                this.Needle.dist(data.NAV_CDI_2 / 127);
                this.NAV.text("VOR2");
            }
        }
    }


    setColors(m) {
        switch (m) {
            case 1: // VOR1
            this.Needle.fill("#00e113")
            this.Needle.stroke({width: 1, color: "black"})
            this.NAV.text("VOR1").fg("#00e113")
                this.ENR.hide()
                this.XTK.hide()
                break
            case 2: // VOR2
                this.Needle.fill("transparent")
                this.Needle.stroke({width: 3, color: "#00e113"})
                this.NAV.text("VOR2").fg("#00e113")
                this.ENR.hide()
                this.XTK.hide()
                break
            case 3: // GPS
                this.Needle.fill("magenta")
                this.Needle.stroke({width: 1, color: "black"})
                this.NAV.text("GPS").fg("magenta")
                this.ENR.show()
                this.XTK.show()
                break

        }
    }

    setDIR(d) {
        d = Math.round(d);
        if (d != this.DIR.Deg) {
            this.DIR.text(padLeft((d % 360 == 0 ? 360 : d % 360), 3, "0") + "°")
            this.DIR.Deg = d
        }
    }

    setHDG(d) {
        d = Math.round(d);
        if (d != this.HDGText.Deg) {
            this.HDGText.text(padLeft((d % 360 == 0 ? 360 : d % 360), 3, "0") + "°")
            this.HDGText.Deg = d
        }
    }

    setCRS(d) {
        d = Math.round(d);
        if (d != this.CRSText.Deg) {
            this.CRSText.text(padLeft((d % 360 == 0 ? 360 : d % 360), 3, "0") + "°")
            this.CRSText.Deg = d
        }
    }
}

