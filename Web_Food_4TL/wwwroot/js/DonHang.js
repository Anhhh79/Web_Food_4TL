
function loadDonHang() {
    $('#modaldonhang').modal('toggle');
    $.ajax({
        url: "/api/donhang/danhsach",
        type: "GET",
        success: function (response) {
            console.log(response)
            if (response.success && response.data.length > 0) {
                let donHangs = response.data;
                let htmlContent = "";
                let tongTien = 0;

                donHangs.forEach(donHang => {
                    if (!donHang.chiTiets || donHang.chiTiets.length === 0) {
                        console.warn("⚠ Không có chi tiết đơn hàng cho đơn hàng:", donHang.id);
                        return;
                    }

                    donHang.chiTiets.forEach(chiTiet => {
                        htmlContent += `
                                                            <div class="row gx-3 gy-2 align-items-center py-2">
                                    <div class="col-12 col-md-4 text-center my-2">
                                        <img src="/uploads/monan/${chiTiet.monAn.anhMonAn}" alt=""
                                             class="img-fluid" style="height:170px; width:170px; object-fit:cover;">
                                    </div>

                                    <div class="col-12 col-md-8">
                                        <div class="row mt-4 pt-3">
                                        <input type="hidden" name="MonAnId" value="${chiTiet.monAn.id}" />
                                            <span class="col-12 col-sm-6 fz text-warning text-center text-sm-start">${chiTiet.tenMonAn}</span>
                                            <div class="col-12 col-sm-6 text-center text-sm-start">
                                                <span class="f fw-bold"> Số lượng: </span>
                                                <span class="f">${chiTiet.soLuong}</span>
                                            </div>
                                        </div>

                                        <div class="row mt-1">
                                            <div class="col-12 col-sm-6 text-center text-sm-start">
                                                <span class="f fw-bold"> Ngày thanh toán: </span>
                                                <span class="f">${new Date(donHang.ngayTao).toLocaleDateString()}</span>
                                            </div>
                                            <div class="col-12 col-sm-6 text-center text-sm-start">
                                                <span class="f fw-bold"> Danh mục: </span>
                                                <span class="f me-5">${chiTiet.monAn.danhMuc}</span>
                                                 <span class="f fw-bold"> Giá: </span>
                                                <span class="f text-warning"> ${chiTiet.gia.toLocaleString()} VND</span>
                                            </div>
                                        </div>

                                        <div class="row mt-2">
                                            <div class="col-12 col-sm-6 text-center text-sm-start">
                                                 <span class="f fw-bold"> Tổng: </span>
                                                <span class="f text-warning"> ${(chiTiet.gia * chiTiet.soLuong).toLocaleString()} VND</span>
                                            </div>
                                            <div class="col-12 col-sm-6 text-center text-sm-start">
                                                <a class="btn bg-warning text-light" onclick="openModalDanhGia(this)" data-ten="${chiTiet.tenMonAn}" data-ma="${chiTiet.monAn.id}">Gửi Đánh Giá Của Bạn</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr class="m-0">

                        `;
                    });

                    tongTien += donHang.tongTien;
                });

                $("#dhContent").html(htmlContent);
                $("#tongTT").text(`${tongTien.toLocaleString()} VND`);

                $("#modaldonhang").modal("show");
            } else {
                console.log("Không có đơn hàng nào")
            }
        },
        error: function (error) {
            console.error("Lỗi:", error);
            alert("Lỗi khi tải đơn hàng!");
        }
    });
}


function clearDataAssessInput() {
    const inputFields = document.getElementsByClassName('Assess');
    Array.from(inputFields).forEach((input) => {
        input.value = '';
    });
}
// <!-- xoa so sao -->
function clearStarAssess() {
    const radioOptions = document.getElementsByClassName('star');
    Array.from(radioOptions).forEach((radio) => {
        radio.checked = false;
    });
}

//ham xoa so dien thoại danh gia
function clearPhoneNumberDanhGia() {
    const NumberInput = document.getElementById('PhoneNumberAssess');
    NumberInput.value = '';
}

