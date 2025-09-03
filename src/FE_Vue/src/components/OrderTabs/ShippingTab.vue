<template>
  <div class="tab-pane">
    <h3>Thông tin giao hàng</h3>
    
    <form class="shipping-form" @submit.prevent="handleSubmit">
      <div class="form-row">
        <div class="form-group">
          <label for="fullName">
            <v-icon>mdi-account</v-icon>
            Họ và tên *
          </label>
          <input 
            type="text" 
            id="fullName" 
            :value="customer.fullName"
            readonly
          />
        </div>
        <div class="form-group">
          <label for="phone">
            <v-icon>mdi-phone</v-icon>
            Số điện thoại *
          </label>
          <input 
            type="tel" 
            id="phone" 
            :value="customer.phoneNumber"
            readonly
          />
        </div>
      </div>

      <div class="form-group">
        <label for="address">
          <v-icon>mdi-map-marker</v-icon>
          Địa chỉ giao hàng *
        </label>
        
        <!-- Hiển thị địa chỉ mặc định nếu có -->
        <div v-if="customer.address && !isEditingAddress" class="default-address">
          <div class="address-display">
            <span class="address-text">{{ customer.address }}</span>
            <span class="default-label">(Địa chỉ mặc định)</span>
          </div>
          <button 
            type="button" 
            class="btn-change-address" 
            @click="enableAddressEdit"
          >
            Thay đổi địa chỉ giao hàng
          </button>
        </div>

        <!-- Form nhập địa chỉ mới -->
        <div v-if="!customer.address || isEditingAddress">
          <textarea 
            id="address" 
            v-model="deliveryAddress"
            :placeholder="customer.address ? 'Nhập địa chỉ giao hàng mới' : 'Số nhà, tên đường, phường/xã, quận/huyện, tỉnh/thành phố'"
            rows="3"
            required
          ></textarea>
          
          <!-- Nút sử dụng địa chỉ mặc định nếu đang edit -->
          <div v-if="customer.address && isEditingAddress" class="address-actions">
            <button 
              type="button" 
              class="btn-use-default" 
              @click="useDefaultAddress"
            >
              Sử dụng địa chỉ mặc định
            </button>
          </div>
        </div>
      </div>

      <div class="form-row">
        <div class="form-group">
          <label for="deliveryTime">
            <v-icon>mdi-truck-delivery</v-icon>
            Đơn vị vận chuyển
          </label>
          <select id="deliveryTime" v-model="selectedShippingCost" @change="updateShippingCost">
            <template v-for="item in shipItems" :key="item.id">
              <option :value="item.cost">
                {{ item.name }} - {{ formatPrice(item.cost) }}
              </option>
            </template>
          </select>
          
          <!-- Hiển thị phí vận chuyển được chọn -->
          <div v-if="selectedShippingCost > 0" class="shipping-cost-display">
            <span class="cost-label">Phí vận chuyển:</span>
            <span class="cost-value">{{ formatPrice(selectedShippingCost) }}</span>
          </div>
        </div>
        <div class="form-group">
          <label for="deliveryNote">
            <v-icon>mdi-note-text</v-icon>
            Ghi chú
          </label>
          <input 
            type="text" 
            id="deliveryNote" 
            v-model="deliveryNote"
            placeholder="Ghi chú cho người giao hàng"
          />
        </div>
      </div>
    </form>

    <div class="navigation-buttons">
      <button class="btn btn-secondary" @click="$emit('previous')">
        <v-icon>mdi-arrow-left</v-icon>
        Quay lại
      </button>
      <button class="btn btn-primary" @click="handleNext">
        Tiếp tục
        <v-icon>mdi-arrow-right</v-icon>
      </button>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import { getUserId } from '@/utils/jwtHelper';

// Import Store 
import { useUserStore } from '@/stores/api/userStore';
import { useShipStore } from '@/stores/api/shipStore';
import { useOrderStore } from '@/stores/api/orderStore';

const userStore = useUserStore();
const shipStore = useShipStore();
const orderStore = useOrderStore();

const emit = defineEmits(['next', 'previous']);

const customer = ref({});
const shipItems = ref([]);
const isEditingAddress = ref(false);

// Các biến riêng lẻ thay vì formData
const deliveryAddress = ref('');
const selectedShippingCost = ref(0);
const deliveryNote = ref('');

// Computed để lấy địa chỉ giao hàng cuối cùng
const finalDeliveryAddress = computed(() => {
  // Nếu đang edit hoặc không có địa chỉ mặc định, dùng địa chỉ từ input
  if (isEditingAddress.value || !customer.value.address) {
    return deliveryAddress.value;
  }
  // Ngược lại dùng địa chỉ mặc định
  return customer.value.address;
});

// Format giá tiền
const formatPrice = (price) => {
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price);
};

// Tìm đơn vị vận chuyển có phí thấp nhất
const findCheapestShipping = () => {
  if (shipItems.value.length > 0) {
    const cheapest = shipItems.value.reduce((min, current) => {
      return current.cost < min.cost ? current : min;
    });
    return cheapest;
  }
  return null;
};

