$(document).ready(function () {
    getEmployeesAsync();
})

function getEmployeesAsync() {
    $.ajax({
        type: "GET",
        url: "/Employee/GetAllEmployeesAsync",
        contentType: "application/json; charset=utf-8",
        success: function (employees) {

            $.each(employees, function (i, employee) {
                $("#employeeTable").append(`<tr>
                    <td>${employee.id}</td>
                    <td>${employee.name}</td>
                    <td>${employee.age}</td>
                    <td>${employee.speciality}</td>
                    <td>${new Date(employee.employementDate).toLocaleDateString()}</td>
                    <td>
                        <button class="btn btn-outline-primary table-btn" type="button">View Tasks</button>
                        <a href="/Employee/Edit?id=${employee.id}" class="btn btn-outline-success table-btn" type="button">Edit</a>
                        <a href="/Employee/Delete?id=${employee.id}" class="btn btn-outline-danger table-btn">Delete</a>
                    </td>
                </tr>`)
            })
        },
        error: function () {
            alert("Cannot load data for Employees! Please, try later.")
        }
    })
}