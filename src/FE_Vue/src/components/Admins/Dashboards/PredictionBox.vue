<template>
  <div class="prediction-section">
    <h3 class="section-title">Dự đoán tháng tiếp theo</h3>
    
    <!-- Loading State -->
    <div v-if="isLoading" class="prediction-loading">
      <v-progress-circular indeterminate color="primary" size="32"></v-progress-circular>
      <span class="loading-text">Đang tải dự đoán...</span>
    </div>

    <!-- Prediction Cards -->
    <div v-else class="prediction-cards">
      <div class="prediction-card revenue-prediction">
        <v-icon class="prediction-icon">mdi-trending-up</v-icon>
        <div>
          <div class="prediction-title">Dự đoán doanh thu</div>
          <div class="prediction-range">
            <div class="range-item high">
              <span class="range-label">Cao nhất:</span>
              <span class="range-value">{{ formatVND(predictions.revenueHigh) }}</span>
            </div>
            <div class="range-item low">
              <span class="range-label">Thấp nhất:</span>
              <span class="range-value">{{ formatVND(predictions.revenueLow) }}</span>
            </div>
          </div>
        </div>
      </div>
      
      <div class="prediction-card products-prediction">
        <v-icon class="prediction-icon">mdi-star-circle</v-icon>
        <div>
          <div class="prediction-title">Sản phẩm bán chạy dự kiến</div>
          <div class="hot-products">
            <div 
              v-for="(product, index) in predictions.hotProducts" 
              :key="product.id || index"
              class="hot-product-item"
            >
              <span class="product-rank">#{{ index + 1 }}</span>
              <span class="product-name">{{ product.phoneName || 'Sản phẩm' }}</span>
              <span class="product-score">{{ product.predictedSold || 0 }}</span>
            </div>
            <div v-if="predictions.hotProducts.length === 0" class="no-prediction">
              Chưa đủ dữ liệu để dự đoán
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { formatVND } from '@/utils/formatters'

defineProps({
  predictions: {
    type: Object,
    required: true
  },
  isLoading: {
    type: Boolean,
    default: false
  }
})
</script>

<style scoped>
.prediction-section {
  margin-bottom: 32px;
}
.section-title {
  font-size: 1.8rem;
  font-weight: bold;
  color: #e65100;
  margin-bottom: 20px;
  padding-left: 16px;
  position: relative;
}
.section-title::before {
  content: "";
  position: absolute;
  left: 0;
  top: 50%;
  transform: translateY(-50%);
  width: 4px;
  height: 24px;
  background-color: #e65100;
}
.prediction-cards {
  display: flex;
  gap: 24px;
  flex-wrap: wrap;
}
.prediction-card {
  flex: 1 1 0;
  min-width: 320px;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 2px 12px 0 #ffe0b2;
  padding: 24px 18px;
  display: flex;
  align-items: flex-start;
  gap: 18px;
  border-left: 6px solid #f57c00;
}
.prediction-icon {
  font-size: 32px;
  color: #f57c00;
  margin-top: 4px;
}
.prediction-title {
  font-size: 1.3rem;
  font-weight: bold;
  color: #e65100;
  margin-bottom: 12px;
}
.prediction-range {
  display: flex;
  flex-direction: column;
  gap: 8px;
}
.range-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 8px 12px;
  border-radius: 8px;
  background: #fafafa;
}
.range-item.high {
  border-left: 4px solid #4caf50;
}
.range-item.low {
  border-left: 4px solid #ff9800;
}
.range-label {
  font-weight: 500;
  color: #666;
}
.range-value {
  font-weight: bold;
  color: #e65100;
}
.hot-products {
  display: flex;
  flex-direction: column;
  gap: 8px;
}
.hot-product-item {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 10px 12px;
  background: #fafafa;
  border-radius: 8px;
  border-left: 4px solid #2196f3;
}
.product-rank {
  font-weight: bold;
  color: #f57c00;
  min-width: 24px;
}
.product-name {
  flex: 1;
  color: #333;
  font-weight: 500;
}
.product-score {
  font-weight: bold;
  color: #4caf50;
  font-size: 0.9rem;
}
.no-prediction {
  color: #999;
  font-style: italic;
  text-align: center;
  padding: 16px;
}
.prediction-loading {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 40px;
  justify-content: center;
}
.loading-text {
  color: #666;
  font-size: 1rem;
}
</style>
