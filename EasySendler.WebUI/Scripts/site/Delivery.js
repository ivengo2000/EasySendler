﻿function Delivery() {
    "use strict";

    var pageSize = 10;
    var $ddlRecipientLists = $("#ddlRecipientLists");
    var $ddlMailSettingsLists = $("#ddlMailSettingsLists");
    var $recipientAmount = $("#recipientAmount");
    var recipientsAmountUrl = $ddlRecipientLists.get(0).dataset.recipientsAmountUrl;
    var recipientsDetailsUrl = $ddlRecipientLists.get(0).dataset.recipientsDetailsUrl;
    var selectedReciientListId = null;
    var selectedTemplateId = null;
    var selectedMailSettingsId = null;
    var $btnRun = $("#btnRun");
    var $btnOpenRecipientList = $("#btnOpenRecipientList");
    var $modalOpenRecipientList = $("#recipientListDetails");
    var $modalTemplatesList = $("#templatesList");
    var $txtTemplates = $("#txtTemplates");
    var templateOpenRecipientList = $("#recipientListDetailsTemplate").html();
    var templatesListTemplate = $("#templatesListTemplate").html();

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
        selectedReciientListId = null;
        checkConditionsToRun();
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
                checkConditionsToRun();
            },
            error: getAjaxError
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
            },
            error: getAjaxError
        });
    });

    $btnOpenRecipientList.on("click", function (event) {
        if ($(this).hasClass("disabled")) {
            event.stopPropagation();
        } 
    });

    $modalTemplatesList.on("show.bs.modal", function(e) {
        $(this).find("div.modal-body").empty();
    });

    $modalTemplatesList.on("shown.bs.modal", function (e) {
        var $modalBody = $(this).find("div.modal-body");
        $.ajax({
            type: "GET",
            url: $modalTemplatesList.get(0).dataset.templatesListUrl,
            data: { },
            dataType: "json",
            traditional: true,
            success: function (rawData) {
                var data = JSON.parse(rawData);
                renderTemplate($modalBody, templatesListTemplate, { items: data });
                initTemplatesLinks();
            },
            error: getAjaxError
        });
    });

    $ddlMailSettingsLists.select2({
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

    $ddlMailSettingsLists.on("select2:select", function (e) {
        selectedMailSettingsId = e.currentTarget.value;
        checkConditionsToRun();
    });

    $ddlMailSettingsLists.on("select2:unselect", function (e) {
        selectedMailSettingsId = null;
        checkConditionsToRun();
    });

    function checkConditionsToRun() {
        if (selectedReciientListId && selectedTemplateId && selectedMailSettingsId) {
            $btnRun.removeClass("disabled").addClass("active");
        } else {
            $btnRun.removeClass("active").addClass("disabled");
        }
    }

    function initTemplatesLinks() {
        $modalTemplatesList.find("a.template-link").on("click", function (e) {
            var link = e.currentTarget;
            selectedTemplateId = link.dataset.selectedTemplateId;
            $txtTemplates.val(link.dataset.selectedTemplateName);
            $modalTemplatesList.modal('hide');
            checkConditionsToRun();
        });
    }

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