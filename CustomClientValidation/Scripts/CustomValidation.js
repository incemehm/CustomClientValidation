$.validator.addMethod("dategreaterthan", function (value, element, params) {
    return Date.parse(value) > Date.parse($(params).val());
});

$.validator.addMethod("equalstoproperty", function (value, element, params) {
    return value === $(params).val();
});

$.validator.addMethod("excludechars", function (value, element, exclude) {
    if (value) {
        for (var i = 0; i < exclude.length; i++) {
            if ($.inArray(exclude[i], value) != -1) {
                return false;
            }
        }
    }
    return true;
});

$.validator.addMethod("mustbechecked", function (value, element, params) {
    return $(element).is(":checked");
});

$.validator.addMethod("validateage", function (value, element, params) {
    return Date.parse(value) >= Date.parse(params.minumumdate) && Date.parse(value) <= Date.parse(params.maximumdate);
});


$.validator.unobtrusive.adapters.add("dategreaterthan", ["toproperty"], function (options) {
    options.rules["dategreaterthan"] = "#" + options.params.toproperty;
    options.messages["dategreaterthan"] = options.message;
});

$.validator.unobtrusive.adapters.add("equalstoproperty", ["toproperty"], function (options) {
    options.rules["equalstoproperty"] = "#" + options.params.toproperty;
    options.messages["equalstoproperty"] = options.message;
});

$.validator.unobtrusive.adapters.addSingleVal("excludechars", "chars");

$.validator.unobtrusive.adapters.addBool("mustbechecked");

$.validator.unobtrusive.adapters.add("validateage", ["minumumdate", "maximumdate"], function (options) {
    var params = {
        minumumdate: options.params.minumumdate,
        maximumdate: options.params.maximumdate
    };
    options.rules["validateage"] = params;
    options.messages["validateage"] = options.message;
});
