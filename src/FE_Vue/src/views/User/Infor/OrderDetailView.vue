<template>
  <div class="order-detail-container">
    <!-- Header -->
    <div class="header-section">
      <button class="btn-back" @click="goBack">
        <svg-icon type="mdi" :path="mdiArrowLeft"></svg-icon>
        Quay lại
      </button>
      <h1 class="page-title">
        Chi tiết đơn hàng #{{ orderDetails.orderCode }}
      </h1>
    </div>

    <!-- Loading state -->
    <div v-if="isLoading" class="loading-container">
      <div class="loading-spinner"></div>
      <p>Đang tải chi tiết đơn hàng...</p>
    </div>

    <!-- Empty state -->
    <div v-else-if="orderDetails.length === 0" class="empty-state">
      <div class="empty-icon">
        <svg-icon type="mdi" :path="mdiPackageVariantClosed" class="empty-icon-svg"></svg-icon>
      </div>
      <h3>Không có chi tiết đơn hàng</h3>
      <p>Không tìm thấy thông tin chi tiết cho đơn hàng này.</p>
    </div>

    <!-- Order details -->
    <div v-else class="order-content">
      <!-- Order summary -->
      <div class="order-summary-card">
        <div class="summary-header">
          <svg-icon type="mdi" :path="mdiReceiptText" class="summary-icon"></svg-icon>
          <h2>Thông tin đơn hàng</h2>
        </div>
        <div class="summary-content">
          <div class="summary-row">
            <span class="label">Trạng thái:</span>
            <span class="value status" :class="getStatusClass(orderDetails.orderStatus)">
              {{ getStatusText(orderDetails.orderStatus) }}
            </span>
          </div>
          <div class="summary-row">
            <span class="label">Ngày đặt:</span>
            <span class="value">{{ formatDate(orderDetails.orderDate) }}</span>
          </div>
          <div class="summary-row">
            <span class="label">Phương thức thanh toán:</span>
            <span class="value">{{ orderDetails.method }}</span>
          </div>
          <!-- THÊM PHƯƠNG THỨC VẬN CHUYỂN -->
          <div class="summary-row">
            <span class="label">Phương thức vận chuyển:</span>
            <span class="value">{{ orderDetails.ship || 'Giao hàng tiêu chuẩn' }}</span>
          </div>
          <!-- THÊM PHÍ VẬN CHUYỂN -->
          <div class="summary-row">
            <span class="label">Phí vận chuyển:</span>
            <span class="value">{{ formatPrice(orderDetails.cost || 0) }}</span>
          </div>
        </div>
      </div>

      <!-- Products list -->
      <div class="products-section">
        <div class="section-header">
          <svg-icon type="mdi" :path="mdiPackageVariant" class="section-icon"></svg-icon>
          <h2>Sản phẩm đã đặt</h2>
        </div>
        
        <div class="products-list">
          <div 
            v-for="detail, index in orderDetails.orderItems" 
            :key="index" 
            class="product-card"
          >
            <div class="product-image-container">
              <img 
                :src="detail.phoneImageUrl || detail.image || ''" 
                :alt="detail.productName || detail.name"
                class="product-image"
                @error="handleImageError"
              />
            </div>
            
            <div class="product-details">
              <h4 class="product-name">{{ detail.phoneName || detail.name }}</h4>
              <p class="product-specs" v-if="detail.ram && detail.storage">
                {{ detail.ram }}/{{ detail.storage }} - {{ detail.color }}
              </p>
              
              <div class="product-pricing">
                <div class="pricing-row">
                  <span class="pricing-label">Đơn giá:</span>
                  <span class="pricing-value">{{ formatPrice(detail.price) }}</span>
                </div>
                <div class="pricing-row">
                  <span class="pricing-label">Số lượng:</span>
                  <span class="pricing-value">{{ detail.quantity }}</span>
                </div>
                <div class="pricing-row total">
                  <span class="pricing-label">Tổng:</span>
                  <span class="pricing-value total-price">{{ formatPrice(detail.priceAtOrder) }}</span>
                </div>
              </div>
            </div>
            
            <div v-if="orderDetails.orderStatus === 4" class="product-actions">
              <button 
                v-if="detail.isReview === false"
                class="btn btn-review" 
                @click="addReview(detail)"
              >
                <svg-icon type="mdi" :path="mdiStar" class="btn-icon"></svg-icon>
                Đánh giá
              </button>
              <span 
                v-else
                class="reviewed-label"
                style="color: #fff; font-weight: 600; padding: 10px 0;"
              >
                <v-icon class="btn-icon">mdi-check</v-icon>
                Đã đánh giá
              </span>
              <button 
                class="btn btn-reorder" 
                @click="reorderProduct(detail.productId || detail.id)"
              >
                <svg-icon type="mdi" :path="mdiRefresh" class="btn-icon"></svg-icon>
                Mua lại
              </button>
            </div>

          </div>
        </div>
      </div>

      <!-- Order total -->
      <div class="order-total-card">
        <div class="total-header">
          <svg-icon type="mdi" :path="mdiCalculator" class="total-icon"></svg-icon>
          <h3>Tổng cộng đơn hàng</h3>
        </div>
        <div class="total-amount">
          {{ formatPrice(orderDetails.totalAmount) }}
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import SvgIcon from '@jamescoyle/vue-icon';
import { 
  mdiArrowLeft,
  mdiReceiptText,
  mdiPackageVariant,
  mdiPackageVariantClosed,
  mdiStar,
  mdiRefresh,
  mdiCalculator
} from '@mdi/js';

