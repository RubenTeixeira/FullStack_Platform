
$('input:radio[name="radio"]').change(
    function () {
        if ($(this).is(':checked') && $(this).val() == "algav1") {
            $parent_box = $('div.algav1_box');
            $parent_box.siblings().find('.bottom').slideUp(0, 'swing');
            $parent_box.find('.bottom').slideToggle(200, 'swing');
        }

        if ($(this).is(':checked') && $(this).val() == "algav2") {
            $parent_box = $('div.algav2_box');
            $parent_box.siblings().find('.bottom').slideUp(0, 'swing');
            $parent_box.find('.bottom').slideToggle(200, 'swing');
        }
    });


function validateRequest1(request) {
    var bool = true;
    var text = "@Html.Raw(ClassLibrary.Resources.Percurso.MissingData):\n\n";

    if (request.poiList.length == 0) {
        text += "@Html.Raw(ClassLibrary.Resources.Percurso.POIList_Error) \n";
        bool = false;
    }

    if (!request.horaInicialVisita) {
        text += "@Html.Raw(ClassLibrary.Resources.Percurso.StartHour_Error) \n";
        bool = false;
    }

    if (!request.inclinacaoMax) {
        text += "@Html.Raw(ClassLibrary.Resources.Percurso.MaxInclination_Error) \n";
        bool = false;
    }

    if (!request.kilometrosMax) {
        text += "@Html.Raw(ClassLibrary.Resources.Percurso.MaxKilometers_Error) \n";
        bool = false;
    }

    if (!bool) {
        alert(text);
    }
    return bool;
}

function validateRequest2(request) {
    var bool = true;
    var text = "@Html.Raw(ClassLibrary.Resources.Percurso.MissingData):\n\n";

    if (!request.horaInicialVisita) {
        text += "@Html.Raw(ClassLibrary.Resources.Percurso.StartHour_Error) \n";
        bool = false;
    }

    if (!request.inclinacaoMax) {
        text += "@Html.Raw(ClassLibrary.Resources.Percurso.MaxInclination_Error) \n";
        bool = false;
    }

    if (!request.kilometrosMax) {
        text += "@Html.Raw(ClassLibrary.Resources.Percurso.MaxKilometers_Error) \n";
        bool = false;
    }

    if (!bool) {
        alert(text);
    }
    return bool;
}

function GenerateRouteString(json) {

    var area = document.getElementById("PercursoResult");

    //CONSTRUIR HORA
    var min = Math.floor(json.duracao % 60);
    if (min < 10) {
        min = "0" + min;
    }
    var hora = Math.floor(json.duracao / 60) + ":" + min;

    //CONSTRUIR ROTA
    var text = "";
    var visited = [];

    var selectPOIList = document.getElementById("PercursoPOIs");
    var options = selectPOIList && selectPOIList.options;
    var opt;
    var count = 1;
    for (var j = 0, jLen = json.percurso.length; j < jLen; j++) {
        poi = json.percurso[j];
        for (var i = 0, iLen = options.length; i < iLen; i++) {
            opt = options[i];
            if (opt.value == poi) {
                if (j == 0) {
                    text += "@Html.Raw(ClassLibrary.Resources.Percurso.Begin): " + opt.text + "\n";
                }
                else if (j == json.percurso.length - 1) {
                    text += "@Html.Raw(ClassLibrary.Resources.Percurso.End): " + opt.text + "\n\n";
                } else {
                    if (visited.includes(poi)) {
                        text += "@Html.Raw(ClassLibrary.Resources.Percurso.Passing): " + opt.text + "\n";
                    } else {
                        text += "@Html.Raw(ClassLibrary.Resources.Percurso.Stop) " + count + ": " + opt.text + "\n";
                        count++;
                    }
                }
                visited.push(poi);
            }
        }
    }

    text += "@Html.Raw(ClassLibrary.Resources.Percurso.Duration): " + hora + " min \n";
    text += "@Html.Raw(ClassLibrary.Resources.Percurso.Distance): " + Number(Math.round(json.kilometros + 'e3') + 'e-3') + " Km";

    area.value = text;

    var percursoOrderItem = document.getElementById("Percurso_PercursoPOIsOrder");
    percursoOrderItem.value = json.percurso;

    var finishHourItem = document.getElementById("Percurso_FinishHour");
    finishHourItem.value = json.duracao;

    if (document.getElementById("loader1").style.display == "block")
        document.getElementById("loader1").style.display = "none";

    if (document.getElementById("loader2").style.display == "block")
        document.getElementById("loader2").style.display = "none";
}

