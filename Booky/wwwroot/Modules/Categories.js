$(document).ready(function () {
    $('#rolesTable').DataTable({
        "autoWidth": false,
        "responsive": true
    });
});

document.getElementById("defaultOpen").click();

function Delete(Id) {
    Swal.fire({
        title: AreYouSure,
        text: AreYouSureMsg,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: YesBtn,
        cancelButtonText: NoBtn
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = `/Admin/Categories/Delete?Id=${Id}`;
            Swal.fire(
                DeleteConfirm,
                DeleteCategoryMsg,
                SuccessWord
            )
        }
    })
}

function DeleteLog(Id) {
    Swal.fire({
        title: AreYouSure,
        text: AreYouSureMsg,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: YesBtn,
        cancelButtonText: NoBtn
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = `/Admin/Categories/DeleteLog?Id=${Id}`;
            Swal.fire(
                DeleteConfirm,
                DeleteCategoryMsg,
                SuccessWord
            )
        }
    })
}

Edit = (Id, Name, description) => {
    document.getElementById("title").innerHTML = EditCategory;
    document.getElementById("categoryId").value = Id;
    document.getElementById("categoryName").value = Name;
    document.getElementById("categoryDescription").value = description;
    document.getElementById("btnSave").value = EditBtn;
}

Reset = () => {
    document.getElementById("title").innerHTML = CreateNewCategoryBtn;
    document.getElementById("categoryId").value = "";
    document.getElementById("categoryName").value = "";
    document.getElementById("categoryDescription").value = "";
    document.getElementById("btnSave").value = SaveBtn;
}


function openCity(evt, cityName) {
    // Declare all variables
    var i, tabcontent, tablinks;

    // Get all elements with class="tabcontent" and hide them
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }

    // Get all elements with class="tablinks" and remove the class "active"
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

    // Show the current tab, and add an "active" class to the button that opened the tab
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
}