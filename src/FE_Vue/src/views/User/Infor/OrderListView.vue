<template>
  <div class="order-list-container">
    <div class="header-section">
      <h1 class="page-title">---------------
        <svg-icon type="mdi" :path="mdiReceiptText" class="title-icon"></svg-icon>
        Đơn hàng của bạn  ---------------
      </h1>
      <p class="page-subtitle">Quản lý và theo dõi tất cả đơn hàng của bạn</p>
      <!-- Filter by status -->
      <div class="order-status-filter">
        <span
          v-for="option in statusOptions"
          :key="option.value"
          class="status-filter-option"
          :class="{ active: selectedStatus === option.value }"
          @click="selectStatus(option.value)"
        >
          {{ option.label }}
        </span>
      </div>
    </div>

    <!-- Loading state -->
    <div v-if="isLoading" class="loading-container">
      <div class="loading-spinner"></div>
      <p>Đang tải danh sách đơn hàng...</p>
    </div>

    <!-- Empty state -->
    <div v-else-if="orders.length === 0" class="empty-state">
      <div class="empty-icon">
        <svg-icon type="mdi" :path="mdiPackageVariantClosed" class="empty-icon-svg"></svg-icon>
      </div>
      <h3>Chưa có đơn hàng nào</h3>
      <p>Bạn chưa có đơn hàng nào. Hãy bắt đầu mua sắm ngay!</p>
      <button class="btn btn-primary" @click="goToShopping">
        <svg-icon type="mdi" :path="mdiShopping"></svg-icon>
        Bắt đầu mua sắm
      </button>
    </div>

    <!-- Orders list -->
    <div v-else class="orders-grid">
      <div 
        v-for="order in filteredOrders" 
        :key="order.id" 
        class="order-card"
      >
        <div class="order-header">
          <div class="order-id">
            <svg-icon type="mdi" :path="mdiReceiptText" class="order-icon"></svg-icon>
            <span class="order-number">#{{ order.orderCode }}</span>
          </div>
          <div class="order-status" :class="getStatusClass(order.orderStatus)">
            {{ getStatusText(order.orderStatus) }}
          </div>
        </div>

        <div class="order-info">
          <div class="info-row">
            <svg-icon type="mdi" :path="mdiCalendar" class="info-icon"></svg-icon>
            <span class="info-label">Ngày đặt:</span>
            <span class="info-value">{{ formatDate(order.orderDate) }}</span>
          </div>
          
          <div class="info-row">
            <svg-icon type="mdi" :path="mdiCurrencyUsd" class="info-icon"></svg-icon>
            <span class="info-label">Tổng tiền:</span>
            <span class="info-value price">{{ formatPrice(order.totalAmount) }}</span>
          </div>
          
          <div class="info-row">
            <svg-icon type="mdi" :path="mdiCreditCard" class="info-icon"></svg-icon>
            <span class="info-label">Thanh toán:</span>
            <span class="info-value">{{ order.method }}</span>
          </div>
        </div>

        <div class="order-actions">
          <button class="btn btn-outline" @click="viewOrderDetail(order.orderId)">
            <svg-icon type="mdi" :path="mdiEye"></svg-icon>
            Xem chi tiết
          </button>
          <button 
            v-if="order.orderStatus === 1" 
            class="btn btn-danger" 
            @click="showCancelModal(order)"
          >
            <svg-icon type="mdi" :path="mdiCancel"></svg-icon>
            Hủy đơn
          </button>
        </div>
      </div>
    </div>

    <!-- Confirm Modal -->
    <ConfirmModal
      v-model="cancelModal.show"
      type="danger"
      title="Hủy đơn hàng"
      :message="cancelModal.message"
      :details="cancelModal.details"
      confirm-text="Hủy đơn hàng"
      cancel-text="Giữ đơn hàng"
      @confirm="confirmCancelOrder"
      @cancel="cancelModal.show = false"
    />
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import { useRouter } from 'vue-router';
import { getUserId } from '@/utils/jwtHelper';
import SvgIcon from '@jamescoyle/vue-icon';
import { 
  mdiReceiptText, 
  mdiPackageVariantClosed, 
  mdiShopping,
  mdiCalendar,
  mdiCurrencyUsd,
  mdiCreditCard,
  mdiEye,
  mdiCancel
} from '@mdi/js';

