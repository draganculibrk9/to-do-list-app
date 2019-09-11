import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ToDoListComponent } from './to-do-list/to-do-list/to-do-list.component';
import { AuthGuard } from './auth/auth.guard';
import { ToDoListShareComponent } from './to-do-list-share/to-do-list-share/to-do-list-share.component';

const appRoutes: Routes = [
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [
      AuthGuard
    ],
    data : {
      scopes: [
        'delete:to-do-list',
        'read:to-do-list',
        'write:to-do-list'
      ]
    }
  },
  {
    path: 'to-do-list/:id',
    component: ToDoListComponent,
    canActivate: [
      AuthGuard
    ],
    data : {
      scopes: [
        'read:to-do-list', 
        'write:to-do-list',
        'delete:to-do-item', 
        'read:to-do-item',
        'write:to-do-item'
      ]
    }
  },
  {
    path: 'to-do-list',
    component: ToDoListComponent,
    canActivate: [
      AuthGuard
    ],
    data : {
      scopes: [
        'read:to-do-list', 
        'write:to-do-list',
        'read:to-do-item',
        'write:to-do-item'
      ]
    }
  },
  {
    path: 'to-do-list-shares/:id',
    component: ToDoListShareComponent
  },
  {
    path: '',
    redirectTo: '/dashboard',
    pathMatch: 'full'
  }
]

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes,
      { enableTracing: true })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
