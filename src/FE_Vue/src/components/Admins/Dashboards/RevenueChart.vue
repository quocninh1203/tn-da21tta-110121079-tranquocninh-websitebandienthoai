<template>
  <div class="chart-card">
    <h3>Doanh thu theo tháng</h3>
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

const createChart = () => {
  if (!chartCanvas.value || !props.orders.length) return

  if (chartInstance) {
    chartInstance.destroy()
  }

  const revenueByMonth = Array(12).fill(0)
  props.orders.forEach(o => {
    if (
    o.orderDate &&
    o.totalPrice &&
    (o.status === 3 || o.status === 4)
    ) {
      const d = new Date(o.orderDate)
      const month = d.getMonth()
      revenueByMonth[month] += o.totalPrice
    }
  })

  chartInstance = new Chart(chartCanvas.value, {
    type: 'bar',
    data: {
      labels: [
        'Th1', 'Th2', 'Th3', 'Th4', 'Th5', 'Th6',
        'Th7', 'Th8', 'Th9', 'Th10', 'Th11', 'Th12'
      ],
      datasets: [{
        label: 'Doanh thu (VND)',
        data: revenueByMonth,
        backgroundColor: '#ff9800'
      }]
    },
    options: {
      plugins: {
        legend: { display: false }
      },
      scales: {
        y: { beginAtZero: true }
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
