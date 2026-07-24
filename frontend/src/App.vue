<template>
  <header-component/>

  <h1>You did it!</h1>
  <p>
    Visit <a href="https://vuejs.org/" target="_blank" rel="noopener">vuejs.org</a> to read the
    documentation
  </p>

  <footer-component/>
  <cookie-component/>
</template>

<script setup>
import FooterComponent from './components/global-components/FooterComponent.vue'
import HeaderComponent from './components/global-components/HeaderComponent.vue'
import CookieComponent from "@/components/global-components/CookieComponent.vue";

</script>

<style>
body {
  background: gray;
}
</style> 
