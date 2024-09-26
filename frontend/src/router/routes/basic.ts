import { RouteRecordRaw } from "vue-router";
import Home from "@/views/Home.vue";
import About from "@/views/About.vue";
import Privacy from "@/views/Privacy.vue";

export enum RouteName {
  Home = "Home",
  About = "About",
  Error = "Error",
}

export const basicRoutes: RouteRecordRaw[] = [
  {
    path: "/",
    name: RouteName.Home,
    component: Home,
  },
  {
    path: "/about",
    name: RouteName.About,
    component: About,
  },
  {
    path: "/privacy",
    name: "Privacy",
    component: Privacy,
  },
];
