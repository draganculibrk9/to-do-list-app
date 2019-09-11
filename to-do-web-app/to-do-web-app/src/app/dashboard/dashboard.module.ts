import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import { ToDoPreviewComponent } from './to-do-preview/to-do-preview.component';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { DndModule } from 'ng2-dnd';
import { ClipboardModule } from 'ngx-clipboard';

@NgModule({
  declarations: [DashboardComponent, ToDoPreviewComponent],
  imports: [
    CommonModule,
    RouterModule,
    FontAwesomeModule,
    DndModule,
    ClipboardModule
  ],
  exports: [
    DashboardComponent
  ]
})
export class DashboardModule { }
