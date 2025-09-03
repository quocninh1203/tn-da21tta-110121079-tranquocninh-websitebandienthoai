<template>
  <div class="display-section">
    <div class="display-header">
      <h2 class="display-title">Gợi ý cho bạn</h2>
      <div class="filter-group">
        <span
          class="brand-option"
          :class="{ active: selectedBrand === 0 }"
          @click="selectBrand(0)"
        >
          Tất cả
        </span>
        <span
          v-for="brand in brands"
          :key="brand.id"
          class="brand-option brand-img-only"
          :class="{ active: selectedBrand === brand.id }"
          @click="selectBrand(brand.id)"
        >
          <img :src="brand.imageUrl" :alt="brand.name" class="brand-img" />
        </span>
      </div>
    </div>
    <div class="product-grid">
      <ProductItem
        v-for="item in displayProducts"
        :key="item.phoneId"
        :product="item"
        @click="goToProduct(item.phoneId, item.slug)"
      />
    </div>
    <div class="view-all-wrapper">
      <button class="view-all-btn" @click="goToAll">Xem tất cả</button>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { usePhoneStore } from '@/stores/api/phoneStore'
import { useBrandStore } from '@/stores/api/brandStore'
import ProductItem from '@/components/ProductItem.vue'

const router = useRouter()
const phoneStore = usePhoneStore()
const brandStore = useBrandStore()

const products = computed(() => phoneStore.activePhones || [])
const brands = computed(() => brandStore.brands || [])
const selectedBrand = ref(0)

const filteredProducts = computed(() => {
  if (selectedBrand.value === 0) return products.value
  return products.value.filter(p => p.brandId === selectedBrand.value)
})

const displayProducts = computed(() => filteredProducts.value.slice(0, 10))

function selectBrand(brand) {
  selectedBrand.value = brand
}

function goToAll() {
  router.push('/products')
}

async function goToProduct(id, slug) {
  await phoneStore.fetchPhoneById(id)
  router.push(`/product/${slug}`)
}
</script>

<style scoped>
.display-section {
  background: #fff;
  border-radius: 18px;
  box-shadow: 0 4px 16px 0 #e0e0e0;
  padding: 32px 24px;
  margin: 32px auto;
  max-width: 1100px;
}
.display-header {
  display: flex;
  align-items: center;
  margin-bottom: 18px;
  gap: 24px;
}
.display-title {
  font-size: 2rem;
  font-weight: bold;
  color: #ff5722;
  margin-bottom: 0;
  margin-right: 18px;
  text-align: left;
  white-space: nowrap;
}
.filter-group {
  display: flex;
  align-items: center;
  gap: 18px;
  font-size: 16px;
}
.brand-option {
  padding: 6px 18px;
  border-radius: 8px;
  background: #fff;
  cursor: pointer;
  border: 1px solid #eee;
  transition: border 0.2s, background 0.2s, color 0.2s;
  font-weight: 500;
  color: #ff5722;
  display: flex;
  align-items: center;
  justify-content: center;
  min-width: 70px; /* Đảm bảo chiều ngang cố định cho tất cả */
  box-sizing: border-box;
}
.brand-option.active {
  border: 1.5px solid #ff9800;
  color: #e65100;
  box-shadow: 0 2px 8px 0 #ffe0b2;
}
.brand-img-only {
  padding: 0;
  background: #fff;
}
.brand-img {
  width: 100%;
  max-width: 88px; /* Bằng với min-width của .brand-option */
  height: 38px;
  object-fit: contain;
  border-radius: 6px;
  background: #fff;
  border: 1px solid #eee;
  display: block;
}
.product-grid {
  display: grid;
  grid-template-columns: repeat(5, 1fr);
  gap: 18px;
  justify-items: center;
  background: #fff;
}
.view-all-wrapper {
  display: flex;
  justify-content: flex-end;
  margin-top: 18px;
}
.view-all-btn {
  background: linear-gradient(90deg, #ff9800 60%, #ff5722 100%);
  color: #fff;
  font-weight: bold;
  border: none;
  border-radius: 8px;
  padding: 10px 22px;
  font-size: 16px;
  cursor: pointer;
  transition: background 0.2s;
}
.view-all-btn:hover {
  background: linear-gradient(90deg, #e65100 60%, #ff9800 100%);
}
</style>
