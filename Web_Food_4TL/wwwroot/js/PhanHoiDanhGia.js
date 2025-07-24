$(document).ready(function () {
    $.ajax({
        url: "/Admin/PhanHoiDanhGia/LayDanhSachDanhGia", // ← bạn chỉnh URL đúng theo controller API của bạn
        type: "GET",
        dataType: "json",
        success: function (data) {
            let tbody = $("#bodyThongTinDanhGia");
            tbody.empty();

            $.each(data, function (index, item) {
                let row = `
                    <tr>
                        <th scope="row">${index + 1}</th>
                        <td>${item.tenNguoiDung}</td>
                        <td>${item.soDienThoai}</td>
                        <td>${item.noiDungDanhGia}</td>
                        <td>${item.soSao}</td>
                        <td class="text-center">${item.tenMonAn}</td>
                        <td>
                           <button class="btn btn-sm btn-outline-primary btnPhanHoi" data-id="${item.id}" data-noidung="${item.noiDungPhanHoi}">
                                Phản hồi
                            </button>
                        </td>
                    </tr>
                `;
                tbody.append(row);
            });
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi tải đánh giá: ", error);
        }
    });
    // Khi bấm nút phản hồi
    $("#bodyThongTinDanhGia").on("click", ".btnPhanHoi", function () {
        const danhGiaId = $(this).data("id");
        const noiDungHienTai = $(this).data("noidung") || "";

        console.log("==> Bấm nút phản hồi");
        console.log("ID đánh giá:", danhGiaId);
        console.log("Nội dung hiện tại:", noiDungHienTai);

        $("#danhGiaId").val(danhGiaId);
        $("#noiDungPhanHoi").val(noiDungHienTai);

        // Nếu dùng Bootstrap 5:
        const modal = new bootstrap.Modal(document.getElementById('phanHoiModal'));
        modal.show();

        // Nếu dùng Bootstrap 4 thì thay bằng:
        // $("#phanHoiModal").modal("show");
    });

});

$("#btnGuiPhanHoi").on("click", function () {
    const id = $("#danhGiaId").val();
    const noiDungPhanHoi = $("#noiDungPhanHoi").val();

    $.ajax({
        url: "/Admin/PhanHoiDanhGia/GuiPhanHoi",
        type: "POST",
        data: {
            id: id,
            noiDungPhanHoi: noiDungPhanHoi
        },
        success: function (response) {
            alert("Phản hồi đã được lưu!");
            $("#phanHoiModal").modal("hide");
            location.reload(); // hoặc gọi lại AJAX danh sách
        },
        error: function () {
            alert("Có lỗi xảy ra!");
        }
    });
});


