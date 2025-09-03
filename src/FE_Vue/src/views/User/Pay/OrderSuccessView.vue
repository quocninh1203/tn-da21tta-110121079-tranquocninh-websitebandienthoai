<template>
  <div class="order-success-container">
    <div class="success-card">
      <!-- Header thành công -->
      <div class="success-header">
        <div class="success-icon">
          <svg-icon type="mdi" :path="mdiCheckCircle" class="check-icon"></svg-icon>
        </div>
        <h1 class="success-title">Đặt hàng thành công!</h1>
        <p class="success-subtitle">
          Cảm ơn bạn đã mua hàng. Đơn hàng của bạn đang được xử lý.
        </p>
      </div>

      <!-- Thông tin đơn hàng -->
      <div class="order-info-section">
        <div class="section-header">
          <svg-icon type="mdi" :path="mdiReceiptText" class="section-icon"></svg-icon>
          <h2>Thông tin đơn hàng</h2>
        </div>
        
        <div class="order-details">
          <div class="detail-row">
            <span class="label">Mã đơn hàng:</span>
            <span class="value order-id">#{{ orderData.orderCode }}</span>
          </div>
          <div class="detail-row">
            <span class="label">Ngày đặt hàng:</span>
            <span class="value">{{ formatDate(orderData.orderDate) }}</span>
          </div>
          <div class="detail-row">
            <span class="label">Trạng thái:</span>
            <span class="value status">{{ getStatusText(orderData.orderStatus) }}</span>
          </div>
          <div class="detail-row">
            <span class="label">Phương thức thanh toán:</span>
            <span class="value">{{ orderData.method }}</span>
          </div>
        </div>
      </div>

      <!-- Thông tin giao hàng -->
      <div class="shipping-info-section">
        <div class="section-header">
          <svg-icon type="mdi" :path="mdiTruckDelivery" class="section-icon"></svg-icon>
          <h2>Thông tin giao hàng</h2>
        </div>
        
        <div class="shipping-details">
          <div class="customer-info">
            <h4>Người nhận</h4>
            <p class="customer-name">{{ orderData.user.fullName }}</p>
            <p class="customer-phone">{{ orderData.user.phoneNumber }}</p>
          </div>
          
          <div class="address-info">
            <h4>Địa chỉ giao hàng</h4>
            <p class="address">{{ orderData.shippingAddress }}</p>
            <p v-if="shippingData.deliveryNote" class="note">
              <strong>Ghi chú:</strong> {{ shippingData.deliveryNote }}
            </p>
          </div>
          
          <div class="shipper-info">
            <h4>Đơn vị vận chuyển</h4>
            <p class="shipper-name">{{ orderData.ship }}</p>
            <p class="shipping-cost">Phí vận chuyển: {{ formatPrice(orderData.cost) }}</p>
          </div>
        </div>
      </div>

      <!-- Danh sách sản phẩm -->
      <div class="products-section">
        <div class="section-header">
          <svg-icon type="mdi" :path="mdiPackageVariant" class="section-icon"></svg-icon>
          <h2>Sản phẩm đã đặt</h2>
        </div>
        
        <div class="products-list">
          <div 
            v-for="item, index in orderData.orderItems" 
            :key="index"
            class="product-item"
          >
            <div class="product-image">
              <img 
                :src="item.phoneImageUrl || item.image || ''" 
                :alt="item.phoneName || item.name"
                class="item-image"
              />
            </div>
            <div class="product-details">
              <h4 class="product-name">{{ item.phoneName || item.name }}</h4>
              <p class="product-specs">{{ item.ram }}/{{ item.storage }} - {{ item.color }}</p>
              <div class="quantity-price">
                <span class="quantity">Số lượng: {{ item.quantity }}</span>
                <span class="unit-price">{{ formatPrice(item.price) }}</span>
              </div>
            </div>
            <div class="item-total">
              {{ formatPrice(item.priceAtOrder) }}
            </div>
          </div>
        </div>
      </div>

      <!-- Tóm tắt thanh toán -->
      <div class="payment-summary">
        <div class="section-header">
          <svg-icon type="mdi" :path="mdiCreditCard" class="section-icon"></svg-icon>
          <h2>Tóm tắt thanh toán</h2>
        </div>
        
        <div class="summary-details">
          <div class="summary-row">
            <span>Tạm tính:</span>
            <span>{{ formatPrice(subtotal) }}</span>
          </div>
          <div class="summary-row">
            <span>Phí vận chuyển:</span>
            <span>{{ formatPrice(orderData.cost) }}</span>
          </div>
          <div class="summary-row total">
            <span>Tổng cộng:</span>
            <span class="total-amount">{{ formatPrice(orderData.totalAmount) }}</span>
          </div>
        </div>
      </div>

      <!-- Action buttons -->
      <div class="action-buttons">
        <button class="btn btn-secondary" @click="goToHome">
          <svg-icon type="mdi" :path="mdiHome"></svg-icon>
          Về trang chủ
        </button>
        <button class="btn btn-primary" @click="viewOrderHistory">
          <svg-icon type="mdi" :path="mdiHistory"></svg-icon>
          Xem đơn hàng của tôi
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import SvgIcon from '@jamescoyle/vue-icon';
import { 
  mdiCheckCircle, 
  mdiReceiptText, 
  mdiTruckDelivery, 
  mdiPackageVariant,
  mdiCreditCard,
  mdiHome,
  mdiHistory
} from '@mdi/js';

