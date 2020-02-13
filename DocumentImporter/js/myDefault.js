

//Load From File button click
function loadFromFile() {
    DocuViewareAPI.LoadFromFile("docuView"
        , updateToolbarUI
        , updateToolbarUI
    );
}

//Load From URI button click
function loadFromUri() {
    $("#document_uri_area").slideUp();
    var uri = $('input[name="document_uri"]').val();
    if (uri != null) {
        DocuViewareAPI.LoadFromUri("docuView"
            , uri
            , updateToolbarUI
            , updateToolbarUI
        );
    }
}

// TWAIN Acquisition
function acquireTwain() {
    // Open the Scan dialog box
 
    /*
    var e = document.getElementById('docuView');
    if (e) {
        e.control.TwainAcquisitionDialog_Click();
    }*/

    DocuViewareAPI.TwainAcquire("docuView");
}

//Previous Page button click
function previousPage() {
    changePage(parseInt($('input[name="current_page"]').val()) - 1);
}

//Next PAge button click
function nextPage() {
    changePage(parseInt($('input[name="current_page"]').val()) + 1);
}

//Change Page # button click
function changePage(nb) {
    var pageCount = DocuViewareAPI.GetPageCount("docuView");
    if (nb < 1) {
        nb = 1;
    }
    if (nb > pageCount) {
        nb = pageCount;
    }
    if (nb != 0) {
        DocuViewareAPI.SelectPage("docuView", nb);
    }
}

//Zoom In button click
function zoomIn() {
    DocuViewareAPI.ZoomIN("docuView");
}

//Zoom Out button click
function zoomOut() {
    DocuViewareAPI.ZoomOUT("docuView");
}

// Rotate -90 or Rotate +90 button clicked
function applyRotate(actionName) {
    if (actionName) {
        var currentPage = DocuViewareAPI.GetCurrentPage("docuView");

        var params = { currentPage: currentPage };
        // Fire Rotate event to the Server
        DocuViewareAPI.PostCustomServerAction("docuView", true, actionName, params);
    }
    return false;
}

// Save the File
function saveFile() {
    DocuViewareAPI.Save("docuView", function () { alert("Saved Successfully"); },
        function () { alert("Failed to SAve "); });
    //DocuViewareAPI.PostCustomServerAction("docuView", true, "save");
    
}

// Print the File
function printFile() {
    DocuViewareAPI.Print("docuView", function () { alert("Printed Successfully"); },
        function () { alert("Failed to Print "); });
}

function removePage() {


    // If File Type is Tiff or PDF, then proceed for Delete process
    //if (DocuViewareAPI.GetDocumentType("docuView") == DocumentType.DocumentTypeBitmap ||
    //    DocuViewareAPI.GetDocumentType("docuView") == DocumentType.DocumentTypePDF) {

        var pageCount = DocuViewareAPI.GetPageCount("docuView");
        var currentPage = DocuViewareAPI.GetCurrentPage("docuView");

        if (pageCount == 0 || pageCount == 1) {
            alert("Sorry, Doc should contain more than 1 PAge to perform Deletion !!! ")
        } else {

            alert("Remove Page # : " + currentPage);

            var params = { currentPage: currentPage };
            DocuViewareAPI.PostCustomServerAction("docuView", true, "RemovePage", params);
        }
    //} else {
    //    alert("Only TIFF and/or PDF files Supported Docu Type = " + DocuViewareAPI.GetDocumentType("docuView"));
    //}
}

