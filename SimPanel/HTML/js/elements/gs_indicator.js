class GSIndicator extends SVG.Svg {
    constructor(width, height, step = 60, mul = 100) {
        super();

        var font = { 'fill': 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 32 };

        this.rect(width, width);
        this.GL = this.text("G").font(font).center(width / 2, width / 2);
        this.gr = this.rect(width, height).fill("#00000040").y(width);

        this.step = 40;
        var x = width / 2;
        var y = this.gr.bbox().cy;

        this.rc = this.rect(width, height).fill("#f00").y(width)
        this.diamond = this.polygon("-7,0 0,-10 7,0 0,10").fill("#00e113").stroke({ width: 2, color: "#00f113" }).center(this.gr.bbox().cx, this.gr.bbox().cy);
        this.diamond.clipWith(this.rc);

        this.circle(7).fill("none").stroke({ width: 2, color: "white" }).center(x, y - 2 * this.step);
        this.circle(7).fill("none").stroke({ width: 2, color: "white" }).center(x, y - this.step);
        this.circle(4).fill("none").stroke({ width: 2, color: "white" }).center(x, y);
        this.circle(7).fill("none").stroke({ width: 2, color: "white" }).center(x, y + this.step);
        this.circle(7).fill("none").stroke({ width: 2, color: "white" }).center(x, y + 2 * this.step);

        this.diamond.center(this.gr.bbox().cx, this.gr.bbox().cy);

    }

    update(data) {
        
        if (this.prevNAV_GS_FLAG_1 != data.NAV_GS_FLAG_1) {
            if (data.NAV_GS_FLAG_1) {
                this.show();

            } else {
                this.hide();
            }

            this.prevNAV_GS_FLAG_1 = data.NAV_GS_FLAG_1;
        }

        // if (data.AUTOPILOT_GLIDESLOPE_ACTIVE) {

        var nav_mode = data.GPS_DRIVES_NAV1 == 1 ? 3 : data.AUTOPILOT_NAV_SELECTED;
        var dm = "";
        var p = -1000;
        switch (nav_mode) {
            case 1:
                if (data.NAV_HAS_GLIDE_SLOPE_1) {
                    dm = "GS";
                    p = data.NAV_GSI_1 / 127.0;
                }
                break;

            case 2:
                if (data.NAV_HAS_GLIDE_SLOPE_2) {
                    dm = "GS";
                    p = data.NAV_GSI_2 / 127.0;
                }
                break;

            case 3:

                if (data.NAV_HAS_GLIDE_SLOPE_1) {
                    dm = "GS";
                    p = data.NAV_GSI_1 / 127.0;
                } else if (data.NAV_HAS_GLIDE_SLOPE_2) {
                    dm = "GS";
                    p = data.NAV_GSI_2 / 127.0;
                }
                break;
        }

        if (this.prevDiamond != p) {
            if (p == -1000) {
                this.diamond.hide();
                this.GL.fill("white");

            }
            else {
                this.diamond.show();
                this.GL.fill("#00e113");
            }
            let b = this.rc.bbox()
            this.diamond.center(b.cx, b.cy + p * this.step * 3);
            this.prevDiamond = p;
        }
        //}
        // else {
        //     this.diamond.hide();
        // }

    }
}