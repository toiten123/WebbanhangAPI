document.addEventListener("DOMContentLoaded", function () {
    const danhMucButton = document.querySelector(".product-catalog");
    const overlay = document.querySelector(".overlay");
    const gridRow = document.querySelector(".grid__row.row"); // Lấy phần tử gốc
    const headerContentWrapper = document.querySelector(".header-content.grid");

    danhMucButton.addEventListener("click", function () {
        // Kiểm tra trạng thái của lớp phủ
        if (overlay.classList.contains("active")) {
            // Nếu overlay đang hiển thị, ẩn overlay và các bản sao
            overlay.classList.remove("active");
            const allClonedRows = document.querySelectorAll(".grid__row.row.clone");
            allClonedRows.forEach(row => row.remove()); // Xóa các bản sao khỏi DOM
        } else {
            // Nếu overlay chưa hiển thị, hiển thị overlay và tạo bản sao
            overlay.classList.add("active");

            // Nhân bản grid__row.row
            const clonedGridRow = gridRow.cloneNode(true); // Nhân bản toàn bộ grid__row.row

            // Thêm lớp active vào bản sao để hiển thị
            clonedGridRow.classList.add("active", "clone");

            // Thêm bản sao vào wrapper
            headerContentWrapper.appendChild(clonedGridRow);
        }
    });

    overlay.addEventListener("click", function () {
        // Ẩn lớp phủ và các bản sao
        overlay.classList.remove("active");
        const allClonedRows = document.querySelectorAll(".grid__row.row.clone");
        allClonedRows.forEach(row => row.remove()); // Xóa các bản sao khỏi DOM
    });
});