function updateToolbarUI() {
    var pageCount = DocuViewareAPI.GetPageCount("docuView");
    var currentPage = DocuViewareAPI.GetCurrentPage("docuView");
    $("#sp_page_count").html(pageCount);
    $('input[name="current_page"]').val(currentPage);
    if (pageCount === 0) {
        $(".page_navigation").addClass("disabled");
        $("#bt_save").addClass("disabled");
        $("#bt_print").addClass("disabled");
        $("#bt_zoom_out").addClass("disabled");
        $("#bt_zoom_in").addClass("disabled");
        $("#bt_RotateM90").addClass("disabled");
        $("#bt_RotateP90").addClass("disabled");
        $("#bt_CloseDoc").addClass("disabled");

        $("#bt_save").attr("disabled", "disabled");
        $("#bt_print").attr("disabled", "disabled");
        $("#bt_zoom_out").attr("disabled", "disabled");
        $("#bt_zoom_in").attr("disabled", "disabled");
        $("#bt_RotateM90").attr("disabled", "disabled");
        $("#bt_RotateP90").attr("disabled", "disabled");
        $("#bt_Remove_Page").attr("disabled", "disabled");
        $("#bt_CloseDoc").attr("disabled", "disabled");
        $('input[name="current_page"]').attr("disabled", "disabled");

        document.getElementById('docNameLbl').innerText = "";
    }
    else {
        $(".page_navigation").removeClass("disabled");
        $("#bt_save").removeClass("disabled");
        $("#bt_print").removeClass("disabled");
        $("#bt_zoom_out").removeClass("disabled");
        $("#bt_zoom_in").removeClass("disabled");
        $("#bt_Remove_Page").removeClass("disabled");
        $("#bt_CloseDoc").removeClass("disabled");

        $("#bt_save").removeAttr("disabled");
        $("#bt_print").removeAttr("disabled");
        $("#bt_zoom_out").removeAttr("disabled");
        $("#bt_zoom_in").removeAttr("disabled");
        $("#bt_Remove_Page").removeAttr("disabled");
        $("#bt_CloseDoc").removeAttr("disabled");

        $('input[name="current_page"]').removeAttr("disabled");

        // If the file type is not an image file, don't enable Rotate buttons'
        //if (DocuViewareAPI.GetDocumentType("docuView") == DocumentType.DocumentTypeBitmap) {
            $("#bt_RotateM90").removeClass("disabled");
            $("#bt_RotateP90").removeClass("disabled");

            $("#bt_RotateM90").removeAttr("disabled");
            $("#bt_RotateP90").removeAttr("disabled");
        //}

        if (DocuViewareAPI.GetDocumentType("docuView") == DocumentType.DocumentTypePDF) {
            $("#bt_Remove_Page").removeClass("disabled");

            $("#bt_Remove_Page").removeAttr("disabled");
        }
    }
    if (currentPage === 0 || currentPage === 1) {
        $("#bt_select_page_prev").addClass("disabled");
        $("#bt_select_page_prev").attr("disabled", "disabled");
    }
    else {
        $("#bt_select_page_prev").removeClass("disabled");
        $("#bt_select_page_prev").removeAttr("disabled");
    }
    if (currentPage === pageCount) {
        $("#bt_select_page_next").addClass("disabled");
        $("#bt_select_page_next").attr("disabled", "disabled");
    }
    else {
        $("#bt_select_page_next").removeClass("disabled");
        $("#bt_select_page_next").removeAttr("disabled");
    }
    if (pageCount !== 0) {
        $(".toolbar_container").finish().fadeOut(600);
    }
    else {
        $(".toolbar_container").finish().show();
    }

    
}

function onZoomChange() {
    var zoom = Math.round(parseFloat(DocuViewareAPI.GetCurrentZoom("docuView")) * 100);
    $("#zoom_level_value").html(zoom);
    $("#zoom_level").finish().fadeIn(300).delay(1000).fadeOut(300);
}

function isEnabled(elem) {
    var attr = elem.attr("disabled");
    if (typeof attr !== typeof undefined && attr !== false) {
        return false;
    }
    return true;
}

// Filter for Image Operations
function applyFilter(actionName) {
    if (actionName) {
        var pages = DocuViewareAPI.GetSelectedThumbnailItems("docuView");
        var roi = DocuViewareAPI.GetSelectionAreaCoordinates("docuView");
        if (pages.length == 0) {
            pages[0] = DocuViewareAPI.GetCurrentPage("docuView");
        }
        var params = { pages: pages, RegionOfInterest: roi };
        DocuViewareAPI.PostCustomServerAction("docuView", true, actionName, params);
    }
    return false;
}


$(document).ready(function () {
    $(".toolbar_hover").hover(function () {
        $(".toolbar_container").finish().fadeIn(300);
    }, function () {
        if (DocuViewareAPI.GetPageCount("docuView") !== 0) {
            $(".toolbar_container").finish().delay(1000).fadeOut(600);
        }
    });
    $(".toolbar_container").show();
    DocuViewareAPI.RegisterOnPageChanged("docuView", updateToolbarUI);
    DocuViewareAPI.RegisterOnZoomChanged("docuView", onZoomChange);
    updateToolbarUI();
});