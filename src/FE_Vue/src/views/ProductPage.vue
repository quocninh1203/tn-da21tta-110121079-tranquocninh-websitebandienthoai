<template>
  <TheMenu />

  <div class="product-page-layout">
    <aside class="filter-left">
      <div class="filter-group">
        <div class="filter-label">Thương hiệu</div>
        <div class="option-list">
          <!-- Remove "Tất cả" option -->
          <div
            v-for="brand in brands"
            :key="brand.id"
            class="option-box"
            :class="{ selected: selectedBrands.includes(brand.id) }"
            @click="toggleBrand(brand.id)"
          >
            {{ brand.name }}
          </div>
        </div>
      </div>
      <!-- Filter by rating -->
      <div class="filter-group">
        <div class="filter-label">Đánh giá</div>
        <div class="option-list">
          <div
            v-for="star in ratingOptions"
            :key="star"
            class="option-box"
            :class="{ selected: selectedRating === star }"
            @click="toggleRating(star)"
          >
            <span class="star-icons">
              <span v-for="i in star" :key="i" class="star">&#9733;</span>
            </span>
            <span class="star-label">{{ star }}+</span>
          </div>
        </div>
      </div>
      <!-- ...existing code for other filters if any... -->
    </aside>
    <main class="content-right">
      <div class="products-header">
        <div class="products-title">Tất cả sản phẩm</div>
        <div class="sort-box">
          <label for="sort-select" class="sort-label">Sắp xếp:</label>
          <select id="sort-select" v-model="sortBy" class="sort-select">
            <option value="newest">Mới nhất</option>
            <option value="price_asc">Giá thấp đến cao</option>
            <option value="price_desc">Giá cao đến thấp</option>
          </select>
        </div>
      </div>
      <div class="products-grid-row">
        <ProductItem
          v-for="item in phones"
          :key="item.phoneId"
          :product="item"
          @click="goToDetail(item.phoneId, item.slug)"
        />
        <div v-if="phones.length === 0" class="no-result">
          <span>Không tìm thấy sản phẩm phù hợp</span>
        </div>
      </div>
    </main>
  </div>

  <Footer />
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { usePhoneStore } from '@/stores/api/phoneStore'
import TheMenu from '@/components/Global/TheMenu.vue';
import ProductItem from '@/components/ProductItem.vue'
import Footer from '@/components/Global/Footer.vue';

const router = useRouter()
const phoneStore = usePhoneStore()
import { useBrandStore } from '@/stores/api/brandStore'
const brandStore = useBrandStore()

const brands = computed(() => brandStore.brands || [])
const selectedBrands = ref([])

const ratingOptions = [5, 4, 3, 2, 1]
const selectedRating = ref(null)

const toggleBrand = (id) => {
  if (selectedBrands.value.includes(id)) {
    selectedBrands.value = selectedBrands.value.filter(bid => bid !== id)
  } else {
    selectedBrands.value.push(id)
  }
}
const toggleRating = (star) => {
  selectedRating.value = selectedRating.value === star ? null : star
}

const sortBy = ref('newest')

const phones = computed(() => {
  let arr = (phoneStore.activePhones || []).slice()
  // Filter by selected brands if any selected
  if (selectedBrands.value.length > 0) {
    arr = arr.filter(phone => selectedBrands.value.includes(phone.brandId))
  }
  // Filter by rating if selected
  if (selectedRating.value) {
    arr = arr.filter(phone => Math.floor(phone.rating) >= selectedRating.value)
  }
  switch (sortBy.value) {
    case 'price_asc':
      arr.sort((a, b) => (a.priceReview ?? 0) - (b.priceReview ?? 0))
      break
    case 'price_desc':
      arr.sort((a, b) => (b.priceReview ?? 0) - (a.priceReview ?? 0))
      break
    case 'newest':
    default:
      arr.reverse()
      break
  }
  return arr
})

const goToDetail = async (id, slug) => {
  await phoneStore.fetchPhoneById(id)
  router.push(`/product/${slug}`)
}

onMounted(async () => {
  await Promise.all([
    phoneStore.fetchActivePhones(),
    brandStore.fetchAllBrands()
  ])
  // selectedBrands.value = [] // No brand selected by default
})
</script>

<style scoped>
.product-page-layout {
  display: flex;
  min-height: 100vh;
  background: linear-gradient(90deg, #f3f6fb 0 240px, #f7f7f7 240px 100%);
}
.filter-left {
  width: 240px;
  min-width: 200px;
  background: #eaf1fb;
  padding: 32px 20px 32px 32px;
  flex-shrink: 0;
  height: auto;
  min-height: 100vh;
}
.content-right {
  flex: 1;
  min-width: 0;
  display: flex;
  flex-direction: column;
  background: #fff;
  min-height: 100vh;
  padding: 32px 32px 32px 0;
}
.products-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
}
.products-title {
  font-size: 1.25rem;
  font-weight: 600;
  color: #222;
}
.sort-box {
  display: flex;
  align-items: center;
  gap: 8px;
}
.sort-label {
  font-size: 1rem;
  color: #444;
}
.sort-select {
  padding: 6px 12px;
  border-radius: 6px;
  border: 1px solid #ddd;
  background: #fff;
  font-size: 1rem;
  min-width: 160px;
}
.products-grid-row {
  display: flex;
  flex-wrap: wrap;
  gap: 18px;
  justify-content: flex-start;
}
.products-grid-row > * {
  flex: 0 0 calc(16.66% - 15px);
  max-width: calc(16.66% - 15px);
  min-width: 0;
  box-sizing: border-box;
}
.no-result {
  width: 100%;
  text-align: center;
  color: #888;
  padding: 40px 0;
}
.filter-group {
  margin-bottom: 32px;
}
.filter-label {
  font-weight: 600;
  margin-bottom: 12px;
}
.option-list {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
}
.option-box {
  border: 1.5px solid #bbb;
  border-radius: 8px;
  padding: 8px 18px;
  background: #fff;
  cursor: pointer;
  font-size: 1rem;
  font-weight: 500;
  transition: border 0.2s, background 0.2s, color 0.2s, box-shadow 0.2s;
  color: #444;
  user-select: none;
  display: flex;
  align-items: center;
  gap: 6px;
}
.star-icons {
  color: #ffc107;
  font-size: 1.1em;
  display: flex;
}
.star-label {
  color: #444;
  font-size: 0.98em;
  margin-left: 2px;
}
.star {
  margin-right: 1px;
}
.option-box.selected {
  border: 2px solid #1976d2;
  background: #e3f0ff;
  color: #1976d2;
  box-shadow: 0 2px 8px #1976d233;
}
@media (max-width: 1200px) {
  .products-grid-row > * {
    flex: 0 0 calc(20% - 15px);
    max-width: calc(20% - 15px);
  }
}
@media (max-width: 900px) {
  .product-page-layout {
    flex-direction: column;
    background: #f3f6fb;
  }
  .filter-left {
    width: 100%;
    min-width: 0;
    height: auto;
    padding: 24px 12px;
    background: #eaf1fb;
  }
  .content-right {
    width: 100%;
    padding: 24px 0 0 0;
    background: #fff;
  }
  .products-grid-row {
    gap: 14px;
    justify-content: center;
  }
  .products-grid-row > * {
    flex-basis: 45%;
    max-width: 100%;
  }
}
@media (max-width: 600px) {
  .products-grid-row {
    flex-direction: column;
    gap: 12px;
  }
  .products-grid-row > * {
    flex-basis: 100%;
    min-width: 0;
    max-width: 100%;
  }
}
</style>