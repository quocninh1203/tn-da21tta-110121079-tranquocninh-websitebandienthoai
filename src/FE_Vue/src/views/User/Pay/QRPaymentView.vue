<template>
  <div class="qr-payment-container horizontal-layout">
    <div class="payment-card">
      <div class="content-wrapper">
        <!-- QR Code Section - Bên trái -->
        <div class="qr-section">
          <div class="qr-wrapper">
            <img 
              :src="qrCodeUrl" 
              alt="QR Code Sepay MB Bank" 
              class="qr-image"
              @load="handleImageLoad"
              @error="handleImageError"
            />
            <div v-if="isLoading" class="qr-loading">
              <div class="loading-spinner"></div>
              <p>Đang tạo mã QR...</p>
            </div>
          </div>
          
          <!-- Bank Info -->
          <div class="bank-info">
            <div class="bank-logo">
              <svg-icon type="mdi" :path="mdiBank" class="bank-icon"></svg-icon>
            </div>
            <div class="bank-details">
              <h3>MB Bank</h3>
              <p>Số tài khoản: VQRQACTBN8740</p>
            </div>
          </div>
        </div>

        <!-- Info Section - Bên phải -->
        <div class="info-section">
          <!-- Header -->
          <div class="payment-header">
            <div class="header-content">
              <div class="header-icon">
                <svg-icon type="mdi" :path="mdiQrcodeScan" class="qr-icon"></svg-icon>
              </div>
              <div class="header-text">
                <h1 class="payment-title">Thanh toán QR Code</h1>
                <p class="payment-subtitle">Quét mã QR để thanh toán qua MB Bank</p>
              </div>
            </div>
            
            <!-- Countdown Timer -->
            <div class="countdown-timer" :class="{ 'warning': timeLeft <= 120 }">
              <svg-icon type="mdi" :path="mdiTimerSand" class="timer-icon"></svg-icon>
              <span class="timer-text">{{ formatTime(timeLeft) }}</span>
            </div>
          </div>

          <!-- Payment Details -->
          <div class="payment-details">
            <div class="detail-row">
              <span class="detail-label">
                <svg-icon type="mdi" :path="mdiReceiptText" class="detail-icon"></svg-icon>
                Mã đơn hàng:
              </span>
              <span class="detail-value order-id">{{ orderCode }}</span>
            </div>
            
            <div class="detail-row">
              <span class="detail-label">
                <svg-icon type="mdi" :path="mdiCurrencyUsd" class="detail-icon"></svg-icon>
                Số tiền:
              </span>
              <span class="detail-value amount">{{ formatPrice(amount) }}</span>
            </div>
            
            <div class="detail-row">
              <span class="detail-label">
                <svg-icon type="mdi" :path="mdiClockOutline" class="detail-icon"></svg-icon>
                Thời gian:
              </span>
              <span class="detail-value">{{ currentTime }}</span>
            </div>
          </div>

          <!-- Instructions -->
          <div class="instructions">
            <h3 class="instructions-title">
              <svg-icon type="mdi" :path="mdiInformation" class="info-icon"></svg-icon>
              Hướng dẫn thanh toán
            </h3>
            <ol class="instructions-list">
              <li>Mở ứng dụng MB Bank trên điện thoại</li>
              <li>Chọn chức năng "Quét QR" hoặc "Chuyển tiền QR"</li>
              <li>Quét mã QR code bên trái</li>
              <li>Kiểm tra thông tin và xác nhận thanh toán</li>
              <li>Hoàn tất giao dịch trong {{ formatTime(timeLeft) }}</li>
            </ol>
          </div>

          <!-- Footer -->
          <div class="payment-footer">
            <p class="footer-note">
              <svg-icon type="mdi" :path="mdiShieldCheck" class="shield-icon"></svg-icon>
              Giao dịch được bảo mật bởi Sepay
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted } from 'vue';
import { useRouter } from 'vue-router';
import SvgIcon from '@jamescoyle/vue-icon';
import {
  mdiQrcodeScan,
  mdiBank,
  mdiReceiptText,
  mdiCurrencyUsd,
  mdiClockOutline,
  mdiInformation,
  mdiShieldCheck,
  mdiTimerSand
} from '@mdi/js';
import { useOrderStore } from '@/stores/api/orderStore';
import { useNotificationStore } from '@/stores/local/notification';

const router = useRouter();
const orderStore = useOrderStore();

const orderCode = ref(null);
const amount = ref(0);
const orderId = ref(null);
const currentTime = ref('');
const timeLeft = ref(600); // 10 phút
const isLoading = ref(true);

let timeInterval = null;
let countdownInterval = null;
let pollingInterval = null;

const qrCodeUrl = computed(() => {
  const baseUrl = 'https://qr.sepay.vn/img';
  const acc = 'VQRQACTBN8740';
  const bank = 'MBBank';
  const des = encodeURIComponent(orderCode.value);
  return `${baseUrl}?acc=${acc}&bank=${bank}&amount=2000&des=${des}`;
});

