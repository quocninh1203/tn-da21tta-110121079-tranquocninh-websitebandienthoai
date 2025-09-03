<template>
  <div class="product-detail-admin" v-if="product">
    <div class="header-row">
      <h2 class="section-title">{{ product.phoneName }}</h2>
      <button class="btn back-btn" @click="goBack">
        <v-icon style="margin-left: 6px;">mdi-arrow-left</v-icon>
        Quay lại
      </button>
    </div>

    <!-- Thông tin sản phẩm -->
    <section class="info-section">
      <div><strong>Hãng:</strong> {{ product.brandName }}</div>
      <div><strong>Màn hình:</strong> {{ product.screen }}</div>
      <div><strong>Chip:</strong> {{ product.chip }}</div>
      <div><strong>Pin:</strong> {{ product.battery }}</div>
      <div><strong>Mô tả:</strong> {{ product.description }}</div>
    </section>

    <!-- Ảnh sản phẩm -->
    <section class="photo-section">
      <div class="section-header">
        <h3>Ảnh sản phẩm</h3>
        <button class="btn blue" @click="addImage">
           Thêm hình ảnh
        </button>
      </div>
      <div class="photo-list">
        <div v-for="(url, idx) in product.phoneImages" :key="idx" class="photo-item">
          <img :src="url" alt="Ảnh" />
          <div class="actions">
            <button class="btnicon orange" @click="editImage(url)">
              <v-icon>mdi-pencil</v-icon>
            </button>
            <button class="btnicon red" @click="deleteImage(url)">
              <v-icon>mdi-delete</v-icon>
            </button>
          </div>
        </div>
      </div>
      <!-- Sử dụng FormImage.vue -->
      <FormImage v-if="showFormImage" @submit="handleImageSubmit" @close="showFormImage = false" />
    </section>

    <!-- Biến thể -->
    <section class="variant-section">
      <div class="section-header">
        <h3>Biến thể</h3>
        <button class="btn blue" @click="addVariant">
           Thêm biến thể 
        </button>
      </div>
      <table class="variant-table">
        <thead>
          <tr>
            <th>#</th>
            <th>Màu</th>
            <th>RAM</th>
            <th>ROM</th>
            <th>Giá</th>
            <th>Số lượng</th>
            <th>Thao tác</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(variant, idx) in product.phoneVariants" :key="variant.phoneVariantId">
            <td>{{ idx + 1 }}</td>
            <td>{{ variant.colorName }}</td>
            <td>{{ variant.ramSize }}</td>
            <td>{{ variant.storageSize }}</td>
            <td>{{ formatPrice(variant.price) }}</td>
            <td :class="{ 'text-red': variant.stockQuantity < 0 }">
              {{ variant.stockQuantity }}
            </td>
            <td class="actions">
              <button class="btnicon orange" @click="editVariant(variant)" title="Sửa biến thể">
                <v-icon>mdi-pencil</v-icon>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
      <!-- Hiển thị FormVariant khi cần -->
      <FormVariant
        v-if="showFormVariant"
        :isEdit="isEditVariant"
        :variant="selectedVariant"
        :phoneId="phoneId"
        :colors="colors"
        :rams="rams"
        :storages="storages"
        @submit="handleVariantSubmit"
        @close="closeVariantForm"
      />
    </section>

    <!-- Đánh giá -->
    <section class="review-section">
      <h3>Đánh giá</h3>
      <table class="review-table">
        <thead>
          <tr>
            <th>#</th>
            <th>Khách hàng</th>
            <th>Rating</th>
            <th>Bình luận</th>
            <th>Ảnh</th>
            <th>Biến thể</th>
            <th>Ngày</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(review, idx) in product.reviews" :key="idx">
            <td>{{ idx + 1 }}</td>
            <td>
              <img :src="review.userImageUrl" class="avatar" />
              {{ review.fullName }}
            </td>
            <td>{{ review.rating }} ⭐</td>
            <td>{{ review.text }}</td>
            <td v-if="review.reviewImageUrl">
              <img :src="review.reviewImageUrl" class="review-img" />
            </td>
            <td v-else></td>
            <td>{{ review.color }} / {{ review.ram }} / {{ review.storage }}</td>
            <td>{{ new Date(review.reviewDate).toLocaleDateString() }}</td>
          </tr>
        </tbody>
      </table>
    </section>
  </div>
</template>

