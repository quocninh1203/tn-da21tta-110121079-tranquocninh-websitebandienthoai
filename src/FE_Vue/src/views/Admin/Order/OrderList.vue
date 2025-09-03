<template>
  <div class="admin-content">
    <h2 class="title">Danh sách đơn hàng</h2>
    <div class="filter-row">
      <div class="filter-toggle">
        <button
          type="button"
          class="toggle-btn"
          :class="{ active: filterMode === 'date' }"
          @click="filterMode = 'date'"
        >
          Lọc theo ngày
        </button>
        <button
          type="button"
          class="toggle-btn"
          :class="{ active: filterMode === 'month' }"
          @click="filterMode = 'month'"
        >
          Lọc theo tháng
        </button>
      </div>
      <template v-if="filterMode === 'date'">
        <label>
          Từ ngày:
          <input type="date" v-model="filterFromDate" class="filter-input" />
        </label>
        <label>
          Đến ngày:
          <input type="date" v-model="filterToDate" class="filter-input" />
        </label>
      </template>
      <template v-else>
        <label>
          Năm:
          <select v-model="filterYear" class="filter-input">
            <option value="">Tất cả</option>
            <option v-for="y in years" :key="y" :value="y">{{ y }}</option>
          </select>
        </label>
        <label v-if="filterYear">
          Tháng:
          <select v-model="filterMonth" class="filter-input">
            <option value="">Tất cả</option>
            <option v-for="m in 12" :key="m" :value="m">{{ m }}</option>
          </select>
        </label>
      </template>
      <button class="btn filter-btn" @click="clearFilter">Xoá lọc</button>
      <div class="filter-status">
        <label for="statusFilter">Trạng thái:</label>
        <select id="statusFilter" v-model="filterStatus" class="filter-input">
          <option value="">Tất cả</option>
          <option value="Đã huỷ">Đã huỷ</option>
          <option value="Chờ duyệt">Chờ duyệt</option>
          <option value="Đã duyệt">Đã duyệt</option>
          <option value="Đã thanh toán">Đã thanh toán</option>
          <option value="Đã giao">Đã giao</option>
        </select>
      </div>
    </div>
    <table class="admin-table">
      <thead>
        <tr>
          <th>Mã đơn</th>
          <th>Khách hàng</th>
          <th>Ngày đặt</th>
          <th>Tổng tiền</th>
          <th>Trạng thái</th>
          <th>Thao tác</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="order in [...filteredOrders].reverse()" :key="order.id">
          <td>{{ order.orderCode }}</td>
          <td>#</td>
          <td>{{ formatDateDMY(order.orderDate) }}</td>
          <td>{{ formatVND(order.totalPrice) }}</td>
          <td>
            <span :class="['status', statusClass(order.status)]">{{ statusText(order.status) }}</span>
          </td>
          <td>
            <button class="btn view" @click="viewOrder(order.id, order.orderCode)">Xem</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router';
import { useNotificationStore } from '@/stores/local/notification'
import { formatVND, formatDateDMY } from '@/utils/formatters'
import { useOrderStore } from '@/stores/api/orderStore'

const notificationStore = useNotificationStore()
const orderStore = useOrderStore()
const router = useRouter()

onMounted(() => {
  orderStore.fetchAllOrders()
})

const orders = computed(() => orderStore.orders || [])

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

const viewOrder = async (id, orderCode) => {
  await orderStore.fetchOrderDetail(id)
  router.push({ name: 'adminOrderDetail', params: { code: orderCode } })
}

const filterMode = ref('date')
const filterFromDate = ref('')
const filterToDate = ref('')
const filterMonth = ref('')
const filterYear = ref('')
const filterStatus = ref('')

const years = computed(() => {
  const allYears = orders.value.map(o => new Date(o.orderDate).getFullYear())
  return Array.from(new Set(allYears)).sort((a, b) => b - a)
})

const filteredOrders = computed(() => {
  let result = []
  if (filterMode.value === 'date') {
    if (filterFromDate.value && filterToDate.value) {
      const from = new Date(filterFromDate.value)
      const to = new Date(filterToDate.value)
      if (to < from) {
        notificationStore.addNotification(
          'Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.',
          'error',
        )
        return []
      }
    }
    result = orders.value.filter(order => {
      const orderDate = new Date(order.orderDate)
      if (filterFromDate.value) {
        const from = new Date(filterFromDate.value)
        if (orderDate < from) return false
      }
      if (filterToDate.value) {
        const to = new Date(filterToDate.value)
        if (orderDate > to) return false
      }
      return true
    })
  } else {
    result = orders.value.filter(order => {
      const orderDate = new Date(order.orderDate)
      if (filterYear.value && orderDate.getFullYear() !== parseInt(filterYear.value)) return false
      if (filterMonth.value && (orderDate.getMonth() + 1) !== parseInt(filterMonth.value)) return false
      return true
    })
  }
  if (filterStatus.value) {
    result = result.filter(order => statusText(order.status) === filterStatus.value)
  }
  return result
})

function clearFilter() {
  filterFromDate.value = ''
  filterToDate.value = ''
  filterMonth.value = ''
  filterYear.value = ''
  filterStatus.value = ''
}
</script>

<style scoped>
.admin-content {
  padding: 20px;
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

.filter-row {
  display: flex;
  align-items: center;
  gap: 18px;
  margin-bottom: 18px;
}

.filter-toggle {
  display: flex;
  gap: 0;
}

.toggle-btn {
  padding: 8px 18px;
  border: none;
  border-radius: 5px 0 0 5px;
  background: #eee;
  color: #1976d2;
  font-weight: bold;
  font-size: 15px;
  cursor: pointer;
  transition: background 0.2s, color 0.2s;
  outline: none;
}

.toggle-btn:last-child {
  border-radius: 0 5px 5px 0;
  border-left: 1px solid #ddd;
}

.toggle-btn.active {
  background: #1976d2;
  color: #fff;
}

.filter-input {
  padding: 6px 10px;
  border-radius: 5px;
  border: 1px solid #eee;
  font-size: 15px;
  margin-left: 6px;
}

.filter-btn {
  background: #ff9800;
  color: #fff;
  font-weight: bold;
  border: none;
  border-radius: 5px;
  padding: 7px 16px;
  cursor: pointer;
  transition: background 0.2s;
}

.filter-btn:hover {
  background: #e65100;
}

.admin-table {
  width: 100%;
  border-collapse: collapse;
  background: #fff;
  box-shadow: 0 0 5px rgba(0,0,0,0.05);
  border-radius: 6px;
  overflow: hidden;
}

.admin-table th {
  background-color: #ff9800;
  color: white;
  font-weight: bold;
  padding: 14px 16px;
  text-align: left;
  border-bottom: 1px solid #f2f2f2;
  vertical-align: middle;
}

.admin-table td {
  padding: 12px 15px;
  text-align: left;
  border-bottom: 1px solid #eee;
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

.btn {
  padding: 6px 12px;
  border: none;
  border-radius: 4px;
  color: white;
  font-weight: 500;
  cursor: pointer;
}

.btn.view {
  background-color: #7b1fa2;
}

.filter-status {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-left: auto;
}
</style>
