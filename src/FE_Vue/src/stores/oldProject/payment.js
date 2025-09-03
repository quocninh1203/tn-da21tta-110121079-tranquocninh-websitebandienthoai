// import { defineStore } from 'pinia';
// import apiClient from '@/axios.js';

// export const usePaymentStore = defineStore('payment', {
//     state: () => {
//         return {
//             isSuccess: false,
//             payments: [],
//         };
//     },
//     actions: { 
//         // Call API tạo order
//         async createPayment(newData) {
//             try {
//                 const response = await apiClient.post(`/api/PaymentAPI/CreatePayments`, newData);
                
//                 this.setIsSuccess(response.data.success);        
//             } catch (error) {
//                 console.error('Error creating new order:', error);
//             }
//         },
//         async getAllPayment() {
//             try {
//                 const response = await apiClient.get(`/api/PaymentAPI/GetAllPayment`,);
                
//                 this.payments = response.data.model;
//             } catch (error) {
//                 console.error('Error :', error);
//             }
//         },
//         async changeStatusByUser(id) {
//             try {
//                 const response = await apiClient.put(`/api/PaymentAPI/UpdateStatusByUser?id=${id}`, {
//                     params: { id: id }
//                 });
                
//                 this.setIsSuccess(response.data.success);        
//             } catch (error) {
//                 console.error('Error :', error);
//             }
//         },
//         async changeStatusByAdmin(id) {
//             try {
//                 const response = await apiClient.put(`/api/PaymentAPI/UpdateStatusByAdmin?id=${id}`, {
//                     params: { id: id }
//                 });
                
//                 this.setIsSuccess(response.data.success);        
//             } catch (error) {
//                 console.error('Error:', error);
//             }
//         },  
//         async canclePayment(id) {
//             try {
//                 const response = await apiClient.put(`/api/PaymentAPI/Cancle?id=${id}`, {
//                     params: { id: id }
//                 });
                
//                 this.setIsSuccess(response.data.success);        
//             } catch (error) {
//                 console.error('Error:', error);
//             }
//         },

//         getPayments(){
//             return this.payments;
//         },
//         getIsSuccess(){
//             return this.isSuccess;
//         },
//         setIsSuccess(status){
//             this.isSuccess = status;
//         }
//     },
//     persist: {
//         enabled: true,
//         strategies: [
//           {
//             key: 'paymentStore', // Tên lưu trong localStorage
//             storage: localStorage, // Sử dụng localStorage
//           },
//         ],
//     },
// });