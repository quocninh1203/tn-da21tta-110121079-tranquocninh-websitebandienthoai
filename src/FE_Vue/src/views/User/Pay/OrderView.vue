<template>
  <div class="order-container">
    <div class="order-view">
      <!-- Header với tiến trình -->
      <div class="progress-header">
        <h2 class="order-title">Đặt hàng</h2>
        <div class="progress-bar">
          <div 
            class="progress-fill" 
            :style="{ width: progressWidth + '%' }"
          ></div>
        </div>
      </div>

      <!-- Thanh điều hướng Tab - VÔ HIỆU HÓA CLICK -->
      <div class="tabs-nav">
        <div
          v-for="(tab, index) in tabs"
          :key="tab.id"
          :class="['tab-button', { 
            active: activeTab === tab.id,
            completed: isTabCompleted(index),
            disabled: !canAccessTab(index)
          }]"
        >
          <div class="tab-icon">
            <svg-icon type="mdi" :path="tab.icon"></svg-icon>
          </div>
          <span class="tab-text">{{ tab.name }}</span>
          <div v-if="isTabCompleted(index)" class="check-mark">
            <svg-icon type="mdi" :path="mdiCheck"></svg-icon>
          </div>
        </div>
      </div>

      <!-- Nội dung Tab -->
      <div class="tab-content">
        <keep-alive>
          <component 
            :is="activeComponent"
            @next="goToNextTab"
            @previous="goToPreviousTab"
            :can-go-next="canGoNext"
            :can-go-previous="canGoPrevious"
          ></component>
        </keep-alive>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import SvgIcon from '@jamescoyle/vue-icon';
import { mdiCart, mdiTruckDelivery, mdiCreditCard, mdiCheck } from '@mdi/js';

// Import các store
// import { useOrderStore } from '@/stores/api/orderStore';

// Import các component con
import CartTab from '@/components/OrderTabs/CartTab.vue';
import ShippingTab from '@/components/OrderTabs/ShippingTab.vue';
import PaymentTab from '@/components/OrderTabs/PaymentTab.vue';

// const orderStore = useOrderStore();

// const listItems = ref([]); 

const tabs = ref([
  { id: 'cart', name: 'Đơn hàng', component: CartTab, icon: mdiCart },
  { id: 'shipping', name: 'Giao hàng', component: ShippingTab, icon: mdiTruckDelivery },
  { id: 'payment', name: 'Thanh toán', component: PaymentTab, icon: mdiCreditCard },
]);

const activeTab = ref('cart');
const completedTabs = ref(['cart']); // Tab đầu tiên mặc định hoàn thành
const maxAccessibleTabIndex = ref(0); // Chỉ số tab cao nhất có thể truy cập

const activeComponent = computed(() => {
  const currentTab = tabs.value.find(tab => tab.id === activeTab.value);
  return currentTab ? currentTab.component : null;
});

const currentTabIndex = computed(() => {
  return tabs.value.findIndex(tab => tab.id === activeTab.value);
});

const progressWidth = computed(() => {
  return ((currentTabIndex.value + 1) / tabs.value.length) * 100;
});

const canGoNext = computed(() => {
  return currentTabIndex.value < tabs.value.length - 1;
});

const canGoPrevious = computed(() => {
  return currentTabIndex.value > 0;
});

const isTabCompleted = (index) => {
  return completedTabs.value.includes(tabs.value[index].id);
};

const canAccessTab = (index) => {
  return index <= maxAccessibleTabIndex.value;
};

const goToNextTab = () => {
  const currentIndex = currentTabIndex.value;
  if (currentIndex < tabs.value.length - 1) {
    // Đánh dấu tab hiện tại là hoàn thành
    if (!completedTabs.value.includes(activeTab.value)) {
      completedTabs.value.push(activeTab.value);
    }
    
    // Chuyển đến tab tiếp theo
    const nextTabId = tabs.value[currentIndex + 1].id;
    activeTab.value = nextTabId;
    
    // Cập nhật tab cao nhất có thể truy cập
    maxAccessibleTabIndex.value = Math.max(maxAccessibleTabIndex.value, currentIndex + 1);
  }
};

const goToPreviousTab = () => {
  const currentIndex = currentTabIndex.value;
  if (currentIndex > 0) {
    activeTab.value = tabs.value[currentIndex - 1].id;
  }
};

// onMounted( async () => {
//   listItems.value = await orderStore.getProductsFromSession();
//   console.log(listItems.value);
  
// });
</script>

<style>
/* Biến màu mới - tông màu ấm nhưng dịu mắt hơn */
:root {
  --primary-color: #e67e22;
  --secondary-color: #f39c12;
  --accent-color: #d35400;
  --gradient-start: #e67e22;
  --gradient-end: #f39c12;
  --success-color: #27ae60;
  --text-dark: #2c3e50;
  --text-medium: #34495e;
  --text-light: #7f8c8d;
  --text-white: #ffffff;
  --background-soft: #fdf6f0;
  --background-card: #ffffff;
  --border-light: #ecf0f1;
  --border-medium: #bdc3c7;
  --shadow-soft: rgba(230, 126, 34, 0.15);
  --disabled-color: #95a5a6;
}
</style>
<style scoped>

