<template>
  <div class="about-page">
    <!-- Header section -->
    <div class="about-header">
      <img src="/src/assets/images/logo/nhh.png" alt="Logo" class="about-header-logo center-logo" />
    </div>
    <!-- Hero section -->
    <div class="about-hero">
      <h1 class="about-title">Giới thiệu về NHH Store</h1>
      <p class="about-slogan">Nơi bạn tìm thấy chiếc điện thoại phù hợp nhất!</p>
    </div>
    <!-- Content section -->
    <div class="about-content">
      <h2>Về chúng tôi</h2>
      <p>
        NHH Store là website chuyên cung cấp các sản phẩm điện thoại di động chính hãng, đa dạng mẫu mã, giá cả cạnh tranh và dịch vụ khách hàng tận tâm. Chúng tôi cam kết mang đến cho bạn trải nghiệm mua sắm trực tuyến an toàn, tiện lợi và nhanh chóng.
      </p>
      <h2>Lý do chọn NHH Store?</h2>
      <ul>
        <li>Đa dạng sản phẩm từ các thương hiệu nổi tiếng: Samsung, iPhone, Realme, Xiaomi...</li>
        <li>Giá cả minh bạch, nhiều chương trình khuyến mãi hấp dẫn.</li>
        <li>Hỗ trợ tư vấn, bảo hành và đổi trả linh hoạt.</li>
        <li>Giao hàng nhanh chóng trên toàn quốc.</li>
        <li>Đội ngũ nhân viên chuyên nghiệp, nhiệt tình.</li>
      </ul>
      <h2>Liên hệ</h2>
      <p>
        Địa chỉ: 123 Đường ABC, Quận XYZ, TP. Hồ Chí Minh<br>
        Hotline: 0123 456 789<br>
        Email: support@nhhstore.vn
      </p>
    </div>
  </div>
</template>

<script setup>
// ...existing code...
</script>

<style scoped>
.about-header {
  width: 100%;
  background: linear-gradient(90deg, #ff9800 60%, #ff5722 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 18px 18px 0 0;
  margin-bottom: 24px;
}
.about-header-logo.center-logo {
  width: 140px;
  height: 140px;
  object-fit: contain;
  margin: 0 auto;
  display: block;
}
.about-header-title {
  font-size: 2rem;
  font-weight: 700;
  color: #fff;
  letter-spacing: 2px;
}
.about-page {
  max-width: 900px;
  margin: 0 auto;
  font-family: 'Inter', 'Segoe UI', sans-serif;
  background: #fff;
  border-radius: 18px;
  box-shadow: 0 4px 24px #e0e0e0;
}
.about-hero {
  text-align: center;
  margin-bottom: 10px;
}
.about-logo {
  display: none;
}
.about-title {
  font-size: 2.2rem;
  font-weight: 700;
  color: #ff5722;
  margin-bottom: 8px;
}
.about-slogan {
  font-size: 1.2rem;
  color: #ff9800;
}
.about-content{
    padding: 0 32px 10px 32px;
}
.about-content h2 {
  color: #e65100;
  font-size: 1.3rem;
  margin-top: 0px;
  margin-bottom: 10px;
}
.about-content p, .about-content ul {
  font-size: 1rem;
  color: #444;
  margin-bottom: 12px;
}
.about-content ul {
  padding-left: 22px;
}
.about-content li {
  margin-bottom: 6px;
}
</style>