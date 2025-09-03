import { defineStore } from 'pinia';

export const useServerStore = defineStore('server', () => {
    const url = 'https://localhost:7269';

    return {
        url,
    };
});
