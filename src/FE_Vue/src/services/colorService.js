// src/services/colorService.js

import api from '@/services/axiosInstance';

const colorService = {
  /**
   * Tạo một màu mới
   * @param {object} data - Dữ liệu màu mới, ví dụ: { name: 'Xanh' }
   */
  createColor(data) {
    return api.post('/ColorAPI/CreateColor', data);
  },

  /**
   * Lấy danh sách tất cả các màu
   */
  getAllColors() {
    return api.get('/ColorAPI/GetAllColors');
  },

  /**
   * Cập nhật một màu đã có
   * @param {number} colorId - ID của màu cần cập nhật
   * @param {object} data - Dữ liệu màu mới, ví dụ: { name: 'Xanh dương' }
   */
  updateColor(colorId, data) {
    return api.put('/ColorAPI/UpdateColor', data, { 
      params: { colorId } 
    });
  },

  /**
   * Xóa một màu theo ID
   * @param {number} colorId - ID của màu cần xóa
   */
  deleteColor(colorId) {
    return api.delete('/ColorAPI/DeleteColor', { 
      params: { colorId } 
    });
  }
};

export default colorService;
