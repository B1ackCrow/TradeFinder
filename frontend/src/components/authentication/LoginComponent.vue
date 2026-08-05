<template>
  <div class="login-component">
    <div class="welcome-back">С Возвращением</div>
    <input class="input-form" type="email" placeholder="Почта" v-model="email"/>
    <input class="input-form" type="password" placeholder="Пароль" v-model="password"/>
    <div class="error-message" v-if="error">{{ error }}</div>
    <button class="filled-button" @click="login">Войти</button>
    <router-link to='/authentication/registration' class="sign-up-href">
      Регистрация
    </router-link>
    <router-link to='/authentication/login' class="forgot-password-href">
      Забыли пароль?
    </router-link>
  </div>
</template>

<script setup>
import {ref} from "vue";
import http from "@/api/http.js"
import router from "@/router/index.js";
import {useAuthStore} from "@/store/auth.js";

const email = ref("")
const password = ref("")
const error = ref("")

async function login() {
  error.value = ''

  try {
    const response = await http.post(
        '/auth/login',
        {
          email: email.value,
          password: password.value
        }
    )

    router.push("/administration")
  }
  catch (err) {
    if(err.response) {
      error.value = err.response.data
    } else {
      error.value = 'Не удалось подключиться к серверу'
    }
  }
}

</script>

<style scoped>
.login-component {
  background: #ffffff;
  padding: 50px;
  border-radius: 5px;
  border: 1px solid rgba(200, 200, 200, 0.5);
  user-select: none;
  display: flex;
  flex-direction: column;
  justify-content: center;
  text-align: center;
  gap: 15px;
}

.welcome-back {
  font-weight: 400;
  font-size: 40px;
  opacity: 0.8;
  padding-bottom: 40px;
}

.error-message {
  color: red;
  text-align: left;
  font-size: 12px;
  opacity: 1;
  font-weight: 300;
}

.sign-up-href {
  display: flex;
  font-size: 12px;
  height: 50px;

  color: #1460ca;

  border: 1px solid #1460ca;
  border-radius: 5px;

  cursor: pointer;
  justify-content: center;
  align-items: center;
  text-align: center;
  text-decoration: none;
}

.forgot-password-href {
  font-size: 12px;
  text-decoration: none;
  margin-top: 40px;
  background: none;
  border: none;

  color: black;
  opacity: 0.5;

  cursor: pointer;

  transition: opacity 0.2s;
}

.forgot-password-href:hover {
  opacity: 0.7;
}

</style>