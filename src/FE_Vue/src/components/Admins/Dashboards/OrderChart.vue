<template>
  <div class="chart-card">
    <h3>Đơn hàng theo trạng thái</h3>
    <canvas ref="chartCanvas"></canvas>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import Chart from 'chart.js/auto'

const props = defineProps({
  orders: {
    type: Array,
    required: true
  }
})

const chartCanvas = ref(null)
let chartInstance = null

const statusMap = {
  0: 'Đã huỷ',
  1: 'Chờ duyệt',
  2: 'Đã duyệt',
  3: 'Đã thanh toán',
  4: 'Đã giao'
}

const createChart = () => {
  if (!chartCanvas.value || !props.orders.length) return

  if (chartInstance) {
    chartInstance.destroy()
  }

  const statusCounts = [0, 0, 0, 0, 0]
  props.orders.forEach(o => {
    if (typeof o.status === 'number' && statusCounts[o.status] !== undefined) {
      statusCounts[o.status]++
    }
  })

  chartInstance = new Chart(chartCanvas.value, {
    type: 'doughnut',
    data: {
      labels: Object.values(statusMap),
      datasets: [{
        data: statusCounts,
        backgroundColor: [
          '#ffcdd2', '#ffe082', '#fff9c4', '#b3e5fc', '#c8e6c9'
        ]
      }]
    },
    options: {
      plugins: {
        legend: { position: 'bottom' }
      }
    }
  })
}

onMounted(() => {
  createChart()
})

watch(() => props.orders, () => {
  createChart()
}, { deep: true })
</script>

<style scoped>
.chart-card {
  flex: 1 1 0;
  min-width: 350px;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 2px 12px 0 #ffe0b2;
  padding: 24px 18px;
  margin-bottom: 24px;
}
.chart-card h3 {
  font-size: 1.2rem;
  font-weight: bold;
  color: #e65100;
  margin-bottom: 18px;
}
</style>
