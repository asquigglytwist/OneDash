function onBeforePrint() {
    toggleExpansionOfDetails(true);
}

function onAfterPrint() {
    toggleExpansionOfDetails(false);
    onInit();
}

function onInit() {
    var ndCurrentProducts = document.getElementById("CurrentProducts").getElementsByTagName("details");
    if (ndCurrentProducts.length > 0) {
        var firstProduct = ndCurrentProducts[0];
        firstProduct.setAttribute("open", "open");
        var ndVersions = firstProduct.getElementsByTagName("details");
        if (ndVersions.length > 0) {
            var firstVersion = ndVersions[0];
            firstVersion.setAttribute("open", "open");
            var ndReleases = firstVersion.getElementsByTagName("details");
            if (ndReleases.length > 0) {
                var firstRelease = ndReleases[0];
                firstRelease.setAttribute("open", "open");
            }
        }
    }
}

function getAllProductDetailsNodes() {
    return document.getElementById("CurrentProducts").getElementsByTagName("details");
}

function toggleExpansionOfDetails(expand) {
    var ndAllDetails = document.getElementsByTagName("details"), len = ndAllDetails.length;
    if (expand) {
        for (var i = 0; i < len; i++) {
            ndAllDetails[i].setAttribute("open", "open");
        }
    }
    else {
        for (var i = 0; i < len; i++) {
            ndAllDetails[i].removeAttribute("open");
        }
    }
}

window.onload = onInit;
window.onbeforeprint = onBeforePrint;
window.onafterprint = onAfterPrint;
