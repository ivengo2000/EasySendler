function ConfigureRecipientList () {
    "use strict";

    var pageSize = 10;
    var $ddlRecipientLists = $("#ddlRecipientLists");

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
       // alert(e.currentTarget.value);
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