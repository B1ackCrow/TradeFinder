<template>
<div class="footer-bg">
  <div class="footer-component">

    <div class="footer-item">
      <div class="footer-icon">
        <img :src="shieldIcon">
      </div>
      <div>
        <h3>Проверенные компании</h3>
        <p>Только надежные поставщики и подрядчики</p>
      </div>
    </div>

    <div class="footer-item">
      <div class="footer-icon">
        <img :src="shieldIcon">
      </div>
      <div>
        <h3>Удобный поиск</h3>
        <p>Находите компании и товары быстро и легко</p>
      </div>
    </div>

    <div class="footer-item">
      <div class="footer-icon">
        <img :src="shieldIcon">
      </div>
      <div>
        <h3>Актуальная информация</h3>
        <p>Свежие данные о компаниях и их предложениях</p>
      </div>
    </div>

    <div class="footer-item">
      <div class="footer-icon">
        <img :src="shieldIcon">
      </div>
      <div>
        <h3>Поддержка</h3>
        <p>Поможем найти лучшее решение для вашего проекта</p>
      </div>
    </div>
     

  </div>
  </div>
</template>

<script setup>
import shieldIcon from '@/assets/icons/footer/shield.svg'
</script>

<style scoped>
.footer-bg {
  background: #f5f5f5;
  width: 100%;
}

.footer-component {
  display: flex;
  justify-content: space-between;
  padding: 40px;
  max-width: 1200px;
  width: 100%;
  margin: 0 auto;
}

.footer-item {
  width: 220px;
  display: flex;
  align-items: flex-start;
  gap: 12px;
}

.footer-item h3 {
  font-size: 18px;
  font-weight: 600;
  margin: 0 0 6px 0;
}

.footer-item p {
  font-size: 14px;
  color: #666;
  line-height: 1.5;
  margin: 0;
}

.footer-icon {
  width: 40px;
  height: 40px;
  background: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  margin-top: 2px;
}

.footer-item img {
  width: 22px;
  height: 22px;
  object-fit: contain;
}
</style>