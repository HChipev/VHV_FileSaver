<template>
  <section class="flex flex-1 bg-xanthous">
    <div
      class="flex flex-1 flex-col items-center justify-center px-6 py-8 mx-auto">
      <div
        class="w-full bg-white rounded-lg shadow dark:border md:mt-0 sm:max-w-md xl:p-0">
        <div class="p-6 space-y-4 md:space-y-6 sm:p-8">
          <h1
            class="text-xl font-bold leading-tight tracking-tight text-gray-900 md:text-2xl">
            Create your account
          </h1>
          <form class="space-y-4 md:space-y-6" v-on:submit.prevent="register">
            <div>
              <label
                for="firstName"
                class="block mb-2 text-sm font-medium text-gray-900"
                >Your first name</label
              >
              <input
                v-model="firstName"
                type="text"
                name="firstName"
                id="firstName"
                class="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                placeholder="John"
                required />
            </div>
            <div>
              <label
                for="lastName"
                class="block mb-2 text-sm font-medium text-gray-900"
                >Your last name</label
              >
              <input
                v-model="lastName"
                type="text"
                name="lastName"
                id="lastName"
                class="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                placeholder="Doe"
                required />
            </div>
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
              v-if="errorMessage || successMessage"
              class="flex justify-center items-cente"
              :class="!successMessage ? 'text-red-600' : 'text-green-600'">
              {{ errorMessage || successMessage }}
            </p>
            <button
              type="submit"
              class="w-full text-white bg-yale-blue hover:bg-primary-700 focus:ring-4 focus:outline-none focus:ring-primary-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center">
              Sign up
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
  const firstName = ref("");
  const lastName = ref("");
  const errorMessage = ref(null);
  const successMessage = ref(null);

  async function register() {
    const response = await fetch(
      "https://localhost:7124/api/Identity/register",
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("access-token")}`,
        },
        body: JSON.stringify({
          firstName: firstName.value,
          lastName: lastName.value,
          email: email.value,
          password: password.value,
        }),
      }
    );
    if (response.ok) {
      errorMessage.value = null;
      successMessage.value = "Account added successfully";
    } else {
      successMessage.value = null;
      errorMessage.value = await response.text();
    }
  }
</script>
<style></style>
