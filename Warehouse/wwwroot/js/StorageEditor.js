let ItemID;
let ItemStorage;
document.addEventListener('DOMContentLoaded', function () {
    GetItemInfo();
    if (ItemStorage === 'Архив') {
        SetActivateButton(false);
    }
}, false);

function SetActivateButton(e) {
    document.getElementById("backFromArchive").disabled = e;
}

function GetItemInfo() {
    ItemStorage = document.getElementById("_storageItem").value;
    ItemID = document.getElementById("_itemID").value;
}

function DeleteFromArchive() {
    $.ajax({
        type: "GET",
        url: '/api/archive?id=' + ItemID,
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                document.getElementById("currentStorage").innerHTML = "Текущий склад: " + data.text;
                SetActivateButton(true);
            }
            else {
                toastr.error(data.message);
            }
        }
    });
}