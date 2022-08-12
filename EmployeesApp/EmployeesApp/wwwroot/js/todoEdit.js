$(document).ready(function () {
    var id = new URLSearchParams(window.location.search).get("id");
    if (id) {
        disableElements();
        getTodoAsync(id);
    }
})

function getTodoAsync(id) {
    var employees = $.ajax({
        type: "GET",
        url: "/Employee/GetAllEmployeesListAsync",
        contentType: "application/json; charset=utf-8",
        success: function (data) { return data },
        error: function () {
            alert("Cannot load data for current Employees! Please, try later.")
        }
    });
    var todo = $.ajax({
        type: "GET",
        url: "/Todo/GetByIdAsync",
        data: { id: id },
        contentType: "application/json; charset=utf-8",
        success: function (data) { return data },
        error: function () {
            alert("Cannot load data for current Todo! Please, try later.")
        }
    });

    $.when(employees, todo).done(function (employees, todo) {
        employees[0].forEach(employee => {
            $("#inputEmployee").append(getOption(employee, todo[0].employeeId));
        });
        $("#inputName").val(todo[0].name);
        $("#inputDescription").val(todo[0].description);
        if (todo[0].isCompleted) {
            $("#inputIsCompleted").attr("checked", "checked");
        }
        enableElements();
    });
}

function disableElements() {
    ["#inputName", "#inputDescription", "#inputIsCompleted", "#inputEmployee", "#btnUpdate"].forEach(item => {
        $(item).attr("disabled", "disabled");
    });
}

function enableElements() {
    ["#inputName", "#inputDescription", "#inputIsCompleted", "#inputEmployee", "#btnUpdate"].forEach(item => {
        $(item).removeAttr("disabled");
    });
}

function getOption(employee, todoEmpId) {
    return employee.id == todoEmpId
        ? `<option selected value="${employee.id}">${employee.name} - ${employee.speciality}</option>`
        : `<option value="${employee.id}">${employee.name} - ${employee.speciality}</option>`
}