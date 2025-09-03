// import { defineStore } from 'pinia';
// import apiClient from '@/axios.js'; // Import instance từ file axios.js

// export const useCartStore = defineStore('cart', {
//   state: () => {
//     return {
//       cart: {},
//       cartDetails: [],
//       isSuccess: false,
//       cartDetailOfCart: []
//     };
//   },
//   actions: {
//   // function call api 

//     // call api tạo cart
//     async createCart(newCart){
//       try{
//         const response = await apiClient.post(`/api/CartAPI/CreateCarts`, newCart);
        
//         this.setIsSuccess(response.data.success);           
//       }catch(error){
//         console.error('Error creating new cart:', error);
//       }
//     },
//     // call lấy cart bằng userId
//     async getCartOfUser(userId){
//       try{
//         const response = await apiClient.get(`/api/CartAPI/GetCarts`, {
//           params: { userId: userId }
//         });
        
//         this.cart = response.data.model;
//       }catch(error){
//         console.error('Error creating new cart:', error);
//       }      
//     },
//     // call api tạo cart detail
//     async createCartDetail(newCartDetail){
//       try{
//         const response = await apiClient.post(`/api/CartDetailAPI/CreateCartDetails`, newCartDetail);
        
//         this.setIsSuccess(response.data.success);           
//       }catch(error){ 
//         console.error('Error creating new cart detail:', error);
//       }
//     },
//     // call api cập nhật cart detail
//     async updateCartDetail(id, newCartDetail){
//       try{
//         const response = await apiClient.put(`/api/CartDetailAPI/UpdateCartDetails?id=${id}`, newCartDetail, {
//           params: { id: id }
//         });
        
//         this.setIsSuccess(response.data.success); // không cần thiết        
//       }catch(error){
//         console.error('Error updating a cart detail:', error);
//       }
//     },   
//     // call api xóa cart detail
//     async deleteCartDetail(id){
//       try{
//         const response = await apiClient.delete(`/api/CartDetailAPI/DeleteCartDetail?id=${id}`, {
//           params: { id: id }
//         });
        
//         this.setIsSuccess(response.data.success);           
//       }catch(error){
//         console.error('Error deleting a cart detail:', error);
//       }
//     }, 
//     // call api lấy danh sách tất cả cart detail
//     async getAllCartDetail(){
//       try{
//         const response = await apiClient.get("/api/CartDetailAPI/GetAllCartDetail");

//         this.cartDetails = response.data.model;      
//       }catch(error){
//         console.error('Error getting cartdetails of cart:', error);
//       }
//     }, 
//     // call lấy danh sách cart detail của cart bằng cartId
//     async getCartDetailOfCart(cartId){
//       try{
//         const response = await apiClient.get(`/api/CartDetailAPI/GetCartDetailByCartId`, {
//           params: { cartId: cartId }
//         });

//         this.cartDetailOfCart = response.data.model;
//       }catch(error){
//         console.error('Error getting cartdetails of cart:', error);
//       }      
//     },

//   // function get state value

//     // Getter trạng thái 
//     getIsSuccess(){
//       return this.isSuccess;
//     },
//     getCart(){
//       return this.cart;
//     },
//     // Getter tất cả cartDetail 
//     getCartDetails(){
//       return this.cartDetails;
//     },
//     // Getter cartDetail của cart
//     getCartDetail(){
//       return this.cartDetailOfCart;
//     },

//   // function set state value

//     // Setter trạng thái
//     setIsSuccess(newStatus){
//       this.isSuccess = newStatus;
//     },

//   },
//   persist: {
//     enabled: true,
//     strategies: [
//       {
//         key: 'cartStore', // Tên lưu trong localStorage
//         storage: localStorage, // Sử dụng localStorage
//       },
//     ],
//   },
// });