.order-container {
  min-height: 100vh;
  background: linear-gradient(135deg, #fdf6f0 0%, #f8f1e8 100%);
  padding: 20px;
  font-family: 'Inter', 'Segoe UI', sans-serif;
}

.order-view {
  max-width: 900px;
  margin: 0 auto;
  background: var(--background-card);
  border-radius: 16px;
  box-shadow: 0 8px 32px var(--shadow-soft);
  overflow: hidden;
  border: 1px solid var(--border-light);
}

.order-view::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 4px;
  background: linear-gradient(90deg, var(--gradient-start), var(--gradient-end));
}

/* Header với tiến trình - màu dịu hơn */
.progress-header {
  padding: 25px 40px 20px;
  background: linear-gradient(135deg, #f4f1eb, #ede7dc);
  color: var(--text-dark);
  border-bottom: 1px solid var(--border-light);
}

.order-title {
  margin: 0 0 20px 0;
  font-size: 26px;
  font-weight: 600;
  text-align: center;
  color: var(--text-dark);
}

.progress-bar {
  height: 6px;
  background: var(--border-medium);
  border-radius: 3px;
  overflow: hidden;
}

.progress-fill {
  height: 100%;
  background: linear-gradient(90deg, var(--gradient-start), var(--gradient-end));
  border-radius: 3px;
  transition: width 0.4s ease;
}

/* Tab navigation - VÔ HIỆU HÓA CLICK */
.tabs-nav {
  display: flex;
  background: var(--background-soft);
  border-bottom: 2px solid var(--border-light);
}

.tab-button {
  flex: 1;
  padding: 18px 15px;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 8px;
  position: relative;
  color: var(--text-medium);
  font-weight: 500;
  border-bottom: 3px solid transparent;
  /* LOẠI BỎ cursor: pointer và click handler */
  user-select: none; /* Không cho phép select text */
}

/* LOẠI BỎ hover effect cho tab */
.tab-button:hover {
  /* Không có hover effect */
}

.tab-button.active {
  background: var(--background-card);
  color: var(--primary-color);
  border-bottom-color: var(--primary-color);
  box-shadow: 0 2px 8px rgba(230, 126, 34, 0.12);
  font-weight: 600;
}

.tab-button.completed {
  color: var(--success-color);
}

.tab-button.completed:not(.active) {
  background: rgba(39, 174, 96, 0.05);
}

/* Style cho tab bị disabled */
.tab-button.disabled {
  color: var(--disabled-color);
  opacity: 0.6;
}

.tab-button.disabled .tab-icon {
  background: var(--disabled-color);
  color: var(--text-white);
}

.tab-icon {
  width: 36px;
  height: 36px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  background: var(--border-light);
  transition: all 0.3s ease;
  color: var(--text-medium);
}

.tab-button.active .tab-icon {
  background: var(--primary-color);
  color: var(--text-white);
  box-shadow: 0 2px 8px rgba(230, 126, 34, 0.3);
}

.tab-button.completed .tab-icon {
  background: var(--success-color);
  color: var(--text-white);
}

.tab-text {
  font-size: 14px;
  text-align: center;
}

.check-mark {
  position: absolute;
  top: 6px;
  right: 6px;
  width: 18px;
  height: 18px;
  background: var(--success-color);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 10px;
  box-shadow: 0 2px 4px rgba(39, 174, 96, 0.3);
}

/* Nội dung tab */
.tab-content {
  padding: 35px;
  min-height: 400px;
  background: var(--background-card);
}

/* Kiểu chung cho các pane */
:deep(.tab-pane) {
  animation: fadeInUp 0.4s ease;
}

:deep(.tab-pane h3) {
  margin-top: 0;
  margin-bottom: 25px;
  font-size: 22px;
  color: var(--text-dark);
  position: relative;
  padding-bottom: 12px;
  font-weight: 600;
}

:deep(.tab-pane h3::after) {
  content: '';
  position: absolute;
  bottom: 0;
  left: 0;
  width: 50px;
  height: 3px;
  background: linear-gradient(90deg, var(--gradient-start), var(--gradient-end));
  border-radius: 2px;
}

/* Nút điều hướng - màu dịu hơn */
:deep(.navigation-buttons) {
  display: flex;
  justify-content: space-between;
  margin-top: 35px;
  padding-top: 25px;
  border-top: 1px solid var(--border-light);
}

:deep(.btn) {
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

:deep(.btn:disabled) {
  opacity: 0.5;
  cursor: not-allowed;
  transform: none !important;
}

:deep(.btn-primary) {
  background: linear-gradient(135deg, var(--gradient-start), var(--gradient-end));
  color: var(--text-white);
  box-shadow: 0 3px 12px rgba(230, 126, 34, 0.25);
}

:deep(.btn-primary:hover:not(:disabled)) {
  transform: translateY(-2px);
  box-shadow: 0 5px 16px rgba(230, 126, 34, 0.35);
}

:deep(.btn-secondary) {
  background: var(--background-card);
  color: var(--text-medium);
  border-color: var(--border-medium);
}

:deep(.btn-secondary:hover:not(:disabled)) {
  background: var(--background-soft);
  color: var(--text-dark);
  transform: translateY(-1px);
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(15px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* Responsive */
@media (max-width: 768px) {
  .order-container {
    padding: 15px;
  }
  
  .tab-content {
    padding: 25px 20px;
  }
  
  .progress-header {
    padding: 20px 25px 15px;
  }
  
  .order-title {
    font-size: 22px;
  }
}
</style>
