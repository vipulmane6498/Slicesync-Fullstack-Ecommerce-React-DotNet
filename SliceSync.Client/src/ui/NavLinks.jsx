import { Link, useNavigate } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { logoutUser } from '../features/user/userSlice';
import { clearCart } from '../features/cart/cartSlice';

function NavLinks() {
  const token = useSelector((state) => state.user.token);
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const linkClass =
    'text-xs font-semibold tracking-widest text-stone-700 transition-colors hover:text-stone-900';

  function handleLogout() {
    dispatch(logoutUser());
    dispatch(clearCart());
    navigate('/');
  }

  return (
    <nav className="hidden items-center gap-4 sm:flex">
      <Link to="/about" className={linkClass}>
        About
      </Link>

      {token ? (
        <>
          <Link to="/orders" className={linkClass}>
            My Orders
          </Link>
          <button onClick={handleLogout} className={linkClass}>
            LOGOUT
          </button>
        </>
      ) : (
        <>
          <Link to="/login" className={linkClass}>
            Login
          </Link>
          <Link to="/register" className={linkClass}>
            Register
          </Link>
        </>
      )}
    </nav>
  );
}

export default NavLinks;
