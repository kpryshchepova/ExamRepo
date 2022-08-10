$(document).ready(function () {
    getEmployeesAsync();
})

function getEmployeesAsync() {
    $.ajax({
        type: "GET",
        url: "/Employee/GetAllEmployeesAsync",
        contentType: "application/json; charset=utf-8",
        success: function (employees) {

            $.each(employees, function (employee) {
                $("#employeeTable").append(`<tr>
                    <td>${employee.id}</td>
                    <td>${employee.name}</td>
                    <td>${employee.age}</td>
                    <td>${employee.speciality}</td>
                    <td>${new Date(employee.employementDate).toLocaleDateString() }</td>
                    <td>
                        <button class="btn btn-outline-primary table-btn" type="button">View Tasks</button>
                        <a asp-controller="Employee" asp-action="Edit" asp-route-id="${employee.id}" class="btn btn-outline-success table-btn" type="button">Edit</a>
                        <form class="delete-form" asp-controller="Employee" asp-action="Delete" method="post" asp-route-id="${employee.id}">
                            <button class="btn btn-outline-danger table-btn" type="submit">Delete</button>
                        </form>
                    </td>
                </tr>`)
            })
        },
        error: function () {
            alert("Cannot load data for Employees! Please, try later.")
        }
    })
}