var locations = [
    [document.getElementById("Latitude").value.replace(/,/g, '.'), document.getElementById("Longitude").value.replace(/,/g, '.')],
    [document.getElementById("GLat").value.replace(/,/g, '.'), document.getElementById("GLon").value.replace(/,/g, '.')]
]

var map = new google.maps.Map(document.getElementById('map'), {
    zoom: 12,
    // center: new google.maps.LatLng(-33.92, 151.25),
    center: new google.maps.LatLng(36.8857, -76.2599),
    mapTypeId: "roadmap",
});


var marker, i;

marker = new google.maps.Marker({
    position: new google.maps.LatLng(locations[0][0], locations[0][1]),
    map: map
});
var infowindow1 = new google.maps.InfoWindow();
infowindow1.setContent("Location");
infowindow1.open(map, marker);
google.maps.event.addListener(marker, 'click', (function (marker) {
    return function () {
        infowindow1.open(map, marker);
    }
})(marker, 0));

marker = new google.maps.Marker({
    position: new google.maps.LatLng(locations[1][0], locations[1][1]),
    map: map
});
var infowindow2 = new google.maps.InfoWindow();
infowindow2.setContent("Guess");
infowindow2.open(map, marker);
google.maps.event.addListener(marker, 'click', (function (marker) {
    return function () {
        infowindow2.open(map, marker);
    }
})(marker, 0));

var loc = new google.maps.LatLng(locations[0][0], locations[0][1]);
var guess = new google.maps.LatLng(locations[1][0], locations[1][1]);
var bounds = new google.maps.LatLngBounds();
bounds.extend(loc);
bounds.extend(guess);
map.fitBounds(bounds);