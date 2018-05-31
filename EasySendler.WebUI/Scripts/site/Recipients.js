function Recipients() {
    "use strict";

    var $modalOpenPecipients = $("#recipients");
    var templateOpenRecipientList = $("#recipientDetailsTemplate").html();

    $modalOpenPecipients.on("shown.bs.modal", function (e) {
        var $modalBody = $modalOpenPecipients.find("div.modal-body");
        $modalBody.empty();
        var clickedBtn = e.relatedTarget;       
        $.ajax({
            type: "GET",
            url: clickedBtn.dataset.ajaxUrl,
            data: { id: clickedBtn.dataset.id },
           // dataType: "json",
            traditional: true,
            success: function (rawData) {
                $modalBody.html(rawData);
                //var data = JSON.parse(rawData);
                //renderTemplate($modalBody, templateOpenRecipientList, { items: data });
            },
            error: getAjaxError
        });
    });


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