// Import components
import ConfirmModal from '@/components/Global/ConfirmModal.vue';

// Import stores
import { useNotificationStore } from '@/stores/local/notification';
import { useOrderStore } from '@/stores/api/orderStore';

const router = useRouter();
const orderStore = useOrderStore();

const orders = ref([]);
const isLoading = ref(false);

// Cancel modal state
const cancelModal = ref({
  show: false,
  orderId: null,
  message: '',
  details: ''
});

// Status filter
const statusOptions = [
  { value: null, label: 'Tất cả' },
  { value: 1, label: 'Chờ xử lý' },
  { value: 2, label: 'Đã duyệt' },
  { value: 3, label: 'Đã thanh toán' },
  { value: 4, label: 'Đã giao hàng' },
  { value: 0, label: 'Đã hủy' }
]
const selectedStatus = ref(null)

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

const getStatusClass = (status) => {
  return `status-${status}`;
};

// Actions
const fetchOrders = async () => {
  try {
    isLoading.value = true;
    const userId = getUserId();
    await orderStore.fetchOrdersByUserId(userId);
    orders.value = orderStore.orders;
  } catch (error) {
    console.error('Lỗi khi tải đơn hàng:', error);
  } finally {
    isLoading.value = false;
  }
};

const viewOrderDetail = (orderId) => {
  router.push({ name: 'OrderDetail', params: { orderId } });
};

const showCancelModal = (order) => {
  cancelModal.value = {
    show: true,
    orderId: order.orderId,
    message: `Bạn có chắc chắn muốn hủy đơn hàng #${order.orderCode}?`,
    details: `Tổng tiền: ${formatPrice(order.totalAmount)} - Ngày đặt: ${formatDate(order.orderDate)}`
  };
};

const confirmCancelOrder = async () => {
  try {
    // Call API to cancel order
    await orderStore.changeStatus(cancelModal.value.orderId, { status: 0});
    
    // Refresh orders list
    await fetchOrders();
    
    // Close modal
    cancelModal.value.show = false;
    
    // Show success message
    useNotificationStore().addNotification('Đơn hàng đã được hủy thành công!', "success");
  } catch (error) {
    console.error('Lỗi khi hủy đơn hàng:', error);
    useNotificationStore().addNotification('Có lỗi xảy ra khi hủy đơn hàng. Vui lòng thử lại!', "error");
  }
};

const goToShopping = () => {
  router.push('/');
};

const selectStatus = (val) => {
  selectedStatus.value = val
}

const filteredOrders = computed(() => {
  if (selectedStatus.value === null) return [...orders.value].reverse()
  return [...orders.value].reverse().filter(order => order.orderStatus === selectedStatus.value)
})

onMounted(() => {
  fetchOrders();
});
</script>

<style>
:root {
  --primary-color: #e67e22;
  --secondary-color: #f39c12;
  --success-color: #27ae60;
  --warning-color: #f39c12;
  --danger-color: #e74c3c;
  --text-dark: #2c3e50;
  --text-medium: #34495e;
  --text-light: #7f8c8d;
  --text-orange: #ff5722;
  --background-soft: #fdf6f0;
  --border-light: #ecf0f1;
  --shadow-soft: rgba(230, 126, 34, 0.15);
}
</style>
<style scoped>
/* CSS Variables */
.order-list-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0;
  font-family: 'Inter', 'Segoe UI', sans-serif;
}

/* Header */
.header-section {
  text-align: center;
  margin-bottom: 20px;
}

.page-title {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 12px;
  margin: 0 0 0 0;
  font-size: 32px;
  font-weight: 700;
  color: var(--text-orange);
}

.title-icon {
  width: 36px;
  height: 36px;
  color: var(--primary-color);
}

