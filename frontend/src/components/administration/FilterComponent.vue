<template>
<div class="filter-component">
  <div class="filter-component__header">
    <button @click="selectCompany" class="select-form-button" :class="{ active: selectedType === 'company' }">Компании</button>
    <button @click="selectProduct" class="select-form-button" :class="{ active: selectedType === 'product' }">Продукция</button>
  </div>
  <div class="filter-component__body">

  </div>
</div>
</template>

<script setup>
const selectedType = defineModel()

const selectCompany = () => {
  selectedType.value = "company"
}

const selectProduct = () => {
  selectedType.value = "product"
}
</script>

<style scoped>
.filter-component {
  padding: 0 20px 0 20px;
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

</style>