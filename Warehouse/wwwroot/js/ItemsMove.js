var dataTable;
var Storage = "Склад не указан";
$(document).ready(function () {
    LoadDataTable();
});
function LoadDataTable() {
    dataTable = $('#TableItems').DataTable({
        "order": [[1, "asc"]],
        "ajax": {
            "url": "/api/MoveItems",
            "type": "GET",
            "datatype": "json"
        },
        "createdRow": function (row, data, dataIndex) {
            if (data.itemProtect) {
                $('td', row).css('background-color', '#fffbb8');
            }
        },
        "columns": [
            {
                "data": "itemPhoto",
                "render": function (data) {
                    return `<a href="/img/photo/${data}" data-fancybox><img loading="lazy" class="tbImg" src="/img/icons/${data}" /></a>`
                },
                "width": "3%", "height": "auto",
                "bSortable": false
            },
            {
                "data": "itemName",
                "render": function (data) {
                    return `<span class="WordBreaker">${data}</span>`;
                },
                "width": "9%",
            },
            { "data": "itemArticle", "width": "13%" },
            {
                "data": "itemStorageID", "width": "1%",
                "render": function (data) {
                    if (data === "Ремонт") return `<span class="badge badge-pill badge-warning tableFont">${data}</span>`;
                    else if (data === "Своя формовка") return `<span class="bg-danger badge badge-pill badge-primary tableFont">${data}</span>`;
                    else if (data === "Архив" || data === "У заказчика") return `<span class="badge badge-pill badge-primary tableFont">${data}</span>`;
                    else return `<span class="badge badge-pill badge-secondary tableFont">${data}</span>`;
                }
            },
            {
                "data": "itemSizes",
                "render": function (data) {
                    if (data != null) {
                        return `<span class="WordBreaker"">${data}</span>`;
                    }
                    else return `<span>-</span>`;
                },
                "width": "5%",
            },
            { "data": "itemCell", "width": "1%" },
            {
                "data": "itemID",
                "render": function (data) {
                    return `<select onchange="SetStorage(this)" name="Storage${data}" class="custom-select form-control">
<optgroup label="Склады">                                             
<option selected="selected" value="Не выбрано">Склад не указан</option>
                                            <option value="Модельный цех">Модельный цех</option>
                                            <option value="Альком">Альком</option>
                                            <option value="Автозавод">Автозавод</option>
<optgroup class="dropdown-divider">
                                            <optgroup label="Прочее">                                            
                                            <option value="Ремонт">Ремонт</option>
                                            <option value="У заказчика">У заказчика</option>
                                            <option value="Архив">Архив</option>
                                            <option value="Своя формовка">Своя формовка</option>
                                        </select>`;
                }, "width": "18%"
            },
            {
                "data": "itemID",
                "render": function (data) {
                    return `<div><a class='btn btn-dark text-white btn-sm' style='cursor:pointer; width:100%;'
                                    onclick=ChangeItemPos('/api/MoveItems?id=${data}')>Переместить</div>`;
                }, "width": "2%"
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

function SetStorage(item) { if (item != null) Storage = item.value; }
function GetExtensionInfo() { return stringUrl = '&StorageName=' + Storage + '&UserName=' + document.getElementById('UserName').innerHTML; }

function ChangeItemPos(url) {
    swal({
        title: "Вы уверены?",
        text: "Переместить матрицу на выбранный склад?",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url + GetExtensionInfo(),
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else { toastr.error(data.message); }
                }
            });
        } Storage = "Склад не указан"
    });
}