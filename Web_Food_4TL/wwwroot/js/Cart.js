function tangSoLuong(id) {
    let $input = $("#soLuong-" + id);
    let soLuong = parseInt($input.val(), 10);
    soLuong++;
    $input.val(soLuong);

    capNhatTongTien();

    // Kích hoạt lại nút giảm nếu số lượng > 1
    $("#btnGiam-" + id).prop("disabled", soLuong <= 1);
}

function giamSoLuong(id) {
    let $input = $("#soLuong-" + id);
    let soLuong = parseInt($input.val(), 10);

    if (soLuong > 1) {
        soLuong--;
        $input.val(soLuong);
    }

    capNhatTongTien();

    // Vô hiệu hóa nút giảm nếu số lượng <= 1
    $("#btnGiam-" + id).prop("disabled", soLuong <= 1);
}

// Hàm xóa món ăn khỏi danh sách
function xoaMonAn(itemId) {
    if (!confirm("Bạn có chắc chắn muốn xóa món ăn này khỏi giỏ hàng?")) {
        return;
    }

    $.ajax({
        url: "/Customer/Cart/XoaMonAn/" + itemId,
        type: "POST",
        success: function (response) {
            console.log(response);
            console.log(itemId);
            if (response.success) {
                $("#item-" + itemId).remove();

                // Cập nhật tổng tiền
                capNhatTongTien();
            } else {
                alert(response.message);
            }
        },
        error: function () {
            alert("Có lỗi xảy ra, vui lòng thử lại!");
        }
    });
}


// Cập nhật tổng tiền
function capNhatTongTien() {
    let tongTien = 0;
    $(".form-control[id^='soLuong-']").each(function () {
        let id = $(this).attr("id").split("-")[1]; // Lấy ID từ input
        let soLuong = parseInt($(this).val(), 10);
        let gia = parseInt($("#item-" + id).find(".text-danger").text().replace(/[^\d]/g, ""), 10); // Lấy giá từ HTML
        tongTien += gia * soLuong;
    });

    // Hiển thị tổng tiền
    $(".d-flex.justify-content-between p:last").text(tongTien.toLocaleString("vi-VN") + " VND");
}