// Import stores
import { useOrderStore } from '@/stores/api/orderStore';
import { usePhoneStore } from '@/stores/api/phoneStore';
import { useReviewStore } from '@/stores/api/reviewStore';
import { getUserId } from '@/utils/jwtHelper';

const route = useRoute();
const router = useRouter();
const orderStore = useOrderStore();
const phoneStore = usePhoneStore();
const reviewStore = useReviewStore();

const orderId = route.params.orderId;
const orderDetails = ref([]);
const isLoading = ref(false);

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
    4: 'Đã giao hàng',
    0: 'Đã hủy'
  };
  return statusMap[status] || 'Không xác định';
};

const getStatusClass = (status) => `status-${status}`;

// Actions
const fetchOrderDetails = async () => {
  try {
    isLoading.value = true;
    if (orderId) {
      await orderStore.fetchOrderDetail(orderId);
      orderDetails.value = orderStore.orderDetails || [];
    }
  } catch (error) {
    console.error('Lỗi khi tải chi tiết đơn hàng:', error);
  } finally {
    isLoading.value = false;
  }
};

const addReview = async (detail) => {
  const data = {
    userId: getUserId(),
    phoneId: detail.phoneId,
    variantId: detail.variantId,
    orderDetailId: detail.orderDetailId
  }

  await phoneStore.fetchPhoneById(detail.phoneId)

  reviewStore.setReviewProduct(data)

  const slug = phoneStore.selectedPhone.slug
  router.push(`/product/${slug}`)
};

const reorderProduct = (productId) => {
  console.log('Mua lại sản phẩm:', productId);
};

const handleImageError = (event) => {
  event.target.style.display = 'none';
};

const goBack = () => {
  router.go(-1);
};

onMounted(() => {
  fetchOrderDetails();
});
</script>

<!-- GIỮ NGUYÊN TOÀN BỘ CSS CŨ -->
<style scoped>
/* CSS Variables */
:root {
  --primary-color: #e67e22;
  --secondary-color: #f39c12;
  --success-color: #27ae60;
  --warning-color: #f39c12;
  --danger-color: #e74c3c;
  --text-dark: #2c3e50;
  --text-medium: #34495e;
  --text-light: #7f8c8d;
  --background-soft: #fdf6f0;
  --border-light: #ecf0f1;
  --shadow-soft: rgba(230, 126, 34, 0.15);
}

.order-detail-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
  font-family: 'Inter', 'Segoe UI', sans-serif;
  overflow-y: auto;
}

/* Header */
.header-section {
  display: flex;
  align-items: center;
  gap: 20px;
  margin-bottom: 30px;
  padding-bottom: 20px;
  border-bottom: 1px solid var(--border-light);
}

.btn-back {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 10px 16px;
  background: white;
  border: 2px solid var(--border-light);
  border-radius: 8px;
  color: var(--text-medium);
  cursor: pointer;
  transition: all 0.3s ease;
}

.btn-back:hover {
  border-color: var(--primary-color);
  color: var(--primary-color);
}

.page-title {
  margin: 0;
  font-size: 28px;
  font-weight: 700;
  color: var(--text-dark);
}

/* Loading */
.loading-container {
  text-align: center;
  padding: 60px 20px;
}

.loading-spinner {
  width: 40px;
  height: 40px;
  border: 4px solid var(--border-light);
  border-top: 4px solid var(--primary-color);
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin: 0 auto 20px;
}

/* Empty state */
.empty-state {
  text-align: center;
  padding: 80px 20px;
  background: var(--background-soft);
  border-radius: 16px;
  border: 1px solid var(--border-light);
}

.empty-icon-svg {
  width: 80px;
  height: 80px;
  color: var(--text-light);
}

/* Order content */
.order-content {
  display: flex;
  flex-direction: column;
  gap: 30px;
  max-height: 325px;
  padding-right: 8px;
  overflow-y: auto;
}
.order-content::-webkit-scrollbar {
  width: 8px;
}
.order-content::-webkit-scrollbar-thumb {
  background-color: var(--warning-color);
  border-radius: 4px;
}
.order-content::-webkit-scrollbar-thumb:hover {
  background-color: var(--danger-color);
}
.order-content::-webkit-scrollbar-track {
  background: var(--shadow-soft);      
}

/* Order summary card */
.order-summary-card {
  background: white;
  border-radius: 16px;
  border: 1px solid var(--border-light);
  box-shadow: 0 4px 20px var(--shadow-soft);
}

