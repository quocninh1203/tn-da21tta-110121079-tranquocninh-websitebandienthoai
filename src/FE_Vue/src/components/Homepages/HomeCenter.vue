<template>
  <div class="home-center">
    <div class="left-slider">
      <img :src="imagesLeft[currentLeft]" alt="Slider" class="main-img" />
    </div>
    <div class="right-images">
      <img :src="imagesRight[0]" alt="Right Top" class="side-img top-img" />
      <img :src="imagesRight[1]" alt="Right Bottom" class="side-img bottom-img" />
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount } from 'vue'

const imagesLeft = [
  '/src/assets/images/left_1.png',
  '/src/assets/images/left_2.jpg',
  '/src/assets/images/left_3.png',
]
const imagesRight = [
  '/src/assets/images/right_1.jpg',
  '/src/assets/images/right_2.jpg'
]

const currentLeft = ref(0)
let intervalId

onMounted(() => {
  intervalId = setInterval(() => {
    currentLeft.value = (currentLeft.value + 1) % imagesLeft.length
  }, 2500)
})

onBeforeUnmount(() => {
  clearInterval(intervalId)
})
</script>

<style scoped>
.home-center {
  display: flex;
  justify-content: flex-start;       /* Sát nhau */
  align-items: flex-start;
  gap: 64px;                          /* Khoảng cách nhỏ giữa 2 cụm */
  width: 100%;
  padding: 32px 0;
  flex-wrap: wrap;                   /* Đảm bảo responsive trên màn nhỏ */
}

.left-slider {
  flex: 4;
  display: flex;
  justify-content: flex-end;
  align-items: center;
}

.main-img {
  width: 100%;
  max-width: 620px;
  height: auto;
  object-fit: cover;
  border-radius: 18px;
  box-shadow: 0 4px 16px rgba(255, 87, 34, 0.15);
  transition: opacity 0.5s;
}

.right-images {
  flex: 3;
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  justify-content: flex-start;
  gap: 52px;
}

.side-img {
  width: 100%;
  max-width: 430px;
  height: auto;
  object-fit: cover;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(255, 87, 34, 0.10);
}

@media (max-width: 1024px) {
  .home-center {
    flex-direction: column;
    align-items: center;
    gap: 24px;
  }

  .left-slider,
  .right-images {
    flex: unset;
    width: 100%;
    justify-content: center;
    align-items: center;
  }

  .main-img,
  .side-img {
    max-width: 90%;
  }
}
</style>

