$(document).ready(function () {
    //Sự kiện focus khi đăng nhập để xóa thông báo lỗi
    $("#taiKhoanDangNhap").focus(function () {
        $("#taiKhoanDangNhapError").text('');
    })
    $("#matKhauDangNhap").focus(function () {
        $("#matKhauDangNhapError").text('');
    })
    //Sự kiện focus khi đăng Ký để xóa thông báo lỗi
    $("#hoTenDangKy, #soDienThoaiDangKy, #emailDangKy, #matKhauDangKy, #matKhauDangKy2").focus(function () {
        $(this).next(".error-SignUp").text('');
    });

    //Sự kiện bấm đăng nhập
    $("#btnLogin").click(function () {
        checkDataLogin();
    })

    //Sự kiện bấm đăng ký
    $("#btnSignUp").click(function () {
        checkDataSignUp();
    })
});


// kiểm tra thông tin đăng nhập
function checkDataLogin() {
    let taiKhoan = $("#taiKhoanDangNhap").val().trim();
    let matKhau = $("#matKhauDangNhap").val().trim();

    //reset thong bao loi 
    resetErrorLogin();

    let isValid = true;
    if (taiKhoan === "") {
        $("#taiKhoanDangNhapError").text("Bạn chưa nhập tài khoản!");
        isValid = false;
    }
    if (matKhau === "") {
        $("#matKhauDangNhapError").text("Bạn chưa nhập mật khẩu!");
        isValid = false;
    }
    if (!isValid) {
        return false;
    }
    postdataLogin();
    return true;
    
}

//reset thông báo lỗi khi đăng nhập
function resetErrorLogin() {
    $(".error-Login").text('');
}

//Xóa thông tin đăng nhập 
function deleteDataLogin() {
    $("#taiKhoanDangNhap, #matKhauDangNhap").val('');
    resetErrorLogin();
}

// kiểm tra thông tin đăng ký
function checkDataSignUp() {
    let hoTen = $("#hoTenDangKy").val().trim();
    let soDienThoai = $("#soDienThoaiDangKy").val().trim();
    let email = $("#emailDangKy").val().trim();
    let matKhau = $("#matKhauDangKy").val().trim();
    let xacNhanMatKhau = $("#matKhauDangKy2").val().trim();

    // Reset thông báo lỗi
    resetErrorSignUp();

    let isValid = true;

    // Kiểm tra họ tên
    if (hoTen === "") {
        $("#hoTenDangKyError").text("Bạn chưa nhập họ tên!");
        isValid = false;
    }

    // Kiểm tra số điện thoại (chỉ cho phép 10 số)
    let phonePattern = /^[0-9]{10}$/;
    if (soDienThoai === "") {
        $("#soDienThoaiDangKyError").text("Bạn chưa nhập số điện thoại!");
        isValid = false;
    } else if (!phonePattern.test(soDienThoai)) {
        $("#soDienThoaiDangKyError").text("Số điện thoại không hợp lệ!");
        isValid = false;
    }

    // Kiểm tra email (phải đúng định dạng)
    let emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (email === "") {
        $("#emailDangKyError").text("Bạn chưa nhập email!");
        isValid = false;
    } else if (!emailPattern.test(email)) {
        $("#emailDangKyError").text("Email không hợp lệ!");
        isValid = false;
    }

    // Kiểm tra mật khẩu
    if (matKhau === "") {
        $("#matKhauDangKyError").text("Bạn chưa nhập mật khẩu!");
        isValid = false;
    } else if (matKhau.length < 6) {
        $("#matKhauDangKyError").text("Mật khẩu phải có ít nhất 6 ký tự!");
        isValid = false;
    }

    // Kiểm tra xác nhận mật khẩu
    if (xacNhanMatKhau === "") {
        $("#matKhauDangKy2Error").text("Bạn chưa nhập xác nhận mật khẩu!");
        isValid = false;
    } else if (xacNhanMatKhau !== matKhau) {
        $("#matKhauDangKy2Error").text("Mật khẩu xác nhận không khớp!");
        isValid = false;
    }
    // Nếu hợp lệ, xóa dữ liệu nhập
    if (!isValid) {
        return false;
    }
    postDataSignUp();
    return true;
    
}

// Hàm xóa lỗi thông tin đăng ký
function resetErrorSignUp() {
    $(".error-SignUp").text("");
}

// Hàm xóa thông tin đăng ký
function deleteDataSignUp() {
    $("#hoTenDangKy, #soDienThoaiDangKy, #emailDangKy, #matKhauDangKy, #matKhauDangKy2").val("");
    resetErrorSignUp();
}

