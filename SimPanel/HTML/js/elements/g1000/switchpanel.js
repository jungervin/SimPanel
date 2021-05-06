

class SwitchPanel extends PopupPanel {
    constructor(fms, x, y, w, h) {
        super("switchpanel", x, y, w, h, false)

        let lazy = 5
        var g = this.svg.group();
        this.panel = g;
        g.rect(w, h)

        let mag_pos = ["MAGNETO1_OFF", "MAGNETO1_RIGHT", "MAGNETO1_LEFT", "MAGNETO1_BOTH", "MAGNETO1_START" ]
        let magneto = g.magneto(180, 130, null, function (sender, dir) {

            //let p = magneto.Pos
            if (dir == 1) {
                //p++

                fms.sendCommand("MAGNETO_INCR");
            } else {
                fms.sendCommand("MAGNETO_DECR");
               // p--
            }

            // p = Math.max(0, Math.min(4, p))
            // console.log(mag_pos[p])
            // fms.sendCommand(mag_pos[p]);

        });
        magneto.update = function (data) {
            if (data) {

                if (data.GENERAL_ENG_STARTER_1) {
                    magneto.setPos(4);
                }
                else if (data.RECIP_ENG_LEFT_MAGNETO_1 == 1 && data.RECIP_ENG_RIGHT_MAGNETO_1 == 1) {
                    magneto.setPos(3);
                }
                else if (data.RECIP_ENG_LEFT_MAGNETO_1 == 1 && data.RECIP_ENG_RIGHT_MAGNETO_1 == 0) {
                    magneto.setPos(2);
                }
                else if (data.RECIP_ENG_LEFT_MAGNETO_1 == 0 && data.RECIP_ENG_RIGHT_MAGNETO_1 == 1) {
                    magneto.setPos(1);
                }
                else {
                    magneto.setPos(0);
                }
            } else {
                console.log("magneto.update: EMPTY")
            }

        }
        fms.subscribe(magneto, lazy)




        // BATTERY
        let alternator = g.image_button(this, 'ALT', './images/switch_red_on.png', './images/switch_red_off.png', function (sender) {
            fms.sendCommand("TOGGLE_MASTER_ALTERNATOR");
        }).move(220, 20);
        alternator.update = function (data) {

            if (alternator.pValue != data.GENERAL_ENG_MASTER_ALTERNATOR_1) {
                alternator.setIsON(data.GENERAL_ENG_MASTER_ALTERNATOR_1);
                alternator.pValue = data.GENERAL_ENG_MASTER_ALTERNATOR_1
            }
        }
        fms.subscribe(alternator, lazy)


        let battery = g.image_button(this, 'BAT', './images/switch_red_on.png', './images/switch_red_off.png', function (sender) {
            fms.sendCommand("TOGGLE_MASTER_BATTERY");
        }).move(270, 20);
        battery.update = function (data) {
            if (battery.pValue != data.ELECTRICAL_MASTER_BATTERY) {
                battery.setIsON(data.ELECTRICAL_MASTER_BATTERY);
                battery.pValue = data.ELECTRICAL_MASTER_BATTERY
            }
        }
        fms.subscribe(battery, lazy)


        var opt = {};
        var font1 = opt.font || { fill: "white", 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 16, leading: '0em' }


        g.text("ALT").font(font1).center(245, 145);
        g.text("BAT").font(font1).center(295, 145);

        // AVIONICS
        let avbus1 = g.image_button(this, 'BUS1', './images/switch_white_on.png', './images/switch_white_off.png', function (sender) {
            if (fms.SimData.AVIONICS_MASTER_SWITCH == 1) {
                fms.sendCommand("AVIONICS_MASTER_SET", 0);
            } else {
                fms.sendCommand("AVIONICS_MASTER_SET", 1);
            }
        }).move(360, 20);
        let avbus2 = g.image_button(this, 'BUS2', './images/switch_white_on.png', './images/switch_white_off.png', function (sender) {
            if (fms.SimData.AVIONICS_MASTER_SWITCH == 1) {
                fms.sendCommand("AVIONICS_MASTER_SET", 0);
            } else {
                fms.sendCommand("AVIONICS_MASTER_SET", 1);
            }
        }).move(410, 20);

        avbus1.update = function (data) {
            if (avbus1.pValue != data.AVIONICS_MASTER_SWITCH) {
                avbus1.setIsON(data.AVIONICS_MASTER_SWITCH);
                avbus2.setIsON(data.AVIONICS_MASTER_SWITCH);
                avbus1.pValue = data.AVIONICS_MASTER_SWITCH
            }
        }

        // avbus2.update = function (data) {
        //     if (avbus2.pValue != data.AVIONICS_MASTER_SWITCH) {
        //         avbus1.setIsON(data.AVIONICS_MASTER_SWITCH);
        //         avbus2.setIsON(data.AVIONICS_MASTER_SWITCH);
        //         avbus2.pValue = data.AVIONICS_MASTER_SWITCH
        //     }
        // }
        fms.subscribe(avbus1, lazy)
//        fms.subscribe(avbus2, lazy)

        g.text("BUS1").font(font1).center(385, 145);
        g.text("BUS2").font(font1).center(435, 145);

        var switches = g.group();
        var w = 60;
        var h = 32;

        let btnfuel = switches.indicator_button(w, h, "FUEL").move(0, 0).click(function (e) {
            fms.sendCommand("TOGGLE_FUEL_VALVE_ALL");
        });
        btnfuel.update = function (data) {
            if (btnfuel.pValue != data.GENERAL_ENG_FUEL_VALVE_1) {
                btnfuel.setON(data.GENERAL_ENG_FUEL_VALVE_1);
                btnfuel.pValue = data.GENERAL_ENG_FUEL_VALVE_1
            }
        }
        fms.subscribe(btnfuel, lazy)

        let btnpump = switches.indicator_button(w, h, "F.PMP").move(w + 10, 0).click(function (e) {
            fms.sendCommand("FUEL_PUMP");
        });
        btnpump.update = function (data) {
            if (btnpump.pValue != data.GENERAL_ENG_FUEL_PUMP_SWITCH_1) {
                btnpump.setON(data.GENERAL_ENG_FUEL_PUMP_SWITCH_1);
                btnpump.pValue = data.GENERAL_ENG_FUEL_PUMP_SWITCH_1
            }
        }
        fms.subscribe(btnpump, lazy)

        switches.move(500, 20);

        this.Hidden = true;

        var vis = getCookie("switchpanel_visibility");
        if (vis == "true") {
            this.visibility(true);

        } else {
            this.visibility(false);
        }

        //==============================================================================
        // TRIM INDICATOR
        //==============================================================================
        let ti = g.trim_indicator(80, 100).move(g.width() - 100, 35)
        ti.update = function (data) {
            if (this.ELEVATOR_TRIM_POSITION != Math.round(data.ELEVATOR_TRIM_POSITION)) {
                this.setPos(data.ELEVATOR_TRIM_POSITION);
                this.ELEVATOR_TRIM_POSITION = Math.round(data.ELEVATOR_TRIM_POSITION)
            }
        }
        fms.subscribe(ti, 5)

        //==============================================================================
        // FLAPS CONTROL
        //==============================================================================
        let fc = g.flaps_control(130, 100).move(g.width() - 240, 35)
        fc.update = function (data) {
            if (this.TRAILING_EDGE_FLAPS_LEFT_PERCENT != data.TRAILING_EDGE_FLAPS_LEFT_PERCENT) {
                this.setPos(data.TRAILING_EDGE_FLAPS_LEFT_PERCENT);
                this.TRAILING_EDGE_FLAPS_LEFT_PERCENT = data.TRAILING_EDGE_FLAPS_LEFT_PERCENT
            }
        }
        fms.subscribe(fc, 5)
        fc.btnUP.click(function (e) {
            fms.sendCommand("FLAPS_DECR", 0);
        })
        fc.btnDN.click(function (e) {
            fms.sendCommand("FLAPS_INCR", 0);
        })

        //this.update(fms.SimData)
    }


