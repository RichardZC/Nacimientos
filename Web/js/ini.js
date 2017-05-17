var fn = {
    url: function (s) { return window.location.pathname + '/' + s; },
    mensaje: function (p) { Materialize.toast(p, 4000); },
    notificar: function (o) {
        switch (o) {
            case 'add': Materialize.toast('SE CREO EL REGISTRO CORRECTAMENTE!', 4000); break;
            case 'mod': Materialize.toast('SE MODIFICO CORRECTAMENTE!', 4000); break;
            case 'anu': Materialize.toast('SE ANULO CORRECTAMENTE!', 4000); break;
            case 'rem': Materialize.toast('SE ELIMINO CORRECTAMENTE!', 4000); break;
            default: Materialize.toast('SE GRABARON LOS DATOS CORRECTAMENTE!', 4000);
        }
    }
};

$(document).ready(function () {
    
    //https://github.com/devbridge/jQuery-Autocomplete
    if ($('#autocompletar').data('tabla') == 'persona') {
        $.get('Comun/ListarPersonas', function (res) {
            $('#autocompletar').autocomplete({
                //serviceUrl: '@Url.Action("listapais", "Home")',
                lookup: res,
                minChars: 2,
                onSelect: function (suggestion) {
                    if ($(this).data('funcion') != null) {
                        var funcion = $(this).data('funcion') + '(' + suggestion.data + ');';
                        setTimeout(funcion, 0);
                    }
                },
                showNoSuggestionNotice: true,
                noSuggestionNotice: 'Lo siento, no hay resultados',
            });
        });
    };


    $("body").on('click', 'button', function () {

        // Si el boton no tiene el atributo ajax no hacemos nada
        if ($(this).data('ajax') == undefined) return;

        // El metodo .data identifica la entrada y la castea al valor más correcto
        if ($(this).data('ajax') != true) return;

        var form = $(this).closest("form");
        var buttons = $("button", form);
        var button = $(this);
        var url = form.attr('action');

        if (button.data('confirm') != undefined) {
            if (button.data('confirm') == '') {
                if (!confirm('¿Esta seguro de realizar esta acción?')) return false;
            } else {
                if (!confirm(button.data('confirm'))) return false;
            }
        }

        if (button.data('delete') != undefined) {
            if (button.data('delete') == true) {
                url = button.data('url');
            }
        } else if (!form.valid()) {
            return false;
        }

        // Creamos un div que bloqueara todo el formulario
        var block = $('<div class="block-loading" />');
        form.prepend(block);

        // En caso de que haya habido un mensaje de alerta
        $("#card-alert", form).remove();

        // Para los formularios que tengan CKupdate
        //if (form.hasClass('CKupdate')) CKupdate();

        form.ajaxSubmit({
            dataType: 'JSON',
            type: 'POST',
            url: url,
            success: function (r) {
                block.remove();
                if (r.response) {
                    if (!button.data('reset') != undefined) {
                        if (button.data('reset')) form.reset();
                    }
                    else {
                        form.find('input:file').val('');
                    }
                }

                // Mostrar mensaje
                if (r.message != null) {
                    if (r.message.length > 0) {
                        var css = "";
                        if (r.response) css = "green";
                        else css = "red";

                        var message = '<div id="card-alert" class="card ' + css + ' "><div class="card-content white-text"><p><i class="mdi-alert-error"></i>' + r.message + '</p></div><button type="button" class="close white-text" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">×</span></button></div>';
                        form.prepend(message);
                        setTimeout('$("#card-alert .close").click(function () { $(this).closest("#card-alert").fadeOut("slow") });', 0);
                    }
                }

                // Ejecutar funciones
                if (r.function != null) {
                    setTimeout(r.function, 0);
                }
                // Redireccionar
                if (r.href != null) {
                    if (r.href == 'self') window.location.reload(true);
                    else window.location.href = r.href;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                block.remove();
                form.prepend('<div class="alert alert-warning alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' + errorThrown + ' | <b>' + textStatus + '</b></div>');
            }
        });

        return false;
    })
});

jQuery.fn.reset = function () {
    $("input:password,input:file,input:text,textarea", $(this)).val('');
    $("input:checkbox:checked", $(this)).click();
    $("select").each(function () {
        $(this).val($("option:first", $(this)).val());
    })
};

/*
 * Translated default messages for the jQuery validation plugin.
 * Locale: ES
 */
jQuery.extend(jQuery.validator.messages, {
    required: "Este campo es obligatorio.",
    remote: "Por favor, rellena este campo.",
    email: "Por favor, escribe una dirección de correo válida",
    url: "Por favor, escribe una URL válida.",
    date: "Por favor, escribe una fecha válida.",
    dateISO: "Por favor, escribe una fecha (ISO) válida.",
    number: "Por favor, escribe un número entero válido.",
    digits: "Por favor, escribe solo digitos.",
    creditcard: "Por favor, escribe un número de tarjeta válido.",
    equalTo: "Por favor, escribe el mismo valor de nuevo.",
    accept: "Por favor, escribe un valor con una extensión aceptada.",
    maxlength: jQuery.validator.format("Por favor, no escribas mas de {0} caracteres."),
    minlength: jQuery.validator.format("Por favor, no escribas menos de {0} caracteres."),
    rangelength: jQuery.validator.format("Por favor, escribe un valor entre {0} y {1} caracteres."),
    range: jQuery.validator.format("Por favor, escribe un valor entre {0} y {1}."),
    max: jQuery.validator.format("Por favor, escribe un valor menor o igual a {0}."),
    min: jQuery.validator.format("Por favor, escribe un valor mayor o igual a {0}.")
});

