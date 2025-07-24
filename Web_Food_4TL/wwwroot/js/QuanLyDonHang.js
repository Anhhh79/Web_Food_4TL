$(document).ready(function () {
    showOrderList();
    showDangGiaoList();
    showHoanThanhList();
    showYeuCauDoiTraList();
    showDoiTraList();
})

function modalXemLyDo() {
    $('#exchangeReasonModal').modal('toggle');
}

function modalTuChoi() {
    $('#rejectReasonModal').modal('toggle');
}

function thongBaoThanhCong(message) {
    Swal.fire({
        icon: 'success',
        title: 'Thành công',
        text: message
    });
}

function showConfirmModal(message) {
    Swal.fire({
        icon: 'question',
        title: 'Yêu cầu xác nhận',
        text: message,
        showCancelButton: true,
        confirmButtonText: 'Đồng ý',
        cancelButtonText: 'Hủy bỏ'
    })
}

// 2. Validate và gọi Ajax
function checkLyDo() {
    var lyDo = $('#rejectReason').val().trim();
    if (!lyDo) {
        $('#errorMessage').text('Vui lòng nhập lý do từ chối!').show();
        return;
    }
    $('#errorMessage').hide();
    var idDonHang = $('#hdnRejectOrderId').val();
    console.log('ID đơn hàng:', idDonHang);
    RejectDoiTra(idDonHang);
}

function resetDataLyDo() {
    $('#rejectReason').val('');
    $('#errorMessage').hide();
}

// hàm hiển thị danh sách đơn hàng chờ xác nhận
function showOrderList() {
    $.ajax({
        url: '/Admin/QuanLyDonHang/GetDonHangChoXacNhan', // Cập nhật đường dẫn đúng với route trong project của bạn
        method: 'GET',
        success: function (response) {
            if (response.success) {
                let donHangList = response.data;
                let html = '';

                donHangList.forEach(function (donHang, index) {
                    html += `
                      <tr>
    <td>${index + 1}</td>
    <td>
        ${donHang.ngayDatHang}
    </td>
    <td>${donHang.tenKhachHang}</td>
    <td>${donHang.soDienThoai || 'Không có số'}</td>
    <td class="text-nowrap text-truncate text-center"
        style="max-width: 200px; cursor: help;"
        title="${donHang.diaChiGiaoHang}">
        ${donHang.diaChiGiaoHang}
    </td>
    <td class="text-center">${donHang.tongTien.toLocaleString('vi-VN')} VND</td>
     <td>
    <div class="d-flex">
        <a class="btn btn-primary btn-sm" onclick = "chiTietDonHangModal('${donHang.id}'), modalXemChiTiet()">
            Chi tiết
        </a>
        <a class="btn btn-success btn-sm ms-2" onclick="confirmOrder('${donHang.id}')">
            Xác nhận
        </a>
    </div>
</td>

</tr>

                    `;
                });

                $('#listChoXacNhan').html(html); // Đảm bảo có một tbody với id="orderTableBody"
            } else {
                $('#listChoXacNhan').html(`<tr><td colspan="7" class="text-center text-danger">${response.message}</td></tr>`);
            }
        },
        error: function () {
            $('#listChoXacNhan').html('<tr><td colspan="7" class="text-center text-danger">Lỗi khi lấy dữ liệu từ server.</td></tr>');
        }
    });
}

// hàm hiển thị danh sách đơn hàng đang giao
function showDangGiaoList() {
    $.ajax({
        url: '/Admin/QuanLyDonHang/GetDonHangDangGiao', // Cập nhật đường dẫn đúng với route trong project của bạn
        method: 'GET',
        success: function (response) {
            if (response.success) {
                let donHangList = response.data;
                let html = '';

                donHangList.forEach(function (donHang, index) {
                    html += `
                      <tr>
  <td>${index + 1}</td>
  <td>${donHang.ngayDatHang}</td>
  <td>${donHang.tenKhachHang}</td>
  <td>${donHang.soDienThoai || 'Không có số'}</td>
  <td class="text-nowrap text-truncate"
      style="max-width: 200px; cursor: help;"
      title="${donHang.diaChiGiaoHang}">
    ${donHang.diaChiGiaoHang}
  </td>
  <td class="text-center">
    ${donHang.tongTien.toLocaleString('vi-VN')} VND
  </td>
  <!-- Ô chứa 2 nút -->
  <td>
    <div class="d-flex gap-2">
     <a class="btn btn-primary btn-sm" onclick = "chiTietDonHangModal('${donHang.id}'), modalXemChiTiet()">
            Chi tiết
        </a>
      <a class="btn btn-success btn-sm" onclick="completeOrder('${donHang.id}')">
        Đã giao
      </a>
    </div>
  </td>
</tr>


                    `;
                });

                $('#listDangGiao').html(html); // Đảm bảo có một tbody với id="orderTableBody"
            } else {
                $('#listDangGiao').html(`<tr><td colspan="7" class="text-center text-danger">${response.message}</td></tr>`);
            }
        },
        error: function () {
            $('#listDangGiao').html('<tr><td colspan="7" class="text-center text-danger">Lỗi khi lấy dữ liệu từ server.</td></tr>');
        }
    });
}