// Hàm hiển thị hoặc đóng modal modal đăng ký
function modalSignUp() {
    $('#modalDangKy').modal('toggle');
    deleteDataSignUp()
}

// Hàm hiển thị hoặc đóng modal modal đăng nhập
function modalLogin() {
    $('#modalDangNhap').modal('toggle');
    deleteDataLogin();
}

//Hàm lấy dữ liệu đăng ký 
function postDataSignUp() {
    // Lấy dữ liệu từ modal
    let hoTen = $("#hoTenDangKy").val().trim();
    let email = $("#emailDangKy").val().trim();
    let soDienThoai = $("#soDienThoaiDangKy").val().trim();
    let matKhau = $("#matKhauDangKy").val().trim();
    let matKhau2 = $("#matKhauDangKy2").val().trim();
    let btnSignUp = $("#btnSignUp");

    // Kiểm tra dữ liệu nhập
    if (!hoTen || !email || !soDienThoai || !matKhau || !matKhau2) {
        toastr.error("Vui lòng nhập đầy đủ thông tin!", "", {
            timeOut: 2000 // Giới hạn thời gian hiển thị là 1 giây
        });
        return;
    }

    // Vô hiệu hóa nút đăng ký để tránh spam
    btnSignUp.prop("disabled", true).text("Đang đăng ký...");

    // Gửi dữ liệu bằng AJAX
    $.ajax({
        url: "/Customer/Account/SignUp", // Route API chính xác
        type: "POST",
        contentType: "application/json",
        headers: { "Accept": "application/json" },
        data: JSON.stringify({
            FullName: hoTen,
            Email: email,
            Phone: soDienThoai,
            PassWord: matKhau,
            ConfirmPassWord: matKhau2
        }),
        success: function (response) {
            if (response.success) {
                toastr.success(response.message, "", {
                    timeOut: 2000 // Giới hạn thời gian hiển thị là 1 giây
                });
                modalSignUp(); // Ẩn modal đăng ký
                modalLogin(); // Hiển thị modal đăng nhập
                deleteDataSignUp();
            } else {
                toastr.error(response.message, "", {
                    timeOut: 2000 // Giới hạn thời gian hiển thị là 1 giây
                });
            }
        },
        error: function (xhr) {
            let errorMessage = xhr.responseJSON?.message || `Lỗi ${xhr.status}: ${xhr.statusText}`;
            toastr.error(errorMessage, "", {
                timeOut: 2000 // Giới hạn thời gian hiển thị là 1 giây
            });
        },
        complete: function () {
            // Kích hoạt lại nút đăng ký sau khi xử lý xong
            btnSignUp.prop("disabled", false).text("Đăng ký");
        }
    });
}

//Hàm lấy dữ liệu đăng nhập
function postdataLogin() {
    let userNameOrEmail = $("#taiKhoanDangNhap").val().trim();
    let password = $("#matKhauDangNhap").val().trim();
    let btnLogin = $("#btnLogin");

    if (!userNameOrEmail || !password) {
        toastr.error("Vui lòng nhập đầy đủ thông tin!", "", {
            timeOut: 2000 // Giới hạn thời gian hiển thị là 1 giây
        });
        return;
    }

    // Vô hiệu hóa nút đăng nhập để tránh spam
    btnLogin.prop("disabled", true).text("Đang đăng nhập...");

    $.ajax({
        url: "/Customer/Account/Login",
        type: "POST",
        contentType: "application/json",
        headers: {
            "Authorization": "Bearer " + (localStorage.getItem("accessToken") || "")
        },
        data: JSON.stringify({
            UserNameOrEmail: userNameOrEmail,
            Password: password
        }),
        success: function (response) {
            if (response.success) {
                window.location.href = response.redirectUrl; // Chuyển trang
                deleteDataLogin();
            } else {
                toastr.error(response.message, "", {
                    timeOut: 2000 // Giới hạn thời gian hiển thị là 1 giây
                });// Hiển thị lỗi nếu có
            }
        },
        error: function (xhr) {
            let errorMessage;
            if (xhr.status === 0) {
                errorMessage = "Không thể kết nối đến máy chủ. Vui lòng kiểm tra lại mạng!";
            } else {
                errorMessage = xhr.responseJSON?.message || `Lỗi ${xhr.status}: ${xhr.statusText}`;
            }
            toastr.error(errorMessage, "", {
                timeOut: 2000 // Giới hạn thời gian hiển thị là 1 giây
            });
        },
        complete: function () {
            // Kích hoạt lại nút đăng nhập sau khi xử lý xong
            btnLogin.prop("disabled", false).text("Đăng nhập");
        }
    });
}

