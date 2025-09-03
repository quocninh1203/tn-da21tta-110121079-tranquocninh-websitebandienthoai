import api from "@/services/axiosInstance";

const authService = {
  login(credentials) {
    return api.post("/UserAPI/Login", credentials);
  },

  register(data) {
    return api.post("/UserAPI/Register", data);
  },

  logout(userId) {
    return api.put(`/UserAPI/Logout?userId=${userId}`);
  },

  refreshToken() {
    return api.post("/UserAPI/RefreshToken");
  },

  changePassword(userId, data) {
    return api.put(`/UserAPI/ChangePassWord?userId=${userId}`, data);
  },

    /**
   * Gửi yêu cầu mã OTP đến email hoặc số điện thoại của người dùng
   * @param {object} data - Dữ liệu yêu cầu, ví dụ: { email: 'user@example.com' }
   */
  sendOtp(data) {
    return api.post("/OtpAPI/SendOtp", data);
  },

  /**
   * Xác nhận mã OTP do người dùng nhập
   * @param {object} data - Dữ liệu xác nhận, ví dụ: { email: 'user@example.com', otp: '123456' }
   */
  confirmOtp(data) {
    return api.post("/OtpAPI/ConfirmOtp", data);
  },
};

export default authService;
