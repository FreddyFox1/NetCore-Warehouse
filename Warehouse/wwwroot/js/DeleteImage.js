function DeleteImageFromeServer() {
    swal({
        title: "Вы уверены?",
        text: "После удаления фотографии Вы не сможете её восстановить",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: "",
                success: function (data) {
                    if (data.success) {
                        toastr.success("Изображение успешно удалено!");
                    }
                    else {
                        toastr.error("Ошибка удаления. Изображение отсутствует на сервере.");
                    }
                }
            });
        }
    });
}
