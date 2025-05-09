var cookieUtils=function(e){var o="object"==typeof self&&self.self===self&&self||"object"==typeof global&&global.global===global&&global||void 0,n="undefined";function t(e){return o[e]}function i(e,n){return o[e]=n,n}var r=u(),c=l;function f(e){return r?l(e,"",-1):function(e){i(e)}(e)}function u(){if(typeof r!==n)return r;try{l(n,"1"),r=-1!==document.cookie.indexOf(n),f(n)}catch(e){r=!1}return r}function l(e,o,c,f,u,l){if(typeof window!==n){var a=arguments.length>1;return r||(a?i(e,o):t(e)),a?document.cookie=e+"="+encodeURIComponent(o)+(c?"; expires="+new Date(+new Date+1e3*c).toUTCString()+(f?"; path="+f:"")+(u?"; domain="+u:"")+(l?"; secure":""):""):decodeURIComponent((("; "+document.cookie).split("; "+e+"=")[1]||"").split(";")[0])}}return e.getCookie=l,e.hasCookies=u,e.removeCookie=f,e.setCookie=c,e}({});
var storageUtils=function(e){var o="object"==typeof self&&self.self===self&&self||"object"==typeof global&&global.global===global&&global||void 0,t="undefined";function r(e){return o[e]}function n(e,t){return o[e]=t,t}function a(e){n(e)}var l=f(),c=g,i=g;function u(e){return l?g(e,"",-1):a(e)}function f(){if(typeof l!==t)return l;try{g(t,"1"),l=-1!==document.cookie.indexOf(t),u(t)}catch(e){l=!1}return l}function g(e,o,a,c,i,u){if(typeof window!==t){var f=arguments.length>1;return l||(f?n(e,o):r(e)),f?document.cookie=e+"="+encodeURIComponent(o)+(a?"; expires="+new Date(+new Date+1e3*a).toUTCString()+(c?"; path="+c:"")+(i?"; domain="+i:"")+(u?"; secure":""):""):decodeURIComponent((("; "+document.cookie).split("; "+e+"=")[1]||"").split(";")[0])}}var s=v();function v(){if(typeof s!==t)return s;s=!0;try{typeof localStorage!==t&&typeof JSON!==t||(s=!1),localStorage.setItem(t,t),localStorage.removeItem(t)}catch(e){s=!1}return s}function p(e){return void 0===e}function m(e){var o=e;try{if("true"===(o=JSON.parse(e)))return!0;if("false"===o)return!1;if(function(e){if("object"!=typeof e||null===e)return!1;for(var o=e;null!==Object.getPrototypeOf(o);)o=Object.getPrototypeOf(o);return Object.getPrototypeOf(e)===o}(o))return o;parseFloat(o)===o&&(o=parseFloat(o))}catch(e){}if(null!==o&&""!==o)return o}var S="any",b="localStorage",y="cookie",d="global",I=v(),O=f();function k(e,o){if(e){var t=L(o),n=!w(t),a=j(t)?m(localStorage.getItem(e)):void 0;if(n&&!p(a))return a;var l=A(t)?m(c(e)):void 0;if(n&&l)return l;var i=r(e);return n?i:{localStorage:a,cookie:l,global:i}}}function C(e,o,t){if(e&&!p(o)){var a={},l=L(t),u=JSON.stringify(o),f=!w(l);return j(l)&&(a.localStorage={location:b,current:o,previous:m(localStorage.getItem(e))},localStorage.setItem(e,u),f)?a.localStorage:A(l)&&(a.cookie={location:y,current:o,previous:m(c(e))},i(e,u),f)?a.cookie:(a.global={location:d,current:o,previous:r(e)},n(e,o),f?a.global:a)}}function h(e,o){if(e){var t=L(o),r=k(e,"*"),n={};return!p(r.localStorage)&&j(t)&&(localStorage.removeItem(e),n.localStorage=r.localStorage),!p(r.cookie)&&A(t)&&(u(e),n.cookie=r.cookie),!p(r.global)&&N(t,d)&&(a(e),n.global=r.global),n}}function L(e){return e?"string"==typeof e?e:e.storage:S}function j(e){return I&&N(e,b)}function A(e){return O&&N(e,y)}function w(e){return"*"===e||"all"===e}function N(e,o){return e===S||e===o||w(e)}var x={setItem:C,getItem:k,removeItem:h};return e.ALL="*",e.ANY=S,e.COOKIE=y,e.GLOBAL=d,e.LOCAL_STORAGE=b,e.default=x,e.getCookie=c,e.getItem=k,e.globalContext=o,e.hasCookies=f,e.hasLocalStorage=v,e.removeCookie=u,e.removeItem=h,e.setCookie=i,e.setItem=C,e}({});

