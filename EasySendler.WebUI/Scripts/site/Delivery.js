﻿function Delivery() {
    "use strict";

    var pageSize = 10;
    var $ddlRecipientLists = $("#ddlRecipientLists");
    var $ddlMailSettingsLists = $("#ddlMailSettingsLists");
    var $recipientAmount = $("#recipientAmount");
    var $runProgress = $("#runProgress");
    var recipientsAmountUrl = $ddlRecipientLists.get(0).dataset.recipientsAmountUrl;
    var recipientsDetailsUrl = $ddlRecipientLists.get(0).dataset.recipientsDetailsUrl;
    var selectedReciientListId = null;
    var selectedTemplateId = null;
    var selectedMailSettingsId = null;
    var $btnRun = $("#btnRun");
    var runUrl = $btnRun.get(0).dataset.runUrl;
    var $btnOpenRecipientList = $("#btnOpenRecipientList");
    var $modalOpenRecipientList = $("#recipientListDetails");
    var $modalTemplatesList = $("#templatesList");
    var $txtTemplates = $("#txtTemplates");
    var templateOpenRecipientList = $("#recipientListDetailsTemplate").html();
    var templatesListTemplate = $("#templatesListTemplate").html();
    var $runningProcess = $("#runningProcess");

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
        $recipientAmount.val(0).trigger("change");
        $runProgress.val(0).trigger("change");
        $runProgress.trigger("configure", { max: 100 });
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
                $recipientAmount.val(data).trigger("change");
                $runProgress.trigger("configure", { max: data });
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

    $btnRun.on("click", function (event) {
        if ($(this).hasClass("disabled")) {
            event.stopPropagation();
        } else {
            $runningProcess.empty();
            $runProgress.val(0).trigger("change");
            $.ajax({
                type: "GET",
                url: recipientsDetailsUrl,
                data: { id: selectedReciientListId },
                dataType: "json",
                traditional: true,
                success: function (rawData) {
                    var data = JSON.parse(rawData);
                    $runningProcess.append("<div class='alert alert-info' role='alert'>Delivery has been started at " + new Date().toISOString() + "</div>");
                    sendEmail(data, 0); 
                },
                error: getAjaxError
            });
        }
    });

    function sendEmail(data, index) {
        var item = data[index];
        var email = item.Email;
        var name = item.Name;
        var sureName = item.SureName;
        var selectedtId = selectedTemplateId;
        var selectedmsId = selectedMailSettingsId;
        $.ajax({
            type: "GET",
            dataType: "json",
            data: { email: email, name: name, sureName: sureName, templateId: selectedtId, mailSettingsId: selectedmsId },
            url: runUrl,
            success: function (response) {
                var responseObj = JSON.parse(response);
                var alertTypeClass = responseObj.IsSuccessful ? "alert-success" : "alert-danger";

                $runningProcess.append("<div class='alert " + alertTypeClass + "' role='alert'>" + responseObj.ResultMessage + " to <span class='badge'>" + email + "</span></div>");

                $runProgress.val(parseInt($runProgress.val(), 10) + 1).trigger("change");
                if (index < data.length - 1) {
                    sendEmail(data, index + 1);
                } else {
                    $runningProcess.append("<div class='alert alert-info' role='alert'>Delivery has been finished at " + new Date().toISOString() + "</div>");
                    $btnRun.removeClass("active").addClass("disabled");
                }
            },
            error: getAjaxError
        });
    }

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
            $modalTemplatesList.modal("hide");
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