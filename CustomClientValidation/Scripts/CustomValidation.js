var validDateFormats = ["DD/MM/YYYY", "MM/DD/YYYY", "DD-MM-YYYY", "MM-DD-YYYY", "DD.MM.YYYY", "MM.DD.YYYY"];
$.validator.methods.date = function (value, element) {
    return this.optional(element) || moment(value, validDateFormats, true).isValid();
}

$.validator.addMethod("dategreaterthan", function (value, element, params) {
    return moment(value, validDateFormats, true).isAfter(moment($(params).val(), validDateFormats, true));
});

$.validator.addMethod("equalstoproperty", function (value, element, params) {
    return value === $(params).val();
});

$.validator.addMethod("excludechars", function (value, element, params) {
    if (value) 
        for (var i = 0; i < params.length; i++) 
            if ($.inArray(params[i], value) != -1) 
                return false;         
    return true;
});

$.validator.addMethod("mustbechecked", function (value, element, params) {
    return $(element).is(":checked");
});

$.validator.addMethod("validateage", function (value, element, params) {

    return moment(value, validDateFormats, true)
        .isBetween(
        moment(params.minimumdate, validDateFormats),
        moment(params.maximumdate, validDateFormats),
            null, '[]');
});

$.validator.addMethod("requiredif", function (value, element, params) {
    if ($(element).val() != '') return true
    var $toproperty = $('#' + params.toproperty);
    var toPropertyValue = ($toproperty.prop('type').toUpperCase() == "CHECKBOX") ?
                          ($toproperty.prop("checked") ? "true" : "false") :
                          ($toproperty.val().trim() || "");

    switch (params.comparison) {
        case 'isequalto':
            return !((toPropertyValue !== "" && toPropertyValue === params.comparisonvalue) && $.trim(value).length === 0);
        case 'isnotequalto':
            return !((toPropertyValue === "" || toPropertyValue !== params.comparisonvalue) && $.trim(value).length === 0);
        case 'isempty':
            return !(toPropertyValue === "" && $.trim(value).length === 0);
        case 'isnotempty':
            return !(toPropertyValue !== "" && $.trim(value).length === 0);
        default:
            return !((toPropertyValue !== "" && toPropertyValue === params.comparisonvalue) && $.trim(value).length === 0);
    }
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

$.validator.unobtrusive.adapters.add("validateage", ["minimumdate", "maximumdate"], function (options) {
    var params = {
        minimumdate: options.params.minimumdate,
        maximumdate: options.params.maximumdate
    };
    options.rules["validateage"] = params;
    options.messages["validateage"] = options.message;
});

$.validator.unobtrusive.adapters.add("requiredif", ["toproperty", "comparison", "comparisonvalue"], function (options) {
    options.rules['requiredif'] = {
        toproperty: options.params.toproperty,
        comparison: options.params.comparison,
        comparisonvalue: options.params.comparisonvalue
    };
    options.messages['requiredif'] = options.message;
});
