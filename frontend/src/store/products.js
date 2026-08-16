import { defineStore } from 'pinia';
import { ref } from 'vue';

import {getAllProducts, createNewProduct, getProductById, updateProductApi} from '@/api/products';

export const useProductsStore = defineStore('products', () => {

    const products = ref([]);

    //Получает всю продукцию
    const loadProducts = async () => {
        const response = await getAllProducts();
        products.value = response.data;
    };

    //Создает продукт
    const createProduct = async (product) => {
        const response = await createNewProduct(product);

        return response.data;
    };

    //Получает продукт по Id
    const getProduct = async (productId) => {
        const response = await getProductById(productId);

        return response.data;
    }

    const updateProduct = async (product) => {
        await updateProductApi(product);
    }

    return {
        products,
        loadProducts,
        createProduct,
        getProduct,
        updateProduct
    };
});