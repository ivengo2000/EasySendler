function MailSettings() {
    "use strict";

    var $modalOpen = $("#modalMailSettings");
    var templateOpen = $("#mailSettingsDetailsTemplate").html();
    var templateDelete = $("#mailSettingsDeleteTemplate").html();

    $modalOpen.on("shown.bs.modal", function (e) {
        var $modalBody = $modalOpen.find("div.modal-body");
        var $modalTitle = $modalOpen.find("div.modal-header .modal-title");
        $modalBody.empty();
        var clickedBtn = e.relatedTarget;
        var actionType = clickedBtn.dataset.actionType;
        var modalTitle = clickedBtn.dataset.modalTitle;
        $.ajax({
            type: "GET",
            url: clickedBtn.dataset.ajaxUrl,
            data: { id: clickedBtn.dataset.id },
            dataType: actionType === "Edit" ? "html" : "json",
            traditional: true,
            success: function (rawData) {

                $modalTitle.text(modalTitle);
                switch (actionType) {
                    case "Edit":
                        $modalBody.html(rawData);
                        var $form = $modalBody.find("#getEditForm");
                        $.validator.unobtrusive.parse($form);
                        break;
                    case "Details":
                        renderTemplate($modalBody, templateOpen, { item: JSON.parse(rawData) });
                        $(".checkbox").bootstrapToggle();
                        break;
                    case "Delete":
                        renderTemplate($modalBody, templateDelete, { item: JSON.parse(rawData) });
                        $(".checkbox").bootstrapToggle();
                        break;
                    default:
                        break;
                }
            },
            error: getAjaxError
        });
    });


    function getAjaxError(xhr) {
        try {
            var json = $.parseJSON(xhr.responseText);
            alert(json.errorMessage);
        } catch (e) {
            alert("something bad happened");
        }
    }

    function renderTemplate($target, $template, data) {
        var tmpl = _.template($template);
        $target.html(tmpl(data));
    }
}