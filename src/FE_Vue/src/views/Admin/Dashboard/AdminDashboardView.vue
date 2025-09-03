<template>
  <div class="dashboard-container">
    <h2 class="dashboard-title">Dashboard Quản trị</h2>
    
    <DashboardSummary :stats="stats" />
    
    <PredictionBox 
      :predictions="predictionsData" 
      :isLoading="reportStore.isLoading"
    />

    <div class="dashboard-charts">
      <OrderChart :orders="orders" />
      <RevenueChart :orders="orders" />
    </div>

    <!-- Report Detail Section -->
    <ReportDetail 
      :reportData="reportStore.reportData || defaultReportData" 
      :isLoading="reportStore.isLoading"
      @timeFilterChanged="handleTimeFilterChange"
    />
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useOrderStore } from '@/stores/api/orderStore'
import { usePhoneStore } from '@/stores/api/phoneStore'
import { useUserStore } from '@/stores/api/userStore'
import { useReportStore } from '@/stores/api/reportStore'
import DashboardSummary from '@/components/Admins/Dashboards/DashboardSummary.vue'
import PredictionBox from '@/components/Admins/Dashboards/PredictionBox.vue'
import OrderChart from '@/components/Admins/Dashboards/OrderChart.vue'
import RevenueChart from '@/components/Admins/Dashboards/RevenueChart.vue'
import ReportDetail from '@/components/Admins/Dashboards/ReportDetail.vue'

const orderStore = useOrderStore()
const phoneStore = usePhoneStore()
const userStore = useUserStore()
const reportStore = useReportStore()

const stats = ref({
  orders: 0,
  products: 0,
  users: 0,
  revenue: 0
})

const orders = ref([])

// Computed predictions data from reportStore
const predictionsData = computed(() => ({
  revenueHigh: reportStore.revenueForecast?.maxRevenue || 0,
  revenueLow: reportStore.revenueForecast?.minRevenue || 0,
  hotProducts: reportStore.topProductsForecast || []
}))

// Default report data structure
const defaultReportData = {
  revenue: {
    totalRevenue: 0,
    totalCancelledOrdersAmount: 0,
    totalOrders: 0,
    completedOrders: 0,
    cancelledOrders: 0,
    pendingOrders: 0
  },
  products: {
    mostSoldProductName: null,
    mostSoldProductImageUrl: null,
    totalSoldQuantity: 0,
    topRevenueProductName: null,
    topRevenueProductImageUrl: null,
    totalRevenue: 0
  },
  customers: {
    customerName: null,
    phoneNumber: null,
    email: null,
    totalItemsPurchased: 0,
    totalPrice: 0
  },
  paymentMethods: {
    paymentMethod: null,
    totalOrders: 0,
    totalRevenue: 0
  }
}

// Xử lý thay đổi bộ lọc thời gian
const handleTimeFilterChange = async (filter) => {
  try {
    const now = new Date()
    
    switch (filter.type) {
      case 'today':
        await reportStore.fetchTodayReport()
        break
        
      case 'month':
        const month = filter.month || (now.getMonth() + 1)
        const year = filter.year || now.getFullYear()
        await reportStore.fetchMonthlyReport(month, year)
        break
        
      case 'year':
        const reportYear = filter.year || now.getFullYear()
        await reportStore.fetchYearlyReport(reportYear)
        break
        
      case 'custom':
        if (filter.from && filter.to) {
          await reportStore.fetchRangeReport(filter.from, filter.to)
        }
        break
        
      default:
        await reportStore.fetchTodayReport()
    }
  } catch (error) {
    console.error('Lỗi khi lấy dữ liệu báo cáo:', error)
  }
}

onMounted(async () => {
  await Promise.all([
    orderStore.fetchAllOrders(),
    phoneStore.fetchAllPhones(),
    userStore.fetchAllUsers?.()
  ])
  
  const orderList = orderStore.orders || []
  const products = phoneStore.phoneList || []
  const users = userStore.allUsers || []

  orders.value = orderList
  
  stats.value.orders = orderList.length
  stats.value.products = products.length
  stats.value.users = users.length
  stats.value.revenue = orderList.reduce((sum, o) => sum + (o.totalPrice || 0), 0)

  // Fetch predictions and report data from reportStore
  await Promise.all([
    reportStore.fetchRevenueForecast(),
    reportStore.fetchTopSellingProducts(),
    reportStore.fetchTodayReport()
  ])
})
</script>

<style scoped>
.dashboard-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 32px 0;
}

.dashboard-title {
  font-size: 2.2rem;
  font-weight: bold;
  color: #e65100;
  margin-bottom: 28px;
  padding-left: 16px;
  position: relative;
}

.dashboard-title::before {
  content: "";
  position: absolute;
  left: 0;
  top: 50%;
  transform: translateY(-50%);
  width: 5px;
  height: 40px;
  background-color: #e65100;
}

.dashboard-charts {
  display: flex;
  gap: 32px;
  flex-wrap: wrap;
}
</style>

