<template>
  <div class="tab-pane">
    <h3>Giỏ hàng của bạn</h3>
    
    <!-- Hiển thị khi có sản phẩm -->
    <div v-if="cartItems.length > 0">
      <div class="cart-items-container">
        <div class="cart-item" v-for="item in cartItems" :key="item.cartId || item.id">
          <div class="item-image">
            <img
              :src="item.imageUrl || item.image || ''"
              alt="sản phẩm"
              class="product-image"
            />
          </div>
          <div class="item-details">
            <h4 class="item-name">{{ item.phoneName || item.name }}</h4>
            <p class="item-description">{{ item.ram }}/{{ item.storage }} - {{ item.color }}</p>
            <div class="quantity-controls">
              <button class="qty-btn" @click="decreaseQuantity(item)">-</button>
              <span class="quantity">{{ item.quantity }}</span>
              <button class="qty-btn" @click="increaseQuantity(item)">+</button>
            </div>
          </div>
          <div class="item-price">
            <span class="price">{{ formatPrice(item.price * item.quantity) }}</span>
            <button class="remove-btn" @click="removeItem(item.cartId || item.variantId)">
              <v-icon>mdi-delete</v-icon>
            </button>
          </div>
        </div>
      </div>

      <div class="cart-summary"> 
        <div class="summary-row total">
          <span>Tổng cộng:</span>
          <span class="total-amount">{{ formatPrice(total) }}</span>
        </div>
      </div>
    </div>

    <!-- Hiển thị khi không có sản phẩm -->
    <div v-else class="empty-cart">
      <div class="empty-cart-icon">
        <v-icon class="cart-icon">mdi-cart-outline</v-icon>
      </div>
      <h4 class="empty-cart-title">Đơn hàng trống</h4>
      <p class="empty-cart-message">
        Không có sản phẩm nào trong đơn hàng của bạn. 
        Hãy quay lại trang chủ để chọn sản phẩm yêu thích.
      </p>
      <button class="btn btn-secondary" @click="goBackToHome">
        <v-icon>mdi-arrow-left</v-icon>
        Quay lại mua sắm
      </button>
    </div>

    <div class="navigation-buttons">
      <div></div>
      <button 
        class="btn btn-primary" 
        @click="handleNext"
        :disabled="!canGoNext || cartItems.length === 0"
      >
        Tiếp tục
        <v-icon>mdi-arrow-right</v-icon>
      </button>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import { useOrderStore } from '@/stores/api/orderStore';
import { useCartStore } from '@/stores/api/cartStore';
import { getUserId } from '@/utils/jwtHelper';
import { useRouter } from 'vue-router';

const props = defineProps({
  canGoNext: {
    type: Boolean,
    default: true
  },
  canGoPrevious: {
    type: Boolean,
    default: true
  }
});

const emit = defineEmits(['next']);
const orderStore = useOrderStore();
const cartStore = useCartStore();
const router = useRouter();

const cartItems = ref([]);
const shippingFee = ref(50000);

const subtotal = computed(() => {
  return cartItems.value.reduce((sum, item) => sum + (item.price * item.quantity), 0);
});

const total = computed(() => subtotal.value + shippingFee.value);

const formatPrice = (price) => {
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price);
};

const increaseQuantity = (item) => {
  item.quantity++;
  orderStore.saveProductsToSession(cartItems.value); 
};

const decreaseQuantity = (item) => {
  if (item.quantity > 1) {
    item.quantity--;
    orderStore.saveProductsToSession(cartItems.value);
  }
};

const removeItem = async (itemId) => {
  const updatedProducts = await orderStore.removeProductFromSession(itemId);

  cartItems.value = updatedProducts;
};

const handleNext = () => {
  emit('next');
}

const goBackToHome = () => {
  router.push('/cart');
};

onMounted(async () => {
  const savedProducts = await orderStore.getProductsFromSession();
  cartItems.value = savedProducts;
});
</script>

<style scoped>
.cart-items-container {
  margin: 25px 0;
}

.cart-item {
  display: flex;
  align-items: center;
  padding: 20px;
  margin-bottom: 12px;
  background: #fdfbf7;
  border-radius: 10px;
  border: 1px solid #f0ebe1;
  transition: all 0.3s ease;
}

