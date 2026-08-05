<template>
  <div class="app">
    <header-component/>

    <main class="container">
      <router-view/>
    </main>

    <footer-component/>
    <cookie-component/>
  </div>
</template>

<script setup>
import FooterComponent from './components/global-components/FooterComponent.vue'
import HeaderComponent from './components/global-components/HeaderComponent.vue'
import CookieComponent from "@/components/global-components/CookieComponent.vue";
</script>

<style>
html, body, #app {
  height: 100%;
  margin: 0;
}

body {
  font-family: 'Inter', sans-serif;
  background: #f3f4f7;
}

.app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.container {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 20px 0;
}

.surface {
  background: #ffffff;
  border-radius: 5px;
  border: 1px solid rgba(200, 200, 200, 0.5);
}

.input-form {
  width: 350px;
  height: 50px;

  padding: 0 15px;

  background: #ffffff;

  border: 1px solid rgba(200, 200, 200, 0.5);
  border-radius: 5px;

  font-size: 15px;
  font-weight: 300;

  outline: none;
  box-sizing: border-box;

  transition: border-color 0.2s ease;
}

.input-form:focus {
  border-color: #1460ca;
}

.input-form::placeholder {
  color: rgba(0, 0, 0, 0.4);
}

.filled-button {
  background: #1460ca;
  color: white;

  height: 50px;
  border-radius: 5px;

  font-size: 14px;

  border: none;
  cursor: pointer;

  transition: background 0.2s;
}

.filled-button:hover {
  background: #0f4fa8;
}
</style>