// import { defineStore } from 'pinia';
// import apiClient from '@/axios.js'; // Import instance từ file axios.js

//   // Lấy token từ localStorage
//   const userStore = JSON.parse(localStorage.getItem('userStore'));
//   const token = userStore?.Token;

// export const useProductStore = defineStore('product', {
//   state: () => {
//     return{
//       products_isVisible: [], // danh sách product đang được bán
//       products_unVisible: [], // danh sách product chưa được bán
//       products_isSale: [], // danh sách product đang sale
//       products: [], // danh sách tất cả product
//       productIsSelect: [], // 1 product

//       optionIsSelect: [], // danh sách productOption của 1 product
//       productOptions: [], // danh sách tất cả productOption
//       optionById: [], // 1 productOption
      
//       isSuccess: false, // trạng thái hành động
//       error: null,
     
//       productIsSee: null, // product đang được xem cấu hình
//     }
//   },
//   actions: {
//   // actions call api
     
//     // Tạo product
//     async createProduct(newProductData) {
//       try {
//         const response = await apiClient.post(`/api/ProductAPI/CreateProducts`, newProductData, {
//           headers: {
//             'Content-Type': 'multipart/form-data',
//             'Authorization': `Bearer ${token}`
//           },
//         });
        
//         this.setIsSuccess(response.data.success);
//       } catch (error) {
//         console.error('Error creating new product:', error);
//       }
//     },
//     // Cập nhật product
//     async updateProduct(id, newProductData) {
//       try {
//         const response = await apiClient.put(`/api/ProductAPI/UpdateProducts`, newProductData, {
//           params: { id: id },
//           headers: {
//             'Content-Type': 'multipart/form-data',
//             'Authorization':   `Bearer ${token}`
//           },
//         });
        
//         this.setIsSuccess(response.data.success);
//       } catch (error) {
//         console.error('Error creating new product:', error);
//       }
//     },    
//     // Lấy danh sách tất cả product 
//     async callProducts(){
//       try {
//         const response = await apiClient.get('/api/ProductAPI/GetAllProduct');
//         this.products = response.data.model; // Lưu dữ liệu sản phẩm vào state
//       } catch (error) {
//         this.error = error; // Lưu lỗi nếu xảy ra
//         console.error('Error fetching products is sale:', error);
//       }
//     },
//     // Lấy 1 product bằng id
//     async callProductById(id){
//       try {
//         const response = await apiClient.get(`/api/ProductAPI/GetProducts`,{
//           params: {id: id}
//         });
//         this.productIsSelect = response.data.model; // Lưu dữ liệu sản phẩm vào state
//       } catch (error) {
//         this.error = error; // Lưu lỗi nếu xảy ra
//         console.error('Error fetching products:', error);
//       }
//     },
//     // Lấy danh sách product đang được bán
//     async callProducts_isVisible() {
//       try {
//         const response = await apiClient.get('/api/ProductAPI/GetAllProduct-IsVisible');
//         this.products_isVisible = response.data.model; // Lưu dữ liệu sản phẩm vào state
//       } catch (error) {
//         this.error = error; // Lưu lỗi nếu xảy ra
//         console.error('Error fetching products:', error);
//       }
//     },
//     // Lấy danh sách product chưa được bán
//     async callProducts_unVisible() {
//       try {
//         const response = await apiClient.get('/api/ProductAPI/GetAllProduct-UnVisible');
//         this.products_unVisible = response.data.model; // Lưu dữ liệu sản phẩm vào state
//       } catch (error) {
//         this.error = error; // Lưu lỗi nếu xảy ra
//         console.error('Error fetching products:', error);
//       }
//     },
//     // Lấy danh sách product đang sale
//     async callProduct_isSale(){
//       try {
//         const response = await apiClient.get('/api/ProductAPI/GetAllProduct-IsSale');
//         this.products_isSale = response.data.model; // Lưu dữ liệu sản phẩm vào state
//       } catch (error) {
//         this.error = error; // Lưu lỗi nếu xảy ra
//         console.error('Error fetching products is sale:', error);
//       }
//     },    
//     // Đưa product sang trạng thái bán
//     async turnOnProduct(id){
//       try {
//         const response = await apiClient.put(`/api/ProductAPI/TurnOnProducts?id=${id}`);

