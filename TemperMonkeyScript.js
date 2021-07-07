// ==UserScript==
// @name         SyncCookieForm115AndJavLibrary
// @namespace    http://tampermonkey.net/
// @version      0.1
// @description  try to take over the world!
// @author       You
// @match        https://*.115.com/*
// @match        http://*.javlibrary.com/cn/*
// @icon         https://www.google.com/s2/favicons?domain=115.com
// @grant        GM_xmlhttpRequest
// @connect      http://www.cainqs.com:8087/avapi/Save115Cookie
// @connect      http://www.cainqs.com:20001/api/JavLibraryApi/SaveJavlibraryCookie
// ==/UserScript==

(function () {
    'use strict';

    GM_xmlhttpRequest({
        url: "http://www.cainqs.com:20001/api/OneOneFive/SaveOneOneFiveCookie?cookie=" + document.cookie + "&userAgent=" + navigator.userAgent,
        method: "POST",
        headers: {
            "Content-type": "application/x-www-form-urlencoded"
        },
        onload: function (xhr) {
            console.log("Core =====> " + xhr.responseText);
        },
        onerror: function (data) {
            console.log("Core =====> " + data);
        }
    });

    GM_xmlhttpRequest({
        url: "http://www.cainqs.com:20001/api/JavLibraryApi/SaveJavlibraryCookie?cookie=" + document.cookie + "&userAgent=" + navigator.userAgent,
        method: "POST",
        headers: {
            "Content-type": "application/x-www-form-urlencoded"
        },
        onload: function (xhr) {
            console.log("Core =====> " + xhr.responseText);
        },
        onerror: function (data) {
            console.log("Core =====> " + data);
        }
    });
})();

