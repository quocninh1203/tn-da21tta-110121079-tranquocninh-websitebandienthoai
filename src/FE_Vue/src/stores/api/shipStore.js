// src/stores/shipStore.js

import { defineStore } from 'pinia';
import shipService from '@/services/shipService';

export const useShipStore = defineStore('ship', {
  state: () => ({
    shippers: [],
    isLoading: false,
    error: null,
  }),

  actions: {
    /**
     * Tải danh sách tất cả các đơn vị vận chuyển từ API
     */
    async fetchAllShippers() {
      this.isLoading = true;
      this.error = null;
      try {
        const response = await shipService.getAllShippers();
        this.shippers = response.data.model || [];
      } catch (err) {
        this.error = "Lỗi khi tải danh sách đơn vị vận chuyển.";
        console.error("Lỗi khi tải đơn vị vận chuyển:", err);
        this.shippers = [];
      } finally {
        this.isLoading = false;
      }
    },

    /**
     * Tạo một đơn vị vận chuyển mới và làm mới danh sách
     * @param {object} shipperData - Dữ liệu của đơn vị vận chuyển mới
     */
    async createShipper(shipperData) {
      try {
        const response = await shipService.createShipper(shipperData);
        await this.fetchAllShippers(); // Tải lại danh sách để đồng bộ
        return response.data;
      } catch (err) {
        this.error = "Lỗi khi tạo đơn vị vận chuyển.";
        console.error("Lỗi khi tạo đơn vị vận chuyển:", err);
        throw err;
      }
    },

    /**
     * Cập nhật thông tin đơn vị vận chuyển và làm mới danh sách
     * @param {number} shipId - ID của đơn vị vận chuyển
     * @param {object} shipperData - Dữ liệu cập nhật
     */
    async updateShipper(shipId, shipperData) {
      try {
        const response = await shipService.updateShipper(shipId, shipperData);
        await this.fetchAllShippers(); // Tải lại để phản ánh thay đổi
        return response.data;
      } catch (err) {
        this.error = "Lỗi khi cập nhật đơn vị vận chuyển.";
        console.error(`Lỗi khi cập nhật đơn vị vận chuyển ${shipId}:`, err);
        throw err;
      }
    },

    /**
     * Xóa một đơn vị vận chuyển và làm mới danh sách
     * @param {number} shipId - ID của đơn vị vận chuyển cần xóa
     */
    async deleteShipper(shipId) {
      try {
        const response = await shipService.deleteShipper(shipId);
        await this.fetchAllShippers(); // Tải lại để loại bỏ mục đã xóa
        return response.data;
      } catch (err) {
        this.error = "Lỗi khi xóa đơn vị vận chuyển.";
        console.error(`Lỗi khi xóa đơn vị vận chuyển ${shipId}:`, err);
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
     * Getter để kiểm tra xem có đơn vị vận chuyển nào không
     */
    hasShippers: (state) => state.shippers.length > 0,

    /**
     * Getter để lấy số lượng đơn vị vận chuyển
     */
    shipperCount: (state) => state.shippers.length,

    /**
     * Getter để lấy đơn vị vận chuyển có chi phí thấp nhất
     */
    cheapestShipper: (state) => {
      if (state.shippers.length === 0) return null;
      return state.shippers.reduce((min, shipper) => 
        shipper.cost < min.cost ? shipper : min
      );
    },
  },
});
