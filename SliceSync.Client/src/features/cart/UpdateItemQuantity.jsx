import { useDispatch } from 'react-redux';
import { useSelector } from 'react-redux';
import Button from '../../ui/Button';
import { decreaseItemQuantity, increaseItemQuantity } from './cartSlice';
import { addToCart, removeFromCart } from '../../services/apiRestaurant';

function UpdateItemQuantity({ pizzaId, currentQuantity }) {
  const dispatch = useDispatch();
  const { token, userId } = useSelector((state) => state.user);

  async function handleIncrease() {
    if (token && userId) {
      await addToCart(pizzaId);
    }

    dispatch(increaseItemQuantity(pizzaId));
  }

  async function handleDecrease() {
    if (token && userId) {
      await removeFromCart(pizzaId);
    }

    dispatch(decreaseItemQuantity(pizzaId));
  }

  return (
    <div className="flex items-center gap-2 md:gap-3">
      <Button
        type="round"
        onClick={handleDecrease}
      >
        -
      </Button>
      <span className="text-sm font-medium">{currentQuantity}</span>
      <Button
        type="round"
        onClick={handleIncrease}
      >
        +
      </Button>
    </div>
  );
}

export default UpdateItemQuantity;
