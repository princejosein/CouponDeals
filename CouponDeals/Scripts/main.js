function disableDatepicker(datePicker) {
    $(datePicker).datepicker({
        showOn: "off",
    });
    //alert("Test");
}

function enableDatepicker(datePicker, minDate, maxDate) {
    $(datePicker).datepicker({
        minDate: 0,
        dateFormat: "yy-mm-dd",
        showOn: "both",
        minDate: minDate,
        maxDate: maxDate,
        buttonText: "<i class='fa fa-calendar'></i>"
    });
    // alert("Test OK");
}