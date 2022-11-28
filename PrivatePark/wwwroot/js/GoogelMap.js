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
    infoWindow = new google.maps.InfoWindow();

    const locationButton = document.createElement("button");

    locationButton.textContent = "Pan to Current Location";
    locationButton.classList.add("custom-map-control-button");
    map.controls[google.maps.ControlPosition.TOP_CENTER].push(locationButton);
    locationButton.addEventListener("click", () => {
        // Try HTML5 geolocation.
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    const pos = {
                        lat: position.coords.latitude,
                        lng: position.coords.longitude,
                    };

                    infoWindow.setPosition(pos);
                    infoWindow.setContent("Location found.");
                    infoWindow.open(map);
                    map.setCenter(pos);
                },
                () => {
                    handleLocationError(true, infoWindow, map.getCenter());
                }
            );
        } else {
            // Browser doesn't support Geolocation
            handleLocationError(false, infoWindow, map.getCenter());
        }
    });
}

function handleLocationError(browserHasGeolocation, infoWindow, pos) {
    infoWindow.setPosition(pos);
    infoWindow.setContent(
        browserHasGeolocation
            ? "Error: The Geolocation service failed."
            : "Error: Your browser doesn't support geolocation."
    );
    infoWindow.open(map);
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



