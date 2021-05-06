class FlightPlan extends PopupPanel {
    constructor(fms) {
        super("flightplan", -1, -1, 1020, 500)
        const self = this;
        this.FMS = fms;



        var g = this.svg.group(); //this.svg2;

        this.lineh = 26
        let linen = 12
        var font1 = { 'fill': 'magenta', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 22, leading: '1em' };
        var font2 = { 'fill': 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 22, leading: '1em' };

        var Rect = g.rect(1020, 440).fill("black").stroke({ color: "white", width: 3 })

        g.text("FLIGHT PLAN").font(font2).move(10, 10);
        this.display = g.rect(925, this.lineh * linen).move(30, 75).fill("#202020");

        // var w = parseInt(panel.style.width, 10);
        // g.move((w - g.width()) / 2, 100)



        this.Lines = new Array();
        this.FlightPlan = new Array();
        this.CursorPos = 0;
        this.Offset = 0;
        this.btnDown = g.icon_button(32, 32, "M9,4H15V12H19.84L12,19.84L4.16,12H9V4Z").click(function (sender) {
            if (fms.FlightPlan)
                if (self.CursorPos < fms.FlightPlan.length - 1) {
                    self.CursorPos++;
                    // if(self.CursorPos > self.Lines.length) {
                    //     self.Offset++;
                    // }

                    self.invalidate();
                }
        })
        this.btnDown.move(this.display.bbox().x2 + 15, this.display.bbox().y2 - 42)

        this.btnUp = g.icon_button(32, 32, "M15,20H9V12H4.16L12,4.16L19.84,12H15V20Z").click(function (sender) {
            if (self.CursorPos > 0) {
                self.CursorPos--;
                self.invalidate();
            }
        })
        this.btnUp.move(this.display.bbox().x2 + 15, this.display.bbox().y)

        let timer = null
        let btnClose = g.button(100, 30, "CLOSE").click(function (sender) {
            if (timer != null) {
                clearInterval(timer)
                timer = null
            }
            self.close();
        });
        btnClose.center(Rect.bbox().cx, Rect.bbox().y2 - 30);

        let y = 50
        let x = this.display.x() + 10;
        let col1 = g.text("ICAO").font(font1).move(x, y)
        let col2 = g.text("DESC").font(font1).move(x + 80, y)
        let col3 = g.text("T").font(font1).move(x + 340, y)
        let col4 = g.text("BRG").font(font1).move(x + 370, y)
        let col5 = g.text("DIST").font(font1).move(x + 440, y)
        let col6 = g.text("REM").font(font1).move(x + 570, y)
        let col7 = g.text("ETE").font(font1).move(x + 700, y)
        let col8 = g.text("ALT").font(font1).move(x + 810, y)


        for (var i = 0; i < linen; i++) {
            y = this.display.y() + i * this.lineh;

            var line = {
                rect: g.rect(this.display.width(), 26).move(this.display.x(), y).fill("gray"),
                icao: g.text("____").font(font2).move(col1.bbox().x, y),
                desc: g.text("____________________").font(font2).move(col2.bbox().x, y),
                type: g.text("_").font(font2).move(col3.bbox().x, y),
                brg: g.text("___°").font(font2).move(col4.bbox().x, y),
                dist: g.text("____._ NM").font(font2).move(col5.bbox().x, y),
                rem: g.text("____._ NM").font(font2).move(col6.bbox().x, y),
                ete: g.text("__:__").font(font2).move(col7.bbox().x, y),
                alt: g.text("______ FT").font(font2).move(col8.bbox().x, y),

            }
            this.Lines.push(line);
        }

        this.CurrentWaypoint = g.path("M4,15V9H12V4.16L19.84,12L12,19.84V15H4Z").fill("yellow").stroke({ width: 1, color: "yellow" })
        var i = this.FMS.SimData.GPS_FLIGHT_PLAN_WP_INDEX != undefined ? this.FMS.SimData.GPS_FLIGHT_PLAN_WP_INDEX : 0
        this.CurrentWaypoint.move(this.display.bbox().x - 20, this.display.bbox().y + 9 + i * this.lineh).hide();

        var ws = new WebSocket(SETTINGS.ws_sim);
        //g.scale(1);
        // ws.onopen = function () {
        //     ws.send("getflightplan")
        // }
        // ws.onmessage = function (e) {
        //     var res = JSON.parse(e.data);
        //     if (res.type == "flightplan") {
        //         self.FlightPlan = res;
        //         self.update()
        //     }

        //     ws.close();
        // }

        // ws.onerror = function (e) {

        // }

        // self.update()

        self.invalidate()
        timer = setInterval(function (e) {
            self.invalidate()
        }, 1000)

    }

    invalidate() {
        if (this.FMS && this.FMS.FlightPlan) {

            let dy = Math.max(0, this.CursorPos - this.Lines.length + 1);
            let offset = dy


            let i =  this.FMS.SimData.GPS_FLIGHT_PLAN_WP_INDEX != undefined ? this.FMS.SimData.GPS_FLIGHT_PLAN_WP_INDEX : 0
            i -= offset
            if (i >= 0 && i < this.Lines.length) {
                this.CurrentWaypoint.show();
                this.CurrentWaypoint.move(this.display.bbox().x - 20, this.display.bbox().y + 5 + i * this.lineh)
            }
            else {
                this.CurrentWaypoint.hide();
            }

            for (let i = 0; i < this.Lines.length; i++) {
                let line = this.Lines[i];

                let p = offset + i;
                let wp = p < this.FMS.FlightPlan.length ? this.FMS.FlightPlan[p] : null;

                let dist = 0;
                let brg = "";
                let ete = ""
                if (wp != null) {


                }
                // if (d != null && i > 0) {
                //     var p1 = new LatLonSpherical(this.FlightPlan.data[p - 1].lat, this.FlightPlan.data[p - 1].lng);
                //     var p2 = new LatLonSpherical(d.lat, d.lng);

                //     brg = (p1.rhumbBearingTo(p2) - (this.FMS.SimData.GPS_MAGVAR || 0)).toFixed(0); 
                //     dist = p1.distanceTo(p2) * LatLonSpherical.metresToNauticalMiles;
                // }

                let cp = this.CursorPos - offset;

                if (offset > 0) {
                    cp = this.Lines.length - 1;
                }

                if (wp != null && i == cp) {
                    line.rect.fill("gray");
                } else {
                    line.rect.fill("#101010");
                }
                line.icao.text(wp != null ? wp.icao : "");
                var desc = wp != null ? wp.desc.substring(0, 21) : "";
                if (desc.length > 20) {
                    desc = desc.substring(0, 19) + '..';
                }

                line.desc.text(desc);
                line.type.text(wp != null ? wp.wptype : "");
                line.brg.text(wp != null ? padLeft(wp.brg, 3, "\u00A0") + "°" : "");
                line.dist.text(wp != null ? padLeft(wp.dist.toFixed(1), 6, "\u00A0") + " NM" : "");
                if (wp != null) {
                    if (wp.ete != Infinity) {
                        if (wp.ete != 0) {
                            let t = secsToTime(wp.ete)
                            line.ete.text(t.h + ":" + padLeft(t.m, 2, "0") + ":" + padLeft(t.s, 2, "0"))
                        } else {
                            line.ete.text("")
                        }
                    } else {
                        line.ete.text("")
                    }
                } else {
                    line.ete.text("")
                }


                line.rem.text(wp != null && wp.rem != 0 ? padLeft(wp.rem.toFixed(1), 6, "\u00A0") + " NM" : "");
                line.alt.text(wp != null ? padLeft(wp.alt.toFixed(0), 5, "\u00A0") + " FT" : "\u00A0");

            }
        } else {
            
        }
    }


    invalidate2() {
        if (this.FMS && this.FMS.FlightPlan) {

            if (this.FlightPlan)
                this.CurrentWaypoint.show();
            // if(this.CursorPos > this.Lines.length) {

            // }
            var dy = Math.max(0, this.CursorPos - this.Lines.length + 1);
            var offset = dy

            for (var i = 0; i < this.Lines.length; i++) {
                var line = this.Lines[i];

                var p = offset + i;
                var d = this.FlightPlan.data != null && p < this.FlightPlan.data.length ? this.FlightPlan.data[p] : null;

                var dist = 0;
                var brg = "";
                if (d != null && i > 0) {
                    var p1 = new LatLonSpherical(this.FlightPlan.data[p - 1].lat, this.FlightPlan.data[p - 1].lng);
                    var p2 = new LatLonSpherical(d.lat, d.lng);

                    brg = (p1.rhumbBearingTo(p2) - (this.FMS.SimData.GPS_MAGVAR || 0)).toFixed(0);
                    dist = p1.distanceTo(p2) * LatLonSpherical.metresToNauticalMiles;
                }

                var cp = this.CursorPos - offset;

                if (offset > 0) {
                    cp = this.Lines.length - 1;
                }

                if (d != null && i == cp) {
                    line.rect.fill("gray");
                } else {
                    line.rect.fill("black");
                }
                line.icao.text(d != null ? d.icao : "");
                var desc = d != null ? d.desc.substring(0, 21) : "";
                if (desc.length > 20) {
                    desc = desc.substring(0, 19) + '..';
                }

                line.desc.text(desc);
                line.type.text(d != null ? d.wptype : "");
                line.brg.text(brg != "" ? padLeft(brg, 3, "\u00A0") + "°" : "");
                line.dist.text(d != null ? padLeft(dist.toFixed(1), 6, "\u00A0") + " NM" : "");
                line.alt.text(d != null ? padLeft(d.alt.toFixed(0), 5, "\u00A0") + " FT" : "\u00A0");

            }
        }
    }

}