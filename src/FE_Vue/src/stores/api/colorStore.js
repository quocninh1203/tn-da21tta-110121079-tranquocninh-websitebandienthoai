// src/stores/colorStore.js

import { defineStore } from 'pinia';
import colorService from '@/services/colorService';

export const useColorStore = defineStore('color', {
  state: () => ({
    colors: [],
    isLoading: false,
  }),

  actions: {
    /**
     * Tải danh sách tất cả màu sắc từ API và cập nhật state
     */
    async fetchAllColors() {
      this.isLoading = true;
      try {
        const response = await colorService.getAllColors();
        this.colors = response.data.model || [];
      } catch (error) {
        console.error("Lỗi khi tải danh sách màu sắc:", error);
        this.colors = []; // Reset state nếu có lỗi
      } finally {
        this.isLoading = false;
      }
    },

    /**
     * Tạo một màu mới và tải lại danh sách
     * @param {object} colorData - Dữ liệu màu để tạo
     */
    async createColor(colorData) {
      try {
        const response = await colorService.createColor(colorData);
        await this.fetchAllColors(); // Tải lại danh sách để đồng bộ state
        return response;
      } catch (error) {
        console.error("Lỗi khi tạo màu:", error);
        throw error; // Ném lỗi ra để component có thể xử lý
      }
    },

    /**
     * Cập nhật một màu và tải lại danh sách
     * @param {number} colorId - ID của màu cần cập nhật
     * @param {object} colorData - Dữ liệu cập nhật
     */
    async updateColor(colorId, colorData) {
      try {
        const response = await colorService.updateColor(colorId, colorData);
        await this.fetchAllColors(); // Tải lại danh sách để đồng bộ state
        return response;
      } catch (error) {
        console.error(`Lỗi khi cập nhật màu ${colorId}:`, error);
        throw error;
      }
    },

    /**
     * Xóa một màu và tải lại danh sách
     * @param {number} colorId - ID của màu cần xóa
     */
    async deleteColor(colorId) {
      try {
        const response = await colorService.deleteColor(colorId);
        await this.fetchAllColors(); // Tải lại danh sách để đồng bộ state
        return response;
      } catch (error) {
        console.error(`Lỗi khi xóa màu ${colorId}:`, error);
        throw error;
      }
    },
  },
});
