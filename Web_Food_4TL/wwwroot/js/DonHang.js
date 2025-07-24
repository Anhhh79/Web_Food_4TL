
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

                    // Chỉ hiển thị nút khi đơn đã duyệt và đã giao
                    let showButtons = (donHang.trangThaiDonHang === "Đã duyệt" && donHang.trangThaiGiaoHang === "Đã giao");

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
                                            <span class="f fw-bold">Số lượng:</span>
                                            <span class="f">${chiTiet.soLuong}</span>
                                        </div>
                                    </div>

                                    <div class="row mt-1">
                                        <div class="col-12 col-sm-6 text-center text-sm-start">
                                            <span class="f fw-bold">Ngày thanh toán:</span>
                                            <span class="f">${new Date(donHang.ngayTao).toLocaleDateString()}</span>
                                        </div>
                                        <div class="col-12 col-sm-6 text-center text-sm-start">
                                            <span class="f fw-bold">Danh mục:</span>
                                            <span class="f me-5">${chiTiet.monAn.danhMuc}</span>
                                            <span class="f fw-bold">Giá:</span>
                                            <span class="f text-warning">${chiTiet.gia.toLocaleString()} VND</span>
                                        </div>
                                    </div>

                                    <div class="row mt-2">
                                        <div class="col-12 col-sm-6 text-center text-sm-start">
                                            <span class="f fw-bold">Tổng:</span>
                                            <span class="f text-warning">${(chiTiet.gia * chiTiet.soLuong).toLocaleString()} VND</span>
                                        </div>
                                        <div class="col-12 col-sm-6 text-center text-sm-start d-flex flex-wrap justify-content-center gap-2">
                                            ${showButtons ? `<a class="btn btn-success" onclick="xacNhanNhanHang(${donHang.id})">Đã nhận</a>` : ""}
                                            ${showButtons ? `<a class="btn btn-outline-danger" onclick="moModalLyDo(${donHang.id})">Hoàn Tiền</a>` : ""}
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


// === KHAI BÁO BIẾN TOÀN CỤC ===

let currentHoaDonId = null;

let currentMonAnIdForReview = null;

let modalDanhSachDonHang;

let modalChiTietDonHang;

let modalLyDo;

let modalDanhGia;



// Khởi tạo modals sau khi DOM ready

document.addEventListener('DOMContentLoaded', () => {

    const dsEl = document.getElementById('modaldsdonhang');

    if (dsEl) modalDanhSachDonHang = new bootstrap.Modal(dsEl);

    const ctEl = document.getElementById('modaldonhang');

    if (ctEl) modalChiTietDonHang = new bootstrap.Modal(ctEl);

    const lyEl = document.getElementById('modalLyDo');

    if (lyEl) modalLyDo = new bootstrap.Modal(lyEl);

    const dgEl = document.getElementById('modalDanhGia');

    if (dgEl) modalDanhGia = new bootstrap.Modal(dgEl);

});



// === LUỒNG XEM ĐƠN HÀNG ===

function loadDonHang() {

    fetch('/Customer/HoaDon/List')

        .then(res => res.ok ? res.json() : Promise.reject('Lỗi mạng hoặc server không phản hồi.'))

        .then(data => {

            renderDonHang(data);

            if (modalDanhSachDonHang) modalDanhSachDonHang.show();

        })

        .catch(error => {

            console.error('Lỗi khi tải danh sách đơn hàng:', error);

            alert('Không thể tải danh sách đơn hàng. Vui lòng thử lại.');

        });

}



