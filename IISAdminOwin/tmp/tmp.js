/**
 * Created by Artemchuk on 14.03.2015.
 */
define("IFrameResourceInjector", ["ext-base", "jQuery2", "underscore"], function (Ext, $, _) {
    "use strict";
    /**
     * @class Terrasoft.controls.IFrameResourceInjector
     * Миксин IFrameResourceInjector предназначен для подготовки и внедрения js/css ресурсов в iFrame.
     */
    var injector = Ext.define("Terrasoft.configuration.mixins.IFrameResourceInjector", {
        alternateClassName: "Terrasoft.IFrameResourceInjector",

        getIFrameDom: Ext.emptyFn,

        siteOrigin: "",
        modulesToInject: [],

        convertToText: function (obj) {
            var string = [];
            if (obj instanceof Array) {
                string.push("[");
                for (var itemIndex in obj) {
                    if (!obj.hasOwnProperty(itemIndex)) { continue; }
                    string.push(this.convertToText(obj[itemIndex]), ",");
                }
                string.push("]");
            }
            else if (typeof obj === "object") {
                string.push("{");
                for (var propertyName in obj) {
                    if (!obj.hasOwnProperty(propertyName)) { continue; }
                    string.push(propertyName, ": ", this.convertToText(obj[propertyName]), ",");
                }
                string.push("}");
            } else if (typeof(obj) === "function") {
                string.push(obj.toString());
            } else {
                string.push(JSON.stringify(obj));
            }

            return string.join("");
        },

        getJsModuleSources: function (modules, moduleNames) {
            var js = [];
            for (var i = 0; i < modules.length; i++) {
                js.push({
                    name: moduleNames[i],
                    src: this.convertToText(modules[i])
                });
            }
            return js;
        },

        getModuleCssHref: function (descriptor) {
            if (descriptor && descriptor.path && descriptor.css instanceof Array){
                var name = descriptor.css.pop();
                return {
                    name: name,
                    path: descriptor.path + "/" + name + ".css"
                };
            }
        },

        getCssSrc: function (styleSheet) {
            var rules = styleSheet.cssRules || styleSheet.rules;
            return _(rules).map(function (rule) {
                return rule && rule.cssText;
            }).join();
        },

        getCssSources: function (moduleDescriptors) {
            var me = this;
            var cssHRefs = _(moduleDescriptors).map(this.getModuleCssHref);
            var cssRes = [];
            _(document.styleSheets).each(function (sheet) {
                if (sheet && sheet.href && sheet.href.indexOf){
                    var hRef = _(cssHRefs).find(function (h) {
                        return sheet.href.indexOf(h.path) >- 1;
                    });
                    if (hRef){
                        cssHRefs = _(cssHRefs).without(hRef);
                        var src = me.getCssSrc.call(me, sheet);
                        cssRes.push({
                            name: hRef.name,
                            src: src
                        });
                    }
                }
            });
            return cssRes;
        },

        getValidModuleNames: function () {
            var re = new RegExp(/[a-zA-Z_$][0-9a-zA-Z_$]*/);
            var moduleNames = _(this.modulesToInject).filter(function (item) {
                var res = re.test(item);
                if (!res) {
                    Ext.global.console.error("Invalid module name to inject: " + item);
                }
                return res;
            });
            return moduleNames;
        },

        getModulesData: function (callback) {
            if (this.modulesData){
                callback.call(this);
            }
            var me = this;
            var requireNames = ["core"];
            var moduleNames = this.getValidModuleNames();
            requireNames = _(requireNames).union(moduleNames);
            requireNames = _(requireNames).union(_(moduleNames).map(function (module) {
                return "css!" + module;
            }));
            require(requireNames, function () {
                var core = requireNames[0];
                var modules = _(arguments).slice(1, moduleNames.length + 1);
                var js = me.getJsModuleSources.call(me, modules, moduleNames);
                var moduleDescriptors = _(moduleNames).each(core.getModuleDescriptor);
                var css = me.getCssSources.call(me, moduleDescriptors);
                me.modulesData = {js: js, css: css};
                callback.call(me);
            });
        },

        injectModules: function () {
            var iFrame = this.getIFrameDom();
            if (!iFrame || !iFrame.contentWindow || !iFrame.contentWindow.postMessage) {
                Ext.global.console.error("Can't find IFrame window");
                return;
            }
            this.getModulesData(function () {
                var msgData = {
                    senderName: "BpmOnlineAppResourcesInjector",
                    action: "LoadModules",
                    js: this.modulesData.js,
                    css: this.modulesData.css
                };
                iFrame.contentWindow.postMessage(msgData, this.siteOrigin);
            });
        },

        tryInjectModules: function () {
            if (this.modulesToInject && this.modulesToInject.length) {
                this.injectModules();
            }
        },

        setModulesToInject: function (value) {
            if (value instanceof Array) {
                this.modulesToInject = value;
            }
        },

        getBindConfig: function () {
            return {
                modulesToInject: {
                    changeMethod: "setModulesToInject"
                }
            };
        },

        updateSiteOrigin: function () {
            var url = new URL(this.siteUrl);
            this.siteOrigin = url.origin;
        },

        catchEvent: function (event) {
            var realEvent = event || window.event;
            if (realEvent.stopPropagation) {
                realEvent.stopPropagation()
            } else {
                realEvent.cancelBubble = true
            }
        },
        receiveMessage: function (e) {
            var data = e.data;
            if (data.origin === this.siteOrigin && data.senderName === "BpmOnlineIFrameResourcesInjector") {
                this.catchEvent(e);
                if (data.action === "Ready") {
                    this.tryInjectModules();
                }
            }
        },

        init: function () {
            var me = this;
            window.addEventListener("message", function (e) {
                me.receiveMessage.call(me, e);
            }, true);
        },

        dispose: function () {
            var me = this;
            window.removeEventListener("message", function (e) {
                me.receiveMessage.call(me, e);
            });
        }
    });

    return Ext.create(injector);
});
