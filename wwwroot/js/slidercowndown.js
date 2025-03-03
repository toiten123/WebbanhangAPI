document.addEventListener('DOMContentLoaded', () => {
    const productSlider = document.querySelector('.product-slider');
    const prevButton = document.querySelector('.prev-button');
    const nextButton = document.querySelector('.next-button');
    let index = 0;

    const totalProducts = document.querySelectorAll('.product-card').length;
    const productsPerSlide = 4;
    const maxIndex = Math.ceil(totalProducts / productsPerSlide) - 1; // Tính số slide tối đa

    let autoSlideInterval = setInterval(autoSlide, 3000); // Tự động chạy mỗi 3 giây

    prevButton.addEventListener('click', () => {
        if (index > 0) {
            index--;
        } else {
            index = maxIndex; // Quay về slide cuối cùng
        }
        updateSlider();
        resetAutoSlide(); // Reset lại thời gian tự động
    });

    nextButton.addEventListener('click', () => {
        if (index < maxIndex) {
            index++;
        } else {
            index = 0; // Quay lại slide đầu tiên
        }
        updateSlider();
        resetAutoSlide(); // Reset lại thời gian tự động
    });

    function updateSlider() {
        const offset = -index * (200 / productsPerSlide);
        productSlider.style.transform = `translateX(${offset}%)`;
    }

    function resetAutoSlide() {
        clearInterval(autoSlideInterval); // Dừng auto slide cũ
        autoSlideInterval = setInterval(autoSlide, 3000); // Bắt đầu lại tự động chạy
    }

    function autoSlide() {
        if (index < maxIndex) {
            index++;
        } else {
            index = 0; // Quay lại slide đầu tiên
        }
        updateSlider();
    }
});

// Đếm ngược
function updateCountdown() {
    const daysEl = document.getElementById('days');
    const hoursEl = document.getElementById('hours');
    const minutesEl = document.getElementById('minutes');
    const secondsEl = document.getElementById('seconds');

    let days = parseInt(daysEl.textContent);
    let hours = parseInt(hoursEl.textContent);
    let minutes = parseInt(minutesEl.textContent);
    let seconds = parseInt(secondsEl.textContent);

    function updateTime() {
        seconds--;

        if (seconds < 0) {
            seconds = 59;
            minutes--;

            if (minutes < 0) {
                minutes = 59;
                hours--;

                if (hours < 0) {
                    hours = 23;
                    days--;

                    if (days < 0) {
                        clearInterval(timerInterval);
                        return;
                    }
                }
            }
        }

        daysEl.textContent = days.toString().padStart(2, '0');
        hoursEl.textContent = hours.toString().padStart(2, '0');
        minutesEl.textContent = minutes.toString().padStart(2, '0');
        secondsEl.textContent = seconds.toString().padStart(2, '0');
    }

    const timerInterval = setInterval(updateTime, 1000);
}

document.addEventListener('DOMContentLoaded', updateCountdown);