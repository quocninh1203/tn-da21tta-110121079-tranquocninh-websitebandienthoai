<template>
  <div class="admin-content">
    <div class="header-row">
      <h2 class="title">Chi tiết đơn hàng</h2>
      <button class="btn back-btn" @click="goBack">
        <v-icon left style="margin-right: 6px;">mdi-arrow-left</v-icon>
        Quay lại
      </button>
    </div>
    <div v-if="order">
      <div class="order-info">
        <div><strong>Mã đơn:</strong> {{ order.orderCode }}</div>
        <div><strong>Khách hàng:</strong> {{ order.user.fullName }}</div>
        <div><strong>Ngày đặt:</strong> {{ formatDateDMY(order.orderDate) }}</div>
        <div><strong>Địa chỉ giao hàng:</strong> {{ order.shippingAddress }}</div>
        <div><strong>Phương thức thanh toán:</strong> {{ order.paymentMethod || order.method || '---' }}</div>
        <div>
          <strong>Trạng thái:</strong>
          <span :class="['status', statusClass(order.orderStatus)]">{{ statusText(order.orderStatus) }}</span>
          <select
            v-if="canChangeStatus(order.orderStatus)"
            v-model="orderStatusDraft"
            @change="onChangeStatus"
            class="status-select"
            title="Thay đổi trạng thái đơn hàng tại đây"
          >
            <option disabled value="">Cập nhật trạng thái</option>
            <option v-for="opt in getStatusOptions(order.orderStatus)" :key="opt.value" :value="opt.value">
              {{ opt.label }}
            </option>
          </select>
        </div>
        <div><strong>Tổng tiền sản phẩm:</strong> {{ formatVND(order.totalAmount - order.cost) }}</div>
        <div><strong>Tiền ship:</strong> {{ formatVND(order.cost || 0) }}</div>
        <div><strong>Tổng thanh toán:</strong> {{ formatVND((order.totalAmount || 0) + (order.shippingFee || order.shipFee || 0)) }}</div>
      </div>
      <h3 class="sub-title">Sản phẩm trong đơn hàng</h3>
      <table class="admin-table">
        <thead>
          <tr>
            <th>#</th>
            <th>Ảnh</th>
            <th>Tên sản phẩm</th>
            <th>Màu</th>
            <th>RAM/Storage</th>
            <th>Số lượng</th>
            <th>Giá</th>
            <th>Tổng</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(item, idx) in order.orderItems || []" :key="item.id">
            <td>{{ idx + 1 }}</td>
            <td>
              <img :src="item.phoneImageUrl || ''" alt="Ảnh sản phẩm" style="width:48px;height:48px;object-fit:cover;border-radius:6px;">
            </td>
            <td>{{ item.phoneName }}</td>
            <td>{{ item.colorName || item.color || '-' }}</td>
            <td>{{ (item.ram ? item.ram : '') + (item.storage ? ' / ' + item.storage : '') }}</td>
            <td>{{ item.quantity }}</td>
            <td>{{ formatVND(item.price) }}</td>
            <td>{{ formatVND(item.priceAtOrder) }}</td>
          </tr>
        </tbody>
      </table>
    </div>
    <div v-else>
      <p>Không tìm thấy đơn hàng.</p>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useOrderStore } from '@/stores/api/orderStore'
import { formatVND, formatDateDMY } from '@/utils/formatters'
import { useNotificationStore } from '@/stores/local/notification'

const orderStore = useOrderStore()
const notificationStore = useNotificationStore()
const order = ref(null)
const orderStatusDraft = ref('')
const router = useRouter()

onMounted(async () => {
  order.value = orderStore.orderDetails
  orderStatusDraft.value = ''
})

const goBack = () => {
  router.back()
}

const statusClass = (status) => {
  switch (status) {
    case 4: return 'delivered'
    case 3: return 'paid'
    case 2: return 'approved'
    case 1: return 'pending'
    case 0: return 'cancelled'
    default: return ''
  }
}

