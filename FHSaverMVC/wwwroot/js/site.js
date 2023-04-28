window.onload = function () {
    document.getElementById("downloadLink").addEventListener("click", function (event) {
        event.preventDefault();
        var select = document.getElementById("folderchooser");
        var value = select.options[select.selectedIndex].value;
        window.location.href = "/Home/DownloadFile?folderName=" + encodeURIComponent(value);
    });
};

