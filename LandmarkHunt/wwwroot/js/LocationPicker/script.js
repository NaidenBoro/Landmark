$(function () {

    $('#us2').locationpicker({
        location: { latitude: 46.15242437752303, longitude: 2.7470703125 },
        radius: 0,
        inputBinding: {
            latitudeInput: $('#Latitude'),
            longitudeInput: $('#Longitude'),
            locationNameInput: $('#location')
        },
        enableAutocomplete: true
    });


});
