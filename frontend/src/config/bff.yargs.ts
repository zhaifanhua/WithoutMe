import yargs from 'yargs';
const argv = yargs(process.argv.slice(2)).argv;

export const INSTAGRAM_TOKEN = argv.instagram_token;
export const YOUTUBE_API_KEY = argv.youtube_token;
export const TWITTER_COOKIE = argv.twitter_cookie;
export const WEB_SCRAPER_TOKEN = argv.web_scraper_token;
