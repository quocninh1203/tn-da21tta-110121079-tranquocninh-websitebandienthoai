import api from "@/services/axiosInstance";

const userService = {
  getProfile(userId) {
    return api.get(`/UserAPI/GetProfileUser?userId=${userId}`);
  },

  updateProfile(userId, data) {
    return api.put(`/UserAPI/UpdateProfile?userId=${userId}`, data);
  },

  getAllUsers() {
    return api.get("/UserAPI/GetAllUsers");
  },
};

export default userService;