.cart-item:hover {
  box-shadow: 0 3px 12px rgba(230, 126, 34, 0.08);
  transform: translateY(-1px);
  border-color: #e8dcc6;
}

.item-image {
  width: 56px;
  height: 56px;
  border-radius: 10px;
  overflow: hidden;
  margin-right: 18px;
  background: linear-gradient(135deg, #e67e22, #f39c12);
  display: flex;
  align-items: center;
  justify-content: center;
}

.product-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  object-position: center;
}

.item-details {
  flex: 1;
}

.item-name {
  margin: 0 0 4px 0;
  font-size: 17px;
  font-weight: 600;
  color: #2c3e50;
}

.item-description {
  margin: 0 0 12px 0;
  color: #7f8c8d;
  font-size: 14px;
}

.quantity-controls {
  display: flex;
  align-items: center;
  gap: 12px;
}

.qty-btn {
  width: 30px;
  height: 30px;
  border: 2px solid #e67e22;
  background: white;
  color: #e67e22;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.3s ease;
  font-size: 14px;
}

.qty-btn:hover {
  background: #e67e22;
  color: white;
  transform: scale(1.05);
}

.quantity {
  font-weight: 600;
  min-width: 25px;
  text-align: center;
  color: #2c3e50;
}

.item-price {
  text-align: right;
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  gap: 8px;
}

.price {
  font-size: 17px;
  font-weight: 700;
  color: #e67e22;
}

.remove-btn {
  background: #e74c3c;
  border: none;
  color: white;
  padding: 6px;
  border-radius: 6px;
  cursor: pointer;
  transition: all 0.3s ease;
}

.remove-btn:hover {
  background: #c0392b;
  transform: scale(1.05);
}

.cart-summary {
  background: linear-gradient(135deg, #fdfbf7, #f8f1e8);
  padding: 22px;
  border-radius: 10px;
  margin-top: 25px;
  border: 1px solid #f0ebe1;
}

.summary-row {
  display: flex;
  justify-content: space-between;
  margin-bottom: 12px;
  font-size: 15px;
  color: #2c3e50;
}

.summary-row.total {
  border-top: 2px solid #e67e22;
  padding-top: 12px;
  margin-top: 12px;
  font-size: 18px;
  font-weight: 700;
}

.total-amount {
  color: #e67e22;
}

/* Styles cho empty cart */
.empty-cart {
  text-align: center;
  padding: 60px 20px;
  background: linear-gradient(135deg, #fdfbf7, #f8f1e8);
  border-radius: 12px;
  border: 1px solid #f0ebe1;
  margin: 25px 0;
}

.empty-cart-icon {
  margin-bottom: 20px;
}

.cart-icon {
  width: 80px;
  height: 80px;
  color: #bdc3c7;
}

.empty-cart-title {
  margin: 0 0 15px 0;
  font-size: 24px;
  font-weight: 600;
  color: #2c3e50;
}

.empty-cart-message {
  margin: 0 0 30px 0;
  color: #7f8c8d;
  font-size: 16px;
  line-height: 1.5;
  max-width: 400px;
  margin-left: auto;
  margin-right: auto;
}

/* Navigation buttons */
.navigation-buttons {
  display: flex;
  justify-content: space-between;
  margin-top: 35px;
  padding-top: 25px;
  border-top: 1px solid var(--border-light, #ecf0f1);
}

.btn {
  padding: 12px 28px;
  border: none;
  border-radius: 8px;
  font-weight: 600;
  font-size: 15px;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  gap: 8px;
  border: 2px solid transparent;
}

.btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
  transform: none !important;
}

.btn-primary {
  background: linear-gradient(135deg, #e67e22, #f39c12);
  color: white;
  box-shadow: 0 3px 12px rgba(230, 126, 34, 0.25);
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 5px 16px rgba(230, 126, 34, 0.35);
}

.btn-secondary {
  background: white;
  color: #7f8c8d;
  border-color: #bdc3c7;
}

.btn-secondary:hover {
  background: #f8f9fa;
  color: #2c3e50;
  transform: translateY(-1px);
}
</style>
