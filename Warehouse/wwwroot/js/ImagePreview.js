function ShowPreview(event) {
    var output = document.getElementById('imagePreview');
    output.setAttribute("class", "data - fancybox");
    output.hidden = false;
    output.src = URL.createObjectURL(event.target.files[0]);
}