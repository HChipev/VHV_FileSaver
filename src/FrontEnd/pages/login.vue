<template>
  <section class="flex flex-1 bg-xanthous">
    <div
      class="flex flex-1 flex-col items-center justify-center px-6 py-8 mx-auto">
      <div
        class="w-full bg-white rounded-lg shadow dark:border md:mt-0 sm:max-w-md xl:p-0">
        <div class="p-6 space-y-4 md:space-y-6 sm:p-8">
          <h1
            class="text-xl font-bold leading-tight tracking-tight text-gray-900 md:text-2xl">
            Sign in to your account
          </h1>
          <form class="space-y-4 md:space-y-6" v-on:submit.prevent="login">
            <div>
              <label
                for="email"
                class="block mb-2 text-sm font-medium text-gray-900"
                >Your email</label
              >
              <input
                v-model="email"
                type="email"
                name="email"
                id="email"
                class="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                placeholder="user@email.com"
                required />
            </div>
            <div>
              <label
                for="password"
                class="block mb-2 text-sm font-medium text-gray-900"
                >Password</label
              >
              <input
                v-model="password"
                type="password"
                name="password"
                id="password"
                placeholder="••••••••"
                class="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                required />
            </div>
            <p
              v-if="message"
              class="flex justify-center items-center text-red-600">
              {{ message }}
            </p>
            <button
              type="submit"
              class="w-full text-white bg-yale-blue hover:bg-primary-700 focus:ring-4 focus:outline-none focus:ring-primary-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center">
              Sign in
            </button>
          </form>
        </div>
      </div>
    </div>
  </section>
</template>
<script setup>
  const email = ref("");
  const password = ref("");
  const message = ref(null);

  async function login() {
    const response = await fetch("https://localhost:7124/api/Identity/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        email: email.value,
        password: password.value,
      }),
    });
    if (response.ok) {
      message.value = null;
      localStorage.setItem(
        "access-token",
        response.headers.get("Access-Token")
      );
      localStorage.setItem(
        "refresh-token",
        response.headers.get("Refresh-Token")
      );
      navigateTo("/");
    } else {
      message.value = await response.text();
    }
  }
</script>
<style></style>
