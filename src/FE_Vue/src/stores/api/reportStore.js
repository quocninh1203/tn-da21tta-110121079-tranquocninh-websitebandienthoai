// src/stores/reportStore.js

import { defineStore } from 'pinia';
import reportService from '@/services/reportService';

export const useReportStore = defineStore('report', {
  state: () => ({
    reportData: null,
    revenueForecast: null,
    topProductsForecast: [],
    isLoading: false,
    error: null
  }),

  actions: {
    async fetchTodayReport() {
      this.isLoading = true;
      try {
        const res = await reportService.getTodayReport();
        this.reportData = res.data;
      } catch (err) {
        console.error("Lỗi khi lấy báo cáo hôm nay:", err);
        this.reportData = null;
        this.error = err;
      } finally {
        this.isLoading = false;
      }
    },

    async fetchMonthlyReport(month, year) {
      this.isLoading = true;
      try {
        const res = await reportService.getMonthlyReport(month, year);
        this.reportData = res.data;
      } catch (err) {
        console.error("Lỗi khi lấy báo cáo theo tháng:", err);
        this.reportData = null;
        this.error = err;
      } finally {
        this.isLoading = false;
      }
    },

    async fetchYearlyReport(year) {
      this.isLoading = true;
      try {
        const res = await reportService.getYearlyReport(year);
        this.reportData = res.data;
      } catch (err) {
        console.error("Lỗi khi lấy báo cáo theo năm:", err);
        this.reportData = null;
        this.error = err;
      } finally {
        this.isLoading = false;
      }
    },

    async fetchRangeReport(startDate, endDate) {
      this.isLoading = true;
      try {
        const res = await reportService.getRangeReport(startDate, endDate);
        this.reportData = res.data;
      } catch (err) {
        console.error("Lỗi khi lấy báo cáo theo khoảng thời gian:", err);
        this.reportData = null;
        this.error = err;
      } finally {
        this.isLoading = false;
      }
    },

    /**
     * Gọi dự đoán doanh thu tháng tới
     */
    async fetchRevenueForecast() {
      this.isLoading = true;
      try {
        const res = await reportService.getRevenueForecast();
        this.revenueForecast = res.data;
      } catch (err) {
        console.error("Lỗi khi lấy dự đoán doanh thu:", err);
        this.revenueForecast = null;
        this.error = err;
      } finally {
        this.isLoading = false;
      }
    },

    /**
     * Gọi dự đoán sản phẩm bán chạy
     */
    async fetchTopSellingProducts() {
      this.isLoading = true;
      try {
        const res = await reportService.getTopSellingProducts();
        this.topProductsForecast = res.data;
      } catch (err) {
        console.error("Lỗi khi lấy sản phẩm bán chạy:", err);
        this.topProductsForecast = [];
        this.error = err;
      } finally {
        this.isLoading = false;
      }
    },

    /**
     * Xóa toàn bộ dữ liệu
     */
    clearReport() {
      this.reportData = null;
      this.revenueForecast = null;
      this.topProductsForecast = [];
      this.error = null;
    }
  }
});
