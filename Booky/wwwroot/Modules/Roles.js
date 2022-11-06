$(document).ready(function () {
    $('#rolesTable').DataTable({
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
            window.location.href = `/Admin/Accounts/DeleteRole?Id=${Id}`;
            Swal.fire(
                DeleteConfirm,
                DeleteRoleMsg,
                SuccessWord
            )
        }
    })
}

Edit = (Id, Name) => {
    document.getElementById("title").innerHTML = EditUserRole;
    document.getElementById("roleId").value = Id;
    document.getElementById("roleName").value = Name;
    document.getElementById("btnSave").value = EditBtn;
}

Reset = () => {
    document.getElementById("title").innerHTML = CreateNewRole;
    document.getElementById("roleId").value = "";
    document.getElementById("roleName").value = "";
    document.getElementById("btnSave").value = SaveBtn;
}