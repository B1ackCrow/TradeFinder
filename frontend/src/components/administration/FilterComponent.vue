<template>
<div class="filter-component">
  <div class="filter-component__header">
    <button @click="selectCompany" class="select-form-button" :class="{ active: selectedType === 'company' }">Компании</button>
    <button @click="selectProduct" class="select-form-button" :class="{ active: selectedType === 'product' }">Продукция</button>
  </div>
  <div class="filter-component__body">
    <app-tree-selector
        :items="categories"
        v-model="selectedProductId"
        v-if="selectedType === 'product'"
    />
  </div>
  <div class="filter-component__footer">
    <button @click="createNewProduct" class="filled-button">+ Создать новую категорию</button>
  </div>
</div>
</template>

<script setup>
import {useProductsStore} from "@/store/products.js";
import AppTreeSelector from "@/components/ui/AppTreeSelector.vue";
import {storeToRefs} from "pinia";

const productsStore = useProductsStore();
const { products: categories } = storeToRefs(productsStore);

const selectedType = defineModel()
const selectedProductId = defineModel("selectedProductId");

const selectCompany = () => {
  selectedType.value = "company"
}

const selectProduct = () => {
  selectedType.value = "product"
}
</script>

<style scoped>
.filter-component {
  display: flex;
  flex-direction: column;
}

.filter-component__header {
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: space-between;
  position: relative;
}

.filter-component__header:after {
  content: "";
  position: absolute;
  bottom: 0;
  left: 0;
  width: 100%;
  height: 1px;
  background-color: rgba(200, 200, 200, 0.5);
}

.select-form-button {
  border-radius: 5px 5px 0 0;
  background-color: #ffffff;
  border: none;
  cursor: pointer;
  opacity: 0.7;
  font-weight: 400;
  font-size: 14px;
  padding: 20px;
  width: 100%;
}

.select-form-button.active {
  opacity: 0.9;
  color: #1460ca;
}

.filter-component__body {
  flex: 1;
  padding: 10px;

  overflow: auto;
}

.filter-component__footer {
  display: flex;
  flex-direction: column;
  gap: 10px;
  margin-top: auto;
  width: 100%;
}

.filled-button {
  border-radius: 0 0 5px 5px;
}

</style>