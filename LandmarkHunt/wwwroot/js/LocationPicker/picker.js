var map = new google.maps.Map(document.getElementById('map'), {
    zoom: 12,
    center: new google.maps.LatLng(document.getElementById("Latitude").value.replace(/,/g, '.'), document.getElementById("Longitude").value.replace(/,/g, '.')),
    mapTypeId: "roadmap",
});

marker = new google.maps.Marker({
    position: new google.maps.LatLng(document.getElementById("Latitude").value.replace(/,/g, '.'), document.getElementById("Longitude").value.replace(/,/g, '.')),
    map: map,
    draggable: true,
});

google.maps.event.addListener(marker, 'dragend', function (marker) {
    var latLng = marker.latLng;
    currentLatitude = latLng.lat();
    currentLongitude = latLng.lng();
    console.log(latLng.lat(), latLng.lng());
    document.getElementById("Latitude").value = latLng.lat();
    document.getElementById("Longitude").value = latLng.lng();
    jQ('#latitude').val(currentLatitude);
    jQ('#longitude').val(currentLongitude);
});

function initialize() {
    var mapOptions = {
        zoom: 8,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
    };

    initializeMarker();
}

initialize();