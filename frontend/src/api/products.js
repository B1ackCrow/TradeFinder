import http from "@/api/http.js";

export const getAllProducts = () => {
    return http.get('/products/get-all');
}

export const createNewProduct = (product) => {
    return http.post('/products/create', product);
}

export const getProductById = (id) => {
    return http.get('/products/get-by-id', {
        params: {id}
    });
}

export const updateProductApi = (product) => {
    return http.post('products/update', product);
}