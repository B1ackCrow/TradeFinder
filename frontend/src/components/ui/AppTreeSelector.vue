<template>
  <div class="app-tree-selector">
    <div
        v-for="item in items"
        :key="item.id"
        class="app-tree-selector__item"
    >
      <div class="app-tree-selector__parent">

        <div
            v-if="item.children?.length"
            class="parent-button"
            :class="{ expanded: expandedItems.has(item.id) }"
            @click="toggleItem(item.id)"
        >
          >
        </div>

        <div
            class="parent-name"
            :class="{ selected: selectedId === item.id }"
            @click="selectItem(item.id)"
        >
          {{ item.name }}
        </div>

      </div>

      <div
          v-if="item.children?.length && expandedItems.has(item.id)"
          class="app-tree-selector__children"
      >
        <AppTreeSelector
            v-model="selectedId"
            :items="item.children"
        />
      </div>
    </div>
  </div>
</template>

<script setup>

import { ref } from 'vue';

defineProps({
  items: {
    type: Array,
    required: true
  }
});

const selectedId = defineModel();

const expandedItems = ref(new Set());

const toggleItem = (id) => {
  if (expandedItems.value.has(id)) {
    expandedItems.value.delete(id);
  } else {
    expandedItems.value.add(id);
  }
};

const selectItem = (id) => {
  selectedId.value = id;
};

</script>

<style scoped>
.app-tree-selector {
  display: flex;
  flex-direction: column;
}

.app-tree-selector__item {
  padding: 0 5px;
}

.app-tree-selector__parent {
  display: flex;
  align-items: center;
}

.parent-button {
  width: 20px;
  height: 20px;

  display: flex;
  align-items: center;
  justify-content: center;

  cursor: pointer;

  font-size: 18px;
  line-height: 1;

  transition: transform 0.2s ease;

  font-weight: 100;
}

.parent-button.expanded {
  transform: rotate(90deg);
}

.parent-name {
  cursor: pointer;
  font-weight: 300;
  opacity: 0.7;

  transition:
      opacity 0.2s ease,
      color 0.2s ease;
}

.parent-name:hover {
  opacity: 1;
  color: #1460ca;
}

.parent-name.selected {
  opacity: 1;
  color: #1460ca;
  font-weight: 400;
}

.app-tree-selector__children {
  margin-left: 10px;
  padding-left: 14px;

  border-left: 1px solid rgba(150, 150, 150, 0.25);
}
</style>