    visibility(visible) {
        //var d = document.getElementById("switchpaneldiv");
        if (visible) {
            this.panel.show()
            this.Hidden = false;
            setCookie("switchpanel_visibility", true);
        } else {
            this.panel.hide()
            this.Hidden = true;
            setCookie("switchpanel_visibility", false);
        }
    }

    // update(data) {

    //     if (!this.Hidden) {

    //         if (this.GENERAL_ENG_FUEL_VALVE_1 != data.GENERAL_ENG_FUEL_VALVE_1) {
    //             this.FuelCutOffButton.setIsON(data.GENERAL_ENG_FUEL_VALVE_1);
    //             this.GENERAL_ENG_FUEL_VALVE_1 = data.GENERAL_ENG_FUEL_VALVE_1
    //         }
    //         if (this.GENERAL_ENG_FUEL_PUMP_SWITCH_1 != data.GENERAL_ENG_FUEL_PUMP_SWITCH_1) {
    //             this.FUELPMPButton.setIsON(data.GENERAL_ENG_FUEL_PUMP_SWITCH_1);
    //             this.GENERAL_ENG_FUEL_PUMP_SWITCH_1 = data.GENERAL_ENG_FUEL_PUMP_SWITCH_1
    //         }


    //         if (this.ELECTRICAL_MASTER_BATTERY != data.ELECTRICAL_MASTER_BATTERY) {
    //             this.Battery.setIsON(data.ELECTRICAL_MASTER_BATTERY);
    //             this.ELECTRICAL_MASTER_BATTERY = data.ELECTRICAL_MASTER_BATTERY;
    //         }

