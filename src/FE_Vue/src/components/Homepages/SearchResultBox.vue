<template>
  <div class="search-result-box" v-if="hasInput">
    <template v-if="filteredProducts.length > 0">
      <ul class="search-result-list">
        <li v-for="product in filteredProducts" :key="product.id" class="search-result-item">
          <a @click.prevent="goToProduct(product.slug)" class="item-link">
            <img :src="product.imageUrl || fallbackImage" class="item-image" alt="Ảnh" />
            <div class="item-info">
              <div class="item-name">{{ product.name }}</div>
              <div class="item-spec">Màn hình: {{ product.screen || 'Đang cập nhật' }}</div>
            </div>
          </a>
        </li>
      </ul>
    </template>
    <template v-else>
      <div class="empty-message">Không tìm thấy sản phẩm phù hợp.</div>
    </template>
  </div>
</template>

<script setup>
import { computed, toRef } from 'vue'
import { useRouter } from 'vue-router'
import { usePhoneStore } from '@/stores/api/phoneStore'

const props = defineProps({
  keyword: String
})

const router = useRouter()
const phoneStore = usePhoneStore()
const keyword = toRef(props, 'keyword')

const hasInput = computed(() => keyword.value.trim().length > 0)

const fallbackImage = '/images/placeholder-phone.jpg'

const filteredProducts = computed(() => {
  const search = keyword.value.trim().toLowerCase()
  if (!search) return []
  return phoneStore.activePhones.filter(p =>
    p.name.toLowerCase().includes(search)
  )
})

const goToProduct = (slug) => {
  router.push(`/product/${slug}`)
}
</script>

<style scoped>
.search-result-box {
  position: absolute;
  top: 140px;
  left: 0;
  right: 0;
  margin: 0 auto;
  background: #fff;
  border-radius: 10px;
  box-shadow: 0 4px 16px 0 #ffccbc;
  max-width: 700px;
  padding: 18px 24px;
  font-size: 16px;
  z-index: 101;
}

.search-result-list {
  list-style: none;
  margin: 0;
  padding: 0;
}

.search-result-item {
  display: flex;
  align-items: center;
  padding: 10px 0;
  border-bottom: 1px solid #ffe0b2;
}

.search-result-item:last-child {
  border-bottom: none;
}

.item-link {
  display: flex;
  align-items: center;
  text-decoration: none;
  color: inherit;
  width: 100%;
}
.item-link:hover {
  opacity: 0.6;
}

.item-image {
  width: 60px;
  height: 60px;
  object-fit: cover;
  border-radius: 8px;
  margin-right: 16px;
  border: 1px solid #eee;
}

.item-info {
  display: flex;
  flex-direction: column;
}

.item-name {
  font-weight: 600;
  color: #e65100;
  margin-bottom: 4px;
}

.item-spec {
  font-size: 14px;
  color: #757575;
}

.empty-message {
  text-align: center;
  color: #e65100;
  font-style: italic;
  padding: 12px 0;
}
</style>