const statusText = (status) => {
  switch (status) {
    case 4: return 'Đã giao'
    case 3: return 'Đã thanh toán'
    case 2: return 'Đã duyệt'
    case 1: return 'Chờ duyệt'
    case 0: return 'Đã huỷ'
    default: return status
  }
}

function canChangeStatus(status) {
  return status === 1 || status === 2 || status === 3
}

function getStatusOptions(status) {
  if (status === 1) {
    return [
      { value: 2, label: 'Đã duyệt' },
      { value: 0, label: 'Đã huỷ' }
    ]
  }
  if (status === 2 || status === 3) {
    return [
      { value: 4, label: 'Đã giao' }
    ]
  }
  return []
}

async function onChangeStatus() {
  if (!order.value) return
  const newStatus = orderStatusDraft.value
  if (newStatus !== undefined && newStatus !== '') {
    try {
      await orderStore.changeStatus(order.value.orderId, { status: Number(newStatus) })
      notificationStore.addNotification('Cập nhật trạng thái thành công', 'success')
      await orderStore.fetchOrderDetail(order.value.orderId)
      order.value = orderStore.orderDetails
      orderStatusDraft.value = ''
    } catch (err) {
      notificationStore.addNotification('Cập nhật trạng thái thất bại', 'error')
    }
  }
}
</script>

<style scoped>
.admin-content {
  padding: 20px;
}
.header-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
}
.title {
  position: relative;
  padding-left: 16px;
  font-size: 26px;
  color: #e65100;
  margin-bottom: 20px;
  display: flex;
  align-items: center;
}
.title::before {
  content: "";
  position: absolute;
  left: 0;
  top: 50%;
  transform: translateY(-50%);
  width: 5px;
  height: 40px;
  background-color: #e65100;
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
}
.order-info {
  margin-bottom: 24px;
  background: #fff8f0;
  border-radius: 8px;
  padding: 18px 24px;
  box-shadow: 0 2px 8px #ffe0b2;
  font-size: 16px;
  line-height: 2;
}
.sub-title {
  font-size: 20px;
  font-weight: bold;
  color: #e65100;
  margin-bottom: 12px;
  margin-top: 24px;
}
.admin-table {
  width: 100%;
  border-collapse: collapse;
  background: #fff;
  box-shadow: 0 0 5px rgba(0,0,0,0.05);
  border-radius: 6px;
  overflow: hidden;
  margin-bottom: 24px;
}
.admin-table th,
.admin-table td {
  padding: 14px 16px;
  text-align: left;
  border-bottom: 1px solid #f2f2f2;
  vertical-align: middle;
}
.admin-table th {
  background-color: #ff9800;
  color: white;
  font-weight: bold;
}
.status {
  padding: 4px 10px;
  border-radius: 5px;
  font-weight: bold;
  font-size: 14px;
}
.status.delivered {
  background-color: #c8e6c9;
  color: #388e3c;
}
.status.paid {
  background-color: #b3e5fc;
  color: #0277bd;
}
.status.approved {
  background-color: #fff9c4;
  color: #fbc02d;
}
.status.pending {
  background-color: #ffe082;
  color: #f57c00;
}
.status.cancelled {
  background-color: #ffcdd2;
  color: #c62828;
}
.status-select {
  margin-left: 10px;
  padding: 4px 8px;
  border-radius: 5px;
  border: 1px solid #eee;
  font-size: 14px;
  background: #e3f2fd;
  font-weight: bold;
  color: #1976d2;
}
.status-hint {
  display: inline-flex;
  align-items: center;
  margin-left: 8px;
  background: #e3f2fd;
  color: #1976d2;
  border-radius: 4px;
  padding: 2px 8px 2px 4px;
  font-size: 13px;
  font-weight: 500;
  user-select: none;

  font-weight: 500;
  user-select: none;
}
.status-hint .hint-text {
  margin-left: 4px;
}
</style>
