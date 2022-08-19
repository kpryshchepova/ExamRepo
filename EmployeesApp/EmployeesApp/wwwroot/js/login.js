function login(userName, password) {
    $.ajax({
        url: "/LoginUser",
        type: "POST",
        data: JSON.stringify({
            "userName": userName,
            "password": password   
        }),
        dataType: "json",
        contentType: "application/json;charset=utf-8",
        success: function (result) {
            
            if (!result) {
                $("#alertError").attr("style", "display: block");
                return;
            } else {
                $("#alertError").attr("style", "display: none");
                var token = result;
                sessionStorage.setItem("token", token);
                window.location.href = "/Employee/EmployeeList";
            }

        }
    });
}
