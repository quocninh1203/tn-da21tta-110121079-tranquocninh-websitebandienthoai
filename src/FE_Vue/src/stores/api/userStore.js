import { defineStore } from "pinia";
import userService from "@/services/userService";

export const useUserStore = defineStore("user", {
  state: () => ({
    profile: null,
    allUsers: [],
    loading: false,
    error: null,
  }),

  actions: {
    async fetchProfile(userId) {
      this.loading = true;
      try {
        const res = await userService.getProfile(userId);
        this.profile = res.data.model;
      } catch (err) {
        this.error = err;
        throw err;
      } finally {
        this.loading = false;
      }
    },

    async updateProfile(userId, data) {
      try {
        const res = await userService.updateProfile(userId, data);
        await this.fetchProfile(userId);
        return res.data;
      } catch (err) {
        this.error = err;
        throw err;
      }
    },

    async fetchAllUsers() {
      this.loading = true;
      try {
        const res = await userService.getAllUsers();
        this.allUsers = res.data.model || [];
      } catch (err) {
        this.error = err;
      } finally {
        this.loading = false;
      }
    },
  },
});
