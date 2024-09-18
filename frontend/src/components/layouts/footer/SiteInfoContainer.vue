<!-- 站点信息容器组件 -->

<template>
  <div id="site-info-container" class="site-info-container">
    <div v-if="siteOptimization">
      <p>
        <span v-if="siteOptimization.map">
          <a :href="siteOptimization.map">网站地图</a>
        </span>
      </p>
      <p>
        <span>致谢</span>
        <span>&nbsp;</span>
        <span v-if="siteOptimization.cdnName">{{ siteOptimization.cdnName }}</span>
        <span v-if="siteOptimization.cdnUrl">
          <a :href="siteOptimization.cdnUrl">
            <img class="cdn-logo" :src="siteOptimization.cdnLogo" />
          </a>
        </span>
        <span>&nbsp;</span>
        <span>提供云加速服务</span>
      </p>
    </div>
    <div v-if="siteAuthor">
      <p>
        <span>Copyright</span>
        <span>&nbsp;</span>
        <span>&copy;</span>
        <span v-if="siteAuthor.startYear">
          <time>{{ siteAuthor.startYear }}</time>
          <span>-</span>
        </span>
        <span v-if="siteAuthor.endYear">
          <time>{{ siteAuthor.endYear }} </time>
        </span>
        <span>&nbsp;</span>
        <span v-if="siteAuthor.author">
          <a :href="siteAuthor.author.url" target="_blank">
            {{ siteAuthor.author.name }}
          </a>
        </span>
        <span>&nbsp;</span>
        <span>All Rights Reserved.</span>
      </p>
    </div>
    <div v-if="siteIcp">
      <p>
        <span v-if="siteIcp.icp">
          <a :href="siteIcp.icpUrl" target="_blank">{{ siteIcp.icp }}</a>
        </span>
        <span>&nbsp;</span>
        <span v-if="siteIcp.police">
          <a :href="siteIcp.policeUrl" target="_blank">{{ siteIcp.police }}</a>
        </span>
      </p>
    </div>
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

          &.cdn-logo {
            height: 12px;
          }
        }
      }
    }
  }
</style>
