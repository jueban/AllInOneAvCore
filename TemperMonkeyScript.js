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
// @connect      /avapi/Save115Cookie
// @connect      /api/OneOneFive/SaveOneOneFiveCookie
// ==/UserScript==

(function() {
    'use strict';

    GM_xmlhttpRequest({
        url:"/avapi/Save115Cookie?cookie=" + document.cookie,
        method :"POST",
        headers: {
            "Content-type": "application/x-www-form-urlencoded"
        },
        onload:function(xhr){
            console.log(xhr.responseText);
        },
        onerror:function(data){
            console.log(data);
        }
    });

    GM_xmlhttpRequest({
        url:"/api/OneOneFive/SaveOneOneFiveCookie?cookie=" + document.cookie + "&userAgent=" + navigator.userAgent,
        method :"POST",
        headers: {
            "Content-type": "application/x-www-form-urlencoded"
        },
        onload:function(xhr){
            console.log("Core =====> " + xhr.responseText);
        },
        onerror:function(data){
            console.log("Core =====> " + data);
        }
    });
})();