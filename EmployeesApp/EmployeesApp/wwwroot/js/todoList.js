$(document).ready(function () {
    var employeeId = new URLSearchParams(window.location.search).get("employeeId");
    employeeId ? getTodosAsync(null, 1, employeeId) : getTodosAsync(null, 1, null);
})

function getTodosAsync(txtSearch, page, employeeId) {
    $.ajax({
        url: "/Todo/GetAllTodosAsync",
        type: "GET",
        data: { txtSearch: txtSearch, page: page, employeeId: employeeId },
        dataType: "json",
        contentType: "application/json;charset=utf-8",
        success: function (result) {
            var str = "";
            $.each(result.todos, function (index, todo) {
                var employee = result.employees.find(emp => emp.id == todo.employeeId);
                str += `<tr>
                    <td>${todo.id}</td>
                    ${getEmployeeData(employee)}
                    <td>${todo.name}</td>
                    <td>${todo.description}</td>
                    <td>${todo.isCompleted ? "Yes" : "No"}</td>
                    <td>
                        <a href="/Todo/Edit?id=${todo.id}" class="btn btn-outline-success table-btn" type="button">Edit</a>
                        <a href="/Todo/Delete?id=${todo.id}" class="btn btn-outline-danger table-btn">Delete</a>
                    </td>
                </tr>`;

                var paginationString = "";
                var pageCurrent = result.pageCurrent;
                var numSize = result.numSize;

                if (pageCurrent > 1) {
                    var pagePrevious = pageCurrent - 1;
                    paginationString += `<li class="page-item"><a href="" class="page-link" data-page=${pagePrevious}>Previous</a></li>`;
                }
                for (i = 1; i <= numSize; i++) {
                    if (i === pageCurrent) {
                        paginationString += `<li class="page-item active"><a href="" class="page-link" data-page=${i}>${pageCurrent}</a></li>`;
                    } else {
                        paginationString += `<li class="page-item"><a href="" class="page-link" data-page=${i}>${i}</a></li>`;
                    }
                }

                if (pageCurrent > 0 && pageCurrent < numSize) {
                    var pageNext = pageCurrent + 1;
                    paginationString += `<li class="page-item"><a href="" class="page-link"  data-page=${pageNext}>Next</a></li>`;
                }

                $("#load-pagination").html(paginationString);
            });
            $("#todoTable").html(str);
        },
        error: function () {
            alert("Cannot load data for Todos! Please, try later.")
        }
    });
}

function getEmployeeData(employee) {
    return employee ? `<td>${employee.name} - ${employee.speciality}</td>` : `<td>Unknown</td>`;
}

$("body").on("click", ".pagination li a", function (event) {
    event.preventDefault();
    var page = $(this).attr("data-page");
    var txtSearch = $(".txtSearch").val();
    txtSearch ? getTodosAsync(txtSearch, page, null) : getTodosAsync(null, page, null);
});

$("#search").click(function () {
    var txtSearch = $(".txtSearch").val();
    txtSearch ? getTodosAsync(txtSearch, 1, null) : getTodosAsync(null, 1, null);
});