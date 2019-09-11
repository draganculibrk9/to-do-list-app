import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { ToDoItem } from 'src/app/models/to-do-item.model';
import { ToDoApiService } from 'src/app/core/to-do-api.service';
import { ToastrService } from 'ngx-toastr';
import { ToDoListService } from '../to-do-list.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'to-do-item',
  templateUrl: './to-do-item.component.html',
  styleUrls: ['./to-do-item.component.css']
})
export class ToDoItemComponent implements OnInit, OnDestroy {
  @Input() toDoItem: ToDoItem = new ToDoItem();
  @Input() toDoListId: string;
  apiService: ToDoApiService;
  toastrService: ToastrService;
  subscription: Subscription;
  @Output() refreshItems: EventEmitter<any> = new EventEmitter();

  constructor(apiService: ToDoApiService, toastrService: ToastrService, private toDoListService: ToDoListService) {
    this.apiService = apiService;
    this.toastrService = toastrService;

    this.subscription = this.toDoListService.getToDoList.subscribe(result => {
      if (result) {
        this.toDoListId = result.id;
        this.createToDoItem();
      }
    });
  }

  editOrCreateToDoItem() {
    if (this.toDoItem.id != null) {
      this.editToDoItem();
    } else if (this.toDoListId != null) {
      this.createToDoItem();
    } else {
      this.toDoListService.createToDoList();
    }
  }

  editToDoItem() {
    this.apiService.editToDoItem(this.toDoItem.toDoListId, this.toDoItem).subscribe(
      {
        error: () => {
          this.toastrService.error("Failed to edit item");
          this.refreshItems.emit();
        }
      }
    )
  }

  createToDoItem() {
    this.apiService.createToDoItem(this.toDoListId, this.toDoItem).subscribe({
      next: () => {
        this.toDoItem = new ToDoItem();
        this.refreshItems.emit();
      },
      error: () => {
        this.toastrService.error("Failed to create item");
        this.refreshItems.emit();
      }
    });
  }

  removeToDoItem() {
    this.apiService.removeToDoItem(this.toDoItem.toDoListId, this.toDoItem.id).subscribe({
      next: () => {
        this.refreshItems.emit();
      }
    })
  }

  ngOnInit() {
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

}
