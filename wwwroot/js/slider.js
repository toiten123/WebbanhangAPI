document.addEventListener("DOMContentLoaded", () => {
    const header = document.querySelector(".header-fixed");

    if (!header) return;

    const headerOffsetTop = header.offsetTop;

    // Xóa trạng thái trước đó khi tải trang để đảm bảo tính chính xác
    localStorage.removeItem("headerFixed");
    header.classList.remove("fixed");

    // Xử lý sự kiện cuộn trang
    window.addEventListener("scroll", () => {
        if (window.scrollY > headerOffsetTop) {
            if (!header.classList.contains("fixed")) {
                header.classList.add("fixed");
                localStorage.setItem("headerFixed", "true");
            }
        } else {
            if (header.classList.contains("fixed")) {
                header.classList.remove("fixed");
                localStorage.setItem("headerFixed", "false");
            }
        }
    });
});

// Khi chuyển sang trang khác thì sẽ có luôn quay về đầu trang
document.querySelectorAll('a').forEach(link => {
    link.addEventListener('click', () => {
        window.scrollTo(0, 0); // Cuộn về đầu trang
    });
});

const sliderContainer = document.getElementById("list-slider");
const listSliders = sliderContainer.getElementsByClassName("slider-item");

let indexSlider = 0;
let autoSlideInterval;

renderSlider(indexSlider);

function startAutoSlide() {
    clearInterval(autoSlideInterval);
    autoSlideInterval = setInterval(() => {
        if (indexSlider < listSliders.length - 1) {
            indexSlider += 1;
        } else {
            indexSlider = 0;
        }
        renderSlider();
    }, 3000);
}

startAutoSlide();

$(document).ready(function () {
    $('#icon-right').click(() => {
        if (indexSlider < listSliders.length - 1) {
            indexSlider += 1;
        } else {
            indexSlider = 0;
        }
        renderSlider();
        startAutoSlide();
    });

    $('#icon-left').click(() => {
        if (indexSlider > 0) {
            indexSlider -= 1;
        } else {
            indexSlider = listSliders.length - 1;
        }
        renderSlider();
        startAutoSlide();
    });
});

function renderSlider() {
    for (let i = 0; i < listSliders.length; i++) {
        listSliders[i].classList.remove('active', 'effect');
    }

    // Áp dụng lớp 'active' và sau đó thêm hiệu ứng lóa sáng
    listSliders[indexSlider].classList.add('active');
    setTimeout(() => {
        listSliders[indexSlider].classList.add('effect');
    }, 300); // Hiệu ứng lóa sáng trong 0.3 giây
}


