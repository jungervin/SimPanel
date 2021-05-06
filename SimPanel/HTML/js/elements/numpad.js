class NumPad extends PopupPanel {
    //  constructor(svg, title, format = "", callback = null) {
    constructor(title, format = "", validnums = "0123456789") {
        super("numpad", -1, -1, 355, 450)
        this.ValidNums = validnums;

        var svg = this.svg; // SVG().addTo('#panel').size(1366, 768)

        //this.svg = svg;
        var g = svg.group(); //this.svg2;

        var font = { 'fill': 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 28, leading: '1em' };

        var font2 = { 'fill': 'white', 'font-family': 'consolas', 'font-weight': 'bold', 'font-size': 48, leading: '1em' };

        g.rect(355, 450).fill("black").stroke({width: 3, color: "white"});
        this.display = g.rect(335, 54).move(10, 50).fill("gray");
        this.CursorRect = g.rect(28, 54).move(10, 50).fill("#00000060");

        //var btn1 = new ButtonView(owner)

        this.Format = format;
        this.Value = format;
        this.Pos = 0;
        g.text(title).font(font).move(10, 10);
        this.ValueText = g.text(this.Value).font(font2);
        this.ValueText.move(this.display.bbox().x2 - w, this.display.bbox().y - 3);

        let numpad = this;
        var b = ["7", "8", "9", "DEL", "4", "5", "6", "CLR", "1", "2", "3", "ESC", "", "0", "ENTER", ""];
        for (var i = 0; i < 4; i++) {
            for (var j = 0; j < 4; j++) {
                var c = b[i * 4 + j];
                if (c != "") {
                    var w = c == "ENTER" ? 140 : 60;
                    let e = this.ValidNums.includes(c) || c == "DEL" || c == "CLR" || c == "ESC" || c == "ENTER"
                    g.rect_button(w, 60, c, function (c) {

                        //var c = sender.text.text();
                        if (c == "ENTER" || c == "ESC") {
                            numpad.Result = c;
                            //window.CloseCurrentLayer(calc);
                            numpad.fnclose(numpad)
                            return;
                        };

                        numpad.process(c);

                    }, e).font(font).move(30 + j * 80, 130 + i * 80);
                }
            }
        }

        //g.move(550, 100);

        //g.center(svg.bbox().cx, svg.bbox().cy);
        this.update();
    }


    process(c) {
        switch (c) {
            case "DEL":
                this.Value = setCharAt(this.Value, this.Pos, '0');
                if (this.Pos > 0) {
                    this.Pos--;
                }
                if (this.Value[this.Pos] == ".") {
                    this.Pos--;

                }
                break;
            case "CLR":
                this.Value = this.Format;
                this.Pos = 0;
                break;
            default:
                if (this.ValidNums.includes(c)) {
                    this.Value = setCharAt(this.Value, this.Pos, c);
                    if (this.Pos < this.Format.length - 1) {
                        this.Pos++;
                        if (this.Value[this.Pos] == ".") {
                            this.Pos++;
                        }
                    }
                }
                break;
        }


        // }
        this.update();


    }
    update() {
        this.ValueText.text(this.Value);
        var w = this.ValueText.bbox().w + 20;
        this.ValueText.move(this.display.bbox().x2 - w);
        var s = this.ValueText.bbox().width / this.Format.length;
        this.CursorRect.x(this.display.bbox().x2 - (this.Format.length - this.Pos) * s - 20);

    }
}