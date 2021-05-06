class SpeedIndicator extends SVG.Svg {
    constructor(width, height, num = 5, mul = 100) {
        super();

        var opt = {};
        var font1 = opt.font || { fill: "white", 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 32, leading: '0em' }

        this.height = height;
        this.width = width;
        this.cy = height / 2.0;

        this.rect(width, height).fill("#00000040");

        // this.v0 = 45;
        // this.v1 = 120;
        // this.v2 = 180;
        // this.v3 = 300;

        this.mul = 10;
        this.step = (height / num);
        this.num = (height / this.step) + 3
        if (this.num % 2 == 0) {
            this.num++;
        }
        var settings = {}

        this.SpeedsGroup = this.group();
        this.SpeedClip = this.rect(width, height).fill("red");
        this.ScaleGroup = this.group();
        this.ScaleClip = this.rect(width, height).fill("red");

        var n = Math.ceil((this.num - 1) / 2);
        this.Texts = new Array();
        for (var i = -n; i <= n; i++) {

            var y1 = Math.round(this.cy - i * this.step);

            var v = i >= 0 ? i * this.mul : "\u00A0";
            var t = this.ScaleGroup.text(v.toString()).font(font1);
            t.move(width - t.bbox().w - 25, y1 - t.bbox().h / 2)
            this.Texts.push(t);

            var l = this.ScaleGroup.line(width, y1, width - 20, y1).stroke({ width: 2, color: "white" });

            if (i > -n) {
                var s2 = this.step / 2;
                var y = Math.round(y1 + 1 * s2);
                var l = this.ScaleGroup.line(width, y, width - 20, y).stroke({ width: 2, color: "white" });
            }
        }

        this.ScaleGroup.clipWith(this.ScaleClip)
        this.ScaleGroup.y(this.cy - this.ScaleGroup.height() / 2 + 0)

        //===================================================================================================
        // SPEED LIMITS
        //===================================================================================================
        this.speed_max = 999;
        this.v0p = (settings.v0 || 45) * this.step / this.mul;
        this.v1p = (settings.v1 || 130) * this.step / this.mul;
        this.v2p = (settings.v0 || 165) * this.step / this.mul;
        this.v3p = this.speed_max * this.step / this.mul;
        this.f0p = (settings.f0 || 40) * this.step / this.mul;
        this.f1p = (settings.f1 || 90) * this.step / this.mul;

        this.SpeedsGroup.line(0, 0, 0, -this.v0p).stroke({ width: 13, color: "red" });
        this.SpeedsGroup.line(0, -this.v0p, 0, -this.v1p).stroke({ width: 13, color: "green" });
        this.SpeedsGroup.line(0, -this.v1p, 0, -this.v2p).stroke({ width: 13, color: "yellow" });
        this.SpeedsGroup.line(0, -this.v2p, 0, -this.v3p).stroke({ width: 13, color: "red" });
        this.SpeedsGroup.line(4, -this.f0p, 4, -this.f1p).stroke({ width: 6, color: "whitesmoke" });

        this.SpeedsGroup.move(width - 7, this.cy - this.SpeedsGroup.height());
        this.SpeedsGroup.clipWith(this.SpeedClip)





        //===================================================================================================
        // SPEED BUG
        //===================================================================================================
        this.SpeedBug = this.ScaleGroup.polygon('0,-12 12,-12 12,12 0,12 0,6 6,0 0,-6').fill('cyan').stroke({ width: 1, color: 'black' });
        this.SpeedBug.move(this.width - 12, this.cy - this.SpeedBug.height() / 2 + 0)




        //===================================================================================================
        // COUNTER
        //===================================================================================================

        // var polyline = this.polyline('2,0 20,-18 83,-18 83,-38 126,-38 125,38 83,38 83,18, 20,18 2,0').fill('black').stroke({ width: 2, color: 'black' });//.move(this.left + 4, this.centerY - 40);
        // var polyline1 = this.polyline('2,0 20,-20 81,-20 81,-40 122,-40 122,40 81,40 81,20, 20,20 2,0').fill('black').stroke({ width: 2, color: 'black' });//.move(this.left + 4, this.centerY - 40);
        // polyline.move(width - polyline.width() , this.cy - polyline.bbox().height / 2);
        // polyline1.move(width - polyline.width() , this.cy - polyline1.bbox().height / 2);
        var polyline = this.polyline('50,-18 106,-18 106,-28 130,-28 130,-10 140,0 130,10 130,28 106,28 106,18, 50,20 50,-18').fill('black').stroke({ width: 6, color: 'black' })
        polyline.move(0, this.cy - polyline.bbox().height / 2);
        //var polyline1 = this.svg.polyline('50,-20 100,-20 100,-30 130,-30 130,-20 138,-20 138,-10 150,0 138,10 138,20 130,20 130,30 100,30 100,20 50,20').fill('black').stroke({ width: 2, color: 'black' }).move(this.right - w, this.centerY - 40);
        var polyline1 = this.polyline('50,-20 106,-20 106,-30 130,-30 130,-10 142,0 130,10 130,30 106,30 106,20, 50,20 50,-20').fill('black').stroke({ width: 2, color: 'black' })
        polyline1.move(0, this.cy - polyline1.bbox().height / 2);


        this.nums = new Array();
        this.container = this.group();

        for (var j = 0; j < 3; j++) {
            var group = this.group();
            this.nums.push(group);
            for (var i = 0; i < 5; i++) {
                var text = this.text((i).toString()).font(font1);
                group.add(text.y(i * 31));
            }
            group.move(0 + (n - j) * (group.bbox().width + 1), this.top - group.bbox().height / 2);
            this.container.add(group);

        }

        this.container.move(20, polyline.bbox().cy - this.container.bbox().h / 2);

        this.container.clipWith(polyline);



        // ZERO
        //this.line(0, this.cy, width, this.cy).stroke({ width: 3, color: "yellow" });


        this.currentSpeed = 0;
        this.currentSpeedBug = 0;
    };

    setSpeed(speed) {
        if (this.pSpeed != speed) {
            speed = Math.max(0, Math.min(this.speed_max, speed));

            var dy = (speed % this.mul) / this.mul * this.step;
            this.ScaleGroup.y(Math.round(this.cy - this.ScaleGroup.height() / 2 + dy))

            let a1 = (speed >= 0 ? Math.floor(speed / 10) : Math.ceil(speed / 10))
            let a2 = (this.currentSpeed >= 0 ? Math.floor(this.currentSpeed / 10) : Math.ceil(this.currentSpeed / 10))

            if (a1 != a2) {
                var c = a1 * 10 - 40

                this.Texts.forEach(t => {
                    var v = c >= 0 ? c : "\u00A0";
                    t.text((v).toString())
                    c += 10;
                    t.x(this.width - t.bbox().w - 25)
                });
                this.currentSpeed = speed;
            }

            var dy = speed / this.mul * this.step;
            this.SpeedsGroup.y(Math.round(this.cy - this.SpeedsGroup.height() + dy));

            this.updateCounter(speed);
            this.pSpeed = speed;

            dy = Math.max(this.height / -2, Math.min((this.currentSpeedBug - speed) / this.mul * this.step, this.height / 2))
            this.SpeedBug.move(this.width - 13, Math.round(this.cy - this.SpeedBug.height() / 2 - dy))

        }

        //
        // BUG
        //


    }

    setBug(speed) {
        if (this.currentSpeedBug != speed) {
            speed = Math.max(0, Math.min(999, speed));
            this.currentSpeedBug = speed;
            //this.setSpeed(this.currentSpeed)
        }
    }


    updateGroup(group, d, mul, dy) {

        var nump2 = group.node.childNodes[0].instance;
        var nump1 = group.node.childNodes[1].instance;
        var num0 = group.node.childNodes[2].instance;
        var numm1 = group.node.childNodes[3].instance;
        var numm2 = group.node.childNodes[4].instance;

        var h = group.bbox().height;
        group.y(Math.round(this.cy - h / 2 + (h - 10) / 5 * dy) - 1);

        var d = Math.floor(d / mul) % 10;
        var e = d % 10 + 2;
        var z = "";
        if (mul == 1)
            z = "";

        nump2.text(((e--) % 10).toString() + z);
        nump1.text(((e--) % 10).toString() + z);
        num0.text((e--).toString() + z);
        e = e < 0 ? 10 + e : e;
        numm1.text(((e--) % 10).toString() + z);
        e = e < 0 ? 10 + e : e;
        numm2.text(((e--) % 10).toString() + z);
    }

    pad(num, n) {
        while (num.length < n) {
            num = '1' + num;
        }
        return num;
    }

    updateCounter(d) {
        var s = Math.sign(d);
        d = Math.abs(d)


        var dy = d % 1;
        var num = this.pad(Math.floor(d).toString(), 3);

        this.updateGroup(this.nums[0], d, 1, dy);
        var he = 10;
        for (var i = 1; i < this.nums.length; i++) {
            if (num[num.length - i] != '9') {
                dy = 0;
            }

            this.updateGroup(this.nums[i], d, he, dy);
            he = he * 10;
        }
    }


}

