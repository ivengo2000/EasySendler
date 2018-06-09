function Recipients() {
    "use strict";

    var $modalOpenPecipients = $("#modalRecipient");
    var templateOpenRecipient = $("#recipientDetailsTemplate").html();
    var templateDeleteRecipient = $("#recipientDeleteTemplate").html();

    $modalOpenPecipients.on("shown.bs.modal", function (e) {
        var $modalBody = $modalOpenPecipients.find("div.modal-body");
        var $modalTitle = $modalOpenPecipients.find("div.modal-header .modal-title");
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
                        renderTemplate($modalBody, templateOpenRecipient, { item: JSON.parse(rawData) });
                        break;
                    case "Delete":
                        renderTemplate($modalBody, templateDeleteRecipient, { item: JSON.parse(rawData) });
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