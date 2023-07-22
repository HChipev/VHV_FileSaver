<template>
  <div class="w-screen min-w-min text-white">
    <div class="footer flex px-2 py-1 border-t border-xanthous bg-black">
      <div
        class="flex flex-1 flex-col items-center justify-end px-2 min-h-full">
        <a href="/" class="flex items-center justify-between"
          ><img
            class="logo w-32"
            src="../assets/images/logo.png"
            alt="logo-footer"
        /></a>
        <p
          class="copyright text-center whitespace-nowrap text-text-muted-color py-2">
          © {{ new Date().getFullYear() }} VHV Engineering, LTD
        </p>
      </div>
    </div>
  </div>
</template>
<script setup></script>
<style></style>
