$.ajax({
    url: "/api/thongke/tongtien",
    type: "GET",
    dataType: "json",
    success: function (response) {
        if (response.success) {
            $("#TongDanhthu").text(response.tongTien.toLocaleString() + " VND");
        }
    },
    error: function () {
        console.error("Lỗi khi lấy tổng tiền!");
    }
});


$.ajax({
    url: "/api/thongke/monan",
    type: "GET",
    dataType: "json",
    success: function (response) {
        if (response.success) {
            $("#MonAn").text(response.soLuong);
        } else {
            console.warn("Không lấy được số lượng món ăn:", response);
        }
    },
    error: function (xhr, status, error) {
        console.error("📌 Lỗi AJAX:", xhr.status, xhr.responseText || error);
    }
});

$.ajax({
    url: "/api/thongke/nguoidung",
    type: "GET",
    dataType: "json",
    success: function (response) {
        if (response.success) {
            $("#NguoiDung").text(response.soLuong);
        } else {
            console.warn("Không lấy được số lượng món ăn:", response);
        }
    },
    error: function (xhr, status, error) {
        console.error("📌 Lỗi AJAX:", xhr.status, xhr.responseText || error);
    }
});


$.ajax({
    url: "/api/thongke/thongke-theo-thang",
    type: "GET",
    dataType: "json",
    success: function (response) {


        if (response.success) {
            var doanhThuData = response.data;


            var months = ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6',
                'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'];

            var doanhThu = new Array(12).fill(0);

            doanhThuData.forEach(function (item) {
                doanhThu[item.thang - 1] = item.tongDoanhThu;
            });


            var ctx = document.getElementById('combinedChart');


            if (window.combinedChart instanceof Chart) {
                window.combinedChart.destroy();
                window.combinedChart = null;
            }

            window.combinedChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: months,
                    datasets: [
                        {
                            label: 'Tổng doanh thu (VND)',
                            data: doanhThu,
                            backgroundColor: 'rgba(54, 162, 235, 0.8)',
                            borderColor: 'rgba(54, 162, 235, 1)',
                            borderWidth: 1
                        }
                    ]
                },
                options: {
                    responsive: true,
                    scales: {
                        x: { beginAtZero: true },
                        y: { beginAtZero: true }
                    },
                    plugins: {
                        legend: { position: 'bottom' }
                    }
                }
            });
        } else {
            console.warn("Không có dữ liệu thống kê.");
        }
    },
    error: function (xhr, status, error) {
        console.error("📌 Lỗi AJAX:", xhr.responseText || error);
    }
});