//hàm hiển thị đơn hàng hoàn thành
function showHoanThanhList() {
    $.ajax({
        url: '/Admin/QuanLyDonHang/GetDonHangHoanThanh', // Cập nhật đường dẫn đúng với route trong project của bạn
        method: 'GET',
        success: function (response) {
            if (response.success) {
                let donHangList = response.data;
                let html = '';

                donHangList.forEach(function (donHang, index) {
                    html += `
                      <tr>
    <td>${index + 1}</td>
    <td>
        ${donHang.ngayDatHang}
    </td>
    <td>${donHang.tenKhachHang}</td>
    <td>${donHang.soDienThoai || 'Không có số'}</td>
    <td class="text-nowrap text-truncate text-center"
        style="max-width: 200px; cursor: help;"
        title="${donHang.diaChiGiaoHang}">
        ${donHang.diaChiGiaoHang}
    </td>
    <td class="text-center">${donHang.tongTien.toLocaleString('vi-VN')} VND</td>
    <td><a class="btn btn-primary btn-sm" onclick = "chiTietDonHangModal('${donHang.id}'), modalXemChiTiet()">
            Chi tiết
        </a></td>
</tr>

                    `;
                });

                $('#listHoanThanh').html(html); // Đảm bảo có một tbody với id="orderTableBody"
            } else {
                $('#listHoanThanh').html(`<tr><td colspan="7" class="text-center text-danger">${response.message}</td></tr>`);
            }
        },
        error: function () {
            $('#listHoanThanh').html('<tr><td colspan="7" class="text-center text-danger">Lỗi khi lấy dữ liệu từ server.</td></tr>');
        }
    });
}

//hàm hiển thị đơn hàng yêu cầu đổi trả
function showYeuCauDoiTraList() {
    $.ajax({
        url: '/Admin/QuanLyDonHang/GetDonHangYeuCauDoiTra', // Cập nhật đường dẫn đúng với route trong project của bạn
        method: 'GET',
        success: function (response) {
            if (response.success) {
                let donHangList = response.data;
                let html = '';

                donHangList.forEach(function (donHang, index) {
                    html += `
                      <tr>
    <td>${index + 1}</td>
    <td>
        ${donHang.ngayDatHang}
    </td>
    <td>${donHang.tenKhachHang}</td>
    <td>${donHang.soDienThoai || 'Không có số'}</td>
    <td class="text-nowrap text-truncate"
        style="max-width: 200px; cursor: help;"
        title="${donHang.diaChiGiaoHang}">
        ${donHang.diaChiGiaoHang}
    </td>
    <td class="text-center">${donHang.tongTien.toLocaleString('vi-VN')} VND</td>
                                     <td>
    <div class="d-flex gap-2">
     <a class="btn btn-primary btn-sm" onclick = "chiTietDonHangModal('${donHang.id}'), modalXemChiTiet()">
            Chi tiết
        </a>
      <a href="#" onclick="showReason('${donHang.lydo}'), openAcceptModal(${donHang.id})">
                                            Xem lý do
                                        </a>
    </div>
  </td>
</tr>

                    `;
                });

                $('#listYeuCauDoiTra').html(html); // Đảm bảo có một tbody với id="orderTableBody"
                
            } else {
                $('#listYeuCauDoiTra').html(`<tr><td colspan="7" class="text-center text-danger">${response.message}</td></tr>`);
            }
        },
        error: function () {
            $('#listYeuCauDoiTra').html('<tr><td colspan="7" class="text-center text-danger">Lỗi khi lấy dữ liệu từ server.</td></tr>');
        }
    });
}

//hàm hiển thị modal lý do
function showReason(lyDo) {
    $('#exchangeReasonModal').modal('show'); // Hiển thị modal
    $('#exchangeReason').val(lyDo);
}