function HttpRequest(url, request) {

    var http = new XMLHttpRequest();
    http.open("POST", url, true);

    http.setRequestHeader("Content-type", "application/json");

    var area = document.getElementById("PercursoResult");

    http.onreadystatechange = function () {
        if (http.readyState == 4 && http.status == 200) {
            var json = JSON.parse(http.responseText);

            GenerateRouteString(json);

        }
        if (http.readyState == 4 && http.status == 400) {
            area.value = "@Html.Raw(ClassLibrary.Resources.Percurso.InvalidResult)";
            if (document.getElementById("loader1").style.display == "block")
                document.getElementById("loader1").style.display = "none";

            if (document.getElementById("loader2").style.display == "block")
                document.getElementById("loader2").style.display = "none";
        }
    }
    var data = JSON.stringify(request);
    http.send(data);
}

function RequestAlgav1() {

    var selectPOI = document.getElementById("StartPOI_1");
    var selectTransport = document.getElementById("transport_list_1");

    var poiOrigin = selectPOI.options[selectPOI.selectedIndex].value;
    var transport = selectTransport.options[selectTransport.selectedIndex].value;

    var poiList = [];
    var selectPOIList = document.getElementById("PercursoPOIs");
    var options = selectPOIList && selectPOIList.options;
    var opt;

    for (var i = 0, iLen = options.length; i < iLen; i++) {
        opt = options[i];

        if (opt.selected) {
            poiList.push(opt.value);
        }
    }

    var startHour = document.getElementById("Percurso_StartHour").value;
    var kilometers = document.getElementById("kilometers_1").value;
    var inclination = document.getElementById("inclination_1").value;

    var request = {
        poiOrigem: poiOrigin,
        poiList: poiList,
        horaInicialVisita: startHour,
        inclinacaoMax: inclination,
        tipoVeiculo: transport,
        kilometrosMax: kilometers
    }

    if (validateRequest1(request)) {
        document.getElementById("loader1").style.display = "block";
        HttpRequest("https://10.8.11.86/PVAPI/api/Algav1", request);
    }
}

function RequestAlgav2() {

    var selectPOI = document.getElementById("StartPOI_2");
    var selectTransport = document.getElementById("transport_list_2");
    var selectDuration = document.getElementById("duration");

    var poiOrigin = selectPOI.options[selectPOI.selectedIndex].value;
    var transport = selectTransport.options[selectTransport.selectedIndex].value;
    var duration = selectDuration.options[selectDuration.selectedIndex].value;

    var startHour = document.getElementById("Percurso_StartHour").value;
    var kilometers = document.getElementById("kilometers_2").value;
    var inclination = document.getElementById("inclination_2").value;

    var request = {
        poiOrigem: poiOrigin,
        maxHorasVisita: duration,
        horaInicialVisita: startHour,
        inclinacaoMax: inclination,
        tipoVeiculo: transport,
        kilometrosMax: kilometers
    }

    if (validateRequest2(request)) {
        document.getElementById("loader2").style.display = "block";
        HttpRequest("https://10.8.11.86/PVAPI/api/Algav2", request);
    }
}

function ChangeTextAreaOrig() {
    document.getElementById("PercursoOriginal").style.display = "none";
    document.getElementById("PercursoOriginalSpan").style.color = null;
    document.getElementById("PercursoResult").style.display = "block";
    document.getElementById("PercursoResultSpan").style.color = "#428bca";
}

function ChangeTextAreaRes() {
    document.getElementById("PercursoOriginal").style.display = "block";
    document.getElementById("PercursoOriginalSpan").style.color = "#428bca";
    document.getElementById("PercursoResult").style.display = "none";
    document.getElementById("PercursoResultSpan").style.color = null;
}
