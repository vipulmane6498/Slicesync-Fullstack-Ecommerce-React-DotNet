import Header from './Header';
import Loader from './Loader';
import CartOverview from '../features/cart/CartOverview';
import { Outlet, useNavigation } from 'react-router-dom';

function AppLayout() {
  const navigation = useNavigation();
  const isLoading = navigation.state === 'loading';

  return (
    <div className="grid min-h-screen grid-rows-[auto_1fr_auto]">
      {isLoading && <Loader />}

      <Header />

      <main className="mx-auto w-full max-w-3xl">
        <Outlet />
      </main>

      <CartOverview />
    </div>
  );
}

export default AppLayout;
