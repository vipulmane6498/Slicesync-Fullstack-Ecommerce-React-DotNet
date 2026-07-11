import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { getAddress } from '../../services/apiGeocoding';

function getPosition() {
  return new Promise(function (resolve, reject) {
    navigator.geolocation.getCurrentPosition(resolve, reject);
  });
}

export const fetchAddress = createAsyncThunk(
  'user/fetchAddress',
  async function () {
    // 1) We get the user's geolocation position
    const positionObj = await getPosition();
    const position = {
      latitude: positionObj.coords.latitude,
      longitude: positionObj.coords.longitude,
    };

    // 2) Then we use a reverse geocoding API to get a description of the user's address, so we can display it the order form, so that the user can correct it if wrong
    const addressObj = await getAddress(position);
    const locality = addressObj?.locality || 'Shivajinagar';
    const postcode = addressObj?.postcode || '411005';
    const address = `${locality}, Pune, Maharashtra ${postcode}, India`;

    // 3) Then we return an object with the data that we are interested in.
    // Payload of the FULFILLED state
    return { position, address };
  }
);

// Load persisted auth from localStorage
const storedToken = localStorage.getItem('jwtToken') || '';
const storedUsername = localStorage.getItem('username') || '';
const storedUserId = localStorage.getItem('userId') || '';

const initialState = {
  username: storedUsername,
  userId: storedUserId,
  token: storedToken,
  status: 'idle',
  position: {},
  address: '',
  error: '',
};

const userSlice = createSlice({
  name: 'user',
  initialState,
  reducers: {
    updateName(state, action) {
      state.username = action.payload;
    },
    loginUser(state, action) {
      // payload: { personName, jwtToken, userId }
      state.username = action.payload.personName || '';
      state.userId = action.payload.userId || '';
      state.token = action.payload.jwtToken || '';
      localStorage.setItem('jwtToken', state.token);
      localStorage.setItem('username', state.username);
      localStorage.setItem('userId', String(state.userId));
    },
    logoutUser(state) {
      state.username = '';
      state.userId = '';
      state.token = '';
      localStorage.removeItem('jwtToken');
      localStorage.removeItem('username');
      localStorage.removeItem('userId');
    },
  },
  extraReducers: (builder) =>
    builder
      .addCase(fetchAddress.pending, (state, action) => {
        state.status = 'loading';
      })
      .addCase(fetchAddress.fulfilled, (state, action) => {
        state.position = action.payload.position;
        state.address = action.payload.address;
        state.status = 'idle';
      })
      .addCase(fetchAddress.rejected, (state, action) => {
        state.status = 'error';
        state.error =
          'There was a problem getting your address. Make sure to fill this field!';
      }),
});

export const { updateName, loginUser, logoutUser } = userSlice.actions;

export default userSlice.reducer;
