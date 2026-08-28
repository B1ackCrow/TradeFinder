<template>
<div class="product-form-component">
  <div class="product-form-component__body">

    <AppImageInput
        v-model="imageFile"
        :image-url="imagePreview"
        placeholder="Выберите изображение логотипа"
        alt="Изображение продукта"
        @error="error = $event"
    />

    <div class="product-settings">
      <input type="text" v-model="productName" placeholder="Название продукта" class="input-form"/>
    </div>

    <div class="product-settings">
      <div class="selected-category-name" :class="{ 'selected-category-name--placeholder': !selectedCategory }">
        <div class="selected-category-name-text">
          {{ selectedCategory?.name || 'Категория' }}
        </div>
        <button @click="clearSelectedCategory" v-if="selectedCategory" class="delete-selected-category-name">@</button>
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
import { computed, onBeforeUnmount, ref, watch } from 'vue';
import {useProductsStore} from '@/store/products';
import { storeToRefs } from 'pinia';

import AppTreeSelector from "@/components/ui/AppTreeSelector.vue";
import AppImageInput from "@/components/ui/AppImageInput.vue";

const selectedCategoryId = ref(null);
const imageFile = ref(null);
const imagePreview = ref(null);

const productName = ref('');
const error = ref("");
const success = ref("");

const productsStore = useProductsStore();
const { products: categories } = storeToRefs(productsStore);

const selectedProductId = defineModel("selectedProductId");

const clearSelectedCategory = () => {
  selectedCategoryId.value = null;
}

const saveProduct = async () => {
  error.value = "";
  success.value = "";

  if(productName.value === "" || productName.value <= 0) {
    error.value = "Заполните поле имени продукта";
    return;
  }

  if(selectedProductId.value === selectedCategoryId.value) {
    error.value = "Категория не может ссылаться на саму себя"
    return;
  }

  const formData = new FormData();

  formData.append("Name", productName.value.trim());

  if (selectedCategoryId.value) {
    formData.append("ParentProductId", selectedCategoryId.value);
  }

  if (imageFile.value) {
    formData.append("Image", imageFile.value);
  }

  try {
    if(selectedProductId.value) {
      formData.append("Id", selectedProductId.value);
      await productsStore.updateProduct(formData);
    }
    else {
      selectedProductId.value = await productsStore.createProduct(formData);
    }
    await productsStore.loadProducts();
    success.value = "Сохранено";
  }
  catch (err) {
    if(err.response) {
      error.value = err.response.data
    } else {
      error.value = 'Не удалось подключиться к серверу'
    }
  }
}

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
  return findCategory(categories.value, selectedCategoryId.value);
});

onBeforeUnmount(() => {
  if (imagePreview.value) {
    URL.revokeObjectURL(imagePreview.value);
  }
});

watch(selectedProductId, async (id) => {
  error.value = "";
  success.value = "";

  if (!id) {
    productName.value = "";
    selectedCategoryId.value = null;
    imageFile.value = null;

    if (imagePreview.value?.startsWith("blob:")) { {
      URL.revokeObjectURL(imagePreview.value);}
    }

    imagePreview.value = null;

    return;
  }

  const product = await productsStore.getProduct(id);

  productName.value = product.name;
  selectedCategoryId.value = product.parentId;

  imageFile.value = null;
  imagePreview.value = product.image ? `http://localhost:5185${product.image}` : null;
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

.delete-selected-category-name {
  background: none;
  border: none;
  cursor: pointer;
  opacity: 0.5;
}

.delete-selected-category-name:hover {
  background: none;
  border: none;
  cursor: pointer;
  opacity: 1;
}

.selected-category-name--placeholder {
  opacity: 0.4;
}
</style>