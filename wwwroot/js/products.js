// Hàm tạo nút "Prev" và "Next"
function createSliderControls(buttonContainer, sliderSelector, prevClass, nextClass) {
    // Kiểm tra xem phần tử buttonContainer có tồn tại không
    if (!buttonContainer) return;

    // Hàm tạo nút với SVG
    const createButton = (className, svgContent) => {
        const button = document.createElement('button');
        button.classList.add(className);
        button.innerHTML = svgContent;
        return button;
    };

    // SVG cho nút "Prev"
    const prevButtonSvg = `
        <svg width="18" height="18" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 50 50">
            <polygon points="34.675,47.178 12.497,25 34.675,2.822 37.503,5.65 18.154,25 37.503,44.35"></polygon>
        </svg>
    `;
    // SVG cho nút "Next"
    const nextButtonSvg = `
        <svg width="18" height="18" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24">
            <polygon points="6.379,20.908 7.546,22.075 17.621,12 7.546,1.925 6.379,3.092 15.287,12 "></polygon>
        </svg>
    `;

    // Tạo các nút "Prev" và "Next" bằng hàm createButton
    const prevButton = createButton(prevClass, prevButtonSvg);
    const nextButton = createButton(nextClass, nextButtonSvg);

    // Thêm các nút vào container
    buttonContainer.appendChild(prevButton);
    buttonContainer.appendChild(nextButton);

    // Lấy phần tử slider tương ứng
    const sliderWrapper = document.querySelector(`${sliderSelector} .product-slider-wrapper`);
    const sliderItems = sliderWrapper.querySelectorAll('.product-card-item');
    const itemWidth = sliderItems[0].offsetWidth + 13; // Khoảng cách giữa các sản phẩm
    let isTransitioning = false; // Biến để kiểm tra xem slider có đang chuyển động không

    // Hàm xử lý khi nhấn nút "Next"
    const handleNext = () => {
        if (isTransitioning) return;
        isTransitioning = true;

        sliderWrapper.style.transition = 'transform 0.3s ease';
        sliderWrapper.style.transform = `translateX(-${2 * itemWidth}px)`;

        sliderWrapper.addEventListener('transitionend', () => {
            const firstItem = sliderWrapper.firstElementChild;
            sliderWrapper.appendChild(firstItem);
            sliderWrapper.style.transition = 'none';
            sliderWrapper.style.transform = `translateX(-${itemWidth}px)`;

            isTransitioning = false;
        }, { once: true });
    };

    // Hàm xử lý khi nhấn nút "Prev"
    const handlePrev = () => {
        if (isTransitioning) return;
        isTransitioning = true;

        sliderWrapper.style.transition = 'transform 0.3s ease';
        sliderWrapper.style.transform = `translateX(0px)`;

        sliderWrapper.addEventListener('transitionend', () => {
            const lastItem = sliderWrapper.lastElementChild;
            sliderWrapper.prepend(lastItem);
            sliderWrapper.style.transition = 'none';
            sliderWrapper.style.transform = `translateX(-${itemWidth}px)`;

            isTransitioning = false;
        }, { once: true });
    };

    // Gán sự kiện click cho các nút "Prev" và "Next"
    prevButton.addEventListener('click', handlePrev);
    nextButton.addEventListener('click', handleNext);
}
