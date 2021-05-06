var fonts =
{
    font1: { 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 42, leading: '1em' },
    font2: { 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 35, leading: '1em' },
    font3: { 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 28, leading: '1em' },
    font4: { 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 21, leading: '1em' },
    font5: { 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 14, leading: '1em' },
    font5: { 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 10, leading: '1em' },
}

var button = { font: fonts.font5, bg: "black", fg: "white" };

function polarToCartesian(cx, cy, r, deg) {
    var ra = (deg - 90) * Math.PI / 180.0;
    return { x: cx + (r * Math.cos(ra)), y: cy + (r * Math.sin(ra)) };
}

function describeArc(x, y, radius, startAngle, endAngle) {

    var start = polarToCartesian(x, y, radius, endAngle);
    var end = polarToCartesian(x, y, radius, startAngle);

    var largeArcFlag = endAngle - startAngle <= 180 ? "0" : "1";
    //largeArcFlag = 1;
    var d = [
        "M", start.x, start.y,
        "A", radius, radius, 0, largeArcFlag, 0, end.x, end.y,
        ""
    ].join(" ");

    return d;
}

SVG.BoxedText = class extends SVG.G {
    constructor(w, h, t) {
        super()
        this.Rect = super.rect(w,h)
        this.Text = super.text(t)
    }

    text(t) {
        this.Text.text(t).center(this.Rect.bbox().cx, this.Rect.bbox().cy)
        return this
    }

    font(font) {
        this.Text.font(font)
        this.Text.center(this.Rect.bbox().cx,this.Rect.bbox().cy)
        return this
    }
    move(x,y) {
        this.Rect.move(x,y)
        this.Text.center(this.Rect.bbox().cx,this.Rect.bbox().cy)
        return this
    }

    center(x,y) {
        this.Rect.center(x,y)
        this.Text.center(this.Rect.bbox().cx,this.Rect.bbox().cy)
        return this
    }

    fg(color) {
        this.Text.fill(color)
        return this
    }
    bg(color) {
        this.Rect.fill(color)
        return this
    }
}

SVG.Button = class extends SVG.G {

    constructor(w, h, title, options, callback) {
        super();

        this.attr({ cursor: "pointer" });
        var opt = options || {};
        this.bg = opt.bg || "black";
        this.fg = opt.fg || "white";
        this.font = opt.font || { 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 14, leading: '0em' }
        this.r = opt.radius || 5;
        this.stroke = opt.stroke || { width: 2, color: this.fg };
        this.rect(w, h).attr({ rx: this.r, ry: this.r }).fill(this.bg).stroke(this.stroke);
        this.Text = this.text(title).font(this.font).fill(this.fg).center(w / 2, h / 2);
    }
    move(x, y) {
        super.move(x, y);
        this.Text.center(super.bbox().cx, super.bbox().cy);
        return this;
    }

};

SVG.IndicatorButton = class extends SVG.Button {
    constructor(w, h, title, options, callback) {
        super(w, h, title, options, callback);

        var opt = options || {};
        this.color_on = opt.color_on || "#22ff33";
        this.color_off = opt.color_off || "gray";
        this.LED = this.rect(w * 0.8, 4).fill(this.color_off).move(w * 0.1, 3);
        this.ON = false;
    }

    setON(on) {
        this.ON = on;
        this.LED.fill(on ? this.color_on : this.color_off);
        return this;
    }

    update(data) { }
}

SVG.CircleButton = class extends SVG.G {
    constructor(owner, diam, text, callback = null) {
        super();
        this.owner = owner;
        this.attr({ cursor: "pointer" });
        var c = this.circle(diam).stroke({ width: 2, color: "white" });
        this.add(c);
        var b = c.bbox();
        this.text = this.text(text).fill("white");
        this.add(this.text);
        this.text.center(b.cx, b.cy - this.text.bbox().height);

        this.on("mousedown", function () {
            if (callback != null) {
                callback(this);
            }
        })

    }

    font(f) {
        this.text.font(f);
        this.text.center(this.cx(), this.cy());
        return this;
    }
}

