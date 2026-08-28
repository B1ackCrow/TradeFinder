<template>
  <label class="app-image-input">
    <input
        type="file"
        hidden
        accept="image/*"
        @change="handleImageChange"
    >

    <img
        v-if="preview"
        :src="preview"
        :alt="alt"
        class="app-image-input__preview"
    >

    <span
        v-else
        class="app-image-input__placeholder"
    >
      {{ placeholder }}
    </span>
  </label>
</template>

<script setup>
import { onBeforeUnmount, ref, watch } from "vue";

const props = defineProps({
  imageUrl: {
    type: String,
    default: null
  },

  placeholder: {
    type: String,
    default: "Выберите изображение"
  },

  alt: {
    type: String,
    default: "Изображение"
  },

  maxSize: {
    type: Number,
    default: 10 * 1024 * 1024
  }
});

const imageFile = defineModel();

const emit = defineEmits([
  "error"
]);

const preview = ref(props.imageUrl);

const clearBlobPreview = () => {
  if (preview.value?.startsWith("blob:")) {
    URL.revokeObjectURL(preview.value);
  }
};

const handleImageChange = (event) => {
  const file = event.target.files?.[0];

  if (!file) {
    return;
  }

  if (!file.type.startsWith("image/")) {
    emit("error", "Можно загружать только изображения");
    event.target.value = "";
    return;
  }

  if (file.size > props.maxSize) {
    emit("error", "Размер изображения не должен превышать 10 МБ");
    event.target.value = "";
    return;
  }

  clearBlobPreview();

  imageFile.value = file;
  preview.value = URL.createObjectURL(file);
};

watch(
    () => props.imageUrl,
    (newImageUrl) => {
      clearBlobPreview();

      preview.value = newImageUrl;
      imageFile.value = null;
    }
);

onBeforeUnmount(() => {
  clearBlobPreview();
});
</script>

<style scoped>
.app-image-input {
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

.app-image-input__placeholder {
  opacity: 0.4;
  font-weight: 200;
  font-size: 14px;
}

.app-image-input__preview {
  width: 100%;
  height: 100%;

  object-fit: cover;
  display: block;
}
</style>