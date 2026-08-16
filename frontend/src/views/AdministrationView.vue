<template>
<div class="administration-view">
  <filter-component
      class="filter-component surface"
      v-model="selectedForm"
      v-model:selected-product-id="selectedProductId"
  />
  <company-form-component
      class="form-component surface"
      v-if="selectedForm === 'company'"
  />
  <product-form-component
      class="form-component surface"
      v-if="selectedForm === 'product'"
      v-model:selected-product-id="selectedProductId"
  />
</div>
</template>

<script setup>
import { ref, onMounted, watch } from "vue";
import {useProductsStore} from "../store/products.js";

import FilterComponent from "@/components/administration/FilterComponent.vue";
import CompanyFormComponent from "@/components/administration/CompanyFormComponent.vue";
import ProductFormComponent from "@/components/administration/ProductFormComponent.vue";

const productStore = useProductsStore();

const selectedForm = ref("company");
const selectedProductId = ref(null);

onMounted(() => {
  productStore.loadProducts();
});

watch(selectedProductId, (id) => {
  console.log("Выбран продукт:", id);
})
</script>

<style scoped>
.administration-view {
  display: flex;
  gap: 20px;
  width: 100%;
  max-width: 1400px;
  box-sizing: border-box;
}

.filter-component {
  padding: 0;
  width: 100%;
  max-width: 300px;
  height: 700px;
}

.form-component {
  width: 100%;
  padding: 20px;
  box-sizing: border-box;
}


</style>