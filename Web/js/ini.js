var fn = {
    //url: function (s) { return baseUrl + s; },
    resetForm: function () {
        $('.validar').each(function () {
            $(this).validate().resetForm();
        });
    },
    mensaje: function (p) { Materialize.toast(p, 4000); },
    notificar: function (o) {
        switch (o) {
            case 'add': Materialize.toast('SE CREO EL REGISTRO CORRECTAMENTE!', 4000); break;
            case 'mod': Materialize.toast('SE MODIFICO CORRECTAMENTE!', 4000); break;
            case 'anu': Materialize.toast('SE ANULO CORRECTAMENTE!', 4000); break;
            case 'rem': Materialize.toast('SE ELIMINO CORRECTAMENTE!', 4000); break;
            default: Materialize.toast('SE GRABARON LOS DATOS CORRECTAMENTE!', 4000);
        }
    },
    prompt: function (titulo, control, valor, mcallback) {
        swal({
            title: titulo,
            input: control,
            showCancelButton: true,
            closeOnConfirm: false,
            inputPlaceholder: "Ingrese dato",
            inputValue: valor,
            confirmButtonText: '<i class="mdi-navigation-check"></i> ACEPTAR',
            cancelButtonText: '<i class="mdi-navigation-close"></i> CANCELAR',
            inputValidator: function (value) {
                return new Promise(function (resolve, reject) {
                    if (value) {
                        resolve();
                    } else {
                        reject('Tu necesitas ingresar un dato válido!');
                    }
                });
            }
        }).then(function (result) {
            if (typeof mcallback === 'function') { mcallback(result); swal.close(); }
        });
    },
    CargarCombo: function (strUrl, strComboId, callbackOk) {
        $.ajax({
            url: window.location.origin + strUrl,
            data: {},
            success: function (result) {
                if (result !== null) {
                    var html = '';
                    $.each(result, function () {
                        html += "<option value=\"" + this.Id + "\">" + this.Valor + "</option>";
                    });
                    $("#" + strComboId).html(html);
                    $("#" + strComboId).not('.disabled').material_select();
                    if (typeof callbackOk === 'function') { callbackOk.call(this); }
                }
            }
        });
    }
};

var tabla = {
    guardar: function (t, id) {
        var txt = "";
        if (id > 0) txt = $("#" + t + id).text();

        fn.prompt("CREAR " + t, 'text', txt, function (valor) {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: baseUrl + "Mantenimiento/Guardar",
                data: { tabla: t, data: { Id: id, Denominacion: valor } },
                success: function (res) {
                    if (id > 0) {
                        $("#" + t + res).text(valor.toUpperCase());
                    }
                    else {
                        $("#" + t).append("<li class='collection-item'>" +
                      "<div><span id='" + t + res + "'>" + valor.toUpperCase() + "</span><a href='#!' onclick='tabla.guardar(\"" + t + "\"," + res + ");' class='secondary-content'><i class='mdi-content-send'></i></a></div>" +
                    "</li>");
                    }
                    fn.notificar();
                },
                error: function (req, status, err) {
                    alert('Ocurrio un error: ' + err);
                }
            });
        });
    }
};
$(document).ready(function () {
    $('.validar').each(function () {
        $(this).validate({
            errorElement: 'div',
            errorPlacement: function (error, element) {
                var placement = $(element).data('error');
                if (placement) {
                    $(placement).append(error);
                } else {
                    error.insertAfter(element);
                }
            }
        });
    });
    
    //https://github.com/devbridge/jQuery-Autocomplete
    if ($('#autocompletar').data('url') !== null) {
        var txt = $('#autocompletar');
        if (txt.data('boton') !== null) $("#" + txt.data('boton')).attr('disabled', true);

        $.get(txt.data('url'), function (res) {
            txt.autocomplete({
                //serviceUrl: '@Url.Action("listapais", "Home")',
                lookup: res,
                minChars: 2,
                onSelect: function (suggestion) {
                    if ($(this).data('seleccion') !== null) $("#" + $(this).data('seleccion')).val(suggestion.data);
                    if ($(this).data('boton') !== null) $("#" + $(this).data('boton')).attr('disabled', false);
                    if ($(this).data('funcion') !== null) {
                        var funcion = $(this).data('funcion') + '(' + suggestion.data + ');';
                        setTimeout(funcion, 0);
                    }
                },
                onInvalidateSelection: function () {
                    if ($(this).data('seleccion') !== null) $("#" + $(this).data('seleccion')).val(0);
                    if ($(this).data('boton') !== null) $("#" + $(this).data('boton')).attr('disabled', true);
                },
                showNoSuggestionNotice: true,
                noSuggestionNotice: 'Lo siento, no hay resultados'
            });
        });
    }


    $("body").on('click', 'button', function () {

        // Si el boton no tiene el atributo ajax no hacemos nada
        if ($(this).data('ajax') === undefined) return;

        // El metodo .data identifica la entrada y la castea al valor más correcto
        if ($(this).data('ajax') !== true) return;

        var form = $(this).closest("form");
        var buttons = $("button", form);
        var button = $(this);
        var url = form.attr('action');

        if (button.data('confirm') !== undefined) {
            if (button.data('confirm') === '') {
                if (!confirm('¿Esta seguro de realizar esta acción?')) return false;
            } else {
                if (!confirm(button.data('confirm'))) return false;
            }
        }

        if (button.data('delete') !== undefined) {
            if (button.data('delete') === true) {
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
                    if (!button.data('reset') !== undefined) {
                        if (button.data('reset')) form.reset();
                    }
                    else {
                        form.find('input:file').val('');
                    }
                }

                // Mostrar mensaje
                if (r.message !== null) {
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
                if (r.function !== null) {
                    setTimeout(r.function, 0);
                }
                // Redireccionar
                if (r.href !== null) {
                    if (r.href === 'self') window.location.reload(true);
                    else window.location.href = r.href;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                block.remove();
                form.prepend('<div class="alert alert-warning alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' + errorThrown + ' | <b>' + textStatus + '</b></div>');
            }
        });

        return false;
    });
});

jQuery.fn.reset = function () {
    $("input:password,input:file,input:text,textarea", $(this)).val('');
    $("input:checkbox:checked", $(this)).click();
    $("select").each(function () {
        $(this).val($("option:first", $(this)).val());
    });
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

