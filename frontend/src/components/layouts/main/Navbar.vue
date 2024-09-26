<template>
  <!-- navbar-start -->
  <nav id="navbar" class="navbar">
    <ul>
      <li>
        <RouterLink to="/">
          <Icon icon="iconamoon:home-duotone" />
          <span>主页</span>
        </RouterLink>
      </li>
      <li>
        <RouterLink to="/about">
          <Icon icon="tdesign:look-around" />
          <span>关于</span>
        </RouterLink>
      </li>
      <li>
        <RouterLink to="/privacy">
          <Icon icon="iconamoon:home-bold" />
          <span>隐私</span>
        </RouterLink>
      </li>
    </ul>
  </nav>
  <!-- navbar-end -->
</template>

<script setup lang="ts">
  import { Icon } from "@iconify/vue";
</script>

<style scoped lang="scss">
  @import "@/styles/base/mixins.scss";
  @import "@/styles/base/themes.scss";

  .navbar {
    grid-area: navbar;
    margin-right: $base-gap-sm;
    width: $base-navbar-width;
    height: 500px;
    @include useZindex(overlay);
    @include useBorderRadius;

    ul {
      width: 100%;
      list-style: none;

      li {
        & + li {
          margin-top: 10px;
        }

        a {
          border-radius: 10px;
          width: 100%;
          height: 50px;
          text-decoration-line: none;
          font-weight: 600;
          font-size: 18px;
          transition: all 0.2s ease-in-out;
          @include useFlexBox(row, start, center, center);
          // @include useBackdropFilter($base-module-bg-filter-blur);
          @include useBorderRadius;
          @include useTheme {
            background-color: getVar(module-bg-color);
            color: getVar(link-color);
          }

          & > * {
            margin-left: 16px;
          }
        }

        a:hover {
          @include useTheme {
            color: getVar(link-color-hover);
          }
        }

        a.active {
          @include useTheme {
            color: getVar(link-color-active);
          }
        }
      }
    }
  }
</style>