// Import stores
import { useOrderStore } from '@/stores/api/orderStore';

const router = useRouter();
// const route = useRoute();
const orderStore = useOrderStore();

const orderData = computed(() => orderStore.orderDetails);
const shippingData = ref({});

// Computed properties
const subtotal = computed(() => {
  return orderData.value.totalAmount - orderData.value.cost;
});

// Format functions
const formatPrice = (price) => {
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price);
};

const formatDate = (dateString) => {
  const date = new Date(dateString);
  return date.toLocaleDateString('vi-VN', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  });
};

const getStatusText = (status) => {
  const statusMap = {
    1: 'Đang chờ xử lý',
    2: 'Đã duyệt',
    3: 'Đã thanh toán',
    4: 'Đang giao hàng',
    0: 'Đã hủy'
  };
  return statusMap[status] || 'Không xác định';
};

// Navigation functions
const goToHome = () => {
  router.push('/');
};

const viewOrderHistory = () => {
  router.push({name: 'orders'});
};

// Load data on mount
onMounted(async () => {
    // const orderId = orderStore.yourOrder;

    // await orderStore.fetchOrderDetail(orderId);
    // orderData.value = orderStore.orderDetails;
    // console.log(orderData.value);
});
</script>

<style scoped>
/* CSS Variables */
:root {
  --success-color: #4caf50;
  --primary-color: #e67e22;
  --secondary-color: #f39c12;
  --text-dark: #2c3e50;
  --text-medium: #34495e;
  --text-light: #7f8c8d;
  --background-soft: #fdf6f0;
  --border-light: #ecf0f1;
  --shadow-soft: rgba(76, 175, 80, 0.15);
}