<script setup>
import { usePhoneStore } from '@/stores/api/phoneStore'
import { useNotificationStore } from '@/stores/local/notification'
import { useColorStore } from '@/stores/api/colorStore'
import { useRamStore } from '@/stores/api/ramStore'
import { useStorageStore } from '@/stores/api/storageStore'
import { ref, onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import FormImage from '@/components/Admins/Products/FormImage.vue'
import FormVariant from '@/components/Admins/Products/FormVariant.vue'

const phoneStore = usePhoneStore()
const router = useRouter()
const notificationStore = useNotificationStore()
const colorStore = useColorStore()
const ramStore = useRamStore()
const storageStore = useStorageStore()

const phoneId = ref(0)

onMounted(async () => {
  phoneId.value = product.value.phoneId
  await phoneStore.fetchPhoneById(phoneId.value)
})

const product = computed(() => phoneStore.getSelectedPhone)
const rams = computed(() => ramStore.rams)
const storages = computed(() => storageStore.storages)
const colors = computed(() => colorStore.colors)

const formatPrice = (val) =>
  val?.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' })

const showFormImage = ref(false)
const showFormVariant = ref(false)
const isEditVariant = ref(false)
const selectedVariant = ref(null)

const addImage = () => {
  showFormImage.value = true
}

const addVariant = async () => {
  await loadConfig()
  isEditVariant.value = false
  selectedVariant.value = null
  showFormVariant.value = true
}

const editVariant = async (variant) => {
  await loadConfig()
  isEditVariant.value = true
  selectedVariant.value = variant
  showFormVariant.value = true
}

function closeVariantForm() {
  showFormVariant.value = false
}

async function reload() {
  await phoneStore.fetchPhoneById(phoneId.value)
}

async function loadConfig(){
  await colorStore.fetchAllColors()
  await ramStore.fetchAllRams()
  await storageStore.fetchAllStorages()
}

async function handleImageSubmit({ imageBase64 }) {
  try {
    const res = await phoneStore.createPhoneImage({
      phoneId: phoneId.value,
      imageBase64: imageBase64
    })

    if (res && res.success) {
      notificationStore.addNotification('Thêm ảnh sản phẩm thành công!', 'success')
    } else {
      notificationStore.addNotification(res?.message || 'Thêm ảnh sản phẩm thất bại!', 'error')
    }

    await reload()
    showFormImage.value = false
  } catch (e) {
    // handle error nếu cần
  }
}

async function handleVariantSubmit(variantData) {
  if (isEditVariant.value && selectedVariant.value?.phoneVariantId) {
    const res = await phoneStore.updatePhoneVariant(selectedVariant.value.phoneVariantId, variantData)

    if (res && res.success) {
      notificationStore.addNotification('Cập nhật biến thể sản phẩm thành công!', 'success')
    } else {
      notificationStore.addNotification(res?.message || 'Cập nhật biến thể sản phẩm thất bại!', 'error')
    }
  } else {
    const res = await phoneStore.createPhoneVariant(variantData)

    if (res && res.success) {
      notificationStore.addNotification('Thêm biến thể sản phẩm thành công!', 'success')
    } else {
      notificationStore.addNotification(res?.message || 'Thêm biến thể sản phẩm thất bại!', 'error')
    }
  }
  showFormVariant.value = false
  await reload()
}

const editImage = (url) => {}
const deleteImage = (url) => {}

function goBack() {
  router.back()
}
</script>

<style scoped>
.product-detail-admin {
  padding: 24px;
  background: #fff;
}
.header-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
}
.section-title {
  position: relative;
  padding-left: 16px; /* để tạo chỗ cho đường trước */
  font-size: 26px;
  color: #e65100;
  margin-bottom: 16px;
  display: flex;
  align-items: center;
}

.section-title::before {
  content: "";
  position: absolute;
  left: 0;
  top: 50%;
  transform: translateY(-50%);
  width: 5px;
  height: 40px;
  background-color: #e65100;
}

.info-section div {
  margin-bottom: 8px;
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin: 24px 0 12px;
}

.btnicon {
  border: none;
  border-radius: 4px;
  color: white;
  font-weight: bold;
  cursor: pointer;
  font-size: 14px;
  padding: 6px 10px;
}

.btn {
  border: none;
  border-radius: 4px;
  color: white;
  font-weight: bold;
  cursor: pointer;
  font-size: 16px;
  padding: 10px 20px;
}

.btnicon.green {
  background-color: #388e3c;
}

.btn.blue {
  background-color: #035ae5;
}

.btnicon.orange {
  background-color: #fb8c00;
}

.btnicon.red {
  background-color: #e53935;
}

.photo-list {
  display: flex;
  gap: 16px;
  flex-wrap: wrap;
}

.photo-item {
  width: 120px;
  position: relative;
  text-align: center;
}

.photo-item img {
  width: 100%;
  height: 100px;
  object-fit: cover;
  border-radius: 6px;
  box-shadow: 0 0 4px rgba(0, 0, 0, 0.1);
}

.variant-table .actions,
.photo-item .actions {
  margin-top: 4px;
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
  gap: 6px;
}

.variant-table th,
.variant-table td,
.review-table th,
.review-table td {
  padding: 14px 16px;
  text-align: left;
  border-bottom: 1px solid #f2f2f2;
  vertical-align: middle;
}

.variant-table th{
  background-color: #5f3864;
  color: white;
  font-weight: bold;
}

.review-table th {
  background-color: #87a2dc;
  color: white;
  font-weight: bold;
}

.variant-table,
.review-table {
  width: 100%;
  border-collapse: collapse;
  background: #fafafa;
  margin-top: 16px;
}

.avatar {
  width: 28px;
  height: 28px;
  border-radius: 50%;
  object-fit: cover;
  margin-right: 6px;
  vertical-align: middle;
}

.review-img {
  width: 60px;
  height: 60px;
  object-fit: cover;
}

.text-red {
  color: #d32f2f;
  font-weight: bold;
}

.btn.back-btn {
  background: #ff9800;
  color: #fff;
  font-weight: bold;
  border: none;
  border-radius: 6px;
  padding: 8px 22px;
  font-size: 15px;
  cursor: pointer;
  transition: background 0.2s;
  margin-bottom: 10px;
  display: flex;
  align-items: center;
  gap: 4px;
}

.btn.back-btn:hover {
  background: #e65100;
  display: flex;
  align-items: center;
  gap: 4px;
}

.btn.back-btn:hover {
  background: #e65100;
}
</style>
