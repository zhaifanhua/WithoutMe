import { RouteRecordRaw } from "vue-router";
import Home from "@/views/Home.vue";
import About from "@/views/About.vue";
import Privacy from "@/views/Privacy.vue";
import Test from "@/views/Test.vue";

export enum RouteName {
  Home = "Home",
  Test = "Test",
  About = "About",
  Privacy = "Privacy",
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
  {
    path: "/test",
    name: "Test",
    component: Test,
  },
];