.order-success-container {
  min-height: 100vh;
  background: linear-gradient(135deg, #f8fff8 0%, #e8f5e8 100%);
  padding: 20px;
  font-family: 'Inter', 'Segoe UI', sans-serif;
}

.success-card {
  max-width: 1000px;
  margin: 0 auto;
  background: white;
  border-radius: 20px;
  box-shadow: 0 20px 60px var(--shadow-soft);
  overflow: hidden;
}

/* Success Header */
.success-header {
  background: linear-gradient(135deg, var(--success-color), #66bb6a);
  color: white;
  text-align: center;
  padding: 40px 20px;
}

.success-icon {
  margin-bottom: 20px;
}

.check-icon {
  width: 80px;
  height: 80px;
  color: white;
}

.success-title {
  margin: 0 0 15px 0;
  font-size: 32px;
  font-weight: 700;
}

.success-subtitle {
  margin: 0;
  font-size: 18px;
  opacity: 0.9;
}

/* Section Styles */
.order-info-section,
.shipping-info-section,
.products-section,
.payment-summary {
  padding: 30px;
  border-bottom: 1px solid var(--border-light);
}

.section-header {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 25px;
}

.section-icon {
  width: 24px;
  height: 24px;
  color: var(--primary-color);
}

.section-header h2 {
  margin: 0;
  font-size: 22px;
  font-weight: 600;
  color: var(--text-dark);
}

/* Order Details */
.order-details {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 15px;
}

.detail-row {
  display: flex;
  justify-content: space-between;
  padding: 12px 0;
  border-bottom: 1px solid #f0f0f0;
}

.label {
  font-weight: 600;
  color: var(--text-medium);
}

.value {
  color: var(--text-dark);
}

.order-id {
  font-family: 'Courier New', monospace;
  font-weight: 700;
  color: var(--primary-color);
}

.status {
  background: var(--success-color);
  color: white;
  padding: 4px 12px;
  border-radius: 20px;
  font-size: 14px;
  font-weight: 600;
}

/* Shipping Details */
.shipping-details {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 25px;
}

.customer-info h4,
.address-info h4,
.shipper-info h4 {
  margin: 0 0 10px 0;
  font-size: 16px;
  font-weight: 600;
  color: var(--text-dark);
}

.customer-name {
  font-size: 18px;
  font-weight: 600;
  color: var(--text-dark);
  margin: 0 0 5px 0;
}

.customer-phone,
.address,
.note,
.shipper-name,
.shipping-cost {
  margin: 5px 0;
  color: var(--text-medium);
  line-height: 1.5;
}

/* Products List */
.products-list {
  space-y: 15px;
}

.product-item {
  display: flex;
  align-items: center;
  padding: 20px;
  background: var(--background-soft);
  border-radius: 12px;
  margin-bottom: 15px;
}

.product-image {
  width: 80px;
  height: 80px;
  border-radius: 10px;
  overflow: hidden;
  margin-right: 20px;
  background: #f0f0f0;
}

.item-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.product-details {
  flex: 1;
}

.product-name {
  margin: 0 0 5px 0;
  font-size: 18px;
  font-weight: 600;
  color: var(--text-dark);
}

.product-specs {
  margin: 0 0 10px 0;
  color: var(--text-light);
  font-size: 14px;
}

.quantity-price {
  display: flex;
  gap: 20px;
}

.quantity,
.unit-price {
  font-size: 14px;
  color: var(--text-medium);
}

.item-total {
  font-size: 18px;
  font-weight: 700;
  color: var(--primary-color);
}

/* Payment Summary */
.summary-details {
  max-width: 400px;
  margin-left: auto;
}

.summary-row {
  display: flex;
  justify-content: space-between;
  padding: 12px 0;
  font-size: 16px;
}

.summary-row.total {
  border-top: 2px solid var(--primary-color);
  padding-top: 15px;
  margin-top: 15px;
  font-size: 20px;
  font-weight: 700;
}

.total-amount {
  color: var(--primary-color);
}

/* Action Buttons */
.action-buttons {
  padding: 30px;
  display: flex;
  gap: 15px;
  justify-content: center;
}

.btn {
  padding: 12px 30px;
  border: none;
  border-radius: 25px;
  font-weight: 600;
  font-size: 16px;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  gap: 8px;
}

.btn-primary {
  background: linear-gradient(135deg, var(--primary-color), var(--secondary-color));
  color: white;
  box-shadow: 0 4px 15px rgba(230, 126, 34, 0.3);
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(230, 126, 34, 0.4);
}

.btn-secondary {
  background: #6c757d;
  color: white;
}

.btn-secondary:hover {
  background: #5a6268;
  transform: translateY(-2px);
}

/* Responsive */
@media (max-width: 768px) {
  .order-success-container {
    padding: 10px;
  }
  
  .success-header {
    padding: 30px 20px;
  }
  
  .success-title {
    font-size: 24px;
  }
  
  .order-info-section,
  .shipping-info-section,
  .products-section,
  .payment-summary {
    padding: 20px;
  }
  
  .order-details,
  .shipping-details {
    grid-template-columns: 1fr;
  }
  
  .product-item {
    flex-direction: column;
    text-align: center;
  }
  
  .product-image {
    margin: 0 0 15px 0;
  }
  
  .action-buttons {
    flex-direction: column;
  }
}
</style>
