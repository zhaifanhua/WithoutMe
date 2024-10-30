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
  @use "@/styles/base/mixins" as mixins;
  @use "@/styles/base/themes" as themes;
  @use "@/styles/base/variables" as variables;

  .navbar {
    grid-area: navbar;
    margin-right: variables.$base-gap-sm;
    width: variables.$base-navbar-width;
    height: 500px;
    @include mixins.useZindex(overlay);
    @include mixins.useBorderRadius;

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
          @include mixins.useFlexBox(row, start, center, center);
          // @include mixins.useBackdropFilter(variables.$base-module-bg-filter-blur);
          @include mixins.useBorderRadius;
          @include themes.useTheme {
            background-color: themes.getVar(module-bg-color);
            color: themes.getVar(link-color);
          }

          & > * {
            margin-left: 16px;
          }
        }

        a:hover {
          @include themes.useTheme {
            color: themes.getVar(link-color-hover);
          }
        }

        a.active {
          @include themes.useTheme {
            color: themes.getVar(link-color-active);
          }
        }
      }
    }
  }
</style>
