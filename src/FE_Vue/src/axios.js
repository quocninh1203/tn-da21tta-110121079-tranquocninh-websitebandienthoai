import axios from 'axios'; // Import axios để gọi API

// Tạo một instance axios với cấu hình mặc định
const apiClient = axios.create({
  baseURL: 'https://localhost:7269', 
  headers: {
    'Content-Type': 'application/json', // Header mặc định cho JSON
  },
});

apiClient.interceptors.request.use(
  (config) => {
    // Lấy token từ localStorage
    const userStore = JSON.parse(localStorage.getItem('userStore'));
    const token = userStore?.Token;

    // Nếu có token, thêm vào header Authorization
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;    
    }else {
      console.warn('Token không tồn tại trong userStore');  
    }

    console.log("token: ",token);
    return config; // Trả về config đã được sửa đổi
  },
  (error) => {
    // Xử lý lỗi trong interceptor
    console.error('Lỗi khi parse userStore từ localStorage:', error);
    return Promise.reject(error);
  }
);

export default apiClient; // Export instance để sử dụng trong các file khác