SVG.SpeedIndicatorGauge = class extends SVG.G {
    constructor(w, h) {
        super();


        var opt = {};
        var font1 = opt.font || { fill: "cyan", 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 32, leading: '0em' }
        var font2 = opt.font || { fill: "cyan", 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 28, leading: '0em' }

        this.Ruler = this.speedindicator(w, h)

        this.SpeedLockGroup = this.group();
        this.SpeedLockBox = this.SpeedLockGroup.rect(w, 34).move(0, this.Ruler.bbox().y - 34)
        this.SpeedLockText = this.SpeedLockGroup.text("###").font(font1)
        this.SpeedLockText.move(this.SpeedLockBox.bbox().x + 5, this.SpeedLockBox.bbox().y - 2)

        this.SpeedLockGroup.polygon('0,-12 12,-12 12,12 0,12 0,6 6,0 0,-6').fill('cyan').stroke({ width: 1, color: 'black' }).center(w - 10, this.SpeedLockBox.bbox().cy)

        this.TASBox = this.rect(w, 34).move(0, this.Ruler.bbox().y + h)
        this.TASText = this.text("###KT").font(font2)
        this.TASText.move(5, this.TASBox.bbox().y)

    }

    update(data) {

        this.setSpeed(data.AIRSPEED_INDICATED)
        this.setSpeedLock(data.AUTOPILOT_AIRSPEED_HOLD_VAR)
        this.setSpeedBug(data.AUTOPILOT_AIRSPEED_HOLD_VAR)
        this.setTAS(data.AIRSPEED_TRUE)

        //this.SpeedLockText.move(this.SpeedLockBox.bbox().x + 5, this.SpeedLockBox.bbox().y -2)
        //this.TASText.move(this.TASBox.bbox().x, this.TASBox.bbox().y)
        if (this.prevAUTOPILOT_FLIGHT_LEVEL_CHANGE != data.AUTOPILOT_FLIGHT_LEVEL_CHANGE) {
            if (data.AUTOPILOT_FLIGHT_LEVEL_CHANGE == 1) {
                this.SpeedLockGroup.show();
                this.Ruler.SpeedBug.show();
            }
            else {
                this.SpeedLockGroup.hide();
                this.Ruler.SpeedBug.hide()
            }
            this.prevAUTOPILOT_FLIGHT_LEVEL_CHANGE = data.AUTOPILOT_FLIGHT_LEVEL_CHANGE;
        }


    }

    setSpeed(speed) {
        this.Ruler.setSpeed(speed)
        // if (this.pSpeedLock != speed) {
        //     speed = Math.max(0, Math.min(999, speed));
        //     this.SpeedLockText.text(speed.toFixed(0))
        //     //this.SpeedLockText.x(this.SpeedLockBox.bbox().x + 5);
        //     this.pSpeedLock = speed;
        // }

    }
    setSpeedBug(speed) {
        this.Ruler.setBug(speed)
        // if (this.pSpeedLock != speed) {
        //     speed = Math.max(0, Math.min(999, speed));
        //     this.SpeedLockText.text(speed.toFixed(0))
        //     //this.SpeedLockText.x(this.SpeedLockBox.bbox().x + 5);
        //     this.pSpeedLock = speed;
        // }

    }

    setSpeedLock(speed) {
        if (this.pSpeedLock != speed) {
            speed = Math.max(0, Math.min(999, speed));
            this.SpeedLockText.text(speed.toFixed(0))
            this.SpeedLockText.x(this.SpeedLockBox.bbox().x2 - this.SpeedLockText.bbox().w - 25);
            this.pSpeedLock = speed;
        }
    }

    setTAS(tas) {
        if (this.pTAS != tas) {
            tas = Math.max(0, Math.min(999, tas));
            this.TASText.text(tas.toFixed(0) + "KT")
            this.TASText.x(this.TASBox.bbox().x2 - this.TASText.bbox().w - 5);
            this.pTAS = tas;
        }
    }

    // translate(x,y) {
    //     super.translate(x,y)
    //     //this.SpeedLockBox.translate(x,y)
    //     return this;
    // }
}
