// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//event
function adjustCarouselItems() {
    const items = document.querySelectorAll('#serviceCarousel .carousel-item');
    // Xóa các thẻ clone đã thêm trước đó (để tránh dư thừa khi resize)
    items.forEach((el) => {
        let row = el.querySelector('.row');
        // Giữ lại phần tử đầu tiên, loại bỏ các phần tử clone thêm
        while (row.children.length > 1) {
            row.removeChild(row.lastElementChild);
        }
    });

    // Xác định số lượng clone cần thêm dựa trên kích thước màn hình
    let cloneCount;
    if (window.innerWidth >= 768) {      // Màn hình lớn + trung: hiển thị 3 card (1 ban đầu + 2 clone)
        cloneCount = 2;
    } else {                             // Màn hình nhỏ: hiển thị 1 card (không clone)
        cloneCount = 0;
    }

    // Thêm các card clone theo số lượng xác định
    items.forEach((el) => {
        let row = el.querySelector('.row');
        let next = el.nextElementSibling;
        for (let i = 0; i < cloneCount; i++) {
            if (!next) {
                next = items[0];
            }
            let cloneCol = next.querySelector('.col-md-4');
            let clonedNode = cloneCol.cloneNode(true);
            row.appendChild(clonedNode);
            next = next.nextElementSibling;
        }
    });
}

// Khởi tạo carousel khi tải trang
adjustCarouselItems();

// Cập nhật carousel khi kích thước cửa sổ thay đổi
window.addEventListener('resize', adjustCarouselItems);
// Hỗ trợ kéo chuột trên desktop và vuốt trên mobile
document.addEventListener('DOMContentLoaded', function () {
    const carousel = document.querySelector('#serviceCarousel');
    let isDragging = false;
    let startPos = 0;
    let currentDelta = 0;
    const swipeThreshold = 50; // Ngưỡng chuyển slide

    // Lấy instance của carousel
    let carouselInstance = bootstrap.Carousel.getInstance(carousel);
    if (!carouselInstance) {
        carouselInstance = new bootstrap.Carousel(carousel);
    }

    // Xử lý sự kiện chuột cho desktop
    carousel.addEventListener('mousedown', (e) => {
        isDragging = true;
        startPos = e.pageX;
    });

    carousel.addEventListener('mousemove', (e) => {
        if (!isDragging) return;
        currentDelta = e.pageX - startPos;
    });

    carousel.addEventListener('mouseup', () => {
        if (!isDragging) return;
        isDragging = false;
        if (currentDelta > swipeThreshold) {
            carouselInstance.prev();
        } else if (currentDelta < -swipeThreshold) {
            carouselInstance.next();
        }
        currentDelta = 0;
    });

    carousel.addEventListener('mouseleave', () => {
        if (isDragging) {
            isDragging = false;
            if (currentDelta > swipeThreshold) {
                carouselInstance.prev();
            } else if (currentDelta < -swipeThreshold) {
                carouselInstance.next();
            }
            currentDelta = 0;
        }
    });

    // Xử lý sự kiện cảm ứng cho mobile
    carousel.addEventListener('touchstart', (e) => {
        isDragging = true;
        startPos = e.touches[0].clientX;
    });

    carousel.addEventListener('touchmove', (e) => {
        if (!isDragging) return;
        currentDelta = e.touches[0].clientX - startPos;
    });

    carousel.addEventListener('touchend', () => {
        if (!isDragging) return;
        isDragging = false;
        if (currentDelta > swipeThreshold) {
            carouselInstance.prev();
        } else if (currentDelta < -swipeThreshold) {
            carouselInstance.next();
        }
        currentDelta = 0;
    });

    carousel.addEventListener('touchcancel', () => {
        isDragging = false;
        currentDelta = 0;
    });
});
//$(document).ready(function () {
//    $(".view-detail").click(function () {
//        var monAnId = $(this).data("id"); // Lấy ID món ăn

//        $.ajax({
//            url: "/Customer/Home/GetMonAnDetail",
//            type: "GET",
//            data: { id: monAnId },
//            success: function (data) {
//                $("#modalContent").html(data); // Load nội dung vào modal

//                // Gọi hàm đánh giá sau khi nội dung modal đã được gán
//                viewDetail11({ dataset: { id: monAnId } });

//                // Hiển thị modal
//                var modalElement = document.getElementById("monAnDetailModal");
//                if (modalElement) {
//                    var modalInstance = new bootstrap.Modal(modalElement);
//                    modalInstance.show();
//                } else {
//                    console.error("Không tìm thấy modal với ID monAnDetailModal");
//                }
//            },
//            error: function () {
//                alert("Có lỗi xảy ra khi tải dữ liệu.");
//            }
//        });
//    });
//});

$(document).ready(function () {
    $(".view-detail").click(function () {
        var monAnId = $(this).data("id"); // Lấy ID món ăn

        $.ajax({
            url: "/Customer/Home/GetMonAnDetail",
            type: "GET",
            data: { id: monAnId },
            success: function (data) {
                $("#modalContent").html(data); // Load nội dung vào modal

                // Gọi hàm đánh giá sau khi nội dung modal đã được gán
                viewDetail11(monAnId);

                // Hiển thị modal
                var modalElement = document.getElementById("monAnDetailModal");
                if (modalElement) {
                    var modalInstance = new bootstrap.Modal(modalElement);
                    modalInstance.show();
                } else {
                    console.error("Không tìm thấy modal với ID monAnDetailModal");
                }
            },
            error: function () {
                alert("Có lỗi xảy ra khi tải dữ liệu.");
            }
        });
    });
});

$(document).ready(function () {
    updateCartCount();
})

