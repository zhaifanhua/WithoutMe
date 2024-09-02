/**
 * 站点信息
 */

const env = import.meta.env;

// 环境信息
const isDev = env.DEV;
const isProd = env.PROD;
const isServer = env.SSR;
const isClient = !isServer;

// 站点基础信息
const siteBaseInfo = {
  title: {
    cn: env.VITE_SITE_TITLE_CN,
    en: env.VITE_SITE_TITLE_EN,
  },
  subtitle: {
    cn: env.VITE_SITE_SUBTITLE_CN,
    en: env.VITE_SITE_SUBTITLE_EN,
  },
  keywords: env.VITE_SITE_KEYWORDS,
  url: env.VITE_SITE_URL,
  icon: env.VITE_SITE_ICON,
  logo: env.VITE_SITE_LOGO,
  bless: env.VITE_SITE_BLESS,
};
// 站点作者信息
const siteAuthorInfo = {
  startYear: env.VITE_SITE_START_YEAR,
  endYear: new Date().getFullYear(),
  author: {
    name: env.VITE_SITE_AUTHOR_NAME,
    email: env.VITE_SITE_AUTHOR_EMAIL,
    url: env.VITE_SITE_AUTHOR_URL,
  },
};
// 站点优化信息
const siteOptimizationInfo = {
  map: env.VITE_SITE_MAP,
  statistics: env.VITE_SITE_STATISTICS,
  cdnName: env.VITE_SITE_CDN_NAME,
  cdnUrl: env.VITE_SITE_CDN_URL,
};
// 站点备案信息
const siteIcpInfo = {
  icp: env.VITE_SITE_ICP,
  icpUrl: env.VITE_SITE_ICP_URL,
  police: env.VITE_SITE_POLICE,
  policeUrl: env.VITE_SITE_POLICE_URL,
};

export { isDev, isProd, isServer, isClient, siteBaseInfo, siteAuthorInfo, siteOptimizationInfo, siteIcpInfo };
