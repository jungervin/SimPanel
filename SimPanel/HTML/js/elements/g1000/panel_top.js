class G1000PanelTop {
    constructor(fms, w, h) {

        this.panel = fms.svg.nested()
        this.panel.rect(w, h).fill("#000");

        let lazy = 5
        let font1 = { fill: 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 20, leading: "0em" }
        let font2 = { fill: 'magenta', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 20, leading: "0em" };
        let font3 = { fill: 'green', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 20, leading: "0em" };
        let y1 = 2
        let y2 = 26
        let panel_nav = this.panel.group()

        let SelectedNAVIndex = 0;

        let sel = panel_nav.icon_button(25, 40, "M10,8H6L12,2L18,8H14V16H18L12,22L6,16H10V8Z").click(function (sender) {
            if (SelectedNAVIndex == 0) {
                SelectedNAVIndex = 1
                NAVCursor.y(n2b.y)
                nswap.y(n2b.y)
                NAV1StndBy.fill("#fff")
                NAV2StndBy.fill("#000")
                //__PFD.sendCommand("NAV1_RADIO_SWAP");
            } else {
                //__PFD.sendCommand("NAV2_RADIO_SWAP");
                SelectedNAVIndex = 0
                NAVCursor.y(n1b.y)
                nswap.y(n1b.y)
                NAV1StndBy.fill("#000")
                NAV2StndBy.fill("#fff")
            }
        }).move(0, 4);

        let nav1 = panel_nav.text("NAV1").font(font1).move(sel.bbox().x2 + 10, y1)
        let nav2 = panel_nav.text("NAV2").font(font1).move(sel.bbox().x2 + 10, y2)

        let NAV1StndBy = panel_nav.text("999.99").font(font1).fill("black").move(nav1.bbox().x2 + 10, y1);
        fms.subscribe(NAV1StndBy, lazy)
        NAV1StndBy.update = function (data) {
            if (NAV1StndBy.pValue != data.NAV_STANDBY_FREQUENCY_1) {
                NAV1StndBy.text(data.NAV_STANDBY_FREQUENCY_1.toFixed(2));
                NAV1StndBy.pValue = data.NAV_STANDBY_FREQUENCY_1
            }
        }

        let n1b = NAV1StndBy.bbox()
        let NAVCursor = panel_nav.rect(n1b.w, n1b.h).fill("white").move(n1b.x, n1b.y).backward()

        let NAV2StndBy = panel_nav.text("999.99").font(font1).move(nav1.bbox().x2 + 10, y2);
        fms.subscribe(NAV2StndBy, lazy)
        NAV2StndBy.update = function (data) {
            if (NAV2StndBy.pValue != data.NAV_STANDBY_FREQUENCY_2) {
                NAV2StndBy.text(data.NAV_STANDBY_FREQUENCY_2.toFixed(2));
                NAV2StndBy.pValue = data.NAV_STANDBY_FREQUENCY_2
            }
        }

        let n2b = NAV2StndBy.bbox()

        let nswap = panel_nav.icon_button(40, 20, "M8,14V18L2,12L8,6V10H16V6L22,12L16,18V14H8Z").click(function (sender) {
            if (SelectedNAVIndex == 0) {
                fms.sendCommand("NAV1_RADIO_SWAP");
            } else {
                fms.sendCommand("NAV2_RADIO_SWAP");
            }

        }).move(NAV1StndBy.bbox().x2 + 10, 2); // .fill("cyan");

        let NAV1Act = panel_nav.text("999.99").font(font1).move(nswap.bbox().x2 + 10, y1);
        NAV1Act.update = function (data) {
            if (NAV1Act.pValue != data.NAV_ACTIVE_FREQUENCY_1) {
                NAV1Act.text(data.NAV_ACTIVE_FREQUENCY_1.toFixed(2));
                NAV1Act.pValue = data.NAV_ACTIVE_FREQUENCY_1
            }
        }
        fms.subscribe(NAV1Act, lazy)

        let NAV2Act = panel_nav.text("999.99").font(font1).move(nswap.bbox().x2 + 10, y2);
        NAV2Act.update = function (data) {
            if (NAV2Act.pValue != data.NAV_ACTIVE_FREQUENCY_2) {
                NAV2Act.text(data.NAV_ACTIVE_FREQUENCY_2.toFixed(2));
                NAV2Act.pValue = data.NAV_ACTIVE_FREQUENCY_2
            }
        }
        fms.subscribe(NAV2Act, lazy)

        let NAV1Ident = panel_nav.text("---").font(font1).move(NAV1Act.bbox().x2 + 10, y1);
        NAV1Ident.update = function (data) {
            if (NAV1Ident.pValue != data.NAV_IDENT_1) {
                NAV1Ident.text(data.NAV_IDENT_1);
                NAV1Ident.pValue = data.NAV_IDENT_1
            }
        }
        fms.subscribe(NAV1Ident, lazy)

        let NAV2Ident = panel_nav.text("---").font(font1).move(NAV1Act.bbox().x2 + 10, y2);
        NAV2Ident.update = function (data) {
            if (NAV2Ident.pValue != data.NAV_IDENT_2) {
                NAV2Ident.text(data.NAV_IDENT_2);
                NAV2Ident.pValue = data.NAV_IDENT_2
            }
        }
        fms.subscribe(NAV2Ident, lazy)


        //====================================================================================
        // COM 
        //====================================================================================
        var panel_com = this.panel.group()
        let SelectedCOMIndex = 0;

        let COM1Act = panel_com.text("999.999").font(font1).move(0, y1);
        COM1Act.update = function (data) {
            if (COM1Act.pValue != data.COM_ACTIVE_FREQUENCY_1) {
                COM1Act.text(data.COM_ACTIVE_FREQUENCY_1.toFixed(3));
                COM1Act.pValue = data.COM_ACTIVE_FREQUENCY_1
            }
        }
        fms.subscribe(COM1Act, lazy)


        let COM2Act = panel_com.text("999.999").font(font1).move(0, y2);
        COM2Act.update = function (data) {
            if (COM2Act.pValue != data.COM_ACTIVE_FREQUENCY_2) {
                COM2Act.text(data.COM_ACTIVE_FREQUENCY_2.toFixed(3));
                COM2Act.pValue = data.COM_ACTIVE_FREQUENCY_2
            }
        }
        fms.subscribe(COM2Act, lazy)

        let cswap = panel_com.icon_button(40, 20, "M8,14V18L2,12L8,6V10H16V6L22,12L16,18V14H8Z").click(function (sender) {
            if (SelectedCOMIndex == 0) {
                fms.sendCommand("COM1_RADIO_SWAP");
            } else {
                fms.sendCommand("COM2_RADIO_SWAP");
            }
        }).move(COM1Act.bbox().x2 + 10, 2); // .fill("cyan");


        let COM1StndBy = panel_com.text("999.999").font(font1).fill("black").move(cswap.bbox().x2 + 10, y1)
        COM1StndBy.update = function (data) {
            if (COM1StndBy.pValue != data.COM_STANDBY_FREQUENCY_1) {
                COM1StndBy.text(data.COM_STANDBY_FREQUENCY_1.toFixed(3));
                COM1StndBy.pValue = data.COM_STANDBY_FREQUENCY_1
            }
        }
        fms.subscribe(COM1StndBy, lazy)

        let c1b = COM1StndBy.bbox()
        let COMCursor = panel_com.rect(c1b.w, c1b.h).fill("white").move(c1b.x, y1).backward()

        let COM2StndBy = panel_com.text("999.999").font(font1).move(cswap.bbox().x2 + 10, y2);
        COM2StndBy.update = function (data) {
            if (COM2StndBy.pValue != data.COM_STANDBY_FREQUENCY_2) {
                COM2StndBy.text(data.COM_STANDBY_FREQUENCY_2.toFixed(3));
                COM2StndBy.pValue = data.COM_STANDBY_FREQUENCY_2
            }
        }
        fms.subscribe(COM2StndBy, lazy)

        let c2b = COM2StndBy.bbox()

        let com1 = panel_com.text("COM1").font(font1).move(COM2StndBy.bbox().x2 + 10, y1)
        let com2 = panel_com.text("COM2").font(font1).move(COM2StndBy.bbox().x2 + 10, y2)

        let csel = panel_com.icon_button(25, 40, "M10,8H6L12,2L18,8H14V16H18L12,22L6,16H10V8Z").click(function (sender) {
            if (SelectedCOMIndex == 0) {
                SelectedCOMIndex = 1
                COMCursor.y(c2b.y)
                cswap.y(c2b.y)
                COM1StndBy.fill("#fff")
                COM2StndBy.fill("#000")
            } else {
                SelectedCOMIndex = 0
                COMCursor.y(c1b.y)
                cswap.y(c1b.y)
                COM1StndBy.fill("#000")
                COM2StndBy.fill("#fff")
            }
        }).move(com1.bbox().x2 + 5, 4);



        //====================================================================================
        // INFOS 
        //====================================================================================
        // LHBP 110.5
        // NAV_AVAILABLE_1 bool
        // NAV_SIGNAL_1 bool
        // NAV_IDENT_1 FER
        // NAV_NAME_1 => ILS RW13R
        // NAV_CDI_1


        var panel_inf = this.panel.group()

        let fromto = panel_inf.text("_____ → _____").font(font2).move(0, y1)
        fromto.update = function (data) {
            if (fromto.pid != data.GPS_WP_PREV_ID || fromto.nid != data.GPS_WP_NEXT_ID) {
                fromto.text(data.GPS_WP_PREV_ID + " → " + data.GPS_WP_NEXT_ID)
                fromto.pid = data.GPS_WP_PREV_ID
                fromto.nid = data.GPS_WP_NEXT_ID
            }
        }
        fms.subscribe(fromto, lazy)

        let d = panel_inf.text("DIS").font(font1).move(0, y2)
        let dist = panel_inf.text("999.99NM").font(font2).move(d.bbox().x2 + 20, y2)

        let e = panel_inf.text("ETE").font(font1).move(fromto.bbox().x2 + 20, y2)
        let ete = panel_inf.text("12:00:00").font(font2).move(e.bbox().x2 + 20, y2)

        dist.update = function (data) {
            if (dist.dist != data.GPS_WP_DISTANCE) {
                dist.text(m2nm(data.GPS_WP_DISTANCE).toFixed(2) + "NM")
                dist.dist = data.GPS_WP_DISTANCE
            }

            if (data.GPS_GROUND_SPEED > 1) {
                let dt = m2nm(data.GPS_WP_DISTANCE) / data.GPS_GROUND_SPEED * 3600
                if (ete.ete != dt) {
                    let t = secsToTime(dt)
                    ete.text(t.h + ":" + padLeft(t.m, 2, "0") + ":" + padLeft(t.s, 2, "0"))
                    ete.ete = dt
                }
            } else {
                ete.text("N/A")
            }

        }
        fms.subscribe(dist, lazy)

        let b = panel_inf.text("BRG").font(font1).move(fromto.bbox().x2 + 20, y1)
        let brg = panel_inf.text("___°").font(font2).move(b.bbox().x2 + 20, y1)
        brg.update = function (data) {
            if (brg.brg != data.GPS_WP_BEARING) {
                brg.text(Math.round(data.GPS_WP_BEARING) + "°")
                brg.brg = data.GPS_WP_BEARING
            }
        }
        fms.subscribe(brg, lazy)

        // ete.update = function (data) {
        //     if (ete.ete != data.GPS_WP_ETE) {
        //         let t = secsToTime(data.GPS_WP_ETE)
        //         ete.text(t.h + ":" + padLeft(t.m, 2, "0") + ":" + padLeft(t.s, 2, "0"))
        //         ete.ete = data.GPS_WP_ETE
        //     }
        // }
        // pfd.subscribe(ete, lazy)

        panel_nav.x(20)
        panel_inf.move(panel_nav.bbox().x2 + 30, y1)
        panel_com.x(730)


        //===============================================================================================================
        // MORE INFOS
        //===============================================================================================================

        let g = this.panel.group()
        font1 = { fill: 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 18, leading: "0em" }
        font2 = { fill: 'green', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 18, leading: "0em" }
        font3 = { fill: 'magenta', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 18, leading: "0em" }
        g.rect(w, 22).fill("#202020")



        // GPS_IS_ACTIVE_WAY_POINT
        // GPS_IS_ACTIVE_FLIGHT_PLAN
        // GPS_IS_ARRIVED
        // GPS_IS_DIRECTTO_FLIGHTPLAN
        // GPS_WP_PREV_VALID

        let destl = g.text("ROUTE:").font(font1).move(20, 0)
        let destv = g.text("N/A").font(font3).move(destl.bbox().x2 + 2, 0)
        destv.update = function (data) {
            if (fms.FlightPlan.length > 0) {
                let f = fms.FlightPlan[0].icao
                let t = f + " → " + fms.FlightPlan[fms.FlightPlan.length - 1].icao
                if (destv.pValue != t) {
                    destv.text(t)
                    destv.pValue = t
                }

            } else {
                destv.text("N/A")
            }

        }
        fms.subscribe(destv, 5)

        let x = 235
        let etel = g.text("ETE:").font(font1).move(x, 0)
        let etev = g.text("00:00:00").font(font3).move(etel.bbox().x2 + 2, 0)
        let etal = g.text("ETA:").font(font1).move(x + 137, 0)
        let etav = g.text("00:00:00").font(font3).move(etal.bbox().x2 + 2, 0)

        let rangel = g.text("RNG:").font(font1).move(x + 305, 0)
        let rangev = g.text("__._NM").font(font3).move(rangel.bbox().x2 + 2, 0)

        // let ltm = g.text("0:00:00").font(font1).move(g.width() - 80, 0)
        setInterval(function (p) {
            let t = secsToTime(todaySecs())
            // ltm.text(t.h + ":" + padLeft(t.m, 2, "0") + ":" + padLeft(t.s, 2, "0"))
            if (fms.SimData.GPS_IS_ACTIVE_WAY_POINT || 0) {
                let t = secsToTime(fms.SimData.GPS_ETE)
                etev.text(t.h + ":" + padLeft(t.m, 2, "0") + ":" + padLeft(t.s, 2, "0"))
                let tm = todaySecs()
                let t2 = secsToTime((tm + fms.FlightPlan.eta.eta))
                etav.text(t2.h + ":" + padLeft(t2.m, 2, "0") + ":" + padLeft(t2.s, 2, "0"))


            }
            else {
                etev.text("N/A")
                etav.text("N/A")
            }

            let r = 0
            if (fms.SimData.RECIP_ENG_FUEL_FLOW_1) {
                r = (fms.SimData.FUEL_TANK_LEFT_MAIN_QUANTITY + fms.SimData.FUEL_TANK_RIGHT_MAIN_QUANTITY) / fms.SimData.RECIP_ENG_FUEL_FLOW_1 / 0.17 * fms.SimData.GPS_GROUND_SPEED
            }
            rangev.text(r.toFixed(1) + "NM")

        }, 1000)

        g.move(0, 50)
    }

    move(x, y) {
        this.panel.move(x, y)
    }
}