<template>
  <div class="flex min-h-screen flex-col justify-start">
    <header>
      <Header />
    </header>
    <slot />
    <footer class="mt-auto">
      <Footer />
    </footer>
  </div>
</template>
<script setup></script>
<style scoped></style>
