
var map; 

    $(document).ready(function ()
    {
        initMap();
        $.get("/GoogelMap/GetAllLocation", function (location)
        {
            var mark = []; 
            console.log(mark)
            for(var i=0;i<location.length;i++){
               mark [i] =  new google.maps.Marker({
                    position: {lat: location[i].lat, lng:  location[i].lng},
                    map : map
                });
        }
})})


    function initMap()
    {

        const myLatLng = { lat: 58.3421, lng: 8.5945 }; 


       map = new google.maps.Map(document.getElementById("map"), {
            center: myLatLng,
            zoom: 16,
        });
    }



    






