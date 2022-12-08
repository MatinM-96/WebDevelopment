// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('#myDropdown').on({
    "click":function(e){
        e.stopPropagation();
    }
});



$(function() {

    $(".filter").on("change", function() {
        var hash = $(".filter:checked").map(function() {
            return this.value;
        }).toArray();
        console.log(hash);
        hash = hash.join("&");
        location.hash = hash;
        console.log(hash);
    });

    if (location.hash !== "") {
        var hash = location.hash.substr(1).split("&");
        hash.forEach(function(value) {
            $("input[value=" + value + "]").prop("checked", true);
        });
    }
});