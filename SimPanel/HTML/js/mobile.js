
class C172Mobile extends FMS {
    constructor(width, height) {
        super();
        const fms = this;

        var opt = {};

        this.SimPanel = document.getElementById("simpanel");

        this.SimPanel.appendChild(this.createLayer(0, 0, width, height, "bottom_layer"));
        this.svg = SVG().addTo("#bottom_layer").size(width, height);

        let svg = this.svg;

        let cx = width / 2;

        let mapl = this.SimPanel.appendChild(this.createLayer(0, 0, width, width * 0.8, "map"))
        let MAP = new Map(fms, "map")


        this.RequestCounter = 0;

        let font1 = { 'fill': 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 36, leading: '1em' };
        let font2 = { 'fill': 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 24, leading: '1em' };



        let bw = 60;
        let bh = 28;

        svg.rect(width, height);



        let lw = svg.group();
        let lazy = 5;



        //=====================================================================
        // BEACON
        //=====================================================================
        let btnBeacon = lw.indicator_button(bw, bh, "BEACON").click(function (e) {
            fms.sendCommand("TOGGLE_BEACON_LIGHTS")
        })
        btnBeacon.update = function (data) {
            if (this.LIGHT_BEACON != data.LIGHT_BEACON) {
                this.setON(data.LIGHT_BEACON);
                this.LIGHT_BEACON = data.LIGHT_BEACON;
            }
        }
        fms.subscribe(btnBeacon, lazy);
        this.btnBeacon = btnBeacon

        //=====================================================================
        // LAND
        //=====================================================================
        let btnLand = lw.indicator_button(bw, bh, "LAND").move(70, 0).click(function (e) {
            fms.sendCommand("LANDING_LIGHTS_TOGGLE")
        })
        btnLand.update = function (data) {
            if (this.LIGHT_LANDING != data.LIGHT_LANDING) {
                this.setON(data.LIGHT_LANDING);
                this.LIGHT_LANDING = data.LIGHT_LANDING
            }
        }
        fms.subscribe(btnLand, lazy)


        //=====================================================================
        // TAXI
        //=====================================================================
        let btnTaxi = lw.indicator_button(bw, bh, "TAXI").move(0, 40).click(function (e) {
            fms.sendCommand("TOGGLE_TAXI_LIGHTS")
        })
        btnTaxi.update = function (data) {
            if (this.LIGHT_TAXI != data.LIGHT_TAXI) {
                this.setON(data.LIGHT_TAXI);
                this.LIGHT_TAXI = data.LIGHT_TAXI
            }
        }
        fms.subscribe(btnTaxi, lazy)

        //=====================================================================
        // NAV
        //=====================================================================
        let btnNAV = lw.indicator_button(bw, bh, "NAV").move(70, 40).click(function (e) {
            fms.sendCommand("TOGGLE_NAV_LIGHTS")
        })
        btnNAV.update = function (data) {
            if (this.LIGHT_NAV != data.LIGHT_NAV) {
                this.setON(data.LIGHT_NAV);
                this.LIGHT_NAV = data.LIGHT_NAV
            }
        }
        fms.subscribe(btnNAV, lazy)

        //=====================================================================
        // STROBE
        //=====================================================================
        let btnSTROBE = lw.indicator_button(bw, bh, "STROBE").move(0, 80).click(function (e) {
            fms.sendCommand("STROBES_TOGGLE");
        })
        btnSTROBE.update = function (data) {
            if (this.LIGHT_STROBE != data.LIGHT_STROBE) {
                this.setON(data.LIGHT_STROBE);
                this.LIGHT_STROBE = data.LIGHT_STROBE
            }
        }
        fms.subscribe(btnSTROBE, lazy);

        //=====================================================================
        // PITOT HEAT
        //=====================================================================
        let btnPHEAT = lw.indicator_button(bw, bh, "P.HEAT").move(70, 80).click(function (e) {
            fms.sendCommand("PITOT_HEAT_TOGGLE");
        })
        btnPHEAT.update = function (data) {
            if (this.PITOT_HEAT != data.PITOT_HEAT) {
                this.setON(data.PITOT_HEAT);
                this.PITOT_HEAT = data.PITOT_HEAT
            }
        }
        fms.subscribe(btnPHEAT, lazy);

        //=====================================================================
        // PARKING BREAK
        //=====================================================================
        let btnPBREAK = lw.indicator_button(bw * 2 + 10, bh, "PARK.BREAK", { color_on: "red" }).move(0, 120).click(function (e) {
            fms.sendCommand("PARKING_BRAKES")
        })
        btnPBREAK.update = function (data) {
            if (this.BRAKE_PARKING_INDICATOR != data.BRAKE_PARKING_INDICATOR) {
                this.setON(data.BRAKE_PARKING_INDICATOR);
                this.BRAKE_PARKING_INDICATOR = data.BRAKE_PARKING_INDICATOR
            }
        }
        fms.subscribe(btnPBREAK, lazy);

        lw.move(25, width + 10)


        lw = svg.group();


        //=====================================================================
        // AP
        //=====================================================================
        let btnAP = lw.indicator_button(bw, bh, "AP").move(0, 390).click(function (e) {
            fms.sendCommand("AP_MASTER");
        });
        btnAP.update = function (data) {
            if (this.AUTOPILOT_MASTER != data.AUTOPILOT_MASTER) {
                this.setON(data.AUTOPILOT_MASTER);
                this.AUTOPILOT_MASTER = data.AUTOPILOT_MASTER
            }
        }
        fms.subscribe(btnAP, lazy);

        //=====================================================================
        // FD
        //=====================================================================
        let btnFD = lw.indicator_button(bw, bh, "FD").move(70, 390).click(function (e) {
            fms.sendCommand("TOGGLE_FLIGHT_DIRECTOR");
        });
        btnFD.update = function (data) {
            if (this.AUTOPILOT_FLIGHT_DIRECTOR_ACTIVE != data.AUTOPILOT_FLIGHT_DIRECTOR_ACTIVE) {
                this.setON(data.AUTOPILOT_FLIGHT_DIRECTOR_ACTIVE);
                this.AUTOPILOT_FLIGHT_DIRECTOR_ACTIVE = data.AUTOPILOT_FLIGHT_DIRECTOR_ACTIVE
            }
        }
        fms.subscribe(btnFD, lazy);

        //=====================================================================
        // HDG
        //=====================================================================
        let btnHDG = lw.indicator_button(bw, bh, "HDG").move(0, 430).click(function (e) {
            var h = fms.SimData.AUTOPILOT_HEADING_LOCK_DIR;
            fms.sendCommand("AP_HDG_HOLD");
            setTimeout(function () {
                fms.sendCommand("HEADING_BUG_SET", h);
            }, 10);
        })
        btnHDG.update = function (data) {
            if (this.AUTOPILOT_HEADING_LOCK != data.AUTOPILOT_HEADING_LOCK) {
                this.setON(data.AUTOPILOT_HEADING_LOCK);
                this.AUTOPILOT_HEADING_LOCK = data.AUTOPILOT_HEADING_LOCK
            }
        }
        fms.subscribe(btnHDG, lazy);

        //=====================================================================
        // ALT
        //=====================================================================
        let btnALT = lw.indicator_button(bw, bh, "ALT").move(70, 430).click(function (e) {
            fms.sendCommand("AP_ALT_HOLD");
        })
        btnALT.update = function (data) {
            if (this.AUTOPILOT_ALTITUDE_LOCK != data.AUTOPILOT_ALTITUDE_LOCK) {
                this.setON(data.AUTOPILOT_ALTITUDE_LOCK);
                this.AUTOPILOT_ALTITUDE_LOCK = data.AUTOPILOT_ALTITUDE_LOCK
            }
        }
        fms.subscribe(btnALT, lazy);


        //=====================================================================
        // NAV
        //=====================================================================
        let apNAV = lw.indicator_button(bw, bh, "NAV").move(0, 470).click(function (e) {
            fms.sendCommand("AP_NAV1_HOLD");
        })
        apNAV.update = function (data) {
            if (this.AUTOPILOT_NAV1_LOCK != data.AUTOPILOT_NAV1_LOCK) {
                this.setON(data.AUTOPILOT_NAV1_LOCK);
                this.AUTOPILOT_NAV1_LOCK = data.AUTOPILOT_NAV1_LOCK
            }
        }
        fms.subscribe(apNAV, lazy);

        //=====================================================================
        // VNV
        //=====================================================================
        lw.indicator_button(bw, bh, "VNV").move(70, 470).click(function (e) {
            console.log("INOP");
        });

        //=====================================================================
        // APR
        //=====================================================================
        let apAPR = lw.indicator_button(bw, bh, "APR").move(0, 510).click(function (e) {
            fms.sendCommand("AP_APR_HOLD");
        })
        apAPR.update = function (data) {
            if (this.AUTOPILOT_APPROACH_HOLD != data.AUTOPILOT_APPROACH_HOLD) {
                this.setON(data.AUTOPILOT_APPROACH_HOLD);
                this.AUTOPILOT_APPROACH_HOLD = data.AUTOPILOT_APPROACH_HOLD
            }
        }
        fms.subscribe(apAPR, lazy);


        //=====================================================================
        // BC
        //=====================================================================
        let apBC = lw.indicator_button(bw, bh, "BC").move(70, 510).click(function (e) {
            fms.sendCommand("AP_BC_HOLD");
        })
        apBC.update = function (data) {
            if (this.AUTOPILOT_BACKCOURSE_HOLD != data.AUTOPILOT_BACKCOURSE_HOLD) {
                this.setON(data.AUTOPILOT_BACKCOURSE_HOLD);
                this.AUTOPILOT_BACKCOURSE_HOLD = data.AUTOPILOT_BACKCOURSE_HOLD
            }
        }
        fms.subscribe(apBC, lazy);

        //=====================================================================
        // VS
        //=====================================================================
        let apVS = lw.indicator_button(bw, bh, "VS").move(0, 550).click(function (e) {
            fms.sendCommand("AP_VS_HOLD");
        })
        apVS.update = function (data) {
            if (this.AUTOPILOT_VERTICAL_HOLD != data.AUTOPILOT_VERTICAL_HOLD) {
                this.setON(data.AUTOPILOT_VERTICAL_HOLD);
                this.AUTOPILOT_VERTICAL_HOLD = data.AUTOPILOT_VERTICAL_HOLD
            }
        }
        fms.subscribe(apVS, lazy);

        //=====================================================================
        // UP
        //=====================================================================
        lw.button(bw, bh, "UP").move(70, 550).click(function (e) {
            if (fms.SimData.AUTOPILOT_FLIGHT_LEVEL_CHANGE == 1) {
                fms.sendCommand("AP_SPD_VAR_INC");
            }
            else if (fms.SimData.AUTOPILOT_VERTICAL_HOLD == 1) {
                fms.sendCommand("AP_VS_VAR_INC");
            }
        });

        //=====================================================================
        // FLC
        //=====================================================================
        let apFLC = lw.indicator_button(bw, bh, "FLC").move(0, 590).click(function (e) {
            var s = fms.SimData.AIRSPEED_INDICATED;
            fms.sendCommand("FLIGHT_LEVEL_CHANGE");
            fms.sendCommand("AP_SPD_VAR_SET", s);
        })
        apFLC.update = function (data) {
            if (this.AUTOPILOT_FLIGHT_LEVEL_CHANGE != data.AUTOPILOT_FLIGHT_LEVEL_CHANGE) {
                this.setON(data.AUTOPILOT_FLIGHT_LEVEL_CHANGE);
                this.AUTOPILOT_FLIGHT_LEVEL_CHANGE = data.AUTOPILOT_FLIGHT_LEVEL_CHANGE
            }
        }
        fms.subscribe(apFLC, lazy);

        //=====================================================================
        // DN
        //=====================================================================
        lw.button(bw, bh, "DN").move(70, 590).click(function (e) {
            if (fms.SimData.AUTOPILOT_FLIGHT_LEVEL_CHANGE == 1) {
                fms.sendCommand("AP_SPD_VAR_DEC");
            }
            else if (fms.SimData.AUTOPILOT_VERTICAL_HOLD == 1) {
                fms.sendCommand("AP_VS_VAR_DEC");
            }
        });

        lw.move(200, width + 10)


        font1 = { 'fill': 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 20, leading: '1em', "letter-spacing": "0.11em" }
        var alt = this.svg.group();

        alt.text(function (add) {
            add.tspan('S').newLine()
            add.tspan('P').newLine()
            add.tspan('D').newLine()
        }).font(font1).move(0, 0 * 92)
        var spd = alt.lcdDisplay(130, 60, "0", { fill: "cyan", pat: "000", }).move(20, 0 * 92);
        spd.update = function (data) {
            spd.setText(data.AIRSPEED_INDICATED.toFixed(0))
        }
        fms.subscribe(spd, 1)


        alt.text(function (add) {
            add.tspan('A').newLine()
            add.tspan('L').newLine()
            add.tspan('T').newLine()
        }).font(font1).move(180, 0 * 92)
        var lcd_alt = alt.lcdDisplay(200, 60, "0", { fill: "cyan", pat: "00000" }).move(200, 0 * 92);
        lcd_alt.update = function (data) {
            lcd_alt.setText(data.INDICATED_ALTITUDE.toFixed(0))
        }
        fms.subscribe(lcd_alt,1)

        alt.scale(0.8).translate(-20, width * .8 + 10)



        MAP.connect();
        this.connect()


    }

}