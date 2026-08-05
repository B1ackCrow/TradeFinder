<template>
<div class="product-form-component">
  <div class="product-form-component__body">
    <label class="product-image">
      <input type="file" hidden accept="image/*">
      <span class="product-image-placeholder">Выберите изображение</span>
    </label>
    <input type="text" placeholder="Название продукта" class="input-form"/>
    <TreeComponent
        :categories="categories"
    />
  </div>
  <div class="product-form-component__footer">
    <div class="error-message">Это пример ошибки</div>
    <button class="filled-button">Сохранить</button>
  </div>
</div>
</template>

<script setup>
import TreeComponent from "@/components/ui/TreeComponent.vue";

const categories = [
  {
    id: 1,
    name: 'Металл',
    children: [
      {
        id: 2,
        name: 'Трубы',
        children: [
          {
            id: 3,
            name: 'Профильная труба'
          },
          {
            id: 4,
            name: 'Нержавеющая труба'
          }
        ]
      },
      {
        id: 5,
        name: 'Арматура'
      }
    ]
  },
  {
    id: 10,
    name: 'Бетон',
    children: [
      {
        id: 11,
        name: 'Товарный бетон'
      }
    ]
  }
]
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
}

.product-image-placeholder {
  opacity: 0.4;
  font-weight: 200;
  font-size: 14px;
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

</style>