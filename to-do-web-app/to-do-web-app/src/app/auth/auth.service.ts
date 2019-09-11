import { Injectable } from '@angular/core';
import * as auth0 from 'auth0-js';
import { environment } from './../../environments/environment';
import { Router } from '@angular/router';

@Injectable()
export class AuthService {
  auth0 = new auth0.WebAuth({
    clientID: environment.auth.clientID,
    domain: environment.auth.domain,
    responseType: 'token id_token',
    redirectUri: environment.auth.redirect,
    audience: environment.auth.audience,
    scope: environment.auth.scope
  });

  constructor(private router: Router) {
    this.getAccessToken();
  }

  login() {
    this.auth0.authorize();
  }

  handleLoginCallback() {
    this.auth0.parseHash((err, authResult) => {
      if (authResult && authResult.accessToken) {
        window.location.hash = '';
        this._setSession(authResult, {});
        this.getUserInfo(authResult);
        this.router.navigate(['/dashboard']);
      } else if (err) {
        console.error(`Error: ${err.error}`);
      }
    });
  }

  getAccessToken() {
    this.auth0.checkSession({}, (_err, authResult) => {
      if (authResult && authResult.accessToken) {
        this.getUserInfo(authResult);
      }
    });
  }

  getUserInfo(authResult) {
    this.auth0.client.userInfo(authResult.accessToken, (_err, profile) => {
      if (profile) {
        this._setSession(authResult, profile);
      }
    });
  }

  private _setSession(authResult, profile) {
    localStorage.setItem('expiresAt', JSON.stringify(authResult.expiresIn * 1000 + Date.now()));
    localStorage.setItem('accessToken', authResult.accessToken);
    localStorage.setItem('userProfile', JSON.stringify(profile));
   // localStorage.setItem('scopes', JSON.stringify(authResult.scope.split(' ')));
  }

  logout() {
    localStorage.removeItem('expiresAt');
    localStorage.removeItem('accessToken');
    localStorage.removeItem('userProfile');
    localStorage.removeItem('scopes');

    this.auth0.logout({
      returnTo: 'http://localhost:4200',
      clientID: environment.auth.clientID
    });
  }

  get isLoggedIn(): boolean {
    return Date.now() < JSON.parse(localStorage.getItem('expiresAt') || "{}");
  }

  get getUsername(): string {
    const user = JSON.parse(localStorage.getItem('userProfile'));

    if (user['given_name'] != undefined)
      return user['given_name'];
    else
      return user['name'];
  }

  get returnAccessToken(): string {
    return localStorage.getItem('accessToken');
  }

  checkScopes(neededScopes: string[]): boolean {
    let userScopes = this.returnScopes;

    for (let scope of neededScopes) {
      if (!userScopes.includes(scope))
        return false;
    }
    return true;
  }

  get returnScopes(): string[] {
    return JSON.parse(localStorage.getItem('scopes'));
  }
}
