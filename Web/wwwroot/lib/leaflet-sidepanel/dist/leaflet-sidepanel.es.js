import * as t from "leaflet";
class d extends t.Control {
  // Optional 'cause it's initialized in `addTo`
  constructor(i, e) {
    if (e = e || {}, Object.keys(e).forEach((o) => {
      e[o] === void 0 && delete e[o];
    }), e.position) {
      const o = "Leaflet.SidePanel: You cannot set the `position` option. It is controlled by the `panelPosition` option.";
      e.panelPosition ? console.warn(o) : console.error(o);
    }
    e.panelPosition && !["left", "right"].includes(e.panelPosition) && (console.error(
      'Leaflet.SidePanel: The `panelPosition` option must be either "left" or "right".'
    ), e.panelPosition = "left"), e.tabsPosition && !["top", "bottom", "left", "right"].includes(e.tabsPosition) && (console.error(
      'Leaflet.SidePanel: The `tabsPosition` option must be either "top", "bottom", "left", or "right".'
    ), e.tabsPosition = "top"), super(e), this.options = {
      panelPosition: "left",
      hasTabs: !0,
      tabsPosition: "top",
      darkMode: !1,
      pushControls: !1,
      defaultTab: 1,
      size: "400px",
      onTabClick: () => {
      },
      onToggle: () => {
      },
      ...e
      // Merge with default options
    }, this._panel = t.DomUtil.get(i), this._panel.style.setProperty("--panel-width", this.options.size), t.setOptions(this, e);
  }
  addTo(i) {
    if (this._map = i, t.DomUtil.addClass(this._panel, "sidepanel-" + this.options.panelPosition), this.options.darkMode && t.DomUtil.addClass(this._panel, "sidepanel-dark"), t.DomEvent.disableScrollPropagation(this._panel), t.DomEvent.disableClickPropagation(this._panel), this.options.pushControls) {
      const e = this._map, s = (e instanceof t.Map ? e.getContainer() : e).querySelector(
        ".leaflet-control-container"
      );
      t.DomUtil.addClass(
        s,
        "leaflet-animate-control-container"
      ), s.style.setProperty(
        `--panel-width-${this.options.panelPosition}`,
        this.options.size
      );
    }
    return this.options.hasTabs ? this._initTabs(this.options.tabsPosition) : this._initContent(), this;
  }
  // Define the private methods as part of the class
  _initContent() {
    this._toggleButton();
  }
  _initTabs(i) {
    t.DomUtil.addClass(this._panel, "tabs-" + i);
    const e = this._panel.querySelectorAll(
      "a.sidebar-tab-link"
    ), o = this._panel.querySelectorAll(
      ".sidepanel-tab-content"
    );
    e.forEach((s, a) => {
      let r = !1, n;
      typeof this.options.defaultTab == "number" && this.options.defaultTab - 1 === a && (r = !0, n = o[a]), typeof this.options.defaultTab == "string" && this.options.defaultTab === s.dataset.tabLink && (r = !0, n = this._panel.querySelector(
        `.sidepanel-tab-content[data-tab-content="${this.options.defaultTab}"]`
      )), r && (t.DomUtil.addClass(s, "active"), t.DomUtil.addClass(n, "active")), t.DomEvent.on(
        s,
        "click",
        (h) => {
          t.DomEvent.preventDefault(h), e.forEach((l) => t.DomUtil.removeClass(l, "active")), t.DomUtil.addClass(s, "active"), o.forEach((l) => {
            s.dataset.tabLink === l.dataset.tabContent ? t.DomUtil.addClass(l, "active") : t.DomUtil.removeClass(l, "active");
          }), this.options.onTabClick(s);
        },
        s
      );
    }), this._toggleButton();
  }
  toggle(i) {
    let e = !0;
    const o = t.DomUtil.hasClass(this._panel, "opened"), s = t.DomUtil.hasClass(this._panel, "closed");
    if (!o && !s ? t.DomUtil.addClass(this._panel, "opened") : !o && s ? (t.DomUtil.addClass(this._panel, "opened"), t.DomUtil.removeClass(this._panel, "closed")) : o && !s && (e = !1, t.DomUtil.removeClass(this._panel, "opened"), t.DomUtil.addClass(this._panel, "closed")), this.options.pushControls) {
      const a = this._map;
      if (a) {
        const n = (a instanceof t.Map ? a.getContainer() : a).querySelector(
          ".leaflet-control-container"
        );
        e ? (t.DomUtil.removeClass(
          n,
          this.options.panelPosition + "-closed"
        ), t.DomUtil.addClass(
          n,
          this.options.panelPosition + "-opened"
        )) : (t.DomUtil.removeClass(
          n,
          this.options.panelPosition + "-opened"
        ), t.DomUtil.addClass(
          n,
          this.options.panelPosition + "-closed"
        ));
      } else
        console.error(
          "Leaflet.SidePanel: You must add the SidePanel to the map before setting the `pushControls` option."
        );
    }
    this.options.onToggle(e);
  }
  isOpened() {
    return t.DomUtil.hasClass(this._panel, "opened");
  }
  open() {
    this.isOpened() || this.toggle();
  }
  close() {
    this.isOpened() && this.toggle();
  }
  _toggleButton() {
    const i = this._panel.querySelector(
      ".sidepanel-toggle-container"
    ), e = i.querySelector(
      ".sidepanel-toggle-button"
    );
    t.DomEvent.on(
      e,
      "click",
      (o) => {
        this.toggle(o);
      },
      i
    );
  }
}
t.Control.SidePanel = d;
t.control.sidepanel = function(p, i) {
  return new d(p, i);
};
