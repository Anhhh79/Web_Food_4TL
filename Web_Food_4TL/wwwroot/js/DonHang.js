function modalDonHang() {
    $('#modaldonhang').modal('toggle');
}
function loadDonHang() {
    $('#modaldonhang').modal('toggle');
    $.ajax({
        url: "/api/donhang/danhsach",
        type: "GET",
        success: function (response) {
            if (response.success && response.data.length > 0) {
                let donHangs = response.data;
                let htmlContent = "";
                let tongTien = 0;

                donHangs.forEach(donHang => {
                    console.log("📝 Đơn hàng:", donHang);

                    if (!donHang.chiTiets || donHang.chiTiets.length === 0) {
                        console.warn("⚠ Không có chi tiết đơn hàng cho đơn hàng:", donHang.id);
                        return;
                    }

                    donHang.chiTiets.forEach(chiTiet => {
                        htmlContent += `
                            <div class="row">
                                <div class="col-12 col-md-4 text-center my-2">
                                    <img src="/uploads/monan/${chiTiet.monAn.anhMonAn}" alt="" class="img-fluid" style="height:170px; width:170px;">
                                </div>
                                <div class="col-12 col-md-8">
                                    <div class="row mt-4 pt-3">
                                        <span class="col-6 fz text-warning">${chiTiet.tenMonAn}</span>
                                        <div class="col-6">
                                            <span class="f fw-bold"> Số lượng: </span>
                                            <span class="f">${chiTiet.soLuong}</span>
                                        </div>
                                    </div>
                                    <div class="row mt-1">
                                        <div class="col-6">
                                            <span class="f fw-bold"> Ngày thanh toán: </span>
                                            <span class="f">${new Date(donHang.ngayTao).toLocaleDateString()}</span>
                                        </div>
                                        <div class="col-6">
                                            <span class="f fw-bold"> Danh mục: </span>
                                            <span class="f">${chiTiet.monAn.danhMuc}</span>
                                        </div>
                                    </div>
                                    <div class="row mt-2">
                                        <div class="col-6">
                                            <span class="f fw-bold"> Giá: </span>
                                            <span class="f text-warning"> ${chiTiet.gia.toLocaleString()} VND</span>
                                        </div>
                                        <div class="col-6">
                                            <span class="f fw-bold"> Tổng: </span>
                                            <span class="f text-warning"> ${(chiTiet.gia * chiTiet.soLuong).toLocaleString()} VND</span>
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
                alert(response.message || "Không có đơn hàng nào.");
            }
        },
        error: function (error) {
            console.error("Lỗi:", error);
            alert("Lỗi khi tải đơn hàng!");
        }
    });
}



