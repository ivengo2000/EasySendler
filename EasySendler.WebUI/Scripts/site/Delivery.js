function Delivery() {
    "use strict";

    var pageSize = 10;
    var $ddlRecipientLists = $("#ddlRecipientLists");
    var $recipientAmount = $("#recipientAmount");
    var $recipientAmountKnob;
    //var $dlbRecipientsJq;
    var recipientsAmountUrl = $ddlRecipientLists.get(0).dataset.recipientsAmountUrl;
    //var saveConfiguredListUrl = $ddlRecipientLists.get(0).dataset.saveConfiguredListUrl;
    var selectedReciientListId = 0;

    $ddlRecipientLists.select2({
        minimumInputLength: 0,
        ajax: {
            quietMillis: 500,
            dataType: "json",
            delay: 500,
            data: function (params) {
                return {
                    pageSize: pageSize,
                    pageNum: params.page || 1,
                    searchTerm: params.term
                };
            },
            processResults: function (rawData, params) {
                var data = JSON.parse(rawData);
                params.page = params.page || 1;
                return {
                    results: data.Results,
                    pagination: {
                        more: params.page * parseInt(pageSize) < data.Total
                    }
                };
            }
        },
        templateResult: getTemplateForDdlOptions,
        templateSelection: getTemplateForDdlOptions
    });

    $ddlRecipientLists.on("select2:select", function (e) {
        selectedReciientListId = e.currentTarget.value;
        $.ajax({
            type: "GET",
            url: recipientsAmountUrl,
            data: { id: selectedReciientListId },
            dataType: "json",
            traditional: true,
            success: function (rawData) {
                var data = JSON.parse(rawData);
                $recipientAmount.val(data).trigger('change');
            },
            error: function (xhr) {
                try {
                    var json = $.parseJSON(xhr.responseText);
                    alert(json.errorMessage);
                } catch (e) {
                    alert('something bad happened');
                }
            }
        });
    });


    $(".dial").knob({
        "readOnly" : true
    });

    function getTemplateForDdlOptions(data) {
        if (!data.id) {
            return data.text;
        }
        var arr = data.text.split("|");
        var $optionItem = $(
          "<span class='option-item-name'>" + arr[0] + "</span><span class='option-item-descr-title'>description:</span><span class='option-item-description'>" + arr[1] + "</span>"
        );
        return $optionItem;
    }
}