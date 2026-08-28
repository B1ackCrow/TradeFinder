<template>
  <div
      class="app-input"
      :class="`app-input--label-${labelPosition}`"
  >
    <label
        v-if="label"
        class="app-input__label"
    >
      {{ label }}
    </label>

    <div class="app-input__container">

      <div
          v-if="icon"
          class="app-input__icon app-input__icon--left"
      >
        <img :src="icon" alt="">
      </div>

      <textarea
          v-if="multiline"
          v-model="model"
          :placeholder="placeholder"
          :disabled="disabled"
          :rows="rows"
          class="app-input__field app-input__field--textarea"
      />

      <input
          v-else
          :value="model"
          :type="inputType"
          :placeholder="placeholder"
          :disabled="disabled"
          class="app-input__field"
          @input="handleInput"
      >

      <button
          v-if="clearable && model"
          type="button"
          class="app-input__button"
          @click="clear"
      >
        ×
      </button>

      <button
          v-if="type === 'password'"
          type="button"
          class="app-input__button"
          @click="togglePassword"
      >
        {{ passwordVisible ? 'Скрыть' : 'Показать' }}
      </button>

      <slot name="right" />

    </div>
  </div>
</template>

<script setup>
import { computed, ref } from "vue";

const props = defineProps({
  type: {
    type: String,
    default: "text"
  },

  placeholder: {
    type: String,
    default: ""
  },

  label: {
    type: String,
    default: ""
  },

  labelPosition: {
    type: String,
    default: "top",
    validator: value =>
        ["top", "bottom", "left", "right"].includes(value)
  },

  icon: {
    type: String,
    default: null
  },

  clearable: {
    type: Boolean,
    default: false
  },

  disabled: {
    type: Boolean,
    default: false
  },

  multiline: {
    type: Boolean,
    default: false
  },

  rows: {
    type: Number,
    default: 4
  },

  mask: {
    type: String,
    default: null
  }
});

const model = defineModel();

const passwordVisible = ref(false);

const inputType = computed(() => {
  if (props.type !== "password") {
    return props.type;
  }

  return passwordVisible.value
      ? "text"
      : "password";
});

const clear = () => {
  model.value = "";
};

const togglePassword = () => {
  passwordVisible.value = !passwordVisible.value;
};

const handleInput = (event) => {
  let value = event.target.value;

  if (!props.mask) {
    model.value = value;
    return;
  }

  const digits = value.replace(/\D/g, "");

  let result = "";
  let digitIndex = 0;

  for (const symbol of props.mask) {
    if (symbol === "#") {
      if (digitIndex >= digits.length) {
        break;
      }

      result += digits[digitIndex];
      digitIndex++;
    } else {
      result += symbol;
    }
  }

  model.value = result;
};
</script>

<style scoped>
.app-input {
  display: flex;
  gap: 7px;
  width: 100%;
}

.app-input--label-top {
  flex-direction: column;
}

.app-input--label-bottom {
  flex-direction: column-reverse;
}

.app-input--label-left {
  flex-direction: row;
  align-items: center;
}

.app-input--label-right {
  flex-direction: row-reverse;
  align-items: center;
}

.app-input__label {
  font-size: 14px;
  font-weight: 300;
  opacity: 0.7;
}

.app-input__container {
  display: flex;
  align-items: center;

  width: 100%;
  min-height: 50px;

  background: #ffffff;

  border: 1px solid rgba(200, 200, 200, 0.5);
  border-radius: 5px;

  box-sizing: border-box;

  overflow: hidden;
}

.app-input__field {
  flex: 1;

  min-width: 0;

  padding: 15px;

  border: none;
  outline: none;
  background: transparent;

  font-size: 15px;
  font-weight: 300;

  box-sizing: border-box;
}

.app-input__field::placeholder {
  opacity: 0.4;
}

.app-input__icon {
  display: flex;
  justify-content: center;
  align-items: center;

  padding-left: 15px;
}

.app-input__icon img {
  width: 18px;
  height: 18px;

  object-fit: contain;

  opacity: 0.5;
}

.app-input__button {
  height: 100%;

  padding: 0 15px;

  border: none;
  background: transparent;

  cursor: pointer;

  opacity: 0.4;

  white-space: nowrap;
}

.app-input__button:hover {
  opacity: 0.8;
}

.app-input__field--textarea {
  resize: vertical;
  min-height: 100px;
  font-family: inherit;
  line-height: 1.4;
}
</style>