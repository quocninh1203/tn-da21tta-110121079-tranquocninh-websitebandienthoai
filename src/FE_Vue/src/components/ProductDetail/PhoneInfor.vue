<template>
  <v-row>
    <v-col cols="12" md="12">
      <v-card class="main-card">
        <v-row>
          <v-col cols="12" md="6">
            <div class="image-carousel-wrapper">
              <v-btn icon class="nav-arrow left" @click="prevImage" :disabled="currentImageIndex === 0">
                <v-icon>mdi-chevron-left</v-icon>
              </v-btn>
              <v-img
                :src="phone.phoneImages[currentImageIndex]"
                height="500"
                class="main-image"
              ></v-img>
              <v-btn icon class="nav-arrow right" @click="nextImage" :disabled="currentImageIndex === phone.phoneImages.length - 1">
                <v-icon>mdi-chevron-right</v-icon>
              </v-btn>
            </div>
            <div class="thumb-list-wrapper">
              <v-btn icon class="thumb-nav left" @click="thumbPrev" :disabled="thumbStart === 0">
                <v-icon small>mdi-chevron-left</v-icon>
              </v-btn>
              <div class="thumb-list">
                <img
                  v-for="(img, idx) in visibleThumbs"
                  :key="idx + thumbStart"
                  :src="img"
                  class="thumb-img"
                  :class="{ active: idx + thumbStart === currentImageIndex }"
                  @click="currentImageIndex = idx + thumbStart"
                  alt="Ảnh nhỏ"
                />
              </div>
              <v-btn icon class="thumb-nav right" @click="thumbNext" :disabled="thumbStart + thumbCount >= phone.phoneImages.length">
                <v-icon small>mdi-chevron-right</v-icon>
              </v-btn>
            </div>
          </v-col>
          <v-col cols="12" md="6">
            <v-card class="sub-card no-border pl-4">
              <v-card-title class="custom-title">{{ phone.phoneName }}</v-card-title>
              <v-card-title>
                Giá: {{ formatVND(selectedVariant?.price) || "" }}
              </v-card-title>
              <v-card-title>
                Thương hiệu: <strong>{{ phone.brandName }}</strong>
              </v-card-title>
              <v-card-text class="font-weight-bold">Chọn màu sắc</v-card-text>
              <div class="color-container">
                <button
                  v-for="color in colorList"
                  :key="color"
                  class="color-circle"
                  :style="{ backgroundColor: mapColor(color) }"
                  :class="{ IsCheck: selectedColor === color }"
                  @click="selectedColor = color"
                >
                  <v-icon v-if="selectedColor === color" class="check-icon-white">mdi-check</v-icon>
                </button>
              </div>
              <v-card-text class="font-weight-bold mt-3">Chọn cấu hình</v-card-text>
              <div class="option-container">
                <button
                  v-for="variant in filteredVariants"
                  :key="variant.phoneVariantId"
                  class="option-item"
                  :class="{ IsCheck: selectedVariant?.phoneVariantId === variant.phoneVariantId }"
                  @click="selectedVariant = variant"
                >
                  <v-text>{{ variant.ramSize }} + {{ variant.storageSize }}</v-text>
                  <span
                    class="check-container"
                    :class="{ IsCheck: selectedVariant?.phoneVariantId === variant.phoneVariantId }"
                  >
                    <v-icon class="check-icon">mdi-check</v-icon>
                  </span>
                </button>
              </div>
              
              <!-- Stock quantity display -->
              <v-card-text v-if="selectedVariant" class="stock-info mt-2 mb-0">
                <span class="font-weight-medium">Còn lại: </span>
                <span class="stock-quantity" :class="{ 'low-stock': selectedVariant.stockQuantity <= minStock }">
                  {{ selectedVariant.stockQuantity }} sản phẩm
                </span>
              </v-card-text>
              
              <v-card-text class="font-weight-bold mt-3">Số lượng</v-card-text>
              <div class="quantity-container">
                <v-btn :disabled="quantity <= 1" icon @click="quantity--" size="30">
                  <v-icon>mdi-minus</v-icon>
                </v-btn>
                <span class="mx-3" style="font-size: 18px">{{ quantity }}</span>
                <v-btn :disabled="quantity >= selectedVariant?.stockQuantity" icon @click="quantity++" size="30">
                  <v-icon>mdi-plus</v-icon>
                </v-btn>
              </div>
              <v-card-actions class="mt-4">
                <v-btn color="white" class="IsCheck py-12 px-16 rounded-lg bg-red" @click="buyNow">
                  <v-card-title class="font-weight-bold">Mua ngay</v-card-title>
                </v-btn>
                <v-btn color="red" class="IsCheck py-12 px-16 rounded-lg" @click="addToCart">
                  <v-icon size="40px">mdi-cart-plus</v-icon>
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-col>
        </v-row>
      </v-card>
    </v-col>
  </v-row>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue';
