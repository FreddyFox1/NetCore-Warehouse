var dataTable;


$(document).ready(function() {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DTItemsLog').DataTable({
        "order": [[6, "desc"]],
        "ajax": {
            "url": "/api/Logs",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { 'data': 'logItemName', "width": "25%" },
            { 'data': 'logItemArticle', "width": "15%" },
            { 'data': 'logUserName', "width": "15%" },
            { 'data': 'logOldStorage', "width": "15%" },
            { 'data': 'logCurStorage', "width": "15%" },
            {
                'data': 'logDateTransfer', 
                "render": function(data) {
                    return new Date(data).toLocaleDateString('ru', {
                        year: 'numeric',
                        month: 'long',
                        day: 'numeric'
                    });
                },
                "width": "15%"
            },
            {
                'data': 'id', "width": "0%",
                "visible": false,
            },

        ], "language": {
            "emptyTable": "Ничего не найдено"
        },
        "width": "100%"
    });
}
