function About() {
  return (
    <div className="px-4 py-6">
      <div className="mx-auto max-w-2xl">
        <h2 className="mb-2 text-xl font-semibold">
          About SliceSync
        </h2>
        <p className="mb-8 text-stone-500">
          Straight out of the oven, straight to you.
        </p>

        <div className="space-y-8">
          <section>
            <h3 className="mb-3 text-base font-semibold uppercase tracking-wide text-yellow-600">
              Our Story
            </h3>
            <p className="text-sm leading-relaxed text-stone-600">
              SliceSync started in Pune with one simple idea: great pizza should
              be fast, fresh, and fuss-free. We hand-stretch every dough, use
              quality toppings, and get it to your door while it is still piping
              hot.
            </p>
          </section>

          <section>
            <h3 className="mb-3 text-base font-semibold uppercase tracking-wide text-yellow-600">
              Why Choose Us
            </h3>
            <ul className="space-y-2 text-sm text-stone-600">
              {[
                '🍕 Fresh dough made daily — never frozen',
                '🚀 Delivery in 30–45 minutes, priority orders even faster',
                '⭐ Priority orders get bumped to the front of the queue',
                '📍 Real-time order tracking so you always know where your pizza is',
                '🔒 Secure accounts so your order history is always at hand',
              ].map((point) => (
                <li
                  key={point}
                  className="flex items-start gap-2 rounded-xl bg-stone-50 px-4 py-3"
                >
                  {point}
                </li>
              ))}
            </ul>
          </section>

          <section className="rounded-xl bg-yellow-400 px-6 py-5">
            <h3 className="mb-1 text-base font-semibold uppercase tracking-wide">
              Get in Touch
            </h3>
            <p className="text-sm text-stone-700">
              📍 FC Road, Shivajinagar, Pune, Maharashtra 411005
            </p>
            <p className="text-sm text-stone-700">📞 +91 20 4123 4567</p>
            <p className="text-sm text-stone-700">
              ✉️ hello@slicesync.in
            </p>
          </section>
        </div>
      </div>
    </div>
  );
}

export default About;
