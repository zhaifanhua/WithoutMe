<!-- 站点信息容器组件 -->

<template>
  <div id="site-info-container" class="site-info-container">
    <div v-if="siteOptimization">
      <p>
        <a :href="siteOptimization.map">网站地图</a>
        <span>&nbsp;</span>
        <a :href="siteOptimization.statistics">站长统计</a>
      </p>
      <p>
        <span>致谢</span>
        <a :href="siteOptimization.cdnUrl">
          <img class="upyun-logo" src="/images/icon/upyun.png" :alt="siteOptimization.cdnName" />
        </a>
        <span>提供云加速服务</span>
      </p>
    </div>

    <p v-if="siteAuthor">
      <span>Copyright &copy;</span>
      <time> {{ siteAuthor.startYear }}-{{ siteAuthor.endYear }} </time>
      <span>&nbsp;</span>
      <a :href="siteAuthor.author.url" target="_blank">
        {{ siteAuthor.author.name }}
      </a>
      <span>&nbsp;</span>
      <span>All Rights Reserved.</span>
    </p>

    <p v-if="siteIcp">
      <a v-if="siteIcp.icp" :href="siteIcp.icpUrl" target="_blank">{{ siteIcp.icp }}</a>
      <span>&nbsp;</span>
      <a v-if="siteIcp.police" :href="siteIcp.policeUrl" target="_blank">{{ siteIcp.police }}</a>
    </p>
  </div>
</template>

<script setup lang="ts">
  import { ref } from 'vue';
  import { siteAuthorInfo, siteOptimizationInfo, siteIcpInfo } from '@/app/siteInfo';

  const siteOptimization = ref(siteOptimizationInfo);
  const siteAuthor = ref(siteAuthorInfo);
  const siteIcp = ref(siteIcpInfo);
</script>

<style scoped lang="scss">
  @import '@/styles/base/mixins.scss';
  @import '@/styles/base/themes.scss';

  .site-info-container {
    grid-area: site-info-container;
    @include useFlexBox(column, center, center, center);
    @include useTheme {
      color: getVar(text-color-secondary-translucent);
    }

    p {
      font-size: 12px;

      a {
        color: inherit;
        text-decoration: none;

        &:hover {
          color: inherit;
          text-decoration: underline;
        }

        img {
          display: inline-block;
          vertical-align: middle;

          &.upyun-logo {
            height: 12px;
          }
        }
      }
    }
  }
</style>
