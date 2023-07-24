import jwt_decode from "jwt-decode";

export default defineNuxtRouteMiddleware((to, from) => {
  if (!localStorage.getItem("access-token") && to.path !== "/login") {
    return navigateTo("/login");
  } else if (to.path !== "/login") {
    try {
      jwt_decode(localStorage.getItem("access-token"));
    } catch {
      return navigateTo("/login");
    }
  }

  if (
    to.path === "/admin/register" &&
    jwt_decode(localStorage.getItem("access-token")).role !== "Administrator"
  ) {
    return navigateTo("/");
  }
});
