declare interface DeveloperInfo {
  name: string;
  email: string;
  url: string;
}

declare interface AppInfo {
  name: string;
  version: string;
  revisionDate: string;
  author: DeveloperInfo;
  license: string;
  homepage: string;
  repository: Recordable<string>;
  dependencies: Recordable<string>;
  devDependencies: Recordable<string>;
}

declare const __APP_INFO__: AppInfo;
