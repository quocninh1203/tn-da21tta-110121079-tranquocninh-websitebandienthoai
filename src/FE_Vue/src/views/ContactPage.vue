<template>
  <div class="contact-page">
    <div class="contact-header">
      <img src="/src/assets/images/logo/nhh.png" alt="Logo" class="contact-logo" />
      <h1 class="contact-title">Liên hệ NHH Store</h1>
      <p class="contact-slogan">Chúng tôi luôn sẵn sàng hỗ trợ bạn!</p>
    </div>
    <div class="contact-content">
      <h2>Thông tin liên hệ</h2>
      <ul>
        <li><strong>Địa chỉ:</strong> 123 Đường ABC, Quận XYZ, TP. Hồ Chí Minh</li>
        <li><strong>Hotline:</strong> 0123 456 789</li>
        <li><strong>Email:</strong> support@nhhstore.vn</li>
        <li><strong>Facebook:</strong> <a href="https://facebook.com/nhhstore" target="_blank">facebook.com/nhhstore</a></li>
      </ul>
      <h2>Gửi phản hồi</h2>
      <form class="contact-form" @submit.prevent="submitForm">
        <input type="text" v-model="name" placeholder="Họ và tên" required />
        <input type="email" v-model="email" placeholder="Email" required />
        <textarea v-model="message" placeholder="Nội dung liên hệ" rows="4" required></textarea>
        <button type="submit" class="btn-send">Gửi liên hệ</button>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
const name = ref('')
const email = ref('')
const message = ref('')

const submitForm = () => {
  // Xử lý gửi liên hệ, có thể gọi API hoặc hiển thị thông báo
  alert('Cảm ơn bạn đã liên hệ! Chúng tôi sẽ phản hồi sớm nhất.')
  name.value = ''
  email.value = ''
  message.value = ''
}
</script>

<style scoped>
.contact-page {
  max-width: 700px;
  margin: 0 auto;
  padding: 32px 18px;
  font-family: 'Inter', 'Segoe UI', sans-serif;
  background: #fff;
  border-radius: 18px;
  box-shadow: 0 4px 24px #e0e0e0;
}
.contact-header {
  text-align: center;
  margin-bottom: 32px;
}
.contact-logo {
  width: 80px;
  height: 80px;
  object-fit: contain;
  margin-bottom: 14px;
}
.contact-title {
  font-size: 2rem;
  font-weight: 700;
  color: #ff5722;
  margin-bottom: 8px;
}
.contact-slogan {
  font-size: 1.1rem;
  color: #ff9800;
  margin-bottom: 18px;
}
.contact-content h2 {
  color: #e65100;
  font-size: 1.15rem;
  margin-top: 18px;
  margin-bottom: 10px;
}
.contact-content ul {
  font-size: 1rem;
  color: #444;
  margin-bottom: 18px;
  padding-left: 18px;
}
.contact-content li {
  margin-bottom: 6px;
}
.contact-content a {
  color: #1976d2;
  text-decoration: underline;
}
.contact-form {
  display: flex;
  flex-direction: column;
  gap: 12px;
  margin-top: 10px;
}
.contact-form input,
.contact-form textarea {
  padding: 10px 12px;
  border-radius: 8px;
  border: 1.5px solid #ffccbc;
  font-size: 1rem;
  outline: none;
  background: #fff8f0;
  color: #e65100;
  transition: border 0.2s;
}
.contact-form input:focus,
.contact-form textarea:focus {
  border-color: #ff9800;
}
.btn-send {
  background: linear-gradient(90deg, #ff9800 60%, #ff5722 100%);
  color: #fff;
  font-weight: bold;
  border: none;
  border-radius: 8px;
  padding: 10px 0;
  font-size: 1rem;
  cursor: pointer;
  transition: background 0.2s;
}
.btn-send:hover {
  background: linear-gradient(90deg, #e65100 60%, #ff9800 100%);
}
</style>