SVG.RectButton = class extends SVG.G {
    constructor(width, height, text, callback = null, enabled = true) {
        super();
        this.enabled = enabled
        let color = enabled ? "white" : "gray"
        if (enabled) {
            this.attr({ cursor: "pointer", rx: 15, ry: 15 });
            let self = this;
            this.on("mousedown", function () {
                if (callback != null) {
                    callback(self.text.text());
                }
            })
        }

        var c = this.rect(width, height).stroke({ width: 2, color: color }).attr({ rx: 10, ry: 10 });;
        this.add(c);
        var b = c.bbox();
        this.text = this.text(text).fill(color);
        this.add(this.text);
        this.text.center(b.cx, b.cy - this.text.bbox().height);


    }

    font(f) {
        let color = this.enabled ? "white" : "gray"
        this.text.font(f).fill(color);
        this.text.center(this.cx(), this.cy());
        return this;
    }

}

SVG.ImageButton = class extends SVG.G {
    constructor(owner, text, image_on, image_off, callback = null) {
        super();
        this.owner = owner;
        this.attr({ cursor: "pointer" });
        this.ImageOn = this.image(image_on);
        this.ImageOff = this.image(image_off);
        //this.Text = this.text(text);


        this.add(this.ImageOn);
        this.add(this.ImageOff);
        //this.add(this.Text);

        this.on("mousedown", function () {
            if (callback != null) {
                callback(this);
            }
        })

    }

    setIsON(state) {
        if (state) {
            this.ImageOff.hide();
        } else {
            this.ImageOff.show();
        }

    }

    font(f) {
        //this.Text.font(f);
        var b = this.bbox(); //this.ImageOn.bbox();
        //this.Text.move(b.cx, b.y2 + b.height);
        return this;
    }
}

SVG.IconButton = class extends SVG.G {
    constructor(width, height, path) {
        super();

        var c = this.rect(width, height).stroke({ width: 1, color: "white" }).attr({ rx: 3, ry: 3 });;
        //this.add(c);

        var p = this.path(path).fill("white");
        p.center(c.bbox().cx, c.bbox().cy);
        //this.add(p);



        this.attr({ cursor: "pointer" });
        //this.add(this.Text);

        // this.on("mousedown", function () {
        //     if (callback != null) {
        //         callback(this);
        //     }
        // })

    }

    setIsON(state) {
        if (state) {
            this.ImageOff.hide();
        } else {
            this.ImageOff.show();
        }

    }

}

SVG.Rotary = class extends SVG.G {
    constructor() {
        super();
        this.attr({ cursor: "pointer" });
        this.circle(60).fill("#202020").stroke({ width: 3, color: "#a0a0a0" })
        this.rect(3, 12).fill("#808080").center(this.bbox().cx, 10);
    }

    rotate(dir) {
        super.rotate(dir / 100);
    }

}

SVG.LCDDisplay = class extends SVG.G {
    constructor(w, h, text, options) {
        super();
        var opt = options || {};
        var fill = opt.fill || "red";
        var pat = opt.pat || "00000";
        var font_size = opt.font_size || 32
        this.Rect = this.rect(w, h).stroke({ width: 5, color: "gray" });

        this.Text2 = this.text(pat).font({ "font-family": "SevenSegment", fill: "#404040", size: font_size, leading: '0em' });
        this.Text2.attr({ "x": this.Rect.bbox().x2 - 5, "y": this.Rect.bbox().cy + 12, "dominant-baseline": "middle", "text-anchor": "end" });

        this.Text = this.text(text).font({ "font-family": "SevenSegment", fill: fill, size: font_size, leading: '0em' });
        this.Text.attr({ "x": this.Rect.bbox().x2 - 5, "y": this.Rect.bbox().cy + 12, "dominant-baseline": "middle", "text-anchor": "end" });

    }

   

    setText(text) {
        this.Text.text(text);
   
    } 
}

