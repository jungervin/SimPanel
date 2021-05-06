let SETTINGS = {
    version : "0.1",
    date : "2021-04-02",
    ws_sim : "ws://192.168.1.174:5000/sim",
    ws_variables: "ws://192.168.1.174:5000/variables",
    mfd_url:  window.location.origin + "/c172mfd.html",
    timeout: 30,
    animation : { duration: 150, delay: 0, when: "now", swing: true, times: 1, wait: 0 },

    airplanes : [
        {
            name: "Cessna 172 G1000",
            settings:
            {
                rpm_gauge: {green_start: 2000, green_end: 2700, max: 3000 },
                egt_gauge: {max : 2500, divider: 500},
                speeds : {v0: 45, v1: 130, v2: 165, f0: 45, f1: 85},
                vac_gauge: {min: 3.5, green_start: 4.5, green_end: 5.5, max: 7}
            }
        },
    ]
}