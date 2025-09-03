// src/services/storageService.js

import api from '@/services/axiosInstance';

const storageService = {
  /**
   * Lấy danh sách tất cả các loại bộ nhớ
   */
  getAllStorages() {
    return api.get('/StorageAPI/GetAllStorages');
  },

  /**
   * Tạo một loại bộ nhớ mới
   * @param {object} data - Dữ liệu bộ nhớ mới, ví dụ: { size: '256GB' }
   */
  createStorage(data) {
    return api.post('/StorageAPI/CreateStorage', data);
  },

  /**
   * Cập nhật thông tin bộ nhớ
   * @param {number} storageId - ID của bộ nhớ cần cập nhật
   * @param {object} data - Dữ liệu mới, ví dụ: { size: '512GB' }
   */
  updateStorage(storageId, data) {
    return api.put('/StorageAPI/UpdateStorage', data, {
      params: { storageId }
    });
  },

  /**
   * Xóa một loại bộ nhớ theo ID
   * @param {number} storageId - ID của bộ nhớ cần xóa
   */
  deleteStorage(storageId) {
    return api.delete('/StorageAPI/DeleteStorage', {
      params: { storageId }
    });
  }
};

export default storageService;