SVG.RPMGauge = class extends SVG.G {
    constructor(w, h, r, options = null) {
        super();
        this.options = options;

        //this.rect(w, h).fill("blue").move(0, 0);
        var r = r - 5;
        var r1 = r - 8;
        this.cx = w / 2;
        this.cy = h / 2;

        this.path(describeArc(this.cx, this.cy, r1, -110, 110)).fill("none").stroke({ color: 'white', width: 8 })
        this.path(describeArc(this.cx, this.cy, r1, -110 + this.getAngle(options.opt1), -110 + this.getAngle(options.opt2))).fill("none").stroke({ color: 'green', width: 8 })
        this.path(describeArc(this.cx, this.cy, r1, -110 + this.getAngle(options.opt2), 110)).fill("none").stroke({ color: 'red', width: 8 })
        this.path(describeArc(this.cx, this.cy, r, -110, 110)).fill("none").stroke({ color: 'white', width: 4 });//.center(cx,cy)

        this.needle = this.polyline("0,0 7,13 3,13 3,50 -3,50 -3,13 -7,13 0,0").fill("white").stroke({ width: 1, color: "black" })
            .center(this.cx, this.cy / 2 + 5)

        this.needle.rotate(-110, this.cx, this.cy)
        this.prevAngle = 0;

        this.text("RPM").font(options.font1).center(this.cx, this.cy + 18);
        this.RPM = this.text("1000").font(options.font2).center(this.cx, this.cy + 40);

        var p = polarToCartesian(this.cx, this.cy, r, 240);
        this.text("0").font(options.font1).center(p.x, p.y);

        var p = polarToCartesian(this.cx, this.cy, r, 120);
        this.text((options.max).toString()).font(options.font1).center(p.x, p.y);
    }

    setRPM(rpm) {
        rpm = Math.max(this.options.min, Math.min(this.options.max, rpm));
        let a = this.getAngle(rpm);
        this.needle.rotate(a - this.prevAngle, this.cx, this.cy);
        this.prevAngle = a;

        this.RPM.text(rpm.toFixed(0).toString());
        this.RPM.center(this.bbox().cx);
    }

    getAngle(v) {
        return v / this.options.max * 220;
    }

    // move(x,y){
    //     this.panel.move(x,y)
    //     return this;
    // }
}

SVG.HorizontalGauge = class extends SVG.G {
    constructor(width, height, opt = null) {
        super()
        this.opt = opt;
        // this.panel = this.group();
        var g = this;

        var cx = width / 2;
        var cy = height / 2;
        g.rect(width, height);//.fill("red");
        g.text(opt.title).font(opt.font1).center(cx, 5);

        var y = cy - 5;
        var yy = y + 12;
        var l = 10;
        var r = width - l;


        g.line(l, yy, r, yy).stroke({ width: 3, color: "white" });

        for (var j = 0; j < opt.limits.length; j++) {
            var lm = opt.limits[j];
            g.line(l + this.getValue(lm.min), yy - 5, l + this.getValue(lm.max), yy - 5).stroke({ width: 8, color: lm.color });
        }

        if (opt.div > 0) {
            for (var i = 0; i <= opt.max; i += opt.div) {
                g.line(l + this.getValue(i), y, l + this.getValue(i), yy + 1).stroke({ width: 2, color: "white" });
            }
        } else {
            g.line(l, y, l, yy + 1).stroke({ width: 2, color: "white" });
            g.line(r, y, r, yy + 1).stroke({ width: 2, color: "white" });

        }

        g.text(opt.min.toString()).font(opt.font1).center(l, yy + 10);
        var t = g.text(opt.max.toString()).font(opt.font1);
        t.move(g.bbox().x2 - t.bbox().w, yy + 0);

        this.marker = g.polyline("0,0 10,0 10,10 5,15 0,10  ").fill("white").center(l, y);

        this.marker.center(l + this.getValue(10));
        this.ValueText = g.text("0000").font(opt.font3).center(cx, cy + 20);
        const self = this;

        this.setValue = function (v) {
            if(v) {
            var b = self.bbox();
            self.marker.center(self.bbox().x + 10 + self.getValue(v));
            self.ValueText.text(v.toFixed(self.opt.fixed).toString()).center(b.cx);
            }
        }


    }

    // move(x, y) {
    //     this.group.move(x, y);
    //     return this.group;
    // }

    // center(x, y) {
    //     this.group.center(x, y);
    //     return this.group;
    // }

    getValue(v) {
        return v / this.opt.max * (this.bbox().w - 20);
    }

}

