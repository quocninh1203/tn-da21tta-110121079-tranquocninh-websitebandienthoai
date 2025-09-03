<template>
  <div class="report-container">
    <div class="report-header">
      <h2 class="report-title">Báo cáo chi tiết</h2>
      
      <!-- Time Filter Controls -->
      <div class="time-filters">
        <v-btn-toggle 
          v-model="selectedTimeFilter" 
          mandatory 
          class="filter-toggle"
          @update:modelValue="onTimeFilterChange"
          :disabled="isLoading"
        >
          <v-btn value="today" size="small">Hôm nay</v-btn>
          <v-btn value="month" size="small">Theo tháng</v-btn>
          <v-btn value="year" size="small">Theo năm</v-btn>
          <v-btn value="custom" size="small">Mốc thời gian</v-btn>
        </v-btn-toggle>
        
        <!-- Month-Year Filter -->
        <div v-if="selectedTimeFilter === 'month'" class="month-year-filter">
          <v-select
            v-model="selectedMonth"
            :items="monthOptions"
            label="Chọn tháng"
            variant="outlined"
            density="compact"
            class="month-select"
            :disabled="isLoading"
          />
          <v-select
            v-model="selectedYear"
            :items="yearOptions"
            label="Chọn năm"
            variant="outlined"
            density="compact"
            class="year-select"
            :disabled="isLoading"
          />
          <v-btn 
            color="primary" 
            @click="applyMonthYearFilter"
            :loading="isLoading"
            :disabled="!selectedMonth || !selectedYear"
          >
            Áp dụng
          </v-btn>
        </div>

        <!-- Year Filter -->
        <div v-if="selectedTimeFilter === 'year'" class="year-filter">
          <v-select
            v-model="selectedYearOnly"
            :items="yearOptions"
            label="Chọn năm"
            variant="outlined"
            density="compact"
            class="year-select"
            :disabled="isLoading"
          />
          <v-btn 
            color="primary" 
            @click="applyYearFilter"
            :loading="isLoading"
            :disabled="!selectedYearOnly"
          >
            Áp dụng
          </v-btn>
        </div>
        
        <!-- Custom Date Range -->
        <div v-if="selectedTimeFilter === 'custom'" class="custom-date-range">
          <v-text-field
            v-model="dateRange.from"
            type="date"
            label="Từ ngày"
            variant="outlined"
            density="compact"
            class="date-input"
            :disabled="isLoading"
          />
          <v-text-field
            v-model="dateRange.to"
            type="date"
            label="Đến ngày"
            variant="outlined"
            density="compact"
            class="date-input"
            :disabled="isLoading"
          />
          <v-btn 
            color="primary" 
            @click="applyCustomFilter"
            :loading="isLoading"
            :disabled="!dateRange.from || !dateRange.to"
          >
            Áp dụng
          </v-btn>
        </div>

        <!-- Current Filter Display -->
        <div v-if="currentFilterDisplay" class="current-filter">
          <v-chip color="primary" variant="outlined">
            <v-icon start>mdi-filter</v-icon>
            {{ currentFilterDisplay }}
          </v-chip>
        </div>
      </div>
    </div>

    <!-- Loading State -->
    <div v-if="isLoading" class="loading-container">
      <v-progress-circular indeterminate color="primary" size="64"></v-progress-circular>
      <p class="loading-text">Đang tải dữ liệu báo cáo...</p>
    </div>

    <!-- Report Content -->
    <div v-else class="report-content">
      <!-- Revenue Section -->
      <div class="report-section">
        <h3 class="section-title">
          <v-icon class="section-icon">mdi-chart-line</v-icon>
          Doanh thu
        </h3>
        <div class="data-grid">
          <div class="data-card primary">
            <div class="data-value">{{ formatVND(reportData.revenue?.totalRevenue || 0) }}</div>
            <div class="data-label">Tổng doanh thu</div>
          </div>
          <div class="data-card danger">
            <div class="data-value">{{ formatVND(reportData.revenue?.totalCancelledOrdersAmount || 0) }}</div>
            <div class="data-label">Giá trị đơn hủy</div>
          </div>
          <div class="data-card info">
            <div class="data-value">{{ reportData.revenue?.totalOrders || 0 }}</div>
            <div class="data-label">Tổng đơn hàng</div>
          </div>
          <div class="data-card success">
            <div class="data-value">{{ reportData.revenue?.completedOrders || 0 }}</div>
            <div class="data-label">Đơn hoàn thành</div>
          </div>
          <div class="data-card warning">
            <div class="data-value">{{ reportData.revenue?.pendingOrders || 0 }}</div>
            <div class="data-label">Đơn chờ xử lý</div>
          </div>
          <div class="data-card error">
            <div class="data-value">{{ reportData.revenue?.cancelledOrders || 0 }}</div>
            <div class="data-label">Đơn đã hủy</div>
          </div>
        </div>
      </div>

      <!-- Products Section -->
      <div class="report-section">
        <h3 class="section-title">
          <v-icon class="section-icon">mdi-package-variant</v-icon>
          Sản phẩm
        </h3>
        <div class="products-grid">
          <div class="product-highlight">
            <h4>Sản phẩm bán chạy nhất</h4>
            <div class="product-card">
              <div class="product-image">
                <img 
                  v-if="reportData.products?.mostSoldProductImageUrl" 
                  :src="reportData.products.mostSoldProductImageUrl" 
                  :alt="reportData.products.mostSoldProductName"
                />
                <v-icon v-else size="48">mdi-image-off</v-icon>
              </div>
              <div class="product-info">
                <div class="product-name">
                  {{ reportData.products?.mostSoldProductName || 'Chưa có dữ liệu' }}
                </div>
                <div class="product-stat">
                  Đã bán: <strong>{{ reportData.products?.totalSoldQuantity || 0 }}</strong> sản phẩm
                </div>
              </div>
            </div>
          </div>
          
          <div class="product-highlight">
            <h4>Sản phẩm doanh thu cao nhất</h4>
            <div class="product-card">
              <div class="product-image">
                <img 
                  v-if="reportData.products?.topRevenueProductImageUrl" 
                  :src="reportData.products.topRevenueProductImageUrl" 
                  :alt="reportData.products.topRevenueProductName"
                />
                <v-icon v-else size="48">mdi-image-off</v-icon>
              </div>
              <div class="product-info">
                <div class="product-name">
                  {{ reportData.products?.topRevenueProductName || 'Chưa có dữ liệu' }}
                </div>
                <div class="product-stat">
                  Doanh thu: <strong>{{ formatVND(reportData.products?.totalRevenue || 0) }}</strong>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Customer Section -->
      <div class="report-section">
        <h3 class="section-title">
          <v-icon class="section-icon">mdi-account-star</v-icon>
          Khách hàng VIP
        </h3>
        <div class="customer-card">
          <div class="customer-info">
            <div class="customer-avatar">
              <v-icon size="32">mdi-account-circle</v-icon>
            </div>
            <div class="customer-details">
              <div class="customer-name">
                {{ reportData.customers?.customerName || 'Chưa có dữ liệu' }}
              </div>
              <div class="customer-contact">
                <span v-if="reportData.customers?.phoneNumber">
                  <v-icon size="16">mdi-phone</v-icon> {{ reportData.customers.phoneNumber }}
                </span>
                <span v-if="reportData.customers?.email">
                  <v-icon size="16">mdi-email</v-icon> {{ reportData.customers.email }}
                </span>
              </div>
            </div>
          </div>
          <div class="customer-stats">
            <div class="stat-item">
              <div class="stat-value">{{ reportData.customers?.totalItemsPurchased || 0 }}</div>
              <div class="stat-label">Sản phẩm đã mua</div>
            </div>
            <div class="stat-item">
              <div class="stat-value">{{ formatVND(reportData.customers?.totalPrice || 0) }}</div>
              <div class="stat-label">Tổng chi tiêu</div>
            </div>
          </div>
        </div>
      </div>

      <!-- Payment Methods Section -->
      <div class="report-section">
        <h3 class="section-title">
          <v-icon class="section-icon">mdi-credit-card</v-icon>
          Phương thức thanh toán phổ biến
        </h3>
        <div class="payment-card">
          <div class="payment-method">
            <v-icon class="payment-icon">mdi-wallet</v-icon>
            <div class="payment-details">
              <div class="payment-name">
                {{ reportData.paymentMethods?.paymentMethod || 'Chưa có dữ liệu' }}
              </div>
              <div class="payment-stats">
                <span class="payment-orders">{{ reportData.paymentMethods?.totalOrders || 0 }} đơn hàng</span>
                <span class="payment-revenue">{{ formatVND(reportData.paymentMethods?.totalRevenue || 0) }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { formatVND } from '@/utils/formatters'

const props = defineProps({
  reportData: {
    type: Object,
    required: true
  },
  isLoading: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['timeFilterChanged'])

const selectedTimeFilter = ref('today')
const selectedMonth = ref(null)
const selectedYear = ref(null)
const selectedYearOnly = ref(null)
const dateRange = ref({
  from: '',
  to: ''
})

// Generate month options
const monthOptions = [
  { title: 'Tháng 1', value: 1 },
  { title: 'Tháng 2', value: 2 },
  { title: 'Tháng 3', value: 3 },
  { title: 'Tháng 4', value: 4 },
  { title: 'Tháng 5', value: 5 },
  { title: 'Tháng 6', value: 6 },
  { title: 'Tháng 7', value: 7 },
  { title: 'Tháng 8', value: 8 },
  { title: 'Tháng 9', value: 9 },
  { title: 'Tháng 10', value: 10 },
  { title: 'Tháng 11', value: 11 },
  { title: 'Tháng 12', value: 12 }
]

// Generate year options (last 5 years + current year + next year)
const yearOptions = computed(() => {
  const currentYear = new Date().getFullYear()
  const years = []
  for (let i = currentYear - 5; i <= currentYear + 1; i++) {
    years.push({ title: `Năm ${i}`, value: i })
  }
  return years
})

// Current filter display
const currentFilterDisplay = computed(() => {
  switch (selectedTimeFilter.value) {
    case 'today':
      return 'Hôm nay'
    case 'month':
      if (selectedMonth.value && selectedYear.value) {
        return `Tháng ${selectedMonth.value}/${selectedYear.value}`
      }
      return null
    case 'year':
      if (selectedYearOnly.value) {
        return `Năm ${selectedYearOnly.value}`
      }
      return null
    case 'custom':
      if (dateRange.value.from && dateRange.value.to) {
        return `${formatDate(dateRange.value.from)} - ${formatDate(dateRange.value.to)}`
      }
      return null
    default:
      return null
  }
})

const formatDate = (dateString) => {
  const date = new Date(dateString)
  return date.toLocaleDateString('vi-VN')
}

const onTimeFilterChange = (value) => {
  if (value === 'today') {
    emit('timeFilterChanged', { type: value })
  }
  // For other filters, wait for user to apply manually
}

const applyMonthYearFilter = () => {
  if (selectedMonth.value && selectedYear.value) {
    emit('timeFilterChanged', {
      type: 'month',
      month: selectedMonth.value,
      year: selectedYear.value
    })
  }
}

const applyYearFilter = () => {
  if (selectedYearOnly.value) {
    emit('timeFilterChanged', {
      type: 'year',
      year: selectedYearOnly.value
    })
  }
}

const applyCustomFilter = () => {
  if (dateRange.value.from && dateRange.value.to) {
    emit('timeFilterChanged', {
      type: 'custom',
      from: dateRange.value.from,
      to: dateRange.value.to
    })
  }
}

onMounted(() => {
  // Set default values
  const now = new Date()
  selectedMonth.value = now.getMonth() + 1
  selectedYear.value = now.getFullYear()
  selectedYearOnly.value = now.getFullYear()
  
  // Set default date range for custom filter
  const oneWeekAgo = new Date(now.getTime() - 7 * 24 * 60 * 60 * 1000)
  dateRange.value.from = oneWeekAgo.toISOString().split('T')[0]
  dateRange.value.to = now.toISOString().split('T')[0]
})
</script>

<style scoped>
.report-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 24px;
}

.report-header {
  margin-bottom: 32px;
}

.report-title {
  font-size: 2rem;
  font-weight: bold;
  color: #e65100;
  margin-bottom: 20px;
}

.time-filters {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.filter-toggle {
  align-self: flex-start;
}

.month-year-filter,
.year-filter {
  display: flex;
  gap: 16px;
  align-items: end;
  flex-wrap: wrap;
}

.month-select,
.year-select {
  min-width: 140px;
}

.custom-date-range {
  display: flex;
  gap: 16px;
  align-items: end;
  flex-wrap: wrap;
}

.date-input {
  min-width: 160px;
}

.report-content {
  display: flex;
  flex-direction: column;
  gap: 32px;
}

.report-section {
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 2px 12px 0 #ffe0b2;
  padding: 24px;
}

.section-title {
  display: flex;
  align-items: center;
  gap: 12px;
  font-size: 1.4rem;
  font-weight: bold;
  color: #e65100;
  margin-bottom: 20px;
}

.section-icon {
  color: #f57c00;
}

.data-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 16px;
}

.data-card {
  background: #f8f9fa;
  border-radius: 8px;
  padding: 20px;
  text-align: center;
  border-left: 4px solid;
}

.data-card.primary { border-left-color: #1976d2; }
.data-card.success { border-left-color: #388e3c; }
.data-card.warning { border-left-color: #f57c00; }
.data-card.error { border-left-color: #d32f2f; }
.data-card.info { border-left-color: #0288d1; }
.data-card.danger { border-left-color: #c62828; }

.data-value {
  font-size: 1.5rem;
  font-weight: bold;
  color: #333;
  margin-bottom: 8px;
}

.data-label {
  font-size: 0.9rem;
  color: #666;
}

.products-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(350px, 1fr));
  gap: 24px;
}

.product-highlight h4 {
  color: #e65100;
  margin-bottom: 12px;
  font-weight: 600;
}

.product-card {
  display: flex;
  gap: 16px;
  padding: 16px;
  background: #f8f9fa;
  border-radius: 8px;
  align-items: center;
}

.product-image {
  width: 64px;
  height: 64px;
  border-radius: 8px;
  overflow: hidden;
  background: #e0e0e0;
  display: flex;
  align-items: center;
  justify-content: center;
}

.product-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.product-info {
  flex: 1;
}

.product-name {
  font-weight: 600;
  color: #333;
  margin-bottom: 4px;
}

.product-stat {
  color: #666;
  font-size: 0.9rem;
}

.customer-card {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px;
  background: #f8f9fa;
  border-radius: 8px;
  flex-wrap: wrap;
  gap: 16px;
}

.customer-info {
  display: flex;
  gap: 16px;
  align-items: center;
}

.customer-avatar {
  color: #f57c00;
}

.customer-name {
  font-weight: 600;
  color: #333;
  margin-bottom: 4px;
}

.customer-contact {
  display: flex;
  flex-direction: column;
  gap: 4px;
  font-size: 0.9rem;
  color: #666;
}

.customer-contact span {
  display: flex;
  align-items: center;
  gap: 4px;
}

.customer-stats {
  display: flex;
  gap: 24px;
}

.stat-item {
  text-align: center;
}

.stat-value {
  font-size: 1.2rem;
  font-weight: bold;
  color: #333;
}

.stat-label {
  font-size: 0.8rem;
  color: #666;
}

.payment-card {
  padding: 20px;
  background: #f8f9fa;
  border-radius: 8px;
}

.payment-method {
  display: flex;
  gap: 16px;
  align-items: center;
}

.payment-icon {
  font-size: 32px;
  color: #f57c00;
}

.payment-name {
  font-weight: 600;
  color: #333;
  margin-bottom: 8px;
}

.payment-stats {
  display: flex;
  gap: 16px;
  flex-wrap: wrap;
}

.payment-orders {
  color: #666;
  font-size: 0.9rem;
}

.payment-revenue {
  font-weight: 600;
  color: #e65100;
}

.loading-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 300px;
  gap: 16px;
}

.loading-text {
  color: #666;
  font-size: 1.1rem;
  margin: 0;
}

@media (max-width: 768px) {
  .custom-date-range {
    flex-direction: column;
    align-items: stretch;
  }
  
  .customer-card {
    flex-direction: column;
    text-align: center;
  }
  
  .customer-stats {
    justify-content: center;
  }
}
</style>
