<template>
  <div class="tab-pane">
    <h3>Phương thức thanh toán</h3>
    
    <div class="payment-methods">
      <div 
        v-for="method in paymentMethods" 
        :key="method.id"
        :class="['payment-option', { selected: selectedMethod?.id === method.id }]"
        @click="selectedMethod = method"
      >
        <div class="option-header">
          <div class="option-icon">
            <v-icon :icon="method.icon" size="24"></v-icon>
          </div>
          <div class="option-info">
            <h4>{{ method.name }}</h4>
            <p>{{ method.description }}</p>
          </div>
          <div class="option-radio">
            <input 
              type="radio" 
              :id="'method-' + method.id" 
              :value="method" 
              v-model="selectedMethod"
            />
          </div>
        </div>
        
      </div>
    </div>

    <!-- Tóm tắt đơn hàng -->
    <div class="order-summary">
      <h4>Tóm tắt đơn hàng</h4>
      <div class="summary-item">
        <span>Tổng tiền hàng:</span>
        <span>{{ formatVND(subtotal) }}</span>
      </div>
      <div class="summary-item">
        <span>Phí vận chuyển:</span>
        <span>{{ formatVND(shippingCost) }}</span>
      </div>
      <div class="summary-item total">
        <span>Tổng thanh toán:</span>
        <span class="total-price">{{ formatVND(total) }}</span>
      </div>
    </div>

    <div class="navigation-buttons">
      <button class="btn btn-secondary" @click="$emit('previous')" :disabled="isProcessing">
        <v-icon>mdi-arrow-left</v-icon>
        Quay lại
      </button>
      <button 
        class="btn btn-primary btn-complete" 
        @click="completeOrder"
        :disabled="isProcessing || !selectedMethod"
      >
        <v-icon>mdi-check</v-icon>
        {{ isProcessing ? 'Đang xử lý...' : 'Hoàn tất đặt hàng' }}
      </button>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { getUserId } from '@/utils/jwtHelper'
import { formatVND } from '@/utils/formatters'
import { useOrderStore } from '@/stores/api/orderStore'
import { useCartStore } from '@/stores/api/cartStore'
import { useNotificationStore } from '@/stores/local/notification'

const orderStore = useOrderStore()
const cartStore = useCartStore()
const notificationStore = useNotificationStore()
const router = useRouter()
const emit = defineEmits(['previous'])

// Dữ liệu reactive
const selectedMethod = ref(null) // Phương thức thanh toán được chọn
const isProcessing = ref(false) // Trạng thái đang xử lý đơn hàng
const cartItems = ref([]) // Danh sách sản phẩm trong giỏ hàng
const shipMethodData = ref({}) // Thông tin phương thức vận chuyển
const shipAddress = ref("") // Địa chỉ giao hàng

// Cấu hình các phương thức thanh toán
const paymentMethods = [
  {
    id: 1,
    name: 'Thanh toán khi nhận hàng',
    description: 'Thanh toán bằng tiền mặt khi nhận được hàng',
    icon: 'mdi-cash'
  },
  {
    id: 4,
    name: 'Chuyển khoản ngân hàng',
    description: 'Chuyển khoản qua Internet Banking',
    icon: 'mdi-bank'
  }
]

// Tính toán tự động (Computed properties)
// Tính tổng tiền hàng (chưa bao gồm phí vận chuyển)
const subtotal = computed(() => 
  cartItems.value.reduce((sum, item) => sum + (item.price * item.quantity), 0)
)

// Lấy phí vận chuyển từ phương thức vận chuyển đã chọn
const shippingCost = computed(() => shipMethodData.value.cost || 0)

// Tính tổng số tiền phải thanh toán (tiền hàng + phí vận chuyển)
const total = computed(() => subtotal.value + shippingCost.value)

// Kiểm tra tính hợp lệ của đơn hàng trước khi tạo
const validateOrder = () => {
  // Kiểm tra đã chọn phương thức thanh toán chưa
  if (!selectedMethod.value) {
    alert('Vui lòng chọn phương thức thanh toán!')
    return false
  }

  // Kiểm tra giỏ hàng có sản phẩm không
  if (cartItems.value.length === 0) {
    alert('Không có sản phẩm nào trong giỏ hàng!')
    return false
  }

  // Kiểm tra thông tin vận chuyển
  if (!shipMethodData.value.cost) {
    alert('Không có thông tin vận chuyển!')
    return false
  }

  // Kiểm tra phương thức thanh toán được hỗ trợ (chỉ COD và chuyển khoản)
  if (![1, 4].includes(selectedMethod.value.id)) {
    alert('Hiện tại chỉ hỗ trợ thanh toán khi nhận hàng và chuyển khoản ngân hàng!')
    return false
  }

  return true
}

