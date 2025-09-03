import { defineStore } from 'pinia';
import cartService from '@/services/cartService';

export const useCartStore = defineStore('cart', {
  state: () => ({
    carts: [],
  }),

  actions: {
    // Tạo cart mới
    async createCart(data, userId) {
      const res = await cartService.createCart(data);
      await this.fetchCartByUserId(userId); // Tự cập nhật lại danh sách sau khi thêm
      return res.data;
    },

    // Cập nhật số lượng sản phẩm trong giỏ hàng
    async updateCartQuantity(cartId, quantity, userId) {
      const res = await cartService.updateCartQuantity(cartId, { quantity });
      await this.fetchCartByUserId(userId); // Cập nhật lại giỏ hàng
      return res;
    },

    // Lấy tất cả giỏ hàng theo userId
    async fetchCartByUserId(userId) {
      try {
        const res = await cartService.getAllCartByUserId(userId);
        this.carts = res.data.model || [];
      }catch (error) {
        console.error(`Failed to fetch cart for user ${userId}:`, error);
        this.carts = [];
      }
    },

    // Xóa sản phẩm khỏi giỏ hàng
    async deleteCart(cartId, userId) {
      const res = await cartService.deleteCart(cartId);
      await this.fetchCartByUserId(userId); // Cập nhật lại sau khi xóa
      return res;
    },
  },
});
