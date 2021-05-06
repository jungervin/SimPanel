class VerticalSpeed extends SVG.Svg {
    constructor(width, height, num = 5, mul = 100) {
        super();

        var opt = {};
        var font1 = opt.font || { fill: "cyan", 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 20, leading: '0em' }
        var font2 = opt.font || { fill: "white", 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 17, leading: '0em' }


        //this.rect(width, height).fill("#00000040");

        this.height = height;
        this.width = width;
        this.cy = height / 2.0;

        //=======================================================
        // VERTICAL SPEED
        //=======================================================

        var bbox = this.bbox();

        var left = 0;
        var cy = height / 2;
        var w = this.width;
        var h = this.height - 72;
        this.hstep = 45;



        for (var i = -2.5; i <= 2.5; i += 0.5) {
            if (i % 1 == 0) {
                this.line(left, cy + i * this.hstep, left + 10, cy + i * this.hstep).stroke({ width: 2, color: 'white' });

                if (i != 0) {
                    this.text(i.toString()).center(20, cy - i * this.hstep).fill('white').font(font2);
                }
            }
            else {
                this.line(left, cy + i * this.hstep, left + 5, cy + i * this.hstep).stroke({ width: 2, color: 'white' });
            }

        }

        var p = [
            [left, cy - h / 2],
            [left + w, cy - h / 2],
            [left + w, cy - 25],
            [left + w - 25, cy],
            [left + w, cy + 25],
            [left + w, cy + h / 2],
            [left, cy + h / 2]
        ];
        this.Base = this.polygon(p).fill('#00000040');

        this.VsFlagGroup = this.group();

        var flag = this.VsFlagGroup.polygon("0,0 15,15 65,15 65,-15, 15,-15").fill('#000000');

        this.VsText = this.VsFlagGroup.text('1500').font(font1);
        this.VsText.move(13, -this.VsText.bbox().h / 2);
        this.VsFlagGroup.move(40, cy - flag.bbox().h / 2)


        this.VSLockBox = this.rect(w, 30).move(left, this.Base.y() - 30).fill('#000000');
        this.VsLockText = this.text('-1500').fill('cyan').font(font1);
        this.VsLockText.move(
            this.VSLockBox.bbox().x2 - this.VsLockText.bbox().w - 5,
            this.VSLockBox.bbox().cy
        )


        this.VsBug = this.polygon('0,0 0,-12 12,-12 12,-6 6,0 12,6 12,12 0,12').fill('cyan').stroke({ width: 1, color: 'black' })
        this.VsBug.move(35, this.cy - this.VsBug.height() / 2 + 0)
        var clip = this.rect(this.Base.width(), this.Base.height()).y(this.Base.y()).fill("red");
        this.VsBug.clipWith(clip)

        this.setVS(1000)
        this.setVSLock(-22500)
    }

    update(data) {
        this.setVS(data.VERTICAL_SPEED);
        this.setVSLock(data.AUTOPILOT_VERTICAL_HOLD_VAR)

        if (this.AUTOPILOT_VERTICAL_HOLD != data.AUTOPILOT_VERTICAL_HOLD) {
            if (data.AUTOPILOT_VERTICAL_HOLD) {
                this.VsLockText.show()
                this.VsBug.show()
            } else {
                this.VsLockText.hide()
                this.VsBug.hide()
            }
            this.AUTOPILOT_VERTICAL_HOLD = data.AUTOPILOT_VERTICAL_HOLD
        }

    }
    setVS(vs) {

        if (this.pVS != vs) {
            var m = Math.min(9999, Math.max(-9999, vs));

            var vs50 = (Math.round(vs / 50) * 50);
            var vss = Math.min(2500, Math.max(-2500, vs));
            if (vs50 == 0) {
                this.VsText.hide()
            } else {
                this.VsText.show()
                this.VsText.text(vs50.toFixed(0));
                if (vss < 0) {
                    this.VsText.x(44);
                } else {
                    this.VsText.x(56);
                }

            }

            this.VsFlagGroup.y(this.cy - this.VsFlagGroup.bbox().h / 2 - vss / 2000 * this.hstep * 2);
            this.pVS = vs
        }
    }

    setVSLock(vs) {
        if (this.pVSLock != vs) {
            var m = Math.min(9999, Math.max(-9999, vs));
            this.VsLockText.text(m.toFixed(0));
            this.VsLockText.move(
                this.VSLockBox.bbox().x2 - this.VsLockText.bbox().w - 5,
                this.VSLockBox.bbox().y + 4
                )
            this.pVSLock = vs

            var vss = Math.min(3000, Math.max(-3000, vs));
            var y = this.cy - this.VsBug.height() / 2 + - vss / 2000 * this.hstep * 2;
            y = Math.max(this.Base.bbox().y - 12, Math.min(y, this.Base.bbox().y2 - 12))
            this.VsBug.move(35, y)
        }
    }
}