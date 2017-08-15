function Delivery() {
    "use strict";

    var pageSize = 10;
    var $ddlRecipientLists = $("#ddlRecipientLists");
    var $recipientAmount = $("#recipientAmount");
    var recipientsAmountUrl = $ddlRecipientLists.get(0).dataset.recipientsAmountUrl;
    var recipientsDetailsUrl = $ddlRecipientLists.get(0).dataset.recipientsDetailsUrl;
    var selectedReciientListId = 0;
    var $btnOpenRecipientList = $("#btnOpenRecipientList");
    var $modalOpenRecipientList = $("#recipientListDetails");
    var templateOpenRecipientList = $("#recipientListDetailsTemplate").html();

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

    $ddlRecipientLists.on("select2:unselect", function(e) {
        $recipientAmount.val(0).trigger('change');
        $btnOpenRecipientList.removeClass("active").addClass("disabled");
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
                $btnOpenRecipientList.removeClass("disabled").addClass("active");
            },
            error: getAjaxError
            //error: function (xhr) {
            //    try {
            //        var json = $.parseJSON(xhr.responseText);
            //        alert(json.errorMessage);
            //    } catch (e) {
            //        alert('something bad happened');
            //    }
            //}
        });
    });

    $(".dial").knob({
        "readOnly" : true
    });

    $modalOpenRecipientList.on("shown.bs.modal", function (e) {
        var $modalBody = $modalOpenRecipientList.find("div.modal-body");
        $modalBody.empty();
        $.ajax({
            type: "GET",
            url: recipientsDetailsUrl,
            data: { id: selectedReciientListId },
            dataType: "json",
            traditional: true,
            success: function (rawData) {
                var data = JSON.parse(rawData);
                renderTemplate($modalBody, templateOpenRecipientList, { items: data });
                //for (var i = 0; i < data.length; i++) {
                //    $modalBody.append("<ul><li>" + data[i].Email + "</li><li>" + data[i].Name + "</li><li>" + data[i].SureName + "</li></ul>");
                //}

                //$recipientAmount.val(data).trigger('change');
                //$btnOpenRecipientList.removeClass("disabled").addClass("active");
            },
            error: getAjaxError
            //error: function (xhr) {
            //    try {
            //        var json = $.parseJSON(xhr.responseText);
            //        alert(json.errorMessage);
            //    } catch (e) {
            //        alert('something bad happened');
            //    }
            //}
        });
    });

    $btnOpenRecipientList.on("click", function (event) {
        if ($(this).hasClass("disabled")) {
            event.stopPropagation();
        } else {
            $('#applyRemoveDialog').modal("show");
        }
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

    function getAjaxError(xhr) {
        try {
            var json = $.parseJSON(xhr.responseText);
            alert(json.errorMessage);
        } catch (e) {
            alert('something bad happened');
        }
    }

    function renderTemplate($target, $template, data) {
        var tmpl = _.template($template);
        $target.html(tmpl(data));
    }
}