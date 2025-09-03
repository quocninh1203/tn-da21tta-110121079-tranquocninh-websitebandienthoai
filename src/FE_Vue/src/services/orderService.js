import api from '@/services/axiosInstance';

const orderService = {
  createOrder(data) {
    return api.post('/OrderAPI/CreateOrder', data);
  },

  getAllOrders() {
    return api.get('/OrderAPI/GetAllOrders');
  },

  getOrdersByUserId(userId) {
    return api.get('/OrderAPI/GetOrderByUserId', {
      params: { userId }
    });
  },

  getOrderDetailByOrderId(orderId) {
    return api.get('/OrderAPI/GetOrderDetailByOrderId', {
      params: { orderId }
    });
  },

  changeStatus(orderId, data) {
    return api.put('/OrderAPI/ChangeStatus', data, {
      params: { orderId }
    });
  },

  removeOrder(orderId){
    return api.delete('/OrderAPI/DeleteOrder', {
      params: { orderId }
    })
  }
};

export default orderService;