//hàm hiển thị đơn hàng đổi trả
function showDoiTraList() {
    $.ajax({
        url: '/Admin/QuanLyDonHang/GetDonHangDoiTra', // Cập nhật đường dẫn đúng với route trong project của bạn
        method: 'GET',
        success: function (response) {
            if (response.success) {
                let donHangList = response.data;
                let html = '';

                donHangList.forEach(function (donHang, index) {
                    html += `
                      <tr>
    <td>${index + 1}</td>
    <td>
        ${donHang.ngayDatHang}
    </td>
    <td>${donHang.tenKhachHang}</td>
    <td>${donHang.soDienThoai || 'Không có số'}</td>
    <td class="text-nowrap text-truncate text-center"
        style="max-width: 200px; cursor: help;"
        title="${donHang.diaChiGiaoHang}">
        ${donHang.diaChiGiaoHang}
    </td>
    <td class="text-center">${donHang.tongTien.toLocaleString('vi-VN')} VND</td>
    <td><a class="btn btn-primary btn-sm" onclick = "chiTietDonHangModal('${donHang.id}'), modalXemChiTiet()">
            Chi tiết
        </a></td>
</tr>

                    `;
                });

                $('#listDoiTra').html(html); // Đảm bảo có một tbody với id="orderTableBody"
            } else {
                $('#listDoiTra').html(`<tr><td colspan="7" class="text-center text-danger">${response.message}</td></tr>`);
            }
        },
        error: function () {
            $('#listDoiTra').html('<tr><td colspan="7" class="text-center text-danger">Lỗi khi lấy dữ liệu từ server.</td></tr>');
        }
    });
}

//Hàm xử lý đơn chờ xác nhận
function confirmOrder(orderId) {
    Swal.fire({
        icon: 'question',
        title: 'Yêu cầu xác nhận',
        text: "Xác nhận đơn hàng, đơn hàng sẽ được giao đến khách hàng!",
        showCancelButton: true,
        confirmButtonText: 'Đồng ý',
        cancelButtonText: 'Hủy bỏ'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Admin/QuanLyDonHang/XuLyDonHangChoXacNhan', // Cập nhật đường dẫn đúng với route trong project của bạn
                method: 'POST',
                data: { id: orderId },
                success: function (response) {
                    if (response.success) {
                        thongBaoThanhCong('Đơn hàng đã được xác nhận thành công!');
                        showOrderList(); // Cập nhật lại danh sách đơn hàng
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Lỗi',
                            text: response.message
                        });
                    }
                },
                error: function () {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: 'Không thể xác nhận đơn hàng. Vui lòng thử lại sau.'
                    });
                }
            });
        }
    });
}

//hàm xử lý đơn hàng đang giao
function completeOrder(orderId) {
    Swal.fire({
        icon: 'question',
        title: 'Yêu cầu xác nhận',
        text: "Xác nhận đơn hàng đã được giao đến khách hàng!",
        showCancelButton: true,
        confirmButtonText: 'Đồng ý',
        cancelButtonText: 'Hủy bỏ'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Admin/QuanLyDonHang/XuLyDonHangDangGiao', // Cập nhật đường dẫn đúng với route trong project của bạn
                method: 'POST',
                data: { id: orderId },
                success: function (response) {
                    if (response.success) {
                        thongBaoThanhCong('Đơn hàng đã được xác nhận thành công!');
                        showDangGiaoList(); // Cập nhật lại danh sách đơn hàng
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Lỗi',
                            text: response.message
                        });
                    }
                },
                error: function () {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: 'Không thể xác nhận đơn hàng. Vui lòng thử lại sau.'
                    });
                }
            });
        }
    });
}

//Hàm xác nhận đơn đổi trả
function AcceptDoiTra(idDonHang) {
    Swal.fire({
        icon: 'question',
        title: 'Yêu cầu xác nhận',
        text: "Xác nhận đổi trả, đơn hàng sẽ được chuyển thành đơn đổi trả!",
        showCancelButton: true,
        confirmButtonText: 'Đồng ý',
        cancelButtonText: 'Hủy bỏ'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Admin/QuanLyDonHang/AcceptYeuCauDoiTra', // Cập nhật đường dẫn đúng với route trong project của bạn
                method: 'POST',
                data: { id: idDonHang },
                success: function (response) {
                    if (response.success) {
                        $('#exchangeReasonModal').modal('hide');
                        Swal.fire({
                            icon: 'success',
                            title: 'Thành công',
                            text: 'Đơn hàng đã được xác nhận thành công!'
                        });
                        showYeuCauDoiTraList();
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Lỗi',
                            text: response.message
                        });
                    }
                },
                error: function () {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: 'Không thể xác nhận đơn hàng. Vui lòng thử lại sau.'
                    });
                }
            });
        }
    });
}

// Gọi khi nhấn nút "Đồng ý đổi trả" trên từng dòng
function openAcceptModal(id) {
    $('#hdnRejectOrderId').val(id);
    // Bỏ mọi handler cũ để không gán đôi sự kiện
    $('#btnAcceptDoiTra').off('click')
        .on('click', function () {
            // Khi nhấn "Đồng ý" trong modal:
            AcceptDoiTra(id);
        });
    
}