const formatPrice = (value) => {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(value);
};

const formatTime = (seconds) => {
  const minutes = Math.floor(seconds / 60);
  const secs = seconds % 60;
  return `${minutes.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;
};

const updateTime = () => {
  const now = new Date();
  currentTime.value = now.toLocaleString('vi-VN', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit'
  });
};

const checkOrderStatus = async () => {
  if (!orderId.value) return;

  try {
    await orderStore.fetchOrderDetail(orderId.value);
    const status = orderStore.orderDetails?.orderStatus;

    if (status === 3) {
      clearInterval(pollingInterval);
      clearInterval(countdownInterval);
      router.push({ name: 'orderSuccess' });
    }

    // nếu là 1 thì bỏ qua, tiếp tục lần sau
  } catch (error) {
    console.error('Lỗi khi kiểm tra trạng thái đơn hàng:', error);
  }
};

const handleRemoveOrder = () => {
  const status = orderStore.orderDetails?.orderStatus;
  if(status === 3) return;
  orderStore.removeOrder(orderId.value)
    .then(() => {
      setTimeout(() => {
        useNotificationStore().addNotification("Thanh toán thất bại. Đơn hàng của bạn sẽ bị hủy.", "error");
      }, 1000);
    })
    .catch(error => {
      console.error('Lỗi khi xóa đơn hàng:', error);
    });
}

const handleTimeExpired = () => {
  clearInterval(pollingInterval);
  handleRemoveOrder();
  router.push('/cart');
};

const fetchOrderByCode = async () => {
  try {
    const response = orderStore.orderDetails;
    orderId.value = response.orderId;
    orderCode.value = response.orderCode;
    amount.value = response.totalAmount;
  } catch (error) {
    console.error('Lỗi khi lấy thông tin đơn hàng:', error);
  }
};

const handleImageLoad = () => {
  isLoading.value = false;
};

const handleImageError = () => {
  isLoading.value = false;
  console.error('Lỗi tải mã QR');
};

onMounted(async () => {
  await fetchOrderByCode();

  updateTime();
  timeInterval = setInterval(updateTime, 1000);

  countdownInterval = setInterval(() => {
    timeLeft.value--;
    if (timeLeft.value <= 0) {
      clearInterval(countdownInterval);
      handleTimeExpired();
    }
  }, 1000);

  pollingInterval = setInterval(checkOrderStatus, 5000);
});

onUnmounted(() => {
  clearInterval(timeInterval);
  clearInterval(countdownInterval);
  clearInterval(pollingInterval);
  handleRemoveOrder();
});
</script>


<style scoped>
/* CSS Variables */
:root {
  --primary-color: #e67e22;
  --secondary-color: #f39c12;
  --success-color: #27ae60;
  --warning-color: #f39c12;
  --danger-color: #e74c3c;
  --info-color: #3498db;
  --text-dark: #2c3e50;
  --text-medium: #34495e;
  --text-light: #7f8c8d;
  --background-soft: #fdf6f0;
  --border-light: #ecf0f1;
  --shadow-soft: rgba(230, 126, 34, 0.15);
  --mb-blue: #0066cc;
}

.qr-payment-container {
  min-height: 100vh;
  background: linear-gradient(135deg, #f8fff8 0%, #e8f5e8 100%);
  padding: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-family: 'Inter', 'Segoe UI', sans-serif;
}

.payment-card {
  background: white;
  border-radius: 20px;
  box-shadow: 0 20px 60px var(--shadow-soft);
  max-width: 1200px;
  width: 100%;
  overflow: hidden;
  border: 1px solid var(--border-light);
}

/* Horizontal Layout */
.horizontal-layout .content-wrapper {
  display: flex;
  flex-direction: row;
  min-height: 600px;
}

/* QR Section - Bên trái */
.horizontal-layout .qr-section {
  flex: 1;
  background: linear-gradient(135deg, var(--mb-blue), #0052a3);
  padding: 40px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  gap: 30px;
}

.horizontal-layout .qr-wrapper {
  position: relative;
  background: white;
  padding: 20px;
  border-radius: 20px;
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.2);
}

.horizontal-layout .qr-image {
  max-width: 280px;
  width: 100%;
  height: auto;
  border-radius: 12px;
  display: block;
}

.qr-loading {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  background: rgba(255, 255, 255, 0.95);
  padding: 20px;
  border-radius: 12px;
  text-align: center;
}

.loading-spinner {
  width: 40px;
  height: 40px;
  border: 4px solid var(--border-light);
  border-top: 4px solid var(--mb-blue);
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin: 0 auto 10px;
}

/* Bank Info trong QR Section */
.horizontal-layout .bank-info {
  background: rgba(255, 255, 255, 0.1);
  padding: 20px;
  border-radius: 16px;
  display: flex;
  align-items: center;
  gap: 15px;
  color: white;
}

.horizontal-layout .bank-logo {
  width: 50px;
  height: 50px;
  background: rgba(255, 255, 255, 0.2);
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.horizontal-layout .bank-icon {
  width: 28px;
  height: 28px;
  color: white;
}

.horizontal-layout .bank-details h3 {
  margin: 0 0 5px 0;
  font-size: 18px;
  font-weight: 700;
  color: white;
}

.horizontal-layout .bank-details p {
  margin: 0;
  font-size: 14px;
  color: rgba(255, 255, 255, 0.8);
  font-family: 'Courier New', monospace;
}

/* Info Section - Bên phải */
.horizontal-layout .info-section {
  flex: 1;
  padding: 40px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

/* Header trong Info Section */
.horizontal-layout .payment-header {
  margin-bottom: 30px;
}

.horizontal-layout .header-content {
  display: flex;
  align-items: center;
  gap: 15px;
  margin-bottom: 20px;
}

.horizontal-layout .header-icon {
  width: 60px;
  height: 60px;
  background: linear-gradient(135deg, var(--mb-blue), #0052a3);
  border-radius: 15px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.horizontal-layout .qr-icon {
  width: 30px;
  height: 30px;
  color: white;
}

.horizontal-layout .header-text h1 {
  margin: 0 0 5px 0;
  font-size: 28px;
  font-weight: 700;
  color: var(--text-dark);
}

.horizontal-layout .header-text p {
  margin: 0;
  font-size: 16px;
  color: var(--text-light);
}

/* Countdown Timer */
.countdown-timer {
  display: flex;
  align-items: center;
  gap: 10px;
  background: linear-gradient(135deg, var(--success-color), #2ecc71);
  color: white;
  padding: 12px 20px;
  border-radius: 25px;
  font-size: 18px;
  font-weight: 700;
  transition: all 0.3s ease;
  width: fit-content;
}

.countdown-timer.warning {
  background: linear-gradient(135deg, var(--danger-color), #c0392b);
  animation: pulse 1s infinite;
}

.timer-icon {
  width: 20px;
  height: 20px;
}

@keyframes pulse {
  0% { transform: scale(1); }
  50% { transform: scale(1.05); }
  100% { transform: scale(1); }
}

/* Payment Details */
.payment-details {
  background: var(--background-soft);
  padding: 25px;
  border-radius: 16px;
  margin-bottom: 25px;
}

.detail-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 15px 0;
  border-bottom: 1px solid #f0f0f0;
}

.detail-row:last-child {
  border-bottom: none;
}

.detail-label {
  display: flex;
  align-items: center;
  gap: 8px;
  font-weight: 600;
  color: var(--text-medium);
}

.detail-icon {
  width: 18px;
  height: 18px;
  color: var(--primary-color);
}

.detail-value {
  font-weight: 600;
  color: var(--text-dark);
}

.order-id {
  font-family: 'Courier New', monospace;
  background: white;
  padding: 4px 8px;
  border-radius: 6px;
  font-size: 14px;
}

.amount {
  font-size: 18px;
  color: var(--primary-color);
}

/* Instructions */
.instructions {
  margin-bottom: 25px;
}

.instructions-title {
  display: flex;
  align-items: center;
  gap: 10px;
  margin: 0 0 15px 0;
  font-size: 18px;
  font-weight: 600;
  color: var(--text-dark);
}

.info-icon {
  width: 20px;
  height: 20px;
  color: var(--info-color);
}

.instructions-list {
  margin: 0;
  padding-left: 20px;
  color: var(--text-medium);
}

.instructions-list li {
  margin-bottom: 8px;
  line-height: 1.5;
}

/* Footer */
.payment-footer {
  text-align: center;
  padding-top: 20px;
  border-top: 1px solid var(--border-light);
}

.footer-note {
  margin: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  font-size: 14px;
  color: var(--text-light);
}

.shield-icon {
  width: 16px;
  height: 16px;
  color: var(--success-color);
}

/* Animations */
@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

/* Responsive */
@media (max-width: 1024px) {
  .horizontal-layout .content-wrapper {
    flex-direction: column;
  }
  
  .horizontal-layout .qr-section {
    padding: 30px;
  }
  
  .horizontal-layout .info-section {
    padding: 30px;
  }
}

@media (max-width: 768px) {
  .qr-payment-container {
    padding: 15px;
  }
  
  .horizontal-layout .qr-section,
  .horizontal-layout .info-section {
    padding: 20px;
  }
  
  .horizontal-layout .header-content {
    flex-direction: column;
    text-align: center;
  }
  
  .detail-row {
    flex-direction: column;
    gap: 8px;
    text-align: center;
  }
}
</style>
