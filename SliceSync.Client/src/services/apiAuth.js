const API_URL = 'https://localhost:7094/api';

export async function login({ email, password }) {
  try {
    const res = await fetch(`${API_URL}/auth/login`, {
      method: 'POST',
      body: JSON.stringify({ email, password }),
      headers: { 'Content-Type': 'application/json' },
    });

    if (!res.ok) {
      const text = await res.text();
      throw new Error(text || 'Invalid email or password.');
    }

    const data = await res.json();
    return data;
  } catch (err) {
    throw new Error(err.message || 'Login failed. Please try again.');
  }
}

export async function register({ fullName, email, password, phoneNumber }) {
  try {
    const res = await fetch(`${API_URL}/auth/register`, {
      method: 'POST',
      body: JSON.stringify({
        fullName,
        email,
        password,
        phoneNumber,
        userTypeOptions: 'Customer',
      }),
      headers: { 'Content-Type': 'application/json' },
    });

    if (!res.ok) {
      const text = await res.text();
      throw new Error(text || 'Registration failed.');
    }

    const data = await res.json();
    return data;
  } catch (err) {
    throw new Error(err.message || 'Registration failed. Please try again.');
  }
}
