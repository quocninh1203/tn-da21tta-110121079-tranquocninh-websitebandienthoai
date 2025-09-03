<template>
  <div class="dashboard-summary">
    <div class="summary-card orange">
      <v-icon class="summary-icon">mdi-cart</v-icon>
      <div>
        <div class="summary-value">{{ stats.orders }}</div>
        <div class="summary-label">Đơn hàng</div>
      </div>
    </div>
    <div class="summary-card blue">
      <v-icon class="summary-icon">mdi-cellphone</v-icon>
      <div>
        <div class="summary-value">{{ stats.products }}</div>
        <div class="summary-label">Sản phẩm</div>
      </div>
    </div>
    <div class="summary-card green">
      <v-icon class="summary-icon">mdi-account-group</v-icon>
      <div>
        <div class="summary-value">{{ stats.users }}</div>
        <div class="summary-label">Khách hàng</div>
      </div>
    </div>
    <div class="summary-card purple">
      <v-icon class="summary-icon">mdi-cash-multiple</v-icon>
      <div>
        <div class="summary-value">{{ formatVND(stats.revenue) }}</div>
        <div class="summary-label">Doanh thu</div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { formatVND } from '@/utils/formatters'

defineProps({
  stats: {
    type: Object,
    required: true
  }
})
</script>

<style scoped>
.dashboard-summary {
  display: flex;
  gap: 24px;
  margin-bottom: 32px;
  flex-wrap: wrap;
}
.summary-card {
  flex: 1 1 0;
  min-width: 210px;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 2px 12px 0 #ffe0b2;
  padding: 24px 18px;
  display: flex;
  align-items: center;
  gap: 18px;
}
.summary-card .summary-icon {
  font-size: 38px;
  margin-right: 10px;
}
.summary-card.orange { border-left: 6px solid #ff9800; }
.summary-card.blue { border-left: 6px solid #1976d2; }
.summary-card.green { border-left: 6px solid #388e3c; }
.summary-card.purple { border-left: 6px solid #8e24aa; }
.summary-value {
  font-size: 2rem;
  font-weight: bold;
  color: #e65100;
}
.summary-label {
  font-size: 1.1rem;
  color: #757575;
}
</style>