SVG.FuelQuantityGauge = class extends SVG.G {
    constructor(width, height, opt = null) {
        super()
        this.opt = opt;
        let font = { 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 14, leading: '0em' }
        var g = this; //.group();

        var cx = width / 2;
        var cy = height / 2;
        g.rect(width, height)//.fill("red");
        g.text(opt.title).font(opt.font1).center(cx, 5);

        var y = cy - 12;
        var yy = y + 12;
        var l = 10;
        var r = width - l;


        g.line(l, yy, r, yy).stroke({ width: 7, color: "gray" });

        for (var j = 0; j < opt.limits.length; j++) {
            var lm = opt.limits[j];
            g.line(l + this.getValue(lm.min), yy, l + this.getValue(lm.max), yy).stroke({ width: 7, color: lm.color });
        }

        if (opt.div > 0) {
            for (var i = 0; i <= opt.max; i += opt.div) {
                g.line(l + this.getValue(i), y + 5, l + this.getValue(i), yy + 7).stroke({ width: 2, color: "white" });
            }
        } else {
            g.line(l, y + 2, l, yy + 7).stroke({ width: 2, color: "white" });
            g.line(r, y, r, yy + 1).stroke({ width: 2, color: "white" });
        }

        g.text(opt.min.toString()).font(opt.font1).center(l, yy + 14);
        var t = g.text(opt.max.toString()).font(opt.font1);
        t.center(g.bbox().x2 - t.bbox().w, yy + 14);

        this.markerL = g.polyline("0,0 10,0 10,10 5,15 0,10").fill("white").center(l, y - 3);

        this.markerR = g.polyline("5,0 10,5 10,15 0,15, 0,5").fill("white").center(l, y + 27);

        this.textL = g.text("L").font(font).center(this.markerL.bbox().cx, this.markerL.bbox().cy - 1)
        this.textR = g.text("R").font(font).center(this.markerR.bbox().cx + 2, this.markerR.bbox().cy + 2)


        this.textT = g.text("00").font(font).fill("cyan").center(this.bbox().cx + 2, this.markerR.bbox().cy + 12)

        const self = this;
        this.setValue = function (left, right) {

            var v = g.bbox().x + 10 + self.getValue(left);
            self.markerL.center(v);
            self.textL.center(v);

            var v = g.bbox().x + 10 + self.getValue(right)
            self.markerR.center(v)
            self.textR.center(v)

            this.textT.text((left+right).toFixed(1))
            this.textT.center(this.bbox().cx)

        }
    }

    getValue(v) {
        return v / this.opt.max * (this.bbox().w - 20);
    }

}

SVG.Magneto = class extends SVG.Svg {
    constructor(width, height, opt = null, callback) {
        super()
        this.callback = callback;
        var font = { 'fill': 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 14, leading: '1em' };

        var g = this.g = this.group();
        g.rect(width, height);//.fill("red");

        var r0 = 40;

        // g.circle(1.5 * r0 * 2).fill("#202020").stroke({ width: 1, color: "gray" }).center(box.cx, box.cy);

        this.Box = g.bbox();
        var c = g.circle(2 * r0).fill("#202020").stroke({ width: 4, color: "gray" }).center(this.Box.cx, height - r0 - 4).attr({ cursor: "pointer" });
        this.Box = c.bbox();

 
        this.Key = g.group();
        this.Key.rect(15, r0 * 0.8).fill("gray").center(this.Box.cx, this.Box.cy);
        this.Key.circle(4).fill("#202020").stroke({ width: 4, color: "black" }).center(this.Box.cx, c.y()-3 )
        //this.Ring = g.circle(30).fill("none").stroke({width: 3, color: "khaki"});

        var gradient = this.Key.gradient('linear', function (add) {
            add.stop(0, 'saddlebrown')
            add.stop(1, 'khaki')
        })

        this.Key.rect(10, r0 * 1.4).fill(gradient).center(this.Box.cx, this.Box.cy);

        //this.Key.rotate(-110, box.cx, box.cy);
        var labels = ["OFF", "R", "L", "BOTH", "START"];
        for (var i = 0; i < labels.length; i++) {
            var r = i < 3 ? r0 * 1.5 : r0 * 1.7;
            var p = polarToCartesian(this.Box.cx, this.Box.cy, r, -90 + i * 45);
            var t = g.text(labels[i]).font(font);
            t.center(p.x, p.y);
        }

        this.Key.rotate(0, this.Box.cx, this.Box.cy);

        this.Pos = 0;
        const mag = this;
        this.Rect = g.rect(width, height).fill("transparent").attr({ cursor: "pointer" }).on("click", function (e) {

            var b = e.currentTarget.instance.bbox(); // mag.Rect.bbox();
            console.log(b);
            if (e.screenX < b.cx) {
                mag.callback(mag, -1);
            } else {
                mag.callback(mag, 1);
            }
            console.log(mag.Pos);
        })

        //        this.update();
    }

    move(x, y) {
        this.g.move(x, y);
    }

    invalidate() {
        if (this.prevPos >= 0) {
            this.Key.rotate(90 + -this.prevPos * 45, this.Box.cx, this.Box.cy);
        }
        this.Key.rotate(-90 + this.Pos * 45, this.Box.cx, this.Box.cy);
        this.prevPos = this.Pos;

    }
    setPos(p) {
        this.Pos = p;
        this.invalidate();
    }
}

