<template>
<div class="company-form-component">
  <AppImageInput
    v-model="imageFile"
    :image-url="imagePreview"
    placeholder="Выберите изображение логотипа"
    alt="Изображение продукта"
    @error="error = $event"
  />

  <AppInput
    v-model="companyName"
    label="Название компании"
    :clearable="true"
    placeholder="ООО Компания"
  />

  <AppInput
    label="Описание"
    :clearable="true"
    placeholder="Деятельность компании"
    multiline
  />

  <AppInput
    label="ИНН"
    :clearable="true"
    placeholder="0000000000000"
    mask="############"
  />

  <div>Контакты</div>
  <div>Локации</div>
  <div>Продукты</div>
</div>
</template>

<script setup>
import {ref} from "vue";
import AppImageInput from "@/components/ui/AppImageInput.vue";
import AppInput from "@/components/ui/AppInput.vue";

const imageFile = ref(null);
const imagePreview = ref(null);

const error = ref("");

const companyName = ref("");
</script>

<style scoped>

</style>