function renderDonHang(data) {

    const container = document.getElementById('dsDonHang');

    if (!container) return;

    container.innerHTML = data.length ? '' : '<p class="text-center">Chưa có đơn hàng nào.</p>';

    data.forEach(hd => {

        const ngay = new Date(hd.ngayTao);

        container.innerHTML += `

              <div class="border-bottom py-2 d-flex justify-content-between align-items-center">

                <div>

                    <div><strong>Ngày:</strong> ${isNaN(ngay) ? '—' : ngay.toLocaleString('vi-VN')}</div>

                    <div><strong>Tổng tiền:</strong> ${hd.tongTien.toLocaleString('vi-VN')} VND</div>

                    <div><strong>Trạng thái:</strong> ${hd.trangThaiDonHang || 'Đang xử lý'}</div>

                </div>

                <button class="btn btn-sm btn-primary" onclick="xemChiTiet(${hd.id})">Xem chi tiết</button>

            </div>`;

    });

}



function xemChiTiet(id) {
    currentHoaDonId = id;

    fetch(`/Customer/HoaDon/ChiTiet/${id}`)
        .then(res => res.ok ? res.json() : Promise.reject('Không thể tải chi tiết đơn hàng.'))
        .then(hd => {
            const content = document.getElementById('ctHoaDonContent');
            const tongEl = document.getElementById('tongTT');
            const actions = document.getElementById('btnActions');

            if (!content || !tongEl || !actions) return console.error('Lỗi cấu trúc HTML của modalChiTiet.');

            let tongTien = 0;
            // Thêm thông tin số điện thoại và địa chỉ giao hàng
            let infoHeader = `<div class=\"mb-2\"><strong>Số điện thoại:</strong> ${hd.soDienThoai || ''}</div>`;
            infoHeader += `<div class=\"mb-2\"><strong>Địa chỉ giao hàng:</strong> ${hd.diaChiGiaoHang || ''}</div>`;

            const detailsHtml = (hd.hoaDonChiTiets || []).map(item => {
                tongTien += item.gia * item.soLuong;
                // Thêm hình ảnh món ăn nếu có
                let imgHtml = item.anhMonAn ? `<img src=\"/uploads/monan/${item.anhMonAn}\" alt=\"${item.tenMonAn}\" class=\"img-thumbnail me-2\" style=\"height:80px;width:80px;object-fit:cover;\">` : '';
                return `
                    <div class=\"border-bottom py-2 d-flex align-items-center\">
                        <div class=\"me-3\">${imgHtml}</div>
                        <div>
                            <div><strong>${item.tenMonAn}</strong></div>
                            <div>Số lượng: ${item.soLuong}</div>
                            <div>Giá: ${item.gia.toLocaleString('vi-VN')} VND</div>
                            ${!item.danhGiaDaTonTai
                        ? (hd.trangThaiDonHang === "Hoàn thành" ? `<button class=\"btn btn-sm btn-outline-warning mt-2\" onclick=\"openModalDanhGia(${item.monAnId}, '${item.tenMonAn}')\">Đánh giá</button>` : "")
                        : `<div class=\"text-success mt-2\"><i class=\"bi bi-check-circle-fill\"></i> Đã đánh giá</div>`
                    }
                        </div>
                    </div>`;
            }).join('');

            let infoHtml = `<div class=\"mt-2\"><strong>Trạng thái giao hàng:</strong> ${hd.trangThaiGiaoHang || 'Chưa có'}</div>`;
            if (hd.lydo) infoHtml += `<div class=\"text-danger mt-1\"><strong>Lý do hoàn tiền:</strong> ${hd.lydo}</div>`;
            if (hd.lyDoTuChoi) infoHtml += `<div class=\"text-danger mt-1\"><strong>Lý do từ chối:</strong> ${hd.lyDoTuChoi}</div>`;

            content.innerHTML = infoHeader + detailsHtml + infoHtml;
            tongEl.innerText = `${tongTien.toLocaleString('vi-VN')} VND`;

            const trangThaiDH = (hd.trangThaiDonHang || '').toLowerCase();
            const trangThaiGH = (hd.trangThaiGiaoHang || '').toLowerCase();

            // Chỉ hiển thị nút khi đúng trạng thái đơn và giao hàng
            if (trangThaiDH === 'đã duyệt' && trangThaiGH === 'đã giao') {
                actions.innerHTML = `
                    <button class=\"btn btn-success me-2\" onclick=\"xacNhanNhanHang()\">Đã nhận hàng</button>
                    <button class=\"btn btn-warning\" onclick=\"moModalLyDo(${currentHoaDonId})\">Yêu cầu hoàn tiền</button>
                `;
            } else {
                actions.innerHTML = '';
            }

            if (modalDanhSachDonHang) modalDanhSachDonHang.hide();
            if (modalChiTietDonHang) modalChiTietDonHang.show();
        })
        .catch(error => alert(error));
}



// === HÀNH ĐỘNG & LUỒNG HOÀN TIỀN ===

function xacNhanNhanHang() {

    Swal.fire({
        title: 'Xác nhận đã nhận hàng?',
        text: 'Bạn chắc chắn đã nhận được hàng? Hành động này không thể hoàn tác!',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Đã nhận hàng',
        cancelButtonText: 'Hủy',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/Customer/HoaDon/CapNhatTrangThai/CapNhatTrangThai/${currentHoaDonId}`, {
                method: 'POST', headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ trangThai: 'Hoàn thành' })
            })
                .then(res => res.ok ? res.json() : Promise.reject('Cập nhật thất bại!'))
                .then(() => {
                    Swal.fire({
                        icon: 'success',
                        title: 'Thành công!',
                        text: 'Cảm ơn bạn đã xác nhận đã nhận hàng.',
                        timer: 1800,
                        showConfirmButton: false
                    });
                    xemChiTiet(currentHoaDonId);
                })
                .catch(err => {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: err || 'Có lỗi xảy ra khi xác nhận.',
                        confirmButtonText: 'Đóng'
                    });
                });
        }
    });
}



function moModalLyDo(id) {

    if (id) {
        currentHoaDonId = id;
    }
    if (modalChiTietDonHang) modalChiTietDonHang.hide();
    if (modalLyDo) modalLyDo.show();
}

function backToChiTiet() {

    if (modalLyDo) modalLyDo.hide();

    if (modalChiTietDonHang) modalChiTietDonHang.show();

}



function guiLyDoHoanTien() {
    const lyDo = document.getElementById('lyDoText').value.trim();
    if (!lyDo) return alert('Vui lòng nhập lý do');
    if (!currentHoaDonId) {
        alert('Không tìm thấy thông tin đơn hàng!');
        return;
    }
    console.log(currentHoaDonId)
    // Đóng modal Chi Tiết trước khi gửi
    if (modalChiTietDonHang) modalChiTietDonHang.hide();

    fetch(`/Customer/HoaDon/GuiYeuCauHoanTien/GuiYeuCauHoanTien/${currentHoaDonId}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' },
        body: JSON.stringify({ Lydo: lyDo })
    })
        .then(res => {
            if (!res.ok) throw new Error('Gửi yêu cầu thất bại');
            return res.json();
        })
        .then(() => {
            alert('Gửi yêu cầu hoàn tiền thành công!');
            if (modalLyDo) modalLyDo.hide();
            xemChiTiet(currentHoaDonId);
            return;
        })
        .catch(err => {
            console.error('Lỗi hoàn tiền:', err);
            alert('Không thể gửi yêu cầu hoàn tiền. Vui lòng thử lại.');
            // hiển thị lại modaldonhang nếu cần
            if (modalChiTietDonHang) modalChiTietDonHang.show();
        });
}





// === LUỒNG ĐÁNH GIÁ SẢN PHẨM ===

function clearDataAssessInput() {

    document.querySelectorAll('.Assess').forEach(i => i.value = '');

    document.querySelectorAll('.star').forEach(r => r.checked = false);

    const phoneNumberError = document.getElementById('phoneNumberError');

    const starRatingError = document.getElementById('starRatingError');

    if (phoneNumberError) phoneNumberError.textContent = '';

    if (starRatingError) starRatingError.textContent = '';

}



function validateAssessmentForm() {
    const phoneNumberInput = document.getElementById('PhoneNumberAssess');
    const contentInput = document.getElementById('ContentAssess');
    const ratingInput = document.querySelector('input[name="rating"]:checked');

    const phoneNumberError = document.getElementById('phoneNumberError');
    const starRatingError = document.getElementById('starRatingError');

    resetError();
    let isValid = true;

    if (!phoneNumberInput || phoneNumberInput.value.trim() === '') {
        phoneNumberError.textContent = "Vui lòng nhập số điện thoại.";
        isValid = false;
    } else {
        const phone = phoneNumberInput.value.trim();
        const phoneRegex = /^[0-9]{10}$/;
        if (!phoneRegex.test(phone)) {
            phoneNumberError.textContent = "Số điện thoại phải đúng 10 chữ số.";
            isValid = false;
        }
    }

    if (!ratingInput) {
        starRatingError.textContent = "Vui lòng chọn số sao để đánh giá.";
        isValid = false;
    }

    return isValid;
}

function resetError() {
    document.getElementById('phoneNumberError').textContent = '';
    document.getElementById('starRatingError').textContent = '';
}

function clearDataAssessInput() {
    document.getElementById("PhoneNumberAssess").value = "";
    document.getElementById("ContentAssess").value = "";
}

function clearStarAssess() {
    const starRadios = document.querySelectorAll('input[name="rating"]');
    starRadios.forEach(radio => radio.checked = false);
}
function openModalDanhGia(monAnId, tenMonAn) {
    if (modalChiTietDonHang) modalChiTietDonHang.hide();
    // Gán giá trị vào modal
    document.getElementById('ModalMonAnId').value = monAnId;
    document.getElementById('TenMonAn').textContent = tenMonAn;

    // Reset các input cũ nếu cần
    clearDataAssessInput();
    clearStarAssess();
    resetError();

    // Hiển thị modal (Bootstrap 5)
    const modal = new bootstrap.Modal(document.getElementById('modalDanhGia'));
    modal.show();
}

function submitAssessmentForm() {
    const monAnId = document.getElementById('ModalMonAnId').value.trim();
    const phoneNumber = document.getElementById('PhoneNumberAssess').value.trim();
    const content = document.getElementById('ContentAssess').value.trim();
    const ratingElement = document.querySelector('input[name="rating"]:checked');
    const rating = ratingElement ? parseInt(ratingElement.value) : null;

    // Kiểm tra đầu vào
    let isValid = true;

    if (!phoneNumber) {
        document.getElementById('phoneNumberError').innerText = "Vui lòng nhập số điện thoại.";
        isValid = false;
    } else {
        document.getElementById('phoneNumberError').innerText = "";
    }

    if (!content) {
        alert("Vui lòng nhập nội dung đánh giá.");
        isValid = false;
    }

    if (!rating) {
        document.getElementById('starRatingError').innerText = "Vui lòng chọn số sao đánh giá.";
        isValid = false;
    } else {
        document.getElementById('starRatingError').innerText = "";
    }

    if (!isValid) return;

    // Chuyển sang FormData
    const formData = new FormData();
    formData.append("MonAnId", monAnId);
    formData.append("PhoneNumber", phoneNumber);
    formData.append("Content", content);
    formData.append("Rating", rating);

    fetch('/api/donhang/DanhGia', {
        method: 'POST',
        body: formData
    })
        .then(res => {
            if (!res.ok) {
                return res.json().then(errorData => {
                    throw errorData;
                });
            }
            return res.json();
        })
        .then(response => {
            alert("Cảm ơn bạn đã đánh giá!");
            const modal = bootstrap.Modal.getInstance(document.getElementById('modalDanhGia'));
            if (modal) modal.hide();
            clearDataAssessInput();
            clearStarAssess();
        })
        .catch(error => {
            console.error("Lỗi đánh giá:", error);
            alert("Gửi đánh giá thất bại. Vui lòng kiểm tra lại thông tin.");
        });
}
