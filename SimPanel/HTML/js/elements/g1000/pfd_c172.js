
class C172 extends FMS {
    constructor(width, height) {
        super();
        const fms = this;

        var opt = {};
        var font1 = opt.font || { fill: "white", 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 28, leading: '0em' }

        this.SimPanel = document.getElementById("simpanel");

        this.SimPanel.appendChild(this.createLayer(0, 0, width, height, "bottom_layer"));
        this.svg = SVG().addTo("#bottom_layer").size(width, height);

        let svg = this.svg;

        let cx = width / 2;
        this.Attitude = this.svg.attitude_indicator(width, height, width / 2 - 30, 300)
        this.subscribe(this.Attitude, 1)

        this.Compass = this.svg.compass_gauge(270, 280).center(width / 2 - 30, height / 2 + 150);
        this.Compass.Rose.rotate(45)
        this.Compass.Rose.rotate(0)
        this.Compass.Needle.rotate(90)
        this.Compass.Needle.dist(1.5)
        this.Compass.Bug.rotate(-45)
        this.subscribe(this.Compass, 1)

        this.GSIndicator = this.svg.gsindicator(40, 230).move(cx + 240, 147);
        this.subscribe(this.GSIndicator, 2)
        this.Altimeter = this.svg.altimeter_gauge(130, 320).move(cx + 280, 104);
        this.subscribe(this.Altimeter, 1)
        this.VerticalSpeed = this.svg.verticalspeed(65, 330).move(cx + 410, 135);
        this.subscribe(this.VerticalSpeed, 1)

        let ssvg = this.svg.nested()
        this.SpeedIndicator = ssvg.speedindicator_gauge(100, 320).move(cx - 400, 104);
        this.SpeedIndicator.Ruler.setBug(123)
        
        this.subscribe(this.SpeedIndicator, 1)

        //==============================================================================
        // TRIM INDICATOR
        //==============================================================================
        let ti = svg.trim_indicator(80, 100).move(width - 160 - 90, height - 180)
        ti.update = function (data) {
            if (this.ELEVATOR_TRIM_POSITION != Math.round(data.ELEVATOR_TRIM_POSITION)) {
                this.setPos(data.ELEVATOR_TRIM_POSITION);
                this.ELEVATOR_TRIM_POSITION = Math.round(data.ELEVATOR_TRIM_POSITION)
            }
        }
        this.subscribe(ti, 5)

        //==============================================================================
        // FLAPS CONTROL
        //==============================================================================
        let fc = svg.flaps_control(130, 100).move(width - 160 - 230, height - 180)
        fc.update = function (data) {
            if (this.TRAILING_EDGE_FLAPS_LEFT_PERCENT != data.TRAILING_EDGE_FLAPS_LEFT_PERCENT) {
                this.setPos(data.TRAILING_EDGE_FLAPS_LEFT_PERCENT);
                this.TRAILING_EDGE_FLAPS_LEFT_PERCENT = data.TRAILING_EDGE_FLAPS_LEFT_PERCENT
            }
        }
        this.subscribe(fc, 5)
        fc.btnUP.click(function (e) {
            fms.sendCommand("FLAPS_DECR", 0);
        })
        fc.btnDN.click(function (e) {
            fms.sendCommand("FLAPS_INCR", 0);
        })

        //==============================================================================
        // WIND GAUGE
        //==============================================================================
        let wg = svg.wind_gauge(100, 50).move(390, 340)
        wg.update = function (data) {
            wg.setData(
                Math.round(data.AMBIENT_WIND_DIRECTION),
                Math.round(data.PLANE_HEADING_DEGREES_GYRO),
                Math.round(data.AMBIENT_WIND_VELOCITY)
            )
        }
        this.subscribe(wg, 5)

        let mapl = this.SimPanel.appendChild(this.createLayer(164, 499, 257, 194, "map"));
        let MAP = new Map(fms, "map")


        this.LeftPanel = new G1000PFDPanelLeft(this, 160, this.svg.height());
        this.RightPanel = new G1000MFDPanelRight(this, 160, this.svg.height())
        this.RightPanel.translate(width - 160, 0)
        this.PanelTop = new G1000PanelTop(this, 1366 - 320, 50).move(160, 0)
        this.PanelBottom = new G1000PFDPanelBottom(this, 1366 - 318, 70).move(159, 768 - 70)

        this.PanelBottom.btnZoom.click(function (e) {
            let map = document.getElementById("map");
            let btn = e.currentTarget.instance;
            if (btn.ON) {
                map.style.top = "499px"
                map.style.left = "164px"
                map.style.width = "257px"
                map.style.height = "194px"
                MAP.map.invalidateSize()
                btn.setON(false)
            } else {
                map.style.top = "72px"
                map.style.left = "160px"
                map.style.width = "1046px"
                map.style.height = "626px"
                MAP.map.invalidateSize()
                btn.setON(true)
            }
        })

        this.PanelBottom.btnTRCK.setON(MAP.tracking)
        this.PanelBottom.btnTRCK.click(function (e) {
            MAP.tracking = !MAP.tracking;
            e.currentTarget.instance.setON(MAP.tracking)
        })

        // this.SwitchPanel = null;
        // this.PanelBottom.btnMenu.on("click", function () {
        //     if (pfd.SwitchPanel == null) {
        //         pfd.SwitchPanel = new SwitchPanel(pfd, 160, 498, 1046, 200);
        //         pfd.SwitchPanel.visibility(true)
        //         // pfd.SimPanel.appendChild(pfd.createLayer(160, 498, 1046, 200, "switch_layer"));
        //         // pfd.svg = SVG().addTo("#switch_layer").size(1046, 200);
        //         // let sl = document.getElementById("switch_layer")
        //         // sl.style.zIndex = getZIndex() + 1
        //         // pfd.SwitchPanel = new SwitchPanel(pfd, 1046, 200);//.move(161, height - 231)
        //     } else {
        //         pfd.SwitchPanel.close();
        //         pfd.SwitchPanel = null;
        //     }

        // });

        let swp = null;
        this.PanelBottom.btnMenu.on("click", function () {
            if (swp == null) {
                swp = new SwitchPanel(fms, 160, 498, 1046, 200);
                console.log(swp)
            } else {
                swp.close();
                swp = null;
            }
        });



        var font = { 'fill': 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 32 };
        let MarkerBeacon = svg.boxed_text(40, 40, "O").font(font).move(cx + 240, 107)
        MarkerBeacon.update = function (data) {
            if (MarkerBeacon.pValue != data.MARKER_BEACON_STATE) {
                if (data.MARKER_BEACON_STATE > 0) {
                    MarkerBeacon.show();
                    switch (data.MARKER_BEACON_STATE) {
                        case 1:
                            MarkerBeacon.fg("black")
                            MarkerBeacon.bg("cyan")
                            MarkerBeacon.text("O")
                            break;
                        case 2:
                            MarkerBeacon.fg("black")
                            MarkerBeacon.bg("yellow")
                            MarkerBeacon.text("M")
                            break;
                        case 3:
                            MarkerBeacon.fg("white")
                            MarkerBeacon.bg("blue")
                            this.MarkerBeacon.text("I")
                            break;
                    }
                }
                else {
                    MarkerBeacon.hide()
                }
                MarkerBeacon.pValue = data.MARKER_BEACON_STATE;
            }
        }
        this.subscribe(MarkerBeacon)


        var font = { 'fill': 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 28, leading: '0em' };
        let ra = svg.boxed_text(150,32, "0").font(font).move(cx + 74, 364)
        ra.update = function (data) {
            //let a = data.PLANE_ALTITUDE - m2feet(data.GROUND_ALTITUDE)
            ra.text("RA" + padLeft(data.PLANE_ALT_ABOVE_GROUND.toFixed(0), 5) + "ft")
        }
        this.subscribe(ra, 2)

        this.RequestCounter = 0;

        MAP.connect();
        //this.WSClient.connect()
        this.connect()

    }
    update(res) {
        //alert("UPDATE")
        super.update(res)
    }



}