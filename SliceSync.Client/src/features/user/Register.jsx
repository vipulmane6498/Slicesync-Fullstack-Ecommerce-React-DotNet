import { Form, redirect, useActionData, useNavigation } from 'react-router-dom';
import { register } from '../../services/apiAuth';
import { loginUser } from './userSlice';
import store from '../../store';
import Button from '../../ui/Button';
import LinkButton from '../../ui/LinkButton';

function Register() {
  const formErrors = useActionData();
  const navigation = useNavigation();
  const isSubmitting = navigation.state === 'submitting';

  return (
    <div className="my-10 px-4 text-center sm:my-16">
      <h2 className="mb-8 text-xl font-semibold md:text-3xl">
        Join us today!
        <br />
        <span className="text-yellow-500">Fresh pizza, delivered fast.</span>
      </h2>

      <Form method="POST" className="mx-auto max-w-sm">
        <div className="mb-5 flex flex-col gap-2">
          <label className="text-sm font-medium text-stone-700">Full Name</label>
          <input
            className="input w-full"
            type="text"
            name="fullName"
            placeholder="Your full name"
            required
          />
        </div>

        <div className="mb-5 flex flex-col gap-2">
          <label className="text-sm font-medium text-stone-700">Email</label>
          <input
            className="input w-full"
            type="email"
            name="email"
            placeholder="you@slicesync.in"
            required
          />
        </div>

        <div className="mb-5 flex flex-col gap-2">
          <label className="text-sm font-medium text-stone-700">Phone Number</label>
          <input
            className="input w-full"
            type="tel"
            name="phoneNumber"
            placeholder="+91 98765 43210"
          />
        </div>

        <div className="mb-5 flex flex-col gap-2">
          <label className="text-sm font-medium text-stone-700">Password</label>
          <input
            className="input w-full"
            type="password"
            name="password"
            placeholder="Create a password"
            required
          />
        </div>

        {formErrors?.message && (
          <p className="mb-4 rounded-md bg-red-100 p-2 text-xs text-red-700">
            {formErrors.message}
          </p>
        )}

        <div className="space-y-3">
          <Button disabled={isSubmitting} type="primary">
            {isSubmitting ? 'Creating account...' : 'Create account'}
          </Button>
        </div>

        <p className="mt-6 text-sm text-stone-600">
          Already have an account?{' '}
          <LinkButton to="/login">Sign in</LinkButton>
        </p>
      </Form>
    </div>
  );
}

export async function action({ request }) {
  const formData = await request.formData();
  const fullName = formData.get('fullName');
  const email = formData.get('email');
  const password = formData.get('password');
  const phoneNumber = formData.get('phoneNumber');

  try {
    const authData = await register({ fullName, email, password, phoneNumber });
    store.dispatch(loginUser(authData));
    return redirect('/menu');
  } catch (err) {
    return { message: err.message };
  }
}

export default Register;
