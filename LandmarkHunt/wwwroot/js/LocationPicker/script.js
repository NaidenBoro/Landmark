$(function () {

    $('#us2').locationpicker({
        location: { latitude: document.getElementById('Latitude').value.replace(/,/g, '.'), longitude: document.getElementById('Longitude').value.replace(/,/g, '.') },
        radius: 0,
        inputBinding: {
            latitudeInput: $('#Latitude'),
            longitudeInput: $('#Longitude'),
            locationNameInput: $('#location')
        },
        enableAutocomplete: true
    });


});
