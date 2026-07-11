import { useLoaderData, useNavigate, Link, redirect } from 'react-router-dom';
import { getMyOrders } from '../../services/apiRestaurant';
import { formatCurrency, formatDate } from '../../utils/helpers';
import store from '../../store';
import SearchOrder from './SearchOrder';

const STATUS_COLORS = {
  OrderConfirmed: 'bg-blue-500',
  Preparing: 'bg-yellow-500',
  OutForDelivery: 'bg-orange-500',
  Delivered: 'bg-green-500',
  Cancelled: 'bg-stone-400',
  Returned: 'bg-red-500',
  Pending: 'bg-stone-300 text-stone-700',
};

function Orders() {
  const orders = useLoaderData();
  const navigate = useNavigate();

  if (!orders || orders.length === 0) {
    return (
      <div className="px-4 py-6 text-center">
        <h2 className="mb-4 text-xl font-semibold">My Orders</h2>
        <div className="mb-6 flex justify-center">
          <SearchOrder />
        </div>
        <p className="text-stone-500">
          You haven&apos;t placed any orders yet.
        </p>
        <button
          onClick={() => navigate('/menu')}
          className="mt-6 inline-block rounded-full bg-yellow-400 px-6 py-3 text-sm font-semibold uppercase tracking-wide text-stone-800 transition-colors duration-300 hover:bg-yellow-300"
        >
          Browse menu
        </button>
      </div>
    );
  }

  return (
    <div className="px-4 py-6">
      <div className="mb-6 flex flex-wrap items-center justify-between gap-3">
        <h2 className="text-xl font-semibold">My Orders</h2>
        <SearchOrder />
      </div>

      <ul className="space-y-3">
        {orders.map((order) => {
          const statusColor =
            STATUS_COLORS[order.status] ?? 'bg-stone-400';
          const itemCount = order.cart.reduce(
            (sum, item) => sum + item.quantity,
            0
          );
          const total = order.orderPrice + order.priorityPrice;

          return (
            <li key={order.id}>
              <Link
                to={`/order/${order.id}`}
                className="flex flex-wrap items-center justify-between gap-2 rounded-xl border border-stone-200 bg-stone-50 px-5 py-4 transition-colors hover:bg-stone-100"
              >
                <div className="flex items-center gap-3">
                  <span
                    className={`rounded-full px-3 py-1 text-xs font-semibold uppercase tracking-wide text-white ${statusColor}`}
                  >
                    {order.status}
                  </span>
                  {order.priority && (
                    <span className="rounded-full bg-red-500 px-3 py-1 text-xs font-semibold uppercase tracking-wide text-red-50">
                      Priority
                    </span>
                  )}
                </div>

                <div className="flex items-center gap-6">
                  <p className="text-sm text-stone-500">
                    {itemCount} pizza{itemCount !== 1 ? 's' : ''}
                  </p>
                  <p className="font-semibold">{formatCurrency(total)}</p>
                  {order.createdAt && (
                    <p className="hidden text-sm text-stone-400 sm:block">
                      {formatDate(order.createdAt)}
                    </p>
                  )}
                  <span className="text-stone-400">&rarr;</span>
                </div>
              </Link>
            </li>
          );
        })}
      </ul>
    </div>
  );
}

export async function loader() {
  const token = store.getState().user.token;
  if (!token) return redirect('/login');
  return getMyOrders();
}

export default Orders;
