$(document).ready(function () {
    var id = new URLSearchParams(window.location.search).get("id");
    if (id) {
        disableElements();
        getTodoAsync(id);
    }
})

function getTodoAsync(id) {
    $.ajax({
        type: "GET",
        url: "/Employee/GetByIdAsync",
        data: {id: id},
        contentType: "application/json; charset=utf-8",
        success: function (employee) {
            $("#inputName").val(employee.name);
            $("#inputAge").val(employee.age);
            $("#inputSpeciality").val(employee.speciality);
            $("#inputEmployementDate").val(new Date(employee.employementDate).toISOString().split('T')[0]);
            if (employee.imageName) {
                $("#avatarImg").removeAttr("src");
                $("#avatarImg").attr("src", `../img/${employee.imageName}`);
            }
            enableElements();
        },
        error: function () {
            alert("Cannot load data for current Employee! Please, try later.")
        }
    })
}

function disableElements() {
    ["#inputName", "#inputAge", "#inputSpeciality", "#inputEmployementDate", "#btnUpdate"].forEach(item => {
        $(item).attr("disabled", "disabled");
    });
}

function enableElements() {
    ["#inputName", "#inputAge", "#inputSpeciality", "#inputEmployementDate", "#btnUpdate"].forEach(item => {
        $(item).removeAttr("disabled");
    });
}

function showImage(event) {
    var imageFile = event.target.files[0];
    $("#avatarImg").removeAttr("src");
    $("#avatarImg").attr("src", window.URL.createObjectURL(imageFile));
} 