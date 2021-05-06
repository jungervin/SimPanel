class Map {

    constructor(fms, div_id) {

        this.FMS = fms;
        this.map = L.map(div_id, { dragging: true }).setView([47.180086, 19.503736], 7);
        this.pBounds = {
            _northEast: { lat: 0, lng: 0 },
            _southWest: { lat: 0, lng: 0 },
        }

        this.tracking = true;

        var layer = new L.TileLayer('https://tile.opentopomap.org/{z}/{x}/{y}.png', {
            maxZoom: 22,
            minZoom: 1,
            format: "image/png",
            subdomains: ["a", "b", "c"]
        }).addTo(this.map);


        var osm = new L.TileLayer("http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
            maxZoom: 18,
            minZoom: 2,
            format: "image/png",
            subdomains: ["a", "b", "c"]
        });

        var openaip_cached_basemap = new L.TileLayer("http://{s}.tile.maps.openaip.net/geowebcache/service/tms/1.0.0/openaip_basemap@EPSG%3A900913@png/{z}/{x}/{y}.png", {
            maxZoom: 14,
            minZoom: 4,
            tms: true,
            detectRetina: true,
            subdomains: "12",
            format: "image/png",
            transparent: false
        });

        var esri = L.tileLayer("https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}.jpg", {
            maxZoom: 17,
            //attribution: 'Tiles © Esri — Source: Esri, i-cubed, USDA, USGS, AEX, GeoEye, Getmapping, Aerogrid, IGN, IGP, UPR-EGP, and the GIS User Community'
        });

        var stamen_black_white = new L.TileLayer("http://{s}.tile.stamen.com/toner/{z}/{x}/{y}.png", {
            maxZoom: 15,
            minZoom: 2,
            format: "image/png",
            subdomains: ["a", "b", "c"]
        });

        var stamen_terrain = new L.TileLayer("http://{s}.tile.stamen.com/terrain/{z}/{x}/{y}.png", {
            maxZoom: 18,
            minZoom: 2,
            format: "image/png",
            subdomains: ["a", "b", "c"]
        });

        var stamen_water = new L.TileLayer("http://{s}.tile.stamen.com/watercolor/{z}/{x}/{y}.png", {
            maxZoom: 18,
            minZoom: 2,
            format: "image/png",
            subdomains: ["a", "b", "c"]
        });

        var carto_dark = new L.TileLayer("https://cartodb-basemaps-{s}.global.ssl.fastly.net/dark_all/{z}/{x}/{y}.png", {
            maxZoom: 18,
            minZoom: 2,
            format: "image/png",
            subdomains: ["a", "b", "c"]
        });


        // var attrib = L.control.attribution({ position: "bottomleft" });
        // attrib.addAttribution("<a href=\"https://www.openstreetmap.org/copyright\" target=\"_blank\" style=\"\">Topo</a>");
        // attrib.addAttribution("<a href=\"https://www.openstreetmap.org/copyright\" target=\"_blank\" style=\"\">OpenStreetMap</a>");
        // attrib.addAttribution("<a href=\"https://www.openstreetmap.org/copyright\" target=\"_blank\" style=\"\">ESRI</a>");
        // attrib.addAttribution("<a href=\"https://carto.com/\" target=\"_blank\" style=\"\">Esri</a>");
        // attrib.addAttribution("<a href=\"https://www.openaip.net\" target=\"_blank\" style=\"\">openAIP</a>");
        // attrib.addAttribution("<a href=\"http://maps.stamen.com\" target=\"_blank\" style=\"\">Stamen</a>");
        // attrib.addAttribution("<a href=\"https://carto.com/\" target=\"_blank\" style=\"\">Carto</a>");

        //attrib.addTo(this.map);


        var baseMaps = {
            "Topo": layer,
            "OpenStreetMap": osm,
            "ESRI": esri,
            "Stamen Terrain": stamen_terrain,
            "Stamen Toner": stamen_black_white,
            "Stamen Water": stamen_water,
            "Carto Dark (Night Mode)": carto_dark,
        };

        this.RunwayLayer = L.featureGroup();
        this.map.addLayer(this.RunwayLayer);

        this.parkings = L.featureGroup();
        this.map.addLayer(this.parkings);


        var overlayMaps = {
            "OpenAIP": openaip_cached_basemap,
            "Airports": this.RunwayLayer,
        };

        L.control.layers(baseMaps, overlayMaps).addTo(this.map);

        L.control.scale().addTo(this.map);

        this.map.on('click', function (e) {
            var coord = e.latlng;
            var lat = coord.lat;
            var lng = coord.lng;
            console.log("You clicked the map at latitude: " + lat + " and longitude: " + lng);
        });

        this.PlaneIcon = L.icon({
            iconUrl: "images/plane.svg",
            iconSize: [30, 30],

        });


        this.DepartureIcon = L.icon({
            iconUrl: "images/departure.svg",
            iconSize: [22, 22]
        });

        this.ArrivalIcon = L.icon({
            iconUrl: "images/arrival.svg",
            iconSize: [22, 22]
        });

        this.WaypointIcon = L.icon({
            iconUrl: "images/waypoint.svg",
            iconSize: [12, 12]
        });

        this.FuelIcon = L.icon({
            iconUrl: "images/fuel.svg",
            iconSize: [12, 12]
        });

        this.ParkingIcon = L.icon({
            iconUrl: "images/parking.svg",
            iconSize: [12, 12]
        });

        this.AirportIcon = L.icon({
            iconUrl: "images/airport.svg",
            iconSize: [18, 18]
        });


        this.markers = L.featureGroup();
        this.map.addLayer(this.markers);



        this.planes = L.featureGroup();
        this.planes.setZIndex(100);
        this.AirPlane = L.marker([47.180086, 19.503736], { icon: this.PlaneIcon, rotationAngle: 145, rotationOrigin: 'center center', range: 10, }); //.bindPopup(l); //.openPopup();
        this.AirPlane.addTo(this.planes);

        this.map.addLayer(this.planes);
        this.AirPlane.setRotationAngle(270);

        this.markers.setZIndex(10);
        this.planes.setZIndex(100);



        var request_num = 0;

        this.FlightPlan = null;
        this.Timestamp = 0;
        this.InfosTimeStamp = 0;
        this.InfosDeprecated = true;
        this.prevZoom = 0;


        const self = this;
        this.map.on("zoomend", function (e) {
            //console.log("Zoom End");
            // if(self.prevZoom > self.map.getZoom()) {
            //     self.InfosDeprecated = true;
            //     self.prevZoom = self.map.getZoom();
            // }
        })

        this.map.on("moveend", function (e) {
            //console.log("Position  Changed");

            //var distance = getDistance([lat1, lng1], [lat2, lng2])

            //self.InfosDeprecated = true;
        })
    }

    getInfos() {
        var t = new Date();
        if (t - this.InfosTimeStamp > 1000) {

            //     1° = 111 km  (or 60 nautical miles)
            //     0.1° = 11.1 km
            //     0.01° = 1.11 km (2 decimals, km accuracy)
            //     0.001° =111 m
            //     0.0001° = 11.1 m
            //     0.00001° = 1.11 m
            //     0.000001° = 0.11 m (7 decimals, cm accuracy)

            let b = this.map.getBounds()
            let u = Math.abs(b._northEast.lat - this.pBounds._northEast.lat) > 0.005
            u = u || Math.abs(b._northEast.lng - this.pBounds._northEast.lng) > 0.005
            u = u || Math.abs(b._southWest.lat - this.pBounds._southWest.lat) > 0.005
            u = u || Math.abs(b._southWest.lng - this.pBounds._southWest.lng) > 0.005
            if (u) {
                this.pBounds = b
                this.InfosDeprecated = false;

                var rating = 5;
                if (this.map.getZoom() <= 5) {
                    rating = 5;
                } else if (this.map.getZoom() <= 6) {
                    rating = 4;
                }
                else if (this.map.getZoom() <= 7) {
                    rating = 3;
                }
                else if (this.map.getZoom() <= 8) {
                    rating = 2;
                }
                else if (this.map.getZoom() <= 9) {
                    rating = 1;
                }
                else {
                    rating = 0;
                }


                var q = JSON.stringify({ cmd: "getrunways", data: { zoom: this.map.getZoom(), rating: rating, bounds: this.map.getBounds() } });
                //console.log(q);
                this.ws.send(q);
                if (this.map.getZoom() >= 12) {
                    var q = JSON.stringify({ cmd: "getparkings", data: { zoom: this.map.getZoom(), rating: rating, bounds: this.map.getBounds() } });
                    this.ws.send(q);
                } else {
                    // Letörölhetjük a pakolókat, esetleg?
                }
                //this.ws.send("getflightplan")
                this.InfosTimeStamp = t;
            }
        }
    }

    processData(data) {

        switch (data.type) {

            case "flightplan":

                if (this.Timestamp == data.timestamp) {
                    return;
                }
                if (this.FlightPlan != null) {
                    this.FlightPlan.remove();
                }

                this.FlightPlan = L.featureGroup();
                this.Timestamp = data.timestamp;
                var pointList = [];
                for (var i = 0; i < data.data.length; i++) {
                    var wp = data.data[i];
                    var p = new L.LatLng(wp.lat, wp.lng);
                    pointList.push(p);

                    let desc = wp.icao != "" ? "ICAO: " + wp.icao + "<br>Desc: " + wp.desc : "Desc: " + wp.desc
                    if (i == 0) {
                        // var center = [wp.lat, wp.lng];
                        // map.panTo(center);
                        L.marker(p, { icon: this.DepartureIcon }).addTo(this.FlightPlan).bindPopup("<strong>" + desc + "</strong>"); //.openPopup();

                    }
                    else if (i == data.data.length - 1) {
                        // var center = [wp.lat, wp.lng];
                        // map.panTo(center);
                        L.marker(p, { icon: this.ArrivalIcon }).addTo(this.FlightPlan).bindPopup("<strong>" + desc + "</strong>"); //.openPopup();

                    } else {
                        L.marker(p, { icon: this.WaypointIcon }).addTo(this.FlightPlan).bindPopup("<strong>" + desc + "</strong>"); //.openPopup();
                    }

                }

                var lines = new L.polyline(pointList, {
                    color: 'fuchsia',
                    weight: 5,
                    opacity: 0.9,
                    smoothFactor: 10

                });

                lines.addTo(this.FlightPlan);
                this.FlightPlan.addTo(this.map);
                this.planes.bringToFront();
                break

            case "runways":
                let fms = this.FMS;
                this.RunwayLayer.clearLayers();
                for (var i = 0; i < data.runways.length; i++) {
                    let rw = data.runways[i];
                    //var p = [airport.laty, airport.lonx];
                    // var p1 = new L.LatLng(airport.top_laty, airport.left_lonx);
                    // var p2 = new L.LatLng(airport.bottom_laty, airport.right_lonx);
                    // var p1 = [airport.top_laty, airport.left_lonx];
                    // var p2 = [airport.bottom_laty, airport.right_lonx];
                    //var r =   L.GeometryUtil.angle(this.map, p1, p2) + 10;
                    // L.marker([airport.laty, airport.lonx], { icon: this.AirportIcon, rotationAngle: airport.heading }).addTo(this.airports).bindPopup("<strong>" + airport.ident + "</strong><br>" + airport.city + "<br>"); //.openPopup();
                    L.marker([rw.laty, rw.lonx], { icon: this.AirportIcon, rotationAngle: rw.heading }).addTo(this.RunwayLayer).bindPopup(
                        "ICAO: <strong>" + rw.ident + "</strong><br>" +
                        "NAME: <strong>" + rw.name + "</strong><br>" +
                        "CITY: <strong>" + rw.city + "</strong><br>" +
                        "ALT: <strong>" + rw.altitude + " ft</strong><br>" +
                        "ALT: <strong>" + (rw.heading.toFixed(1) + "° (" + ((rw.heading + 180) % 360).toFixed(1)) + "°)</strong><br>" +
                        "TOWER: <strong>" + (rw.tower_frequency != null ? (rw.tower_frequency / 1000).toFixed(3) + " kHz" : "N/A") + "</strong><br>" +
                        "ATIS: <strong>" + (rw.atis_frequency != null ? (rw.atis_frequency / 1000).toFixed(3) + " kHz" : "N/A") + "</strong><br>"

                    )
                    // L.marker([rw.laty, rw.lonx], { icon: this.AirportIcon, rotationAngle: rw.heading }).addTo(this.RunwayLayer).on("click", function (e) {

                    //     let d = fms.createModal(800, 400)

                    //     let runways = document.querySelector('#runways').content.cloneNode(true);
                    //     d.appendChild(runways)

                    //     let table = document.querySelector('#runwaytable');
                    //     let tbody = table.querySelector('tbody');

                    //     function addRow(label, value) {
                    //         let row = document.querySelector('#runwayrow').content.cloneNode(true)
                    //         let td = row.querySelectorAll("td")
                    //         td[0].innerHTML = "<b>" + label + "</b>"
                    //         td[1].innerHTML = "<b>" + value + "</b>"
                    //         tbody.appendChild(row)
                    //     }

                    //     addRow("ICAO", rw.ident)
                    //     addRow("CITY", rw.city)
                    //     addRow("NAME", rw.name)
                    //     addRow("ALT", rw.altitude + " ft")
                    //     addRow("HDG", rw.heading.toFixed(1) + "° (" + ((rw.heading+180) % 360).toFixed(1)  + "°)")
                    //     addRow("TOWER", rw.tower_frequency != null ? (rw.tower_frequency / 1000).toFixed(3) + " kHz" : "N/A")
                    //     addRow("ATIS",  rw.atis_frequency != null ? (rw.atis_frequency / 1000).toFixed(3) + " kHz" : "N/A")

                    //     function close() {
                    //         d.remove()
                    //          document.getElementById("simpanel").removeEventListener("click", close)
                    //     }
                    //    // document.getElementById("simpanel").addEventListener("click", close)
                    //     let btn = document.getElementById("btnRunwaysClose")
                    //     btn.onclick = function name(params) {
                    //         d.remove()
                    //     }

                    // })




                    // ); //.openPopup();
                }

                break;

            case "parkings":
                // Esetleg letörölhetjük, ha már kicsi a nagyítás, mert így addig marad, amíg új
                this.parkings.clearLayers();
                for (var i = 0; i < data.parkings.length; i++) {
                    var park = data.parkings[i];
                    //var p = new L.LatLng(park.laty, park.lonx);
                    //pointList.push(p);
                    switch (park.type) {
                        case "FUEL":
                            L.marker([park.laty, park.lonx], { icon: this.FuelIcon }).addTo(this.parkings);
                            //L.circle(p, 30).addTo(this.airports);                       
                            //L.circleMarker(p, {radius: 7, fillColor: "red", weight: 1, color: "red"}).addTo(this.airports);
                            break;

                        default:
                            L.marker([park.laty, park.lonx], { icon: this.ParkingIcon }).addTo(this.parkings);
                            //L.circleMarker(p, {radius: 7, fillColor: "green", weight: 1, color: "green"}).addTo(this.airports);
                            break;
                    }
                }
                break;
        }

    }


    AirPlaneSetPos(lat, lng, deg, track) {
        if (this.tracking) {
            var center = [lat, lng];
            this.AirPlane.setRotationAngle(deg - 34);
            this.AirPlane.setLatLng(center);
            if (track) {
                this.map.panTo(center);

            }
        }
    }



    update333(res) {

        if (res.type == "flightplan") {

            if (this.Timestamp == res.timestamp) {
                return;
            }
            if (this.FlightPlan != null) {
                this.FlightPlan.remove();
            }

            this.FlightPlan = L.featureGroup();
            this.Timestamp = res.timestamp;
            var pointList = [];
            for (var i = 0; i < res.data.length; i++) {
                var wp = res.data[i];
                var p = new L.LatLng(wp.lat, wp.lng);
                pointList.push(p);

                if (i == 0) {
                    // var center = [wp.lat, wp.lng];
                    // map.panTo(center);
                    L.marker(p, { icon: this.DepartureIcon }).addTo(this.FlightPlan).bindPopup("<strong>" + wp.id + "</strong><br>Freq: 117.1 kHz<br>Range: 100 km"); //.openPopup();

                }
                else if (i == res.data.length - 1) {
                    // var center = [wp.lat, wp.lng];
                    // map.panTo(center);
                    L.marker(p, { icon: this.ArrivalIcon }).addTo(this.FlightPlan).bindPopup("<strong>" + wp.id + "</strong><br>Freq: 117.1 kHz<br>Range: 100 km"); //.openPopup();

                } else {
                    L.marker(p, { icon: this.WaypointIcon }).addTo(this.FlightPlan).bindPopup("<strong>" + wp.id + "</strong><br>Freq: 117.1 kHz<br>Range: 100 km"); //.openPopup();
                }

            }

            // this.FlightPlan = new L.polyline(pointList, {
            //     color: 'fuchsia',
            //     weight: 5,
            //     opacity: 0.9,
            //     smoothFactor: 10

            // });
            var lines = new L.polyline(pointList, {
                color: 'fuchsia',
                weight: 5,
                opacity: 0.9,
                smoothFactor: 10

            });

            lines.addTo(this.FlightPlan);
            this.FlightPlan.addTo(this.map);
            //this.map.fitBounds(this.FlightPlan.getBounds());
            //this.markers.setZIndex(10);
            //this.planes.setZIndex(100);
            this.planes.bringToFront();

            //                                planes.bringToFront();

            // setTimeout(function () {
            //     ws.send("getflightplane");
            // }, 10000);


        }

        // https://gis.stackexchange.com/questions/182880/leaflet-js-add-text-to-circles

        else if (res.type == "parkingsiiiiiiiiiiiii") {
            console.log(res.data);

            var pointList = [];
            this.RunwayLayer = L.featureGroup();
            this.map.addLayer(this.RunwayLayer);


            var lines = res.data.split(';');
            for (var i = 0; i < lines.length; i++) {
                var cols = lines[i].split(",");
                var parking_id = cols[0];
                var airport_id = cols[1];
                var type = cols[2];
                var pushback = cols[3];
                var name = cols[4];
                var number = cols[5];
                var airline_codes = cols[6];
                var radius = cols[7];
                var heading = cols[8];
                var has_jetway = cols[9];
                var lonx = cols[10];
                var laty = cols[11];

                var p = new L.LatLng(laty, lonx);
                //pointList.push(p);
                switch (type) {
                    case "FUEL":
                        L.marker(p, { icon: this.FuelIcon }).addTo(this.RunwayLayer);
                        //L.circle(p, 30).addTo(this.airports);                       
                        //L.circleMarker(p, {radius: 7, fillColor: "red", weight: 1, color: "red"}).addTo(this.airports);
                        break;

                    default:
                        L.marker(p, { icon: this.ParkingIcon }).addTo(this.RunwayLayer);
                        //L.circleMarker(p, {radius: 7, fillColor: "green", weight: 1, color: "green"}).addTo(this.airports);
                        break;
                }


            }
            this.RunwayLayer.addTo(this.map);
            var b = this.map.getBounds();
            console.log(b);
        }



    }

    connect() {

        this.request_num = -1;
        const MAP = this;
        const url = "ws://192.168.1.174:5000/sim"
        this.ws = new WebSocket(url);
        this.ws.onopen = function () {
            this.request_num = -1;
            MAP.ws.send("getposition");
            //MAP.ws.send("getparking:51858");
            var wstatus = document.getElementById("ws_status");
            if (wstatus) {
                wstatus.style.display = "none";
            }
        };

        this.ws.onmessage = function (e) {
            if (this.url != url) {
                return
            }

            var timeout = 1000;
            var req = "getposition";

            try {
                this.request_num++;
                var res = JSON.parse(e.data);

                if (res.type == "variables") {
                    console.log("WRONG ANSWER!")
                    this.close();
                }

                else if (res.type == "runways") {
                    MAP.processData(res);
                }

                else if (res.type == "parkings") {
                    MAP.processData(res);
                }

                else if (res.type == "flightplan" && res.data && res.data.length > 1) {
                    MAP.processData(res);
                }

                else if (res.type == "position") {
                    MAP.AirPlaneSetPos(res.data.PLANE_LATITUDE, res.data.PLANE_LONGITUDE, res.data.PLANE_HEADING_DEGREES_TRUE, 1);
                    MAP.getInfos();
                }

            }
            catch (err) {
                //ex_counter++;
                console.log('Exeption at MAP.ws.onmessage');
                console.log(err);
            }
            finally {

                if (req != "") {
                    setTimeout(function () {
                        MAP.ws.send(req);
                    }, 1000);
                }
            }
            //}

        };

        this.ws.onclose = function (e) {
            //document.getElementById("ws_status").style.display = "block";
            console.log('MAP Socket is closed. Reconnect will be attempted in 5 second.', e.reason);
            setTimeout(function () {
                MAP.connect();
            }, 5000);
        };

        this.ws.onerror = function (err) {
            //document.getElementById("ws_status").style.display = "block";
            var wstatus = document.getElementById("ws_status");
            if (wstatus) {
                wstatus.style.display = "none";
            }

            //console.error('Socket encountered error! Closing socket!');
            MAP.ws.close();
        };
    }

}