function sendMessage() {
    let message = $("#messageInput").val().trim();
    if (message === "") return;

    // Hiển thị tin nhắn bên giao diện
    $("#chatBody").append(`
        <div class="chat-message" style="justify-content: flex-end;">
            <div class="chat-bubble bg-primary text-white">
                ${message}
            </div>
        </div>
    `);
    $("#messageInput").val("");

    // Kiểm tra session trước khi gửi tin nhắn
    $.ajax({
        url: "/customer/api/chat/check-session",
        type: "GET",
        success: function (response) {
            // Nếu session còn, gửi tin nhắn đi
            $.ajax({
                url: "/customer/api/chat/send/admins", // Gửi cho tất cả quản lý
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ messageText: message }),
                success: function (response) {
                    console.log("Gửi tin nhắn thành công đến tất cả admin!", response);
                },
                error: function (error) {
                    console.error("Lỗi khi gửi tin nhắn:", error);
                    alert("Không thể gửi tin nhắn. Vui lòng thử lại.");
                }
            });
        },
        error: function (error) {
            console.error("Lỗi kiểm tra session:", error);
            alert("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.");
        }
    });
}
