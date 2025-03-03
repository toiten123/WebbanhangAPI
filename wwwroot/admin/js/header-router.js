document.addEventListener("DOMContentLoaded", function () {
    function updateSidebarText() {
        let trangchu = "Trang Chủ";
        let quanlyhanghoa = "Quản Lý Hàng Hóa";
        let quanlynguoidung = "Quản Lý Người Dùng";
        let quanlyluong = "Quản Lý Lương";
        let quanlythanhtoan = "Quản Lý Thanh Toán"; 
        let caidat = "Cài Đặt";

        // Cập nhật cho desktop sidebar
        document.getElementById("text-trangchu").innerText = trangchu;
        document.getElementById("text-quanlyhanghoa").innerText = quanlyhanghoa;
        document.getElementById("text-quanlynguoidung").innerText = quanlynguoidung;
        document.getElementById("text-quanlyluong").innerText = quanlyluong;
        document.getElementById("text-quanlythanhtoan").innerText = quanlythanhtoan;
        document.getElementById("text-caidat").innerText = caidat;

        // Cập nhật cho mobile sidebar
        let mobileElements = document.querySelectorAll("#text-trangchu, #text-quanlyhanghoa, #text-quanlynguoidung, #text-quanlyluong, #text-quanlythanhtoan, #text-caidat");
        mobileElements.forEach((el) => {
            if (el.id === "text-trangchu") el.innerText = trangchu;
            if (el.id === "text-quanlyhanghoa") el.innerText = quanlyhanghoa;
            if (el.id === "text-quanlynguoidung") el.innerText = quanlynguoidung;
            if (el.id === "text-quanlyluong") el.innerText = quanlyluong;
            if (el.id === "text-quanlythanhtoan") el.innerText = quanlythanhtoan;
            if (el.id === "text-caidat") el.innerText = caidat;
        });
    }

    updateSidebarText();

    // Lắng nghe sự kiện mở menu mobile để cập nhật lại text
    document.querySelector('[aria-label="Menu"]').addEventListener("click", function () {
        setTimeout(updateSidebarText, 50); // Cho Alpine.js xử lý xong DOM trước khi cập nhật
    });
});

/// thay đổi màu dark light
tailwind.config = {
    darkMode: 'class', // Sử dụng chế độ 'class'
    theme: {
        extend: {
            colors: {
                customDark: '#ffffff', // Thêm màu tùy chỉnh nếu cần
            },
        },
    },
};