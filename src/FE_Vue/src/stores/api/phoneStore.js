import { defineStore } from "pinia";
import phoneService from "@/services/phoneService";
import phoneImageService from "@/services/phoneImageService";
import phoneVariantService from "@/services/phoneVariantService";

export const usePhoneStore = defineStore("phone", {
  state: () => ({
    phones: [],
    activePhones: [],
    selectedPhone: null,
    loading: false,
    error: null,
  }),

  actions: {
    async fetchAllPhones() {
      this.loading = true;
      try {
        const res = await phoneService.getAllPhones();
        this.phones = res.data.model || [];
      } catch (err) {
        this.error = err;
      } finally {
        this.loading = false;
      }
    },

    async fetchActivePhones() {
      this.loading = true;
      try {
        const res = await phoneService.getActivePhones();
        this.activePhones = res.data.model || [];
      } catch (err) {
        this.error = err;
      } finally {
        this.loading = false;
      }
    },

    async fetchPhoneById(phoneId) {
      this.loading = true;
      try {
        const res = await phoneService.getPhoneById(phoneId);
        this.selectedPhone = res.data.model;
        console.log(this.selectedPhone);
        
      } catch (err) {
        this.error = err;
      } finally {
        this.loading = false;
      }
    },

    async createPhone(phoneData) {
      try {
        const res = await phoneService.createPhone(phoneData);
        await this.fetchAllPhones();
        return res.data;
      } catch (err) {
        this.error = err;
        throw err;
      }
    },

    async updatePhone(phoneId, phoneData) {
      try {
        const res = await phoneService.updatePhone(phoneId, phoneData);
        await this.fetchAllPhones();
        return res.data;
      } catch (err) {
        this.error = err;
        throw err;
      }
    },

    async toggleActive(phoneId) {
      try {
        const res = await phoneService.toggleActive(phoneId);
        await this.fetchAllPhones();
        return res.data;
      } catch (err) {
        this.error = err;
        throw err;
      }
    },

    async deletePhone(phoneId) {
      try {
        const res = await phoneService.deletePhone(phoneId);
        await this.fetchAllPhones();
        return res.data;
      } catch (err) {
        this.error = err;
        throw err;
      }
    },

    // Phone Image Actions
    async createPhoneImage(data) {
      try {
        const res = await phoneImageService.createPhoneImage(data);
        // Optionally refetch images if needed
        return res.data;
      } catch (err) {
        this.error = err;
        throw err;
      }
    },

    async updatePhoneImage(phoneImageId, data) {
      try {
        const res = await phoneImageService.updatePhoneImage(phoneImageId, data);
        // Optionally refetch images if needed
        return res.data;
      } catch (err) {
        this.error = err;
        throw err;
      }
    },

    async deletePhoneImage(phoneImageId) {
      try {
        const res = await phoneImageService.deletePhoneImage(phoneImageId);
        // Optionally refetch images if needed
        return res.data;
      } catch (err) {
        this.error = err;
        throw err;
      }
    },

    // Phone Variant Actions
    async createPhoneVariant(data) {
      try {
        const res = await phoneVariantService.createPhoneVariant(data);
        // Optionally refetch variants if needed
        return res.data;
      } catch (err) {
        this.error = err;
        throw err;
      }
    },

    async updatePhoneVariant(variantId, data) {
      try {
        const res = await phoneVariantService.updatePhoneVariant(variantId, data);
        // Optionally refetch variants if needed
        return res.data;
      } catch (err) {
        this.error = err;
        throw err;
      }
    },

  },

  getters: {
    phoneList(){
      return this.phones;
    },

    activePhoneList(){
      return this.activePhones;
    },

    getSelectedPhone(){
      return this.selectedPhone;
    },
  },

});