.summary-header {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 20px;
  background: linear-gradient(135deg, var(--background-soft), #f8f1e8);
  border-bottom: 1px solid var(--border-light);
}

.summary-icon {
  width: 24px;
  height: 24px;
  color: var(--primary-color);
}

.summary-header h2 {
  margin: 0;
  font-size: 20px;
  font-weight: 600;
  color: var(--text-dark);
}

.summary-content {
  padding: 20px;
}

.summary-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 12px 0;
  border-bottom: 1px solid #f0f0f0;
}

.summary-row:last-child {
  border-bottom: none;
}

.label {
  font-weight: 600;
  color: var(--text-medium);
}

.value {
  color: var(--text-dark);
}

.status {
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 12px;
  font-weight: 600;
  text-transform: uppercase;
}

/* Products section */
.products-section {
  background: white;
  border-radius: 16px;
  border: 1px solid var(--border-light);
  box-shadow: 0 4px 20px var(--shadow-soft);
}

.section-header {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 20px;
  background: linear-gradient(135deg, var(--background-soft), #f8f1e8);
  border-bottom: 1px solid var(--border-light);
}

.section-icon {
  width: 24px;
  height: 24px;
  color: var(--primary-color);
}

.section-header h2 {
  margin: 0;
  font-size: 20px;
  font-weight: 600;
  color: var(--text-dark);
}

.products-list {
  padding: 20px;
  display: flex;
  flex-direction: column;
  gap: 20px;
}

/* Product card */
.product-card {
  display: flex;
  align-items: center;
  gap: 20px;
  padding: 20px;
  background: var(--background-soft);
  border-radius: 12px;
  border: 1px solid var(--border-light);
  transition: all 0.3s ease;
}

.product-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 15px var(--shadow-soft);
}

.product-image-container {
  width: 100px;
  height: 100px;
  border-radius: 12px;
  overflow: hidden;
  background: #f0f0f0;
  flex-shrink: 0;
}

.product-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.product-details {
  flex: 1;
}

.product-name {
  margin: 0 0 8px 0;
  font-size: 18px;
  font-weight: 600;
  color: var(--text-dark);
}

.product-specs {
  margin: 0 0 15px 0;
  color: var(--text-light);
  font-size: 14px;
}

.product-pricing {
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.pricing-row {
  display: flex;
  justify-content: space-between;
  font-size: 14px;
}

.pricing-row.total {
  border-top: 1px solid var(--border-light);
  padding-top: 8px;
  margin-top: 8px;
  font-weight: 600;
}

.pricing-label {
  color: var(--text-medium);
}

.pricing-value {
  color: var(--text-dark);
}

.total-price {
  color: var(--primary-color);
  font-weight: 700;
}

/* Product actions */
.product-actions {
  display: flex;
  flex-direction: column;
  gap: 10px;
  flex-shrink: 0;
}

.btn {
  padding: 10px 16px;
  border: none;
  border-radius: 8px;
  font-weight: 600;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  gap: 6px;
  justify-content: center;
  min-width: 120px;
}

.btn-review {
  background: linear-gradient(135deg, var(--warning-color), #f1c40f);
  color: white;
}

.btn-review:disabled {
  background: #95a5a6;
  cursor: not-allowed;
}

.btn-reorder {
  background: white;
  color: var(--primary-color);
  border: 2px solid var(--primary-color);
}

.btn:hover:not(:disabled) {
  transform: translateY(-2px);
}

.btn-icon {
  width: 16px;
  height: 16px;
}

/* Order total card */
.order-total-card {
  background: linear-gradient(135deg, var(--primary-color), var(--secondary-color));
  color: white;
  border-radius: 16px;
  padding: 25px;
  text-align: center;
}

.total-header {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 12px;
  margin-bottom: 15px;
}

.total-icon {
  width: 24px;
  height: 24px;
}

.total-header h3 {
  margin: 0;
  font-size: 20px;
  font-weight: 600;
}

.total-amount {
  font-size: 32px;
  font-weight: 700;
}

/* Status styles */
.status-1 { background: #fffbe6; color: #ff9800; border: 1.5px solid #ff9800;}
.status-2 { background: #e6fffa; color: #009688; border: 1.5px solid #009688;}
.status-3 { background: #e3f2fd; color: #1976d2; border: 1.5px solid #1976d2;}
.status-4 { background: #e8f5e9; color: #43a047; border: 1.5px solid #43a047;}
.status-0 { background: #ffebee; color: #e53935; border: 1.5px solid #e53935;}

/* Responsive */
@media (max-width: 768px) {
  .order-detail-container {
    padding: 15px;
  }
  
  .header-section {
    flex-direction: column;
    align-items: flex-start;
  }
  
  .page-title {
    font-size: 22px;
  }
  
  .product-card {
    flex-direction: column;
    text-align: center;
  }
  
  .product-actions {
    flex-direction: row;
    width: 100%;
  }
  
  .btn {
    flex: 1;
  }
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.reviewed-label {
  background: #337fd5;
  border-radius: 8px;
  padding: 8px 18px;
  display: inline-block;
  margin-bottom: 6px;
  border-radius: 8px;
  padding: 8px 18px;
  display: inline-block;
  margin-bottom: 6px;
}
</style>
