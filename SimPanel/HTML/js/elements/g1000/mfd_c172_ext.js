
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

        let mapl = this.SimPanel.appendChild(this.createLayer(160, 72, 1046, 626, "map"))
        let MAP = new Map(fms, "map")


        this.LeftPanel = new G1000PFDPanelLeft(this, 160, this.svg.height());
        this.RightPanel = new G1000MFDPanelRight(this, 160, this.svg.height())
        this.RightPanel.translate(width - 160, 0)
        this.PanelTop = new G1000PanelTop(this, 1366 - 320, 50).move(160, 0)
        this.PanelBottom = new G1000PFDPanelBottom(this, 1366 - 318, 70).move(159, 768 - 70)
        this.PanelBottom.btnZoom.hide()



        let apl = this.createLayer(160 + 1046 -350, 72, 0, 626, "airportinfo")
        apl.style.block ="none"
        this.SimPanel.appendChild(apl)
        
        $('#airportinfo').load("airport_info.html", function () {
            load_airportinfo('')
        }); 

        let bg = this.PanelBottom.panel;
        let bbox = bg.bbox();
        this.btnSplit = bg.indicator_button(60, 32, "INFOS").move(bbox.x + 50, bbox.y + 30)
        this.btnSplit.isON = false
        this.btnSplit.setON(false)
        this.btnSplit.click(function (e) {
            fms.btnSplit.isON = !fms.btnSplit.isON
            fms.btnSplit.setON(fms.btnSplit.isON)
            let mapp = document.getElementById("map")
            

            if(fms.btnSplit.isON) {
                apl.style.left = (160 + 1046 -350) + "px"
                apl.style.width = 350 + "px"
                mapp.style.width = (1046 -350) + "px"
                MAP.map.invalidateSize()
            } else {
                apl.style.left = (-10000) + "px"
                apl.style.width = (0) + "px"
                mapp.style.width = (1046) + "px"
                MAP.map.invalidateSize()
            }


        })

        this.PanelBottom.btnTRCK.click(function (e){
            MAP.tracking = !MAP.tracking;
            e.currentTarget.instance.setON(MAP.tracking)
        })

        this.PanelBottom.btnTRCK.setON(MAP.tracking)
        // this.PanelBottom.btnTRCK.click(function (e){
        //     MAP.tracking = !MAP.tracking;
        //     e.currentTarget.instance.setON(MAP.tracking)
        // })

        
        

        // this.PanelBottom.btnMenu.on("click", function () {
            
        //     if (sp.Hidden) {
        //         //sp.style.top = "2000px"
        //         sp.visibility(true)
        //     } else {
        //         sp.visibility(false)
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


        this.RequestCounter = 0;

        //==============================================================================
        // WIND GAUGE
        //==============================================================================
        

        let l = this.createLayer(170,160,100,50, "windgauge");
        l.style.backgroundColor = "red"
        l.style.zIndex = 1000
        document.body.appendChild(l)
        let lsvg = SVG().addTo("#windgauge").size(100,50)
        let wg = lsvg.wind_gauge(100, 50, {gyro: false});
        wg.update = function (data) {
            wg.setData(
                Math.round(data.AMBIENT_WIND_DIRECTION),
                Math.round(data.PLANE_HEADING_DEGREES_GYRO),
                Math.round(data.AMBIENT_WIND_VELOCITY)
            )
        }
        this.subscribe(wg, 5)

        MAP.connect();
        this.connect()

    }
    update(res) {
        super.update(res)
        // //alert("UPDATE")
        // if (res && res.type == "variables") {
        //     this.SimData = res.data;
        //     this.LeftPanel.btnBeacon.update(res.data)

        //     this.Controls.forEach(element => {
        //         if (this.RequestCounter % element.lazy == 0) {
        //             element.control.update(res.data)
        //         }
        //     });

        //     // this.Altimeter.update(res.data)
        //     // this.SpeedIndicator.update(res.data)



        //     this.RequestCounter++
        // }
    }

}