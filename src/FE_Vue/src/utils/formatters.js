/**
 * Định dạng số thành tiền VND (có icon)
 * @param {number} value - Số tiền
 * @returns {string} - Chuỗi định dạng: "7.450.000 ₫"
 */
export function formatVND(value) {
  if (value == null) return '';
  const formatted = value.toLocaleString('vi-VN');
  return `${formatted} ₫`;
}

/**
 * Chuyển File thành chuỗi base64 (Data URI)
 * @param {File} file - File ảnh
 * @returns {Promise<string>} - Base64 dạng: data:image/png;base64,...
 */
export function convertToBase64(file) {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.onload = () => resolve(reader.result);
    reader.onerror = error => reject(error);
    reader.readAsDataURL(file);
  });
}

/**
 * Chuyển ngày đánh giá thành dạng thời gian tương đối
 * @param {string} dateString - ISO date string
 * @returns {string} - Ví dụ: "15 phút trước", "3 giờ trước", "2 ngày trước"
 */
export function formatTimeAgo(dateString) {
  const now = new Date();
  const reviewDate = new Date(dateString);
  const diffMs = now - reviewDate;

  const minutes = Math.floor(diffMs / (1000 * 60));
  const hours = Math.floor(diffMs / (1000 * 60 * 60));
  const days = Math.floor(diffMs / (1000 * 60 * 60 * 24));

  if (minutes < 1) return 'Vừa xong';
  if (minutes < 60) return `${minutes} phút trước`;
  if (hours < 24) return `${hours} giờ trước`;
  if (days === 1) return '1 ngày trước';
  return `${days} ngày trước`;
}

/**
 * Nhận vào kiểu datetime, trả ra ngày/tháng/năm
 * @param {string|Date} dateInput - Chuỗi hoặc đối tượng Date
 * @returns {string} - Chuỗi định dạng: "dd/mm/yyyy"
 */
export function formatDateDMY(dateInput) {
  const d = new Date(dateInput)
  if (isNaN(d)) return ''
  const day = String(d.getDate()).padStart(2, '0')
  const month = String(d.getMonth() + 1).padStart(2, '0')
  const year = d.getFullYear()
  return `${day}/${month}/${year}`
}
