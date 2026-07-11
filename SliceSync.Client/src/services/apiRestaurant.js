import store from '../store';

const API_URL = 'https://localhost:7094/api';

function authHeaders() {
  const token = store.getState().user.token;
  return token ? { Authorization: `Bearer ${token}` } : {};
}

function getCurrentUserContext() {
  const state = store.getState();
  return {
    token: state.user.token,
    userId: state.user.userId,
  };
}

export async function getMenu() {
  const res = await fetch(`${API_URL}/menu`);

  // fetch won't throw error on 400 errors (e.g. when URL is wrong), so we need to do it manually. This will then go into the catch block, where the message is set
  if (!res.ok) throw Error('Failed getting menu');

  const menu = await res.json();
  return menu;
}

export async function getOrder(id) {
  const res = await fetch(`${API_URL}/orders/${id}`);
  if (!res.ok) throw Error(`Couldn't find order #${id}`);

  const order = await res.json();
  return order;
}

export async function createOrder(newOrder) {
  try {
    const res = await fetch(`${API_URL}/orders`, {
      method: 'POST',
      body: JSON.stringify(newOrder),
      headers: {
        'Content-Type': 'application/json',
        ...authHeaders(),
      },
    });

    if (!res.ok) throw Error();
    const order = await res.json();
    return order;
  } catch {
    throw Error('Failed creating your order');
  }
}

export async function updateOrder(id, updateObj) {
  try {
    const res = await fetch(`${API_URL}/orders/${id}`, {
      method: 'PATCH',
      body: JSON.stringify(updateObj),
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (!res.ok) throw Error();
    // We don't need the data, so we don't return anything
  } catch (err) {
    throw Error('Failed updating your order');
  }
}

export async function getMyOrders() {
  const res = await fetch(`${API_URL}/orders/mine`, {
    headers: {
      ...authHeaders(),
    },
  });

  if (!res.ok) throw Error('Failed to load your orders');

  const orders = await res.json();
  return orders;
}

export async function addToCart(pizzaId) {
  const { token, userId } = getCurrentUserContext();
  if (!token || !userId) return;

  const res = await fetch(`${API_URL}/customer/addtocart`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      ...authHeaders(),
    },
    body: JSON.stringify({ userId, pizzaId }),
  });

  if (!res.ok) throw Error('Failed to add pizza to server cart');
}

export async function removeFromCart(pizzaId) {
  const { token, userId } = getCurrentUserContext();
  if (!token || !userId) return;

  const res = await fetch(`${API_URL}/customer/removefromcart`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      ...authHeaders(),
    },
    body: JSON.stringify({ userId, pizzaId }),
  });

  if (!res.ok) throw Error('Failed to remove pizza from server cart');
}
