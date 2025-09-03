// import { defineStore } from 'pinia';
// import apiClient from '@/axios.js'; // Import instance từ file axios.js
 
//   // Lấy token từ localStorage
//   const userStore = JSON.parse(localStorage.getItem('userStore'));
//   const token = userStore?.Token;

// export const useCompanyStore = defineStore('company', {
//   state: () => {
//     return {
//       companyIsSelect: [], // Company đang được chọn
//       companies: [],       // Tất cả company
//       isSuccess: false     // Trạng thái khi tạo hoặc cập nhật company
//     };
//   },
//   actions: {
//   // function call api

//     // lấy 1 company bằng id
//     async callCompanyById(id) {
//       try {
//         const response = await apiClient.get(`/api/CompanyAPI/GetCompanies`, {
//           params: { id: id },
//         });
        
//         this.companyIsSelect = response.data.model; 
//       } catch (error) {
//         console.error('Error fetching company by ID:', error);
//       }
//     },

//     // Call API lấy tất cả company
//     async callAllCompanies() {
//       try {
//         const response = await apiClient.get(`/api/CompanyAPI/GetAllCompany`);

//         this.companies = response.data.model;
//       } catch (error) {
//         console.error('Error fetching all companies:', error);
//       }
//     },

//     // Call API tạo company
//     async createCompany(newCompanyData) {
//       try {
//         const response = await apiClient.post(`/api/CompanyAPI/CreateCompanies`, newCompanyData, {
//           headers: {
//             'Content-Type': 'multipart/form-data',
//             'Authorization':   `Bearer ${token}`
//           },
//         });
         
//         this.setIsSuccess(response.data.success);        
//       } catch (error) {
//         console.error('Error creating new company:', error);
//       }
//     },

//     // Call api cập nhật company
//     async updateCompany(id, newCompanyData) {
//       try {
//         const response = await apiClient.put(`/api/CompanyAPI/UpdateCompanies?id=${id}`, newCompanyData,{
//           params: { id: id },
//           headers: {
//             'Content-Type': 'multipart/form-data',
//             'Authorization':   `Bearer ${token}`
//           },
//         });
        
//         this.setIsSuccess(response.data.success);        
//       } catch (error) {
//         console.error('Error creating new company:', error);
//       }
//     },

//   // function get state value

//     // Getter để lấy 1 company 
//     getCompanyIsSelect() {
//       return this.companyIsSelect;
//     },

//     // Getter để lấy tất cả company
//     getCompanies() {
//       return this.companies;
//     },

//     // Getter lấy trạng thái khi thực hiện các hành động
//     getIsSuccess(){
//       return this.isSuccess;
//     },

//   // function set state value

//     // Setter cập nhật trạng thái
//     setIsSuccess(newStatus){
//       this.isSuccess = newStatus; 
//     },

//   },
//   persist: {
//     enabled: true,
//     strategies: [
//       {
//         key: 'companyStore', // Tên lưu trong localStorage
//         storage: localStorage, // Sử dụng localStorage
//       },
//     ],
//   },
// });
