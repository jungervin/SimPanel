class WSClient {
    constructor(url, query, timeout, onrec, onerr) {
        this.url = url;
        this.query = query;
        this.timeout = timeout;
        this.onrec = onrec;
        this.onerr = onerr;
        this.request_num = 0;
        this.render_sum = 0;
        this.render_cnt = 0;
        this.ex_counter = 0;
        this.ws = null;
    }
    connect() {
        const self = this;
        this.ws = new WebSocket(this.url);
        this.ws.onopen = function () {
            //self.ws.send(JSON.stringify(self.query));
            self.ws.send(self.query);
        };

        this.ws.onmessage = function (e) {
            // var timeout = this.timeout;
            // var req = "getvariables";

            //if (e.data) {
            try {
                var t = new Date();
                self.request_num++;

                queueMicrotask(function () {
                    var res = JSON.parse(e.data);

                    self.onrec(res);
                    self.render_sum += (new Date() - t);
                    self.render_cnt++;
                });
                // setTimeout(function () {
                //     var res = JSON.parse(e.data);

                //     self.onrec(res);
                //     self.render_sum += (new Date() - t);
                //     self.render_cnt++;
                // }, 1);
            }
            catch (err) {
                self.ex_counter++;
                console.log('Exeption at ws.onmessage');
                console.log(err);
            }
            finally {
                if (self.request_num % 10 == 0) {
                    var t2 = new Date();
                    var dt = (t2 - self.t1) / 10;
                    self.t1 = t2;
                    var rt = (self.render_sum / self.render_cnt);
                    // self.render_sum = __PFD.render_cnt = 0;
                    // self.panel_bottom.FPS.text((1000 / dt).toFixed(1) + " " + dt.toFixed(1) + 'ms Ex:' + __PFD.ex_counter + " Pars+Render: " + rt + "ms");

                }

                setTimeout(function () {
                    //self.ws.send(JSON.stringify(self.query));
                    self.ws.send(self.query);
                }, self.timeout);
            }
        };

        this.ws.onclose = function (e) {
            //document.getElementById("ws_status").style.display = "block";
            console.log('Socket is closed. Reconnect will be attempted in 5 second.', e.reason);
            setTimeout(function () {
                self.connect();
            }, 5000);
        };

        this.ws.onerror = function (err) {
            self.ws.close();
            self.onerr(err);
        };
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

}