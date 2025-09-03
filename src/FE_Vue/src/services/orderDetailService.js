import api from '@/services/axiosInstance';

const orderDetailService = {
  createOrderDetail(data) {
    return api.post('/OrderDetailAPI/CreateOrderDetail', data);
  },
};

export default orderDetailService;