//         this.setIsSuccess(response.data.success);      
//       } catch (error) {
//         this.error = error; 
//         console.error('Error fetching products is sale:', error);
//       }
//     }, 
//     // Đưa product sang trạng thái chưa bán
//     async turnOffProduct(id){
//       try {
//         const response = await apiClient.put(`/api/ProductAPI/DeleteProducts?id=${id}`);

//         this.setIsSuccess(response.data.success);
//       } catch (error) {
//         this.error = error; 
//         console.error('Error fetching products is sale:', error);
//       }
//     },           
//     // Tạo productOption
//     async createProductOption(newProductOption) {
//       try {
//         const response = await apiClient.post(`/api/ProductOptionAPI/CreateProductOptions`, newProductOption);       
//         this.setIsSuccess(response.data.success);
//       } catch (error) {
//         console.error('Error creating new product:', error);
//       }
//     },    
//     // Cập nhật productOption
//     async updateProductOption(id, newProductOption) {
//       try {
//         const response = await apiClient.put(`/api/ProductOptionAPI/UpdateProductOptions?id=${id}`, newProductOption, {
//           params: {id: id}
//         });       
//         this.setIsSuccess(response.data.success);
//       } catch (error) {
//         console.error('Error creating new product:', error);
//       }
//     },      
//     // Lấy danh sách productOption bằng id của product 
//     async callProductOptionsByProductId(productId){
//       try {
//         const response = await apiClient.get(`/api/ProductOptionAPI/GetProductOptionByProductId`,{
//           params: {productId: productId}
//         });
//         this.optionIsSelect = response.data.model; // Lưu dữ liệu sản phẩm vào state
        
//       } catch (error) {
//         this.error = error; // Lưu lỗi nếu xảy ra
//         console.error('Error fetching products:', error);
//       }
//     },
//     // Lấy 1 productOption bằng id
//     async callProductOptionsById(id){
//       try {
//         const response = await apiClient.get(`/api/ProductOptionAPI/GetProductOptions`,{
//           params: { id: id }
//         });
        
//         this.optionById = response.data.model;
//       } catch (error) {
//         this.error = error; // Lưu lỗi nếu xảy ra
//         console.error('Error fetching products:', error);
//       }
//     },
//     // Lấy danh sách tất cả productOption
//     async callProductOptions(){
//       try {
//         const response = await apiClient.get('/api/ProductOptionAPI/GetAllProductOption');
//         this.productOptions = response.data.model; // Lưu dữ liệu sản phẩm vào state
//       } catch (error) {
//         this.error = error; // Lưu lỗi nếu xảy ra
//         console.error('Error fetching productOptions:', error);
//       }
//     },


//   // actions get state
    
//     // lấy 1 product
//     getProductIsSelect() {  
//       return this.productIsSelect; 
//     },
//     // lấy danh sách productOption của 1 product
//     getOptionIsSelect() {
//       return this.optionIsSelect; 
//     },
//     getPriceMin() {
//       if (!this.optionIsSelect || this.optionIsSelect.length === 0) {
//         return 0;
//       }
//       const prices = this.optionIsSelect.map(option => option.price);
//       return Math.min(...prices); 
//     },
//     // Lấy danh sách tất cả product
//     getProducts(){
//       return this.products;
//     },
//     // Lấy danh sách product đang được bán
//     getProductOn(){
//       return this.products_isVisible;
//     },
//     // Lấy danh sách product chưa được bán
//     getProductOff(){
//       return this.products_unVisible;
//     },
//     // Lấy danh sách product đang sale
//     getProductSale(){
//       return this.products_isSale;
//     },
//     // Lấy trạng thái hành động
//     getIsSuccess(){
//       return this.isSuccess;
//     },
//     // Lấy giá trị product đang được xem cấu hình chi tiết - admin/product
//     getProductSee(){
//       return this.productIsSee;
//     },
//     // Lấy giá trị tất cả productOption
//     getProductOptions(){
//       return this.productOptions;
//     },
//     getProductOption(){
//       return this.optionById
//     },
//   // function set state value
//     setIsSuccess(newStatus){
//       this.isSuccess = newStatus;
//     },
//     // Đặt giá trị product đang được xem cấu hình chi tiết - admin/product
//     setProductSee(product){
//       this.productIsSee = product;
//     }
//   },
//   persist: {
//     enabled: true,
//     strategies: [
//       {
//         key: 'productStore', // Tên lưu trong localStorage
//         storage: localStorage, // Sử dụng localStorage
//       },
//     ],
//   },
// });
