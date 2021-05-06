class AirPlane {
    constructor() {

        const self = this;
        this.SimData = {};
        this.ws = null;
        // this.WSClient = new WSClient("ws://127.0.0.1:5000/sim",
        //     //{ cmd: "getvars", data: null },
        //     "getvariables",
        //     30,
        //     function (res) {
        //         self.update(res)
        //     },
        //     function (err) {

        //     });

        this.Controls = new Array();
        this._navmode = 3;
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
    }

    createLayer(x, y, w, h, id) {
        var l = document.createElement("div");
        l.setAttribute("id", id);
        l.style.left = x + "px";
        l.style.top = y + "px";
        l.style.width = w + "px";
        l.style.height = h + "px";
        l.style.position = "absolute";
        return l;
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
        const pfd = this;
        pfd.counter = 0;
        pfd.processed = 0;
        pfd.locked = false;
        pfd.ws = new WebSocket(SETTINGS.ws_variables);

        pfd.ws.onopen = function () {
            pfd.ws.send("HELLO");
            pfd.locked = false;
        };

        pfd.ws.onmessage = function (e) {

            if(e.target.url != SETTINGS.ws_variables) {
                return
            }
            pfd.counter++
            if (!pfd.locked && pfd.counter % 1 == 0) {
                pfd.locked = true;
                    var res = JSON.parse(e.data)
                    pfd.update(res)
                    pfd.locked = false
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

        pfd.ws.onclose = function (e) {
            console.log('Socket is closed. Reconnect will be attempted in 5 second.', e.reason);
            setTimeout(function () {
                pfd.connect();
            }, 5000);
        };

        pfd.ws.onerror = function (err) {
            pfd.ws.close();

        };

    }

}

