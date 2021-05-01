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
                "data": "itemID",
                "render": function (data) {
                    return `<div class="p-1 col">
                        <a href="/Items/Edit?id=${data}" class='btn btn-success text-white btn-sm' style='cursor:pointer; width:100%;'>
                            Изменить <svg class="bi bi-pencil-square" width="1em" height="1em" viewBox="0 0 16 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
  <path d="M15.502 1.94a.5.5 0 010 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 01.707 0l1.293 1.293zm-1.75 2.456l-2-2L4.939 9.21a.5.5 0 00-.121.196l-.805 2.414a.25.25 0 00.316.316l2.414-.805a.5.5 0 00.196-.12l6.813-6.814z"/>
  <path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 002.5 15h11a1.5 1.5 0 001.5-1.5v-6a.5.5 0 00-1 0v6a.5.5 0 01-.5.5h-11a.5.5 0 01-.5-.5v-11a.5.5 0 01.5-.5H9a.5.5 0 000-1H2.5A1.5 1.5 0 001 2.5v11z" clip-rule="evenodd"/>
</svg></a></div>
                        <div class="col p-1"><a class='btn btn-danger text-white btn-sm' style='cursor:pointer; width:100%;'
                            onclick=Delete('/api/Items?id='+${data})>
                            Удалить <svg class="bi bi-trash-fill" width="1em" height="1em" viewBox="0 0 16 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
  <path fill-rule="evenodd" d="M2.5 1a1 1 0 00-1 1v1a1 1 0 001 1H3v9a2 2 0 002 2h6a2 2 0 002-2V4h.5a1 1 0 001-1V2a1 1 0 00-1-1H10a1 1 0 00-1-1H7a1 1 0 00-1 1H2.5zm3 4a.5.5 0 01.5.5v7a.5.5 0 01-1 0v-7a.5.5 0 01.5-.5zM8 5a.5.5 0 01.5.5v7a.5.5 0 01-1 0v-7A.5.5 0 018 5zm3 .5a.5.5 0 00-1 0v7a.5.5 0 001 0v-7z" clip-rule="evenodd"/>
</svg></a></div>`;
                }, "width": "10%"
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

function Delete(url) {
    swal({
        title: "Вы уверены?",
        text: "После удаления вы не сможете восстановить запись",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
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

    return `<table width="100%" cellpadding="5" cellspacing="0" border="0">
                <tr class="bg-white">
                     <div class="d-flex">
                        <div class="d-flex flex-row">
                            <p class="text-left WordBreaker"><b>Примечание: </b> ${value}</p>
                        </div>
                     <div class="d-flex">
                         <a class="btn-sm flex-row-reverse btn btn-info mr-2 text-center text-white" onclick=CreateTask('api/Bitrix/CreateTask?id'+${d.itemID})>Создать задачу</a>                    
                         <a class="btn-sm btn btn-info mr-2 text-center text-white" onclick=Copy('api/Items/CreateCopy?id='+${d.itemID})>Копировать</a>
                     </div>
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

function CreateTask(url) {
    swal({
        title: "Создать задачу в Bitrix24?",
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