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
    Search_Box();
    googelmarker();
    currentposition()


});











function Search_Box()
{

    var searchInput = document.getElementById('sok');
    var searchBtn = document.getElementById('sokbutton');

    
    var searchBox = new google.maps.places.SearchBox(searchInput);
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(document.getElementById('sokbutton'));
    
    var markers = [];
    

    searchBtn.onclick = function () {
        displaySearchResults(map,searchBox,markers);
    }
    
    
}



function displaySearchResults(map, searchBox, markers) {
    
    var places = searchBox.getPlaces();
    
    if (places.length === 0) {
        return;
    }

    
    markers.forEach(function (marker) {
        marker.setMap(null);
    });
    
    // For each place, get the icon, name and location.
    var bounds = new google.maps.LatLngBounds();
    
    
    
    places.forEach(function (place) {
        if (!place.geometry) {
            console.log("Returned place contains no geometry");
            return;
        }


        markers.push(new google.maps.Marker({
            map: map,
            position: place.geometry.location
        }));

        if (place.geometry.viewport) {
            
            bounds.union(place.geometry.viewport);
        } else {
            bounds.extend(place.geometry.location);
        }
        
    });
    map.fitBounds(bounds);

}











function currentposition()
{
    infoWindow = new google.maps.InfoWindow();

    const locationButton = document.getElementById("currentposition")
    
    
    
    
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
        var parkingmark = [];
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

            parkingmark [i] =  new google.maps.Marker(
                {
                    position: {lat: parking[i].location.lat, lng:parking[i].location.lng},
                    map : map,
                    icon:icon
                });
        }
    })
};





function initMap()
{
    const myLatLng = { lat: 58.3421, lng: 8.5945 };

    map = new google.maps.Map(document.getElementById("map"),
        {
            center: myLatLng,
            zoom: 12,
        });

}






