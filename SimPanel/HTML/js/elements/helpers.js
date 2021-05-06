class PopupPanel {
    constructor(idname, x, y, w, h, withCover = true) {

        this.fnclose = null;
        this.cover = null
        if (withCover) {
            
            
            let sp = document.getElementById("bottom_layer")
            this.cover = this.createLayer("cover", 0, 0, sp.style.width.replace("px", ""), sp.style.height.replace("px", ""));
            
            this.cover.style.backgroundColor = "#00000080"
            document.body.appendChild(this.cover)
            this.cover.popup = this;
            this.cover.addEventListener("click", function (e) {
                cover.popup.close()
            })
            this.cover.style.zIndex = getZIndex() + 1
        }


       
        this.layer = this.createLayer(idname, x, y, w, h);
        document.body.appendChild(this.layer)
        this.layer.style.zIndex = getZIndex() + 1
        this.svg = SVG().addTo("#" + idname).size(w, h)
    }

    createLayer(idname, x, y, w, h) {
        if (x == -1) {
            let sp = document.getElementById("bottom_layer")
            x = ( sp.style.width.replace("px", "") - w) / 2;
            y = ( sp.style.height.replace("px", "") - h) / 2;
        }

        var l = document.createElement("div");
        l.setAttribute("id", idname);
        l.style.left = x + "px";
        l.style.top = y + "px";
        l.style.width = w + "px";
        l.style.height = h + "px";
        l.style.position = "absolute";
        return l;
    }

    
    close() {
        if(this.cover != null) {
            this.cover.remove();
            delete this.cover
        }

        if(this.layer != null) {
            this.layer.remove()
            delete this.layer
        }

        delete this
    }

    onclose(func) {
        this.fnclose = func;
        return this;
    }

  
}
