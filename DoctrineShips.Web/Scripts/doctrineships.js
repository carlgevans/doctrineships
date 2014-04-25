// ***********************************************
// Pass the function a string that should be the id 
// of the content div. The form being submitted should
// have the same name but with _form appended. The
// confirm button should have _confirm appended.
// ***********************************************
function confirmDialog(id) {
    // Remove the # from the id if it exists.
    id = id.replace("#", "");

    // Build the div, button and form ids.
    var divId = "#" + id;
    var buttonId = "#" + id + "_confirm";
    var formId = "form#" + id + "_form";

    // Attach a form submit click handler to the confirm button.
    $(buttonId).click(function() {
        $(formId).submit();
    });

    // Open the modal.
    $(divId).modal();
}

// ***********************************************
// Pass the function a string that should be the id 
// of the content div.
// ***********************************************
function warningDialog(id) {
    // Remove the # from the id if it exists.
    id = id.replace("#", "");

    // Build the div id.
    var divId = "#" + id;

    // Open the modal.
    $(divId).modal();
}

// ***********************************************
// Eve Functions & Browser Checks
// ***********************************************
function showContract(solarSystemId, contractId) {
    if (~navigator.userAgent.indexOf("EVE-IGB")) {
        CCPEVE.showContract(solarSystemId, contractId);
    }
    else {
        warningDialog('igbcontract');
    }
}

function showFitting(fittingString) {
    if (~navigator.userAgent.indexOf("EVE-IGB")) {
        CCPEVE.showFitting(fittingString);
    }
    else {
        warningDialog('igbfitting');
    }
}

function setDestination(destination) {
    if (~navigator.userAgent.indexOf("EVE-IGB")) {
        CCPEVE.setDestination(destination);
    }
    else {
        warningDialog('igbsetdest');
    }
}

function buyComponent(componentId) {
    if (~navigator.userAgent.indexOf("EVE-IGB")) {
        CCPEVE.showMarketDetails(componentId);
        setTimeout(function () { CCPEVE.buyType(componentId); }, 2000);
    }
    else {
        warningDialog('igbbuy');
    }
}

// ***********************************************
// Url shortener
// ***********************************************
function shortenUrl(urlToShorten) {
    // Show the generating progress bar.
    $("#shortenUrlResult").hide();
    $("#shortenUrlGenerating").show();

    // Open the modal.
    $("#shortenUrl").modal();

    // Encode the passed url.
    var encodedUrl = encodeURIComponent(urlToShorten);

    $.ajax({
        url: "/Tools/ShortenUrl",
        type: 'post',
        data: {
            'longUrl': encodedUrl
        },
        success: function (shortUrl) {
            $("#shortenUrlResult").html(shortUrl);
            $("#shortenUrlGenerating").hide();
            $("#shortenUrlResult").show();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $("#shortenUrlResult").html("An Error Occured: " + textStatus + " - " + errorThrown);
            $("#shortenUrlGenerating").hide();
            $("#shortenUrlResult").show();
        }
    });
}

// ***********************************************
// EFT Fitting
// ***********************************************
function showEftFitting(shipFitId) {
    $("#eftFittingResult").hide();
    $("#eftFittingLoading").show();

    // Open the modal.
    $("#eftFitting").modal();

    $.ajax({
        url: "/Api/EftFittingString",
        type: 'get',
        data: { id: shipFitId },
        success: function (eftFittingString) {
            eftFittingStringWithBreaks = eftFittingString.replace(/\r\n/g, '<br />');

            $("#eftFittingResult").html(eftFittingStringWithBreaks);
            $("#eftFittingLoading").hide();
            $("#eftFittingResult").show();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $("#eftFittingResult").html("An Error Occured: " + textStatus + " - " + errorThrown);
            $("#eftFittingLoading").hide();
            $("#eftFittingResult").show();
        }
    });
}

// ***********************************************
// Misc functions
// ***********************************************
function showFittingDenied() {
    warningDialog('fittingauth');
}