// Tạo đơn hàng mới
const createOrder = async () => {
  const userId = getUserId() // Lấy ID người dùng từ JWT token
  
  // Chuẩn bị dữ liệu đơn hàng
  const orderData = {
    userId: userId,
    methodId: selectedMethod.value.id, // ID phương thức thanh toán
    shipperId: shipMethodData.value.id, // ID phương thức vận chuyển
    shippingAddress: shipAddress.value, // Địa chỉ giao hàng
  }
  
  console.log('Tạo đơn hàng với dữ liệu:', orderData)
  await orderStore.createOrder(orderData)
  
  // Kiểm tra đơn hàng đã được tạo thành công chưa
  if (!orderStore.yourOrder) {
    throw new Error('Không thể tạo đơn hàng')
  }
  
  // Trả về ID của đơn hàng vừa tạo
  return orderStore.yourOrder.id || orderStore.yourOrder.orderId
}

// Tạo chi tiết đơn hàng cho từng sản phẩm
const createOrderDetails = async (orderId) => {
  console.log('Tạo chi tiết đơn hàng cho order ID:', orderId)
  
  // Duyệt qua từng sản phẩm trong giỏ hàng và tạo chi tiết đơn hàng
  for (const item of cartItems.value) {
    const orderDetailData = {
      orderId: orderId, // ID đơn hàng 
      variantId: item.variantId, // ID biến thể sản phẩm
      quantity: item.quantity, // Số lượng
    }
    
    console.log('Tạo chi tiết đơn hàng:', orderDetailData)
    const res = await orderStore.createOrderDetail(orderDetailData)

    if (res.data.success === false) {
      notificationStore.addNotification(res.data.message, 'error');
      orderStore.removeOrder(orderId);
      return;
    }
  }
  
  // Lấy lại thông tin chi tiết đơn hàng sau khi tạo
  await orderStore.fetchOrderDetail(orderId)
}

// Xóa sản phẩm đã mua khỏi giỏ hàng
const clearCart = async () => {
  try {
    // Lấy danh sách sản phẩm trong giỏ hàng từ session
    const listItems = await orderStore.getProductsFromSession() // Thêm await
    const userId = getUserId()
    console.log(listItems);
    
    // Kiểm tra listItems có phải là mảng không
    if (!Array.isArray(listItems) || listItems.length === 0) {
      console.log('Không có sản phẩm nào trong giỏ hàng để xóa')
      return
    }

    // Gọi deleteCart cho từng item
    for (const item of listItems) {
      if (item && item.cartId) {
        await cartStore.deleteCart(item.cartId, userId)
        console.log(`Đã xóa sản phẩm cartId: ${item.cartId}`)
      }
    }
    // Xóa sản phẩm khỏi session storage
    orderStore.clearProductsFromSession()
  } catch (error) {
    console.error('Lỗi khi xóa sản phẩm khỏi giỏ hàng:', error)
  }
}

// Xử lý chuyển hướng theo phương thức thanh toán
const handlePaymentRedirect = (orderId) => {
  if (selectedMethod.value.id === 4) {
    // Phương thức chuyển khoản ngân hàng - chuyển đến trang thanh toán
    router.push('/order/payment/')
    return
  }
  
  // Thanh toán khi nhận hàng (COD) - chuyển đến trang thành công
  navigateToSuccessPage(orderId)
}

// Điều hướng đến trang thành công với route động
const navigateToSuccessPage = (orderId) => {
  const routeName = `OrderSuccess-${orderId}` // Tên route động
  const routePath = `/order-success/${orderId}` // Đường dẫn route
  
  // Tạo route động nếu chưa tồn tại
  if (!router.hasRoute(routeName)) {
    router.addRoute({
      path: routePath,
      name: routeName,
      component: () => import('@/views/User/Pay/OrderSuccessView.vue'),
      props: true,
      meta: {
        requiresAuth: true, // Yêu cầu đăng nhập
        temporary: true, // Route tạm thời
        orderId: orderId
      }
    })
  }
  
  // Chuyển hướng đến trang thành công
  router.push({
    name: routeName,
    params: { orderId: orderId }
  })
}

