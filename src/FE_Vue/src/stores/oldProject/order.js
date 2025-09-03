// import { defineStore } from 'pinia';
// import apiClient from '@/axios.js';

// export const useOrderStore = defineStore('order', {
//     state: () => {
//         return {
//             order: null, // thông tin order hiện tại
//             orders: [], // tất cả order
//             listOrder: [], // danh sách các sản phẩm trong đơn hàng (dữ liệu để tạo orderDetail)

//             orderDetails: [],
//             orderDetailForOrder: [],
//             isSuccess: false,     // Trạng thái 
//         };
//     }, 
//     actions: { 
//         // Call API tạo order
//         async createOrder(newOrder) {
//             try {
//                 const response = await apiClient.post(`/api/OrderAPI/CreateOrders`, newOrder);
                
//                 this.order = response.data.model;
//                 this.setOrderSuccess(response.data.success);        
//             } catch (error) {
//                 console.error('Error creating new order:', error);
//             }
//         },
//         async callAllOrder() {
//             try {
//                 const response = await apiClient.get(`/api/OrderAPI/GetAllOrder`,);
                
//                 this.orders = response.data.model;
//             } catch (error) {
//                 console.error('Error :', error);
//             }
//         },
//         async callOrders(id) {
//             try {
//                 const response = await apiClient.get(`/api/OrderAPI/GetOrders?id=${id}`, {
//                     params: { id: id }
//                 });
                
//                 this.order = response.data.model;
//             } catch (error) {
//                 console.error('Error :', error);
//             }
//         },
//         async createOrderDetail(newOrder) {
//             try {
//                 const response = await apiClient.post(`/api/OrderDetailAPI/CreateOrderDetails`, newOrder);
                
//                 this.setOrderSuccess(response.data.success);        
//             } catch (error) {
//                 console.error('Error creating new order:', error);
//             }
//         },
//         async callAllOrderDetail() {
//             try {
//                 const response = await apiClient.get(`/api/OrderDetailAPI/GetAllOrderDetail`,);
                
//                 this.orderDetails = response.data.model;
//             } catch (error) {
//                 console.error('Error :', error);
//             }
//         },
//         async callOrderDetailByOrderId(id) {
//             try {
//                 const response = await apiClient.get(`/api/OrderDetailAPI/GetOrderDetailByOrderId?orderId=${id}`, {
//                     params: { orderId: id }
//                 });
                
//                 this.orderDetailForOrder = response.data.model;
//             } catch (error) {
//                 console.error('Error :', error);
//             }
//         },

//         getOrder(){
//             return this.order;
//         },
//         getOrderSuccess(){
//             return this.isSuccess;
//         },
//         setOrderSuccess(newStatus){
//             this.isSuccess = newStatus; 
//         },
//         setListOrder(list){
//             this.listOrder = list;
//         },
//         getListOrder(){
//             return this.listOrder;
//         },
//         getOrders(){
//             return this.orders;
//         },
//         getOrderDetails(){
//             return this.orderDetails;
//         },
//         getOrderDetailForOrder(){
//             return this.orderDetailForOrder;
//         }
//     },
//     persist: {
//         enabled: true,
//         strategies: [
//           {
//             key: 'orderStore', // Tên lưu trong localStorage
//             storage: localStorage, // Sử dụng localStorage
//           },
//         ],
//     },
// });