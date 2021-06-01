CKEDITOR.replace("PostBody", {
    height: 400,
    uiColor: '#CCEAEE',
    autoGrow_minHeight: 400,
    autoGrow_maxHeight: 400,
    autoGrow_bottomSpace: 50,
    removePlugins: 'resize'
})




document.getElementById("OpenImgUpload").onclick = function () {
    document.getElementById("file").click();


}

var loadFile = function (event) {
    var image = document.getElementById('OpenImgUpload');
    image.src = URL.createObjectURL(event.target.files[0]);
};
var textboxForCategory = document.getElementById("textboxforcategory");
var dropdownlist = document.getElementById('dropDownListForCategories');
textboxForCategory.value = dropdownlist.options[dropdownlist.selectedIndex].value;
dropdownlist.onchange = function () {
    if (this.options[this.selectedIndex].value == "Other") {
        textboxForCategory.style.display = "block";
        textboxForCategory.value = "";
    } else {
        textboxForCategory.value = this.options[this.selectedIndex].value;
        textboxForCategory.style.display = "none";
    }

}