//hàm từ chối đơn đổi trả
function RejectDoiTra(idHd) {
    var lyDoTc = $('#rejectReason').val().trim();
    Swal.fire({
        icon: 'question',
        title: 'Yêu cầu xác nhận',
        text: "Từ chối đổi trả, đơn hàng này sẽ được chuyển thành đơn hoàn thành!!",
        showCancelButton: true,
        confirmButtonText: 'Đồng ý',
        cancelButtonText: 'Hủy bỏ'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Admin/QuanLyDonHang/RejectYeuCauDoiTra', // Cập nhật đường dẫn đúng với route trong project của bạn
                method: 'POST',
                data: { id: idHd, lyDo: lyDoTc },
                success: function (response) {
                    if (response.success) {
                        modalTuChoi();
                        Swal.fire({
                            icon: 'success',
                            title: 'Thành công',
                            text: 'Đơn hàng đã được xác nhận thành công!'
                        });
                        showYeuCauDoiTraList();
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Lỗi',
                            text: response.message
                        });
                    }
                },
                error: function () {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: 'Không thể xác nhận đơn hàng. Vui lòng thử lại sau.'
                    });
                }
            });
        }
    });
}

//Hàm mở modal 
function modalXemChiTiet() {
    $('#modalDonHangAd').modal('toggle');
}

// Hàm hiển thị danh sách chi tiet đơn hàng
function chiTietDonHangModal(id) {
    // URL API: truyền thẳng id đơn hàng
    const apiUrl = `/Admin/QuanLyDonHang/GetDanhSachChiTiet/${id}`;

    // Reset nội dung modal
    $("#donHangContent").empty();
    $("#tongThanhToan").text("0 VND");

    // Mở modal ngay (đỡ phải gọi nhiều chỗ)

    $.ajax({
        url: apiUrl,
        type: "GET",
        dataType: "json",
        success: function (response) {

            if (!response.success || !Array.isArray(response.data) || response.data.length === 0) {
                $("#donHangContent").html(
                    '<p class="text-center text-muted">Không có đơn hàng nào.</p>'
                );
                return;
            }

            let htmlContent = "";
            let tongTien = 0;

            response.data.forEach(donHang => {
                // cộng tổng tiền của đơn
                tongTien += donHang.tongTien;

                if (Array.isArray(donHang.chiTiets) && donHang.chiTiets.length) {
                    donHang.chiTiets.forEach(chiTiet => {
                        const anhMonAn = chiTiet.monAn?.anhMonAn || "default.jpg";
                        const danhMuc = chiTiet.monAn?.danhMuc || "Không xác định";
                        const ngayThanhToan = new Date(donHang.ngayTao)
                            .toLocaleDateString();
                        const tongGia = (chiTiet.gia * chiTiet.soLuong)
                            .toLocaleString();

                        htmlContent += `
                            <div class="row py-3 align-items-center">
                                <div class="col-12 col-md-4 text-center">
                                    <img 
                                        src="/uploads/monan/${anhMonAn}" 
                                        alt="${chiTiet.tenMonAn}" 
                                        class="img-fluid" 
                                        style="height:170px; width:170px;"
                                    >
                                </div>
                                <div class="col-12 col-md-8">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <h5>${chiTiet.tenMonAn}</h5>
                                            <p class="mb-1">
                                                <strong>Số lượng:</strong> ${chiTiet.soLuong}
                                            </p>
                                        </div>
                                        <div class="col-sm-6">
                                            <p class="mb-1">
                                                <strong>Ngày thanh toán:</strong> ${ngayThanhToan}
                                            </p>
                                            <p class="mb-1">
                                                <strong>Danh mục:</strong> ${danhMuc}
                                            </p>
                                        </div>
                                    </div>
                                    <div class="row mt-2">
                                        <div class="col-sm-6">
                                            <p class="mb-0">
                                                <strong>Giá:</strong> ${chiTiet.gia.toLocaleString()} VND
                                            </p>
                                        </div>
                                        <div class="col-sm-6">
                                            <p class="mb-0">
                                                <strong>Tổng:</strong> ${tongGia} VND
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <hr class="my-0">
                        `;
                    });
                }
            });

            $("#donHangContentModal").html(htmlContent);
            $("#tongThanhToanModal").text(`${tongTien.toLocaleString()} VND`);
        },
        error: function (xhr, status, error) {
            console.error("📌 Lỗi AJAX:", xhr.responseText || error);
            $("#donHangContentModal").html(
                '<p class="text-center text-danger">Lỗi khi tải đơn hàng!</p>'
            );
        }
    });
}



