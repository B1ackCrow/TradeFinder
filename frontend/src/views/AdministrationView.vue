<template>
<div class="administration-view">
  <filter-component class="filter-component surface" v-model="selectedForm"/>
  <company-form-component class="form-component surface" v-if="selectedForm === 'company'"/>
  <product-from-component class="form-component surface" v-if="selectedForm === 'product'"/>
</div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import {useProductsStore} from "../store/products.js";

const productStore = useProductsStore();

onMounted(() => {
  productStore.loadProducts();
})

import FilterComponent from "@/components/administration/FilterComponent.vue";
import CompanyFormComponent from "@/components/administration/CompanyFormComponent.vue";
import ProductFromComponent from "@/components/administration/ProductFromComponent.vue";

const selectedForm = ref("company");

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