//Reset thong bao lỗi
function resetError() {
    const phoneNumberError = document.getElementById('phoneNumberError'); // Phần tử thông báo lỗi cho số điện thoại
    const starRatingError = document.getElementById('starRatingError'); // Phần tử thông báo lỗi cho đánh giá sao
    
    phoneNumberError.textContent = '';
    starRatingError.textContent = '';
}
//Kiem tra nhap thong tin trong chuc nang danh gia
function validateAssessmentForm() {

    // Lấy giá trị của số điện thoại
    const phoneNumber = document.getElementById('PhoneNumberAssess').value.trim();
    const phoneNumberError = document.getElementById('phoneNumberError'); // Phần tử thông báo lỗi cho số điện thoại

    // Kiểm tra xem có radio nào được chọn cho phần đánh giá sao hay không
    const starRating = document.querySelector('input[name="rating"]:checked');
    const starRatingError = document.getElementById('starRatingError'); // Phần tử thông báo lỗi cho đánh giá sao

    // Biểu thức chính quy để kiểm tra số điện thoại (10 chữ số)
    const phoneRegex = /^[0-9]{10}$/;

    // Reset các thông báo lỗi trước khi kiểm tra
    resetError();

    // Biến cờ để theo dõi trạng thái hợp lệ của form
    let isValid = true;

    // Kiểm tra xem số điện thoại đã được nhập và hợp lệ chưa
    if (phoneNumber === "" || !phoneRegex.test(phoneNumber)) {
        phoneNumberError.textContent = "Vui lòng nhập số điện thoại hợp lệ (10 chữ số).";
        isValid = false;
    }

    // Kiểm tra xem người dùng có chọn số sao hay chưa
    if (!starRating) {
        starRatingError.textContent = "Vui lòng chọn số sao để đánh giá khách sạn.";
        isValid = false;
    }

    // Nếu có lỗi, không tiếp tục thực hiện
    if (!isValid) {
        return false;
    }

    // ThemDanhGia();
    return true;
}

function showSweetAlertDG() {
    Swal.fire({
        title: 'Thành công!',
        text: 'Cảm ơn bạn đã đánh giá!.',
        icon: 'success', // success, error, warning, info, question
        confirmButtonText: 'OK',
        customClass: {
            popup: 'custom-swal'
        }
    });
}

function submitAssessmentForm() {
    if (!validateAssessmentForm()) {
        return; // Nếu validate fail thì không gửi
    }

    var phoneNumber = $("#PhoneNumberAssess").val().trim();
    var content = $("#ContentAssess").val().trim();
    var rating = $('input[name="rating"]:checked').val();
    var maMonAn = $("#ModalMonAnId").val();  // Lấy đúng từ thẻ hidden

    $.ajax({
        url: '/api/donhang/DanhGia',
        type: 'POST',
        data: {
            PhoneNumber: phoneNumber,
            Content: content,
            Rating: rating,
            MonAnId: maMonAn
        },
        success: function (response) {
            if (response.success) {
                modalDanhGia.hide(); // Ẩn modal
                clearDataAssessInput(); // Xóa input
                clearStarAssess();      // Xóa sao
                resetError();           // Xóa lỗi
                //loadDanhGia(maChuSan);
                //tinhTrungBinhSao();
                //demDanhGia();
                //DemDanhGia5();
                showSweetAlertDG();     // Thông báo thành công
            } else {
                // Thay alert bằng SweetAlert2 đẹp hơn
                Swal.fire({
                    icon: 'error',
                    title: 'Không thể đánh giá',
                    text: response.message
                });
            }
        },
        error: function (xhr, status, error) {
            console.error('Status:', status);
            console.error('Error:', error);
            console.error('Response:', xhr.responseText);
            Swal.fire({
                icon: 'error',
                title: 'Lỗi hệ thống',
                text: 'Có lỗi xảy ra khi gửi đánh giá. Vui lòng thử lại.'
            });
        }
    });
}

// Thêm sự kiện focus để ẩn thông báo lỗi khi nhấp vào ô nhập
document.getElementById('PhoneNumberAssess').addEventListener('focus', function () {
    document.getElementById('phoneNumberError').textContent = '';
});

const starInputs = document.querySelectorAll('input[name="rating"]');
starInputs.forEach(function (input) {
    input.addEventListener('change', function () {
        document.getElementById('starRatingError').textContent = '';
    });
});

function showSweetAlertDG() {
    Swal.fire({
        title: 'Thành công!',
        text: 'Cảm ơn bạn đã đánh giá!.',
        icon: 'success', // success, error, warning, info, question
        confirmButtonText: 'OK',
        customClass: {
            popup: 'custom-swal'
        }
    });
}

// Hàm khởi tạo và mở modal
var modalDanhGia;
var modalDonHang = new bootstrap.Modal(document.getElementById('modaldonhang'));

function openModalDanhGia(btn) {
    // Lấy ID món ăn từ input hidden gần nút được bấm
    var monAnId = $(btn).data('ma');

    // Lấy tên món ăn từ thuộc tính data-ten
    var tenMonAn = $(btn).data('ten');

    // Gán ID và tên vào modal đánh giá
    $('#ModalMonAnId').val(monAnId);
    $('#TenMonAn').text(tenMonAn);

    // Nếu modalDanhGia chưa khởi tạo thì khởi tạo
    if (!modalDanhGia) {
        modalDanhGia = new bootstrap.Modal(document.getElementById('modalDanhGia'));
    }

    // Đóng modal đơn hàng nếu nó đang mở
    modalDonHang.hide();

    // Hiển thị modal đánh giá
    modalDanhGia.show();
}


