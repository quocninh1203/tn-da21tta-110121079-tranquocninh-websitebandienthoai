import { jwtDecode } from 'jwt-decode';

export function getAccessToken() {
  return sessionStorage.getItem('token');
}

export function decodeToken() {
  const token = getAccessToken();
  if (!token) return null;
  console.log(token);
  
  try {
    return jwtDecode(token); // trả toàn bộ payload
  } catch (error) {
    console.error('Invalid token:', error);
    return null;
  }
}

export function getUserId() {
  const decoded = decodeToken();
  
  return decoded?.userId || null;
}

export function getUserRole() {
  const decoded = decodeToken();

  return decoded?.role || null; 
}
