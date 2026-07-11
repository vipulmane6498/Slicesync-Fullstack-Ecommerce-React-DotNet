import { Link } from 'react-router-dom';
import Username from '../features/user/Username';
import NavLinks from './NavLinks';

function Header() {
  return (
    <header className="flex items-center justify-between border-b border-stone-200 bg-yellow-400 px-4 py-3 uppercase sm:px-6">
      <Link to="/" className="tracking-widest">
        SliceSync
      </Link>

      <div className="flex items-center gap-4">
        <NavLinks />
        <Username />
      </div>
    </header>
  );
}

export default Header;
