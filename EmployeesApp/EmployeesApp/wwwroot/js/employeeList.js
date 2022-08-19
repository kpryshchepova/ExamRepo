$(document).ready(function () {
    getEmployeesAsync(null, 1);
});

function getEmployeesAsync(txtSearch, page) {
    $.ajax({
        url: "/Employee/GetAllEmployeesAsync",
        type: "GET",
        data: { txtSearch: txtSearch, page: page },
        dataType: "json",
        contentType: "application/json;charset=utf-8",
        success: function (result) {
            var str = "";
            $.each(result.data, function (index, employee) {
                str += `<tr>
                    <td>${getAvatar(employee)}</td>
                    <td>${employee.name}</td>
                    <td>${employee.age}</td>
                    <td>${employee.speciality}</td>
                    <td>${new Date(employee.employementDate).toLocaleDateString()}</td>
                    <td>
                        <a href="/Todo/TodoList?employeeId=${employee.id}" class="btn btn-outline-primary table-btn" type="button">View Tasks</a>
                        <a href="/Employee/Edit?id=${employee.id}" class="btn btn-outline-success table-btn" type="button">Edit</a>
                        <a href="/Employee/Delete?id=${employee.id}" class="btn btn-outline-danger table-btn">Delete</a>
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
            $("#employeeTable").html(str);
        },
        error: function () {
            alert("Cannot load data for Employees! Please, try later.")
        }
    });
}

$("body").on("click", ".pagination li a", function (event) {
    event.preventDefault();
    var page = $(this).attr("data-page");
    var txtSearch = $(".txtSearch").val();
    txtSearch ? getEmployeesAsync(txtSearch, page) : getEmployeesAsync(null, page);
});

$("#search").click(function () {
    var txtSearch = $(".txtSearch").val();
    txtSearch ? getEmployeesAsync(txtSearch, 1) : getEmployeesAsync(null, 1);
});

function getAvatar(employee) {
    return employee.imageName
        ? `<img class="rounded-circle avatar-img" src="../img/${employee.imageName}" alt="Avatar">`
        : `<img class="rounded-circle avatar-img" src="../img/avatar.jpg" alt="Avatar">`;
}
