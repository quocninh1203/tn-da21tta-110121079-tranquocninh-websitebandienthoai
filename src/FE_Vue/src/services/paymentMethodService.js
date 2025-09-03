// src/services/paymentMethodService.js

import api from '@/services/axiosInstance';

const paymentMethodService = {
  /**
   * Lấy danh sách tất cả các phương thức thanh toán
   */
  getAllPaymentMethods() {
    return api.get('/PaymentMethodAPI/GetAllPaymentMethods');
  },

  /**
   * Tạo một phương thức thanh toán mới
   * @param {object} data - Dữ liệu phương thức mới, ví dụ: { name: 'Thanh toán bằng ZaloPay' }
   */
  createPaymentMethod(data) {
    return api.post('/PaymentMethodAPI/CreatePaymentMethod', data);
  },

  /**
   * Cập nhật một phương thức thanh toán
   * @param {number} methodId - ID của phương thức cần cập nhật
   * @param {object} data - Dữ liệu mới, ví dụ: { name: 'Thanh toán khi nhận hàng' }
   */
  updatePaymentMethod(methodId, data) {
    return api.put('/PaymentMethodAPI/UpdatePaymentMethod', data, {
      params: { methodId }
    });
  },

  /**
   * Xóa một phương thức thanh toán theo ID
   * @param {number} methodId - ID của phương thức cần xóa
   */
  deletePaymentMethod(methodId) {
    return api.delete('/PaymentMethodAPI/DeletePaymentMethod', {
      params: { methodId }
    });
  }
};

export default paymentMethodService;
