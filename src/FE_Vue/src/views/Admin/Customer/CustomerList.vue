<template>
  <div class="customer-list-page">
    <h1 class="page-title">Quản lý người dùng</h1>
    <div class="toolbar">
      <input v-model="search" class="search-input" placeholder="Tìm kiếm theo tên, email..." />
      <button class="btn-refresh" @click="fetchCustomers">Làm mới</button>
    </div>
    <div class="table-wrapper">
      <table class="customer-table">
        <thead>
          <tr>
            <th>STT</th>
            <th>Họ tên</th>
            <th>Email</th>
            <th>Số điện thoại</th>
            <th>Thao tác</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(customer, idx) in filteredCustomers" :key="customer.id">
            <td>{{ idx + 1 }}</td>
            <td>{{ customer.fullName }}</td>
            <td>{{ customer.email }}</td>
            <td>{{ customer.phoneNumber }}</td>
            <td>
              <button class="btn-action" @click="viewOrders(customer)">
                Xem đơn hàng
              </button>
            </td>
          </tr>
          <tr v-if="filteredCustomers.length === 0">
            <td colspan="5" class="no-data">Không tìm thấy người dùng phù hợp.</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useUserStore } from '@/stores/api/userStore'
import { useRouter } from 'vue-router'
const userStore = useUserStore()
const router = useRouter()

const search = ref('')

const fetchCustomers = async () => {
  await userStore.fetchAllUsers()
}

const customers = computed(() => userStore.allUsers || [])

const filteredCustomers = computed(() => {
  if (!search.value) return customers.value
  const s = search.value.toLowerCase()
  return customers.value.filter(c =>
    (c.fullName || '').toLowerCase().includes(s) ||
    (c.email || '').toLowerCase().includes(s) ||
    (c.phone || '').includes(s)
  )
})

const viewOrders = (customer) => {
  router.push({ name: 'OrderList', query: { userId: customer.id } })
}

onMounted(() => {
  fetchCustomers()
})
</script>

<style scoped>
.customer-list-page {
  max-width: 1000px;
  margin: 0 auto;
  padding: 32px 18px;
  background: #fff;
  border-radius: 18px;
  box-shadow: 0 4px 24px #e0e0e0;
  font-family: 'Inter', 'Segoe UI', sans-serif;
}
.page-title {
  font-size: 2rem;
  font-weight: 700;
  color: #ff5722;
  margin-bottom: 24px;
  text-align: center;
}
.toolbar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 18px;
  gap: 12px;
}
.search-input {
  padding: 8px 14px;
  border-radius: 8px;
  border: 1.5px solid #ffccbc;
  font-size: 1rem;
  width: 260px;
  background: #fff8f0;
  color: #e65100;
}
.btn-refresh {
  background: linear-gradient(90deg, #ff9800 60%, #ff5722 100%);
  color: #fff;
  font-weight: bold;
  border: none;
  border-radius: 8px;
  padding: 8px 22px;
  font-size: 1rem;
  cursor: pointer;
  transition: background 0.2s;
}
.btn-refresh:hover {
  background: linear-gradient(90deg, #e65100 60%, #ff9800 100%);
}
.table-wrapper {
  overflow-x: auto;
}
.customer-table {
  width: 100%;
  border-collapse: collapse;
  background: #fff;
}
.customer-table th, .customer-table td {
  padding: 12px 10px;
  border-bottom: 1px solid #eee;
  text-align: left;
}
.customer-table th {
  background: #fff3e0;
  color: #e65100;
  font-weight: 600;
}
.btn-action {
  background: #fff;
  color: #1976d2;
  border: 1.5px solid #1976d2;
  border-radius: 8px;
  padding: 6px 14px;
  font-size: 0.98rem;
  font-weight: 500;
  cursor: pointer;
  margin-right: 6px;
  transition: background 0.2s, color 0.2s, border 0.2s;
}
.btn-action:hover {
  background: #e3f0ff;
  color: #0d47a1;
  border-color: #0d47a1;
}
.no-data {
  text-align: center;
  color: #888;
  font-style: italic;
  padding: 4px 12px;
  border-radius: 8px;
  font-size: 0.98rem;
  font-weight: 600;
}
.status.active {
  background: #e8f5e9;
  color: #43a047;
}
.status.inactive {
  background: #ffebee;
  color: #e53935;
}
.btn-action {
  background: #fff;
  color: #1976d2;
  border: 1.5px solid #1976d2;
  border-radius: 8px;
  padding: 6px 14px;
  font-size: 0.98rem;
  font-weight: 500;
  cursor: pointer;
  margin-right: 6px;
  transition: background 0.2s, color 0.2s, border 0.2s;
}
.btn-action:hover {
  background: #e3f0ff;
  color: #0d47a1;
  border-color: #0d47a1;
}
.btn-danger {
  color: #e53935;
  border-color: #e53935;
}
.btn-danger:hover {
  background: #ffebee;
  color: #b71c1c;
  border-color: #b71c1c;
}
.no-data {
  text-align: center;
  color: #888;
  font-style: italic;
  text-align: center;
  color: #888;
  font-style: italic;
}
</style>