var analyticsIndicative = (function (cookieUtils, storageUtils) {
  'use strict';

  function ownKeys(object, enumerableOnly) {
    var keys = Object.keys(object);

    if (Object.getOwnPropertySymbols) {
      var symbols = Object.getOwnPropertySymbols(object);

      if (enumerableOnly) {
        symbols = symbols.filter(function (sym) {
          return Object.getOwnPropertyDescriptor(object, sym).enumerable;
        });
      }

      keys.push.apply(keys, symbols);
    }

    return keys;
  }

  function _objectSpread2(target) {
    for (var i = 1; i < arguments.length; i++) {
      var source = arguments[i] != null ? arguments[i] : {};

      if (i % 2) {
        ownKeys(Object(source), true).forEach(function (key) {
          _defineProperty(target, key, source[key]);
        });
      } else if (Object.getOwnPropertyDescriptors) {
        Object.defineProperties(target, Object.getOwnPropertyDescriptors(source));
      } else {
        ownKeys(Object(source)).forEach(function (key) {
          Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key));
        });
      }
    }

    return target;
  }

  function _defineProperty(obj, key, value) {
    if (key in obj) {
      Object.defineProperty(obj, key, {
        value: value,
        enumerable: true,
        configurable: true,
        writable: true
      });
    } else {
      obj[key] = value;
    }

    return obj;
  }

  var PREFIX = '__';
  var USER_ID = PREFIX + 'user_id'; // __user_id

  var defaultConfig = {
    enabled: true,
    recordSessions: true,
    sessionsThreshold: 30,
    cookiesOnMainDomain: true
  };

  function indicative(pluginConfig) {
    var isInitialized = false;
    return {
      name: "indicative-analytics",
      config: _objectSpread2(_objectSpread2({}, defaultConfig), pluginConfig),
      initialize: function initialize(plugin) {
        var config = plugin.config,
            instance = plugin.instance;
        var apiKey = config.apiKey;
        var script = document.createElement("script");
        script.src = "//cdn.indicative.com/js/1.0.2/Indicative.min.js";
        script.type = "text/javascript";
        script.async = true;

        script.onload = script.onreadystatechange = function () {
          var rs = this.readyState;
          if (isInitialized || rs && rs != "complete" && rs != "loaded") return;
          isInitialized = true;
          window.Indicative.initialize(apiKey, config);
        };

        document.body.appendChild(script);
      },
      loaded: function loaded() {
        return Boolean(window["Indicative"]);
      },

      /* Sync id when ready */
      ready: function ready(_ref) {
        var instance = _ref.instance,
            config = _ref.config;
        var userId;

        if (cookieUtils.hasCookies()) {
          userId = cookieUtils.getCookie(USER_ID);

          if (!userId) {
            userId = storageUtils.getItem(USER_ID);

            if (!userId) {
              userId = instance.user('anonymousId');
            }
          }
        }

        if (userId) {
          cookieUtils.setCookie(USER_ID, userId);
          instance.identify(userId);
        }
      },
      identify: function identify(_ref2) {
        var payload = _ref2.payload,
            config = _ref2.config;
        // var payload = props.payload, config = props.config;
        cookieUtils.setCookie(USER_ID, payload.userId); // Indicative has NO identify() call in their JS file. Bug reported, for now just make the call manually
        // axios_1["default"]
        //     .post("https://api.indicative.com/service/alias/" + config.apiKey, {
        //     previousId: payload.anonymousId,
        //     newId: payload.userId
        // })
        //     .then(function () {
        //     axios_1["default"]
        //         .post("https://api.indicative.com/service/identify/" + config.apiKey, {
        //         uniqueId: payload.userId,
        //         properties: payload.traits
        //     })["catch"](console.error);
        // })["catch"](console.error);
      },
      // page: ({ payload, config })
      page: function page(_ref3) {
        var payload = _ref3.payload,
            config = _ref3.config,
            instance = _ref3.instance;
        // var payload = trackData.payload, config = trackData.config, instance = trackData.instance;
        var properties = payload.properties;
        window.Indicative.buildEvent("Page View", properties);
      },
      // Set parameter scope at event level with 'event' method
      track: function track(_ref4) {
        var payload = _ref4.payload,
            config = _ref4.config;
        // var payload = trackData.payload;
        var event = payload.event,
            properties = payload.properties;
        window.Indicative.buildEvent(event, properties);
      } // setCooomProps()

    };
  }

  /* This module will shake out unused code and work in browser and node 🎉 */

  var index =  indicative ;

  return index;

}(cookieUtils, storageUtils));
