import { useDispatch } from 'react-redux';
import { useSelector } from 'react-redux';
import Button from '../../ui/Button';
import { deleteItem } from './cartSlice';
import { removeFromCart } from '../../services/apiRestaurant';

function DeleteItem({ pizzaId, quantity = 1 }) {
  const dispatch = useDispatch();
  const { token, userId } = useSelector((state) => state.user);

  async function handleDelete() {
    if (token && userId) {
      for (let i = 0; i < quantity; i++) {
        await removeFromCart(pizzaId);
      }
    }

    dispatch(deleteItem(pizzaId));
  }

  return (
    <Button type="small" onClick={handleDelete}>
      Delete
    </Button>
  );
}

export default DeleteItem;
