class G1000MFDPanelRight {
    constructor(fms, w, h) {


        const MFD = fms;
        this.svg = fms.svg; //.nested()

        let bw = 60;
        let bh = 28;

        this.lw = this.svg.group()
        let lw = this.lw;
        this.svg.add(lw)
        lw.rect(w, h);
        let lazy = 5;


        let font1 = { 'fill': 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 14, leading: '0em' }
        let font2 = { 'fill': 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 28, leading: '0em' }
        let font3 = { 'fill': 'cyan', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 14, leading: '0em' }

        let rpm = lw.rpm_gauge(160, 128, 65, {
            font1: font1,
            font2: font2,
            min: 0,
            opt1: 2000,
            opt2: 2700,
            max: 3000,
        }).translate(0, 10);
        rpm.update = function (data) {
            if (this.GENERAL_ENG_RPM_1 != Math.round(data.GENERAL_ENG_RPM_1)) {
                this.setRPM(data.GENERAL_ENG_RPM_1);
                this.GENERAL_ENG_RPM_1 = Math.round(data.GENERAL_ENG_RPM_1)
            }
        }
        fms.subscribe(rpm, 1);

        let ff = lw.horizontal_gauge(130, 50, {
            title: "FFLOW GPH",
            font1: font1,
            font2: font2,
            font3: font3,
            min: 0,
            max: 20,
            div: 2,
            limits: [
                { min: 0, max: 12, color: "green" },
            ],
            fixed: 1,
        }).move(15, 150);
        ff.update = function (data) {
            if (this.RECIP_ENG_FUEL_FLOW_1 != data.RECIP_ENG_FUEL_FLOW_1) {
                this.setValue(data.RECIP_ENG_FUEL_FLOW_1 * 0.17);
                this.RECIP_ENG_FUEL_FLOW_1 = data.RECIP_ENG_FUEL_FLOW_1
            }
        }
        fms.subscribe(ff, lazy);

        let op = lw.horizontal_gauge(130, 50, {
            title: "OIL PRESS",
            font1: font1,
            font2: font2,
            font3: font3,
            min: 0,
            max: 120,
            div: 0,
            limits: [
                { min: 0, max: 20, color: "red" },
                { min: 40, max: 90, color: "green" },
                { min: 110, max: 120, color: "red" },
            ],
            fixed: 0,
        }).move(15, 220);
        op.update = function (data) {
            if (this.GENERAL_ENG_OIL_PRESSURE_1 != data.GENERAL_ENG_OIL_PRESSURE_1) {
                this.setValue(data.GENERAL_ENG_OIL_PRESSURE_1);
                this.GENERAL_ENG_OIL_PRESSURE_1 = data.GENERAL_ENG_OIL_PRESSURE_1
            }
        }
        fms.subscribe(op, lazy);


        let ot = lw.horizontal_gauge(130, 50, {
            title: "OIL TEMP",
            font1: font1,
            font2: font2,
            font3: font3,
            min: 0,
            max: 1000,
            div: 0,
            limits: [
                // { min: 0, max: 4, color: "red" },
                { min: 200, max: 960, color: "green" },
                { min: 960, max: 1000, color: "red" },
            ],
            fixed: 0,
        }).move(15, 290);
        ot.update = function (data) {
            if (this.GENERAL_ENG_OIL_TEMPERATURE_1 != Math.round(data.GENERAL_ENG_OIL_TEMPERATURE_1)) {
                this.setValue(data.GENERAL_ENG_OIL_TEMPERATURE_1);
                this.GENERAL_ENG_OIL_TEMPERATURE_1 = Math.round(data.GENERAL_ENG_OIL_TEMPERATURE_1)
            }
        }
        fms.subscribe(ot, lazy);


        let et = lw.horizontal_gauge(130, 50, {
            title: "EGT",
            font1: font1,
            font2: font2,
            font3: font3,
            min: 0,
            max: 2500,
            div: 500,
            limits: [],
            fixed: 0,

        }).move(15, 360);
        et.update = function (data) {
            if (this.ENG_EXHAUST_GAS_TEMPERATURE_1 != Math.round(data.ENG_EXHAUST_GAS_TEMPERATURE_1)) {
                this.setValue(data.ENG_EXHAUST_GAS_TEMPERATURE_1);
                this.ENG_EXHAUST_GAS_TEMPERATURE_1 = Math.round(data.ENG_EXHAUST_GAS_TEMPERATURE_1)
            }
        }
        fms.subscribe(et, lazy);

        let vc = lw.horizontal_gauge(130, 50, {
            title: "VAC",
            font1: font1,
            font2: font2,
            font3: font3,
            min: 0,
            max: 10,
            div: 0,
            limits: [
                { min: 4, max: 7, color: "green" },
            ],
            fixed: 1,

        }).move(15, 430);
        vc.update = function (data) {
            if (this.SUCTION_PRESSURE != data.SUCTION_PRESSURE) {
                this.setValue(data.SUCTION_PRESSURE);
                this.SUCTION_PRESSURE = data.SUCTION_PRESSURE;
            }
        }
        fms.subscribe(vc, lazy);


        let fq = lw.fuelquantity_gauge(130, 70, {
            title: "FUEL QTY GAL",
            font1: font1,
            font2: font2,
            font3: font3,
            min: 0,
            max: 30,
            div: 10,
            limits: [
                { min: 0, max: 2, color: "red" },
                { min: 2, max: 5, color: "yellow" },
                { min: 5, max: 30, color: "green" },
            ],
            fixed: 0,

        }).move(15, 500);
        fq.update = function (data) {
            let finv = false;
            
            if (Math.abs((this.FUEL_TANK_LEFT_MAIN_QUANTITY || 0)- data.FUEL_TANK_LEFT_MAIN_QUANTITY) > 0.1) {
                finv = true;
                this.FUEL_TANK_LEFT_MAIN_QUANTITY = data.FUEL_TANK_LEFT_MAIN_QUANTITY
            }

            if (Math.abs((this.FUEL_TANK_RIGHT_MAIN_QUANTITY || 0) - data.FUEL_TANK_RIGHT_MAIN_QUANTITY) > 0.1) {
                finv = true;
                this.FUEL_TANK_RIGHT_MAIN_QUANTITY = data.FUEL_TANK_RIGHT_MAIN_QUANTITY
            }

            if (finv) {
                this.setValue(data.FUEL_TANK_LEFT_MAIN_QUANTITY, data.FUEL_TANK_RIGHT_MAIN_QUANTITY);
            }
        }
        fms.subscribe(fq, lazy);

        var font = { 'fill': 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 14, leading: '1em' };
        lw.text("ENG HRS").font(font).move(20, fq.bbox().y2 + 10);
        let eh = lw.text("0").font(font).move(120, fq.bbox().y2 + 10);
        eh.update = function (data) {
            eh.text(data.GENERAL_ENG_ELAPSED_TIME_1.toFixed(1));
            eh.x(lw.bbox().x2 - eh.bbox().w - 20)
        }
        fms.subscribe(eh, lazy)

        var t = lw.text("- ELECTRICAL -").font(font).center(lw.bbox().cx, eh.bbox().y2 + 20);

        var y = t.bbox().y2 + 10;
        t = lw.text("M").font(font).move(lw.bbox().x + 20, y);
        t = lw.text("BUS").font(font);
        t.move(lw.bbox().cx - t.bbox().w / 2, y);
        t = lw.text("E").font(font);
        t.move(lw.bbox().x2 - 20 - t.bbox().w, y);

        var y = t.bbox().y2
        t = lw.text("VOLTS").font(font)
        t.move(lw.bbox().cx - t.bbox().w / 2, y)
        let v1 = lw.text("__._").font(font).move(lw.bbox().x + 20, y)
        v1.update = function (data) {
            v1.text(data.ELECTRICAL_MAIN_BUS_VOLTAGE.toFixed(1))
        }
        fms.subscribe(v1, lazy)

        let v2 = lw.text("__._").font(font)
        v2.move(lw.bbox().x2 - v2.bbox().w - 20, y)
        v2.update = function (data) {
            let v = data.ELECTRICAL_MAIN_BUS_VOLTAGE - data.ELECTRICAL_HOT_BATTERY_BUS_VOLTAGE
            v2.text(v.toFixed(1))
        }
        fms.subscribe(v2, lazy)

        var y = t.bbox().y2 + 10;
        t = lw.text("M").font(font).move(lw.bbox().x + 20, y);
        t = lw.text("BATT").font(font);
        t.move(lw.bbox().cx - t.bbox().w / 2, y);
        t = lw.text("S").font(font);
        t.move(lw.bbox().x2 - 20 - t.bbox().w, y);

        var y = t.bbox().y2;
        t = lw.text("AMPS").font(font);
        t.move(lw.bbox().cx - t.bbox().w / 2, y);
        let a1 = lw.text("__._").font(font).move(lw.bbox().x + 20, y);
        a1.update = function (data) {
            a1.text((-data.ELECTRICAL_BATTERY_BUS_AMPS).toFixed(1))
        }
        fms.subscribe(a1, lazy)

        let a2 = lw.text("__._").font(font);
        a2.move(lw.bbox().x2 - v2.bbox().w - 20, y);
        a2.update = function (data) {
            //a2.text(data.ELECTRICAL_GENALT_BUS_AMPS_1.toFixed(1));
            a2.text((-data.ELECTRICAL_BATTERY_BUS_AMPS).toFixed(1))
            a2.x(lw.bbox().x2 - a2.bbox().w - 20);            
        }
        fms.subscribe(a2, lazy)
        //lw.x(svg.width() - 160)
        //this.lw.translate(100,100)
    }

    translate(x, y) {
        this.lw.translate(x, y)
        return this;
    }
    // x(x) {
    //     this.lw.x(x)
    //     return this;
    // }
}