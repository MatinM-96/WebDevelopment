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
    currentposition();
});




var searchBtn = document.getElementById('sokbutton');



function Search_Box()
{

    var searchInput = document.getElementById('sok');

    
    var searchBox = new google.maps.places.SearchBox(searchInput);
    
    var markers = [];
    
    

    searchBtn.onclick = function () {
        displaySearchResults(map,searchBox,markers);
    }
    
    var m = document.getElementById('sok').value;
    
    
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
        var icon;
        var userMail = [];
        var userCars = '';

        $.get("/info/GetAddressUser", function(user) {
            for (var j = 0; j < user.length; j++) {
                userMail[j] = user[j].user.email;
                console.log(userMail[j]);
            }

            $.get("/info/GetAllcarForEachUser", function (car) {
                var cars = JSON.parse(car);

                for (var k = 0; k < cars.length; k++) {
                    userCars += '<option>'+ cars[k].RegistrationNumber +'</option>';
                    console.log(cars[k].RegistrationNumber);
                }
                console.log(userCars);

                for(var i = 0; i < parking.length; i++)
                {
                    if(parking[i].quantity < 1)
                    {
                        icon = red;
                    }
                    else
                    {
                        icon = green;
                    }

                    var parkingmark =  new google.maps.Marker(
                        {
                            position: {lat: parking[i].location.lat, lng:parking[i].location.lng},
                            map : map,
                            icon: icon
                        });

                    var addressId = parking[i].id;
                    var buttonId = 'rent-button-' + i;

                    console.log(userCars);
                    console.log(userMail[i]);

                    var contentAvailable = '<form method="post">' +
                        '<div><input name="addressId" value="'+addressId+'" hidden/></div>' +
                        '<div><label for="parking-time">Rent until: </label>' +
                        '<input id="parking-time" name="time" type="datetime-local" required/></div>' +
                        '<div><label id="parking-car">Select car: </label>' +
                        '<select id="parking-car" name="car" required>' +
                        '<option selected disabled hidden></option>' +
                        userCars +
                        '</select></div>' +
                        '<div><input id="'+buttonId+'" type="submit" value="Click to rent!"/></div>' +
                        '</form>' +
                        '<form action="/Chat/Index" method="post">' +
                        '<div><input name="username" value="'+userMail[i]+'" hidden/></div>' +
                        '<div><input type="submit" value="Message renter"/></div>' +
                        '</form>';

                    console.log(contentAvailable);

                    var contentOccupied = '<p>Spot currently unavailable</p>' +
                    '</form>' +
                    '<form action="/Chat/Index" method="post">' +
                    '<div><input name="username" value="'+userMail[i]+'" hidden/></div>' +
                    '<div><input type="submit" value="Message renter"/></div>' +
                    '</form>';

                    var infowindow = new google.maps.InfoWindow();

                    if (icon === green) {

                        google.maps.event.addListener(parkingmark, 'click', (function(parkingmark, contentAvailable, infowindow) {
                            return function() {
                                infowindow.setContent(contentAvailable);
                                infowindow.open(map, parkingmark);
                            };
                        })(parkingmark, contentAvailable, infowindow));
                    }
                    else {
                        google.maps.event.addListener(parkingmark, 'click', (function(parkingmark, contentOccupied, infowindow) {
                            return function() {
                                infowindow.setContent(contentOccupied);
                                infowindow.open(map, parkingmark);
                            }
                        })(parkingmark, contentOccupied, infowindow));
                    }
                }
            });
        });
    });
}





function initMap()
{
    const myLatLng = { lat: 58.3421, lng: 8.5945 };

    map = new google.maps.Map(document.getElementById("map"),
        {
            center: myLatLng,
            zoom: 12,
        });

}
