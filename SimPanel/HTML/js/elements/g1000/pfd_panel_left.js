class G1000PFDPanelLeft {
    constructor(fms, w, h) {

        let font1 = { 'fill': 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 36, leading: '1em' };
        let font2 = { 'fill': 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 24, leading: '1em' };
        const PFD = fms;
        let svg = fms.svg



        let bw = 60;
        let bh = 28;

        svg.rect(w, h);


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

        //=====================================================================
        // CRS ROTRAY
        //=====================================================================
        let crs = lw.rotary().center(65, 200).on("wheel", function (e) {
            e.currentTarget.instance.rotate(e.deltaY * 10);
            // if (PFD.Compass.NAVMode == 1) {
            //     if (e.deltaY < 0)
            //         PFD.sendCommand("VOR1_OBI_INC");
            //     else PFD.sendCommand("VOR1_OBI_DEC");
            // }
            // else if (PFD.Compass.NAVMode == 2) {
            //     if (e.deltaY < 0)
            //         PFD.sendCommand("VOR2_OBI_INC");
            //     else PFD.sendCommand("VOR2_OBI_DEC");
            // }
        }, { passive: false });
        lw.text("CRS").font(font2).fill("white").center(crs.bbox().cx, 250)

        //=====================================================================
        // HDG ROTARY
        //=====================================================================
        let hdg = lw.rotary().center(65, 310).on("wheel", function (e) {
            e.currentTarget.instance.rotate(e.deltaY * -10);
            if (e.deltaY < 0)
                fms.sendCommand("HEADING_BUG_INC");
            else fms.sendCommand("HEADING_BUG_DEC");

        }, { passive: true });
        lw.text("HDG").font(font2).fill("white").center(hdg.bbox().cx, 360)

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

        //=====================================================================
        // ALT ROTARY
        //=====================================================================
        let roALT = lw.rotary().center(65, 670).on("wheel", function (e) {
            e.currentTarget.instance.rotate(e.deltaY * -10);
            if (e.deltaY < 0)
                fms.sendCommand("AP_ALT_VAR_INC");
            else fms.sendCommand("AP_ALT_VAR_DEC");
        }, { passive: true });
        roALT.update = function (data) {
            if (this.prevALTValue < data.AUTOPILOT_ALTITUDE_LOCK_VAR) {
                this.setAngle(this.Angle + 10);
            } else if (this.prevALTValue > data.AUTOPILOT_ALTITUDE_LOCK_VAR) {
                this.setAngle(this.Angle - 10);
            }
        }
        fms.subscribe(roALT, lazy)
        lw.text("ALT").font(font2).fill("white").center(hdg.bbox().cx, 720)

        lw.move(10, 15)

    }


}