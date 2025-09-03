// src/stores/paymentMethodStore.js

import { defineStore } from 'pinia';
import paymentMethodService from '@/services/paymentMethodService';

export const usePaymentMethodStore = defineStore('paymentMethod', {
  state: () => ({
    paymentMethods: [],
    isLoading: false,
    error: null,
  }),

  actions: {
    /**
     * Tải danh sách tất cả các phương thức thanh toán từ API
     */
    async fetchAllPaymentMethods() {
      this.isLoading = true;
      this.error = null;
      try {
        const response = await paymentMethodService.getAllPaymentMethods();
        this.paymentMethods = response.data.model || [];
      } catch (err) {
        this.error = "Lỗi khi tải danh sách phương thức thanh toán.";
        console.error("Lỗi khi tải phương thức thanh toán:", err);
        this.paymentMethods = [];
      } finally {
        this.isLoading = false;
      }
    },

    /**
     * Tạo một phương thức thanh toán mới và làm mới danh sách
     * @param {object} methodData - Dữ liệu của phương thức mới
     */
    async createPaymentMethod(methodData) {
      try {
        const response = await paymentMethodService.createPaymentMethod(methodData);
        await this.fetchAllPaymentMethods(); // Tải lại danh sách để đồng bộ
        return response.data;
      } catch (err) {
        this.error = "Lỗi khi tạo phương thức thanh toán.";
        console.error("Lỗi khi tạo phương thức thanh toán:", err);
        throw err;
      }
    },

    /**
     * Cập nhật một phương thức thanh toán và làm mới danh sách
     * @param {number} methodId - ID của phương thức
     * @param {object} methodData - Dữ liệu cập nhật
     */
    async updatePaymentMethod(methodId, methodData) {
      try {
        const response = await paymentMethodService.updatePaymentMethod(methodId, methodData);
        await this.fetchAllPaymentMethods(); // Tải lại để phản ánh thay đổi
        return response.data;
      } catch (err) {
        this.error = "Lỗi khi cập nhật phương thức thanh toán.";
        console.error(`Lỗi khi cập nhật phương thức ${methodId}:`, err);
        throw err;
      }
    },

    /**
     * Xóa một phương thức thanh toán và làm mới danh sách
     * @param {number} methodId - ID của phương thức cần xóa
     */
    async deletePaymentMethod(methodId) {
      try {
        const response = await paymentMethodService.deletePaymentMethod(methodId);
        await this.fetchAllPaymentMethods(); // Tải lại để loại bỏ mục đã xóa
        return response.data;
      } catch (err) {
        this.error = "Lỗi khi xóa phương thức thanh toán.";
        console.error(`Lỗi khi xóa phương thức ${methodId}:`, err);
        throw err;
      }
    },

    /**
     * Xóa lỗi khỏi state
     */
    clearError() {
      this.error = null;
    },
  },

  getters: {
    /**
     * Getter để kiểm tra xem có phương thức thanh toán nào không
     */
    hasPaymentMethods: (state) => state.paymentMethods.length > 0,

    /**
     * Getter để lấy số lượng phương thức thanh toán
     */
    paymentMethodCount: (state) => state.paymentMethods.length,
  },
});
