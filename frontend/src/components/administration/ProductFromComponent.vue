<template>
<div class="product-form-component">
  <div class="product-form-component__body">

    <label class="product-image">
      <input
          type="file"
          hidden
          accept="image/*"
          @change="handleImageChange"
      >

      <img
          v-if="imagePreview"
          :src="imagePreview"
          alt="Изображение продукта"
          class="product-image__preview"
      >

      <span
          v-else
          class="product-image-placeholder"
      >
    Выберите изображение
  </span>
    </label>

    <div class="product-settings">
      <input type="text" v-model="productName" placeholder="Название продукта" class="input-form"/>
    </div>

    <div class="product-settings">
      <div class="selected-category-name" :class="{ 'selected-category-name--placeholder': !selectedCategory }">
        {{ selectedCategory?.name || 'Категория' }}
      </div>
      <AppTreeSelector class="app-tree-selector surface" :items="categories" v-model="selectedCategoryId"/>
    </div>

  </div>
  <div class="product-form-component__footer">
    <div class="error-message" v-if="error">{{error}}</div>
    <div class="success-message" v-if="success">{{success}}</div>
    <button @click="saveProduct" class="filled-button">Сохранить</button>
  </div>
</div>
</template>

<script setup>
import { computed, onBeforeUnmount, ref } from 'vue';
import {useProductsStore} from '@/store/products';

import AppTreeSelector from "@/components/ui/AppTreeSelector.vue";

let selectedCategoryId = ref(null);

const imageFile = ref(null);
const imagePreview = ref(null);

const productsStore = useProductsStore();

const categories = productsStore.products;
console.log(categories);

let productName = ref('');

const error = ref("");
const success = ref("");
const saveProduct = async () => {
  const product = {
    name: productName,
    image: null,
    parentProductId: selectedCategoryId
  };

  try {
    const response = await productsStore.createProduct();
    const selectedProduct = await productsStore.getProduct(response.data);

    await productsStore.loadProducts();

    productName = selectedProduct?.response.name;
    selectedCategoryId = selectedProduct?.response.parentProductId;

    success.value = "Сохранено успешно";
  }
  catch (err) {
    if(err.response) {
      error.value = err.response.data
    } else {
      error.value = 'Не удалось подключиться к серверу'
    }
  }



}

const handleImageChange = (event) => {
  const file = event.target.files?.[0];

  if (!file) {
    return;
  }

  if (!file.type.startsWith('image/')) {
    return;
  }

  if (imagePreview.value) {
    URL.revokeObjectURL(imagePreview.value);
  }

  imageFile.value = file;
  imagePreview.value = URL.createObjectURL(file);
};

const findCategory = (items, id) => {
  for (const item of items) {
    if (item.id === id) {
      return item;
    }

    if (item.children?.length) {
      const found = findCategory(item.children, id);

      if (found) {
        return found;
      }
    }
  }

  return null;
};

const selectedCategory = computed(() => {
  return findCategory(categories, selectedCategoryId.value);
});

onBeforeUnmount(() => {
  if (imagePreview.value) {
    URL.revokeObjectURL(imagePreview.value);
  }
});

</script>

<style scoped>
.product-form-component {
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.product-form-component__body {
  display: flex;
  gap: 20px;
  width: 100%;
}

.product-image {
  width: 250px;
  height: 250px;

  border-radius: 5px;
  border: 1px solid rgba(200, 200, 200, 0.5);

  cursor: pointer;

  display: flex;
  justify-content: center;
  align-items: center;

  overflow: hidden;
  box-sizing: border-box;
}

.product-image-placeholder {
  opacity: 0.4;
  font-weight: 200;
  font-size: 14px;
}

.product-image__preview {
  width: 100%;
  height: 100%;

  object-fit: cover;
  display: block;
}

.product-form-component__footer {
  display: flex;
  flex-direction: column;
  gap: 10px;
  margin-top: auto;
  width: 100%;
}

.error-message {
  color: red;
  opacity: 0.8;
  font-size: 16px;
  font-weight: 300;
}

.success-message {
  color: green;
  opacity: 0.8;
  font-size: 16px;
  font-weight: 300;
}

.app-tree-selector {
  width: 350px;
  height: 450px;
  padding: 15px;
  box-sizing: border-box;
}

.product-settings {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.selected-category-name {
  width: 350px;
  height: 50px;

  padding: 15px;

  background: #ffffff;

  border: 1px solid rgba(200, 200, 200, 0.5);
  border-radius: 5px;

  box-sizing: border-box;

  display: flex;
  align-items: center;

  font-size: 15px;
  font-weight: 300;
}

.selected-category-name--placeholder {
  opacity: 0.4;
}
</style>