SVG.TrimIndicator = class extends SVG.G {
    constructor(width, height, opt = null) {
        super();

        var box1 = this.rect(width, height).fill("#202020").stroke({ width: 2, color: "white" }).attr({ rx: 10, ry: 10 });

        this.Rect = this.rect(15, height - 20).move(37, 10).fill("#101010");
        this.Pos = this.rect(40, 5).move(25, 10).fill("white");

        var y = 18;
        var h = height - 35;

        this.line(6, y + h * 0.5, 22, y + h * 0.5).stroke({ width: 5, color: "white" })
        this.setPos(100);
    }

    setPos(p) {
        p =  Math.max(-1, Math.min(1, p * 0.025))
        let b = this.Rect.bbox()
        this.Pos.y(b.cy + b.h / 2 * p - 2.5);
    }
}

SVG.FlapsControl = class extends SVG.G {
    constructor(width, height, opt) {
        super();

        this.box1 = this.rect(width, height).fill("#202020").stroke({ width: 2, color: "white" }).attr({ rx: 10, ry: 10 });
        this.box2 = this.rect(15, height - 20).move(37, 10).fill("#101010");

        this.btnUP = this.button(40, 32, "UP").move(width - 50, 10)
        this.btnDN = this.button(40, 32, "DN").move(width - 50, height - 42)

        this.pos = this.rect(40, 15).move(25, 10).fill("blue");

        var y = 17.5;
        var h = height - 35;

        this.line(6, y + h * 0.0, 22, y + h * 0.0).stroke({ width: 5, color: "white" })
        this.line(6, y + h * 0.33, 22, y + h * 0.33).stroke({ width: 5, color: "white" })
        this.line(6, y + h * 0.66, 22, y + h * 0.66).stroke({ width: 5, color: "white" })
        this.line(6, y + h * 1, 22, y + h * 1).stroke({ width: 5, color: "white" })

        this.setPos(0);
    }

    setPos(p) {
        p = Math.max(0, Math.min(1,p))
        let b = this.box2.bbox()
        var y = (b.h - 15) * p;
        this.pos.y(b.y + y);
    }
}

SVG.WindGauge = class extends SVG.G {
    constructor(width, height, opt = null) {
        super()
        opt = opt || {};
        opt.font = opt.font || { 'fill': 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 18, leading: '0em' };
        opt.gyro = opt.gyro == undefined ? true : opt.gyro

        var g = this;

        this.width = width;
        var cx = width / 4;
        var cy = height / 2;
        let Rect = g.rect(width, height).fill("black");
        let Direction = g.text("112°").font(opt.font).y(0);
        Direction.x(Rect.width() - Direction.bbox().w - 4);
        let Velo = g.text("99KT").font(opt.font);
        Velo.move(Rect.width() - Velo.bbox().w - 4, height - Velo.bbox().height);

        let Arrow = g.group();
        Arrow.circle(height - 10).fill("black").center(cx, cy).fill("#202020")
        let pl = Arrow.polygon("0,0 7,7 2,7 2,34 -2,34 -2,7 -7,7").fill("white").stroke({ width: 1, color: "black" })
        pl.center(Arrow.bbox().cx + 7, Arrow.bbox().cy + 2);
        let prevDir = 0;

        this.setData = function (dir, gyro, velo) {
            gyro = (opt.gyro ? gyro : 0)
            dir = (dir) % 360
            Direction.text(dir + "°")
            Direction.x(Rect.bbox().x2 - Direction.bbox().w - 4);
            Velo.text(velo + "KT");
            Velo.x(Rect.bbox().x2 - Velo.bbox().w - 4);
    
            gyro = (opt.gyro ? gyro : 0)
            dir = (180-dir + gyro) % 360
            
            Arrow.rotate(prevDir - dir, Arrow.bbox().cx, Arrow.bbox().cy);
            prevDir = dir
    
        }
    
    }

    // move(x, y) {
    //     this.panel.move(x, y);
    //     return this.panel;
    // }

    // center(x, y) {
    //     this.panel.center(x, y);
    //     return this.panel;
    // }

}
