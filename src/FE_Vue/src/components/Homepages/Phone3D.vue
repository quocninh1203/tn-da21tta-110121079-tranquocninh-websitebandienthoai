<template>
  <div class="home-page">
    <div class="phone-3d-wrapper">
      <ThreeDModel
        class="phone-3d-model"
        file-path="/models/oppo_reno_8_pro.glb"
        :custom-settings="{
          autoRotate: true,
          scale: 0.8,
          enableZoom: false
        }"
      />
    </div>
    <div class="phone-info">
      <h2>Oppo Reno 8 Pro</h2>
      <p>
        Điện thoại Oppo Reno 8 Pro với thiết kế sang trọng, camera chất lượng cao,
        hiệu năng mạnh mẽ và pin lâu dài.
      </p>
      <button @click="goToDetail">Xem chi tiết</button>
    </div>
  </div>
</template>

<script setup>
import ThreeDModel from "view-3d-model"
import { useRouter } from 'vue-router'

const router = useRouter()

function goToDetail() {
  router.push('/product/oppo-reno-8-pro')
}
</script>

<style scoped>
.home-page {
  display: flex;
  align-items: center;
  gap: 40px;
  padding: 20px;
  max-width: 1000px;
  margin: 50px;
}
.phone-3d-wrapper {
  flex: 1;
  max-width: 400px;
  height: 600px;
  overflow: hidden;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-left: 30px;
}
.phone-3d-model {
  width: 100%;
  height: 100%;
  pointer-events: none;
}
.phone-info {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 20px;
  margin-left: 300px; /* dịch sang phải */
}
.phone-info h2 {
  font-size: 2rem;
  margin: 0;
}
.phone-info p {
  font-size: 1.1rem;
  line-height: 1.5;
}
.phone-info button {
  width: 150px;
  padding: 10px;
  font-size: 1rem;
  background: linear-gradient(90deg, #ff5722 60%, #ff9800 100%);
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-weight: bold;
  transition: background 0.2s;
}
.phone-info button:hover {
  background: linear-gradient(90deg, #e65100 60%, #ff5722 100%);
}
</style>

