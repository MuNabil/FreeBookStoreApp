$(document).ready(function () {
    $('#usersTable').DataTable({
        "autoWidth": false,
        "responsive": true
    });
});

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
            window.location.href = `/Admin/Accounts/DeleteUser?Id=${Id}`;
            Swal.fire(
                DeleteConfirm,
                DeleteRoleMsg,
                SuccessWord
            )
        }
    })
}

Edit = (Id, Name, UserImage, IsActive, Email, userRole) => {
    document.getElementById("title").innerHTML = EditUser;
    document.getElementById("btnSave").value = EditBtn;
    document.getElementById("userId").value = Id;
    document.getElementById("userName").value = Name;
    document.getElementById("userEmail").value = Email;
    document.getElementById("ddlUserRole").value = userRole;
    document.getElementById("userPassword").value = "$$$$$$";
    document.getElementById("userConfirmPassword").value = "$$$$$$";

    var active = document.getElementById("isActive");
    if (IsActive == "True")
        active.checked = true;
    else
        active.checked = false;

    $('#grPassword').hide();
    $('#grConfirmPassword').hide();

    document.getElementById("image").hidden = false;
    document.getElementById("image").src = UserImgPath + "/" + UserImage;
    document.getElementById("imageHide").value = UserImage;
}

Reset = () => {
    document.getElementById("title").innerHTML = CreateNewUser;
    document.getElementById("btnSave").value = SaveBtn;
    document.getElementById("userId").value = "";
    document.getElementById("userName").value = "";
    document.getElementById("userEmail").value = "";
    document.getElementById("ddlUserRole").value = "";
    document.getElementById("userPassword").value = "";
    document.getElementById("userConfirmPassword").value = "";

    document.getElementById("isActive").checked = false;


    $('#grPassword').show();
    $('#grConfirmPassword').show();

    document.getElementById("image").hidden = true;
    document.getElementById("imageHide").value = "";
}

ChangePassword = (id) => {
    document.getElementById("changePasswordId").value = id;
}