// src/services/shipService.js

import api from '@/services/axiosInstance';

const shipService = {
  /**
   * Lấy danh sách tất cả các đơn vị vận chuyển
   */
  getAllShippers() {
    return api.get('/ShipperAPI/GetAllShippers');
  },

  /**
   * Tạo một đơn vị vận chuyển mới
   * @param {object} data - Dữ liệu đơn vị mới, ví dụ: { name: 'Giao hàng nhanh', cost: 25000 }
   */
  createShipper(data) {
    return api.post('/ShipperAPI/CreateShipper', data);
  },

  /**
   * Cập nhật thông tin đơn vị vận chuyển
   * @param {number} shipId - ID của đơn vị vận chuyển cần cập nhật
   * @param {object} data - Dữ liệu mới, ví dụ: { name: 'Giao hàng tiết kiệm', cost: 15000 }
   */
  updateShipper(shipId, data) {
    return api.put('/ShipperAPI/UpdateShipper', data, {
      params: { shipId }
    });
  },

  /**
   * Xóa một đơn vị vận chuyển theo ID
   * @param {number} shipId - ID của đơn vị vận chuyển cần xóa
   */
  deleteShipper(shipId) {
    return api.delete('/ShipperAPI/DeleteShipper', {
      params: { shipId }
    });
  }
};

export default shipService;
