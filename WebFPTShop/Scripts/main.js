

let currentIndex = 0;
const slides = document.querySelectorAll(".carousel img");
const indicators = document.querySelector(".slide-indicators");

function changeSlide(direction) {
    // Hide the current slide
    slides[currentIndex].style.display = "none";

    // Update the current index
    currentIndex += direction;

    // Wrap around if needed
    if (currentIndex < 0) {
        currentIndex = slides.length - 1;
    } else if (currentIndex >= slides.length) {
        currentIndex = 0;
    }

    // Show the new slide
    slides[currentIndex].style.display = "block";

    // Update indicators
    showSlide(currentIndex);
}

function showSlide(index) {
    // Hiển thị số thứ tự đang được hiển thị
    const indicatorItems = indicators.querySelectorAll(".slide-indicator");
    indicatorItems.forEach((item, i) => {
        item.classList.remove("active");
        if (i === index) {
            item.classList.add("active");
        }
    });
}

// Tạo số thứ tự cho từng ảnh
slides.forEach((slide, index) => {
    const indicator = document.createElement("div");
    indicator.classList.add("slide-indicator");
    indicator.addEventListener("click", () => changeSlide(index - currentIndex));
    indicators.appendChild(indicator);
});


const slideInterval = 3000;
document.addEventListener("DOMContentLoaded", function () {





    document.querySelector(".prev").addEventListener("click", () => changeSlide(-1));
    document.querySelector(".next").addEventListener("click", () => changeSlide(1));
    showSlide(currentIndex);

    let autoSlide = setInterval(() => {
        changeSlide(1);
    }, slideInterval);

    const carousel = document.querySelector(".carousel");
    carousel.addEventListener("mouseenter", () => {
        clearInterval(autoSlide);
    });

    carousel.addEventListener("mouseleave", () => {
        autoSlide = setInterval(() => {
            changeSlide(1);
        }, slideInterval);
    });






});
const carousel = document.querySelector(".carousel-brand");
const prevButton = document.querySelector(".prev-button");
const nextButton = document.querySelector(".next-button");
let currentIndexBrand = 0;

function moveCarousel(direction) {
    const slideWidth = 120; // Kích thước của mỗi slide, tùy theo nhu cầu của bạn
    const maxIndex = carousel.children.length - 7;

    currentIndexBrand += direction;
    currentIndexBrand = Math.max(0, Math.min(currentIndexBrand, maxIndex));

    const offset = -currentIndexBrand * slideWidth;
    carousel.style.transform = `translateX(${offset}px)`;

    // Ẩn/hiện nút mũi tên dựa trên vị trí hiện tại
    if (currentIndexBrand === 0) {
        prevButton.style.display = "none";
    } else {
        prevButton.style.display = "block";
    }

    if (currentIndexBrand === maxIndex) {
        nextButton.style.display = "none";
    } else {
        nextButton.style.display = "block";
    }
}

// Khởi động carousel
moveCarousel(0);
var translateCount = 10;
const listItems = document.querySelectorAll('.product-filter-left li');
listItems.forEach((item) => {
    item.style.transform = `translateX(${translateCount}px)`;
    translateCount -= 1;
});





