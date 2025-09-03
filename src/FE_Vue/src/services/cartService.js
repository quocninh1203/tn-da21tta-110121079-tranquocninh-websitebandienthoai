import api from '@/services/axiosInstance';

const cartService = {
  createCart(data) {
    return api.post('/CartAPI/CreateCart', data);
  },

  updateCartQuantity(cartId, data) {
    return api.put(`/CartAPI/UpdateQuantityCart?cartId=${cartId}`, data);
  },

  getAllCartByUserId(userId) {
    return api.get(`/CartAPI/GetAllCartByUserId?userId=${userId}`);
  },

  deleteCart(cartId) {
    return api.delete(`/CartAPI/DeleteCart?cartId=${cartId}`);
  },
};

export default cartService;
