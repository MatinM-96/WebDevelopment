var map;
var green = {
    url: "Icons/green_marker.svg",
    scaledSize: new google.maps.Size(30, 30),
}
var red = {
    url: "Icons/red_marker.svg",
    scaledSize: new google.maps.Size(30, 30),
}

$(document).ready(function() {
    initMap();
    googelmarker();
    

});









function initMap()
{
    const myLatLng = { lat: 58.3421, lng: 8.5945 };

    map = new google.maps.Map(document.getElementById("map"),
        {
            center: myLatLng,
            zoom: 15,
        });
}






function googelmarker()
{
    $.get("/info/GetAllParkings", function (parking)
    {
        var mark = [];
        var icon;
        for(var i=0;i<parking.length;i++)

        {
            if(parking[i].availability == false)
            {
                icon  = red;
            }
            else
            {
                icon = green;
            }

            mark [i] =  new google.maps.Marker(
                {
                    position: {lat: parking[i].location.lat, lng:parking[i].location.lng},
                    map : map,
                    icon:icon
                }
            );
        }
    })
};





