﻿function ConfigureRecipientList() {
    "use strict";

    var pageSize = 10;
    var $ddlRecipientLists = $("#ddlRecipientLists");
    var $dlbRecipients = $("#dlbRecipients");
    var $dlbRecipientsJq;
    var recipientsToConfigureUrl = $ddlRecipientLists.get(0).dataset.recipientsToConfigureUrl;

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

    $dlbRecipientsJq = $dlbRecipients.bootstrapDualListbox({
        nonSelectedListLabel: 'Non-selected',
        selectedListLabel: 'Selected',
        preserveSelectionOnMove: 'moved',
        moveOnSelect: false
    });

    $ddlRecipientLists.on("select2:select", function (e) {
        $.ajax({
            type: "GET",
            url: recipientsToConfigureUrl,
            data: { id: e.currentTarget.value },
            dataType: "json",
            traditional: true,
            success: function (rawData) {
                var data = JSON.parse(rawData);
                $dlbRecipients.html("");
                for (var i = 0; i < data.length; i++) {
                    if (data[i].selected === 1) {
                        $dlbRecipients.append("<option value=" + data[i].id + " selected = 'selected'>" + data[i].text + "</option>");
                    } else {
                        $dlbRecipients.append("<option value=" + data[i].id + ">" + data[i].text + "</option>");
                    }
                }
                $dlbRecipientsJq.bootstrapDualListbox("refresh");
            },
            error: function(xhr) {
                try {
                    var json = $.parseJSON(xhr.responseText);
                    alert(json.errorMessage);
                } catch(e) { 
                    alert('something bad happened');
                }
            }
        });
    });

    function getTemplateForDdlOptions (data) {
            if (!data.id) {
                return data.text;
            }
            var arr = data.text.split("|");
            var $optionItem = $(
              "<span class='option-item-name'>" + arr[0] + "</span><span class='option-item-descr-title'>description:</span><span class='option-item-description'>" + arr[1] + "</span>"
            );
            return $optionItem;
    }

    //ConfigureRecipientList.prototype.init = function init(el, formId) {
    //    $el = el;
    //    $form = $("#" + formId, $el);
    //    $result = $("#result-" + formId, $el);
    //    this.$form = $form;
    //};
};