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
$(document).ready(function () {
    $(".view-detail").click(function () {
        var monAnId = $(this).data("id"); // Lấy ID món ăn

        $.ajax({
            url: "/Customer/Home/GetMonAnDetail",
            type: "GET",
            data: { id: monAnId },
            success: function (data) {
                $("#modalContent").html(data); // Load nội dung vào modal

                // Kiểm tra modal có tồn tại không
                var modalElement = document.getElementById("monAnDetailModal");
                if (modalElement) {
                    var modalInstance = new bootstrap.Modal(modalElement);
                    modalInstance.show(); // Hiển thị modal
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
                if ($("#cartCount").length > 0) {
                    $("#cartCount").text(response.cartCount);
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





function formatVND(amount) {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount);
}

// Cập nhật giá trong giỏ hàng
document.querySelectorAll(".cart-price").forEach(el => {
    let price = parseInt(el.innerText);
    el.innerText = formatVND(price);
});