// Cập nhật phí vận chuyển khi thay đổi lựa chọn
const updateShippingCost = () => {
  emit('shipping-cost-changed', selectedShippingCost.value);
};

// Bật chế độ chỉnh sửa địa chỉ
const enableAddressEdit = () => {
  isEditingAddress.value = true;
  deliveryAddress.value = ''; // Reset address input
};

// Sử dụng địa chỉ mặc định
const useDefaultAddress = () => {
  isEditingAddress.value = false;
  deliveryAddress.value = '';
};

const handleNext = async () => {
  const finalAddress = finalDeliveryAddress.value;
  
  if (!finalAddress.trim()) {
    alert('Vui lòng nhập địa chỉ giao hàng!');
    return;
  }

  // Tìm thông tin đơn vị vận chuyển được chọn
  const selectedShipper = shipItems.value.find(item => item.cost === selectedShippingCost.value);
  
  if (!selectedShipper) {
    alert('Vui lòng chọn đơn vị vận chuyển!');
    return;
  }

  try {
    // Lưu thông tin vận chuyển vào session storage
    await orderStore.saveShipMethodToSession(selectedShipper);
    await orderStore.saveShipAddressToSession(finalAddress);
    
    // Emit để chuyển tab
    emit('next');
  } catch (error) {
    console.error('Lỗi khi lưu thông tin vận chuyển:', error);
    alert('Có lỗi xảy ra. Vui lòng thử lại!');
  }
};

onMounted(async () => {
  await shipStore.fetchAllShippers();
  shipItems.value = shipStore.shippers;
  
  // Set mặc định là đơn vị có phí thấp nhất
  if (shipItems.value.length > 0) {
    const cheapest = findCheapestShipping();
    selectedShippingCost.value = cheapest.cost;
    updateShippingCost();
  }

  const userId = getUserId();
  await userStore.fetchProfile(userId);
  customer.value = userStore.profile;
});
</script>

<style scoped>
.shipping-form {
  margin: 30px 0;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
  margin-bottom: 20px;
}

.form-group {
  margin-bottom: 25px;
}

.form-group label {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 10px;
  font-weight: 600;
  color: #2c3e50;
  font-size: 16px;
}

.label-icon {
  color: #e67e22;
  width: 20px;
  height: 20px;
}

.form-group input,
.form-group textarea,
.form-group select {
  width: 100%;
  padding: 15px;
  border: 2px solid #e9ecef;
  border-radius: 10px;
  font-size: 16px;
  transition: all 0.3s ease;
  box-sizing: border-box;
}

.form-group input:focus,
.form-group textarea:focus,
.form-group select:focus {
  outline: none;
  border-color: #e67e22;
  box-shadow: 0 0 0 3px rgba(230, 126, 34, 0.1);
}

.form-group textarea {
  resize: vertical;
  min-height: 80px;
}

.form-group select {
  cursor: pointer;
}

/* Style cho địa chỉ mặc định */
.default-address {
  border: 2px solid #e9ecef;
  border-radius: 10px;
  padding: 15px;
  background: #f8f9fa;
}

.address-display {
  margin-bottom: 12px;
}

.address-text {
  font-size: 16px;
  color: #2c3e50;
  display: block;
  margin-bottom: 5px;
}

.default-label {
  font-size: 12px;
  color: #6c757d;
  font-style: italic;
}

.btn-change-address {
  background: #e67e22;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 6px;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.3s ease;
}

.btn-change-address:hover {
  background: #d35400;
  transform: translateY(-1px);
}

/* Actions cho địa chỉ */
.address-actions {
  margin-top: 10px;
}

.btn-use-default {
  background: #6c757d;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 6px;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.3s ease;
}

.btn-use-default:hover {
  background: #5a6268;
  transform: translateY(-1px);
}

/* Style cho hiển thị phí vận chuyển */
.shipping-cost-display {
  margin-top: 10px;
  padding: 12px 15px;
  background: linear-gradient(135deg, #fdfbf7, #f8f1e8);
  border-radius: 8px;
  border: 1px solid #f0ebe1;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.cost-label {
  font-weight: 600;
  color: #2c3e50;
  font-size: 14px;
}

.cost-value {
  font-weight: 700;
  color: #e67e22;
  font-size: 16px;
}

/* Navigation buttons */
.navigation-buttons {
  display: flex;
  justify-content: space-between;
  margin-top: 35px;
  padding-top: 25px;
  border-top: 1px solid #ecf0f1;
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

.btn-primary {
  background: linear-gradient(135deg, #e67e22, #f39c12);
  color: white;
  box-shadow: 0 3px 12px rgba(230, 126, 34, 0.25);
}

.btn-primary:hover {
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

@media (max-width: 768px) {
  .form-row {
    grid-template-columns: 1fr;
  }
}
</style>