// Hàm chính xử lý hoàn tất đơn hàng
const completeOrder = async () => {
  // Kiểm tra đang xử lý hoặc validation fail thì return
  if (isProcessing.value || !validateOrder()) return
  
  try {
    isProcessing.value = true // Bắt đầu xử lý
    
    // Bước 1: Tạo đơn hàng
    const orderId = await createOrder()
    console.log('Đơn hàng đã tạo với ID:', orderId)
    
    // Bước 2: Tạo chi tiết đơn hàng cho từng sản phẩm
    await createOrderDetails(orderId)
    console.log('Tất cả chi tiết đơn hàng đã được tạo')
    
    // Bước 3: Xóa sản phẩm khỏi giỏ hàng
    await clearCart()

    // Bước 4: Xử lý thanh toán và chuyển hướng
    handlePaymentRedirect(orderId)
    
  } catch (error) {
    console.error('Lỗi khi tạo đơn hàng:', error)
    alert('Có lỗi xảy ra khi đặt hàng. Vui lòng thử lại!')
  } finally {
    isProcessing.value = false // Kết thúc xử lý
  }
}

// Tải dữ liệu từ session storage
const loadSessionData = async () => {
  try {
    // Lấy danh sách sản phẩm từ session
    cartItems.value = await orderStore.getProductsFromSession()
    // Lấy thông tin phương thức vận chuyển từ session
    shipMethodData.value = await orderStore.getShipMethodFromSession()
    // Lấy địa chỉ giao hàng từ session
    shipAddress.value = await orderStore.getShipAddressFromSession()
    
    console.log('Dữ liệu giỏ hàng:', cartItems.value)
    console.log('Dữ liệu vận chuyển:', shipMethodData.value)
  } catch (error) {
    console.error('Lỗi khi load dữ liệu từ session:', error)
  }
}

// Lifecycle hook - chạy khi component được mount
onMounted(() => {
  loadSessionData() // Tải dữ liệu từ session khi component được khởi tạo
})
</script>

<style scoped>
.payment-methods {
  margin: 30px 0;
}

.payment-option {
  border: 2px solid #e9ecef;
  border-radius: 12px;
  margin-bottom: 15px;
  cursor: pointer;
  transition: all 0.3s ease;
  overflow: hidden;
}

.payment-option:hover {
  border-color: #ff6b35;
  box-shadow: 0 4px 15px rgba(255, 107, 53, 0.1);
}

.payment-option.selected {
  border-color: #ff6b35;
  background: linear-gradient(135deg, #fff8f5, #ffe0d6);
}

.option-header {
  display: flex;
  align-items: center;
  padding: 20px;
}

.option-icon {
  width: 50px;
  height: 50px;
  background: linear-gradient(135deg, #ff6b35, #ff8c42);
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  margin-right: 15px;
}

.option-info {
  flex: 1;
}

.option-info h4 {
  margin: 0 0 5px 0;
  font-size: 18px;
  font-weight: 600;
  color: #2c3e50;
}

.option-info p {
  margin: 0;
  color: #6c757d;
  font-size: 14px;
}

.option-radio input[type="radio"] {
  width: 20px;
  height: 20px;
  accent-color: #ff6b35;
}

.card-form {
  padding: 20px;
  border-top: 1px solid #e9ecef;
  background: white;
}

.card-form .form-row {
  display: grid;
  grid-template-columns: 2fr 1fr 1fr;
  gap: 15px;
  margin-bottom: 15px;
}

.card-form .form-row:first-child {
  grid-template-columns: 1fr;
}

.card-form .form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: 600;
  color: #2c3e50;
}

.card-form .form-group input {
  width: 100%;
  padding: 12px;
  border: 1px solid #e9ecef;
  border-radius: 8px;
  font-size: 16px;
  box-sizing: border-box;
}

.card-form .form-group input:focus {
  outline: none;
  border-color: #ff6b35;
}

.order-summary {
  background: linear-gradient(135deg, #fff8f5, #ffe0d6);
  padding: 25px;
  border-radius: 12px;
  margin: 30px 0;
}

.order-summary h4 {
  margin: 0 0 20px 0;
  font-size: 20px;
  color: #2c3e50;
}

.summary-item {
  display: flex;
  justify-content: space-between;
  margin-bottom: 12px;
  font-size: 16px;
}

.summary-item.total {
  border-top: 2px solid #ff6b35;
  padding-top: 15px;
  margin-top: 15px;
  font-size: 20px;
  font-weight: 700;
}

.total-price {
  color: #ff6b35;
}

.btn-complete {
  background: linear-gradient(135deg, #4caf50, #66bb6a) !important;
  font-size: 18px;
  padding: 15px 40px;
}

.btn-complete:hover {
  background: linear-gradient(135deg, #45a049, #5cb85c) !important;
}

@media (max-width: 768px) {
  .card-form .form-row {
    grid-template-columns: 1fr;
  }
  
  .option-header {
    flex-direction: column;
    text-align: center;
  }
  
  .option-icon {
    margin: 0 0 15px 0;
  }
}

  .option-header {
    flex-direction: column;
    text-align: center;
  }
  
  .option-icon {
    margin: 0 0 15px 0;
  }

</style>