import { useRouter } from 'vue-router';
import { formatVND } from '@/utils/formatters';
import { useNotificationStore } from '@/stores/local/notification';
import { usePhoneStore } from '@/stores/api/phoneStore';
import { useCartStore } from '@/stores/api/cartStore';
import { useOrderStore } from '@/stores/api/orderStore';
import { getUserId } from '@/utils/jwtHelper';

const router = useRouter();
const notificationStore = useNotificationStore();

const phoneStore = usePhoneStore();
const cartStore = useCartStore();
const orderStore = useOrderStore();
const userId = ref(0);
const minStock = 10;

const phone = computed(() => phoneStore.getSelectedPhone);
const selectedColor = ref(null);
const selectedVariant = ref(null);
const quantity = ref(1);

const currentImageIndex = ref(0);

const prevImage = () => {
  if (currentImageIndex.value > 0) currentImageIndex.value--;
};

const nextImage = () => {
  if (currentImageIndex.value < phone.value.phoneImages.length - 1) currentImageIndex.value++;
};

const colorList = computed(() => {
  const variants = phone.value?.phoneVariants || [];
  return [...new Set(variants.map(v => v.colorName))];
});

const filteredVariants = computed(() => {
  if (!phone.value) return [];
  return phone.value.phoneVariants.filter(v => v.colorName === selectedColor.value);
});

watch(selectedColor, () => {
  const match = filteredVariants.value[0];
  selectedVariant.value = match || null;
});

onMounted(() => {
  if (phone.value) {
    selectedColor.value = phone.value.phoneVariants[0].colorName;
    selectedVariant.value = phone.value.phoneVariants[0];
  }
  userId.value = getUserId();
});

const addToCart = async () => {
  if (!selectedVariant.value || !quantity.value) {
    console.warn("Chưa chọn biến thể hoặc số lượng không hợp lệ.");
    return;
  }
  const userId = getUserId();
  const cartData = {
    userId: userId,
    variantId: selectedVariant.value.phoneVariantId,
    quantity: quantity.value,
  };
  try {
    const res = await cartStore.createCart(cartData, userId);
    if (res && res.success === true) {
      notificationStore.addNotification('Thêm vào giỏ hàng thành công!', 'success');
    } else {
      notificationStore.addNotification('Lỗi khi thêm vào giỏ hàng', 'error');
    }
  } catch (err) {
    console.error('❌ Lỗi khi thêm vào giỏ hàng:', err);
  }
};

const buyNow = async () => {
  const product = [{
    imageUrl: phone.value.phoneImages[0],
    phoneName: phone.value.phoneName,
    ram: selectedVariant.value.ramSize,
    color: selectedColor.value,
    storage: selectedVariant.value.storageSize,
    price: selectedVariant.value.price,
    variantId: selectedVariant.value.phoneVariantId,
    quantity: quantity.value
  }];
  await orderStore.saveProductsToSession(product);
  router.push("/order")
};

