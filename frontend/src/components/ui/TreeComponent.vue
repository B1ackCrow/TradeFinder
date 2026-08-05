<script setup>
import { ref } from 'vue'

const props = defineProps({
  categories: {
    type: Array,
    required: true
  }
})

const emit = defineEmits([
  'select'
])

const expanded = ref(new Set())
const selected = ref(null)

function toggle(id) {
  if (expanded.value.has(id)) {
    expanded.value.delete(id)
  } else {
    expanded.value.add(id)
  }

  expanded.value = new Set(expanded.value)
}

function select(category) {
  selected.value = category.id
  emit('select', category)
}
</script>


<template>
  <div class="tree">

    <template
        v-for="item in categories"
        :key="item.id"
    >

      <div
          class="tree-item"
          :class="{ selected: selected === item.id }"
      >

        <button
            v-if="item.children?.length"
            class="expand"
            @click="toggle(item.id)"
        >
          {{ expanded.has(item.id) ? '−' : '+' }}
        </button>

        <span
            v-else
            class="empty"
        />


        <span
            class="title"
            @click="select(item)"
        >
          {{ item.name }}
        </span>

      </div>


      <div
          v-if="item.children?.length && expanded.has(item.id)"
          class="children"
      >
        <CategoryTree
            :categories="item.children"
            @select="$emit('select', $event)"
        />
      </div>

    </template>

  </div>
</template>


<style scoped>

.tree {
  font-family: Inter, sans-serif;
  font-size: 15px;
}


.tree-item {
  height: 42px;
  display: flex;
  align-items: center;
  border-radius: 8px;
  cursor: pointer;
  padding: 0 12px;
}


.tree-item:hover {
  background: #f5f7fa;
}


.tree-item.selected {
  background: #e8f1ff;
  color: #1765d1;
}


.expand {
  width: 26px;
  height: 26px;

  border: none;
  background: transparent;

  font-size: 18px;
  color: #777;

  cursor: pointer;
}


.empty {
  width: 26px;
}


.title {
  flex: 1;
}


.children {
  margin-left: 22px;
  border-left: 1px solid #e4e7ec;
  padding-left: 12px;
}

</style>