    //         if (this.GENERAL_ENG_MASTER_ALTERNATOR_1 != data.GENERAL_ENG_MASTER_ALTERNATOR_1) {
    //             this.Alternator.setIsON(data.GENERAL_ENG_MASTER_ALTERNATOR_1);
    //             this.GENERAL_ENG_MASTER_ALTERNATOR_1 = data.GENERAL_ENG_MASTER_ALTERNATOR_1
    //         }

    //         if (this.AVIONICS_MASTER_SWITCH != data.AVIONICS_MASTER_SWITCH) {
    //             this.AvionicsBus1.setIsON(data.AVIONICS_MASTER_SWITCH);
    //             this.AvionicsBus2.setIsON(data.AVIONICS_MASTER_SWITCH);
    //             this.AVIONICS_MASTER_SWITCH = data.AVIONICS_MASTER_SWITCH
    //         }

    //         // if (this.AVIONICS_MASTER_SWITCH != data.AVIONICS_MASTER_SWITCH) {
    //         //     this.AvionicsBus2.setIsON(data.AVIONICS_MASTER_SWITCH);
    //         //     this.AVIONICS_MASTER_SWITCH != data.AVIONICS_MASTER_SWITCH
    //         // }

    //         if (data.GENERAL_ENG_STARTER_1) {
    //             this.Magneto.setPos(4);
    //         }
    //         else if (data.RECIP_ENG_LEFT_MAGNETO_1 && data.RECIP_ENG_RIGHT_MAGNETO_1) {
    //             this.Magneto.setPos(3);
    //         }
    //         else if (data.RECIP_ENG_LEFT_MAGNETO_1) {
    //             this.Magneto.setPos(2);
    //         }
    //         else if (data.RECIP_ENG_RIGHT_MAGNETO_1) {
    //             this.Magneto.setPos(1);
    //         }
    //         else {
    //             this.Magneto.setPos(0);
    //         }
    //     }
    // }

    move(x, y) {
        this.panel.move(x, y)
        return this;
    }
}