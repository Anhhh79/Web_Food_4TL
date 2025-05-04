function modalDonHang() {
    $('#modalKH').modal('toggle');
}
function loadDonHangKH(event) {
    let btnChiTiet = event.target;
    let row = btnChiTiet.closest("tr");
    let nguoiDungId = row.querySelector("#nguoidungid")?.textContent.trim();

    if (!nguoiDungId) {
        $("#donHangContent").html('<p class="text-center text-danger">Không tìm thấy ID người dùng!</p>');
        $("#modalKH").modal("show");
        return;
    }

    let apiUrl = `/api/hoadon/ds/${nguoiDungId}`;
    console.log("📌 URL gọi API:", apiUrl);

    $('#modalKH').modal('show');

    $("#donHangContent").empty();
    $("#tongThanhToan").text("0 VND");

    $.ajax({
        url: apiUrl,
        type: "GET",
        dataType: "json",
        success: function (response) {
            console.log("📌 Response từ API:", response);

            if (!response.success || !Array.isArray(response.data) || response.data.length === 0) {
                $("#donHangContent").html('<p class="text-center text-muted">Không có đơn hàng nào.</p>');
                return;
            }

            let donHangs = response.data;
            let htmlContent = "";
            let tongTien = 0;

            donHangs.forEach(donHang => {
                if (!donHang.chiTiets || !Array.isArray(donHang.chiTiets) || donHang.chiTiets.length === 0) {
                    return;
                }

                donHang.chiTiets.forEach(chiTiet => {
                    let anhMonAn = chiTiet.monAn?.anhMonAn;
                    let danhMuc = chiTiet.monAn?.danhMuc || "Không xác định";
                    let ngayThanhToan = new Date(donHang.ngayTao).toLocaleDateString();
                    let tongGia = (chiTiet.gia * chiTiet.soLuong).toLocaleString();

                    htmlContent += `
                        <div class="row">
                            <div class="col-12 col-md-4 text-center my-2">
                                <img src="/uploads/monan/${anhMonAn}" alt="" class="img-fluid" style="height:170px; width:170px;">
                            </div>
                            <div class="col-12 col-md-8">
                                <div class="row mt-4 pt-3">
                                    <span class="col-6 fz">${chiTiet.tenMonAn}</span>
                                    <div class="col-6">
                                        <span class="f fw-bold"> Số lượng: </span>
                                        <span class="f">${chiTiet.soLuong}</span>
                                    </div>
                                </div>
                                <div class="row mt-1">
                                    <div class="col-6">
                                        <span class="f fw-bold"> Ngày thanh toán: </span>
                                        <span class="f">${ngayThanhToan}</span>
                                    </div>
                                    <div class="col-6">
                                        <span class="f fw-bold"> Danh mục: </span>
                                        <span class="f">${danhMuc}</span>
                                    </div>
                                </div>
                                <div class="row mt-2">
                                    <div class="col-6">
                                        <span class="f fw-bold"> Giá: </span>
                                        <span class="f text-warning"> ${chiTiet.gia.toLocaleString()} VND</span>
                                    </div>
                                    <div class="col-6">
                                        <span class="f fw-bold"> Tổng: </span>
                                        <span class="f text-warning"> ${tongGia} VND</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <hr class="m-0">
                    `;
                });

                tongTien += donHang.tongTien;
            });

            $("#donHangContent").html(htmlContent);
            $("#tongThanhToan").text(`${tongTien.toLocaleString()} VND`);
        },
        error: function (xhr, status, error) {
            console.error("📌 Lỗi AJAX:", xhr.responseText || error);
            $("#donHangContent").html('<p class="text-center text-danger">Lỗi khi tải đơn hàng!</p>');
        }
    });
}





$(document).ready(function () {
    $.ajax({
        url: "/api/hoadon/danhsach",
        type: "GET",
        dataType: "json",
        success: function (data) {
            let tbody = $("#bodyThongTinKhachHang");
            tbody.empty();

            $.each(data, function (index, item) {
                let row = `
                <tr>
                    <th scope="row">${index + 1}</th>
                    <td>${item.tenNguoiDung}</td>
                    <td>${item.email}</td>
                    <td>${item.soDienThoai}</td>
                    <td>${item.ngayTao}</td>
                    <td class="text-center">${item.tongTien.toLocaleString()} VND</td>
                    <td id="nguoidungid" style="display:none">${item.id}</td>
                    <td>
                        <a class="chitiet cursor me-2 mt-1 d-flex justify-content-end" 
                           style="color: darkblue; font-style: italic; text-decoration: underline;">
                            Chi tiết
                        </a>
                    </td>
                </tr>`;
                tbody.append(row);
            });
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi tải dữ liệu: ", error);
        }
    });

    $("#bodyThongTinKhachHang").on("click", ".chitiet", function (event) {
        loadDonHangKH(event);
    });
});

$(document).ready(function () {
    //tìm kiếm  
    $('#timKiemKhachHang').on('input', function () {
        var query = $(this).val().toLowerCase(); // Lấy giá trị tìm kiếm và chuyển thành chữ thường
        $('#bodyThongTinKhachHang tr').each(function () {
            var rowText = $(this).text().toLowerCase(); // Lấy toàn bộ văn bản của một dòng
            if (rowText.includes(query)) {  // Nếu dòng chứa chuỗi tìm kiếm
                $(this).show();  // Hiển thị dòng
            } else {
                $(this).hide();  // Ẩn dòng
            }
        });
    });
});

document.addEventListener("DOMContentLoaded", function () {
    var userDropdown = document.getElementById("userDropdown");
    var dropdownInstance = new bootstrap.Dropdown(userDropdown);

    userDropdown.addEventListener("click", function (event) {
        event.preventDefault(); // Ngăn load lại trang nếu cần
        dropdownInstance.toggle();
    });
});




