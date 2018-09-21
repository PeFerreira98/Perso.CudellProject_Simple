$(document).ready(function () {

    var faturasPendentesTable = $("#faturasPendentesTable").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once  
        "info": false,
        "lengthChange": false,
        "oLanguage": {
            "sProcessing": "<img style=\"margin-left: 250px;\" src=\"/images/loading1.gif\">"
        },

        "ajax": {
            "url": "/Table/LoadFirstTableData",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs":
        [{
            "targets": [1],
            "visible": false,
            "searchable": false
        }],  
        "columns": [
            
            { "data": "fornecedor", "name": "fornecedor" },
            { "data": "faturaID", "name": "faturaID" },
            { "data": "dataFatura", "name": "dataFatura" },
            { "data": "dataVencimento", "name": "dataVencimento" },
            { "data": "valor", "name": "valor" },
            {
                "title": "Opcoes",
                "sClass": "editarFaturaColumn",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return '<a href="EditFatura/' + full.faturaID + '"class="editFatura">Editar</a>';
                }
            }
        ]
    });

    var ownFaturasTable = $("#ownFaturasTable").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once
        "info": false,
        "lengthChange": false,
        "oLanguage": {
            "sProcessing": "<img style=\"margin-left: 250px;\" src=\"/images/loading1.gif\">"
        },
        
        "ajax": {
            "url": "/Table/LoadSecondTableData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "fornecedor", "name": "fornecedor" },
            { "data": "dataFatura", "name": "dataFatura" },
            { "data": "dataVencimento", "name": "dataVencimento" },
            { "data": "valor", "name": "valor" },
            { "data": "estado", "name": "estado" }
        ]
    });

    $('input.global_filter').on('keyup click', function () {
        filterGlobal(faturasPendentesTable, ownFaturasTable);
    });

    $('select.sel_page_res').on('change', function () {
        resultsNumber(faturasPendentesTable, ownFaturasTable);
    });
});

function filterGlobal(faturas1, faturas2) {
    faturas1.search($('#global_filter').val()).draw();
    faturas2.search($('#global_filter').val()).draw();
}

function resultsNumber(faturas1, faturas2) {
    faturas1.page.len($('#sel_page_res').val()).draw();
    faturas2.page.len($('#sel_page_res').val()).draw();
}