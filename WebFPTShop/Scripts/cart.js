// cart.js
function addToCart(product) {
    // Get the current cart from localStorage
    var cart = JSON.parse(localStorage.getItem("cart")) || [];

    // Check if the product is already in the cart
    var existingProduct = cart.find((item) => item.name === product.name);

    if (existingProduct) {
        // If the product is already in the cart, increment its quantity
        existingProduct.quantity++;
    } else {
        // If the product is not in the cart, add it to the cart with a quantity of 1
        product.quantity = 1;
        cart.push(product);
    }

    // Update the cart in localStorage
    localStorage.setItem("cart", JSON.stringify(cart));
}

function getCart() {
    // Get the current cart from localStorage
    return JSON.parse(localStorage.getItem("cart")) || [];
}

// Initialize the cart if it doesn't exist
if (!localStorage.getItem("cart")) {
    localStorage.setItem("cart", "[]");
}
