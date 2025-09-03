import api from "@/services/axiosInstance";

const phoneService = {
  createPhone(data) {
    return api.post("/PhoneAPI/CreatePhone", data);
  },

  updatePhone(phoneId, data) {
    return api.put(`/PhoneAPI/UpdatePhone?phoneId=${phoneId}`, data);
  },

  getPhoneById(phoneId) {
    return api.get(`/PhoneAPI/GetPhone?phoneId=${phoneId}`);
  },

  getAllPhones() {
    return api.get("/PhoneAPI/GetAllPhones");
  },

  getActivePhones() {
    return api.get("/PhoneAPI/GetPhoneActivate");
  },

  toggleActive(phoneId) {
    return api.put(`/PhoneAPI/ToggleActive?phoneId=${phoneId}`);
  },

  deletePhone(phoneId) {
    return api.delete(`/PhoneAPI/DeletePhone?phoneId=${phoneId}`);
  },
};

export default phoneService;
