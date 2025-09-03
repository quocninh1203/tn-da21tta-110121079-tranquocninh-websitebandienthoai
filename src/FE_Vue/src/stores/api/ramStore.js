// src/stores/ramStore.js

import { defineStore } from 'pinia';
import ramService from '@/services/ramService';

export const useRamStore = defineStore('ram', {
  state: () => ({
    rams: [],
    isLoading: false,
    error: null,
  }),

  actions: {
    /**
     * Tải danh sách tất cả các loại RAM từ API
     */
    async fetchAllRams() {
      this.isLoading = true;
      this.error = null;
      try {
        const response = await ramService.getAllRams();
        this.rams = response.data.model || [];
      } catch (err) {
        this.error = "Lỗi khi tải danh sách RAM.";
        console.error("Lỗi khi tải RAM:", err);
        this.rams = [];
      } finally {
        this.isLoading = false;
      }
    },

    /**
     * Tạo một loại RAM mới và làm mới danh sách
     * @param {object} ramData - Dữ liệu của RAM mới
     */
    async createRam(ramData) {
      try {
        const response = await ramService.createRam(ramData);
        await this.fetchAllRams(); // Tải lại danh sách để đồng bộ
        return response.data;
      } catch (err) {
        this.error = "Lỗi khi tạo RAM.";
        console.error("Lỗi khi tạo RAM:", err);
        throw err;
      }
    },

    /**
     * Cập nhật thông tin RAM và làm mới danh sách
     * @param {number} ramId - ID của RAM
     * @param {object} ramData - Dữ liệu cập nhật
     */
    async updateRam(ramId, ramData) {
      try {
        const response = await ramService.updateRam(ramId, ramData);
        await this.fetchAllRams(); // Tải lại để phản ánh thay đổi
        return response.data;
      } catch (err) {
        this.error = "Lỗi khi cập nhật RAM.";
        console.error(`Lỗi khi cập nhật RAM ${ramId}:`, err);
        throw err;
      }
    },

    /**
     * Xóa một loại RAM và làm mới danh sách
     * @param {number} ramId - ID của RAM cần xóa
     */
    async deleteRam(ramId) {
      try {
        const response = await ramService.deleteRam(ramId);
        await this.fetchAllRams(); // Tải lại để loại bỏ mục đã xóa
        return response.data;
      } catch (err) {
        this.error = "Lỗi khi xóa RAM.";
        console.error(`Lỗi khi xóa RAM ${ramId}:`, err);
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
     * Getter để kiểm tra xem có loại RAM nào không
     */
    hasRams: (state) => state.rams.length > 0,

    /**
     * Getter để lấy số lượng loại RAM
     */
    ramCount: (state) => state.rams.length,

    /**
     * Getter để sắp xếp RAM theo dung lượng
     */
    sortedRamsBySize: (state) => {
      return [...state.rams].sort((a, b) => {
        // Chuyển đổi size thành số để so sánh (ví dụ: "8GB" -> 8)
        const sizeA = parseInt(a.size);
        const sizeB = parseInt(b.size);
        return sizeA - sizeB;
      });
    },
  },
});
