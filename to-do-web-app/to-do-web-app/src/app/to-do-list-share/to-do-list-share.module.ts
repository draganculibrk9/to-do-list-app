import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToDoListShareComponent } from './to-do-list-share/to-do-list-share.component';
import { FormsModule } from '@angular/forms';
import { ToDoListModule } from '../to-do-list/to-do-list.module';


@NgModule({
  declarations: [ToDoListShareComponent],
  imports: [
    CommonModule,
    FormsModule,
    ToDoListModule
  ]
})
export class ToDoListShareModule { }
