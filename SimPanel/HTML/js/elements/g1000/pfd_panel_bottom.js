class G1000PFDPanelBottom {
    constructor(fms, w, h) {

        let g = fms.svg.group();
        this.panel = g;

        g.rect(w, h)


        let bw = 60;
        let bh = 32;
        let bs = bw + 10;
        let x = 50;
        let y = 30;

        this.btnMenu = g.group();
        var c = this.btnMenu.circle(30).fill("black").stroke({ width: 2, color: "white" });
        this.btnMenu.path("M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2M6,7H18V9H6V7M6,11H18V13H6V11M6,15H18V17H6V15Z").fill("gray").center(c.bbox().cx, c.bbox().cy).scale(1.5);
        this.btnMenu.attr({ cursor: "pointer" });
        this.btnMenu.move(5, 30);

        this.btnZoom = g.indicator_button(bw, bh, "ZOOM").move(x + bs * 0, y)
        this.btnTRCK = g.indicator_button(bw, bh, "TRCK").move(x + bs * 1, y)

        g.button(bw, bh, "NAV1").move(x + bs * 2, y).click(function (e) {
            let numpd = new NumPad("NAV1", "000.00").onclose(function (numpad) {
                if (numpad.Result == "ENTER") {
                    fms.sendCommand("NAV1_STBY_SET", decimalToBCD(numpad.Value));
                }
                numpd.close()
            })
        })

        g.button(bw, bh, "NAV2").move(x + bs * 3, y).click(function (e) {
            let numpd = new NumPad("NAV2", "000.00").onclose(function (numpad) {
                if (numpad.Result == "ENTER") {
                    fms.sendCommand("NAV2_STBY_SET", decimalToBCD(numpad.Value));
                }
                numpd.close()
            })
        })

        g.button(bw, bh, "COM1").move(x + bs * 4, y).click(function (e) {
            let numpd = new NumPad("COM1", "000.000").onclose(function (numpad) {
                if (numpad.Result == "ENTER") {
                    fms.sendCommand("COM_STBY_RADIO_SET", decimalToBCD(numpad.Value));
                }
                numpd.close()
            })
        })

        g.button(bw, bh, "COM2").move(x + bs * 5, y).click(function (e) {
            let numpd = new NumPad("COM2", "000.000").onclose(function (numpad) {
                if (numpad.Result == "ENTER") {
                    fms.sendCommand("COM2_STBY_RADIO_SET", decimalToBCD(numpad.Value));
                }
                numpd.close()
            })
        })

        g.button(bw, bh, "CDI").move(x + bs * 6, y).click(function (e) {
            //pfd.sendCommand("SimPanel.AS1000_PFD_SOFTKEYS_6");
            if (fms.SimData.GPS_DRIVES_NAV1 == 1) {
                fms.sendCommand("TOGGLE_GPS_DRIVES_NAV1", 0);
                fms.sendCommand("AP_NAV_SELECT_SET", 1);
            } else if (fms.SimData.AUTOPILOT_NAV_SELECTED == 1) {
                fms.sendCommand("AP_NAV_SELECT_SET", 2);
            }
            else if (fms.SimData.AUTOPILOT_NAV_SELECTED == 2) {
                fms.sendCommand("TOGGLE_GPS_DRIVES_NAV1", 0);
            }
        })

        g.button(bw, bh, "FPL").move(x + bs * 7, y).click(function (e) {
            //const fp = new FlightPlan(__PFD.WebSockerUrl, "FLIGHT PLAN", __PFD, function (numpad)
            let fpl = new FlightPlan(fms).onclose(function (numpad) {
                console.log(numpad.Result)
                // if(fpl.timer) {
                //     clearInterval(fpl.timer)
                // }
                // fpl.close()
            })
        })

        g.button(bw, bh, "XPDR").move(x + bs * 8, y).click(function (e) {
            let numpd = new NumPad("XPDR", "0000", "01234567").onclose(function (numpad) {
                if (numpad.Result == "ENTER") {
                    fms.sendCommand("XPNDR_SET", decimalToBCD(numpad.Value));
                }
                numpd.close()
            })
        })

        g.button(bw, bh, "BARO").move(x + bs * 9, y).click(function (e) {
            let numpd = new NumPad("BARO", "00.00").onclose(function (numpad) {
                if (numpad.Result == "ENTER") {
                    var mb = Math.round(HgToMilibar(numpad.Value) * 16);
                    fms.sendCommand("KOHLSMAN_SET", mb);
                }
                numpd.close()
            })
        })

        g.button(bw, bh, "MFD").move(x + bs * 10, y).click(function (e) {
            window.open(SETTINGS.mfd_url);
        })


        let font1 = { fill: 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 18, leading: "0em" }
        let font2 = { fill: 'green', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 18, leading: "0em" }
        let font3 = { fill: 'magenta', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 18, leading: "0em" }
        g.rect(w, 22).fill("#202020")

        let oatl = g.text("OAT:").font(font1).move(10, 0)
        let oatv = g.text("NA").font(font3).move(oatl.bbox().x2 + 2, 0)
        oatv.update = function (data) {
            if (oatv.pValue != data.AMBIENT_TEMPERATURE) {
                oatv.text(data.AMBIENT_TEMPERATURE.toFixed(0) + " °C")
                oatv.pValue = data.AMBIENT_TEMPERATURE
            }
        }
        fms.subscribe(oatv, 10)

        let xpdrl = g.text("XPDR:").font(font2).move(610, 0)
        let xpdrv = g.text("NA").font(font2).move(xpdrl.bbox().x2 + 2, 0)
        xpdrv.update = function (data) {
            var state = " IDNT";
            switch (data.TRANSPONDER_STATE_1) {
                case 1: state = " STDY";
                    break;
                case 2: state = " TST";
                    break;
                case 3: state = " ON";
                    break;
                case 4: state = " ALT";
                    break;
            }
            xpdrv.text(padLeft(data.TRANSPONDER_CODE_1.toString(16), 4, "0") + state);
        }
        fms.subscribe(xpdrv, 10)

        let ltm = g.text("0:00:00").font(font1).move(this.panel.width() - 80, 0)
        setInterval(function (params) {
            let t = secsToTime(todaySecs())
            ltm.text(t.h + ":" + padLeft(t.m, 2, "0") + ":" + padLeft(t.s, 2, "0"))
        }, 1000)
    }

    move(x, y) {
        this.panel.move(x, y)
        return this;
    }


}