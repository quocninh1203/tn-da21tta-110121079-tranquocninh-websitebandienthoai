// src/stores/storageStore.js

import { defineStore } from 'pinia';
import storageService from '@/services/storageService';

export const useStorageStore = defineStore('storage', {
  state: () => ({
    storages: [],
    isLoading: false,
    error: null,
  }),

  actions: {
    /**
     * Tải danh sách tất cả các loại bộ nhớ từ API
     */
    async fetchAllStorages() {
      this.isLoading = true;
      this.error = null;
      try {
        const response = await storageService.getAllStorages();
        this.storages = response.data.model || [];
      } catch (err) {
        this.error = "Lỗi khi tải danh sách bộ nhớ.";
        console.error("Lỗi khi tải bộ nhớ:", err);
        this.storages = [];
      } finally {
        this.isLoading = false;
      }
    },

    /**
     * Tạo một loại bộ nhớ mới và làm mới danh sách
     * @param {object} storageData - Dữ liệu của bộ nhớ mới
     */
    async createStorage(storageData) {
      try {
        const response = await storageService.createStorage(storageData);
        await this.fetchAllStorages(); // Tải lại danh sách để đồng bộ
        return response.data;
      } catch (err) {
        this.error = "Lỗi khi tạo bộ nhớ.";
        console.error("Lỗi khi tạo bộ nhớ:", err);
        throw err;
      }
    },

    /**
     * Cập nhật thông tin bộ nhớ và làm mới danh sách
     * @param {number} storageId - ID của bộ nhớ
     * @param {object} storageData - Dữ liệu cập nhật
     */
    async updateStorage(storageId, storageData) {
      try {
        const response = await storageService.updateStorage(storageId, storageData);
        await this.fetchAllStorages(); // Tải lại để phản ánh thay đổi
        return response.data;
      } catch (err) {
        this.error = "Lỗi khi cập nhật bộ nhớ.";
        console.error(`Lỗi khi cập nhật bộ nhớ ${storageId}:`, err);
        throw err;
      }
    },

    /**
     * Xóa một loại bộ nhớ và làm mới danh sách
     * @param {number} storageId - ID của bộ nhớ cần xóa
     */
    async deleteStorage(storageId) {
      try {
        const response = await storageService.deleteStorage(storageId);
        await this.fetchAllStorages(); // Tải lại để loại bỏ mục đã xóa
        return response.data;
      } catch (err) {
        this.error = "Lỗi khi xóa bộ nhớ.";
        console.error(`Lỗi khi xóa bộ nhớ ${storageId}:`, err);
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
     * Getter để kiểm tra xem có loại bộ nhớ nào không
     */
    hasStorages: (state) => state.storages.length > 0,

    /**
     * Getter để lấy số lượng loại bộ nhớ
     */
    storageCount: (state) => state.storages.length,

    /**
     * Getter để sắp xếp bộ nhớ theo dung lượng
     */
    sortedStoragesBySize: (state) => {
      return [...state.storages].sort((a, b) => {
        // Chuyển đổi size thành số để so sánh (ví dụ: "256GB" -> 256)
        const sizeA = parseInt(a.size);
        const sizeB = parseInt(b.size);
        return sizeA - sizeB;
      });
    },

    /**
     * Getter để lọc bộ nhớ theo loại (SSD/HDD)
     */
    storagesByType: (state) => {
      return (type) => state.storages.filter(storage => 
        storage.size.toLowerCase().includes(type.toLowerCase())
      );
    },
  },
});
