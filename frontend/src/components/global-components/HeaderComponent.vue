<template>
  <header class="header-component">

    <div class="header-left">

      <div class="logo-text">
        <h2>СтройПоиск</h2>
        <p>Поиск строительных компаний</p>
      </div>

      <nav class="header-menu">
        <a href="#">Компании</a>
        <a href="#">Товары</a>
      </nav>

    </div>

    <div class="header-right">
      <a href="#">Избранное</a>
      <a href="#">Войти</a>
    </div>

  </header>
</template>

<script setup>
</script>

<style scoped>
.header-component {
    display: flex;
    justify-content: space-between;
    align-items: center;

    padding: 18px 60px;

    background: white;
    border-bottom: 1px solid #e5e5e5;
}

/* Левая часть */
.header-left {
    display: flex;
    align-items: center;
    gap: 60px;
}

/* Логотип */
.logo-text {
    display: flex;
    flex-direction: column;
}

.logo-text h2 {
    margin: 0;
    font-size: 24px;
    font-weight: 700;
    color: #2b2b2b;
    line-height: 1.1;
}

.logo-text p {
    margin: 2px 0 0;
    font-size: 11px;
    color: #8a8a8a;
    line-height: 1.2;
}

/* Меню */
.header-menu {
    display: flex;
    gap: 40px;
}

.header-menu a {
    text-decoration: none;
    color: #333;
    font-size: 16px;
    font-weight: 500;
}

.header-menu a:hover {
    color: #1f6fe5;
}

/* Правая часть */
.header-right {
    display: flex;
    gap: 30px;
}

.header-right a {
    text-decoration: none;
    color: #333;
    font-size: 16px;
    font-weight: 500;
}

.header-right a:hover {
    color: #1f6fe5;
}
</style>