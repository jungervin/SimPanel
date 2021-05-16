class FMS {
    constructor() {
        this.SimData = {};
        this.ws = null;
        this.Controls = new Array();
        this.Devices = new Array();
        this.FlightPlan = {}
        this.FlightPlanData = null
        this._navmode = 3;
        this.Defaults = {
            GPS_MAGVAR: 0,
            GPS_COURSE_TO_STEER: 0,
            GPS_GROUND_TRUE_HEADING: 0,
            GPS_GROUND_MAGNETIC_TRACK: 0,
            GPS_GROUND_TRUE_TRACK: 0,
            GPS_WP_BEARING: 0,
            GPS_WP_TRUE_REQ_HDG: 0,
            NAV_AVAILABLE_1: 0,
            NAV_AVAILABLE_2: 0,
            NAV_SIGNAL_1: 0,
            NAV_SIGNAL_2: 0,
            NAV_NAME_1: 0,
            NAV_NAME_2: 0,
            NAV_CDI_1: 0,
            NAV_CDI_2: 0,
            TITLE: 0,
            ATC_ID: 0,
            NAV_IDENT_1: "",
            NAV_IDENT_2: "",
            ATC_FLIGHT_NUMBER: 0,
            HSI_STATION_IDENT: 0,
            ATC_MODEL: 0,
            CATEGORY: 0,
            AIRSPEED_INDICATED: 0,
            INDICATED_ALTITUDE: 0,
            RADIO_HEIGHT: 0,
            ATTITUDE_INDICATOR_BANK_DEGREES: 0,
            ATTITUDE_INDICATOR_PITCH_DEGREES: 0,
            HEADING_INDICATOR: 0,
            AUTOPILOT_HEADING_LOCK_DIR: 0,
            GPS_IS_ACTIVE_FLIGHT_PLAN: 0,
            GPS_POSITION_LAT: 0,
            GPS_POSITION_LON: 0,
            GPS_POSITION_ALT: 0,
            GPS_WP_CROSS_TRK: 0,
            GPS_WP_DESIRED_TRACK: 0,
            GPS_WP_DISTANCE: 0,
            GPS_WP_NEXT_ID: 0,
            GPS_WP_PREV_ID: 0,
            PLANE_LONGITUDE: 0,
            PLANE_LATITUDE: 0,
            PLANE_HEADING_DEGREES_TRUE: 0,
            PLANE_HEADING_DEGREES_GYRO: 0,
            PLANE_HEADING_DEGREES_MAGNETIC: 0,
            PLANE_PITCH_DEGREES: 0,
            PLANE_BANK_DEGREES: 0,
            LIGHT_TAXI: 0,
            AUTOPILOT_ALTITUDE_LOCK_VAR: 0,
            AUTOPILOT_ALTITUDE_LOCK: 0,
            VERTICAL_SPEED: 0,
            AUTOPILOT_VERTICAL_HOLD_VAR: 0,
            GPS_WP_VERTICAL_SPEED: 0,
            WISKEY_COMPASS_INDICATION_DEGREES: 0,
            AIRSPEED_TRUE: 0,
            AUTOPILOT_AIRSPEED_HOLD_VAR: 0,
            AUTOPILOT_AIRSPEED_HOLD: 0,
            OVERSPEED_WARNING: 0,
            AUTOPILOT_BANK_HOLD: 0,
            AUTOPILOT_FLIGHT_LEVEL_CHANGE: 0,
            BAROMETER_PRESSURE: 0,
            AUTOPILOT_MASTER: 0,
            AUTOPILOT_PITCH_HOLD: 0,
            AUTOPILOT_HEADING_LOCK: 0,
            AUTOPILOT_NAV1_LOCK: 0,
            GENERAL_ENG_FUEL_PUMP_SWITCH_1: 0,
            PITOT_HEAT: 0,
            LIGHT_BEACON: 0,
            LIGHT_LANDING: 0,
            LIGHT_NAV: 0,
            LIGHT_STROBE: 0,
            AUTOPILOT_VERTICAL_HOLD: 0,
            AUTOPILOT_APPROACH_HOLD: 0,
            AUTOPILOT_BACKCOURSE_HOLD: 0,
            AUTOPILOT_FLIGHT_DIRECTOR_PITCH: 0,
            AUTOPILOT_FLIGHT_DIRECTOR_BANK: 0,
            AUTOPILOT_FLIGHT_DIRECTOR_ACTIVE: 0,
            ELECTRICAL_MASTER_BATTERY: 0,
            GENERAL_ENG_MASTER_ALTERNATOR_1: 0,
            GPS_DRIVES_NAV1: 0,
            AVIONICS_MASTER_SWITCH: 0,
            TRAILING_EDGE_FLAPS_LEFT_PERCENT: 0,
            GENERAL_ENG_FUEL_VALVE_1: 0,
            BRAKE_PARKING_INDICATOR: 0,
            ELEVATOR_TRIM_POSITION: 0,
            GENERAL_ENG_RPM_1: 0,
            RECIP_ENG_FUEL_FLOW_1: 0,
            GENERAL_ENG_OIL_PRESSURE_1: 0,
            GENERAL_ENG_OIL_TEMPERATURE_1: 0,
            ENG_EXHAUST_GAS_TEMPERATURE_1: 0,
            FUEL_LEFT_QUANTITY: 0,
            FUEL_RIGHT_QUANTITY: 0,
            NAV_ACTIVE_FREQUENCY_1: 0,
            NAV_STANDBY_FREQUENCY_1: 0,
            COM_ACTIVE_FREQUENCY_1: 0,
            COM_STANDBY_FREQUENCY_1: 0,
            NAV_ACTIVE_FREQUENCY_2: 0,
            NAV_STANDBY_FREQUENCY_2: 0,
            COM_ACTIVE_FREQUENCY_2: 0,
            COM_STANDBY_FREQUENCY_2: 0,
            NAV_HAS_NAV_1: 0,
            NAV_HAS_NAV_2: 0,
            NAV_OBS_1: 0,
            NAV_OBS_2: 0,
            NAV_GS_FLAG_1: 0,
            NAV_GS_FLAG_2: 0,
            NAV_TOFROM_1: 0,
            NAV_TOFROM_2: 0,
            NAV_CODES_1: 0,
            NAV_CODES_2: 0,
            NAV_LOCALIZER_1: 0,
            NAV_MAGVAR_1: 0,
            HSI_CDI_NEEDLE: 0,
            HSI_DISTANCE: 0,
            MAGNETIC_COMPASS: 0,
            AUTOPILOT_NAV_SELECTED: 0,
            AUTOPILOT_GLIDESLOPE_ACTIVE: 0,
            NAV_HAS_GLIDE_SLOPE_1: 0,
            NAV_HAS_GLIDE_SLOPE_2: 0,
            NAV_GSI_1: 0,
            NAV_GSI_2: 0,
            GPS_VERTICAL_ERROR: 0,
            WARNING_VACUUM: 0,
            WARNING_OIL_PRESSURE: 0,
            ELECTRICAL_MAIN_BUS_VOLTAGE: 0,
            MARKER_BEACON_STATE: 0,
            AMBIENT_TEMPERATURE: 0,
            TRANSPONDER_CODE_1: 0,
            TRANSPONDER_STATE_1: 0,
            LOCAL_TIME: 0,
            FUEL_TANK_LEFT_MAIN_QUANTITY: 0,
            FUEL_TANK_RIGHT_MAIN_QUANTITY: 0,
            RECIP_ENG_LEFT_MAGNETO_1: 0,
            RECIP_ENG_RIGHT_MAGNETO_1: 0,
            GENERAL_ENG_STARTER_1: 0,
            AUTOPILOT_FLIGHT_DIRECTOR_PITCH_EX1: 0,
            TURN_COORDINATOR_BALL: 0,
            L_GV_AIRCRAFT_ORIENTATION_AXIS_XYZ_PITCH: 0,
            L_GV_AIRCRAFT_ORIENTATION_AXIS_XYZ_BANK: 0,
            L_GV_AIRCRAFT_ORIENTATION_AXIS_XYZ_HEADING: 0,
            L_GV_AIRCRAFT_ORIENTATION_AXIS_XYZ_ALT: 0,
            L_GPS_CURRENT_PHASE: 0,
            KOHLSMAN_SETTING_MB: 0,
            KOHLSMAN_SETTING_HG: 0,
            GPS_FLIGHT_PLAN_WP_COUNT: 0,
            SUCTION_PRESSURE: 0,
            GENERAL_ENG_ELAPSED_TIME_1: 0,
            AMBIENT_WIND_DIRECTION: 0,
            AMBIENT_WIND_VELOCITY: 0,
            ELECTRICAL_HOT_BATTERY_BUS_VOLTAGE: 0,
            ELECTRICAL_BATTERY_BUS_AMPS: 0,
            ELECTRICAL_GENALT_BUS_AMPS_1: 0,
            GPS_GROUND_SPEED: 0,
            GPS_ETE: 0,
            GPS_ETA: 0,
            STANDARD_ATM_TEMPERATURE: 0,
            GPS_FLIGHT_PLAN_WP_INDEX: 0,
            GPS_TARGET_DISTANCE: 0,
            GPS_WP_ETE: 0,
            GROUND_ALTITUDE: 0,
            PLANE_ALTITUDE:0,
            PLANE_ALT_ABOVE_GROUND: 0
        }
        this.RequestCounter = 0;

    }

    /**
     * @returns {any}
     */
    get NAVMODE() {
        return this._navmode
    }

    set NAVMODE(mode) {
        this._navmode = mode
    }

    subscribe(control, lazy = 1) {
        this.Controls.push({ control: control, lazy: lazy });
    }

    remove(control) {
    }

    update(res) {
        if (res) {
            try {
                if (res.type == "variables") {
                    this.SimData = res.data;
                    //this.LeftPanel.btnBeacon.update(res.data)

                    this.Controls.forEach(element => {
                        if (this.RequestCounter % element.lazy == 0) {
                            //console.log(element.control)
                            element.control.update(res.data)
                        }
                    });
                    this.RequestCounter++
                } else if (res.type == "flightplan") {
                    this.FlightPlanData = res
                    this.updateFlightPlan(this.FlightPlanData)

                }

                if(this.RequestCounter % 10 == 0) {
                    this.updateFlightPlan(this.FlightPlanData)
                }
            }
            catch (e) {
                console.log(e)
            }
        }
    }

    updateFlightPlan(res) {
        if (res.data) {
            let sum = 0;
            let spd = this.SimData.GPS_GROUND_SPEED || 0
            let magvar = this.SimData.GPS_MAGVAR || 0
            let wpi = this.SimData.GPS_FLIGHT_PLAN_WP_INDEX || 0
            let brg = 0
            let dist = 0
            let rem = 0
            let ete = Infinity
            let buff = new Array()
            for (let i = 0; i < res.data.length; i++) {
                if (i > 0) {
                    let wp1 = res.data[i - 1]
                    let wp2 = res.data[i]
                    var p1 = new LatLonSpherical(wp1.lat, wp1.lng);
                    var p2 = new LatLonSpherical(wp2.lat, wp2.lng);
                    brg = (p1.rhumbBearingTo(p2) - magvar).toFixed(0);
                    dist = p1.distanceTo(p2) * LatLonSpherical.metresToNauticalMiles;
                    sum += dist
                    if (i > wpi) {
                        rem += dist
                    } else if (i == wpi) {
                        rem = m2nm(this.SimData.GPS_WP_DISTANCE || 0)
                    }

                    if (spd != 0) {
                        ete = rem / spd * 3600
                    }
                }
                res.data[i].brg = brg
                res.data[i].dist = dist;
                res.data[i].rem = rem;
                res.data[i].ete = ete
                
            }
            this.FlightPlan = res.data;
            this.FlightPlan.brg = { brg: brg }
            this.FlightPlan.sum = { sum: sum }
            this.FlightPlan.rem = { rem: rem }
            this.FlightPlan.eta = { eta: Math.round(rem / spd * 3600) }
            //console.log(this.FlightPlan)
        }
    }

    createLayer(x, y, w, h, id = "div") {
        var l = document.createElement("div");
        l.setAttribute("id", id);
        l.style.left = x + "px";
        l.style.top = y + "px";
        l.style.width = w + "px";
        l.style.height = h + "px";
        l.style.position = "absolute";
        return l;
    }

    createModal(w, h, css = "") {
        let sp = document.getElementById("simpanel")
        let d = this.createLayer((1366 - w) / 2, (768 - h) / 2, w, h, "dialog");
        d.className = "modal"
        sp.appendChild(d)
        return d
    }

    sendCommand(cmd, value = "") {
        if (this.ws && this.ws.readyState == 1) {
            var cmd = "cmd:" + cmd;
            if (value != "") {
                cmd += ",val:" + Math.round(value);
            }

            console.log(cmd);
            this.ws.send(cmd);
        }
    }

    connect() {
        const fms = this;
        fms.counter = 0;
        fms.processed = 0;
        fms.locked = false;
        fms.ws = new WebSocket(SETTINGS.ws_variables);

        fms.ws.onopen = function () {
            fms.ws.send("HELLO");
            fms.locked = false;
        };

        fms.ws.onmessage = function (e) {

            if (e.target.url != SETTINGS.ws_variables) {
                return
            }
            fms.counter++
            if (!fms.locked && fms.counter % 1 == 0) {
                fms.locked = true;
                var res = JSON.parse(e.data)
                fms.update(res)
                fms.locked = false
                // let res = JSON.parse(e.data)

                // pfd.update(res)
                // pfd.locked = false;
                // pfd.processed++
                // if(pfd.processed % 100 == 0) {
                //     console.log(pfd.processed)
                //     console.log(pfd.processed/pfd.counter)
                // }
            }
        };

        fms.ws.onclose = function (e) {
            console.log('Socket is closed. Reconnect will be attempted in 5 second.', e.reason);
            setTimeout(function () {
                fms.connect();
            }, 5000);
        };

        fms.ws.onerror = function (err) {
            fms.ws.close();
            try {
                fms.Controls.forEach(element => {
                    element.control.update(fms.Defaults)
                });
            }
            catch (e) {
                console.log(e)
            }

        };

    }

}

