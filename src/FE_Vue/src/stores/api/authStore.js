import { defineStore } from "pinia";
import authService from "@/services/authService";

export const useAuthStore = defineStore("auth", {
  state: () => ({
    error: null,
    message: null,
    success: false
  }),

  actions: {
    async login(credentials) {
      try {
        const res = await authService.login(credentials);

        if(res.data.success){
        const token = res.data.model.accessToken;
        
        sessionStorage.setItem('token', token);
        }

        this.success = res.data.success;
        this.message = res.data.message;
      } catch (err) {
        this.error = err;
        throw err;
      }
    },

    async register(data) {
      try {
        const res = await authService.register(data);
        return res.data;
      } catch (err) {
        this.error = err;
        throw err;
      }
    },

    async logout(userId) {
      try {
        const res = await authService.logout(userId);

        sessionStorage.removeItem('token');

        this.success = res.data.success;
        this.message = res.data.message;
      } catch (err) {
        this.error = err;
        throw err;
      }
    },

    async changePassword(userId, data) {
      try {
        const res = await authService.changePassword(userId, data);
        return res.data
      } catch (err) {
        this.error = err;
        throw err;
      }
    },

        /**
     * Gửi yêu cầu mã OTP và cập nhật state
     * @param {object} data - Dữ liệu cần thiết để gửi OTP (ví dụ: email)
     */
    async sendOtp(data) {
      try {
        const res = await authService.sendOtp(data);
        this.success = res.data.success;
        this.message = res.data.message;
        return res.data;
      } catch (err) {
        this.error = err;
        this.success = false;
        this.message = "Đã xảy ra lỗi khi gửi mã OTP.";
        throw err;
      }
    },

    /**
     * Xác nhận mã OTP và cập nhật state
     * @param {object} data - Dữ liệu cần thiết để xác nhận OTP (ví dụ: email, otp)
     */
    async confirmOtp(data) {
      try {
        const res = await authService.confirmOtp(data);
        this.success = res.data.success;
        this.message = res.data.message;
        return res.data;
      } catch (err) {
        this.error = err;
        this.success = false;
        this.message = "Đã xảy ra lỗi khi xác nhận mã OTP.";
        throw err;
      }
    },

    setSuccess(value) {
      this.success = value;
    },
  },


});
