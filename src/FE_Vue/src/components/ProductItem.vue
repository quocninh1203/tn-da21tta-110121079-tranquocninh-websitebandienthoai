<template>
  <div class="product-card" @click="handleClick">
    <img :src="product.imageUrl" alt="product" class="product-img" />
    <div class="product-name two-lines">{{ product.name }}</div>
    <div class="product-tags">
      <span class="tag">{{ screenType }}</span>
      <span class="tag">{{ screenHz }}</span>
    </div>
    <div class="product-price">{{ formatPrice(product.priceReview) }}</div>
    <div class="product-old-price">{{ formatPrice(product.priceReview + 1000000) }}</div>
    <div class="product-rating">
      <StarRating :rating="product.rating" />
    </div>
  </div>
</template>

<script setup>
import { defineProps, computed, defineEmits } from 'vue'
import StarRating from '@/components/Global/StarRating.vue'
const props = defineProps({
  product: {
    type: Object,
    required: true
  }
})
const emit = defineEmits(['click'])

function handleClick() {
  emit('click')
}

const screenType = computed(() => {
  if (!props.product.screen) return ''
  const parts = props.product.screen.split(' ')
  return parts[0] || ''
})
const screenHz = computed(() => {
  if (!props.product.screen) return ''
  const parts = props.product.screen.split(' ')
  return parts.slice(1).join(' ') || ''
})

function formatPrice(price) {
  return price ? price.toLocaleString('vi-VN') + 'đ' : ''
}
</script>

<style scoped>
@import url('https://cdn.jsdelivr.net/npm/@mdi/font@7.4.47/css/materialdesignicons.min.css');
.product-card {
  background: #fff;
  border-radius: 14px;
  box-shadow: 0 2px 12px rgba(0,0,0,0.08);
  padding: 16px 12px 12px 12px;
  width: 180px;
  min-height: 320px;
  display: flex;
  flex-direction: column;
  align-items: center;
  margin: 8px;
  cursor: pointer;
  transition: box-shadow 0.2s, transform 0.15s;
}
.product-card:hover {
  box-shadow: 0 4px 24px rgba(255,87,34,0.18);
  transform: translateY(-4px) scale(1.03);
  border: 1.5px solid #ff9800;
}
.product-card:active {
  box-shadow: 0 2px 8px rgba(255,87,34,0.12);
  transform: scale(0.98);
  border: 1.5px solid #ff5722;
}
.product-img {
  width: 120px;
  height: 120px;
  object-fit: contain;
  margin-bottom: 8px;
}
.product-name {
  font-size: 15px;
  font-weight: 600;
  margin-bottom: 6px;
  text-align: center;
  color: #222;
}
.two-lines {
  min-height: 50px;
  display: -webkit-box;
  -webkit-box-orient: vertical;
  overflow: hidden;
  word-break: break-word;
}
.product-tags {
  display: flex;
  gap: 6px;
  margin-bottom: 8px;
}
.tag {
  background: #eee;
  border-radius: 6px;
  padding: 2px 8px;
  font-size: 12px;
  color: #ccc;
  font-weight: 500;
}
.product-price {
  color: #f00;
  font-size: 18px;
  font-weight: bold;
  margin-bottom: 2px;
  margin-top: 4px;
}
.product-old-price {
  color: #888;
  font-size: 13px;
  text-decoration: line-through;
  margin-bottom: 6px;
}
.product-rating {
  display: flex;
  align-items: center;
  gap: 4px;
  font-size: 14px;
  margin-top: auto;
}
</style>
