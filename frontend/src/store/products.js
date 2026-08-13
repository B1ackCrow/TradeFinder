import { defineStore } from 'pinia';
import { ref } from 'vue';

import { getAllProducts, createNewProduct, getCurrentProduct } from '@/api/products';

export const useProductsStore = defineStore('products', () => {

    const products = ref([]);
    const currentProduct = ref(null)
    const currentProductId = ref(null)

    //Получает всю продукцию
    const loadProducts = async () => {
        const response = await getAllProducts();
        products.value = response.data;
    };

    //Создает продукт
    const createProduct = async (product) => {
        const response = await createNewProduct(product);
        currentProductId.value = response.data;
    };

    //Получает продукт по Id
    const getProduct = async (productId) => {
        const response = await getCurrentProduct(productId);
        currentProduct.value = response.data;
    }

    return {
        currentProduct,
        currentProductId,
        products,
        loadProducts,
        createProduct,
        getProduct
    };
});