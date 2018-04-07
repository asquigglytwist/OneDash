function onBeforePrint() {
    var ndAllDetails = document.getElementsByTagName("details"), len = ndAllDetails.length;
    for (var i = 0; i < len; i++) {
        ndAllDetails[i].setAttribute("open", "");
    }
}

function onAfterPrint() {
    var ndAllDetails = document.getElementsByTagName("details"), len = ndAllDetails.length;
    for (var i = 0; i < len; i++) {
        ndAllDetails[i].removeAttribute("open");
    }
    onInit();
}

function onInit() {
    var ndCurrentProducts = document.getElementById("CurrentProducts").getElementsByTagName("details");
    if (ndCurrentProducts.length > 0) {
        var firstProduct = ndCurrentProducts[0];
        firstProduct.setAttribute("open", "");
        var ndVersions = firstProduct.getElementsByTagName("details");
        if (ndVersions.length > 0) {
            var firstVersion = ndVersions[0];
            firstVersion.setAttribute("open", "");
            var ndReleases = firstVersion.getElementsByTagName("details");
            if (ndReleases.length > 0) {
                var firstRelease = ndReleases[0];
                firstRelease.setAttribute("open", "");
            }
        }
    }
}

window.onload = onInit;
window.onbeforeprint = onBeforePrint;
window.onafterprint = onAfterPrint;