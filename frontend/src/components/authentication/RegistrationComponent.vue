<template>
  <div class="registration-component">
    <div class="welcome-back">Регистрация</div>
    <input class="input-form" type="email" placeholder="Почта" v-model="email"/>
    <input class="input-form" type="password" placeholder="Пароль" v-model="password"/>
    <input class="input-form" type="password" placeholder="Повторить пароль" v-model="doublePassword"/>

    <div class="error-message" v-if="error">
      {{ error }}
    </div>

    <button class="sing-up-button" @click="register">Регистрация</button>
    <div class="login">
      <div class="login-text">Уже есть аккаунт?</div>
      <router-link to='/authentication/login' class="login-href">
        Войти
      </router-link>
    </div>

  </div>
</template>

<script setup>
import {ref} from "vue";
import http from "@/api/http.js"

const email = ref('')
const password = ref('')
const doublePassword = ref('')
const error = ref('')

async function register() {
  error.value = ''

  if(password.value !== doublePassword.value) {
    error.value = 'Пароли не совпадают'
    return
  }

  try {
    const response = await http.post(
        '/auth/register',
        {
          email: email.value,
          password: password.value
        }
    )
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
.registration-component {
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

.sing-up-button {
  background: #1460ca;
  color: white;

  height: 50px;
  border-radius: 5px;

  font-size: 14px;

  border: none;
  cursor: pointer;

  transition: background 0.2s;
}

.sing-up-button:hover {
  background: #0f4fa8;
}

.login {
  font-size: 12px;
  margin-top: 40px;
  display: flex;
  flex-direction: row;
  justify-content: center;
  gap: 5px;
  color: black;
}

.login-text {
  opacity: 0.5;
}

.login-href {
  text-decoration: none;
  color: black;
  opacity: 0.7;

  cursor: pointer;

  transition: opacity 0.2s;
}

.login-href:hover {
  opacity: 0.9;
}

</style>