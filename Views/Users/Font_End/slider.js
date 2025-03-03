// thuộc tính cố định đầu trang
document.addEventListener("DOMContentLoaded", () => {
    const header = document.querySelector(".header-fixed");
    const headerOffsetTop = header.offsetTop; // Vị trí ban đầu của header

    // Kiểm tra trạng thái từ localStorage
    if (localStorage.getItem("headerFixed") === "true") {
        header.classList.add("fixed");
    }

    window.addEventListener("scroll", () => {
        if (window.scrollY > headerOffsetTop) {
            header.classList.add("fixed");
            localStorage.setItem("headerFixed", "true"); // Lưu trạng thái
        } else {
            header.classList.remove("fixed");
            localStorage.setItem("headerFixed", "false"); // Lưu trạng thái
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


