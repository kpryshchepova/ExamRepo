$(document).ready(function () {
    getTodosAsync();
})

function getTodosAsync() {
    $.ajax({
        type: "GET",
        url: "/Todo/GetAllTodosAsync",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var employees = data.employees;
            var todos = data.todos;
            $.each(todos, function (i, todo) {
                var employee = employees.find(emp => emp.id == todo.employeeId);
                $("#todoTable").append(`<tr>
                    <td>${todo.id}</td>
                    ${getEmployeeData(employee)}
                    <td>${todo.name}</td>
                    <td>${todo.description}</td>
                    <td>${todo.isCompleted ? "Yes" : "No"}</td>
                    <td>
                        <a href="/Todo/Edit?id=${todo.id}" class="btn btn-outline-success table-btn" type="button">Edit</a>
                        <a href="/Todo/Delete?id=${todo.id}" class="btn btn-outline-danger table-btn">Delete</a>
                    </td>
                </tr>`)
            })
        },
        error: function () {
            alert("Cannot load data for Todos! Please, try later.")
        }
    })
}

function getEmployeeData(employee) {
    return employee ? `<td>${employee.name} - ${employee.speciality}</td>` : `<td>Unknown</td>`;
}