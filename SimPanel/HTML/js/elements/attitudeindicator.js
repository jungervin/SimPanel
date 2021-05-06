class AttitudeIndicator extends SVG.Svg {
    constructor(width, height, cx, cy) {
        super();

        this.cx = cx;
        this.cy = cy;

        this.width = width;
        this.height = height;

        this.pBank = 0;
        this.pP1 = 0
        this.pP2 = 0
        this.pFDBank = 0;
        this.prevFD_PITCH = 0;
        this.prevFD_PITCHX = 0;

        this.prevClipX = 0
        this.pPitch = 0
        this.prevClipRot = 0;


        this.r0 = 200;
        this.r1 = 220;

        this.Horizone = this.rect(this.width * 1.5, this.height * 1.5).move(this.cx - this.width / 2, this.cy).fill('#804000').stroke({ width: 5, color: 'white' });
        this.PitchGroup = this.group();
        this.BankGroup = this.group();

        for (var i = -60; i <= 60; i += 10) {

            if (i % 30 == 0 && i != 0) {
                this.BankGroup.add(this.drawClockFace(this.cx, this.cy, this.r0, this.r1 + 20, -90 + i).stroke({ width: 5, color: 'white' }));
            }
            else {
                this.BankGroup.add(this.drawClockFace(this.cx, this.cy, this.r0, this.r1, -90 + i).stroke({ width: 3, color: 'white' }));
            }
        }

        var triangle = this.polygon('-16,0 0,-24 16,0').fill('white').stroke({ width: 2, color: 'white' }); //.move(this.left + 12, this.centerY - 40);
        triangle.move(this.cx - triangle.bbox().w / 2, this.cy - this.r0 + 5);

        triangle = this.BankGroup.polygon('-16,0 0,24 16,0').fill('white').stroke({ width: 2, color: 'white' }); //.move(this.left + 12, this.centerY - 40);
        triangle.move(this.cx - triangle.bbox().width / 2, this.cy - this.r0 - triangle.bbox().height);

        var w = this.r1 * 1.2;
        var h = this.r1 * 1.4;
        this.step = 15;
        var font1 = { fill: 'white', family: 'consolas', weight: 'bold', size: 18, }
        for (var i = -90; i <= 90; i += 2.5) {
            var y = this.cy - i * this.step;

            if (i % 5 == 0) {
                this.PitchGroup.line(this.cx - w / 5, y, this.cx + w / 5, y).stroke({ width: 2, color: 'white' });

                if (i != 0) {
                    var t = this.PitchGroup.text(i.toString()).font(font1);

                    t.center(this.cx - w / 5 - t.bbox().w, y);
                    t = this.PitchGroup.text(i.toString()).font(font1);

                    t.center(this.cx + w / 5 + t.bbox().width, y);
                }

            }
            else {
                this.PitchGroup.line(this.cx - w / 8, y, this.cx + w / 8, y).stroke({ width: 2, color: 'white' });
            }
        }

        //        this.PitchGroupClip = this.PitchGroup.rect(w, w - 20).move(this.cx - w / 2, this.cy - h / 2).fill('#80400010').stroke({ width: 1, color: 'white' });
        this.PitchGroupClip = this.rect(w, w - 20).center(this.cx, this.cy).fill('#80400010').stroke({ width: 1, color: 'white' });
        this.PitchGroupClip.translate(0, -30)

        this.PitchGroup.clipWith(this.PitchGroupClip)


        // FLIGHT DIRECTOR
        this.FDGroup = this.group();
        var fd1 = this.FDGroup.polygon('-60,0 -60,-10 70,-34 -15,0').fill('#b300b3').stroke({ width: 0.5, color: 'black' });
        fd1.move(this.cx - fd1.bbox().w - 2, this.cy);
        var fd2 = this.FDGroup.polygon('15,0 -70,-34 60,-10 60,0').fill('#b300b3').stroke({ width: 0.5, color: 'black' });
        fd2.move(this.cx + 2, this.cy);


        // CENTER
        var t1 = this.polygon('-20,0 70,-34 15,0').fill('yellow').stroke({ width: 1, color: 'black' });
        t1.move(this.cx - t1.bbox().w - 2, this.cy);
        var t2 = this.polygon('-20,0 -70,-34 15,0').fill('yellow').stroke({ width: 1, color: 'black' });
        t2.move(this.cx + 2, this.cy);

        // HORIZONE
        var pl1 = this.polygon('0,0 0,-10 50,-10 60,0 50,10 0,10').fill('yellow').stroke({ width: 2, color: 'black' });
        pl1.center(this.cx - 220, this.cy);

        var pl2 = this.polygon('0,0 10,-10 60,-10 60,10 10,10 ').fill('yellow').stroke({ width: 2, color: 'black' });
        pl2.center(this.cx + 220, this.cy);

        let rad = Math.PI / 180;
        let bank = 50 * rad
        let pitch = 10 * rad
        this.update2(bank, pitch, false, 0, 0)

        bank = -50 * rad
        pitch = -10 * rad
        this.update2(bank, pitch, false, 0, 0)
    }


    update(data) {
        this.update2(
            data.PLANE_BANK_DEGREES,
            -data.PLANE_PITCH_DEGREES,
            data.AUTOPILOT_FLIGHT_DIRECTOR_ACTIVE,
            data.AUTOPILOT_FLIGHT_DIRECTOR_BANK,
            data.AUTOPILOT_FLIGHT_DIRECTOR_PITCH
        );
    }

    update2(bank, pitch, fd_on, fd_bank, fd_pitch) {
        fd_on = 1
        let rad = 180 / Math.PI;

        pitch = rad * pitch * this.step;
        if (Math.abs(pitch - this.pPitch) > 0.25) {

            let p1 = Math.cos(bank + Math.PI / 2) * pitch;
            let p2 = Math.sin(bank + Math.PI / 2) * pitch;
            this.PitchGroup.translate(p1 - this.pP1, p2 - this.pP2)
            this.Horizone.translate(p1 - this.pP1, p2 - this.pP2);
            this.PitchGroupClip.translate(0, -pitch + this.pPitch)
            this.pPitch = pitch
            this.pP1 = p1
            this.pP2 = p2
        }

        bank = rad * bank;
        if (Math.abs(bank - this.pBank) > 0.25) {
            this.Horizone.rotate(bank - this.pBank, this.cx, this.cy);
            this.BankGroup.rotate(bank - this.pBank, this.cx, this.cy);
            this.PitchGroup.rotate(bank - this.pBank, this.cx, this.cy);
            this.pBank = bank;
        }

        // FD
        if (this.prevFD_On != fd_on) {
            if (fd_on) {
                this.FDGroup.show();
            }
            else {
                this.FDGroup.hide();
            }
            this.prevFD_On != fd_on;
        }

        if (fd_on) {
            fd_bank = rad * -fd_bank + bank;
            this.FDGroup.rotate(fd_bank - this.pFDBank);
            this.pFDBank = fd_bank;

            fd_pitch = pitch - rad * -fd_pitch * this.step;
            //fd_pitch = pitch - fd_pitch;
            this.FDGroup.translate(0, fd_pitch - this.prevFD_PITCH);
            this.prevFD_PITCH = fd_pitch;
        }
    }

    // getLine(x1, y1, x2, y2) {
    //     // (x- p1X) / (p2X - p1X) = (y - p1Y) / (p2Y - p1Y) 
    //     var a = y1 - y2;
    //     var b = x2 - x1;
    //     var c = x1 * y2 - x2 * y1;
    //     return { a: a, b: b, c: c };
    // }
    // getDistance(p1, p2, x1, y1, x2, y2) {
    //     //var a, b, c;
    //     var line = this.getLine(x1, y1, x2, y2);
    //     return (line.a * p1 + line.b * p2 + line.c) / Math.sqrt(line.a * line.a + line.b * line.b);
    //     return Math.abs(line.a * p1 + line.b * p2 + line.c) / Math.sqrt(line.a * line.a + line.b * line.b);
    // }

    drawClockFace(x, y, r1, r2, deg) {
        var r = Math.PI / 180.0 * deg;
        var p1 = Math.cos(r) * r1;
        var p2 = Math.sin(r) * r1;
        var p3 = Math.cos(r) * r2;
        var p4 = Math.sin(r) * r2;
        return this.line(x + p1, y + p2, x + p3, y + p4);
    }

}