class Altimeter extends SVG.Svg {
    constructor(width, height, num = 7, mul = 100) {
        super();

        var opt = {};
        var font1 = opt.font || { fill: "white", 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 32, leading: '0em' }

        this.container = this.rect(width, height).fill("red");
        this.rect(width, height).fill("#00000040");
        this.height = height;
        this.width = width;
        this.cy = height / 2.0;

        // this.v0 = 45;
        // this.v1 = 120;
        // this.v2 = 180;
        // this.v3 = 300;

        this.mul = 100;
        this.step = (height / num);
        this.num = (height / this.step) + 3
        if (this.num % 2 == 0) {
            this.num++;
        }

        this.scale = this.group();

        var n = Math.ceil((this.num - 1) / 2);
        this.Texts = new Array();
        for (var i = -n; i <= n; i++) {
            var y1 = Math.round(this.cy - i * this.step);

            var t = this.scale.text((i * mul).toString()).font(font1);
            t.move(width - t.bbox().w - 7, y1 - t.bbox().h / 2)
            this.Texts.push(t);

            var l = this.scale.line(0, y1, 15, y1).stroke({ width: 2, color: "white" });

            if (i > -n) {
                var s2 = this.step / 5;
                for (var j = 1; j < 5; j++) {
                    var y = Math.round(y1 + j * s2);
                    var l = this.scale.line(0, y, 7, y).stroke({ width: 2, color: "white" });
                }
            }
        }

        this.scale.clipWith(this.container)
        this.scale.y(this.cy - this.scale.height() / 2 + 0)

        //===================================================================================================
        // ALT BUG
        //===================================================================================================
        this.AltBug = this.scale.polygon('0,0 0,-12 12,-12 12,-6 6,0 12,6 12,12 0,12').fill('cyan').stroke({ width: 1, color: 'black' })
        this.AltBug.y(this.cy - this.AltBug.height() / 2 + 0)

        //===================================================================================================
        // COUNTER
        //===================================================================================================

        var polyline = this.polyline('2,0 20,-18 83,-18 83,-38 126,-38 125,38 83,38 83,18, 20,18 2,0').fill('black').stroke({ width: 2, color: 'black' });//.move(this.left + 4, this.centerY - 40);
        var polyline1 = this.polyline('2,0 20,-20 81,-20 81,-40 122,-40 122,40 81,40 81,20, 20,20 2,0').fill('black').stroke({ width: 2, color: 'black' });//.move(this.left + 4, this.centerY - 40);
        polyline.move(width - polyline.width() , this.cy - polyline.bbox().height / 2);
        polyline1.move(width - polyline.width() , this.cy - polyline1.bbox().height / 2);

        this.nums = new Array();
        this.container = this.group();

        for (var j = 0; j < 4; j++) {
            var group = this.group();
            this.nums.push(group);
            for (var i = 0; i < 5; i++) {
                var text = this.text((i).toString()).font(font1);
                group.add(text.y(i * 31));
            }
            group.move(0 + (n-j) * (group.bbox().width+1) , this.cy- group.bbox().height /2);
            this.container.add(group);
            
        }

        this.container.move(width - this.container.width() - 24, polyline.bbox().cy - this.container.bbox().h / 2);
        this.container.clipWith(polyline);

        this.SignText = this.text("-").font(font1)
        this.SignText.move(this.container.bbox().x - 20, this.cy - this.SignText.bbox().h / 2);


        // ZERO
        //this.line(0, this.cy, width, this.cy).stroke({ width: 3, color: "yellow" });
        
        
        this.currentAlt = 0;
        this.currentAltBug = 0;
    };

    setAltitude(alt) {

        if(alt < 0) {
            this.SignText.show();
        } else {
            this.SignText.hide();
        }

        alt = Math.max(-10000, Math.min(99999, alt));
        var dy = (alt % this.mul) / this.mul * this.step;

        let a1 = (alt >= 0 ? Math.floor(alt / 100) : Math.ceil(alt / 100))
        let a2 = (this.currentAlt >= 0 ? Math.floor(this.currentAlt / 100) : Math.ceil(this.currentAlt / 100))

        if (a1 != a2) {
            var c = a1 * 100 - 500

            this.Texts.forEach(t => {
                t.text((c).toString())
                c += 100;
                t.x(this.width - t.bbox().w - 5)
            });
            this.currentAlt = alt;
        }
        this.scale.move(0, Math.round(this.cy - this.scale.height() / 2 + dy))


        this.updateCounter(alt/1);
        //
        // BUG
        //
        var b = this.currentAltBug - alt;
        var dy = Math.round(b / this.mul * this.step);
        dy = Math.max(this.height / -2, Math.min(dy, this.height / 2))
        this.AltBug.y(this.cy - this.AltBug.height() / 2 - dy)

    }

