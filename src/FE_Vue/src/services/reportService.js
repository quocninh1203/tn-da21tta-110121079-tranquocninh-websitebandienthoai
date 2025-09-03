// src/services/reportService.js

import api from '@/services/axiosInstance';

const reportService = {
  getTodayReport() {
    return api.get('/report/today');
  },

  getMonthlyReport(month, year) {
    return api.get('/report/month', {
      params: { month, year }
    });
  },

  getYearlyReport(year) {
    return api.get('/report/year', {
      params: { year }
    });
  },

  getRangeReport(startDate, endDate) {
    return api.get('/report/range', {
      params: { startDate, endDate }
    });
  },

  /**
   * Dự đoán doanh thu tháng tới
   */
  getRevenueForecast() {
    return api.get('/ForecastAPI/revenue');
  },

  /**
   * Dự đoán 10 sản phẩm bán chạy
   */
  getTopSellingProducts() {
    return api.get('/ForecastAPI/top-products');
  }
};

export default reportService;
