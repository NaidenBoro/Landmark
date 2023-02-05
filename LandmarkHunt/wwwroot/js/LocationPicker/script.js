$(function () {

    $('#us3').locationpicker({
        location: { latitude: document.getElementById('Latitude').value.replace(/,/g, '.'), longitude: document.getElementById('Longitude').value.replace(/,/g, '.') },
        radius: 0,
        markerDraggable: false
    });
});

$(function () {

    $('#us4').locationpicker({
        location: { latitude: document.getElementById('Latitude').value.replace(/,/g, '.'), longitude: document.getElementById('Longitude').value.replace(/,/g, '.') },
        radius: 0,
        markerDraggable: false
    });
});

