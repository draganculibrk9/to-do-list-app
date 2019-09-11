import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToDoApiService } from './to-do-api.service';
import { AuthService } from '../auth/auth.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from '../auth/token-interceptor';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  exports: [
  ],
  providers: [
    ToDoApiService,
    AuthService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ]
})

export class CoreModule { }