$(document).on("click", "#addToCart", function () {
    var monAnId = $(this).data("id");
    var quantity = parseInt($("#quantity").val()) || 1; // Chuyển về số, mặc định là 1 nếu lỗi

    console.log("Thêm vào giỏ hàng: ID =", monAnId, "Số lượng =", quantity);

    $.ajax({
        url: "/Customer/Cart/AddToCart",
        type: "POST",
        data: { monAnId: monAnId, quantity: quantity },
        success: function (response) {
            if (response.needLogin) {
                toastr.warning(response.message);
                return;
            }
            if (response.success) {
                // Cập nhật số lượng giỏ hàng nếu phần tử tồn tại
                console.log(response.cartCount);
                if ($(".cartCount").length > 0) {
                    updateCartCount();
                } else {
                    console.warn("Không tìm thấy #cartCount để cập nhật!");
                }

                // Đóng modal nếu tồn tại
                if ($("#monAnDetailModal").length > 0) {
                    $("#monAnDetailModal").modal("hide");
                } else {
                    console.error("Không tìm thấy modal!");
                }

                // Hiển thị thông báo thành công
                toastr.success(response.message);
            } else {
                // Hiển thị lỗi từ server
                toastr.error(response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi thêm vào giỏ hàng:", xhr.responseText);
            toastr.error("Có lỗi xảy ra khi thêm vào giỏ hàng!", "Lỗi", { timeOut: 3000 });
        }
    });
});

//hàm cập nhật số lượng trong giỏ hàng
function updateCartCount() {
    $.ajax({
        url: "/Customer/Cart/GetCartCount", // Route này trả về số lượng sản phẩm trong giỏ hàng
        type: "GET",
        success: function (response) {
            if (response.success) {
                $(".cartCount").text(response.cartCount); // Cập nhật số lượng giỏ hàng
            } else {
                console.warn("Không thể lấy số lượng giỏ hàng.");
            }
        },
        error: function (xhr) {
            console.error("Lỗi khi cập nhật số lượng giỏ hàng:", xhr.responseText);
        }
    });
}



function formatVND(amount) {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount);
}

// Cập nhật giá trong giỏ hàng
document.querySelectorAll(".cart-price").forEach(el => {
    let price = parseInt(el.innerText);
    el.innerText = formatVND(price);
});


function viewDetail11(monAnId) {
    console.log('viewDetail11 được gọi với id:', monAnId);
    resetDanhGia()

    $.ajax({
        url: '/Customer/DanhGia/GetAverageRatingAndDanhGias',
        type: 'GET',
        data: { monAnId: monAnId },
        success: function (data) {
            console.log(data);

            // Thống kê
            $('.averageRating1').text(data.averageRating);
            $('#totalReviews1').text(data.totalReviews);
            $('#totalReviews2').text(data.totalReviews);
            $('#fiveStarCount').text(data.fiveStar);
            $('#fourStarCount').text(data.fourStar);
            $('#threeStarCount').text(data.threeStar);
            $('#twoStarCount').text(data.twoStar);
            $('#oneStarCount').text(data.oneStar);

            var rating = data.averageRating; // ví dụ: 3.5

            $('.saoTrungBinh').each(function () {
                var value = $(this).data('value');
                $(this).removeClass('fas fa-star fas fa-star-half-alt far fa-star text-warning text-muted');

                if (value <= Math.floor(rating)) {
                    $(this).addClass('fas fa-star text-warning'); // sao đầy
                } else if (value - 1 < rating && rating < value) {
                    $(this).addClass('fas fa-star-half-alt text-warning'); // nửa sao
                } else {
                    $(this).addClass('far fa-star text-muted'); // sao rỗng
                }
            });



            // Render danh sách
            var html = '';

            if (data.danhGias.length === 0) {
                html = `
        <div class="text-center text-muted mt-3">
            <i class="bi bi-info-circle-fill"></i> Chưa có đánh giá nào cho món ăn này.
        </div>
    `;
            } else {
                data.danhGias.forEach(function (item) {
                    html += `
            <div class="mb-4">
                <p class="mb-2">
                    <b><span>${item.tenNguoiDung}</span> :</b>
                    <b><span class="text-warning">${item.soSao} sao</span></b>
                </p>
                <p class="text-break mb-2">
                    <span>${item.noiDung}</span>
                </p>
                <p class="text-muted small">
                    Thời gian: <span>${item.thoiGian}</span>
                </p>
            </div>
        `;

                    if (item.noiDungPhanHoi && item.noiDungPhanHoi !== "Chưa phản hồi") {
                        html += `
                <div class="mb-4">
                    <p class="mb-2">
                        <b><span class="text-danger">Food 4TL:</span> <span class="text-success">đã trả lời</span> </b>
                    </p>
                    <p class="text-break mb-2">
                        <span>${item.noiDungPhanHoi}</span>
                    </p>
                </div>
            `;
                    }

                    html += `<hr style="border: none; border-top: 1px dashed #ccc;" class="my-2">`;
                });
            }

            $('#bodyDanhGia').html(html);


            $('#bodyDanhGia').html(html);
            $('#monAnDetailModal').modal('show');
        },
        error: function () {
            alert('Có lỗi xảy ra!');
        }
    });
}

function resetDanhGia() {
    // Reset sao TB
    $('#averageRating1').text('0');

    // Reset tổng đánh giá
    $('#totalReviews1').text('0');
    $('#totalReviews2').text('0');

    // Reset đếm sao
    $('#fiveStarCount').text('0');
    $('#fourStarCount').text('0');
    $('#threeStarCount').text('0');
    $('#twoStarCount').text('0');
    $('#oneStarCount').text('0');

    // Reset màu sao
    $('.saoTrungBinh').removeClass('text-warning');

    // Xóa danh sách đánh giá cũ
    $('#bodyDanhGia').html('');
}
