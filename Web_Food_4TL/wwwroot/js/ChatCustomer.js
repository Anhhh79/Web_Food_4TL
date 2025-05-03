function sendMessage() {
    let message = $("#messageInput").val().trim();
    if (message === "") return;

    $("#messageInput").val("");
    $("#sendButton").prop("disabled", true);

    // Gửi tin nhắn sau khi kiểm tra session
    $.ajax({
        url: "/customer/api/chat/check-session",
        type: "GET",
        success: function () {
            $.ajax({
                url: "/customer/api/chat/send/admins",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ messageText: message }),
                success: function () {
                    // ✅ KHÔNG xử lý nhiều phản hồi → chỉ load lại tin 1 lần
                    loadOldMessages();
                },
                error: function (err) {
                    console.error("Lỗi khi gửi:", err);
                    alert("Không thể gửi tin nhắn. Vui lòng thử lại.");
                },
                complete: function () {
                    $("#sendButton").prop("disabled", false);
                }
            });
        },
        error: function () {
            alert("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.");
            $("#sendButton").prop("disabled", false);
        }
    });
}



function loadOldMessages() {
    $.ajax({
        url: "/customer/api/chat/messages",
        type: "GET",
        success: function (data) {
            const chatBody = $("#chatBody");
            chatBody.empty();

            if (!Array.isArray(data.messages) || data.messages.length === 0) {
                // 👉 Không có tin nhắn → hiển thị lời chào
                chatBody.append(`
                    <div class="chat-message">
                        <div class="chat-bubble">
                            Xin chào! WEB FOOD 4TL HÂN HẠNH PHỤC VỤ BẠN.
                        </div>
                    </div>
                `);
                return;
            }

            // ✅ Nếu có tin nhắn → xử lý như thường
            let lastTimestamp = null;

            data.messages.forEach((msg, index) => {
                const msgTime = new Date(msg.thoiGianGui);

                // Hiển thị thời gian nếu là tin đầu tiên hoặc cách nhau ít nhất 2 tiếng
                if (index === 0 || !lastTimestamp || Math.abs(msgTime - lastTimestamp) >= 2 * 60 * 60 * 1000) {
                    const timeLabel = msgTime.toLocaleString();
                    chatBody.append(`
            <div style="text-align: center; color: gray; font-size: 0.75rem; margin: 10px 0;">
                ${timeLabel}
            </div>
        `);
                    lastTimestamp = msgTime;
                }

                const align = msg.laTinNhanTuKhach ? "flex-end" : "flex-start";
                const bubbleClass = msg.laTinNhanTuKhach ? "bg-primary text-white" : "bg-light";

                chatBody.append(`
        <div class="chat-message" style="justify-content: ${align};">
            <div class="chat-bubble ${bubbleClass}">
                ${msg.noiDung}
            </div>
        </div>
                `);
            });

            chatBody.scrollTop(chatBody[0].scrollHeight);
        },
        error: function (err) {
            console.error("Không thể tải tin nhắn cũ:", err);
        }
    });
}



document.addEventListener("DOMContentLoaded", function () {
    const chatIcon = document.getElementById("chat-icon");
    const chatContainer = document.getElementById("chat-container");

    // Khi click icon chat
    chatIcon.addEventListener("click", function (event) {
        event.stopPropagation();
        chatContainer.classList.toggle("show");

        if (chatContainer.classList.contains("show")) {
            loadOldMessages(); // ← GỌI Ở ĐÂY
        }
    });

    // Ngăn click trong chat không bị đóng
    chatContainer.addEventListener("click", function (event) {
        event.stopPropagation();
    });

    // Click ngoài thì ẩn chat
    document.addEventListener("click", function () {
        chatContainer.classList.remove("show");
    });
});