const mapColor = (color) => {
  const map = {
    Red: '#f44336',
    Blue: '#2196f3',
    Black: '#212121',
    White: '#fff',
    Pink: '#f48fb1',
    Green: '#4caf50',
    Yellow: '#ffeb3b',
    Gray: '#9e9e9e',
    Purple: '#9c27b0'
  };
  return map[color] || '#ccc';
};

const thumbCount = 5
const thumbStart = ref(0)
const visibleThumbs = computed(() => {
  return phone.value.phoneImages.slice(thumbStart.value, thumbStart.value + thumbCount)
})
function thumbPrev() {
  if (thumbStart.value > 0) thumbStart.value--
}
function thumbNext() {
  if (thumbStart.value + thumbCount < phone.value.phoneImages.length) thumbStart.value++
}
watch(currentImageIndex, (val) => {
  if (val < thumbStart.value) thumbStart.value = val
  else if (val >= thumbStart.value + thumbCount) thumbStart.value = val - thumbCount + 1
})
</script>

<style scoped>
.main-card {
  padding: 20px;
  border-radius: 10px;
  background-color: #ffffff;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}
.image-carousel-wrapper {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
}
.main-image {
  max-width: 100%;
  object-fit: contain;
  border-radius: 10px;
}
.nav-arrow {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  z-index: 2;
}
.nav-arrow.left {
  left: -20px;
}
.nav-arrow.right {
  right: -20px;
}
.sub-card {
  height: 100%;
}
.custom-title {
  font-size: 32px;
  font-weight: bold;
}
.option-container {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  margin-bottom: 10px;
}
.option-item {
  position: relative;
  border: 1px solid #ccc;
  padding: 10px 15px;
  border-radius: 10px;
  background-color: white;
  cursor: pointer;
}
.color-container {
  display: flex;
  gap: 10px;
  margin-bottom: 10px;
  align-items: center;
}
.color-circle {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  border: 2px solid #999;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  position: relative;
}
.color-circle.IsCheck {
  border: 2px solid red;
}
.check-icon-white {
  color: white;
  font-size: 18px;
}
.check-container {
  position: absolute;
  top: 0;
  right: 0;
  display: none;
  align-items: center;
  justify-content: center;
  color: #fff;
  background-color: red;
  border-top-right-radius: 10px;
  border-bottom-left-radius: 15px;
}
.option-item.IsCheck {
  border: red solid 2px;
}
.check-icon {
  font-size: 15px;
  padding: 10px;
}
.IsCheck {
  display: flex !important;
  border: red solid 1px !important;
}
.quantity-container {
  display: flex;
  align-items: center;
  gap: 10px;
}
.thumb-list-wrapper {
  display: flex;
  align-items: center;
  justify-content: center;
  max-width: 500px; /* Giới hạn bằng với chiều rộng ảnh lớn */
  margin: 18px auto 0 auto;
}
.thumb-list {
  display: flex;
  gap: 10px;
  justify-content: center;
  max-width: 400px;
  overflow: hidden;
}
.thumb-img {
  width: 64px;
  height: 64px;
  object-fit: cover;
  border-radius: 8px;
  border: 2px solid #eee;
  cursor: pointer;
  transition: border 0.2s, box-shadow 0.2s;
}
.thumb-img.active {
  border: 2px solid #ff9800;
  box-shadow: 0 2px 8px #ffe0b2;
}
.thumb-nav {
  min-width: 32px;
  height: 32px;
  margin: 0 4px;
  padding: 0;
}
.thumb-nav.left {
  margin-right: 8px;
}
.thumb-nav.right {
  margin-left: 8px;
}
.stock-info {
  padding: 8px 0 !important;
}
.stock-quantity {
  color: #4caf50;
  font-weight: bold;
}
.stock-quantity.low-stock {
  color: #ff5722;
}
</style>
