/**
 * 日志
 */

const createLogger = (scope: string) => ({
  log: (...messages: any) => console.log("⚪", `[${scope}]`, ...messages),
  info: (...messages: any) => console.info("🔵", `[${scope}]`, ...messages),
  warn: (...messages: any) => console.warn("🟠", `[${scope}]`, ...messages),
  error: (...messages: any) => console.error("🔴", `[${scope}]`, ...messages),
  debug: (...messages: any) => console.debug("🟤", `[${scope}]`, ...messages),
  success: (...messages: any) => console.log("🟢", `[${scope}]`, ...messages),
  failure: (...messages: any) => console.warn("🔴", `[${scope}]`, ...messages),
});
export { createLogger };
export default createLogger("WithoutMe App");