.page-subtitle {
  margin: 0;
  color: var(--text-light);
  font-size: 16px;
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

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

/* Empty state */
.empty-state {
  text-align: center;
  padding: 80px 20px;
  background: var(--background-soft);
  border-radius: 16px;
  border: 1px solid var(--border-light);
}

.empty-icon {
  margin-bottom: 20px;
}

.empty-icon-svg {
  width: 80px;
  height: 80px;
  color: var(--text-light);
}

.empty-state h3 {
  margin: 0 0 10px 0;
  font-size: 24px;
  color: var(--text-dark);
}

.empty-state p {
  margin: 0 0 30px 0;
  color: var(--text-light);
}

/* Orders grid */
.orders-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(400px, 1fr));
  gap: 20px;
  max-height: 300px;
  padding-right: 8px;
  overflow-y: auto;
}
.orders-grid::-webkit-scrollbar {
  width: 8px;
}
.orders-grid::-webkit-scrollbar-thumb {
  background-color: var(--warning-color);
  border-radius: 4px;
}
.orders-grid::-webkit-scrollbar-thumb:hover {
  background-color: var(--danger-color);
}
.orders-grid::-webkit-scrollbar-track {
  background: var(--shadow-soft);      
}

.order-card {
  background: white;
  border-radius: 16px;
  border: 1px solid var(--border-light);
  box-shadow: 0 4px 20px var(--shadow-soft);
  overflow: hidden;
  transition: all 0.3s ease;
}

.order-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 30px var(--shadow-soft);
}

/* Order header */
.order-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 15px;
  background: linear-gradient(135deg, var(--background-soft), #f8f1e8);
  border-bottom: 1px solid var(--border-light);
}

.order-id {
  display: flex;
  align-items: center;
  gap: 8px;
}

.order-icon {
  width: 20px;
  height: 20px;
  color: var(--primary-color);
}

.order-number {
  font-weight: 700;
  font-size: 18px;
  color: var(--text-dark);
}

.order-status {
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 12px;
  font-weight: 600;
  text-transform: uppercase;
}

.status-1 { background: #fffbe6; color: #ff9800; border: 1.5px solid #ff9800;}
.status-2 { background: #e6fffa; color: #009688; border: 1.5px solid #009688;}
.status-3 { background: #e3f2fd; color: #1976d2; border: 1.5px solid #1976d2;}
.status-4 { background: #e8f5e9; color: #43a047; border: 1.5px solid #43a047;}
.status-0 { background: #ffebee; color: #e53935; border: 1.5px solid #e53935;}

/* Order info */
.order-info {
  padding: 20px;
}

.info-row {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 12px;
}

.info-icon {
  width: 16px;
  height: 16px;
  color: var(--text-light);
}

.info-label {
  font-weight: 600;
  color: var(--text-medium);
  min-width: 80px;
}

.info-value {
  color: var(--text-dark);
}

.info-value.price {
  font-weight: 700;
  color: var(--primary-color);
}

/* Order actions */
.order-actions {
  padding: 20px;
  border-top: 1px solid var(--border-light);
  display: flex;
  gap: 10px;
}

.btn {
  padding: 10px 20px;
  border: none;
  border-radius: 8px;
  font-weight: 600;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  gap: 6px;
  flex: 1;
  justify-content: center;
}

.btn-primary {
  background: linear-gradient(135deg, var(--primary-color), var(--secondary-color));
  color: white;
}

.btn-outline {
  background: white;
  color: var(--primary-color);
  border: 2px solid var(--primary-color);
}

.btn-danger {
  background: var(--danger-color);
  color: white;
}

.btn:hover {
  transform: translateY(-2px);
}

/* Order status filter */
.order-status-filter {
  display: flex;
  gap: 10px;
  justify-content: center;
  margin: 18px 0 0 0;
  flex-wrap: wrap;
}
.status-filter-option {
  border: 1.5px solid #bbb;
  border-radius: 8px;
  padding: 7px 18px;
  background: #fff;
  cursor: pointer;
  font-size: 1rem;
  font-weight: 500;
  color: #444;
  user-select: none;
  transition: border 0.2s, background 0.2s, color 0.2s;
}
.status-filter-option.active {
  border: 2px solid #1976d2;
  background: #e3f0ff;
  color: #1976d2;
  box-shadow: 0 2px 8px #1976d233;
}

/* Responsive */
@media (max-width: 768px) {
  .order-list-container {
    padding: 15px;
  }
  
  .orders-grid {
    grid-template-columns: 1fr;
  }
  
  .page-title {
    font-size: 24px;
  }
  
  .order-actions {
    flex-direction: column;
  }
}
</style>
