<template>
  <div class="footer-component">
    <div class="footer-item">
      <div class="item-icon-background">
        <img class="item-icon" src="@/assets/icons/footer/shield.svg" alt="shield">
      </div>
      <div class="item-info">
        <div class="info-title">Проверенные компании</div>
        <div class="info-text">Только надежные поставщики и подрядчики</div>
      </div>
    </div>

    <div class="footer-item">
      <div class="item-icon-background">
        <img class="item-icon" src="@/assets/icons/footer/shield.svg" alt="shield">
      </div>
      <div class="item-info">
        <div class="info-title">Удобный поиск</div>
        <div class="info-text">Находите компании и товары быстро и легко</div>
      </div>
    </div>

    <div class="footer-item">
      <div class="item-icon-background">
        <img class="item-icon" src="@/assets/icons/footer/shield.svg" alt="shield">
      </div>
      <div class="item-info">
        <div class="info-title">Актуальная информация</div>
        <div class="info-text">Свежие данные о компаниях и их предложениях</div>
      </div>
    </div>

    <div class="footer-item">
      <div class="item-icon-background">
        <img class="item-icon" src="@/assets/icons/footer/shield.svg" alt="shield">
      </div>
      <div class="item-info">
        <div class="info-title">Поддержка</div>
        <div class="info-text">Поможем найти лучшее решение для вашего проекта</div>
      </div>
    </div>
  </div>
</template>

<script setup>
</script>

<style scoped>
.footer-component {
  background: #ffffff;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 40px 100px;
  width: 100%;
  box-sizing: border-box;
  position: relative;
}

.footer-component:before {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 1px;
  background-color: rgba(200, 200, 200, 0.5);
}

.footer-item {
  display: flex;
  align-items: center;
  gap: 16px;
}

.item-icon-background {
  background: #f3f4f7;
  width: 60px;
  height: 60px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
}

.item-icon {
  width: 30px;
}

.item-info {
  display: flex;
  flex-direction: column;
  gap: 10px;
  max-width: 250px;
}

.info-title {
  font-size: 16px;
  font-weight: 500;
}

.info-text {
  font-size: 14px;
  font-weight: 300;
  opacity: 0.8;
}
</style>