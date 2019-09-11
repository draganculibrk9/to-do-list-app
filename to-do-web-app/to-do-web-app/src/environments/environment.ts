// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: true,
  baseURL : 'http://localhost:51430/api',
  auth: {
    clientID: 'OXcVQUntD3NdalSuYA0NK2TnhbD6kGZN',
    domain: 'dev-bp9t5r-u.eu.auth0.com',
    audience: 'http://localhost:51430/api',
    redirect: 'http://localhost:4200/dashboard',
    scope: 'openid profile email delete:to-do-list read:to-do-item read:to-do-list write:to-do-item write:to-do-list write:to-do-list-share'
  }
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
