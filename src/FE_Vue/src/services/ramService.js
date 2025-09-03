// src/services/ramService.js

import api from '@/services/axiosInstance';

const ramService = {
  /**
   * Lấy danh sách tất cả các loại RAM
   */
  getAllRams() {
    return api.get('/RamAPI/GetAllRams');
  },

  /**
   * Tạo một loại RAM mới
   * @param {object} data - Dữ liệu RAM mới, ví dụ: { size: '8GB' }
   */
  createRam(data) {
    return api.post('/RamAPI/CreateRam', data);
  },

  /**
   * Cập nhật thông tin RAM
   * @param {number} ramId - ID của RAM cần cập nhật
   * @param {object} data - Dữ liệu mới, ví dụ: { size: '16GB' }
   */
  updateRam(ramId, data) {
    return api.put('/RamAPI/UpdateRam', data, {
      params: { ramId }
    });
  },

  /**
   * Xóa một loại RAM theo ID
   * @param {number} ramId - ID của RAM cần xóa
   */
  deleteRam(ramId) {
    return api.delete('/RamAPI/DeleteRam', {
      params: { ramId }
    });
  }
};

export default ramService;