    setBug(alt) {
        if (this.currentAltBug != alt) {
            alt = Math.max(-10000, Math.min(99999, alt));
            this.currentAltBug = alt;
            // ide nem az Alt kell, he?
            //this.setAltitude(this.currentAltBug)
        }
    }


    updateGroup(group, d, mul, dy) {
        var nump2 = group.node.childNodes[0].instance;
        var nump1 = group.node.childNodes[1].instance;
        var num0 = group.node.childNodes[2].instance;
        var numm1 = group.node.childNodes[3].instance;
        var numm2 = group.node.childNodes[4].instance;

        var h = group.bbox().height;
        group.y(Math.round(this.cy - h / 2 + (h-10) / 5 * dy )-1);

        var d = Math.floor(d / mul) % 10;
        var e = d % 10 + 2;
        var z = "";
        if(mul == 1)
            z = "0";

        nump2.text(((e--) % 10).toString() + z);
        nump1.text(((e--) % 10).toString() + z);
        num0.text((e--).toString()+ z);
        e = e < 0 ? 10 + e : e;
        numm1.text(((e--) % 10).toString() + z);
        e = e < 0 ? 10 + e : e;
        numm2.text(((e--) % 10).toString()+z);
    }

    pad(num, n) {
        while(num.length < n) {
            num = '1' + num;
        }
        return num;
    }

    updateCounter(d) {
        var s = Math.sign(d);
        d = Math.abs(d)
        
        d = d / 10.0;
        var dy = d % 1;
        //console.log(dy)    
        var num = this.pad(Math.floor(Math.abs(d)).toString(), 4);
        //var num = Math.floor(d.toString());

        this.updateGroup(this.nums[0], d, 1, dy);
        var he = 10;
        
        for(var i = 1; i < this.nums.length; i++) {
            if(num[num.length - i] != '9') {
                dy = 0;
            }

            this.updateGroup(this.nums[i], d, he, dy);
            he = he * 10;
        }
        if(s) {

        }
    }


}

SVG.AltimeterGauge = class extends SVG.G {
    constructor(w, h) {
        super();


        var opt = {};
        var font1 = opt.font || { fill: "cyan", 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 32, leading: '0em' }
        var font2 = opt.font || { fill: "cyan", 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 28, leading: '0em' }

        this.Ruler = this.altimeter(w, h)

        this.AltLockBox = this.rect(w, 34).move(0, this.Ruler.bbox().y - 34)
        this.AltLockText = this.text("#####").font(font1)
        this.AltLockText.move(this.AltLockBox.bbox().x2 - this.AltLockText.bbox().w - 5, this.AltLockBox.bbox().y-3)

        this.polygon('0,0 0,-12 12,-12 12,-6 6,0 12,6 12,12 0,12').fill('cyan').stroke({ width: 1, color: 'black' }).center(10, this.AltLockBox.bbox().cy)
        this.HgBox = this.rect(w, 34).move(0, this.Ruler.bbox().y + h)
        this.HgText = this.text("#####").font(font2)
        this.HgText.move(this.HgBox.bbox().x2 - this.HgText.bbox().w - 5, this.HgBox.bbox().y)

    }

    setAltitude(alt) {
        this.Ruler.setAltitude(alt)
    }

    setTargetAltitude(alt) {
        if (this.pTargetAltitude != alt) {
            alt = Math.max(-10000, Math.min(99999, alt));
            this.AltLockText.text(alt.toFixed(0))
            this.AltLockText.x(this.AltLockBox.bbox().x2 - this.AltLockText.bbox().w - 5);
            this.pTargetAltitude = alt;
        }
    }

    setHg(hg) {
        if (this.pHg != hg) {
            hg = Math.max(0, Math.min(99, hg));
            this.HgText.text(hg.toFixed(2) + "Hg")
            this.HgText.x(this.HgBox.bbox().x2 - this.HgText.bbox().w - 5);
            this.pHg = hg;
        }
    }

    update(data) {
        this.setAltitude(data.INDICATED_ALTITUDE)
        this.setTargetAltitude(data.AUTOPILOT_ALTITUDE_LOCK_VAR)
        this.Ruler.setBug(data.AUTOPILOT_ALTITUDE_LOCK_VAR)
        this.setHg(data.KOHLSMAN_SETTING_HG)
    }
}
