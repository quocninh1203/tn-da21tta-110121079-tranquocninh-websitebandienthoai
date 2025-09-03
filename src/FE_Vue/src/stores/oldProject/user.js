// import { defineStore } from 'pinia';
// import apiClient from '@/axios.js'; 
// import router from '@/router';

//   // Lấy token từ localStorage
//   const userStore = JSON.parse(localStorage.getItem('userStore'));
//   const token = userStore?.Token;

// export const useUserStore = defineStore('user', {
//   state: () => { 
//     return{
//       userData: {
//         Model: {},
//         Token: "",
//         IsLogin: false
//       },
//       registerData: {
//         message: "",
//         success: false
//       },
//       userDataFull: {},
//       users: [],
//       isSuccess: false
//     }
//   },
//   actions: {
//     async callLogin(username, password) {
//       console.log("Call api login is running...");    
//       try {
//         // Gửi yêu cầu POST đến API đăng nhập
//         const response = await apiClient.post('/api/UserAPI/Login', {
//           username,
//           password,
//         });
    
//         // Xử lý thành công
//         if (response.data.success) {
//           this.userData.Model = response.data.model; 
//           this.userData.Token = response.data.token;
//           this.userData.IsLogin = true;
//           this.isSuccess = response.data.success;

//           // Lưu toàn bộ userData vào localStorage
//           localStorage.setItem('userStore', JSON.stringify(this.userData));
          
//           if (this.userData.Model.roleId === 0) {
//             router.push('/');
//           } else if (this.userData.Model.roleId === 1) {
//             router.push('/admin'); // Chuyển đến trang admin
//             console.log("admin ", this.userData.Model.roleId);
            
//           } else {
//             console.error('RoleId không hợp lệ:', this.userData.Model.roleId);
//           }
//         } else {
//           // Xử lý lỗi đăng nhập
//           console.error('Đăng nhập thất bại:', response.data.message);
//         }
//       } catch (error) {
//         // Xử lý lỗi kết nối hoặc lỗi khác
//         this.error = error; // Lưu trữ lỗi
//         console.error('Lỗi khi đăng nhập:', error);
//       }
//     },

//     async callRegister(fullname, email, phonenumber, username, password){
//       console.log("Call api register is running...");   
//       try{
//         // Gửi yêu cầu POST đến API đăng ký
//         const response = await apiClient.post('/api/UserAPI/CreateUsers', {
//           fullName: fullname,
//           email: email,
//           phoneNumber: phonenumber,
//           userName: username,
//           password: password,
//         });

//         // localStorage.removeItem('userStore');

//         // Xử lý thành công
//         if (response.data.success) {
//           console.log('Tạo tài khoản thành công!');

//           // router.push('/login');

//           this.registerData.message = "Tạo tài khoản thành công!";
//           this.registerData.success = response.data.success;
//         } else {
//           // Xử lý lỗi Tạo tài khoản
//           this.registerData.message = response.data.message;   
//           this.registerData.success = response.data.success;
//           console.error('Tạo tài khoản thất bại:', response.data.message);
//         }
//       }catch (error) {
//         // Xử lý lỗi kết nối hoặc lỗi khác
//         this.error = error; // Lưu trữ lỗi
//         console.error('Lỗi khi Tạo tài khoản:', error);
//       }
//     },

//     logout() {
//       // Xóa dữ liệu trong userData
//       this.userData.Model = [];
//       this.userData.Token = "";
//       this.userData.IsLogin = false;

//       // Chuyển hướng về trang đăng nhập hoặc trang chủ
//       router.push('/');
      
//       // Xóa token từ localStorage nếu có
//       localStorage.removeItem('userStore');
//       console.log('Đã đăng xuất!');
//     },

//     async getUserById(id) {
//       try {
//         const response = await apiClient.get(`/api/UserAPI/GetUsers?id=${id}`, {
//           params: { id: id },
//         });
        
//         this.userDataFull = response.data.model; 
//       } catch (error) {
//         console.error('Error fetching company by ID:', error);
//       }
//     },

//     async editAvatar(id, image){
//       try{
//         const response = await apiClient.put(`/api/UserAPI/UpdateAvatars?id=${id}`, image, {
//           params: { id: id },
//           headers: {
//             'Content-Type': 'multipart/form-data',
//             'Authorization': `Bearer ${token}`
//           },
//         });

//         // Xử lý thành công
//         if (response.data.success) {
//           console.log('Đổi avatar thành công!');
//         } else {
//           console.error('Đổi avatar thất bại:', response.data.message);
//         }
//       }catch (error) {
//         this.error = error; // Lưu trữ lỗi
//         console.error('Lỗi khi cập nhật avatar:', error);
//       }
//     },
//     async getAllUser() {
//       try {
//           const response = await apiClient.get(`/api/UserAPI/GetAllUser`,);
          
//           this.users = response.data.model;
//       } catch (error) {
//           console.error('Error :', error);
//       }
//     },
    
//     getUserData(){
//       return this.userData;
//     },
//     getUserDataFull(){
//       return this.userDataFull;
//     },
//     getUsers(){
//       return this.users;
//     },
//     getIsSuccess(){
//       return this.isSuccess;
//     }
//   },
//   persist: {
//     enabled: true,
//     strategies: [
//       {
//         key: 'userStore', // Tên lưu trong localStorage
//         storage: localStorage, // Sử dụng localStorage
//       },
//     ],
//   },
// });
