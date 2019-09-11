import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToDoListComponent } from './to-do-list/to-do-list.component';
import { ToDoItemComponent } from './to-do-item/to-do-item.component';
import { FormsModule } from '@angular/forms';
import { DndModule } from 'ng2-dnd';

@NgModule({
  declarations: [ToDoListComponent, ToDoItemComponent],
  imports: [
    CommonModule,
    FormsModule,
    DndModule
  ],
  exports: [
    ToDoListComponent,
    ToDoItemComponent
  ]
})
export class ToDoListModule { }
