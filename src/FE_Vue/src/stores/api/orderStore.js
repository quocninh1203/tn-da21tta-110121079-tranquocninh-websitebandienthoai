import { defineStore } from 'pinia';
import orderService from '@/services/orderService';
import orderDetailService from '@/services/orderDetailService';

export const useOrderStore = defineStore('order', {
  state: () => ({
    orders: [], 
    yourOrder: null, 
    orderDetails: null,
  }),

  actions: {
    async fetchAllOrders() {
      const res = await orderService.getAllOrders();
      this.orders = res.data.model || [];
    },

    async createOrder(data) {
      const res = await orderService.createOrder(data);
      this.yourOrder = res.data.model || null;
      await this.fetchAllOrders();
      return res;
    },

    async changeStatus(orderId, data) {
      const res = await orderService.changeStatus(orderId, data);
      // await this.fetchAllOrders();
      return res;
    },

    async removeOrder(orderId) {
      const res = await orderService.removeOrder(orderId);
      return res;
    },

    async createOrderDetail(data) {
      const res = await orderDetailService.createOrderDetail(data);
      return res;
    },

    async fetchOrdersByUserId(userId) {
      try {
        const res = await orderService.getOrdersByUserId(userId);
        this.orders = res.data.model || [];
      } catch (error) {
        console.error(`Failed to fetch orders for user ${userId}:`, error);
        this.orders = [];
      }
    },

    async fetchOrderDetail(orderId) {
      try {
        const res = await orderService.getOrderDetailByOrderId(orderId);
        this.orderDetails = res.data.model || null;
        return res;
      } catch (error) {
        console.error(`Failed to fetch order detail for order ${orderId}:`, error);
        this.orderDetails = null;
      }
    },

    // Session Storage Management for Products
    async saveProductsToSession(products) {
      try {
        sessionStorage.setItem('listItems', JSON.stringify(products));
      } catch (error) {
        console.error('Failed to save products to sessionStorage:', error);
      }
    },

    async getProductsFromSession() {
      try {
        const products = sessionStorage.getItem('listItems');
        return products ? JSON.parse(products) : [];
      } catch (error) {
        console.error('Failed to get products from sessionStorage:', error);
        return [];
      }
    },

    async clearProductsFromSession() {
      try {
        sessionStorage.removeItem('listItems');
      } catch (error) {
        console.error('Failed to clear products from sessionStorage:', error);
      }
    },

    async removeProductFromSession(productId) {
      try {
        const currentProducts = await this.getProductsFromSession();
        
        // Lọc bỏ sản phẩm có ID tương ứng
        const updatedProducts = currentProducts.filter(product => 
          (product.cartId || product.variantId) !== productId
        );
        
        sessionStorage.setItem('listItems', JSON.stringify(updatedProducts));
        
        return updatedProducts;
      } catch (error) {
        console.error('Failed to remove product from sessionStorage:', error);
        return [];
      }
    },

    // Payment, Shipping Methods and Shipping Address
    async savePayMethodToSession(method) {
      try {
        sessionStorage.setItem('payMethod', JSON.stringify(method));
      } catch (error) {
        console.error('Failed to save payMethod to sessionStorage:', error);
      }
    },

    async getPayMethodFromSession() {
      try {
        const method = sessionStorage.getItem('payMethod');
        return method ? JSON.parse(method) : "";
      } catch (error) {
        console.error('Failed to get payMethod from sessionStorage:', error);
        return [];
      }
    },

    async saveShipMethodToSession(method) {
      try {
        sessionStorage.setItem('shipMethod', JSON.stringify(method));
      } catch (error) {
        console.error('Failed to save shipMethod to sessionStorage:', error);
      }
    },

    async getShipMethodFromSession() {
      try {
        const method = sessionStorage.getItem('shipMethod');
        return method ? JSON.parse(method) : "";
      } catch (error) {
        console.error('Failed to get shipMethod from sessionStorage:', error);
        return [];
      }
    },

    async saveShipAddressToSession(address) {
      try {
        sessionStorage.setItem('shipAddress', JSON.stringify(address));
      } catch (error) {
        console.error('Failed to save shipAddress to sessionStorage:', error);
      }
    },

    async getShipAddressFromSession() {
      try {
        const address = sessionStorage.getItem('shipAddress');
        return address ? JSON.parse(address) : "";
      } catch (error) {
        console.error('Failed to get shipAddress from sessionStorage:', error);
        return [];
      }
    },
  },
});
