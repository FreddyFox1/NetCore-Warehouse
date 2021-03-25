var dataTable;
var filter = "All";

$(document).ready(function () {
    loadCategoryFilter();
    loadDataTable();
});

function loadCategoryFilter() {
    if ((result = localStorage.getItem("category-filter")) != null) {
        $('#inputGroupSelect04 option[value=' + "\"" + result.innerHTML + "\"" + ']').prop('selected', 'selected');
        filter = result;
    }
    else filter = "All";
}

function loadDataTable() {
    dataTable = $('#DT_Items').DataTable({
        "bStateSave": true,
        "order": [[2, "asc"]],
        "ajax": {
            "url": '/api/Items',
            "data": {
                "filter": function () { return filter; }
            },
            "type": "GET",
            "datatype": "json",
        },
        "createdRow": function (row, data, dataIndex) {
            if (data.itemProtect) {
                $('td', row).css('background-color', '#fffbb8');
            }
        },
        "columns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": "null",
                "defaultContent": '',
                "width": "1%"
            },
            {
                "data": "itemPhoto",
                "render": function (data) {
                    return `<a href="/img/photo/${data}" data-fancybox><img loading="lazy" class="tbImg" src="/img/icons/${data}" /></a>`
                },
                "width": "5%",
                "height": "auto",
                "orderable": false,
                "bSortable": false
            },
            {
                "data": "itemName",
                "render": function (data) {
                    return `<span class="WordBreaker">${data}</span>`;
                },
                "width": "13%",
            },
            { "data": "itemArticle", "width": "11%" },
            {
                "data": "itemSizes",
                "render": function (data) {
                    if (data != null) {
                        return `<span class="WordBreaker">${data}</span>`;
                    }
                    else return `<span>-</span>`;
                },
                "width": "6%"
            },
            {
                "data": "itemStorageID", "width": "6%",
                "render": function (data) {
                    if (data === "Ремонт") return `<span class="badge badge-pill badge-warning tableFont">${data}</span>`;
                    else if (data === "Своя формовка") return `<span class="badge badge-pill badge-primary tableFont">${data}</span>`;
                    else if (data === "Архив" || data === "У заказчика") return `<span class="badge badge-pill badge-primary tableFont">${data}</span>`;
                    else return `<span class="badge badge-pill badge-secondary tableFont">${data}</span>`;
                }
            },
            {
                "data": "itemCount", "width": "1%",
                "render": function (data) {
                    if (data != null) {
                        return `<div>${data}</div>`;
                    }
                    else return `<div>-</div>`;
                }
            },
            {
                "data": "itemCell", "width": "1%",
                "render": function (data) {
                    if (data != null) {
                        return `<div>${data}</div>`;
                    }
                    else return `<div>-</div>`;
                }
            },

            {
                "data": "itemDescription",
                "width": "0%",
                "orderable": false,
                'visible': false
            },

            {
                "data": "itemProtect",
                "width": "0%",
                "orderable": false,
                'visible': false,
            },
        ],
        "language": {
            "emptyTable": "Ничего не найдено"
        },
        "width": "100%",
        "sDom": "<'row pb-3 align-items-center justify-content-center'<'col-sm 'l><p><'col-sm'f>>" +
            "<'row'<'col'tr>>" +
            "<'row pt-2'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
        "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "Все"]]
    });
}

function ShowCategory(_filter) {
    localStorage.setItem("category-filter", _filter);
    filter = _filter;
    dataTable.ajax.reload();
    if (filter != "All") { toastr.success(`Позиции из категории ${filter} загружены`); }
};

$("#ResetCategory").click(function () {
    localStorage.setItem("category-filter", "All");
    filter = "All";
    dataTable.ajax.reload();
    toastr.success("Фильтр сброшен");
    $("#inputGroupSelect04").val('All').attr("selected", true);
});

function format(d) {
    console.log(d.itemID);
    if (d.itemDescription != null)
        value = d.itemDescription;
    else
        value = "Нет примечаний";

    return `<table width="100%" cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;>
                <tr class="bg-white">
                     <div class="d-flex">
                         <p class="text-left WordBreaker"><b>Примечание: </b> ${value}</p>
                    </div>
                </tr>
            </table>`;
}


$('#DT_Items').on('click', 'td.details-control', function () {
    var tr = $(this).closest('tr');
    var row = dataTable.row(tr);
    if (row.child.isShown()) {
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        row.child(format(row.data())).show();
        tr.addClass('shown');
    }
});

function Copy(url) {
    swal({
        title: "Скопировать матрицу?",
        icon: "success",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "GET",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else
                        toastr.error(data.message);
                }
            });
        }
    });
}