import http from "@/api/http.js";

export const getAllProducts = () => {
    return http.get('/products/get-all');
}

export const createNewProduct = (product) => {
    return http.post('/products/create', product);
}

export const getCurrentProduct = (productId) => {
    return http.post('/products/get', productId);
}