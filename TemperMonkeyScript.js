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
// @connect      *
// ==/UserScript==

(function () {
    'use strict';

    GM_xmlhttpRequest({
        url: "http://www.cainqs.com:20001/api/OneOneFive/SaveOneOneFiveCookie?cookie=" + document.cookie + "&userAgent=" + navigator.userAgent,
        method: "POST",
        synchronous: true,
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
        synchronous: true,
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


//Jav老司机添加以下
function getLocalAnd115Check(avId, div) {
    let promise1 = request(`http://localhost:20001/api/Everything/EverythingSearch?content=${avId}`);
    promise1.then((result) => {
        let resultJson = JSON.parse(result.responseText);
        if (resultJson.results.length > 0) {
            var first = resultJson.results[0];
            div.after("<span style='color:red;'>" + first.location + " " + first.sizeStr + " " + first.chinese + "</span>");
        } else {
            div.after("<span style='color:red;'>没有找到</span>");
        }
    });
}

//MT还要更改
//@connect *

function request(url, referrerStr, timeoutInt) {
    return new Promise((resolve, reject) => {
        console.log(`发起网址请求：${url}`);
        GM_xmlhttpRequest({
            url,
            method: 'GET',
            headers: {
                "Cache-Control": "no-cache",
                referrer: referrerStr
            },
            timeout: timeoutInt > 0 ? timeoutInt : 20000,
            onload: response => { //console.log(url + " reqTime:" + (new Date() - time1));
                response.loadstuts = true;
                resolve(response);
            },
            onabort: response => {
                console.log(url + " abort");
                response.loadstuts = false;
                resolve(response);
            },
            onerror: response => {
                console.log(url + " error");
                console.log(response);
                response.loadstuts = false;
                resolve(response);
            },
            ontimeout: response => {
                console.log(`${url} ${timeoutInt}ms timeout`);
                response.loadstuts = false;
                response.finalUrl = url;
                resolve(response);
            },
        });
    });
}