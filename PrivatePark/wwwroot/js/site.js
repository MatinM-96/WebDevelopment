// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function k()
{

    // Create the search box and link it to the UI element.
    var searchInput = document.getElementById('sok');
    var searchBtn = document.getElementById('sokbutton');
    var searchBox = new google.maps.places.SearchBox(searchInput);
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(searchInput);
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(searchBtn);

    // Bias the SearchBox results towards current map's viewport.
    map.addListener('bounds_changed', function () {
        searchBox.setBounds(map.getBounds());
    });

    var markers = [];
    // Listen for the event fired when the user selects a prediction and retrieve
    // more details for that place.
    searchBox.addListener('places_changed', function () {
        //displaySearchResults(map, searchBox, markers);
    });


    searchBtn.onclick = function () {
        displaySearchResults(map,searchBox,markers);
    }
}
