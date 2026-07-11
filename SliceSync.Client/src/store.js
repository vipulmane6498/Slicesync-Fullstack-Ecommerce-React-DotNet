import { configureStore } from '@reduxjs/toolkit';
import userReducer from './features/user/userSlice';
import cartReducer from './features/cart/cartSlice';

const store = configureStore({
  reducer: {
    user: userReducer,
    cart: cartReducer,
  },
});

// Persist cart to localStorage on every change so items survive page refresh
store.subscribe(() => {
  try {
    localStorage.setItem('cart', JSON.stringify(store.getState().cart.cart));
  } catch {
    // ignore storage errors
  }
});

export default store;
