<template>
  <div class="verify-otp-bg">
    <div class="verify-otp-card">
      <div class="verify-otp-title">Xác thực OTP Email</div>
      <div class="verify-otp-text mb-4">
        Vui lòng nhập mã OTP đã gửi tới email: <b>{{ email }}</b>
      </div>
      <form @submit.prevent="onVerify" ref="form" class="verify-otp-form">
        <div class="form-group">
          <label for="otp">
            <i class="mdi mdi-shield-key icon icon-orange"></i>
            Mã OTP
          </label>
          <input
            id="otp"
            v-model="otp"
            type="text"
            maxlength="6"
            class="custom-input"
            required
            autocomplete="one-time-code"
          />
        </div>
        <button type="submit" class="verify-btn">Xác thực</button>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/api/authStore'
import { useNotificationStore } from '@/stores/local/notification'
const authStore = useAuthStore()
const router = useRouter()

const props = defineProps({
  email: String
})

const otp = ref('')

const onVerify = async () => {
  try {
    const res = await authStore.confirmOtp({
      email: props.email,
      otpCode: otp.value
    })
    if (res && res.success) {
      useNotificationStore().addNotification(res.message, "success")
      router.push('/login')
    } else {
      useNotificationStore().addNotification(res.message, "error")
    }
  } catch (err) {
    useNotificationStore().addNotification(err.message, "error")
  }
}
</script>

<style scoped>
@import url('https://cdn.jsdelivr.net/npm/@mdi/font@7.4.47/css/materialdesignicons.min.css');

.verify-otp-bg {
  background: linear-gradient(135deg, #ffe0b2 0%, #fff3e0 100%);
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
}
.verify-otp-card {
  background: #fff;
  border-radius: 18px;
  box-shadow: 0 8px 32px 0 rgba(255, 87, 34, 0.10);
  padding: 36px 32px 28px 32px;
  max-width: 400px;
  width: 100%;
  margin: 40px 0;
  display: flex;
  flex-direction: column;
  align-items: stretch;
}
.verify-otp-title {
  font-size: 1.5rem;
  font-weight: 700;
  color: #e65100;
  text-align: center;
  margin-bottom: 18px;
}
.verify-otp-text {
  color: #e65100;
  font-size: 15px;
  text-align: center;
}
.verify-otp-form {
  display: flex;
  flex-direction: column;
  gap: 18px;
  margin-top: 10px;
}
.form-group {
  display: flex;
  flex-direction: column;
  gap: 6px;
}
label {
  font-weight: 500;
  color: #e65100;
  font-size: 15px;
  margin-bottom: 2px;
  display: flex;
  align-items: center;
  gap: 6px;
}
.custom-input {
  border: 1.5px solid #ffccbc;
  border-radius: 8px;
  padding: 12px 14px;
  font-size: 15px;
  background: #fff8f0;
  transition: border 0.2s, box-shadow 0.2s;
  outline: none;
  color: #222;
}
.custom-input:focus {
  border: 1.5px solid #ff5722;
  background: #ffe0b2;
  box-shadow: 0 2px 8px 0 #ffccbc;
}
.verify-btn {
  font-weight: bold;
  font-size: 16px;
  letter-spacing: 1px;
  border: none;
  border-radius: 8px;
  background: linear-gradient(90deg, #ff5722 60%, #ff9800 100%);
  color: #fff;
  padding: 12px 0;
  margin-top: 10px;
  box-shadow: 0 2px 8px 0 #ffccbc;
  cursor: pointer;
  transition: background 0.2s, box-shadow 0.2s;
}
.verify-btn:hover {
  background: linear-gradient(90deg, #e65100 60%, #ff5722 100%);
  box-shadow: 0 4px 16px 0 #ffccbc;
}
.icon {
  font-size: 20px;
  vertical-align: middle;
}
.icon-orange { color: #ff9800; }
.alert-error {
  margin-top: 18px;
  color: #fff;
  background: #e53935;
  border-radius: 6px;
  padding: 8px 12px;
  font-size: 15px;
  text-align: center;
}
.alert-success {
  margin-top: 18px;
  color: #fff;
  background: #43a047;
  border-radius: 6px;
  padding: 8px 12px;
  font-size: 15px;
  text-align: center;
}
</style>
